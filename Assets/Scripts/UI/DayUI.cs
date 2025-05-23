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
    [SerializeField] Button heroMenuButton;
    [SerializeField] Button heroRosterPanelButton;
    [SerializeField] Button heroCreationPanelButton;
    public Button heroUnlockButton;
    [SerializeField] TextMeshProUGUI heroUnlockText;
    [Header("Hero Creation")]
    public GameObject heroCreationPanel;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_InputField descInput;
    [SerializeField] TMP_Dropdown elementDropdown;
    [SerializeField] Slider costSlider;
    [SerializeField] Image iconPreview;
    public Customer selectedCustomer;
    [SerializeField] TextMeshProUGUI costValueText;
    [SerializeField] Button heroCreationButton;
    [SerializeField] Button heroCreationCancelButton;
    [SerializeField] Button selectIconButton;
    [Header("Hero Roster")]
    public GameObject heroRosterPanel;
    [SerializeField] GameObject heroRosterButtonPrefab;
    [SerializeField] Transform heroRosterParent;
    [SerializeField] Button heroRosterCancelButton;
    [Header("Customer")]
    [SerializeField] GameObject customerButtonPrefab;
    [SerializeField] GameObject WeaponSelectionButtonPrefab;
    public GameObject WeaponSelectionPanel;
    [SerializeField] Transform customerListParent;
    [SerializeField] Transform WeaponSelectionParent;
    [SerializeField] Button WeaponSelectionCancelButton;

    void Awake()
    {
        panelController = UIManager.Instance.panelController;
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

        foreach (WeaponInstance weaponInstance in InventoryManager.Instance.GetWeaponInventory()) 
        {
            WeaponInstance capturedWeapon = weaponInstance;    // 무기 캡처
            Customer capturedCustomer = customer;              // 고객도 함께 캡처

            GameObject btn = Instantiate(WeaponSelectionButtonPrefab, WeaponSelectionParent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{capturedWeapon.data.weaponName} x{capturedWeapon.quantity}";

            btn.GetComponent<Button>().onClick.AddListener(() => {
                if (GameManager.Instance.TryAssignWeapon(capturedWeapon.data))
                {
                    CommonUI.Instance.ShowConfirmation(
                        $"{capturedCustomer.name}에게 {capturedWeapon.data.weaponName}을(를) 빌려주시겠습니까?",
                        () => GameManager.Instance.AssignWeapon(capturedCustomer, capturedWeapon.data)
                    );
                }
                else
                {
                    CommonUI.Instance.ShowMessage("아이템이 존재하지 않습니다.");
                }
                CloseWeaponSelectionPanel();                
            });
        }

        panelController.ShowWeaponSelectionPanel();
    }

    void CloseWeaponSelectionPanel() // 아이템 선택 팝업 닫기
    {
        WeaponSelectionPanel.SetActive(false);
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
