using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CommonUI : MonoBehaviour
{
    public static CommonUI Instance;
    public Transform inventoryPanelParent; // 인벤토리 패널 생성 위치
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

    public void BindUI()
    {
        inventoryPanelParent = UIBinder.Instance.inventoryPanelParent;
        inventoryButtonPrefab = UIBinder.Instance.inventoryButtonPrefab;
        resultText = UIBinder.Instance.resultText;
        goldText = UIBinder.Instance.goldText;
        dayText = UIBinder.Instance.dayText;
        nextPhaseButton = UIBinder.Instance.nextPhaseButton;
    }

    public void InitUI()
    {
        nextPhaseButton.onClick.RemoveAllListeners();
        nextPhaseButton.onClick.AddListener(() => GameManager.Instance.NextPhase());

        UpdateGold(GameManager.Instance.gold);
        UpdatePhase(GameManager.Instance.currentDay, GameManager.Instance.currentPhase);
        UpdateInventoryUI();
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
        if (inventoryPanelParent == null) {
            Debug.LogError("CommonUI.inventoryPanel이 할당되지 않았습니다! UIBinder.canvas 설정을 확인하세요.");
            return;
        }
        foreach (Transform child in inventoryPanelParent)
            Destroy(child.gameObject);

        foreach (WeaponInstance Weapon in InventoryManager.Instance.GetWeaponInventory())
        {
            GameObject btn = Instantiate(inventoryButtonPrefab, inventoryPanelParent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{Weapon.data.weaponName} x{Weapon.quantity}";
        }
    }
}
