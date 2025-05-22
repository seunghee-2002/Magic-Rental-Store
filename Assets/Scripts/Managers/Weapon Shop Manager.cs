using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponShopManager : MonoBehaviour 
{
    public static WeaponShopManager Instance;
    
    [Header("무기 풀")]
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

    public int stockCount = 5; // 상점 무기 개수

    public List<WeaponData> currentStock = new List<WeaponData>();

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void GenerateStock()
    {
        currentStock.Clear();
        for (int i = 0; i < stockCount; i++)
        {
            currentStock.Add(GetRandomWeapon());
        }
    }

    WeaponData GetRandomWeapon()
    {
        float roll = Random.value; // 0.0f 이상 1.0f 미만의 값

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

}