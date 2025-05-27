using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DayUI : MonoBehaviour
{
    PanelController panelController;
    [Header("Hero Menu")]
    public GameObject heroMenuPanel;
    public Button heroMenuButton;
    public Button heroRosterPanelButton;
    public Button heroCreationPanelButton;
    public Button heroUnlockButton;
    public TextMeshProUGUI heroUnlockText;
    [Header("Hero Creation")]
    public GameObject heroCreationPanel;
    public TMP_InputField nameInput;
    public TMP_InputField descInput;
    public TMP_Dropdown elementDropdown;
    public Slider costSlider;
    public Image iconPreview;
    public TextMeshProUGUI costValueText;
    public Button heroCreationButton;
    public Button heroCreationCancelButton;
    public Button selectIconButton;
    [Header("Hero Roster")]
    public GameObject heroRosterPanel;
    public GameObject heroRosterButtonPrefab;
    public Transform heroRosterParent;
    public Button heroRosterCloseButton;
    [Header("Customer Information")]
    public CustomerInfoPanel customerInfoPanel;
    public Button customerInfoCloseButton;
    public GameObject customerButtonPrefab;
    public GameObject weaponSelectionPanel;
    public Transform customerListParent;
    public Button weaponSelectionCloseButton;
    public CustomerInstance selectedCustomer;

    void Awake() { panelController = UIManager.Instance.panelController; }

    public void InitUI()
    {
        heroMenuButton.interactable = false;
        elementDropdown.ClearOptions();
        elementDropdown.AddOptions(System.Enum.GetNames(typeof(Element)).ToList());

        heroMenuButton.onClick.RemoveAllListeners();
        heroMenuButton.onClick.AddListener(() =>
        {
            heroMenuPanel.SetActive(!heroMenuPanel.activeSelf);
        });
        heroCreationPanelButton.onClick.RemoveAllListeners();
        heroCreationPanelButton.onClick.AddListener(ShowHeroCreationPanel);
        heroRosterPanelButton.onClick.RemoveAllListeners();
        heroRosterPanelButton.onClick.AddListener(ShowHeroRosterPanel);
        heroCreationButton.onClick.RemoveAllListeners();
        heroCreationButton.onClick.AddListener(OnHeroCreationClicked);
        heroCreationCancelButton.onClick.RemoveAllListeners();
        heroCreationCancelButton.onClick.AddListener(() =>
        {
            heroCreationPanel.SetActive(false);
        });
        heroRosterCloseButton.onClick.RemoveAllListeners();
        heroRosterCloseButton.onClick.AddListener(() =>
        {
            heroRosterPanel.SetActive(false);
        });
        selectIconButton.onClick.RemoveAllListeners();
        selectIconButton.onClick.AddListener(OnSelectIconClicked);
        heroUnlockButton.onClick.RemoveAllListeners();
        heroUnlockButton.onClick.AddListener(() =>
        {
            GameManager.Instance.PurchaseHeroSystemUnlock();
        });
        costSlider.wholeNumbers = true;
        costSlider.onValueChanged.RemoveAllListeners();
        costSlider.onValueChanged.AddListener(value =>
        {
            costValueText.text = $"투입 Gold: {value}G";
        });
        weaponSelectionCloseButton.onClick.RemoveAllListeners();
        weaponSelectionCloseButton.onClick.AddListener(() => HideCustomerInfoPanel());
        heroUnlockText.text = $"용사 시스템 해금 비용: {GameManager.Instance.heroUnlockGold}G";

        SetupCostSlider();
    }

    public void ShowDayUI(List<CustomerInstance> customers)
    {
        panelController.ShowDayUI();
        foreach (Transform t in customerListParent) Destroy(t.gameObject);
        foreach (CustomerInstance customer in customers)
        {
            GameObject btn = Instantiate(customerButtonPrefab, customerListParent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text =
                $"{customer.customerData.name} (Lv{customer.customerData.level}/{customer.customerData.grade})";
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                ShowCustomerInfoPanel(customer);
            });
        }
    }

    public void OnHeroSystemUnlocked()
    {
        heroMenuButton.interactable = true;
        CommonUI.Instance.DisplayResult("용사 제작 시스템이 해금되었습니다!");
    }

    void ShowHeroCreationPanel()
    {
        panelController.ShowHeroCreationPanel();
        nameInput.text = "";
        descInput.text = "";
        elementDropdown.value = 0;
        iconPreview.sprite = null;
        SetupCostSlider();
        heroCreationPanel.SetActive(true);
    }

    void ShowHeroRosterPanel()
    {
        panelController.ShowHeroRosterPanel();
        UpdateHeroUI();
    }

    void OnHeroCreationClicked()
    {
        string name = nameInput.text.Trim();
        string desc = descInput.text.Trim();
        Element elem = (Element)elementDropdown.value;
        Sprite icon = iconPreview.sprite;
        int cost = (int)costSlider.value;

        if (string.IsNullOrEmpty(name))
        {
            CommonUI.Instance.DisplayResult("용사 이름을 입력하세요!");
            return;
        }

        HeroManager.Instance.CreateHero(name, desc, elem, icon, cost);
        heroCreationPanel.SetActive(false);
    }

    void OnSelectIconClicked()
    {
        // 아이콘 선택 팝업, 프리셋 등 연결
    }

    void SetupCostSlider()
    {
        int currentGold = GameManager.Instance.gold;
        costSlider.minValue = 0;
        costSlider.maxValue = currentGold;
        costSlider.value = currentGold / 2;
        costValueText.text = $"투입 Gold: {costSlider.value}G";
    }

    public void UpdateHeroUI()
    {
        foreach (Transform t in heroRosterParent) Destroy(t.gameObject);
        foreach (HeroData hero in HeroManager.Instance.heroRoster)
        {
            GameObject btn = Instantiate(heroRosterButtonPrefab, heroRosterParent);
            TextMeshProUGUI txt = btn.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = $"{hero.heroName}\n({hero.element}, {hero.grade})";
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                // 던전 파견 등 처리
            });
        }
    }

    void ShowCustomerInfoPanel(CustomerInstance customer)
    {
        customerInfoPanel.SetInfo(customer);
        customerInfoPanel.gameObject.SetActive(true);
    }

    void HideCustomerInfoPanel()
    {
        customerInfoPanel.gameObject.SetActive(false);
    }
}
