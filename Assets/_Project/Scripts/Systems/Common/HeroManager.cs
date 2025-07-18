using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using MagicRentalShop.Data;
using MagicRentalShop.Core;

namespace MagicRentalShop.Systems
{
    /// <summary>
    /// Hero 도감, 고용/해제, 부상 관리를 담당하는 통합 시스템
    /// CustomerManager와 연동하여 Hero 등장 풀을 관리
    /// </summary>
    public class HeroManager : MonoBehaviour
    {
        public static HeroManager Instance { get; private set; }

        // 의존성
        private DataManager dataManager;
        private GameConfig gameConfig;
        private PlayerData playerData;

        // Hero 도감 데이터 (CustomerData ID → HeroCollectionData)
        private Dictionary<string, HeroCollectionData> heroCollection = new Dictionary<string, HeroCollectionData>();
        
        // 현재 고용 중인 Hero들 (CustomerData ID → 고용 상태)
        private HashSet<string> employedHeroIds = new HashSet<string>();
        
        // 부상 중인 Hero들 (인스턴스 ID → 부상 정보)
        private Dictionary<string, InjuryData> injuredHeroes = new Dictionary<string, InjuryData>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            InitializeDependencies();
        }

        #region 초기화

        /// <summary>
        /// 의존성 초기화
        /// </summary>
        private void InitializeDependencies()
        {
            dataManager = DataManager.Instance;
            
            if (dataManager == null)
            {
                Debug.LogError("DataManager not found!");
                return;
            }
            
            // PlayerData 참조 설정
            playerData = GameController.Instance?.PlayerData;
            
            // GameConfig 로드
            gameConfig = dataManager.GetGameConfig();
            
            if (gameConfig == null)
            {
                Debug.LogError("GameConfig not found! Hero 시스템이 제대로 작동하지 않을 수 있습니다.");
            }
        }

        /// <summary>
        /// Hero 도감 초기화 (새 게임 시)
        /// </summary>
        public void InitializeHeroCollection()
        {
            heroCollection.Clear();
            employedHeroIds.Clear();
            injuredHeroes.Clear();

            var allCustomers = dataManager.GetAllCustomerData();
            foreach (var customer in allCustomers)
            {
                heroCollection[customer.id] = new HeroCollectionData
                {
                    isAcquired = false,
                    acquiredDay = 0,
                    shouldInherit = false
                };
            }
            
            Debug.Log($"Hero collection initialized with {heroCollection.Count} entries");
        }

        #endregion

        #region Hero 도감 관리

        /// <summary>
        /// Hero 획득 처리 (Customer → Hero 전환)
        /// </summary>
        public void AcquireHero(string customerID)
        {
            if (!heroCollection.ContainsKey(customerID))
            {
                Debug.LogError($"Hero collection에 없는 ID: {customerID}");
                return;
            }

            if (heroCollection[customerID].isAcquired)
            {
                Debug.LogWarning($"이미 획득한 Hero: {customerID}");
                return;
            }

            // 도감에 등록
            heroCollection[customerID].isAcquired = true;
            heroCollection[customerID].acquiredDay = playerData.currentDay;
            heroCollection[customerID].shouldInherit = true;

            Debug.Log($"Hero acquired: {customerID} on day {playerData.currentDay}");
        }

        /// <summary>
        /// Hero 획득 여부 확인
        /// </summary>
        public bool IsHeroAcquired(string customerID)
        {
            return heroCollection.ContainsKey(customerID) && heroCollection[customerID].isAcquired;
        }

        /// <summary>
        /// 획득한 모든 Hero 목록 반환
        /// </summary>
        public List<CustomerData> GetAllAcquiredHeroes()
        {
            var acquiredHeroes = new List<CustomerData>();
            
            foreach (var kvp in heroCollection)
            {
                if (kvp.Value.isAcquired)
                {
                    var heroData = dataManager.GetCustomerData(kvp.Key);
                    if (heroData != null)
                    {
                        acquiredHeroes.Add(heroData);
                    }
                }
            }
            
            return acquiredHeroes;
        }

        /// <summary>
        /// 도감 데이터 반환 (UI용)
        /// </summary>
        public Dictionary<string, HeroCollectionData> GetHeroCollection()
        {
            return new Dictionary<string, HeroCollectionData>(heroCollection);
        }

        #endregion

        #region Hero 고용/해제 시스템

        /// <summary>
        /// Hero 고용 (도감 → 활성 Hero 풀)
        /// </summary>
        public bool EmployHero(string customerID)
        {
            // 도감에 등록되어 있는지 확인
            if (!IsHeroAcquired(customerID))
            {
                Debug.LogWarning($"Cannot employ Hero: {customerID} not acquired");
                return false;
            }

            // 이미 고용 중인지 확인
            if (employedHeroIds.Contains(customerID))
            {
                Debug.LogWarning($"Hero {customerID} is already employed");
                return false;
            }

            // 최대 고용 Hero 수 확인
            int maxHeroCount = gameConfig?.maxHeroCount ?? 10;
            if (employedHeroIds.Count >= maxHeroCount)
            {
                Debug.LogWarning($"Cannot employ more Heroes. Max count: {maxHeroCount}");
                return false;
            }

            // Hero 고용 처리
            employedHeroIds.Add(customerID);
            
            // CustomerManager에 고용 상태 업데이트 알림
            if (CustomerManager.Instance != null)
            {
                CustomerManager.Instance.OnHeroEmployed(customerID);
            }
            
            Debug.Log($"Hero {customerID} employed. Current employed Heroes: {employedHeroIds.Count}/{maxHeroCount}");
            return true;
        }

        /// <summary>
        /// Hero 임시해제 (활성 Hero 풀 → Customer 풀)
        /// </summary>
        public bool ReleaseHero(string customerID)
        {
            if (!employedHeroIds.Contains(customerID))
            {
                Debug.LogWarning($"Cannot release Hero: {customerID} not employed");
                return false;
            }
            
            // Hero 해제 처리
            employedHeroIds.Remove(customerID);
            
            // CustomerManager에 해제 상태 업데이트 알림
            if (CustomerManager.Instance != null)
            {
                CustomerManager.Instance.OnHeroReleased(customerID);
            }
            
            Debug.Log($"Hero {customerID} released. Current employed Heroes: {employedHeroIds.Count}");
            return true;
        }

        /// <summary>
        /// 고용 중인 모든 Hero 목록 반환
        /// </summary>
        public List<string> GetEmployedHeroIds()
        {
            return new List<string>(employedHeroIds);
        }

        /// <summary>
        /// 고용 가능한 Hero 목록 반환 (도감 보유 - 현재 고용)
        /// </summary>
        public List<CustomerData> GetAvailableToEmployHeroes()
        {
            var availableHeroes = new List<CustomerData>();
            var allAcquiredHeroes = GetAllAcquiredHeroes();
            
            foreach (var heroData in allAcquiredHeroes)
            {
                if (!employedHeroIds.Contains(heroData.id))
                {
                    availableHeroes.Add(heroData);
                }
            }
            
            return availableHeroes;
        }

        /// <summary>
        /// 고용 상태 확인
        /// </summary>
        public bool IsHeroEmployed(string customerID)
        {
            return employedHeroIds.Contains(customerID);
        }

        /// <summary>
        /// 현재 고용 가능한 Hero 슬롯 수
        /// </summary>
        public int GetAvailableHeroSlots()
        {
            int maxHeroCount = gameConfig?.maxHeroCount ?? 10;
            return Mathf.Max(0, maxHeroCount - employedHeroIds.Count);
        }

        /// <summary>
        /// 최대 고용 가능한 Hero 수
        /// </summary>
        public int GetMaxHeroCount()
        {
            return gameConfig?.maxHeroCount ?? 10;
        }

        #endregion

        #region Hero 부상 관리 시스템

        /// <summary>
        /// Hero 부상 처리 (모험 실패 시)
        /// </summary>
        public void InjureHero(string heroInstanceID, InjuryType injury)
        {
            // 기존 부상 제거 (새 부상으로 덮어쓰기)
            if (injuredHeroes.ContainsKey(heroInstanceID))
            {
                injuredHeroes.Remove(heroInstanceID);
                Debug.Log($"Previous injury for {heroInstanceID} removed");
            }

            // 새 부상 추가
            var injuryData = new InjuryData
            {
                heroInstanceID = heroInstanceID,
                injuryType = injury,
                injuryStartDay = playerData.currentDay,
                returnDay = playerData.currentDay + GetInjuryDuration(injury)
            };

            injuredHeroes[heroInstanceID] = injuryData;
            Debug.Log($"Hero {heroInstanceID} injured ({injury}) until day {injuryData.returnDay}");
        }

        /// <summary>
        /// Hero 가용성 확인 (부상 여부 체크)
        /// </summary>
        public bool IsHeroAvailable(string heroInstanceID)
        {
            if (!injuredHeroes.ContainsKey(heroInstanceID))
            {
                return true; // 부상 기록이 없으면 사용 가능
            }
            
            var injury = injuredHeroes[heroInstanceID];
            return playerData.currentDay >= injury.returnDay;
        }

        /// <summary>
        /// 부상 종료일 반환
        /// </summary>
        public int GetHeroRecoveryDay(string heroInstanceID)
        {
            if (injuredHeroes.ContainsKey(heroInstanceID))
            {
                return injuredHeroes[heroInstanceID].returnDay;
            }
            return -1; // 부상 없음
        }

        /// <summary>
        /// 매일 부상 회복 처리
        /// </summary>
        public void ProcessDailyRecovery()
        {
            var recoveredHeroes = new List<string>();

            foreach (var kvp in injuredHeroes)
            {
                if (playerData.currentDay >= kvp.Value.returnDay)
                {
                    recoveredHeroes.Add(kvp.Key);
                }
            }

            foreach (var recoveredHeroId in recoveredHeroes)
            {
                injuredHeroes.Remove(recoveredHeroId);
                Debug.Log($"Hero {recoveredHeroId} recovered from injury");
            }

            if (recoveredHeroes.Count > 0)
            {
                Debug.Log($"{recoveredHeroes.Count} heroes recovered today");
            }
        }

        /// <summary>
        /// 부상 기간 계산
        /// </summary>
        private int GetInjuryDuration(InjuryType injury)
        {
            return injury switch
            {
                InjuryType.Minor => 3,
                InjuryType.Moderate => 7,
                InjuryType.Severe => 14,
                _ => 0
            };
        }

        /// <summary>
        /// 부상 중인 Hero 목록 반환 (UI용)
        /// </summary>
        public List<InjuryData> GetInjuredHeroes()
        {
            return injuredHeroes.Values.ToList();
        }

        #endregion

        #region CustomerManager 연동 메서드

        /// <summary>
        /// 방문 중인 Hero들 중 사용 가능한 Hero 필터링
        /// </summary>
        public List<CustomerInstance> GetAvailableVisitingHeroes()
        {
            if (CustomerManager.Instance == null) return new List<CustomerInstance>();
            
            var visitingCustomers = CustomerManager.Instance.GetVisitingCustomers();
            var availableHeroes = new List<CustomerInstance>();

            foreach (var customer in visitingCustomers)
            {
                if (customer.IsHero() && IsHeroAvailable(customer.instanceID))
                {
                    availableHeroes.Add(customer);
                }
            }

            return availableHeroes;
        }

        /// <summary>
        /// 특정 등급의 고용 중인 Hero 목록 반환 (CustomerManager용)
        /// </summary>
        public List<CustomerData> GetEmployedHeroesByGrade(Grade grade)
        {
            var employedHeroes = new List<CustomerData>();
            
            foreach (var heroId in employedHeroIds)
            {
                var heroData = dataManager.GetCustomerData(heroId);
                if (heroData != null && heroData.grade == grade)
                {
                    employedHeroes.Add(heroData);
                }
            }
            
            return employedHeroes;
        }

        #endregion

        #region 저장/로드 시스템

        /// <summary>
        /// 저장용 데이터 수집
        /// </summary>
        public HeroManagerSaveData GetSaveData()
        {
            return new HeroManagerSaveData
            {
                heroCollection = new Dictionary<string, HeroCollectionData>(heroCollection),
                employedHeroIds = new HashSet<string>(employedHeroIds),
                injuredHeroes = injuredHeroes.Values.ToList()
            };
        }

        /// <summary>
        /// 저장 데이터로부터 복원
        /// </summary>
        public void LoadFromSaveData(HeroManagerSaveData saveData)
        {
            if (saveData == null)
            {
                Debug.LogError("HeroManager save data is null");
                return;
            }

            // 도감 데이터 복원
            heroCollection = saveData.heroCollection ?? new Dictionary<string, HeroCollectionData>();
            
            // 고용 Hero 목록 복원
            employedHeroIds = saveData.employedHeroIds ?? new HashSet<string>();
            
            // 부상 Hero 데이터 복원
            injuredHeroes.Clear();
            if (saveData.injuredHeroes != null)
            {
                foreach (var injury in saveData.injuredHeroes)
                {
                    injuredHeroes[injury.heroInstanceID] = injury;
                }
            }

            Debug.Log($"HeroManager data loaded: {heroCollection.Count} heroes in collection, " +
                     $"{employedHeroIds.Count} employed, {injuredHeroes.Count} injured");
        }

        /// <summary>
        /// PlayerData와 동기화 (호환성 유지)
        /// </summary>
        public void SyncWithPlayerData()
        {
            if (playerData == null) return;

            // PlayerData의 heroCollection과 동기화
            if (playerData.heroCollection != null)
            {
                heroCollection = new Dictionary<string, HeroCollectionData>(playerData.heroCollection);
            }
            
            // PlayerData의 injuredHeroes와 동기화
            injuredHeroes.Clear();
            if (playerData.injuredHeroes != null)
            {
                foreach (var injury in playerData.injuredHeroes)
                {
                    injuredHeroes[injury.heroInstanceID] = injury;
                }
            }
        }

        #endregion

        #region 디버깅 및 모니터링

        /// <summary>
        /// Hero 시스템 상태 디버그 출력
        /// </summary>
        [ContextMenu("Debug Hero System Status")]
        public void DebugHeroSystemStatus()
        {
            int maxHeroCount = gameConfig?.maxHeroCount ?? 10;
            
            Debug.Log($"=== Hero Manager Status ===");
            Debug.Log($"도감 등록 Hero 수: {heroCollection.Count(kvp => kvp.Value.isAcquired)}");
            Debug.Log($"현재 고용 중: {employedHeroIds.Count}/{maxHeroCount}");
            Debug.Log($"부상 중인 Hero: {injuredHeroes.Count}");
            Debug.Log($"고용 가능한 슬롯: {GetAvailableHeroSlots()}");
            
            if (employedHeroIds.Count > 0)
            {
                Debug.Log("고용 중인 Hero들:");
                foreach (var heroId in employedHeroIds)
                {
                    var heroData = dataManager.GetCustomerData(heroId);
                    Debug.Log($"  - {heroData?.name ?? heroId} ({heroData?.grade})");
                }
            }
            
            if (injuredHeroes.Count > 0)
            {
                Debug.Log("부상 중인 Hero들:");
                foreach (var injury in injuredHeroes.Values)
                {
                    int daysLeft = injury.returnDay - playerData.currentDay;
                    Debug.Log($"  - {injury.heroInstanceID}: {injury.injuryType} ({daysLeft}일 남음)");
                }
            }
        }

        /// <summary>
        /// Hero 도감 완성도 디버그
        /// </summary>
        [ContextMenu("Debug Hero Collection Progress")]
        public void DebugHeroCollectionProgress()
        {
            int totalHeroes = heroCollection.Count;
            int acquiredHeroes = heroCollection.Count(kvp => kvp.Value.isAcquired);
            float completionRate = totalHeroes > 0 ? (float)acquiredHeroes / totalHeroes * 100f : 0f;
            
            Debug.Log($"=== Hero Collection Progress ===");
            Debug.Log($"완성도: {acquiredHeroes}/{totalHeroes} ({completionRate:F1}%)");
            
            // 등급별 완성도
            foreach (Grade grade in System.Enum.GetValues(typeof(Grade)))
            {
                var heroesOfGrade = dataManager.GetCustomersByGrade(grade);
                int gradeTotal = heroesOfGrade.Count;
                int gradeAcquired = heroesOfGrade.Count(h => IsHeroAcquired(h.id));
                
                if (gradeTotal > 0)
                {
                    float gradeCompletion = (float)gradeAcquired / gradeTotal * 100f;
                    Debug.Log($"  {grade}: {gradeAcquired}/{gradeTotal} ({gradeCompletion:F1}%)");
                }
            }
        }

        #endregion
    }
    
    /// <summary>
    /// HeroManager 저장용 데이터 구조
    /// </summary>
    [System.Serializable]
    public class HeroManagerSaveData
    {
        public Dictionary<string, HeroCollectionData> heroCollection;
        public HashSet<string> employedHeroIds;
        public List<InjuryData> injuredHeroes;
    }
}