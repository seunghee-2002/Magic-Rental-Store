using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponShopManager : MonoBehaviour
{
    public static WeaponShopManager Instance { get; private set; }

    [Header("Weapon Pool")]
    public List<WeaponData> commonWeapons;
    public List<WeaponData> uncommonWeapons;
    public List<WeaponData> rareWeapons;
    public List<WeaponData> epicWeapons;
    public List<WeaponData> legendaryWeapons;
    [Header("등급 확률 (합 1.0)")]
    public float commonProb = 0.6f;
    public float uncommonProb = 0.2f;
    public float rareProb = 0.1f;
    public float epicProb = 0.07f;
    public float legendaryProb = 0.03f;
    [Header("Status")]
    public List<bool> isPurchaseWeapon;
    int stockCount = 5;
    public List<WeaponData> currentStock = new List<WeaponData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        ResetPurchaseStatus();
    }

    public void GenerateStock()
    {
        currentStock.Clear();
        for (int i = 0; i < stockCount; i++)
            currentStock.Add(GetRandomWeapon());
    }

    WeaponData GetRandomWeapon()
    {
        float roll = Random.value;

        if (roll < commonProb)
            return commonWeapons.GetRandomElement();
        else if (roll < commonProb + uncommonProb)
            return uncommonWeapons.GetRandomElement();
        else if (roll < commonProb + uncommonProb + rareProb)
            return rareWeapons.GetRandomElement();
        else if (roll < commonProb + uncommonProb + rareProb + epicProb)
            return epicWeapons.GetRandomElement();
        else
            return legendaryWeapons.GetRandomElement();
    }

    public void Purchase(WeaponData weapon, int index)
    {
        if (GameManager.Instance.SpendGold(weapon.cost))
        {
            InventoryManager.Instance.AddWeapon(weapon, 1);
            isPurchaseWeapon[index] = true;
            CommonUI.Instance.DisplayResult($"{weapon.weaponName} 구매 완료!");
        }
    }

    public void ResetPurchaseStatus()
    {
        isPurchaseWeapon = Enumerable.Repeat(false, stockCount).ToList();
    }
}
