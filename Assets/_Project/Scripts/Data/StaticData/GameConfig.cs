using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("기본 게임 설정")]
    public int startingGold = 5000;              // 게임 시작 시 초기 자본금
    public float dayDuration = 300f;             // 아침/낮 페이즈 지속 시간(초)
    public int blacksmithUnlockDay = 3;          // 대장간 해금 일자

    [Header("상점 시스템")]
    public int weaponShopItemCount = 5;          // 무기 상점 기본 진열 개수
    public int weaponShopRefreshCost = 1000;     // 상점 새로고침 비용

    [Header("고객 시스템")]
    public int customerMaxLevel = 100;          // 고객 최대 레벨

    [Header("Hero 시스템 설정")]
    public int heroInjuryDays = 10;              // Hero 부상 회복 기간
    public int heroMaxLevel = 100;               // Hero 최대 레벨
    public int maxHeroCount = 50;                // 최대 보유 가능 Hero 수
    public int[] heroUnlockDays = {10, 20, 30, 40, 50}; // 등급별 잠금해제 일수 (Common~Legendary)

    [Header("Hero 보상 시스템")]
    public float heroGoldMultiplier = 1.5f;      // Hero 골드 보정 배율
    public int[][] heroLevelUpTables = {         // 던전별 레벨업 확률 테이블
        new int[]{40, 40, 20, 0, 0},    // Common
        new int[]{30, 40, 25, 5, 0},    // Uncommon  
        new int[]{20, 30, 40, 9, 1},    // Rare
        new int[]{0, 20, 40, 25, 15},   // Epic
        new int[]{0, 10, 30, 30, 30}    // Legendary
    };

    [Header("Hero 페널티 시스템")]
    public int heroGoldPenaltyPerDay = 50;       // 일수당 골드 페널티
    public int heroBaseInjuryDays = 5;           // 기본 부상 기간
    public int heroMinInjuryDays = 1;            // 최소 부상 기간  
    public int heroMaxInjuryDays = 15;           // 최대 부상 기간

    [Header("Hero 전환 시스템")]
    [Range(0f, 10f)]
    public float[] heroBaseConversionRates = {5f, 4f, 3f, 2f, 1f}; // 등급별 기본 확률(%)

    [Header("등급 보정 확률")]
    [Range(-50f, 200f)]
    public float[] weaponGradeBonuses = {-30f, 0f, 20f, 50f, 100f}; // 무기 등급 보정(%)

    [Range(-50f, 200f)]
    public float[] dungeonGradeBonuses = {-20f, 0f, 20f, 50f, 100f}; // 던전 등급 보정(%)

    [Header("경제 시스템 설정")]
    public int rentPaymentDay = 7;               // 월세 납부 주기 (일)
    public int rentMultiplier = 100;             // 일수당 월세 배율
    public bool showRentWarning = true;          // 월세 납부일 임박 경고 표시
    public float weaponSellRate = 0.5f;          // 무기 판매 비율 (구매가의 50%)

    [Header("UI 및 인벤토리 설정")]
    public int inventoryMaxSize = 100;           // 인벤토리 최대 크기
    public int dailyCustomerCount = 5;           // 일일 방문 고객 수
    public int uiPageSize = 8;                   // 리스트 한 페이지당 표시 개수

    [Header("경제 밸런스 설정")]
    public int[] dailyEventRewards = {500, -1000, 0, 0, 0}; // 일일 이벤트 골드 보정
    public float[] gradeSpawnRates = {0.75f, 0.14f, 0.07f, 0.03f, 0.01f}; // 등급별 생성 확률

    [Header("제작 시스템 설정")]
    public int maxSimultaneousCrafting = 5;      // 동시 제작 가능 개수
    public float craftingSpeedMultiplier = 1.0f; // 제작 속도 배율
}