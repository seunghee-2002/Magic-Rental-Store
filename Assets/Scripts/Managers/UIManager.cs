using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Customer")]
    public TextMeshProUGUI customerNameText; // 고객 이름
    public Image customerIcon; // 고객 아이콘

    [Header("Result")]
    public TextMeshProUGUI resultText; // 결과 메시지
    public TextMeshProUGUI goldText; // 소지금

    [Header("Inventory Panel")]
    public Transform inventoryPanel; // 인벤토리 패널
    public GameObject inventoryButtonPrefab; // 인벤토리 버튼 프리팹

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateGold(GameManager.Instance.gold); // 초기 소지금 업데이트
    }

    public void DisplayCustomer(CustomerData customer) // 고객 정보 표시
    {
        customerNameText.text = $"손님: {customer.customerName} ({customer.element})";
        customerIcon.sprite = customer.icon;
    }

    public void DisplayResult(string msg) // 결과 메시지 표시
    {
        resultText.text = msg;
    }

    public void UpdateGold(int gold) // 소지금 업데이트
    {
        goldText.text = $"소지금: {gold}G";
    }

    public void UpdateInventoryUI() // 인벤토리 UI 업데이트
    {
        foreach (Transform child in inventoryPanel)
            Destroy(child.gameObject);

        foreach (var item in InventoryManager.Instance.GetInventory())
        {
            GameObject btn = Instantiate(inventoryButtonPrefab, inventoryPanel);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{item.data.itemName} x{item.quantity}";

            btn.GetComponent<Button>().onClick.AddListener(() => {
                GameManager.Instance.TryLendItem(item.data);
            });
        }
    }
}
