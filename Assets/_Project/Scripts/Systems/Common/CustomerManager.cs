using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MagicRentalShop.Data;
using MagicRentalShop.Core;

namespace MagicRentalShop.Systems
{
    /// <summary>
    /// 고객 생성 및 관리를 담당하는 시스템
    /// 고객 레벨 계산, 던전 배정, 일일 고객 생성 등을 처리
    /// </summary>
    public class CustomerManager : MonoBehaviour
    {
        [Header("매니저 참조")]
        [SerializeField] private DataManager dataManager;
        [SerializeField] private GameConfig gameConfig;
        
        [Header("현재 상태")]
        [SerializeField] private List<CustomerInstance> visitingCustomers = new List<CustomerInstance>();
        
        [Header("레벨 계산 설정")]
        [SerializeField] private float baseLevelMultiplier = 0.2f; // 일수당 레벨 증가율
        [SerializeField] private int[] gradeLevelBonus = {0, 2, 5, 10, 20}; // 등급별 레벨 보너스

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
            // 매니저 참조 설정
            if (dataManager == null)
                dataManager = DataManager.Instance;
                
            if (gameConfig == null)
                gameConfig = Resources.Load<GameConfig>("GameConfig");
        }

        /// <summary>
        /// 오늘 방문할 고객들 생성
        /// </summary>
        public List<CustomerInstance> RequestCustomers(int currentDay)
        {
            visitingCustomers.Clear();
            
            int customerCount = gameConfig?.dailyCustomerCount ?? 6;
            
            for (int i = 0; i < customerCount; i++)
            {
                var customer = GenerateCustomer(currentDay);
                if (customer != null && customer.IsValid())
                {
                    visitingCustomers.Add(customer);
                }
            }
            
            Debug.Log($"Generated {visitingCustomers.Count} customers for day {currentDay}");
            return new List<CustomerInstance>(visitingCustomers);
        }

        /// <summary>
        /// 개별 고객 생성 (레벨 계산 + 던전 배정)
        /// </summary>
        public CustomerInstance GenerateCustomer(int currentDay)
        {
            // 1. 등급별 확률로 고객 선택
            var customerData = SelectRandomCustomer();
            if (customerData == null) return null;

            // 2. 레벨 계산
            int customerLevel = CalculateCustomerLevel(currentDay, customerData.grade);

            // 3. 던전 배정
            var assignedDungeon = AssignDungeon(customerData.grade);
            if (assignedDungeon == null) return null;

            // 4. CustomerInstance 생성
            return new CustomerInstance(customerData, customerLevel, assignedDungeon);
        }

        /// <summary>
        /// 등급별 확률로 랜덤 고객 선택
        /// </summary>
        private CustomerData SelectRandomCustomer()
        {
            if (dataManager == null) return null;

            var allCustomers = dataManager.GetAllCustomerData();
            if (allCustomers.Count == 0) return null;

            // GameConfig의 등급별 생성 확률 사용
            var gradeRates = gameConfig?.gradeSpawnRates ?? new float[]{0.75f, 0.14f, 0.07f, 0.03f, 0.01f};
            
            Grade selectedGrade = SelectGradeByProbability(gradeRates);
            
            // 선택된 등급의 고객들 중에서 랜덤 선택
            var customersOfGrade = allCustomers.Where(c => c.grade == selectedGrade).ToList();
            
            if (customersOfGrade.Count == 0)
            {
                // 해당 등급이 없으면 전체에서 랜덤
                return allCustomers[Random.Range(0, allCustomers.Count)];
            }
            
            return customersOfGrade[Random.Range(0, customersOfGrade.Count)];
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
            
            // 등급별 보너스 추가
            int gradeBonus = gradeLevelBonus[(int)customerGrade];
            
            // 최종 레벨 계산 (최소 1레벨)
            int finalLevel = Mathf.Max(1, Mathf.RoundToInt(baseLevel) + gradeBonus);
            
            // 최대 레벨 제한
            int maxLevel = gameConfig?.customerMaxLevel ?? 100;
            return Mathf.Min(finalLevel, maxLevel);
        }

        /// <summary>
        /// 고객 등급에 따른 던전 배정
        /// </summary>
        private DungeonData AssignDungeon(Grade customerGrade)
        {
            if (dataManager == null) return null;

            // 1. GameConfig에서 던전 배정 확률 가져오기 (없으면 기본값 사용)
            float[] probabilities = GetDungeonAssignmentRates(customerGrade);
            
            // 2. 확률로 던전 등급 선택
            Grade selectedDungeonGrade = SelectGradeByProbability(probabilities);
            
            // 3. 해당 등급의 던전들 중에서 랜덤 선택
            var dungeonsOfGrade = dataManager.GetDungeonsByGrade(selectedDungeonGrade);
            
            if (dungeonsOfGrade.Count == 0)
            {
                // 해당 등급 던전이 없으면 Common 던전 배정
                dungeonsOfGrade = dataManager.GetDungeonsByGrade(Grade.Common);
            }
            
            if (dungeonsOfGrade.Count == 0) return null;
            
            return dungeonsOfGrade[Random.Range(0, dungeonsOfGrade.Count)];
        }

        /// <summary>
        /// 고객 등급별 던전 배정 확률 가져오기 (GameConfig 우선, 없으면 기본값)
        /// </summary>
        private float[] GetDungeonAssignmentRates(Grade customerGrade)
        {
            if (gameConfig != null && gameConfig.dungeonAssignmentRates != null && 
        gameConfig.dungeonAssignmentRates.Length > (int)customerGrade)
    {
        return gameConfig.dungeonAssignmentRates[(int)customerGrade].rates;
    }

            // 기본값 (임시)
            switch (customerGrade)
            {
                case Grade.Common:
                    return new float[]{0.70f, 0.20f, 0.08f, 0.02f, 0.00f};
                case Grade.Uncommon:
                    return new float[]{0.50f, 0.30f, 0.15f, 0.05f, 0.00f};
                case Grade.Rare:
                    return new float[]{0.30f, 0.35f, 0.25f, 0.08f, 0.02f};
                case Grade.Epic:
                    return new float[]{0.15f, 0.25f, 0.35f, 0.20f, 0.05f};
                case Grade.Legendary:
                    return new float[]{0.05f, 0.15f, 0.30f, 0.35f, 0.15f};
                default:
                    return new float[]{0.70f, 0.20f, 0.08f, 0.02f, 0.00f};
            }
        }

        /// <summary>
        /// Hero 전환 시 Customer 풀에서 제거
        /// </summary>
        public void RemoveFromPool(CustomerInstance customer)
        {
            if (customer == null) return;
            
            visitingCustomers.Remove(customer);
            Debug.Log($"Removed customer {customer.GetDisplayName()} from pool (converted to Hero)");
        }

        /// <summary>
        /// 현재 방문 중인 고객 목록 반환
        /// </summary>
        public List<CustomerInstance> GetVisitingCustomers()
        {
            return new List<CustomerInstance>(visitingCustomers);
        }

        /// <summary>
        /// 특정 인스턴스 ID로 고객 찾기
        /// </summary>
        public CustomerInstance GetCustomerByInstanceID(string instanceID)
        {
            return visitingCustomers.FirstOrDefault(c => c.instanceID == instanceID);
        }

        /// <summary>
        /// 방문 고객 목록 복원 (저장/로드용)
        /// </summary>
        public void RestoreCustomers(List<CustomerInstance> customers)
        {
            visitingCustomers = customers ?? new List<CustomerInstance>();
            Debug.Log($"Restored {visitingCustomers.Count} visiting customers");
        }

        /// <summary>
        /// 현재 상태를 저장용 데이터로 변환
        /// </summary>
        public List<CustomerInstanceSaveData> GetSaveData()
        {
            return visitingCustomers.Select(c => new CustomerInstanceSaveData(c)).ToList();
        }

        /// <summary>
        /// 디버그용 정보 출력
        /// </summary>
        [ContextMenu("Debug Customer Info")]
        private void DebugCustomerInfo()
        {
            Debug.Log($"=== Customer Manager Debug Info ===");
            Debug.Log($"Total visiting customers: {visitingCustomers.Count}");
            
            foreach (var customer in visitingCustomers)
            {
                Debug.Log(customer.ToString());
            }
        }
    }
}