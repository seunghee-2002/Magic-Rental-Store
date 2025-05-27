using UnityEngine;
using System.Collections.Generic;

public class BlacksmithManager : MonoBehaviour
{
    public static BlacksmithManager Instance;

    [Header("Recipes")]
    public List<RecipeData> recipes;  // Inspector 에서 드래그
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

        // 비용 차감
        if (!GameManager.Instance.SpendGold(r.cost))
            return;

        // 제작
        InventoryManager.Instance.AddWeapon(r.resultWeapon, 1);
        CommonUI.Instance.DisplayResult($"{r.resultWeapon.weaponName} 제작 완료!");

        // 결과 탭 갱신(원한다면)
        UIManager.Instance.morningUI.UpdateBlacksmithUI();
    }
}
