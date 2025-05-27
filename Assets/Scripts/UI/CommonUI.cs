using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class CommonUI : MonoBehaviour
{
    public static CommonUI Instance;

    public GameObject overlayBlocker; // 오버레이 블로커
    [Header("Inventory")]
    public Transform inventoryPanelParent; // 인벤토리 패널 생성 위치
    public GameObject inventoryButtonPrefab; // 인벤토리 버튼 프리팹
    [Header("Result")]
    public TextMeshProUGUI resultText; // 결과 메시지
    public TextMeshProUGUI goldText; // 소지금 텍스트
    public TextMeshProUGUI dayText; // 날짜 텍스트
    public Button nextPhaseButton; // 다음 단계 버튼
    [Header("Popup")]
    [SerializeField] Transform popupParent; // 팝업 생성 위치
    [SerializeField] GameObject confirmPopupPrefab;
    [SerializeField] GameObject messagePopupPrefab;
    ConfirmPopup confirmPopup;
    MessagePopup messagePopup;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
        if (inventoryPanelParent == null)
        {
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

    public void ShowConfirmation(string message, UnityAction onYes, UnityAction onNo = null)
    {
        overlayBlocker.SetActive(true);

        if (confirmPopup == null)
        {
            confirmPopup = Instantiate(confirmPopupPrefab, popupParent).GetComponent<ConfirmPopup>();
            confirmPopup.gameObject.SetActive(false);
        }

        // onNo가 null이면 "취소 시 아무것도 안 함"
        UnityAction cancelAction = onNo ?? (() => { });

        confirmPopup.Setup(message, onYes, cancelAction);
        confirmPopup.gameObject.SetActive(true);
    }


    public void ShowMessage(string message)
    {
        overlayBlocker.SetActive(true);

        if (messagePopup == null)
        {
            messagePopup = Instantiate(messagePopupPrefab, popupParent).GetComponent<MessagePopup>();
            messagePopup.gameObject.SetActive(false);
        }
        messagePopup.Setup(message);
        messagePopup.gameObject.SetActive(true);
    }
}
