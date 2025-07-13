using UnityEngine;

namespace MagicRentalShop.Data
{

    [CreateAssetMenu(fileName = "CustomerData", menuName = "Data/CustomerData")]
    public class CustomerData : ScriptableObject
    {
        [Header("기본 정보")]
        [Tooltip("고유 식별자")]
        public string id;
        
        [Tooltip("고객/Hero 이름")]
        public string customerName;
        
        [Tooltip("고객/Hero 설명")]
        [TextArea(3, 5)]
        public string description;
        
        [Header("속성")]
        [Tooltip("고객/Hero 등급")]
        public Grade grade;
        
        [Tooltip("고객/Hero 속성")]
        public Element element;
        
        [Header("시각적 요소")]
        [Tooltip("고객 상태일 때의 아이콘")]
        public Sprite customerIcon;
        
        [Tooltip("Hero 상태일 때의 아이콘 (비어있으면 customerIcon 사용)")]
        public Sprite heroIcon;

        [Header("Hero 전환 관련")]
        [Tooltip("Hero 전환 시 표시될 이름 (비어있으면 customerName 사용)")]
        public string heroName;
        
        [Tooltip("Hero 전환 시 표시될 설명 (비어있으면 description 사용)")]
        [TextArea(3, 5)]
        public string heroDescription;
        
        [Tooltip("Hero 전용 특수 능력 설명 (TODO: 추후 구현)")]
        [TextArea(2, 3)]
        public string specialAbility = "TODO: Hero 개별 특수능력";
    }
}