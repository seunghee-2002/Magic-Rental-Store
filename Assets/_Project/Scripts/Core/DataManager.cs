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
        [Header("로드된 데이터 상태")]
        [SerializeField] private bool isInitialized = false;
        [SerializeField] private int totalLoadedAssets = 0;
        
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
        
        // 싱글톤 패턴
        public static DataManager Instance { get; private set; }
        
        // 초기화 상태 확인용 프로퍼티
        public bool IsInitialized => isInitialized;
        public int TotalLoadedAssets => totalLoadedAssets;

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
            LogLoadedDataSummary();
        }

        #region 데이터 로드 메서드들
        
        private void LoadWeaponData()
        {
            foreach (var weapon in dataContainer.weaponDatas)
            {
                if (!string.IsNullOrEmpty(weapon.id))
                {
                    weaponDatas[weapon.id] = weapon;
                    totalLoadedAssets++;
                }
            }
            Debug.Log($"무기 데이터 로드 완료: {weaponDatas.Count}개");
        }
        
        private void LoadCustomerData()
        {
            foreach (var customer in dataContainer.customerDatas)
            {
                if (!string.IsNullOrEmpty(customer.id))
                {
                    customerDatas[customer.id] = customer;
                    totalLoadedAssets++;
                }
            }
            Debug.Log($"고객 데이터 로드 완료: {customerDatas.Count}개");
        }
        
        private void LoadDungeonData()
        {
            foreach (var dungeon in dataContainer.dungeonDatas)
            {
                if (!string.IsNullOrEmpty(dungeon.id))
                {
                    dungeonDatas[dungeon.id] = dungeon;
                    totalLoadedAssets++;
                }
            }
            Debug.Log($"던전 데이터 로드 완료: {dungeonDatas.Count}개");
        }
        
        private void LoadMaterialData()
        {
            
            foreach (var material in dataContainer.materialDatas)
            {
                if (!string.IsNullOrEmpty(material.id))
                {
                    materialDatas[material.id] = material;
                    totalLoadedAssets++;
                }
            }
            Debug.Log($"재료 데이터 로드 완료: {materialDatas.Count}개");
        }
        
        private void LoadRecipeData()
        {
            foreach (var recipe in dataContainer.recipeDatas)
            {
                if (!string.IsNullOrEmpty(recipe.id))
                {
                    recipeDatas[recipe.id] = recipe;
                    totalLoadedAssets++;
                }
            }
            Debug.Log($"레시피 데이터 로드 완료: {recipeDatas.Count}개");
        }
        
        private void LoadDailyEventData()
        {
            foreach (var eventData in dataContainer.dailyEventDatas)
            {
                if (!string.IsNullOrEmpty(eventData.id))
                {
                    dailyEventDatas[eventData.id] = eventData;
                    totalLoadedAssets++;
                }
            }
            Debug.Log($"일일 이벤트 데이터 로드 완료: {dailyEventDatas.Count}개");
        }
        
        #endregion

        #region 등급별 분류 캐시 구성
        
        private void BuildGradeCache()
        {
            // 무기 등급별 분류
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

        #region 개별 데이터 접근 메서드
        
        /// <summary>
        /// 특정 ID의 무기 데이터 반환
        /// </summary>
        public WeaponData GetWeaponData(string id)
        {
            if (weaponDatas.TryGetValue(id, out WeaponData weapon))
            {
                return weapon;
            }
            
            Debug.LogError($"WeaponData not found: {id}");
            return null;
        }
        
        /// <summary>
        /// 특정 ID의 고객 데이터 반환
        /// </summary>
        public CustomerData GetCustomerData(string id)
        {
            if (customerDatas.TryGetValue(id, out CustomerData customer))
            {
                return customer;
            }
            
            Debug.LogError($"CustomerData not found: {id}");
            return null;
        }
        
        /// <summary>
        /// 특정 ID의 던전 데이터 반환
        /// </summary>
        public DungeonData GetDungeonData(string id)
        {
            if (dungeonDatas.TryGetValue(id, out DungeonData dungeon))
            {
                return dungeon;
            }
            
            Debug.LogError($"DungeonData not found: {id}");
            return null;
        }
        
        /// <summary>
        /// 특정 ID의 재료 데이터 반환
        /// </summary>
        public MaterialData GetMaterialData(string id)
        {
            if (materialDatas.TryGetValue(id, out MaterialData material))
            {
                return material;
            }
            
            Debug.LogError($"MaterialData not found: {id}");
            return null;
        }
        
        /// <summary>
        /// 특정 ID의 레시피 데이터 반환
        /// </summary>
        public RecipeData GetRecipeData(string id)
        {
            if (recipeDatas.TryGetValue(id, out RecipeData recipe))
            {
                return recipe;
            }
            
            Debug.LogError($"RecipeData not found: {id}");
            return null;
        }
        
        /// <summary>
        /// 특정 ID의 일일 이벤트 데이터 반환
        /// </summary>
        public DailyEventData GetDailyEventData(string id)
        {
            if (dailyEventDatas.TryGetValue(id, out DailyEventData eventData))
            {
                return eventData;
            }
            
            Debug.LogError($"DailyEventData not found: {id}");
            return null;
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

        #region 등급별 데이터 접근 메서드
        
        /// <summary>
        /// 특정 등급의 무기들 반환
        /// </summary>
        public List<WeaponData> GetWeaponsByGrade(Grade grade)
        {
            return weaponsByGrade.TryGetValue(grade, out List<WeaponData> weapons) ? 
                   new List<WeaponData>(weapons) : new List<WeaponData>();
        }
        
        /// <summary>
        /// 특정 등급의 고객들 반환
        /// </summary>
        public List<CustomerData> GetCustomersByGrade(Grade grade)
        {
            return customersByGrade.TryGetValue(grade, out List<CustomerData> customers) ? 
                   new List<CustomerData>(customers) : new List<CustomerData>();
        }
        
        /// <summary>
        /// 특정 등급의 던전들 반환 (CustomerManager에서 사용)
        /// </summary>
        public List<DungeonData> GetDungeonsByGrade(Grade grade)
        {
            return dungeonsByGrade.TryGetValue(grade, out List<DungeonData> dungeons) ? 
                   new List<DungeonData>(dungeons) : new List<DungeonData>();
        }
        
        /// <summary>
        /// 특정 등급의 재료들 반환
        /// </summary>
        public List<MaterialData> GetMaterialsByGrade(Grade grade)
        {
            return materialsByGrade.TryGetValue(grade, out List<MaterialData> materials) ? 
                   new List<MaterialData>(materials) : new List<MaterialData>();
        }
        
        #endregion

        #region 재료-던전 연결 정보 (MaterialData 확장 지원)
        
        /// <summary>
        /// 특정 재료를 획득할 수 있는 던전 목록 반환
        /// </summary>
        public List<DungeonData> GetDungeonsForMaterial(string materialId)
        {
            var material = GetMaterialData(materialId);
            if (material == null || material.availableDungeonIDs == null)
            {
                return new List<DungeonData>();
            }
            
            var dungeons = new List<DungeonData>();
            foreach (string dungeonId in material.availableDungeonIDs)
            {
                var dungeon = GetDungeonData(dungeonId);
                if (dungeon != null)
                {
                    dungeons.Add(dungeon);
                }
            }
            
            return dungeons;
        }
        
        /// <summary>
        /// 특정 던전에서 획득 가능한 재료 목록 반환
        /// </summary>
        public List<MaterialData> GetMaterialsFromDungeon(string dungeonId)
        {
            return materialDatas.Values
                .Where(m => m.availableDungeonIDs != null && m.availableDungeonIDs.Contains(dungeonId))
                .ToList();
        }
        
        #endregion

        #region 유틸리티 및 디버그 메서드
        
        /// <summary>
        /// 데이터 유효성 검사
        /// </summary>
        public bool ValidateData()
        {
            bool isValid = true;
            
            // 기본 데이터 존재 여부 확인
            if (weaponDatas.Count == 0)
            {
                Debug.LogError("No weapon data loaded!");
                isValid = false;
            }
            
            if (customerDatas.Count == 0)
            {
                Debug.LogError("No customer data loaded!");
                isValid = false;
            }
            
            if (dungeonDatas.Count == 0)
            {
                Debug.LogError("No dungeon data loaded!");
                isValid = false;
            }
            
            // 재료-던전 연결 유효성 확인
            foreach (var material in materialDatas.Values)
            {
                if (material.availableDungeonIDs != null)
                {
                    foreach (string dungeonId in material.availableDungeonIDs)
                    {
                        if (!dungeonDatas.ContainsKey(dungeonId))
                        {
                            Debug.LogError($"Material {material.id} references non-existent dungeon: {dungeonId}");
                            isValid = false;
                        }
                    }
                }
            }
            
            return isValid;
        }
        
        /// <summary>
        /// 로드된 데이터 요약 로그 출력
        /// </summary>
        private void LogLoadedDataSummary()
        {
            Debug.Log("=== 데이터 로드 요약 ===");
            Debug.Log($"무기: {weaponDatas.Count}개");
            Debug.Log($"고객: {customerDatas.Count}개");
            Debug.Log($"던전: {dungeonDatas.Count}개");
            Debug.Log($"재료: {materialDatas.Count}개");
            Debug.Log($"레시피: {recipeDatas.Count}개");
            Debug.Log($"일일이벤트: {dailyEventDatas.Count}개");
            
            // 등급별 분포 출력
            foreach (Grade grade in System.Enum.GetValues(typeof(Grade)))
            {
                Debug.Log($"{grade} 등급 - 무기:{weaponsByGrade[grade].Count}, " +
                         $"고객:{customersByGrade[grade].Count}, " +
                         $"던전:{dungeonsByGrade[grade].Count}, " +
                         $"재료:{materialsByGrade[grade].Count}");
            }
        }
        
        /// <summary>
        /// 런타임에서 데이터 재로드 (개발용)
        /// </summary>
        [ContextMenu("Reload All Data")]
        public void ReloadData()
        {
            // 기존 데이터 클리어
            weaponDatas.Clear();
            customerDatas.Clear();
            dungeonDatas.Clear();
            materialDatas.Clear();
            recipeDatas.Clear();
            dailyEventDatas.Clear();
            
            weaponsByGrade.Clear();
            customersByGrade.Clear();
            dungeonsByGrade.Clear();
            materialsByGrade.Clear();
            
            totalLoadedAssets = 0;
            isInitialized = false;
            
            // 재초기화
            Init();
        }
        
        #endregion
    }
}