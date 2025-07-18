using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using MagicRentalShop.Data;

namespace MagicRentalShop.Core
{
    /// <summary>
    /// 모든 정적 데이터를 Dictionary로 관리하는 '데이터 사전'
    /// 게임 시작 시 모든 ScriptableObject를 로드하여 빠른 접근 제공
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        [Header("데이터 컨테이너")]
        [SerializeField] private GameDataContainer dataContainer;

        [Header("게임 설정")]
        [SerializeField] private GameConfig gameConfig;
        
        [Header("기본 데이터 (에러 시 반환용)")]
        [SerializeField] private WeaponData defaultWeaponData;
        [SerializeField] private CustomerData defaultCustomerData;
        [SerializeField] private DungeonData defaultDungeonData;
        [SerializeField] private MaterialData defaultMaterialData;
        [SerializeField] private RecipeData defaultRecipeData;
        [SerializeField] private DailyEventData defaultEventData;
        
        [Header("로드된 데이터 상태")]
        [SerializeField] private bool isInitialized = false;
        [SerializeField] private int totalLoadedAssets = 0;
        [SerializeField] private int errorCount = 0;
        
        // 정적 데이터 Dictionary들
        private Dictionary<string, WeaponData> weaponDatas = new Dictionary<string, WeaponData>();
        private Dictionary<string, CustomerData> customerDatas = new Dictionary<string, CustomerData>();
        private Dictionary<string, DungeonData> dungeonDatas = new Dictionary<string, DungeonData>();
        private Dictionary<string, MaterialData> materialDatas = new Dictionary<string, MaterialData>();
        private Dictionary<string, RecipeData> recipeDatas = new Dictionary<string, RecipeData>();
        private Dictionary<string, DailyEventData> dailyEventDatas = new Dictionary<string, DailyEventData>();
        
        // 등급별 분류 캐시 (성능 최적화용)
        private Dictionary<Grade, List<WeaponData>> weaponsByGrade = new Dictionary<Grade, List<WeaponData>>();
        private Dictionary<Grade, List<CustomerData>> customersByGrade = new Dictionary<Grade, List<CustomerData>>();
        private Dictionary<Grade, List<DungeonData>> dungeonsByGrade = new Dictionary<Grade, List<DungeonData>>();
        private Dictionary<Grade, List<MaterialData>> materialsByGrade = new Dictionary<Grade, List<MaterialData>>();
        
        // 에러 추적용 (디버깅용)
        private List<string> missingDataIds = new List<string>();
        
        // 싱글톤 패턴
        public static DataManager Instance { get; private set; }
        
        // 초기화 상태 확인용 프로퍼티
        public bool IsInitialized => isInitialized;
        public int TotalLoadedAssets => totalLoadedAssets;
        public int ErrorCount => errorCount;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Init();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 모든 ScriptableObject 데이터를 로드하고 Dictionary 구성
        /// </summary>
        public void Init()
        {
            if (isInitialized)
            {
                Debug.LogWarning("DataManager already initialized!");
                return;
            }
            
            Debug.Log("=== DataManager 초기화 시작 ===");
            
            // 기본 데이터 유효성 검증
            ValidateDefaultData();
            
            // 각 데이터 타입별로 로드
            LoadWeaponData();
            LoadCustomerData();
            LoadDungeonData();
            LoadMaterialData();
            LoadRecipeData();
            LoadDailyEventData();
            
            // 등급별 분류 캐시 생성
            BuildGradeCache();
            
            isInitialized = true;
            
            Debug.Log($"=== DataManager 초기화 완료 ===");
            Debug.Log($"총 로드된 에셋: {totalLoadedAssets}개");
            Debug.Log($"에러 발생 횟수: {errorCount}개");
            
            if (errorCount > 0)
            {
                Debug.LogWarning($"일부 데이터 로드 실패로 기본값 사용: {string.Join(", ", missingDataIds)}");
            }
            
            LogLoadedDataSummary();
        }

        #region 기본 데이터 검증
        
        /// <summary>
        /// 기본 데이터들이 할당되어 있는지 검증
        /// </summary>
        private void ValidateDefaultData()
        {
            if (defaultWeaponData == null)
            {
                Debug.LogError("[DataManager] defaultWeaponData가 설정되지 않았습니다!");
            }
            
            if (defaultCustomerData == null)
            {
                Debug.LogError("[DataManager] defaultCustomerData가 설정되지 않았습니다!");
            }
            
            if (defaultDungeonData == null)
            {
                Debug.LogError("[DataManager] defaultDungeonData가 설정되지 않았습니다!");
            }
            
            if (defaultMaterialData == null)
            {
                Debug.LogError("[DataManager] defaultMaterialData가 설정되지 않았습니다!");
            }
            
            if (defaultRecipeData == null)
            {
                Debug.LogError("[DataManager] defaultRecipeData가 설정되지 않았습니다!");
            }
            
            if (defaultEventData == null)
            {
                Debug.LogError("[DataManager] defaultEventData가 설정되지 않았습니다!");
            }
        }
        
        #endregion

        #region 데이터 로드 메서드들
        
        private void LoadWeaponData()
        {
            if (dataContainer?.weaponDatas == null)
            {
                Debug.LogError("WeaponData 컨테이너가 null입니다!");
                return;
            }
            
            foreach (var weapon in dataContainer.weaponDatas)
            {
                if (weapon != null && !string.IsNullOrEmpty(weapon.id))
                {
                    weaponDatas[weapon.id] = weapon;
                    totalLoadedAssets++;
                }
                else
                {
                    Debug.LogWarning("Invalid WeaponData found (null or empty id)");
                    errorCount++;
                }
            }
            Debug.Log($"무기 데이터 로드 완료: {weaponDatas.Count}개");
        }
        
        private void LoadCustomerData()
        {
            if (dataContainer?.customerDatas == null)
            {
                Debug.LogError("CustomerData 컨테이너가 null입니다!");
                return;
            }
            
            foreach (var customer in dataContainer.customerDatas)
            {
                if (customer != null && !string.IsNullOrEmpty(customer.id))
                {
                    customerDatas[customer.id] = customer;
                    totalLoadedAssets++;
                }
                else
                {
                    Debug.LogWarning("Invalid CustomerData found (null or empty id)");
                    errorCount++;
                }
            }
            Debug.Log($"고객 데이터 로드 완료: {customerDatas.Count}개");
        }
        
        private void LoadDungeonData()
        {
            if (dataContainer?.dungeonDatas == null)
            {
                Debug.LogError("DungeonData 컨테이너가 null입니다!");
                return;
            }
            
            foreach (var dungeon in dataContainer.dungeonDatas)
            {
                if (dungeon != null && !string.IsNullOrEmpty(dungeon.id))
                {
                    dungeonDatas[dungeon.id] = dungeon;
                    totalLoadedAssets++;
                }
                else
                {
                    Debug.LogWarning("Invalid DungeonData found (null or empty id)");
                    errorCount++;
                }
            }
            Debug.Log($"던전 데이터 로드 완료: {dungeonDatas.Count}개");
        }
        
        private void LoadMaterialData()
        {
            if (dataContainer?.materialDatas == null)
            {
                Debug.LogError("MaterialData 컨테이너가 null입니다!");
                return;
            }
            
            foreach (var material in dataContainer.materialDatas)
            {
                if (material != null && !string.IsNullOrEmpty(material.id))
                {
                    materialDatas[material.id] = material;
                    totalLoadedAssets++;
                }
                else
                {
                    Debug.LogWarning("Invalid MaterialData found (null or empty id)");
                    errorCount++;
                }
            }
            Debug.Log($"재료 데이터 로드 완료: {materialDatas.Count}개");
        }
        
        private void LoadRecipeData()
        {
            if (dataContainer?.recipeDatas == null)
            {
                Debug.LogError("RecipeData 컨테이너가 null입니다!");
                return;
            }
            
            foreach (var recipe in dataContainer.recipeDatas)
            {
                if (recipe != null && !string.IsNullOrEmpty(recipe.id))
                {
                    recipeDatas[recipe.id] = recipe;
                    totalLoadedAssets++;
                }
                else
                {
                    Debug.LogWarning("Invalid RecipeData found (null or empty id)");
                    errorCount++;
                }
            }
            Debug.Log($"레시피 데이터 로드 완료: {recipeDatas.Count}개");
        }
        
        private void LoadDailyEventData()
        {
            if (dataContainer?.dailyEventDatas == null)
            {
                Debug.LogError("DailyEventData 컨테이너가 null입니다!");
                return;
            }
            
            foreach (var eventData in dataContainer.dailyEventDatas)
            {
                if (eventData != null && !string.IsNullOrEmpty(eventData.id))
                {
                    dailyEventDatas[eventData.id] = eventData;
                    totalLoadedAssets++;
                }
                else
                {
                    Debug.LogWarning("Invalid DailyEventData found (null or empty id)");
                    errorCount++;
                }
            }
            Debug.Log($"이벤트 데이터 로드 완료: {dailyEventDatas.Count}개");
        }

        #endregion

        #region GameConfig 관리

        /// <summary>
        /// 게임 설정 데이터 반환
        /// </summary>
        public GameConfig GetGameConfig()
        {
            if (gameConfig == null)
            {                
                Debug.LogError("GameConfig를 찾을 수 없습니다!");
            }
            
            return gameConfig;
        }

        /// <summary>
        /// GameConfig 설정 (Inspector나 외부에서 설정 시 사용)
        /// </summary>
        public void SetGameConfig(GameConfig config)
        {
            gameConfig = config;
        }

        #endregion

        #region 개별 데이터 접근 메서드

        /// <summary>
        /// 특정 ID의 무기 데이터 반환 (에러 시 기본값 반환)
        /// </summary>
        public WeaponData GetWeaponData(string id)
        {
            // ID가 유효하지 않은 경우
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogWarning("GetWeaponData: ID가 null이거나 비어있습니다. 기본 무기 데이터를 반환합니다.");
                RecordMissingData($"WeaponData:{id}");
                return defaultWeaponData;
            }

            // 데이터를 찾은 경우
            if (weaponDatas.TryGetValue(id, out WeaponData weapon))
            {
                return weapon;
            }

            // 데이터를 찾지 못한 경우
            Debug.LogWarning($"WeaponData not found: {id}. 기본 무기 데이터를 반환합니다.");
            RecordMissingData($"WeaponData:{id}");
            return defaultWeaponData;
        }
        
        /// <summary>
        /// 특정 ID의 고객 데이터 반환 (에러 시 기본값 반환)
        /// </summary>
        public CustomerData GetCustomerData(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogWarning("GetCustomerData: ID가 null이거나 비어있습니다. 기본 고객 데이터를 반환합니다.");
                RecordMissingData($"CustomerData:{id}");
                return defaultCustomerData;
            }
            
            if (customerDatas.TryGetValue(id, out CustomerData customer))
            {
                return customer;
            }
            
            Debug.LogWarning($"CustomerData not found: {id}. 기본 고객 데이터를 반환합니다.");
            RecordMissingData($"CustomerData:{id}");
            return defaultCustomerData;
        }
        
        /// <summary>
        /// 특정 ID의 던전 데이터 반환 (에러 시 기본값 반환)
        /// </summary>
        public DungeonData GetDungeonData(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogWarning("GetDungeonData: ID가 null이거나 비어있습니다. 기본 던전 데이터를 반환합니다.");
                RecordMissingData($"DungeonData:{id}");
                return defaultDungeonData;
            }
            
            if (dungeonDatas.TryGetValue(id, out DungeonData dungeon))
            {
                return dungeon;
            }
            
            Debug.LogWarning($"DungeonData not found: {id}. 기본 던전 데이터를 반환합니다.");
            RecordMissingData($"DungeonData:{id}");
            return defaultDungeonData;
        }
        
        /// <summary>
        /// 특정 ID의 재료 데이터 반환 (에러 시 기본값 반환)
        /// </summary>
        public MaterialData GetMaterialData(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogWarning("GetMaterialData: ID가 null이거나 비어있습니다. 기본 재료 데이터를 반환합니다.");
                RecordMissingData($"MaterialData:{id}");
                return defaultMaterialData;
            }
            
            if (materialDatas.TryGetValue(id, out MaterialData material))
            {
                return material;
            }
            
            Debug.LogWarning($"MaterialData not found: {id}. 기본 재료 데이터를 반환합니다.");
            RecordMissingData($"MaterialData:{id}");
            return defaultMaterialData;
        }
        
        /// <summary>
        /// 특정 ID의 레시피 데이터 반환 (에러 시 기본값 반환)
        /// </summary>
        public RecipeData GetRecipeData(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogWarning("GetRecipeData: ID가 null이거나 비어있습니다. 기본 레시피 데이터를 반환합니다.");
                RecordMissingData($"RecipeData:{id}");
                return defaultRecipeData;
            }
            
            if (recipeDatas.TryGetValue(id, out RecipeData recipe))
            {
                return recipe;
            }
            
            Debug.LogWarning($"RecipeData not found: {id}. 기본 레시피 데이터를 반환합니다.");
            RecordMissingData($"RecipeData:{id}");
            return defaultRecipeData;
        }
        
        /// <summary>
        /// 특정 ID의 일일 이벤트 데이터 반환 (에러 시 기본값 반환)
        /// </summary>
        public DailyEventData GetDailyEventData(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogWarning("GetDailyEventData: ID가 null이거나 비어있습니다. 기본 이벤트 데이터를 반환합니다.");
                RecordMissingData($"DailyEventData:{id}");
                return defaultEventData;
            }
            
            if (dailyEventDatas.TryGetValue(id, out DailyEventData eventData))
            {
                return eventData;
            }
            
            Debug.LogWarning($"DailyEventData not found: {id}. 기본 이벤트 데이터를 반환합니다.");
            RecordMissingData($"DailyEventData:{id}");
            return defaultEventData;
        }
        
        #endregion

        #region 에러 추적 및 디버깅
        
        /// <summary>
        /// 누락된 데이터 기록 (중복 제거)
        /// </summary>
        private void RecordMissingData(string dataId)
        {
            if (!missingDataIds.Contains(dataId))
            {
                missingDataIds.Add(dataId);
                errorCount++;
            }
        }
        
        /// <summary>
        /// 누락된 데이터 목록 반환 (디버깅용)
        /// </summary>
        public List<string> GetMissingDataIds()
        {
            return new List<string>(missingDataIds);
        }
        
        /// <summary>
        /// 에러 통계 출력 (디버깅용)
        /// </summary>
        [ContextMenu("Debug Error Stats")]
        public void DebugErrorStats()
        {
            Debug.Log($"=== DataManager 에러 통계 ===");
            Debug.Log($"총 에러 횟수: {errorCount}");
            Debug.Log($"누락된 데이터 종류: {missingDataIds.Count}");
            
            if (missingDataIds.Count > 0)
            {
                Debug.Log("누락된 데이터 목록:");
                foreach (var id in missingDataIds)
                {
                    Debug.Log($"  - {id}");
                }
            }
        }
        
        #endregion

        #region 전체 데이터 접근 메서드 
        
        /// <summary>
        /// 모든 고객 데이터 반환
        /// </summary>
        public List<CustomerData> GetAllCustomerData()
        {
            return customerDatas.Values.ToList();
        }
        
        /// <summary>
        /// 모든 무기 데이터 반환
        /// </summary>
        public List<WeaponData> GetAllWeaponData()
        {
            return weaponDatas.Values.ToList();
        }
        
        /// <summary>
        /// 모든 던전 데이터 반환
        /// </summary>
        public List<DungeonData> GetAllDungeonData()
        {
            return dungeonDatas.Values.ToList();
        }
        
        /// <summary>
        /// 모든 재료 데이터 반환
        /// </summary>
        public List<MaterialData> GetAllMaterialData()
        {
            return materialDatas.Values.ToList();
        }
        
        /// <summary>
        /// 모든 레시피 데이터 반환
        /// </summary>
        public List<RecipeData> GetAllRecipeData()
        {
            return recipeDatas.Values.ToList();
        }
        
        #endregion

        #region 등급별 데이터 접근 메서드 (안전성 개선)
        
        /// <summary>
        /// 특정 등급의 무기들 반환
        /// </summary>
        public List<WeaponData> GetWeaponsByGrade(Grade grade)
        {
            if (weaponsByGrade.TryGetValue(grade, out List<WeaponData> weapons))
            {
                return new List<WeaponData>(weapons);
            }
            
            // 해당 등급이 없으면 빈 리스트 반환 (기본 무기 리스트 반환하지 않음)
            Debug.LogWarning($"해당 등급의 무기가 없습니다: {grade}");
            return new List<WeaponData>();
        }
        
        /// <summary>
        /// 특정 등급의 고객들 반환
        /// </summary>
        public List<CustomerData> GetCustomersByGrade(Grade grade)
        {
            if (customersByGrade.TryGetValue(grade, out List<CustomerData> customers))
            {
                return new List<CustomerData>(customers);
            }
            
            Debug.LogWarning($"해당 등급의 고객이 없습니다: {grade}");
            return new List<CustomerData>();
        }
        
        /// <summary>
        /// 특정 등급의 던전들 반환 (CustomerManager에서 사용)
        /// </summary>
        public List<DungeonData> GetDungeonsByGrade(Grade grade)
        {
            if (dungeonsByGrade.TryGetValue(grade, out List<DungeonData> dungeons))
            {
                return new List<DungeonData>(dungeons);
            }
            
            Debug.LogWarning($"해당 등급의 던전이 없습니다: {grade}");
            return new List<DungeonData>();
        }
        
        /// <summary>
        /// 특정 등급의 재료들 반환
        /// </summary>
        public List<MaterialData> GetMaterialsByGrade(Grade grade)
        {
            if (materialsByGrade.TryGetValue(grade, out List<MaterialData> materials))
            {
                return new List<MaterialData>(materials);
            }
            
            Debug.LogWarning($"해당 등급의 재료가 없습니다: {grade}");
            return new List<MaterialData>();
        }
        
        #endregion

        #region 등급별 캐시 구성 (기존 유지)
        
        private void BuildGradeCache()
        {
            // 등급별 무기 분류
            foreach (Grade grade in System.Enum.GetValues(typeof(Grade)))
            {
                weaponsByGrade[grade] = weaponDatas.Values.Where(w => w.grade == grade).ToList();
                customersByGrade[grade] = customerDatas.Values.Where(c => c.grade == grade).ToList();
                dungeonsByGrade[grade] = dungeonDatas.Values.Where(d => d.grade == grade).ToList();
                materialsByGrade[grade] = materialDatas.Values.Where(m => m.grade == grade).ToList();
            }
            
            Debug.Log("등급별 분류 캐시 구성 완료");
        }
        
        #endregion

        #region 로그 출력 메서드
        
        private void LogLoadedDataSummary()
        {
            Debug.Log($"로드된 데이터 요약:\n" +
                     $"- 무기: {weaponDatas.Count}개\n" +
                     $"- 고객: {customerDatas.Count}개\n" +
                     $"- 던전: {dungeonDatas.Count}개\n" +
                     $"- 재료: {materialDatas.Count}개\n" +
                     $"- 레시피: {recipeDatas.Count}개\n" +
                     $"- 이벤트: {dailyEventDatas.Count}개");
        }
        
        #endregion
    }
}