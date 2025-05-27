using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class CommonUI : MonoBehaviour
{
    public static CommonUI Instance;

    public GameObject overlayBlocker;
    [Header("Inventory")]
    public Transform inventoryPanelParent;
    public GameObject inventoryButtonPrefab;
    [Header("Result")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI dayText;
    public Button nextPhaseButton;
    [Header("Popup")]
    public Transform popupParent;
    public GameObject confirmPopupPrefab;
    public GameObject messagePopupPrefab;
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

    public void DisplayResult(string msg) { resultText.text = msg; }
    public void UpdateGold(int gold) { goldText.text = $"소지금: {gold}G"; }

    public void UpdatePhase(int day, DayPhase dayPhase)
    {
        string[] phaseNames = { "아침", "낮", "밤" };
        dayText.text = $"{day}일 {phaseNames[(int)dayPhase]}";
    }

    public void UpdateInventoryUI()
    {
        if (inventoryPanelParent == null)
        {
            Debug.LogError("CommonUI.inventoryPanel이 할당되지 않았습니다! UIBinder.canvas 설정을 확인하세요.");
            return;
        }
        foreach (Transform child in inventoryPanelParent)
            Destroy(child.gameObject);

        foreach (WeaponInstance weapon in InventoryManager.Instance.GetWeaponInventory())
        {
            GameObject btn = Instantiate(inventoryButtonPrefab, inventoryPanelParent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{weapon.data.weaponName} x{weapon.quantity}";
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
