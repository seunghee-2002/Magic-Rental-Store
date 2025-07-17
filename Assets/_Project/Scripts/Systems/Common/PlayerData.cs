using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MagicRentalShop.Data;
using MagicRentalShop.Core;

namespace MagicRentalShop.Systems
{
    /// <summary>
    /// 게임의 모든 동적 상태를 저장하는 메인 데이터 클래스
    /// JSON 직렬화 가능하며, 저장/로드 시 이 클래스 하나만으로 게임 전체 상태를 복원 가능
    /// </summary>
    [System.Serializable]
    public class PlayerData
    {
        [Header("기본 게임 정보")]
        public int gold = 5000;                     // 현재 보유 골드
        public int currentDay = 1;                  // 현재 게임 날짜
        public GamePhase currentPhase = GamePhase.Morning;  // 현재 페이즈
        public float phaseRemainingTime = 0f;       // 페이즈 남은 시간 (초)
        
        [Header("게임 진행 상황")]
        public List<string> unlockedRecipeIDs = new List<string>();     // 해금된 레시피 ID 목록
        public List<string> purchasedWeaponIDs = new List<string>();    // 오늘 구매한 무기 ID 목록 (새로고침용)
        public int lastShopRefreshDay = 0;          // 마지막 상점 새로고침 날짜
        
        [Header("인벤토리 시스템")]
        public List<WeaponInstance> ownedWeapons = new List<WeaponInstance>();      // 보유한 무기 목록
        public List<MaterialInstance> ownedMaterials = new List<MaterialInstance>();  // 보유한 재료 목록
        
        [Header("고객 시스템")]
        public List<CustomerInstance> visitingCustomers = new List<CustomerInstance>();  // 오늘 방문한 고객들
        
        [Header("Hero 시스템")]
        public Dictionary<string, HeroStatusData> heroStatusData = new Dictionary<string, HeroStatusData>();  // Hero 상태 데이터 (도감 포함)
        
        [Header("모험 시스템")]
        public List<AdventureInstance> ongoingAdventures = new List<AdventureInstance>();        // 진행 중인 모험
        public List<AdventureResultData> completedAdventures = new List<AdventureResultData>();  // 완료된 모험 결과
        
        [Header("제작 시스템")]
        public List<CraftingInstance> ongoingCrafting = new List<CraftingInstance>();    // 진행 중인 제작
        
        [Header("월세 시스템")]
        public int lastRentPaymentDay = 0;          // 마지막 월세 납부 날짜
        public bool hasShownRentWarning = false;    // 이번 주기 월세 경고 표시 여부
        
        [Header("일일 이벤트")]
        public int lastEventDay = 0;                // 마지막 이벤트 발생 날짜
        public string lastEventID = "";             // 마지막 이벤트 ID
        
        /// <summary>
        /// 기본 생성자 - 새 게임 시작 시 사용
        /// </summary>
        public PlayerData()
        {
            InitializeNewGame();
        }
        
        /// <summary>
        /// 새 게임 초기화
        /// </summary>
        public void InitializeNewGame()
        {
            gold = 5000;
            currentDay = 1;
            currentPhase = GamePhase.Morning;
            phaseRemainingTime = 0f;
            
            // Hero 도감 초기화 (모든 Customer를 미획득 상태로)
            InitializeHeroCollection();
            
            Debug.Log("New game initialized with starting resources");
        }
        
        /// <summary>
        /// Hero 도감 초기화
        /// </summary>
        private void InitializeHeroCollection()
        {
            heroStatusData.Clear();
            
            // 모든 CustomerData를 Hero 도감에 미획득 상태로 등록
            // 실제 구현에서는 DataManager에서 모든 CustomerData를 가져와야 함
            var allCustomers = DataManager.Instance?.GetAllCustomerData();
            if (allCustomers != null)
            {
                foreach (var customer in allCustomers)
                {
                    heroStatusData[customer.id] = new HeroStatusData
                    {
                        isHero = false,
                        heroConvertedDay = 0,
                        currentInjury = InjuryType.None,
                        injuryStartDay = 0,
                        returnDay = 0,
                        shouldInherit = false
                    };
                }
            }
        }
        
        /// <summary>
        /// 월세 납부 처리
        /// </summary>
        public bool PayRent(int rentAmount)
        {
            if (gold >= rentAmount)
            {
                gold -= rentAmount;
                lastRentPaymentDay = currentDay;
                hasShownRentWarning = false;
                Debug.Log($"Rent paid: {rentAmount} gold. Remaining gold: {gold}");
                return true;
            }
            
            Debug.LogWarning($"Cannot pay rent: {rentAmount} gold. Current gold: {gold}");
            return false;
        }
        
        /// <summary>
        /// 무기 판매 처리
        /// </summary>
        public bool SellWeapon(WeaponInstance weapon, int sellPrice)
        {
            if (ownedWeapons.Remove(weapon))
            {
                gold += sellPrice;
                Debug.Log($"Sold weapon {weapon.instanceID} for {sellPrice} gold");
                return true;
            }
            
            Debug.LogWarning($"Cannot sell weapon: not found in inventory");
            return false;
        }
        
        /// <summary>
        /// Hero 획득 처리
        /// </summary>
        public void AcquireHero(string customerID, int currentDay)
        {
            if (heroStatusData.ContainsKey(customerID))
            {
                heroStatusData[customerID].isHero = true;
                heroStatusData[customerID].heroConvertedDay = currentDay;
                heroStatusData[customerID].shouldInherit = true;
                
                Debug.Log($"Hero acquired: {customerID} on day {currentDay}");
            }
            else
            {
                // 새로운 Hero 상태 데이터 생성
                heroStatusData[customerID] = new HeroStatusData
                {
                    isHero = true,
                    heroConvertedDay = currentDay,
                    currentInjury = InjuryType.None,
                    injuryStartDay = 0,
                    returnDay = 0,
                    shouldInherit = true
                };
                
                Debug.Log($"New Hero acquired: {customerID} on day {currentDay}");
            }
        }
        
        /// <summary>
        /// Hero 부상 처리
        /// </summary>
        public void InjureHero(string customerID, InjuryType injury, int currentDay)
        {
            if (heroStatusData.ContainsKey(customerID) && heroStatusData[customerID].isHero)
            {
                var heroStatus = heroStatusData[customerID];
                heroStatus.currentInjury = injury;
                heroStatus.injuryStartDay = currentDay;
                heroStatus.returnDay = currentDay + GetInjuryDuration(injury);
                
                Debug.Log($"Hero injured: {customerID} with {injury} until day {heroStatus.returnDay}");
            }
        }
        
        /// <summary>
        /// Hero 부상 회복 처리
        /// </summary>
        public void RecoverHero(string customerID, int currentDay)
        {
            if (heroStatusData.ContainsKey(customerID) && heroStatusData[customerID].isHero)
            {
                var heroStatus = heroStatusData[customerID];
                if (currentDay >= heroStatus.returnDay)
                {
                    heroStatus.currentInjury = InjuryType.None;
                    heroStatus.injuryStartDay = 0;
                    heroStatus.returnDay = 0;
                    
                    Debug.Log($"Hero recovered: {customerID} on day {currentDay}");
                }
            }
        }
        
        /// <summary>
        /// Hero 방문 가능 여부 확인
        /// </summary>
        public bool IsHeroAvailable(string customerID, int currentDay)
        {
            if (!heroStatusData.ContainsKey(customerID)) return false;
            
            var heroStatus = heroStatusData[customerID];
            return heroStatus.isHero && 
                   (heroStatus.currentInjury == InjuryType.None || currentDay >= heroStatus.returnDay);
        }
        
        /// <summary>
        /// 부상 기간 계산
        /// </summary>
        private int GetInjuryDuration(InjuryType injury)
        {
            return injury switch
            {
                InjuryType.Minor => 3,    // 경상: 3일
                InjuryType.Moderate => 7, // 중상: 7일  
                InjuryType.Severe => 14,  // 중증: 14일
                _ => 0
            };
        }
        
        /// <summary>
        /// 데이터 유효성 검사
        /// </summary>
        public bool IsValid()
        {
            if (gold < 0)
            {
                Debug.LogError("Invalid PlayerData: Negative gold");
                return false;
            }
            
            if (currentDay < 1)
            {
                Debug.LogError("Invalid PlayerData: Invalid current day");
                return false;
            }
            
            // 인벤토리 검증
            if (ownedWeapons == null || ownedMaterials == null)
            {
                Debug.LogError("Invalid PlayerData: Null inventory lists");
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// 디버그 정보 출력
        /// </summary>
        public void DebugInfo()
        {
            Debug.Log($"=== PlayerData Debug Info ===");
            Debug.Log($"Gold: {gold}, Day: {currentDay}, Phase: {currentPhase}");
            Debug.Log($"Weapons: {ownedWeapons.Count}, Materials: {ownedMaterials.Count}");
            Debug.Log($"Heroes: {heroStatusData.Count(kvp => kvp.Value.isHero)}, Injured Heroes: {heroStatusData.Count(kvp => kvp.Value.currentInjury != InjuryType.None)}");
            Debug.Log($"Ongoing Adventures: {ongoingAdventures.Count}");
            Debug.Log($"Ongoing Crafting: {ongoingCrafting.Count}");
            Debug.Log($"Last Rent Payment: Day {lastRentPaymentDay}");
        }
    }
}