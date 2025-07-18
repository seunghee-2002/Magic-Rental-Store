using UnityEngine;

using MagicRentalShop.Utils;

namespace MagicRentalShop.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Game/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("기본 게임 설정")]
        [Tooltip("게임 시작 시 초기 자본금")]
        public int startingGold = 5000;

        [Tooltip("아침/낮 페이즈 지속 시간(초)")]
        public float dayDuration = 300f;

        [Tooltip("대장간 해금 일자")]
        public int blacksmithUnlockDay = 3;

        [Header("상점 시스템")]
        [Tooltip("무기 상점 기본 진열 개수")]
        public int weaponShopItemCount = 5;

        [Tooltip("무기 상점 새로고침 비용")]
        public int weaponShopRefreshCost = 1000;

        [Header("고객 시스템")]
        [Tooltip("고객 최대 레벨")]
        public int customerMaxLevel = 100;

        [Header("던전 시스템")]
        [Tooltip("고객 등급별 던전 배정 확률")]
        [GradeLabels]
        public DungeonAssignmentRate[] dungeonAssignmentRates = new DungeonAssignmentRate[]
        {
            // Common 고객
            new DungeonAssignmentRate { rates = new float[]{0.70f, 0.20f, 0.08f, 0.02f, 0.00f} },
            // Uncommon 고객  
            new DungeonAssignmentRate { rates = new float[]{0.50f, 0.30f, 0.15f, 0.05f, 0.00f} },
            // Rare 고객
            new DungeonAssignmentRate { rates = new float[]{0.30f, 0.35f, 0.25f, 0.08f, 0.02f} },
            // Epic 고객
            new DungeonAssignmentRate { rates = new float[]{0.15f, 0.25f, 0.35f, 0.20f, 0.05f} },
            // Legendary 고객
            new DungeonAssignmentRate { rates = new float[]{0.05f, 0.15f, 0.30f, 0.35f, 0.15f} }
        };

        [Header("Hero 시스템 설정")]
        [Tooltip("Hero 부상 회복 기간 (일)")]
        public int heroInjuryDays = 10;

        [Tooltip("Hero 최대 보유 개수")]
        public int maxHeroCount = 50;

        [Tooltip("등급별 잠금 해제 일수")]
        [GradeLabels]
        public int[] heroUnlockDays = {10, 20, 30, 40, 50};

        [Tooltip("상점 Hero 등장 확률")]
        [Range(0f, 1f)]
        public float heroVisitChance = 0.1f;

        [Tooltip("Hero 레벨업 보너스")]
        public int heroLevelBonus = 5;

        [Header("Hero 보상 시스템")]
        [Tooltip("Hero 골드 보정 배율")]
        public float heroGoldMultiplier = 1.5f;

        [Tooltip("던전별 Hero 레벨업 테이블")]
        [GradeLabels]
        public HeroLevelUpTable[] HeroLevelUpTables = new HeroLevelUpTable[]
        {
            // Common 던전
            new HeroLevelUpTable { rates = new float[]{0.40f, 0.40f, 0.20f, 0.00f, 0.00f} },
            // Uncommon 던전
            new HeroLevelUpTable { rates = new float[]{0.30f, 0.40f, 0.25f, 0.05f, 0.00f} },
            // Rare 던전
            new HeroLevelUpTable { rates = new float[]{0.20f, 0.30f, 0.40f, 0.09f, 0.01f} },
            // Epic 던전
            new HeroLevelUpTable { rates = new float[]{0.00f, 0.20f, 0.40f, 0.25f, 0.15f} },
            // Legendary 던전
            new HeroLevelUpTable { rates = new float[]{0.00f, 0.10f, 0.30f, 0.30f, 0.30f} }
        };

        [Header("Hero 페널티 시스템")]
        [Tooltip("부상 시 골드 페널티 (일)")]
        public int heroGoldPenaltyPerDay = 50;
        [Tooltip("기본 부상 기간")]
        public int heroBaseInjuryDays = 5;
        [Tooltip("부상 기간 최소 일수")]
        public int heroMinInjuryDays = 1;
        [Tooltip("부상 기간 최대 일수")]
        public int heroMaxInjuryDays = 15;

        [Header("Hero 전환 시스템")]
        [Tooltip("Customer 등급별 Hero 전환 확률")]
        [GradeLabels]
        [Range(0f, 10f)]
        public float[] heroBaseConversionRates = {5f, 4f, 3f, 2f, 1f};

        [Header("등급 보정 확률")]
        [Tooltip("무기 등급 보정")]
        [GradeLabels]
        [Range(-50f, 200f)]
        public float[] weaponGradeBonuses = {-30f, 0f, 20f, 50f, 100f};

        [Tooltip("던전 등급 보정")]
        [GradeLabels]
        [Range(-50f, 200f)]
        public float[] dungeonGradeBonuses = {-20f, 0f, 20f, 50f, 100f};

        [Header("경제 시스템 설정")]
        [Tooltip("월세 납부 주기 (일)")]
        public int rentPaymentDay = 7;
        [Tooltip("일수당 월세 배율")]
        public int rentMultiplier = 100;
        [Tooltip("월세 납부 경고 표시 여부")]
        public bool showRentWarning = true;
        [Tooltip("무기 판매 비율")]
        public float weaponSellRate = 0.5f;

        [Header("UI 및 인벤토리 설정")]
        [Tooltip("인벤토리 최대 크기")]
        public int inventoryMaxSize = 100;
        [Tooltip("일일 방문 고객 수")]
        public int dailyCustomerCount = 5;
        [Tooltip("인벤토리 UI 페이지당 표시 개수")]
        public int uiPageSize = 8;

        [Header("경제 밸런스 설정")]
        [Tooltip("일일 이벤트 골드 보정")]
        public int[] dailyEventRewards = {500, -1000, 0, 0, 0};
        [Tooltip("일일 이벤트 발생 확률")]
        [GradeLabels]
        public float[] gradeSpawnRates = {0.75f, 0.14f, 0.07f, 0.03f, 0.01f};

        [Header("제작 시스템 설정")]
        [Tooltip("동시에 제작 가능한 최대 무기 개수")]
        public int maxSimultaneousCrafting = 5;
        [Tooltip("제작 속도 배율")]
        public float craftingSpeedMultiplier = 1.0f;
    }

    [System.Serializable]
    public class DungeonAssignmentRate
    {
        [Tooltip("던전 등급별 확률")]
        [GradeLabels]
        public float[] rates = new float[5];
    }

    [System.Serializable]
    public class HeroLevelUpTable
    {
        [Tooltip("레벨별 레벨업 확률 테이블")]
        [CustomLabels("+1,+2,+3,+4,+5")]
        public float[] rates = new float[5];
    }
}