using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.TextCore.Text;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panel")]
    public Transform panelParent; // 패널 부모
    public GameObject morningPanelPrefab; // 아침 패널 프리팹
    public GameObject dayPanelPrefab;   // 낮 패널 프리팹
    public GameObject nightPanelPrefab; // 밤 패널 프리팹
    // 런타임 인스턴스 패널
    GameObject morningPanel;
    GameObject dayPanel;
    GameObject nightPanel;
    [Header("Morning Panel")]
    GameObject blacksmithMenuPanel, // 대장장이 메뉴 패널
    blacksmithForgePanel, // 대장장이 제작 패널
    blacksmithRecipePanel, // 대장장이 레시피 패널
    forgeButtonPrefab; // 대장장이 제작 버튼 프리팹
    Transform forgeButtonParent; // 대장장이 제작 버튼 부모
    Button blacksmithForgeCancelButton, // 대장장이 제작 취소 버튼
    blacksmithResipeCancelButton, // 대장장이 레시피 나가기 버튼
    blacksmithMenuButton, // 대장장이 메뉴 버튼
    blacksmithForgeButton, // 대장장이 제작 버튼
    blacksmithRecipeButton; // 대장장이 레시피 버튼
    public Button blacksmithUnlockButton; // 대장장이 해금 버튼
    TextMeshProUGUI blacksmithUnlockText; // 대장장이 해금 비용 텍스트
    [Header("Day Panel")]
    GameObject heroMenuPanel, // 용사 메뉴 패널
    heroCreationPanel, // 용사 제작 패널
    heroRosterPanel, // 용사 명단 패널
    heroRosterButtonPrefab, // 용사 명단 버튼 프리팹
    itemSelectionPanel, // 아이템 선택 패널
    itemSelectionButtonPrefab, // 아이템 선택 버튼 프리팹
    customerButtonPrefab; // 고객 버튼 프리팹
    TMP_InputField nameInput, // 용사 이름 입력 필드
    descInput; // 용사 설명 입력 필드
    Slider costSlider; // 용사 제작 비용 슬라이더
    TMP_Dropdown elementDropdown; // 용사 속성 드롭다운
    Image iconPreview, // 용사 아이콘 미리보기
    customerIcon; // 고객 아이콘
    Button selectIconButton, // 아이콘 선택 버튼
    heroCreationButton, // 용사 제작 버튼
    heroCreationCencelButton, // 용사 제작 취소 버튼
    heroRosterCancelButton,// 용사 명단 나가기 버튼
    itemSelectionCancelButton, // 아이템 선택 취소 버튼
    heroMenuButton, // 용사 메뉴 버튼
    heroRosterPanelButton, // 용사 명단 탭 버튼
    heroCreationPanelButton; // 용사 제작 탭 버튼
    public Button heroUnlockButton; // 용사 해금 버튼
    Transform heroRosterParent, // 용사 명단 부모
    itemSelectionParent, // 아이템 선택 부모
    customerListParent; // 고객 리스트 부모
    Customer selectedCustomer; // 선택된 고객
    TextMeshProUGUI heroUnlockText, // 용사 해금 비용 텍스트
    costValueText, // 용사 제작 비용 텍스트
    customerNameText; // 고객 이름
    [Header("Night Panel")]
    Transform resultListParent; // 결과 리스트 부모
    GameObject resultEntryPrefab; // 결과 항목 프리팹
    [Header("Inventory")]
    public Transform inventoryPanel; // 인벤토리 패널
    public GameObject inventoryButtonPrefab; // 인벤토리 버튼 프리팹
    [Header("Text")]
    public TextMeshProUGUI resultText; // 결과 메시지
    public TextMeshProUGUI goldText; // 소지금 텍스트
    public TextMeshProUGUI dayText; // 날짜 텍스트
    [Header("Button")]
    public Button nextPhaseButton; // 다음 단계 버튼
    

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitPanel();
        BindUI();
        InitUI();
    }

    void Start()
    {
        SetupCostSlider();
    }

    void BindUI()
    {
        // UI 요소 바인딩
        // 아침 패널
        blacksmithMenuPanel = morningPanel.transform.Find("Blacksmith Menu Panel").gameObject;
        blacksmithForgePanel = morningPanel.transform.Find("Blacksmith Forge Panel").gameObject;
        blacksmithRecipePanel = morningPanel.transform.Find("Blacksmith Recipe Panel").gameObject;
        forgeButtonPrefab = Resources.Load<GameObject>("Prefabs/Buttons/Blacksmith Forge Button");
        forgeButtonParent = blacksmithForgePanel.transform.Find("Blacksmith Forge Parent");
        blacksmithForgeCancelButton = blacksmithForgePanel.transform.Find("Cancel Button").GetComponent<Button>();
        blacksmithResipeCancelButton = blacksmithRecipePanel.transform.Find("Cancel Button").GetComponent<Button>();
        blacksmithMenuButton = morningPanel.transform.Find("Blacksmith Menu Button").GetComponent<Button>();
        blacksmithForgeButton = blacksmithMenuPanel.transform.Find("Blacksmith Forge Button").GetComponent<Button>();
        blacksmithRecipeButton = blacksmithMenuPanel.transform.Find("Blacksmith Recipe Button").GetComponent<Button>();
        blacksmithUnlockButton = morningPanel.transform.Find("Blacksmith Unlock Button").GetComponent<Button>();
        blacksmithUnlockText = blacksmithUnlockButton.GetComponentInChildren<TextMeshProUGUI>();
        // 낮 패널
        heroMenuPanel = dayPanel.transform.Find("Hero Menu Panel").gameObject;
        heroCreationPanel = dayPanel.transform.Find("Hero Creation Panel").gameObject;
        heroRosterPanel = dayPanel.transform.Find("Hero Roster Panel").gameObject;
        heroRosterButtonPrefab = Resources.Load<GameObject>("Prefabs/Buttons/Hero Roster Button");
        itemSelectionPanel = dayPanel.transform.Find("Item Selection Panel").gameObject;
        itemSelectionButtonPrefab = Resources.Load<GameObject>("Prefabs/Buttons/Item Selection Button");
        customerButtonPrefab = Resources.Load<GameObject>("Prefabs/Buttons/Customer Button");
        nameInput = heroCreationPanel.transform.Find("Hero Name Input").GetComponent<TMP_InputField>();
        descInput = heroCreationPanel.transform.Find("Hero Desc Input").GetComponent<TMP_InputField>();
        costSlider = heroCreationPanel.transform.Find("Cost Slider").GetComponent<Slider>();
        elementDropdown = heroCreationPanel.transform.Find("Hero Element").GetComponent<TMP_Dropdown>();
        iconPreview = heroCreationPanel.transform.Find("Hero Icon Button").GetComponent<Image>(); // 미완
        customerIcon = dayPanel.transform.Find("Customer Image").GetComponent<Image>();
        selectIconButton = heroCreationPanel.transform.Find("Hero Icon Button").GetComponent<Button>();
        heroCreationButton = heroCreationPanel.transform.Find("Hero Creation Button").GetComponent<Button>();
        heroCreationCencelButton = heroCreationPanel.transform.Find("Cancel Button").GetComponent<Button>();
        heroRosterCancelButton = heroRosterPanel.transform.Find("Cancel Button").GetComponent<Button>();
        itemSelectionCancelButton = itemSelectionPanel.transform.Find("Cancel Button").GetComponent<Button>();
        heroMenuButton = dayPanel.transform.Find("Hero Menu Button").GetComponent<Button>();
        heroRosterPanelButton = heroMenuPanel.transform.Find("Hero Roster Panel Button").GetComponent<Button>();
        heroCreationPanelButton = heroMenuPanel.transform.Find("Hero Creation Panel Button").GetComponent<Button>();
        heroUnlockButton = dayPanel.transform.Find("Hero Unlock Button").GetComponent<Button>();
        heroRosterParent = heroRosterPanel.transform.Find("Hero Roster Parent");
        itemSelectionParent = itemSelectionPanel.transform.Find("Item Selection Parent");
        customerListParent = dayPanel.transform.Find("Customer List Parent");
        heroUnlockText = heroUnlockButton.GetComponentInChildren<TextMeshProUGUI>();
        costValueText = heroCreationPanel.transform.Find("Cost Value Text").GetComponent<TextMeshProUGUI>();
        customerNameText = customerIcon.GetComponentInChildren<TextMeshProUGUI>();
        // 밤 패널
        resultListParent = nightPanel.transform.Find("Result List Parent");
        resultEntryPrefab = Resources.Load<GameObject>("Prefabs/Result Entry");
        // 공통 부분
    }

    void InitUI()
    {
        elementDropdown.ClearOptions();
        elementDropdown.AddOptions(System.Enum.GetNames(typeof(Element)).ToList());

        blacksmithMenuButton.interactable = false;
        heroMenuButton.interactable = false;

        blacksmithMenuButton.onClick.AddListener(() => {
            blacksmithMenuPanel.SetActive(!blacksmithMenuPanel.activeSelf);
        });
        blacksmithForgeButton.onClick.AddListener(ShowBlacksmithForgeTab);
        blacksmithRecipeButton.onClick.AddListener(ShowBlacksmithRecipeTab);
        blacksmithForgeCancelButton.onClick.AddListener(() => {
            blacksmithForgePanel.SetActive(false);
        });
        blacksmithResipeCancelButton.onClick.AddListener(() => {
            blacksmithRecipePanel.SetActive(false);
        });
        heroMenuButton.onClick.AddListener(() => {
            heroMenuPanel.SetActive(!heroMenuPanel.activeSelf);
        });
        heroCreationPanelButton.onClick.AddListener(ShowHeroCreationTab);
        heroRosterPanelButton.onClick.AddListener(ShowHeroRosterTab);
        heroCreationButton.onClick.AddListener(OnHeroCreationClicked);
        heroCreationCencelButton.onClick.AddListener(() => {
            heroCreationPanel.SetActive(false);
        });
        heroRosterCancelButton.onClick.AddListener(() => {
            heroRosterPanel.SetActive(false);
        });
        selectIconButton.onClick.AddListener(OnSelectIconClicked);
        heroUnlockButton.onClick.AddListener(() => {
            GameManager.Instance.PurchaseHeroSystemUnlock();
        });
        blacksmithUnlockButton.onClick.AddListener(() => {
            GameManager.Instance.PurchaseBlacksmithSystemUnlock();
        });
        costSlider.onValueChanged.AddListener(value => {
            costValueText.text = $"투입 Gold: {value}G";
        });
        itemSelectionCancelButton.onClick.AddListener(() => CloseItemSelectionPanel());
        heroUnlockText.text = $"용사 시스템 해금 비용: {GameManager.Instance.heroUnlockGold}G";
        blacksmithUnlockText.text = $"대장장이 시스템 해금 비용: {GameManager.Instance.blacksmithUnlockGold}G";
        nextPhaseButton.onClick.AddListener(() => GameManager.Instance.NextPhase());

        heroMenuPanel.SetActive(false);
        heroCreationPanel.SetActive(false);
        heroRosterPanel.SetActive(false);
        blacksmithMenuPanel.SetActive(false);
        blacksmithForgePanel.SetActive(false);
        blacksmithRecipePanel.SetActive(false);
    }

    public void DisplayCustomer(Customer c) // 고객 정보 표시
    {
        customerNameText.text = $"{c.name} (Lv.{c.level} / {c.grade} / {c.element})\n\"{c.desc}\"";
        customerIcon.sprite = c.icon;
    }

    public void DisplayResult(string msg) // 결과 메시지 표시
    {
        resultText.text = msg;
    }

    public void UpdateGold(int gold) // 소지금 업데이트
    {
        goldText.text = $"소지금: {gold}G";

        if (heroCreationPanel.activeSelf)
        {
            costSlider.maxValue = gold;
            if (costSlider.value > gold)
                costSlider.value = gold;
            costValueText.text = $"투입 Gold: {(int)costSlider.value}G";
        }
    }

    public void UpdatePhase(int day, DayPhase dayPhase) // 날짜 업데이트
    {
        string[] phaseNames = { "아침", "낮", "밤" };
        dayText.text = $"{day}일 {phaseNames[(int)dayPhase]}";
    }

    public void ClearResult() // 결과 메시지 초기화
    {
        resultText.text = "";
    }

    void InitPanel()
    {
        // 패널 초기화
        morningPanel = Instantiate(morningPanelPrefab, panelParent);
        dayPanel = Instantiate(dayPanelPrefab, panelParent);
        nightPanel = Instantiate(nightPanelPrefab, panelParent);

        morningPanel.SetActive(false);
        dayPanel.SetActive(false);
        nightPanel.SetActive(false);
    }
    public void UpdateInventoryUI() // 인벤토리 UI 업데이트
    {
        foreach (Transform child in inventoryPanel)
            Destroy(child.gameObject);

        foreach (ItemInstance item in InventoryManager.Instance.GetInventory())
        {
            GameObject btn = Instantiate(inventoryButtonPrefab, inventoryPanel);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{item.data.itemName} x{item.quantity}";
        }
    }

    public void OnBlacksmithUnlocked()
    {
        blacksmithMenuButton.interactable = true;
        DisplayResult("대장장이 고용이 해금되었습니다!");
    }

    public void OnHeroSystemUnlocked()
    {
        heroMenuButton.interactable = true;
        DisplayResult("용사 제작 시스템이 해금되었습니다!");
    }

    void ShowBlacksmithForgeTab()
    {
        // 초기화
        heroRosterPanel.SetActive(false);
        blacksmithMenuPanel.SetActive(false);

        blacksmithRecipePanel.SetActive(true);
        blacksmithForgePanel.SetActive(true);

        // 레시피 UI 업데이트
        UpdateBlacksmithUI();
    }

    void ShowBlacksmithRecipeTab()
    {
        // 초기화
        heroRosterPanel.SetActive(false);
        blacksmithMenuPanel.SetActive(false);

        blacksmithRecipePanel.SetActive(true);
    }

    void ShowHeroCreationTab()
    {
        // 초기화
        heroMenuPanel.SetActive(false);
        heroRosterPanel.SetActive(false);
        blacksmithMenuPanel.SetActive(false);
        
        nameInput.text = "";
        descInput.text = "";
        elementDropdown.value = 0;
        iconPreview.sprite = null;
        SetupCostSlider();
        heroCreationPanel.SetActive(true);
    }

    void ShowHeroRosterTab()
    {
        heroMenuPanel.SetActive(false);
        heroCreationPanel.SetActive(false);
        heroRosterPanel.SetActive(true);
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
            DisplayResult("용사 이름을 입력하세요!");
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
        costSlider.wholeNumbers = true;        // 정수 단위로만
        costSlider.value = currentGold / 2;        // 기본값은 최대치의 반
        costValueText.text = $"투입 Gold: {costSlider.value}G";
    }

    public void UpdateBlacksmithUI()
    {
        // 이전 버튼 삭제
        foreach (Transform t in forgeButtonParent) Destroy(t.gameObject);

        // 레시피마다 버튼 생성
        for (int i = 0; i < BlacksmithManager.Instance.recipes.Count; i++)
        {
            int idx = i;
            RecipeData recipe = BlacksmithManager.Instance.recipes[i];

            GameObject btn = Instantiate(forgeButtonPrefab, forgeButtonParent);
            TextMeshProUGUI txt = btn.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = $"{recipe.resultItem.itemName} ({recipe.cost}G)";

            btn.GetComponent<Button>()
               .onClick.AddListener(() => {
                    BlacksmithManager.Instance.ForgeItem(idx);
               });
        }
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

    public void ShowMorningUI() // 아침 패널 표시
    {
        morningPanel.SetActive(true);
        dayPanel.SetActive(false);
        nightPanel.SetActive(false);
    }

    public void ShowDayUI(List<Customer> customers) // 아침 패널 표시
    {
        morningPanel.SetActive(false);
        dayPanel.SetActive(true);
        nightPanel.SetActive(false);

        foreach (Transform t in customerListParent) Destroy(t.gameObject);
        foreach (Customer c in customers)
        {
            GameObject btn = Instantiate(customerButtonPrefab, customerListParent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text =
                $"{c.name} (Lv{c.level}/{c.grade})";
            btn.GetComponent<Button>().onClick.AddListener(() => {
                // 아이템 선택 팝업 열고 선택된 아이템 대여
                OpenItemSelectionPanel(c);
            });
        }
    }

    public void ShowNightUI(List<DayResult> dayResults) // 아침 패널 표시
    {
        morningPanel.SetActive(false);
        dayPanel.SetActive(false);
        nightPanel.SetActive(true);

        foreach (Transform t in resultListParent) Destroy(t.gameObject);
        foreach (DayResult r in dayResults)
        {
            GameObject entry = Instantiate(resultEntryPrefab, resultListParent);
            entry.GetComponentInChildren<TextMeshProUGUI>().text =
                r.item == null
                    ? $"{r.customer.name} 아이템 없음"
                    : r.isSuccess
                        ? $"{r.customer.name} 성공! +{r.reward}G"
                        : $"{r.customer.name} 실패…";
        }
    }

    public void OpenItemSelectionPanel(Customer customer) // 아이템 선택 팝업 열기
    {
        selectedCustomer = customer;
        
        foreach (Transform t in itemSelectionParent) Destroy(t.gameObject);
        foreach (ItemInstance item in InventoryManager.Instance.GetInventory())
        {
            GameObject btn = Instantiate(itemSelectionButtonPrefab, itemSelectionParent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{item.data.itemName} x{item.quantity}";
            btn.GetComponent<Button>().onClick.AddListener(() => {
                if (GameManager.Instance.TryAssignItem(selectedCustomer, item.data))
                {
                    DisplayResult($"{selectedCustomer.name}에게 {item.data.itemName}을(를) 빌려주었습니다.");
                }
                else
                {
                    DisplayResult("재고가 없습니다!");
                }
                CloseItemSelectionPanel();                
            });
        }
        itemSelectionPanel.SetActive(true);
    }

    void CloseItemSelectionPanel() // 아이템 선택 팝업 닫기
    {
        itemSelectionPanel.SetActive(false);
        selectedCustomer = null;
    }
}
