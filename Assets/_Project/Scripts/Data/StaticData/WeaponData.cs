using UnityEngine;

namespace MagicRentalShop.Data
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Data/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [Header("기본 정보")]
        [Tooltip("고유 식별자")]
        public string id;

        [Tooltip("무기 이름")]
        public string weaponName;

        [TextArea(3, 5)]
        [Tooltip("무기 설명")]
        public string description;

        [Header("속성")]
        [Tooltip("무기 등급")]
        public Grade grade;

        [Tooltip("무기 속성")]
        public Element element;
        [Header("2번째 등급(전설 등급 무기만 사용)")]
        public Element secondaryElement = Element.None;

        [Header("시각적 요소")]
        [Tooltip("무기 아이콘")]
        public Sprite icon;

        [Header("경제")]
        public int basePrice;                // 기본 가격 
    }
}