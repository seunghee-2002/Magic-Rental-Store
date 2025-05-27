using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DayUI : MonoBehaviour
{
    PanelController panelController;
    [Header("Day UI")]
    public GameObject dayPanel;
    [Header("Hero Menu")]
<<<<<<< HEAD
<<<<<<< HEAD
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
=======
    GameObject heroMenuPanel;
    Button heroMenuButton;
    Button heroRosterPanelButton;
    Button heroCreationPanelButton;
    public Button heroUnlockButton;
    TextMeshProUGUI heroUnlockText;
    [Header("Hero Creation")]
    public GameObject heroCreationPanel;
    TMP_InputField nameInput;
    TMP_InputField descInput;
    TMP_Dropdown elementDropdown;
    Slider costSlider;
    Image iconPreview;
    public Customer selectedCustomer;
    TextMeshProUGUI costValueText;
    Button heroCreationButton;
    Button heroCreationCancelButton;
    Button selectIconButton;
    [Header("Hero Roster")]
=======
    GameObject heroMenuPanel;
    Button heroMenuButton;
    Button heroRosterPanelButton;
    Button heroCreationPanelButton;
    public Button heroUnlockButton;
    TextMeshProUGUI heroUnlockText;
    [Header("Hero Creation")]
    public GameObject heroCreationPanel;
    TMP_InputField nameInput;
    TMP_InputField descInput;
    TMP_Dropdown elementDropdown;
    Slider costSlider;
    Image iconPreview;
    public Customer selectedCustomer;
    TextMeshProUGUI costValueText;
    Button heroCreationButton;
    Button heroCreationCancelButton;
    Button selectIconButton;
    [Header("Hero Roster")]
>>>>>>> parent of 0773a29 (mvp2)
    GameObject heroRosterPanel;
    GameObject heroRosterButtonPrefab;
    Transform heroRosterParent;
    Button heroRosterCancelButton;
    [Header("Customer")]
    GameObject customerButtonPrefab;
    GameObject WeaponSelectionButtonPrefab;
    GameObject WeaponSelectionPanel;
    Transform customerListParent;
    Transform WeaponSelectionParent;
    Button WeaponSelectionCancelButton;

    void Awake()
    {
        panelController = GetComponent<PanelController>();
<<<<<<< HEAD
=======
    }

    public void InitPanel()
    {
        dayPanel = Instantiate(UIBinder.Instance.dayPanelPrefab, UIBinder.Instance.panelParent);
        dayPanel.SetActive(false);
    }

    public void BindUI()
    {
        customerButtonPrefab = UIBinder.Instance.customerButtonPrefab;
        WeaponSelectionButtonPrefab = UIBinder.Instance.WeaponSelectionButtonPrefab;
        WeaponSelectionPanel = UIBinder.Instance.WeaponSelectionPanel;
        heroMenuPanel = UIBinder.Instance.heroMenuPanel;
        heroCreationPanel = UIBinder.Instance.heroCreationPanel;
        heroRosterPanel = UIBinder.Instance.heroRosterPanel;
        heroRosterButtonPrefab = UIBinder.Instance.heroRosterButtonPrefab;
        customerListParent = UIBinder.Instance.customerListParent;
        WeaponSelectionParent = UIBinder.Instance.WeaponSelectionParent;
        heroRosterParent = UIBinder.Instance.heroRosterParent;
        heroMenuButton = UIBinder.Instance.heroMenuButton;
        nameInput = UIBinder.Instance.nameInput;
        descInput = UIBinder.Instance.descInput;
        elementDropdown = UIBinder.Instance.elementDropdown;
        costSlider = UIBinder.Instance.costSlider;
        iconPreview = UIBinder.Instance.iconPreview;
        costValueText = UIBinder.Instance.costValueText;
        heroCreationButton = UIBinder.Instance.heroCreationButton;
        heroCreationCancelButton = UIBinder.Instance.heroCreationCancelButton;
        heroRosterCancelButton = UIBinder.Instance.heroRosterCancelButton;
        selectIconButton = UIBinder.Instance.selectIconButton;
        WeaponSelectionCancelButton = UIBinder.Instance.WeaponSelectionCancelButton;
        heroUnlockText = UIBinder.Instance.heroUnlockText;
        heroRosterPanelButton = UIBinder.Instance.heroRosterPanelButton;
        heroCreationPanelButton = UIBinder.Instance.heroCreationPanelButton;
        heroUnlockButton = UIBinder.Instance.heroUnlockButton;
>>>>>>> parent of 0773a29 (mvp2)
    }

    public void InitPanel()
    {
        dayPanel = Instantiate(UIBinder.Instance.dayPanelPrefab, UIBinder.Instance.panelParent);
        dayPanel.SetActive(false);
    }

    public void BindUI()
    {
        customerButtonPrefab = UIBinder.Instance.customerButtonPrefab;
        WeaponSelectionButtonPrefab = UIBinder.Instance.WeaponSelectionButtonPrefab;
        WeaponSelectionPanel = UIBinder.Instance.WeaponSelectionPanel;
        heroMenuPanel = UIBinder.Instance.heroMenuPanel;
        heroCreationPanel = UIBinder.Instance.heroCreationPanel;
        heroRosterPanel = UIBinder.Instance.heroRosterPanel;
        heroRosterButtonPrefab = UIBinder.Instance.heroRosterButtonPrefab;
        customerListParent = UIBinder.Instance.customerListParent;
        WeaponSelectionParent = UIBinder.Instance.WeaponSelectionParent;
        heroRosterParent = UIBinder.Instance.heroRosterParent;
        heroMenuButton = UIBinder.Instance.heroMenuButton;
        nameInput = UIBinder.Instance.nameInput;
        descInput = UIBinder.Instance.descInput;
        elementDropdown = UIBinder.Instance.elementDropdown;
        costSlider = UIBinder.Instance.costSlider;
        iconPreview = UIBinder.Instance.iconPreview;
        costValueText = UIBinder.Instance.costValueText;
        heroCreationButton = UIBinder.Instance.heroCreationButton;
        heroCreationCancelButton = UIBinder.Instance.heroCreationCancelButton;
        heroRosterCancelButton = UIBinder.Instance.heroRosterCancelButton;
        selectIconButton = UIBinder.Instance.selectIconButton;
        WeaponSelectionCancelButton = UIBinder.Instance.WeaponSelectionCancelButton;
        heroUnlockText = UIBinder.Instance.heroUnlockText;
        heroRosterPanelButton = UIBinder.Instance.heroRosterPanelButton;
        heroCreationPanelButton = UIBinder.Instance.heroCreationPanelButton;
        heroUnlockButton = UIBinder.Instance.heroUnlockButton;
    }
>>>>>>> parent of 0773a29 (mvp2)

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

<<<<<<< HEAD
=======
    public void OpenWeaponSelectionPanel(Customer customer) // 아이템 선택 패널 열기
    {
        selectedCustomer = customer;
        
        foreach (Transform t in WeaponSelectionParent) Destroy(t.gameObject); // 이전에 있던 오브젝트 제거
        foreach (WeaponInstance Weapon in InventoryManager.Instance.GetWeaponInventory()) 
        {
            GameObject btn = Instantiate(WeaponSelectionButtonPrefab, WeaponSelectionParent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{Weapon.data.weaponName} x{Weapon.quantity}";
            btn.GetComponent<Button>().onClick.AddListener(() => {
                if (GameManager.Instance.TryAssignWeapon(selectedCustomer, Weapon.data))
                {
                    CommonUI.Instance.DisplayResult($"{selectedCustomer.name}에게 {Weapon.data.weaponName}을(를) 빌려주었습니다.");
                }
                else
                {
                    CommonUI.Instance.DisplayResult("재고가 없습니다!");
                }
                CloseWeaponSelectionPanel();                
            });
        }
        panelController.ShowWeaponSelectionPanel();
    }

    void CloseWeaponSelectionPanel() // 아이템 선택 팝업 닫기
    {
        panelController.CloseWeaponSelectionPanel();
        selectedCustomer = null;
    }

>>>>>>> parent of 0773a29 (mvp2)
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
