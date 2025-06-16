using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Linq;

public class MorningModel : DayPhaseModel
{
    // 기존 대장장이 관련 속성
    public bool IsBlacksmithUnlocked { get; private set; }
    public int BlacksmithUnlockCost { get; private set; } = 5000;
    public List<RecipeData> Recipes => DataManager.Instance.recipes;

    public List<CustomerInstance> CurrentCustomers { get; private set; }
    public bool IsPanelActive { get; set; }

    [Header("등급 확률 (합 1.0)")]
    private float commonProb = 0.6f;
    private float uncommonProb = 0.2f;
    private float rareProb = 0.1f;
    private float epicProb = 0.07f;
    private float legendaryProb = 0.03f;

    private Dictionary<WeaponData, bool> isPurchaseWeapon { get; private set; }
    private int stockCount = 5; // 상점 무기 개수
    private List<WeaponData> currentStock = new List<WeaponData>();
    public List<WeaponData> CurrentStock { get; private set; }

    public MorningModel() : base()
    {
        // 기존 초기화
        IsBlacksmithUnlocked = false;
        ResetPurchaseStatus();
        CurrentCustomers = new List<CustomerInstance>();
        isPurchaseWeapon = new Dictionary<WeaponData, bool>();
        CurrentStock = new List<WeaponData>();
    }

    public bool TryUnlockBlacksmith()
    {
        if (IsBlacksmithUnlocked) return false;
        
        if (GameManager.Instance.SpendGold(BlacksmithUnlockCost))
        {
            IsBlacksmithUnlocked = true;
            return true;
        }
        return false;
    }

    public bool TryForgeWeapon(int recipeIndex)
    {
        if (!IsBlacksmithUnlocked) return false;
        if (recipeIndex < 0 || recipeIndex >= Recipes.Count) return false;
        
        var recipe = Recipes[recipeIndex];

        // 비용 차감
        if (!GameManager.Instance.SpendGold(recipe.cost))
            return false;

        // 제작
        InventoryManager.Instance.AddWeapon(recipe.resultWeapon, 1);
        return true;
    }

    public void AddRecipe(RecipeData recipe)
    {
        Recipes.Add(recipe);
    }

    // 무기 상점 관련 메서드
    public void GenerateStock()
    {
        currentStock.Clear();
        for (int i = 0; i < stockCount; i++)
        {
            currentStock.Add(GetRandomWeapon());
        }
    }

    private WeaponData GetRandomWeapon()
    {
        float roll = Random.value;

        if (roll < commonProb)
            return DataManager.Instance.GetWeaponsByGrade(Grade.Common).GetRandomElement();
        else if (roll < commonProb + uncommonProb)
            return DataManager.Instance.GetWeaponsByGrade(Grade.Uncommon).GetRandomElement();
        else if (roll < commonProb + uncommonProb + rareProb)
            return DataManager.Instance.GetWeaponsByGrade(Grade.Rare).GetRandomElement();
        else if (roll < commonProb + uncommonProb + rareProb + epicProb)
            return DataManager.Instance.GetWeaponsByGrade(Grade.Epic).GetRandomElement();
        else
            return DataManager.Instance.GetWeaponsByGrade(Grade.Legendary).GetRandomElement();
    }

    public bool Purchase(WeaponData weapon, int index)
    {
        if (GameManager.Instance.SpendGold(weapon.cost))
        {
            InventoryManager.Instance.AddWeapon(weapon, 1);
            isPurchaseWeapon[weapon] = true;
            return true;
        }
        return false;
    }

    public void ResetPurchaseStatus()
    {
        isPurchaseWeapon.Clear();
    }

    public List<WeaponData> GetCurrentStock()
    {
        return currentStock;
    }

    public List<bool> GetPurchaseStatus()
    {
        return isPurchaseWeapon.Values.ToList();
    }

    public void ClearStock()
    {
        // 재고 초기화
        currentStock.Clear();
        isPurchaseWeapon.Clear();
    }

    public void GenerateCustomers()
    {
        CurrentCustomers.Clear();
        for (int i = 0; i < GameManager.Instance.customersPerDay; i++)
        {
            Grade grade = GetRandomGrade();
            CustomerData data = GameManager.Instance.GetRandomCustomerByGrade(grade);
            if (data != null)
            {
                CurrentCustomers.Add(new CustomerInstance(data));
            }
        }
        GameManager.Instance.NotifyCustomersChanged();
    }

    private Grade GetRandomGrade()
    {
        float random = Random.value;
        if (random < 0.6f) return Grade.Common;
        if (random < 0.85f) return Grade.Uncommon;
        if (random < 0.95f) return Grade.Rare;
        if (random < 0.99f) return Grade.Epic;
        return Grade.Legendary;
    }

    public void ClearCurrentCustomer()
    {
        CurrentCustomers.Clear();
        GameManager.Instance.NotifyCustomersChanged();
    }

    public void UnlockBlacksmith()
    {
        IsBlacksmithUnlocked = true;
    }

    public void SetPurchaseStatus(WeaponData weapon, bool isPurchased)
    {
        isPurchaseWeapon[weapon] = isPurchased;
    }

    public bool IsWeaponPurchased(WeaponData weapon)
    {
        return isPurchaseWeapon.ContainsKey(weapon) && isPurchaseWeapon[weapon];
    }

    public void UpdateCurrentStock(List<WeaponData> stock)
    {
        CurrentStock = stock;
    }
} 