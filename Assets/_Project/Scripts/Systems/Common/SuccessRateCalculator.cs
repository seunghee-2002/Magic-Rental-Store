using UnityEngine;
using MagicRentalShop.Core;
using MagicRentalShop.Data;

namespace MagicRentalShop.Systems
{
    /// <summary>
    /// 모험 성공률 계산 시스템
    /// AdventureController에서만 사용하는 인스턴스 기반 클래스
    /// </summary>
    public class SuccessRateCalculator
    {
        private DataManager dataManager;
        private GameConfig gameConfig;
        
        public SuccessRateCalculator()
        {
            dataManager = DataManager.Instance;
            gameConfig = dataManager?.GetGameConfig();
        }
        
        /// <summary>
        /// 최종 성공률 계산 (Customer/Hero 구분 적용)
        /// </summary>
        /// <param name="adventurerLevel">모험가 레벨</param>
        /// <param name="adventurerElement">모험가 속성</param>
        /// <param name="adventurerGrade">모험가 등급</param>
        /// <param name="weaponElement">무기 속성</param>
        /// <param name="weaponGrade">무기 등급</param>
        /// <param name="dungeonLevel">던전 레벨</param>
        /// <param name="dungeonElement">던전 속성</param>
        /// <param name="dungeonGrade">던전 등급</param>
        /// <param name="isHero">Hero 여부</param>
        /// <returns>최종 성공률 (1~100%)</returns>
        public float CalculateFinalRate(
            int adventurerLevel, Element adventurerElement, Grade adventurerGrade,
            Element weaponElement, Grade weaponGrade,
            int dungeonLevel, Element dungeonElement, Grade dungeonGrade,
            bool isHero = false)
        {
            // 1. 기본 성공률 계산
            float baseRate = CalculateBaseRate(adventurerLevel, dungeonLevel);
            
            // 2. 속성 상성 보정 계산
            float elementBonus = CalculateElementBonus(adventurerElement, weaponElement, dungeonElement);
            
            // 3. 등급 보정 계산
            float gradeBonus = CalculateGradeBonus(adventurerGrade, weaponGrade, dungeonGrade);

            // 4. 특수 보정 계산
            float specialBonus = CalculateSpecialBonus(isHero);
            
            // 5. 최종 성공률 = 레벨보정 * 속성보정 * 등급보정 + 특수보정
            float finalRate = baseRate * elementBonus * gradeBonus + specialBonus;
            
            // 6. 1~100% 범위로 제한
            return Mathf.Clamp(finalRate, 1f, 100f);
        }
        
        /// <summary>
        /// 기본 성공률: (모험가레벨 / (모험가레벨 + 던전레벨)) × 100
        /// </summary>
        private float CalculateBaseRate(int adventurerLevel, int dungeonLevel)
        {
            if (adventurerLevel <= 0) adventurerLevel = 1;
            if (dungeonLevel <= 0) dungeonLevel = 1;
            
            return (float)adventurerLevel / (adventurerLevel + dungeonLevel) * 100f;
        }
        
        /// <summary>
        /// 속성 상성 보정 계산
        /// </summary>
        private float CalculateElementBonus(Element adventurerElement, Element weaponElement, Element dungeonElement)
        {           
            // 모험가 vs 던전 속성 상성
            float adventurerBonus = GetElementAdvantage(adventurerElement, dungeonElement);
            
            // 무기 vs 던전 속성 상성
            float weaponBonus = GetElementAdvantage(weaponElement, dungeonElement);
            
            return (weaponBonus + adventurerBonus) / 2f;
        }
        
        /// <summary>
        /// 등급 보정 계산: 무기보정(무기등급 - 던전등급) + 던전보정(모험가등급 - 던전등급)
        /// </summary>
        private float CalculateGradeBonus(Grade adventurerGrade, Grade weaponGrade, Grade dungeonGrade)
        {
            // 모험가 보정: 모험가등급 - 던전등급
            float adventurerBonus = GetAdventurerBonus()[(int)adventurerGrade - (int)dungeonGrade + 4];

            // 무기 보정: 무기등급 - 던전등급
            float weaponBonus = GetWeaponBonus()[(int)weaponGrade - (int)dungeonGrade + 4];
            
            return (weaponBonus + adventurerBonus) / 2f;
        }
        
        /// <summary>
        /// 속성 간 상성 우위 계산
        /// </summary>
        private float GetElementAdvantage(Element attacker, Element defender)
        {
            AdvantageStatus advantage = AdvantageStatus.None;

            // None 속성은 모든 속성에 1배
            if (attacker == Element.None || defender == Element.None)
                return 1f;

            // 같은 속성은 약한 상성
            if (attacker == defender)
                return 0.8f;

            // Light/Dark 속성은 특별한 상성
            if (attacker == Element.Light || attacker == Element.Dark)
                return 1.3f;
            
            // 순환 상성: Fire→Ice, Water→Fire, Thunder→Water, Earth→Thunder, Air→Earth, Ice→Air
            advantage = (attacker, defender) switch
            {
                (Element.Fire, Element.Ice) => AdvantageStatus.Strong,
                (Element.Water, Element.Fire) => AdvantageStatus.Strong,
                (Element.Thunder, Element.Water) => AdvantageStatus.Strong,
                (Element.Earth, Element.Thunder) => AdvantageStatus.Strong,
                (Element.Air, Element.Earth) => AdvantageStatus.Strong,
                (Element.Ice, Element.Air) => AdvantageStatus.Strong,

                (Element.Ice, Element.Fire) => AdvantageStatus.Weak,
                (Element.Fire, Element.Water) => AdvantageStatus.Weak,
                (Element.Water, Element.Thunder) => AdvantageStatus.Weak,
                (Element.Thunder, Element.Earth) => AdvantageStatus.Weak,
                (Element.Earth, Element.Air) => AdvantageStatus.Weak,
                (Element.Air, Element.Ice) => AdvantageStatus.Weak,

                _ => AdvantageStatus.Neutral
            };

            

            // 상성에 따른 보정 배율 반환
            switch (advantage)
            {
                case AdvantageStatus.Strong:
                    return 1.5f; // 강한 상성
                case AdvantageStatus.Weak:
                    return 0.5f; // 약한 상성
                case AdvantageStatus.Neutral:
                    return 1f; // 중립 상성
                default:
                    return 1f; // 기본
            }
        }
        
        private float CalculateSpecialBonus(bool isHero)
        {
            float bonus = 0f;

            // Hero인 경우 추가 보정
            if (isHero)
            {
                bonus += 10f;
            }

            return bonus;
        }
        
        /// <summary>
        /// 모험가 등급차 보정 배율
        /// </summary>
        private float[] GetAdventurerBonus()
        {
            return gameConfig?.adventurerGradeBonuses ?? new[] {0.25f, 0.5f, 0.75f, 0.9f, 1f, 1.2f, 1.5f, 2f, 3f};
        }
        
        /// <summary>
        /// 무기 등급차 보정 배율
        /// </summary>
        private float[] GetWeaponBonus()
        {
            // GameConfig에서 설정값 가져오기
            return gameConfig?.weaponGradeBonuses ?? new[] {0.4f, 0.65f, 0.8f, 0.9f, 1f, 1.1f, 1.4f, 1.7f, 2f};
        }

        #region 디버그 및 테스트 메서드
        
        /// <summary>
        /// 성공률 계산 과정 세부 정보 반환 (디버그용)
        /// </summary>
        public SuccessRateBreakdown GetDetailedBreakdown(
            int adventurerLevel, Element adventurerElement, Grade adventurerGrade,
            Element weaponElement, Grade weaponGrade,
            int dungeonLevel, Element dungeonElement, Grade dungeonGrade,
            bool isHero = false)
        {
            var breakdown = new SuccessRateBreakdown
            {
                baseRate = CalculateBaseRate(adventurerLevel, dungeonLevel),
                elementBonus = CalculateElementBonus(adventurerElement, weaponElement, dungeonElement),
                gradeBonus = CalculateGradeBonus(adventurerGrade, weaponGrade, dungeonGrade),
                specialBonus = CalculateSpecialBonus(isHero),
                isHero = isHero
            };
            
            breakdown.finalRate = Mathf.Clamp(
                breakdown.baseRate * breakdown.elementBonus * breakdown.gradeBonus + breakdown.specialBonus, 
                1f, 100f);
            
            return breakdown;
        }
        
        #endregion
    }

    public enum AdvantageStatus
    {
        None,       // 상성 없음
        Weak,       // 약한 상성
        Strong,     // 강한 상성
        Neutral     // 중립
    }

    
    /// <summary>
    /// 성공률 계산 세부 정보 (디버그용)
    /// </summary>
    [System.Serializable]
    public class SuccessRateBreakdown
    {
        public float baseRate;      // 기본 성공률
        public float elementBonus;  // 속성 보정
        public float gradeBonus;    // 등급 보정
        public float specialBonus;  // 특수 보정
        public float finalRate;     // 최종 성공률
        public bool isHero;         // Hero 여부

        public override string ToString()
        {
            string heroText = isHero ? " (Hero)" : " (Customer)";
            return $"성공률{heroText}: {finalRate:F1}% = 기본{baseRate:F1}% * 속성{elementBonus:+0.0;-0.0;0.0}배 * 등급{gradeBonus:+0.0;-0.0;0.0}배 + 특수{specialBonus:+0.0;-0.0;0.0%}";
        }
    }
}