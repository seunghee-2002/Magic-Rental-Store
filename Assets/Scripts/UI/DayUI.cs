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
        heroRosterCancelButton.onClick.RemoveAllListeners();
        heroRosterCancelButton.onClick.AddListener(() =>
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
        costSlider.wholeNumbers = true; // 정수 단위로만
        costSlider.onValueChanged.RemoveAllListeners();
        costSlider.onValueChanged.AddListener(value =>
        {
            costValueText.text = $"투입 Gold: {value}G";
        });
        WeaponSelectionCancelButton.onClick.RemoveAllListeners();
        WeaponSelectionCancelButton.onClick.AddListener(() => CloseWeaponSelectionPanel());
        heroUnlockText.text = $"용사 시스템 해금 비용: {GameManager.Instance.heroUnlockGold}G";

        SetupCostSlider();
    }


    public void ShowDayUI(List<Customer> customers) // 낮 패널 표시
    {
        panelController.ShowDayUI();

        foreach (Transform t in customerListParent) Destroy(t.gameObject); // 이전에 있던 오브젝트 제거
        foreach (Customer c in customers)
        {
            GameObject btn = Instantiate(customerButtonPrefab, customerListParent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text =
                $"{c.name} (Lv{c.level}/{c.grade})";
            btn.GetComponent<Button>().onClick.AddListener(() => {
                // 아이템 선택 팝업 열고 선택된 아이템 대여
                OpenWeaponSelectionPanel(c);
            });
        }
    }

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

    public void OnHeroSystemUnlocked()
    {
        heroMenuButton.interactable = true;
        CommonUI.Instance.DisplayResult("용사 제작 시스템이 해금되었습니다!");
    }

    void ShowHeroCreationPanel()
    {
        // 초기화
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
        UpdateHeroUI();  // 최신 명단으로 갱신
    }

    
    void OnHeroCreationClicked()
    {
        string name = nameInput.text.Trim();
        string desc = descInput.text.Trim();
        Element elem = (Element)elementDropdown.value;
        Sprite icon = iconPreview.sprite; // (아이콘 선택 기능 필요 시 처리)
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
        // TODO: 파일선택, 아이콘 목록 팝업 등에서 sprite를 골라 iconPreview.sprite 에 할당
        // 간단히 프리셋 아이콘에서 하나 고른다고 가정하면:
        // iconPreview.sprite = somePresetIcon;
    }
    
    void SetupCostSlider()
    {
        int currentGold = GameManager.Instance.gold;
        costSlider.minValue = 0;
        costSlider.maxValue = currentGold;
        costSlider.value = currentGold / 2;        // 기본값은 최대치의 반
        costValueText.text = $"투입 Gold: {costSlider.value}G";
    }

    public void UpdateHeroUI()
    {
        // 이전에 있던 버튼들 삭제
        foreach (Transform t in heroRosterParent) Destroy(t.gameObject);

        // 새로 고용된 용사마다 버튼 생성
        foreach (Hero hero in HeroManager.Instance.heroRoster)
        {
            GameObject btn = Instantiate(heroRosterButtonPrefab, heroRosterParent);
            TextMeshProUGUI txt = btn.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = $"{hero.heroName}\n({hero.element}, {hero.grade})";
            btn.GetComponent<Button>().onClick.AddListener(() => {
                // 던전 파견 등 처리
            });
        }
    }
}
