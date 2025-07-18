using System.Collections.Generic;
using UnityEngine;

using MagicRentalShop.Core;
using MagicRentalShop.Data;

namespace MagicRentalShop.Systems
{
    /// <summary>
    /// 고객 생성, 관리 및 방문 시스템 처리
    /// Hero 등장 확률 기반 시스템 포함
    /// </summary>
    public class CustomerManager : MonoBehaviour
    {
        [Header("고객 생성 설정")]
        [SerializeField] private float baseLevelMultiplier = 0.2f;
        [SerializeField] private int maxVisitingCustomers = 10;
        
        [Header("캐시 및 최적화")]
        [SerializeField] private bool enablePerformanceOptimization = true;
        [SerializeField] private int heroSearchCacheSize = 50;

        // 의존성
        private DataManager dataManager;
        private HeroManager heroManager;
        private GameConfig gameConfig;
        private PlayerData playerData;
        
        // 방문 중인 고객 목록
        private List<CustomerInstance> visitingCustomers = new List<CustomerInstance>();
        
        // 성능 최적화용 캐시
        private Dictionary<Grade, List<CustomerInstance>> heroCacheByGrade = new Dictionary<Grade, List<CustomerInstance>>();
        private int lastCacheUpdateDay = -1;
        private bool isCacheDirty = true;
        
        // 단일 루프 최적화용 임시 리스트 (GC 방지)
        private List<CustomerInstance> tempHeroList = new List<CustomerInstance>();
        private List<CustomerInstance> tempAvailableList = new List<CustomerInstance>();

        // Hero로 전환된 Customer들의 ID를 저장 (생성 풀에서 제외)
        private HashSet<string> heroConvertedCustomerIds = new HashSet<string>();
        
        // 현재 고용 중인 Hero들의 ID 저장 (Hero 등장 풀)
        private HashSet<string> employedHeroIds = new HashSet<string>();

        // 싱글톤 패턴
        public static CustomerManager Instance { get; private set; }

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
            InitializeHeroCache();
        }

        #region 초기화

        /// <summary>
        /// 의존성 초기화
        /// </summary>
        private void InitializeDependencies()
        {
            dataManager = DataManager.Instance;
            heroManager = HeroManager.Instance;
            
            if (dataManager == null)
            {
                Debug.LogError("DataManager not found!");
                return;
            }
            
            if (heroManager == null)
            {
                Debug.LogError("HeroManager not found!");
                return;
            }
            
            // PlayerData 참조 설정
            playerData = GameController.Instance?.PlayerData;
            
            // GameConfig 로드 (나중에 구현 예정)
            // gameConfig = dataManager.GetGameConfig();
        }
        
        /// <summary>
        /// Hero 캐시 초기화
        /// </summary>
        private void InitializeHeroCache()
        {
            if (!enablePerformanceOptimization) return;
            
            // 각 등급별로 빈 리스트 초기화
            foreach (Grade grade in System.Enum.GetValues(typeof(Grade)))
            {
                heroCacheByGrade[grade] = new List<CustomerInstance>();
            }
        }

        #endregion

        #region Hero 고용/해제 시스템

        /// <summary>
        /// Hero 고용 (도감 → 활성 Hero 풀)
        /// </summary>
        public bool EmployHero(string customerID)
        {
            // 도감에 등록되어 있는지 확인
            if (!heroManager.IsHeroAcquired(customerID))
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
            var allAcquiredHeroes = heroManager.GetAllAcquiredHeroes();
            
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

        #endregion

        #region 고객/Hero 생성 시스템

        /// <summary>
        /// 새로운 방문자 생성 (Customer 또는 Hero)
        /// </summary>
        public CustomerInstance GenerateVisitor(int currentDay)
        {
            // 1. 등급 먼저 결정
            Grade selectedGrade = SelectGradeByProbability(GetGradeSpawnRates());
            
            // 2. 해당 등급에서 Hero 방문 여부 결정
            bool shouldGenerateHero = ShouldGenerateHero(selectedGrade);
            
            CustomerInstance newVisitor = null;
            
            if (shouldGenerateHero)
            {
                // Hero 생성 시도
                newVisitor = GenerateHeroVisitor(selectedGrade, currentDay);
            }
            
            if (newVisitor == null)
            {
                // Hero 생성 실패 또는 Customer 생성
                newVisitor = GenerateCustomerVisitor(selectedGrade, currentDay);
            }
            
            if (newVisitor != null)
            {
                AddVisitingCustomer(newVisitor);
            }
            
            return newVisitor;
        }

        /// <summary>
        /// Hero 방문 여부 결정
        /// </summary>
        private bool ShouldGenerateHero(Grade grade)
        {
            // GameConfig에서 Hero 등장 확률 가져오기
            float heroVisitChance = gameConfig?.heroVisitChance ?? 0.1f;
            
            // 해당 등급에 고용 중인 Hero가 있는지 확인
            var employedHeroesOfGrade = GetEmployedHeroesByGrade(grade);
            if (employedHeroesOfGrade.Count == 0)
            {
                return false; // 고용 중인 Hero가 없으면 Hero 등장 불가
            }
            
            // 설정된 확률로 Hero 등장 결정
            return Random.Range(0f, 1f) < heroVisitChance;
        }

        /// <summary>
        /// Hero 방문자 생성
        /// </summary>
        private CustomerInstance GenerateHeroVisitor(Grade grade, int currentDay)
        {
            // 해당 등급의 고용 중인 Hero 중에서 선택
            var employedHeroes = GetEmployedHeroesByGrade(grade);
            if (employedHeroes.Count == 0) return null;
            
            // 랜덤하게 Hero 선택
            var selectedHeroData = employedHeroes[Random.Range(0, employedHeroes.Count)];
            
            // Hero 레벨 계산 (기본 Customer 레벨 + Hero 보정)
            int heroLevel = CalculateHeroLevel(selectedHeroData, currentDay);
            
            // 던전 배정
            var assignedDungeon = AssignDungeon(grade);
            if (assignedDungeon == null) return null;
            
            // Hero CustomerInstance 생성
            var heroVisitor = new CustomerInstance(selectedHeroData, heroLevel, assignedDungeon);
            heroVisitor.ConvertToHero(playerData.currentDay); // Hero 플래그 설정
            
            Debug.Log($"Hero visitor generated: {heroVisitor.GetDisplayName()} (Level {heroLevel})");
            return heroVisitor;
        }

        /// <summary>
        /// 일반 Customer 방문자 생성
        /// </summary>
        private CustomerInstance GenerateCustomerVisitor(Grade grade, int currentDay)
        {
            // 해당 등급에서 아직 Hero로 전환되지 않은 Customer 선택
            var availableCustomers = GetAvailableCustomersOfGrade(grade);
            if (availableCustomers.Count == 0)
            {
                Debug.LogWarning($"No available customers of grade {grade}");
                return null;
            }
            
            // 랜덤 Customer 선택
            var selectedCustomer = availableCustomers[Random.Range(0, availableCustomers.Count)];
            
            // Customer 레벨 계산
            int customerLevel = CalculateCustomerLevel(currentDay, grade);
            
            // 던전 배정
            var assignedDungeon = AssignDungeon(grade);
            if (assignedDungeon == null) return null;
            
            // Customer Instance 생성
            var customerVisitor = new CustomerInstance(selectedCustomer, customerLevel, assignedDungeon);
            
            Debug.Log($"Customer visitor generated: {customerVisitor.GetDisplayName()} (Level {customerLevel})");
            return customerVisitor;
        }

        /// <summary>
        /// Hero 레벨 계산 (기본 Customer 레벨 + Hero 보정)
        /// </summary>
        private int CalculateHeroLevel(CustomerData heroData, int currentDay)
        {
            // 기본 Customer 레벨 계산
            int baseLevel = CalculateCustomerLevel(currentDay, heroData.grade);
            
            // GameConfig에서 Hero 레벨 보정치 가져오기
            int heroLevelBonus = gameConfig?.heroLevelBonus ?? 5;
            
            // 최종 Hero 레벨
            int finalLevel = baseLevel + heroLevelBonus;
            int maxLevel = gameConfig?.customerMaxLevel ?? 100;
            
            return Mathf.Min(finalLevel, maxLevel);
        }

        /// <summary>
        /// 특정 등급의 사용 가능한 Customer 목록 반환 (임시해제된 Hero 포함)
        /// </summary>
        private List<CustomerData> GetAvailableCustomersOfGrade(Grade grade)
        {
            var customersOfGrade = dataManager.GetCustomersByGrade(grade);
            var availableCustomers = new List<CustomerData>();
            
            foreach (var customer in customersOfGrade)
            {
                if (customer != null)
                {
                    // 아직 Hero로 전환되지 않았거나, 임시해제된 Hero인 경우
                    bool isNotConverted = !heroConvertedCustomerIds.Contains(customer.id);
                    bool isTemporarilyReleased = heroConvertedCustomerIds.Contains(customer.id) && 
                                               !employedHeroIds.Contains(customer.id);
                    
                    if (isNotConverted || isTemporarilyReleased)
                    {
                        availableCustomers.Add(customer);
                    }
                }
            }
            
            return availableCustomers;
        }

        /// <summary>
        /// 특정 등급의 고용 중인 Hero 목록 반환
        /// </summary>
        private List<CustomerData> GetEmployedHeroesByGrade(Grade grade)
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

        #region Hero 풀 관리

        /// <summary>
        /// Hero 전환 시 Customer 생성 풀에서 제거
        /// </summary>
        public void RemoveFromPool(CustomerInstance customer)
        {
            if (customer == null || string.IsNullOrEmpty(customer.data?.id)) return;
            
            // 해당 Customer 타입을 생성 풀에서 제외
            heroConvertedCustomerIds.Add(customer.data.id);
            
            Debug.Log($"Removed customer type '{customer.data.id}' from generation pool (converted to Hero). " +
                     $"Current visiting customer '{customer.GetDisplayName()}' remains available.");
        }

        /// <summary>
        /// 방문 고객 추가 (캐시 무효화 포함)
        /// </summary>
        private void AddVisitingCustomer(CustomerInstance customer)
        {
            if (customer == null) return;
            
            visitingCustomers.Add(customer);
            MarkCacheDirty();
            
            // 최대 방문 고객 수 제한
            if (visitingCustomers.Count > maxVisitingCustomers)
            {
                var oldestCustomer = visitingCustomers[0];
                visitingCustomers.RemoveAt(0);
                Debug.Log($"Removed oldest visitor: {oldestCustomer.GetDisplayName()}");
            }
        }

        /// <summary>
        /// Hero 고용 시 호출되는 콜백 (HeroManager에서 호출)
        /// Hero를 Customer 등장 풀에서 제거하고 Hero 등장 풀에 추가
        /// </summary>
        public void OnHeroEmployed(string customerID)
        {
            if (string.IsNullOrEmpty(customerID))
            {
                Debug.LogError("OnHeroEmployed: customerID가 null 또는 빈 문자열입니다.");
                return;
            }

            // Hero로 전환된 Customer ID 목록에 추가 (이미 있어도 중복 추가되지 않음)
            heroConvertedCustomerIds.Add(customerID);
            
            // 고용된 Hero ID 목록에 추가
            employedHeroIds.Add(customerID);
            
            // 캐시 무효화 (Hero 등장 풀 변경됨)
            MarkCacheDirty();
            
            Debug.Log($"Hero employed: {customerID} - Customer 풀에서 제거, Hero 풀에 추가");
        }

        /// <summary>
        /// Hero 해제 시 호출되는 콜백 (HeroManager에서 호출)
        /// Hero를 Hero 등장 풀에서 제거하고 Customer 등장 풀에 추가
        /// </summary>
        public void OnHeroReleased(string customerID)
        {
            if (string.IsNullOrEmpty(customerID))
            {
                Debug.LogError("OnHeroReleased: customerID가 null 또는 빈 문자열입니다.");
                return;
            }

            // 고용된 Hero ID 목록에서 제거
            employedHeroIds.Remove(customerID);
            
            // heroConvertedCustomerIds는 유지 (Hero로 전환된 이력은 남겨둠)
            // 이렇게 하면 임시해제된 Hero가 Customer로 등장할 수 있음
            
            // 캐시 무효화 (Hero 등장 풀 변경됨)
            MarkCacheDirty();
            
            Debug.Log($"Hero released: {customerID} - Hero 풀에서 제거, Customer 풀에 추가");
        }

        /// <summary>
        /// 특정 Hero의 고용 상태 확인
        /// </summary>
        public bool IsHeroEmployedInCustomerManager(string customerID)
        {
            return employedHeroIds.Contains(customerID);
        }

        /// <summary>
        /// 현재 고용 중인 Hero 수 반환
        /// </summary>
        public int GetEmployedHeroCount()
        {
            return employedHeroIds.Count;
        }

        #endregion

        #region 성능 최적화된 Hero 필터링

        /// <summary>
        /// 특정 등급의 방문 가능한 Hero들 반환 (최적화된 버전)
        /// </summary>
        public List<CustomerInstance> GetAvailableHeroesByGrade(Grade grade)
        {
            if (enablePerformanceOptimization && !isCacheDirty && lastCacheUpdateDay == GetCurrentDay())
            {
                // 캐시가 유효한 경우 캐시된 결과 반환
                return GetCachedHeroesByGrade(grade);
            }

            // 캐시가 무효하거나 최적화가 비활성화된 경우 새로 계산
            return CalculateAvailableHeroesByGrade(grade);
        }

        /// <summary>
        /// 모든 방문 가능한 Hero들 반환 (최적화된 버전)
        /// </summary>
        public List<CustomerInstance> GetAllAvailableHeroes()
        {
            tempAvailableList.Clear();

            // 단일 루프로 모든 방문 가능한 Hero 수집
            for (int i = 0; i < visitingCustomers.Count; i++)
            {
                var customer = visitingCustomers[i];
                if (customer.IsHero() && IsHeroAvailable(customer.instanceID))
                {
                    tempAvailableList.Add(customer);
                }
            }

            // 새 리스트로 복사하여 반환 (원본 보호)
            return new List<CustomerInstance>(tempAvailableList);
        }

        /// <summary>
        /// 단일 루프로 특정 등급의 방문 가능한 Hero들 계산
        /// </summary>
        private List<CustomerInstance> CalculateAvailableHeroesByGrade(Grade grade)
        {
            tempHeroList.Clear();

            // 단일 루프로 조건에 맞는 Hero들 수집
            for (int i = 0; i < visitingCustomers.Count; i++)
            {
                var customer = visitingCustomers[i];
                
                // 조건 체크: Hero && 해당 등급 && 방문 가능
                if (customer.IsHero() && 
                    customer.data?.grade == grade &&
                    IsHeroAvailable(customer.instanceID))
                {
                    tempHeroList.Add(customer);
                }
            }

            // 캐시 업데이트
            if (enablePerformanceOptimization)
            {
                UpdateHeroCacheForGrade(grade, tempHeroList);
            }

            // 새 리스트로 복사하여 반환
            return new List<CustomerInstance>(tempHeroList);
        }

        /// <summary>
        /// Hero 가용성 확인 (HeroManager 연동)
        /// </summary>
        private bool IsHeroAvailable(string heroInstanceID)
        {
            if (heroManager != null)
            {
                return heroManager.IsHeroAvailable(heroInstanceID);
            }
            
            // 임시: 항상 사용 가능하다고 가정
            return true;
        }

        #endregion

        #region 캐시 관리 시스템

        /// <summary>
        /// 캐시 무효화 표시
        /// </summary>
        private void MarkCacheDirty()
        {
            isCacheDirty = true;
        }

        /// <summary>
        /// 특정 등급의 Hero 캐시 업데이트
        /// </summary>
        private void UpdateHeroCacheForGrade(Grade grade, List<CustomerInstance> heroes)
        {
            if (!heroCacheByGrade.ContainsKey(grade))
            {
                heroCacheByGrade[grade] = new List<CustomerInstance>();
            }

            heroCacheByGrade[grade].Clear();
            heroCacheByGrade[grade].AddRange(heroes);
        }

        /// <summary>
        /// 캐시된 Hero 목록 반환
        /// </summary>
        private List<CustomerInstance> GetCachedHeroesByGrade(Grade grade)
        {
            if (heroCacheByGrade.TryGetValue(grade, out List<CustomerInstance> cached))
            {
                return new List<CustomerInstance>(cached);
            }
            
            return new List<CustomerInstance>();
        }

        /// <summary>
        /// 전체 Hero 캐시 갱신 (매일 실행)
        /// </summary>
        public void RefreshHeroCache()
        {
            if (!enablePerformanceOptimization) return;

            foreach (Grade grade in System.Enum.GetValues(typeof(Grade)))
            {
                CalculateAvailableHeroesByGrade(grade);
            }

            lastCacheUpdateDay = GetCurrentDay();
            isCacheDirty = false;

            Debug.Log($"Hero cache refreshed for day {lastCacheUpdateDay}");
        }

        /// <summary>
        /// 현재 게임 일수 반환
        /// </summary>
        private int GetCurrentDay()
        {
            // GameController에서 현재 일수 가져오기
            return GameController.Instance?.CurrentDay ?? 1;
        }

        #endregion

        #region 기존 메서드들 (최적화 적용)

        /// <summary>
        /// 등급별 생성 확률 반환
        /// </summary>
        private float[] GetGradeSpawnRates()
        {
            return gameConfig?.gradeSpawnRates ?? new float[]{0.75f, 0.14f, 0.07f, 0.03f, 0.01f};
        }

        /// <summary>
        /// 확률 배열로부터 등급 선택
        /// </summary>
        private Grade SelectGradeByProbability(float[] probabilities)
        {
            float random = Random.Range(0f, 1f);
            float cumulative = 0f;
            
            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulative += probabilities[i];
                if (random <= cumulative)
                {
                    return (Grade)i;
                }
            }
            
            return Grade.Common; // 기본값
        }

        /// <summary>
        /// 고객 레벨 계산 (일수 기반 + 등급 보너스)
        /// </summary>
        private int CalculateCustomerLevel(int currentDay, Grade customerGrade)
        {
            // 기본 레벨 = 일수 × 0.2
            float baseLevel = currentDay * baseLevelMultiplier;
            
            // 최종 레벨 계산 (최소 1레벨)
            int finalLevel = Mathf.Max(1, Mathf.RoundToInt(baseLevel));
            
            // 최대 레벨 제한
            int maxLevel = gameConfig?.customerMaxLevel ?? 100;
            return Mathf.Min(finalLevel, maxLevel);
        }

        /// <summary>
        /// 고객 등급에 따른 던전 배정 (최적화)
        /// </summary>
        private DungeonData AssignDungeon(Grade customerGrade)
        {
            if (dataManager == null) return null;

            // 1. GameConfig에서 던전 배정 확률 가져오기
            float[] probabilities = GetDungeonAssignmentRates(customerGrade);
            
            // 2. 확률로 던전 등급 선택
            Grade selectedDungeonGrade = SelectGradeByProbability(probabilities);
            
            // 3. DataManager의 등급별 캐시에서 직접 선택
            var dungeonsOfGrade = dataManager.GetDungeonsByGrade(selectedDungeonGrade);
            
            if (dungeonsOfGrade.Count == 0)
            {
                // 해당 등급 던전이 없으면 Common 던전 배정
                dungeonsOfGrade = dataManager.GetDungeonsByGrade(Grade.Common);
            }
            
            if (dungeonsOfGrade.Count == 0) 
            {
                // Common 던전도 없으면 안전한 기본값 반환
                return dataManager.GetDungeonData("default_dungeon");
            }
            
            return dungeonsOfGrade[Random.Range(0, dungeonsOfGrade.Count)];
        }

        /// <summary>
        /// 고객 등급별 던전 배정 확률 가져오기
        /// </summary>
        private float[] GetDungeonAssignmentRates(Grade customerGrade)
        {
            if (gameConfig != null && gameConfig.dungeonAssignmentRates != null && 
                gameConfig.dungeonAssignmentRates.Length > (int)customerGrade)
            {
                return gameConfig.dungeonAssignmentRates[(int)customerGrade].rates;
            }

            // 기본값 반환 (switch 대신 배열로 최적화)
            float[][] defaultRates = {
                new float[]{0.70f, 0.20f, 0.08f, 0.02f, 0.00f}, // Common
                new float[]{0.50f, 0.30f, 0.15f, 0.05f, 0.00f}, // Uncommon
                new float[]{0.30f, 0.35f, 0.25f, 0.08f, 0.02f}, // Rare
                new float[]{0.15f, 0.25f, 0.35f, 0.20f, 0.05f}, // Epic
                new float[]{0.05f, 0.15f, 0.30f, 0.35f, 0.15f}  // Legendary
            };

            int gradeIndex = (int)customerGrade;
            return gradeIndex < defaultRates.Length ? defaultRates[gradeIndex] : defaultRates[0];
        }

        #endregion

        #region 기존 호환성 메서드들

        /// <summary>
        /// 새로운 고객 생성 (하위 호환성)
        /// </summary>
        public CustomerInstance GenerateCustomer(int currentDay)
        {
            return GenerateVisitor(currentDay);
        }

        /// <summary>
        /// 현재 방문 중인 고객 목록 반환
        /// </summary>
        public List<CustomerInstance> GetVisitingCustomers()
        {
            return new List<CustomerInstance>(visitingCustomers);
        }

        /// <summary>
        /// 특정 인스턴스 ID로 고객 찾기 (최적화)
        /// </summary>
        public CustomerInstance GetCustomerByInstanceID(string instanceID)
        {
            if (string.IsNullOrEmpty(instanceID)) return null;

            // for문으로 최적화 (LINQ FirstOrDefault 대신)
            for (int i = 0; i < visitingCustomers.Count; i++)
            {
                if (visitingCustomers[i].instanceID == instanceID)
                {
                    return visitingCustomers[i];
                }
            }

            return null;
        }

        /// <summary>
        /// 방문 고객 목록 복원 (저장/로드용)
        /// </summary>
        public void RestoreCustomers(List<CustomerInstance> customers)
        {
            visitingCustomers = customers ?? new List<CustomerInstance>();
            MarkCacheDirty();
            Debug.Log($"Restored {visitingCustomers.Count} visiting customers");
        }

        /// <summary>
        /// Hero로 전환된 고객 ID 목록 복원 (저장/로드용)
        /// </summary>
        public void RestoreHeroConvertedIds(HashSet<string> convertedIds)
        {
            heroConvertedCustomerIds = convertedIds ?? new HashSet<string>();
            Debug.Log($"Restored {heroConvertedCustomerIds.Count} hero-converted customer types");
        }

        /// <summary>
        /// 고용 중인 Hero ID 목록 복원 (저장/로드용)
        /// </summary>
        public void RestoreEmployedHeroIds(HashSet<string> employedIds)
        {
            employedHeroIds = employedIds ?? new HashSet<string>();
            Debug.Log($"Restored {employedHeroIds.Count} employed heroes");
        }

        /// <summary>
        /// Hero로 전환된 고객 ID 목록 반환 (저장용)
        /// </summary>
        public HashSet<string> GetHeroConvertedIds()
        {
            return new HashSet<string>(heroConvertedCustomerIds);
        }

        /// <summary>
        /// 고용 중인 Hero ID 목록 반환 (저장용)
        /// </summary>
        public HashSet<string> GetEmployedHeroIdsSaveData()
        {
            return new HashSet<string>(employedHeroIds);
        }

        /// <summary>
        /// 현재 상태를 저장용 데이터로 변환
        /// </summary>
        public List<CustomerInstanceSaveData> GetSaveData()
        {
            var saveData = new List<CustomerInstanceSaveData>();
            for (int i = 0; i < visitingCustomers.Count; i++)
            {
                saveData.Add(new CustomerInstanceSaveData(visitingCustomers[i]));
            }
            return saveData;
        }

        #endregion

        #region 성능 모니터링 및 디버깅

        /// <summary>
        /// Hero 등장 확률 설정 (디버깅용)
        /// </summary>
        public void SetHeroVisitChance(float chance)
        {
            Debug.LogWarning("Hero visit chance는 GameConfig에서 관리됩니다. GameConfig.heroVisitChance를 수정하세요.");
        }

        /// <summary>
        /// 성능 통계 출력 (디버깅용)
        /// </summary>
        [ContextMenu("Debug Performance Stats")]
        public void DebugPerformanceStats()
        {
            float heroVisitChance = gameConfig?.heroVisitChance ?? 0.1f;
            
            Debug.Log($"=== CustomerManager Performance Stats ===");
            Debug.Log($"성능 최적화 활성화: {enablePerformanceOptimization}");
            Debug.Log($"방문 고객 수: {visitingCustomers.Count}");
            Debug.Log($"Hero 등장 확률: {heroVisitChance * 100}% (GameConfig)");
            Debug.Log($"고용 중인 Hero: {employedHeroIds.Count}명");
            Debug.Log($"최대 Hero 고용 수: {gameConfig?.maxHeroCount ?? 10}명");
            Debug.Log($"캐시 상태: {(isCacheDirty ? "Dirty" : "Clean")}");
            Debug.Log($"마지막 캐시 업데이트: Day {lastCacheUpdateDay}");
            
            if (enablePerformanceOptimization)
            {
                Debug.Log("등급별 캐시된 Hero 수:");
                foreach (var kvp in heroCacheByGrade)
                {
                    Debug.Log($"  {kvp.Key}: {kvp.Value.Count}개");
                }
            }
        }

        /// <summary>
        /// 디버그용 정보 출력
        /// </summary>
        [ContextMenu("Debug Customer Info")]
        private void DebugCustomerInfo()
        {
            Debug.Log($"=== Customer Manager Debug Info ===");
            Debug.Log($"Total visiting customers: {visitingCustomers.Count}");
            Debug.Log($"Hero-converted customer types: {heroConvertedCustomerIds.Count}");
            Debug.Log($"Employed heroes: {employedHeroIds.Count}");
            
            int heroCount = 0;
            int customerCount = 0;
            
            foreach (var customer in visitingCustomers)
            {
                if (customer.IsHero())
                    heroCount++;
                else
                    customerCount++;
                    
                Debug.Log(customer.ToString());
            }
            
            Debug.Log($"Heroes: {heroCount}, Customers: {customerCount}");
            
            if (heroConvertedCustomerIds.Count > 0)
            {
                Debug.Log("Hero로 전환된 고객 타입들:");
                foreach (var id in heroConvertedCustomerIds)
                {
                    string status = employedHeroIds.Contains(id) ? "고용중" : "임시해제";
                    Debug.Log($"  - {id} ({status})");
                }
            }
        }

        /// <summary>
        /// Hero 고용 상태 디버그
        /// </summary>
        [ContextMenu("Debug Hero Employment")]
        private void DebugHeroEmployment()
        {
            int maxHeroCount = gameConfig?.maxHeroCount ?? 10;
            
            Debug.Log($"=== Hero Employment Debug ===");
            Debug.Log($"최대 고용 가능: {maxHeroCount}명");
            Debug.Log($"현재 고용 중: {employedHeroIds.Count}명");
            Debug.Log($"남은 슬롯: {GetAvailableHeroSlots()}개");
            
            if (employedHeroIds.Count > 0)
            {
                Debug.Log("고용 중인 Hero들:");
                foreach (var heroId in employedHeroIds)
                {
                    var heroData = dataManager.GetCustomerData(heroId);
                    Debug.Log($"  - {heroData?.name ?? heroId} ({heroData?.grade})");
                }
            }
            
            var availableToEmploy = GetAvailableToEmployHeroes();
            if (availableToEmploy.Count > 0)
            {
                Debug.Log($"고용 가능한 Hero: {availableToEmploy.Count}명");
                foreach (var hero in availableToEmploy)
                {
                    Debug.Log($"  - {hero.name} ({hero.grade})");
                }
            }
        }

        #endregion
    }
}