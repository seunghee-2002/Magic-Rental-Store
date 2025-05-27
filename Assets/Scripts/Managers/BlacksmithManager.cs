using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

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

    public void UpdateUI()
    {
        Transform forgeParent = UIBinder.Instance.forgeButtonParent;
        
        // 이전 버튼 삭제
        foreach (Transform t in forgeParent) Destroy(t.gameObject);

        // 레시피마다 버튼 생성
        for (int i = 0; i < recipes.Count; i++)
        {
            int idx = i;
            RecipeData recipe = recipes[i];

            GameObject btn = Instantiate(UIBinder.Instance.forgeButtonPrefab, forgeParent);
            TextMeshProUGUI txt = btn.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = $"{recipe.resultWeapon.weaponName} ({recipe.cost}G)";

            btn.GetComponent<Button>()
               .onClick.AddListener(() => {
                    ForgeWeapon(idx);
               });
        }
    }
}
