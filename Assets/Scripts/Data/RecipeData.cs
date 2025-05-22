using UnityEngine;

[CreateAssetMenu(fileName="NewRecipe", menuName="Data/Recipe")]
public class RecipeData : ScriptableObject
{
    public WeaponData resultWeapon;   // 제작 결과 도구
    public string description; // 제작 설명
    public int cost;              // 제작 비용 (Gold)
    public Sprite icon;         // 아이콘
}
