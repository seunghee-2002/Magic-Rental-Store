using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private List<WeaponInstance> weaponInventory = new List<WeaponInstance>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddWeapon(WeaponData data, int amount) // 아이템 추가
    {
        WeaponInstance existing = weaponInventory.Find(i => i.data == data);
        if (existing != null) existing.quantity += amount;
        else weaponInventory.Add(new WeaponInstance(data, amount));
        UpdateInventoryUI();
    }

    public bool UseWeapon(WeaponData data) // 도구 사용 & 여부 확인
    {
        WeaponInstance instance = weaponInventory.Find(i => i.data == data);    
        if (instance != null && instance.quantity > 0)
        {
            instance.quantity--;
            UpdateInventoryUI();
            return true;
        }
        return false;
    }

    public int GetWeaponQuantity(WeaponData data) // 무기 수량 확인
    {
        WeaponInstance instance = weaponInventory.Find(i => i.data == data);
        return instance != null ? instance.quantity : 0;
    }

    public List<WeaponInstance> GetWeaponInventory() // 무기 인벤토리 확인
    {
        return weaponInventory;
    }

    private void UpdateInventoryUI()
    {
        switch (GameManager.Instance.currentPhase)
        {
            case DayPhase.Morning:
                UIManager.Instance.MorningView.UpdateInventoryUI();
                break;
            case DayPhase.Day:
                UIManager.Instance.DayView.UpdateInventoryUI();
                break;
            case DayPhase.Night:
                UIManager.Instance.NightView.UpdateInventoryUI();
                break;
        }
    }
}
