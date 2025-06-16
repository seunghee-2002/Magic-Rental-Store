using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DayView : DayPhaseView
{
    public DayPresenter presenter;

    [Header("Panels")]
    [SerializeField] private GameObject dayPanel;              // 낮 시간대 메인 패널
    [SerializeField] private GameObject heroMenuPanel;         // 영웅 메뉴 패널 (명단/생성 버튼)
    [SerializeField] private GameObject heroRosterPanel;       // 영웅 명단 패널
    [SerializeField] private GameObject heroCreationPanel;     // 영웅 생성 패널
    [SerializeField] private GameObject customerListPanel;     // 고객 명단 패널
    [SerializeField] private GameObject customerInfoPanel;     // 고객 정보 패널
    [SerializeField] private GameObject weaponSelectionPanel;  // 무기 선택 패널
    [SerializeField] private GameObject dungeonInfoPanel;      // 던전 정보 패널
    [SerializeField] private GameObject customerPanel;
    [SerializeField] private GameObject weaponPanel;

    [Header("Customer List")]
    [SerializeField] private Transform customerButtonParent;   // 고객 버튼들이 생성될 부모 오브젝트
    [SerializeField] private GameObject customerButtonPrefab;  // 고객 버튼 프리팹

    [Header("Customer Info")]
    [SerializeField] private TextMeshProUGUI customerNameText;    // 고객 이름 텍스트
    [SerializeField] private TextMeshProUGUI customerDescText;    // 고객 설명 텍스트
    [SerializeField] private TextMeshProUGUI customerLevelText;   // 고객 레벨 텍스트
    [SerializeField] private TextMeshProUGUI customerRequestText; // 고객 요청사항 텍스트
    [SerializeField] private TextMeshProUGUI customerElementText; // 고객 속성 텍스트
    [SerializeField] private TextMeshProUGUI customerGradeText;   // 고객 등급 텍스트
    [SerializeField] private Image customerIcon;                  // 고객 아이콘 이미지
    [SerializeField] private Button weaponSelectButton;           // 무기 선택 버튼
    [SerializeField] private Button dungeonInfoButton;            // 던전 정보 버튼

    [Header("Hero Roster")]
    [SerializeField] private Transform heroRosterContent;      // 영웅 명단 버튼들이 생성될 부모 오브젝트
    [SerializeField] private GameObject heroButtonPrefab;      // 영웅 버튼 프리팹

    [Header("Buttons")]
    [SerializeField] private Button heroMenuButton;            // 영웅 메뉴 열기 버튼
    [SerializeField] private Button heroRosterButton;          // 영웅 명단 열기 버튼
    [SerializeField] private Button heroCreationButton;        // 영웅 생성 열기 버튼
    [SerializeField] private Button heroRosterCancelButton;    // 영웅 명단 닫기 버튼
    [SerializeField] private Button heroCreationCancelButton;  // 영웅 생성 닫기 버튼
    [SerializeField] private Button customerInfoCancelButton;  // 고객 정보 닫기 버튼
    [SerializeField] private Button weaponSelectionCancelButton; // 무기 선택 닫기 버튼
    [SerializeField] private Button nextPhaseButton;

    [Header("Dungeon Info")]
    [SerializeField] private TextMeshProUGUI dungeonNameText;     // 던전 이름 텍스트
    [SerializeField] private TextMeshProUGUI dungeonDescText;     // 던전 설명 텍스트
    [SerializeField] private TextMeshProUGUI dungeonGradeText;    // 던전 등급 텍스트
    [SerializeField] private TextMeshProUGUI dungeonElementText;  // 던전 속성 텍스트
    [SerializeField] private Image dungeonIcon;                   // 던전 아이콘 이미지
    [SerializeField] private Button dungeonInfoCancelButton;      // 던전 정보 닫기 버튼

    private DayModel model;

    public void Initialize(DayModel model)
    {
        this.model = model;
        presenter = new DayPresenter(this, model);
        SetupButtonListeners();
    }

    protected override void SetupButtonListeners()
    {
        if (nextPhaseButton != null)
        {
            nextPhaseButton.onClick.AddListener(() => {
                GameManager.Instance.AdvanceToNextPhase();
            });
        }
        heroMenuButton.onClick.AddListener(() => presenter.OnHeroMenuClicked());
        heroRosterButton.onClick.AddListener(() => presenter.OnHeroRosterClicked());
        heroCreationButton.onClick.AddListener(() => presenter.OnHeroCreationClicked());
        heroRosterCancelButton.onClick.AddListener(() => presenter.OnHeroRosterCancelClicked());
        heroCreationCancelButton.onClick.AddListener(() => presenter.OnHeroCreationCancelClicked());
        customerInfoCancelButton.onClick.AddListener(() => presenter.OnCustomerInfoCancelClicked());
        weaponSelectButton.onClick.AddListener(() => presenter.OnWeaponSelectClicked());
        weaponSelectionCancelButton.onClick.AddListener(() => presenter.OnWeaponSelectionCancelClicked());
        dungeonInfoButton.onClick.AddListener(() => presenter.OnDungeonButtonClicked());
        dungeonInfoCancelButton.onClick.AddListener(() => presenter.OnDungeonInfoCancelClicked());
    }

    public void UpdateCustomers(List<CustomerInstance> customers)
    {
        foreach (Transform child in customerButtonParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < customers.Count; i++)
        {
            int idx = i;
            CustomerInstance customer = customers[i];
            GameObject buttonObj = Instantiate(customerButtonPrefab, customerButtonParent);
            CustomerButton button = buttonObj.GetComponent<CustomerButton>();
            button.Initialize(customer, () => presenter.OnCustomerClicked(customer, idx));
        }
    }

    public void UpdateCustomerInfo(string name, string desc, int level, string request, string element, string grade, Sprite icon, bool hasDungeon)
    {
        customerNameText.text = name;
        customerDescText.text = desc;
        customerLevelText.text = $"레벨: {level}";
        customerRequestText.text = $"요청사항: {request}";
        customerElementText.text = $"속성: {element}";
        customerGradeText.text = $"등급: {grade}";
        customerIcon.sprite = icon;
        
        dungeonInfoButton.interactable = hasDungeon;
    }

    public void UpdateWeaponInfo(string name, string description, string stats)
    {
        // 무기 정보는 더 이상 표시하지 않음
        // 대신 대여 확인 팝업을 표시
    }

    public void UpdateHeroRoster(List<HeroInstance> heroes)
    {
        foreach (Transform child in heroRosterContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < heroes.Count; i++)
        {
            int idx = i;
            HeroInstance hero = heroes[i];
            GameObject buttonObj = Instantiate(heroButtonPrefab, heroRosterContent);
            HeroButton button = buttonObj.GetComponent<HeroButton>();
            button.Initialize(hero, () => presenter.OnHeroClicked(hero, idx));
        }
    }

    public void ShowRentalConfirmPopup(CustomerInstance customer, WeaponData weapon)
    {
        string message = $"{customer.customerData.customerName}에게 {weapon.weaponName}을(를) 대여하시겠습니까?";
        UIManager.Instance.ShowConfirmPopup(message, 
            () => {
                presenter.OnRentalConfirmed(customer, weapon);
                presenter.OnRentalCancelled();
            });
    }

    public override void Show()
    {
        dayPanel.SetActive(true);
    }

    public override void Hide()
    {
        dayPanel.SetActive(false);
    }

    public void ShowHeroMenu()
    {
        HideAllPanels();
        heroMenuPanel.SetActive(true);
        Show();  // 부모 클래스의 Show 호출
    }

    public void ShowHeroRoster()
    {
        HideAllPanels();
        heroRosterPanel.SetActive(true);
        Show();  // 부모 클래스의 Show 호출
    }

    public void ShowHeroCreation()
    {
        HideAllPanels();
        heroCreationPanel.SetActive(true);
        Show();  // 부모 클래스의 Show 호출
    }

    public void ShowCustomerList()
    {
        HideAllPanels();
        customerListPanel.SetActive(true);
        Show();  // 부모 클래스의 Show 호출
    }

    public void ShowCustomerInfo()
    {
        HideAllPanels();
        customerInfoPanel.SetActive(true);
        Show();  // 부모 클래스의 Show 호출
    }

    public void ShowWeaponSelection()
    {
        HideAllPanels();
        weaponSelectionPanel.SetActive(true);
        Show();  // 부모 클래스의 Show 호출
    }

    public void ShowDungeonInfo()
    {
        HideAllPanels();
        dungeonInfoPanel.SetActive(true);
        Show();  // 부모 클래스의 Show 호출
    }

    private void HideAllPanels()
    {
        dayPanel.SetActive(false);
        heroMenuPanel.SetActive(false);
        heroRosterPanel.SetActive(false);
        heroCreationPanel.SetActive(false);
        customerListPanel.SetActive(false);
        customerInfoPanel.SetActive(false);
        weaponSelectionPanel.SetActive(false);
        dungeonInfoPanel.SetActive(false);
    }

    public void OnHeroSystemUnlocked()
    {
        heroMenuButton.gameObject.SetActive(true);
    }
} 