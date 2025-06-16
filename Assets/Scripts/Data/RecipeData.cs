using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName="NewRecipe", menuName="Data/Recipe")]
public class RecipeData : ScriptableObject
{
    public WeaponData resultWeapon;   // 제작 결과 도구
    public string description; // 제작 설명
    public int cost;              // 구매 비용
    public int ForgeCost;              // 제작 비용
    public List<MaterialData> requiredMaterials; // 필요한 재료
    public Sprite icon;         // 아이콘
}
