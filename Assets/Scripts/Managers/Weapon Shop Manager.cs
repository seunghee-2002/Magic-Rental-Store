using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
<<<<<<< HEAD:Assets/Scripts/Managers/WeaponShopManager.cs
using System.Linq;
=======
using Random = UnityEngine.Random;
>>>>>>> parent of 0773a29 (mvp2):Assets/Scripts/Managers/Weapon Shop Manager.cs

public class WeaponShopManager : MonoBehaviour 
{
<<<<<<< HEAD:Assets/Scripts/Managers/WeaponShopManager.cs
    public static WeaponShopManager Instance { get; private set; }

    [Header("Weapon Pool")]
=======
    public static WeaponShopManager Instance;
    
    [Header("무기 풀")]
>>>>>>> parent of 0773a29 (mvp2):Assets/Scripts/Managers/Weapon Shop Manager.cs
=======
using Random = UnityEngine.Random;

public class WeaponShopManager : MonoBehaviour 
{
    public static WeaponShopManager Instance;
    
    [Header("무기 풀")]
>>>>>>> parent of 0773a29 (mvp2)
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
<<<<<<< HEAD
<<<<<<< HEAD:Assets/Scripts/Managers/WeaponShopManager.cs
    [Header("Status")]
    public List<bool> isPurchaseWeapon;
    int stockCount = 5;
=======

    public int stockCount = 5; // 상점 무기 개수

>>>>>>> parent of 0773a29 (mvp2):Assets/Scripts/Managers/Weapon Shop Manager.cs
=======

    public int stockCount = 5; // 상점 무기 개수

>>>>>>> parent of 0773a29 (mvp2)
    public List<WeaponData> currentStock = new List<WeaponData>();

    private void Awake()
    {
<<<<<<< HEAD
<<<<<<< HEAD:Assets/Scripts/Managers/WeaponShopManager.cs
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        ResetPurchaseStatus();
=======
=======
>>>>>>> parent of 0773a29 (mvp2)
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
<<<<<<< HEAD
>>>>>>> parent of 0773a29 (mvp2):Assets/Scripts/Managers/Weapon Shop Manager.cs
=======
>>>>>>> parent of 0773a29 (mvp2)
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

<<<<<<< HEAD
<<<<<<< HEAD:Assets/Scripts/Managers/WeaponShopManager.cs
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
=======
}
>>>>>>> parent of 0773a29 (mvp2):Assets/Scripts/Managers/Weapon Shop Manager.cs
=======
}
>>>>>>> parent of 0773a29 (mvp2)
