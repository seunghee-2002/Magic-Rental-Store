using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CommonUI : MonoBehaviour
{
    public static CommonUI Instance;
    public Transform inventoryPanel; // 인벤토리 패널
    public GameObject inventoryButtonPrefab; // 인벤토리 버튼 프리팹
    public TextMeshProUGUI resultText; // 결과 메시지
    public TextMeshProUGUI goldText; // 소지금 텍스트
    public TextMeshProUGUI dayText; // 날짜 텍스트
    public Button nextPhaseButton; // 다음 단계 버튼

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Init()
    {
        inventoryPanel = UIBinder.Instance.inventoryPanel;
        inventoryButtonPrefab = UIBinder.Instance.inventoryButtonPrefab;
        resultText = UIBinder.Instance.resultText;
        goldText = UIBinder.Instance.goldText;
        dayText = UIBinder.Instance.dayText;
        nextPhaseButton = UIBinder.Instance.nextPhaseButton;

        nextPhaseButton.onClick.AddListener(() => GameManager.Instance.NextPhase());
    }

    public void DisplayResult(string msg) // 결과 메시지 표시
    {
        resultText.text = msg;
    }

    public void UpdateGold(int gold) // 소지금 업데이트
    {
        goldText.text = $"소지금: {gold}G";
    }

    public void UpdatePhase(int day, DayPhase dayPhase) // 날짜 업데이트
    {
        string[] phaseNames = { "아침", "낮", "밤" };
        dayText.text = $"{day}일 {phaseNames[(int)dayPhase]}";
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
}
