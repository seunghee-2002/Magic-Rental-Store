using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<ItemData> testStartItems; // 테스트용 시작 아이템

    public List<CustomerData> customerPool;
    public CustomerData currentCustomer;

    public int gold = 1000;

    [Range(0f, 1f)] public float baseSuccessRate = 0.5f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 예시: 테스트용 초기 인벤토리
        foreach (var item in testStartItems)
            InventoryManager.Instance.AddItem(item, Random.Range(1, 4));

        UIManager.Instance.UpdateInventoryUI();
        NextCustomer();
    }

    public void NextCustomer() // 다음 고객 생성성
    {
        int rand = Random.Range(0, customerPool.Count);
        currentCustomer = customerPool[rand];
        UIManager.Instance.DisplayCustomer(currentCustomer);
    }

    public void TryLendItem(ItemData item) // 아이템 대여 시도
    {
        if (!InventoryManager.Instance.UseItem(item))
        {
            UIManager.Instance.DisplayResult("재고가 없습니다!");
            return;
        }

        float finalRate = baseSuccessRate;
        if (item.element == currentCustomer.element)
            finalRate += 0.2f;

        bool success = Random.value < finalRate;
        int reward = 300;

        if (success)
        {
            gold += reward;
            UIManager.Instance.DisplayResult($"성공! +{reward}G 획득");
        }
        else
        {
            UIManager.Instance.DisplayResult($"실패... 아이템 손실");
        }

        UIManager.Instance.UpdateGold(gold);
        UIManager.Instance.UpdateInventoryUI();
    }
}
