using UnityEngine;
using System.Collections.Generic;

namespace MagicRentalShop.Data
{
    [CreateAssetMenu(fileName = "DailyEventData", menuName = "Data/DailyEventData")]
    public class DailyEventData : ScriptableObject
    {
        [Header("기본 정보")]
        [Tooltip("고유 식별자")]
        public string id;
        
        [Tooltip("이벤트 이름")]
        public string eventName;
        
        [Tooltip("이벤트 설명")]
        [TextArea(3, 5)]
        public string description;
        
        [Header("시각적 요소")]
        [Tooltip("이벤트 아이콘")]
        public Sprite icon;
        
        [Header("이벤트 효과")]
        [Tooltip("이벤트 타입")]
        public EventType eventType;
        
        [Tooltip("골드 변화량 (양수: 보너스, 음수: 페널티)")]
        public int goldModifier = 0;
        
        [Tooltip("상점 무기 개수 변화량")]
        public int shopItemModifier = 0;
        
        [Tooltip("무기 가격 할인율 (0.2 = 20% 할인)")]
        [Range(0f, 1f)]
        public float weaponDiscountRate = 0f;
        
        [Tooltip("제작 속도 배율 (2.0 = 2배 빠름)")]
        [Range(0.1f, 5f)]
        public float craftingSpeedMultiplier = 1f;
        
        [Tooltip("모험 성공률 보정 (10 = +10%)")]
        [Range(-50, 50)]
        public int successRateBonus = 0;
        
        [Header("발생 조건")]
        [Tooltip("이벤트 발생 확률 (0.1 = 10%)")]
        [Range(0f, 1f)]
        public float probability = 0.1f;
        
        [Tooltip("이벤트가 발생할 수 있는 최소 일수")]
        public int minDay = 1;
        
        [Tooltip("이벤트가 발생할 수 있는 최대 일수 (0 = 제한 없음)")]
        public int maxDay = 0;
        
        [Header("지속 시간")]
        [Tooltip("이벤트 지속 일수 (1 = 하루만, 0 = 영구)")]
        public int duration = 1;
        
        [Tooltip("같은 이벤트 재발생 금지 일수")]
        public int cooldownDays = 7;
    }
    
    /// <summary>
    /// 일일 이벤트 타입
    /// </summary>
    [System.Serializable]
    public enum EventType
    {
        Positive,    // 긍정적 이벤트 (보너스)
        Negative,    // 부정적 이벤트 (페널티)
        Neutral,     // 중립적 이벤트 (정보성)
        Special      // 특별 이벤트 (Hero 관련 등)
    }
}