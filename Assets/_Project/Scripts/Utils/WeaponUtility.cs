using UnityEngine;

namespace MagicRentalShop.Data
{
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
}