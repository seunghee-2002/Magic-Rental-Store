using UnityEngine;
using TMPro;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class UIBinder : MonoBehaviour
{
    public static UIBinder Instance;
    [Header("Own Binding")]
    public Transform panelParent; // 패널 부모
    [SerializeField] Transform canvas;
    [Header("Panel")]
    public GameObject morningPanelPrefab; // 아침 패널 프리팹
    public GameObject dayPanelPrefab;   // 낮 패널 프리팹
    public GameObject nightPanelPrefab; // 밤 패널 프리팹
    [Header("Morning Panel")]
    public GameObject blacksmithMenuPanel; // 대장장이 메뉴 패널
    public GameObject blacksmithForgePanel; // 대장장이 제작 패널
    public GameObject blacksmithRecipePanel; // 대장장이 레시피 패널
    public GameObject forgeButtonPrefab; // 대장장이 제작 버튼 프리팹
    public Transform forgeButtonParent; // 대장장이 제작 버튼 부모
    public Button blacksmithForgeCancelButton; // 대장장이 제작 취소 버튼
    public Button blacksmithRecipeCancelButton; // 대장장이 레시피 나가기 버튼
    public Button blacksmithMenuButton; // 대장장이 메뉴 버튼
    public Button blacksmithForgeButton; // 대장장이 제작 버튼
    public Button blacksmithRecipeButton; // 대장장이 레시피 버튼
    public Button blacksmithUnlockButton; // 대장장이 해금 버튼
    public TextMeshProUGUI blacksmithUnlockText; // 대장장이 해금 비용 텍스트
    [Header("Day Panel")]
    public GameObject heroMenuPanel; // 용사 메뉴 패널
    public GameObject heroCreationPanel; // 용사 제작 패널
    public GameObject heroRosterPanel; // 용사 명단 패널
    public GameObject heroRosterButtonPrefab; // 용사 명단 버튼 프리팹
    public GameObject itemSelectionPanel; // 아이템 선택 패널
    public GameObject itemSelectionButtonPrefab; // 아이템 선택 버튼 프리팹
    public GameObject customerButtonPrefab; // 고객 버튼 프리팹
    public TMP_InputField nameInput; // 용사 이름 입력 필드
    public TMP_InputField descInput; // 용사 설명 입력 필드
    public Slider costSlider; // 용사 제작 비용 슬라이더
    public TMP_Dropdown elementDropdown; // 용사 속성 드롭다운
    public Image iconPreview; // 용사 아이콘 미리보기
    public Image customerIcon; // 고객 아이콘
    public Button selectIconButton; // 아이콘 선택 버튼
    public Button heroCreationButton; // 용사 제작 버튼
    public Button heroCreationCancelButton; // 용사 제작 취소 버튼
    public Button heroRosterCancelButton;// 용사 명단 나가기 버튼
    public Button itemSelectionCancelButton; // 아이템 선택 취소 버튼
    public Button heroMenuButton; // 용사 메뉴 버튼
    public Button heroRosterPanelButton; // 용사 명단 탭 버튼
    public Button heroCreationPanelButton; // 용사 제작 탭 버튼
    public Button heroUnlockButton; // 용사 해금 버튼
    public Transform heroRosterParent; // 용사 명단 부모
    public Transform itemSelectionParent; // 아이템 선택 부모
    public Transform customerListParent; // 고객 리스트 부모
    public Customer selectedCustomer; // 선택된 고객
    public TextMeshProUGUI heroUnlockText; // 용사 해금 비용 텍스트
    public TextMeshProUGUI costValueText; // 용사 제작 비용 텍스트
    public TextMeshProUGUI customerNameText; // 고객 이름
    [Header("Night Panel")]
    public Transform resultListParent; // 결과 리스트 부모
    public GameObject resultEntryPrefab; // 결과 항목 프리팹
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
    }

    public void Init()
    {
        morningPanelPrefab = Resources.Load<GameObject>("Prefabs/Panels/Morning Panel");
        dayPanelPrefab = Resources.Load<GameObject>("Prefabs/Panels/Day Panel");
        nightPanelPrefab = Resources.Load<GameObject>("Prefabs/Panels/Night Panel");
    }

    public void BindUI()
    {
        // UI 요소 바인딩
        GameObject morningPanel = GetComponent<MorningUI>().morningPanel;
        GameObject dayPanel = GetComponent<DayUI>().dayPanel;
        GameObject nightPanel = GetComponent<NightUI>().nightPanel;
        // 아침 패널
        blacksmithMenuPanel = morningPanel.transform.Find("Blacksmith Menu Panel").gameObject;
        blacksmithForgePanel = morningPanel.transform.Find("Blacksmith Forge Panel").gameObject;
        blacksmithRecipePanel = morningPanel.transform.Find("Blacksmith Recipe Panel").gameObject;
        forgeButtonPrefab = Resources.Load<GameObject>("Prefabs/Buttons/Blacksmith Forge Button");
        forgeButtonParent = blacksmithForgePanel.transform.Find("Blacksmith Forge Parent");
        blacksmithForgeCancelButton = blacksmithForgePanel.transform.Find("Cancel Button").GetComponent<Button>();
        blacksmithRecipeCancelButton = blacksmithRecipePanel.transform.Find("Cancel Button").GetComponent<Button>();
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
        heroCreationCancelButton = heroCreationPanel.transform.Find("Cancel Button").GetComponent<Button>();
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
        inventoryPanel = canvas.Find("Inventory Panel");
        inventoryButtonPrefab = Resources.Load<GameObject>("Prefabs/Buttons/Inventory Button");
        resultText = canvas.Find("Result Text").GetComponent<TextMeshProUGUI>();
        goldText = canvas.Find("Gold Text").GetComponent<TextMeshProUGUI>();
        dayText = canvas.Find("Day Text").GetComponent<TextMeshProUGUI>();
        nextPhaseButton = canvas.Find("Next Phase Button").GetComponent<Button>();
    }
}
