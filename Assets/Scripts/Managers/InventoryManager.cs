using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private List<WeaponInstance> weaponInventory = new List<WeaponInstance>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddWeapon(WeaponData data, int amount)
    {
        var existing = weaponInventory.Find(i => i.data == data);
        if (existing != null) existing.quantity += amount;
        else weaponInventory.Add(new WeaponInstance(data, amount));
        CommonUI.Instance.UpdateInventoryUI();
    }

    public bool UseWeapon(WeaponData data)
    {
<<<<<<< HEAD
<<<<<<< HEAD
        WeaponInstance instance = weaponInventory.Find(i => i.data == data);
=======
        var instance = weaponInventory.Find(i => i.data == data);
>>>>>>> parent of 0773a29 (mvp2)
=======
        var instance = weaponInventory.Find(i => i.data == data);
>>>>>>> parent of 0773a29 (mvp2)
        if (instance != null && instance.quantity > 0)
        {
            instance.quantity--;
            CommonUI.Instance.UpdateInventoryUI();
            return true;
        }
        return false;
    }

    public int GetWeaponQuantity(WeaponData data)
    {
        var instance = weaponInventory.Find(i => i.data == data);
        return instance != null ? instance.quantity : 0;
    }

    public List<WeaponInstance> GetWeaponInventory()
    {
        return weaponInventory;
    }
}
