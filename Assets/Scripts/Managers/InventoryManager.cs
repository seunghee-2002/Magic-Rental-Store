using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<ItemInstance> inventory = new List<ItemInstance>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(ItemData data, int amount) // 아이템 추가
    {
        var existing = inventory.Find(i => i.data == data);
        if (existing != null) existing.quantity += amount;
        else inventory.Add(new ItemInstance(data, amount));
        UIManager.Instance.UpdateInventoryUI();
    }

    public bool UseItem(ItemData data) // 아이템 사용 & 여부 확인
    {
        var instance = inventory.Find(i => i.data == data);
        if (instance != null && instance.quantity > 0)
        {
            instance.quantity--;
            UIManager.Instance.UpdateInventoryUI();
            return true;
        }
        return false;
    }

    public int GetQuantity(ItemData data) // 아이템 수량 확인
    {
        var instance = inventory.Find(i => i.data == data);
        return instance != null ? instance.quantity : 0;
    }

    public List<ItemInstance> GetInventory() // 인벤토리 확인
    {
        return inventory;
    }
}
