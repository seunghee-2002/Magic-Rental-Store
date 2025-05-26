using UnityEngine;
using System.Collections.Generic;

public class BlacksmithManager : MonoBehaviour
{
    public static BlacksmithManager Instance { get; private set; }

    [Header("Recipes")]
    public List<RecipeData> recipes;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ForgeWeapon(int recipeIndex)
    {
        if (!GameManager.Instance.isBlacksmithUnlocked) return;
        if (recipeIndex < 0 || recipeIndex >= recipes.Count) return;

        var r = recipes[recipeIndex];
        if (!GameManager.Instance.SpendGold(r.cost))
            return;

        InventoryManager.Instance.AddWeapon(r.resultWeapon, 1);
        CommonUI.Instance.DisplayResult($"{r.resultWeapon.weaponName} 제작 완료!");
        UIManager.Instance.morningUI.UpdateBlacksmithUI();
    }
}
