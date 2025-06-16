using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [Header("Weapon Settings")]
    public int initialWeaponCount = 5;
    public float weaponDurabilityLossPerUse = 0.1f;
    public float repairCostMultiplier = 1.5f;

    private List<WeaponInstance> availableWeapons = new List<WeaponInstance>();
    private List<WeaponInstance> rentedWeapons = new List<WeaponInstance>();
    private GameManager gameManager;
    private DataManager dataManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            gameManager = GameManager.Instance;
            dataManager = DataManager.Instance;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize()
    {
        ClearAllWeapons();
        GenerateInitialWeapons();
    }

    private void ClearAllWeapons()
    {
        availableWeapons.Clear();
        rentedWeapons.Clear();
    }

    private void GenerateInitialWeapons()
    {
        for (int i = 0; i < initialWeaponCount; i++)
        {
            GenerateWeapon();
        }
    }

    public void GenerateWeapon()
    {
        WeaponData weaponData = dataManager.GetRandomWeaponData();
        if (weaponData == null) return;

        WeaponInstance weapon = new WeaponInstance
        {
            data = weaponData,
            durability = 1.0f,
            isRented = false
        };

        availableWeapons.Add(weapon);
        gameManager.OnWeaponGenerated?.Invoke(weapon);
    }

    public bool RentWeapon(WeaponInstance weapon, CustomerInstance customer)
    {
        if (weapon == null || customer == null) return false;
        if (!availableWeapons.Contains(weapon)) return false;

        weapon.isRented = true;
        customer.weapon = weapon;
        availableWeapons.Remove(weapon);
        rentedWeapons.Add(weapon);

        gameManager.OnWeaponRented?.Invoke(weapon);
        return true;
    }

    public void ReturnWeapon(WeaponInstance weapon)
    {
        if (weapon == null) return;
        if (!rentedWeapons.Contains(weapon)) return;

        // 내구도 감소
        weapon.durability -= weaponDurabilityLossPerUse;
        weapon.durability = Mathf.Clamp(weapon.durability, 0f, 1f);

        weapon.isRented = false;
        rentedWeapons.Remove(weapon);
        availableWeapons.Add(weapon);

        gameManager.OnWeaponReturned?.Invoke(weapon);
    }

    public bool RepairWeapon(WeaponInstance weapon)
    {
        if (weapon == null) return false;
        if (weapon.durability >= 1.0f) return false;

        int repairCost = CalculateRepairCost(weapon);
        if (gameManager.Gold < repairCost) return false;

        gameManager.AddGold(-repairCost);
        weapon.durability = 1.0f;
        gameManager.OnWeaponRepaired?.Invoke(weapon);
        return true;
    }

    private int CalculateRepairCost(WeaponInstance weapon)
    {
        float durabilityLoss = 1.0f - weapon.durability;
        int baseCost = weapon.data.basePrice;
        return Mathf.RoundToInt(baseCost * durabilityLoss * repairCostMultiplier);
    }

    public List<WeaponInstance> GetAvailableWeapons()
    {
        return availableWeapons;
    }

    public List<WeaponInstance> GetRentedWeapons()
    {
        return rentedWeapons;
    }

    public bool IsWeaponAvailable(WeaponInstance weapon)
    {
        return availableWeapons.Contains(weapon);
    }

    public bool IsWeaponRented(WeaponInstance weapon)
    {
        return rentedWeapons.Contains(weapon);
    }
} 