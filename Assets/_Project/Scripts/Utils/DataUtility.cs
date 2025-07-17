using UnityEngine;
using System.Collections.Generic;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 무기 데이터 관련 유틸리티 클래스
    /// </summary>
    public static class WeaponUtility
    {        
        /// <summary>
        /// 전설 등급 무기인지 확인 & 2번째 속성을 보유하고 있는지 확인
        /// </summary>
        public static bool IsLegendary(WeaponData weapon)
        {
            return weapon.grade == Grade.Legendary && weapon.secondaryElement != Element.None;
        }
                
        /// <summary>
        /// UI 표시용 속성 텍스트 생성
        /// </summary>
        public static string GetElementText(WeaponData weapon)
        {
            if (IsLegendary(weapon))
            {
                return $"{weapon.element} + {weapon.secondaryElement}";
            }
            else
            {
                return weapon.element.ToString();
            }
        }
        
        /// <summary>
        /// 주 속성 반환
        /// </summary>
        public static Element GetElement(WeaponData weapon)
        {
            return weapon.element;
        }
        
        /// <summary>
        /// 보조 속성 반환 (전설급이고 보조 속성이 있는 경우에만)
        /// </summary>
        public static Element GetSecondaryElement(WeaponData weapon)
        {
            return IsLegendary(weapon) ? weapon.secondaryElement : Element.None;
        }
    }

    // <summary>
    /// 레시피 데이터 관련 유틸리티 클래스
    /// </summary>
    public static class RecipeUtility
    {
        /// <summary>
        /// 제작 가능 여부 확인 (골드 + 재료)
        /// </summary>
        public static bool CanCraft(RecipeData recipe, Dictionary<string, int> playerMaterials, int playerGold)
        {
            // 골드 확인
            if (playerGold < recipe.craftingCost) return false;
            
            // 재료 확인
            foreach (var material in recipe.requiredMaterials)
            {
                if (!playerMaterials.ContainsKey(material.materialID) || 
                    playerMaterials[material.materialID] < material.quantity)
                {
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// 부족한 재료 목록 (UI 표시용)
        /// </summary>
        /// <param name="recipe">확인할 레시피</param>
        /// <param name="playerMaterials">플레이어 보유 재료 딕셔너리</param>
        /// <returns>부족한 재료 문자열 리스트 (예: "철광석 (2/5)", "용의비늘 (0/1)")</returns>
        public static List<string> GetMissingMaterialNames(RecipeData recipe, Dictionary<string, int> playerMaterials)
        {
            var missing = new List<string>();
            foreach (var material in recipe.requiredMaterials)
            {
                int have = playerMaterials.ContainsKey(material.materialID) ? playerMaterials[material.materialID] : 0;
                if (have < material.quantity)
                {
                    missing.Add($"{material.materialID} ({have}/{material.quantity})");
                }
            }
            return missing;
        }
    }
}