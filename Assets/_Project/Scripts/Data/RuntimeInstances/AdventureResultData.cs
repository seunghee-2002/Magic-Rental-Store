using System;
using UnityEngine;

using MagicRentalShop.Core;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 모험 완료 후 결과를 저장하는 데이터 클래스
    /// </summary>
    [System.Serializable]
    public class AdventureResultData
    {
        [Header("모험 참가자")]
        [Tooltip("모험에 참가한 Customer 인스턴스")]
        public CustomerInstance customer;
        
        [Header("모험 결과")]
        [Tooltip("모험 성공 여부")]
        public bool isSuccess;
        
        [Tooltip("무기 회수 여부 (실패 시에도 회수 가능)")]
        public bool isWeaponRecovered;
        
        [Tooltip("Hero 전환 여부 (Customer가 Hero로 전환되었는지)")]
        public bool heroConverted;
        
        [Header("보상")]
        [Tooltip("획득한 골드")]
        public int goldReward;
        
        [Tooltip("획득한 재료들")]
        public MaterialInstance[] materialRewards;
        
        [Header("고유 식별자")]
        [Tooltip("결과 데이터 고유 ID")]
        public string resultID;
        
        [Tooltip("모험 완료 날짜")]
        public int completionDate;

        /// <summary>
        /// 모험 결과 생성자
        /// </summary>
        public AdventureResultData(CustomerInstance customer, bool isSuccess, bool isWeaponRecovered, 
                                 int goldReward, MaterialInstance[] materialRewards = null)
        {
            this.customer = customer;
            this.isSuccess = isSuccess;
            this.isWeaponRecovered = isWeaponRecovered;
            this.goldReward = goldReward;
            this.materialRewards = materialRewards ?? new MaterialInstance[0];
            
            // Hero 관련 초기값
            this.heroConverted = false;
            
            // 기본 정보
            this.resultID = GenerateResultID();
            this.completionDate = GameController.Instance != null ? GameController.Instance.CurrentDay : 0;
        }

        /// <summary>
        /// 기본 생성자 (역직렬화용)
        /// </summary>
        public AdventureResultData()
        {
            resultID = GenerateResultID();
            materialRewards = new MaterialInstance[0];
        }

        /// <summary>
        /// Hero 전환 처리
        /// </summary>
        public void SetHeroConverted()
        {
            if (customer != null && !customer.IsHero())
            {
                heroConverted = true;
                Debug.Log($"{customer.GetDisplayName()} converted to Hero!");
            }
        }

        /// <summary>
        /// 모험 참가자가 Hero인지 확인
        /// </summary>
        public bool IsHero() => customer?.IsHero() ?? false;

        /// <summary>
        /// 모험 참가자 이름 반환
        /// </summary>
        public string GetCustomerName() => customer?.GetDisplayName() ?? "unknown";

        /// <summary>
        /// 모험 참가자 아이콘 반환
        /// </summary>
        public Sprite GetCustomerIcon() => customer?.GetIcon() ?? null;

        /// <summary>
        /// 모험 참가자 인스턴스 ID 반환
        /// </summary>
        public string GetCustomerInstanceID() => customer?.instanceID ?? "";

        /// <summary>
        /// 던전 데이터 반환
        /// </summary>
        public DungeonData GetDungeon() => customer?.assignedDungeon ?? null;

        /// <summary>
        /// 던전 이름 반환
        /// </summary>
        public string GetDungeonName() => customer?.GetAssignedDungeonName() ?? "unknown Dungeon";

        /// <summary>
        /// 던전 아이콘 반환
        /// </summary>
        public Sprite GetDungeonIcon() => customer?.GetAssignedDungeonIcon() ?? null;

        /// <summary>
        /// 무기 이름 반환
        /// </summary>
        public string GetWeaponName() => customer?.rentedWeapon?.GetDisplayName() ?? "unknown Weapon";

        /// <summary>
        /// 무기 아이콘 반환
        /// </summary>
        public Sprite GetWeaponIcon() => customer?.rentedWeapon?.GetIcon() ?? null;

        /// <summary>
        /// 총 재료 보상 개수 반환
        /// </summary>
        public int GetMaterialRewardCount() => materialRewards?.Length ?? 0;

        /// <summary>
        /// 결과 요약 문자열 반환 (UI 표시용)
        /// </summary>
        public string GetResultSummary()
        {
            string successText = isSuccess ? "성공" : "실패";
            string weaponText = isWeaponRecovered ? "회수" : "손실";
            string heroText = "";
            
            if (heroConverted)
            {
                heroText = " (Hero 전환!)";
            }
            
            return $"{successText} | 무기 {weaponText} | 골드 {goldReward}G{heroText}";
        }

        /// <summary>
        /// 데이터 유효성 검사
        /// </summary>
        public bool IsValid()
        {
            return customer != null && customer.IsValid();
        }

        /// <summary>
        /// 고유 결과 ID 생성
        /// </summary>
        private string GenerateResultID()
        {
            return "result_" + Guid.NewGuid().ToString("N")[..8];
        }

        /// <summary>
        /// 디버그용 문자열 표현
        /// </summary>
        public override string ToString()
        {
            return $"AdventureResult[{resultID}] {GetCustomerName()} -> {GetDungeonName()} | {GetResultSummary()}";
        }
    }

    /// <summary>
    /// 저장/로드용 AdventureResultData 데이터
    /// </summary>
    [System.Serializable]
    public class AdventureResultSaveData
    {
        [Tooltip("참가 Customer의 인스턴스 ID")]
        public string customerInstanceID;
        
        [Tooltip("모험 성공 여부")]
        public bool isSuccess;
        
        [Tooltip("무기 회수 여부")]
        public bool isWeaponRecovered;
        
        [Tooltip("Hero 전환 여부")]
        public bool heroConverted;
        
        [Tooltip("획득 골드")]
        public int goldReward;
        
        [Tooltip("획득 재료 ID들")]
        public string[] materialRewardIDs;
        
        [Tooltip("획득 재료 수량들")]
        public int[] materialRewardQuantities;
        
        [Tooltip("결과 고유 ID")]
        public string resultID;
        
        [Tooltip("모험 완료 날짜")]
        public int completionDate;

        /// <summary>
        /// AdventureResultData에서 저장 데이터 생성
        /// </summary>
        public AdventureResultSaveData(AdventureResultData result)
        {
            customerInstanceID = result.GetCustomerInstanceID();
            isSuccess = result.isSuccess;
            isWeaponRecovered = result.isWeaponRecovered;
            heroConverted = result.heroConverted;
            goldReward = result.goldReward;
            resultID = result.resultID;
            completionDate = result.completionDate;

            // 재료 보상 데이터 변환
            if (result.materialRewards != null && result.materialRewards.Length > 0)
            {
                materialRewardIDs = new string[result.materialRewards.Length];
                materialRewardQuantities = new int[result.materialRewards.Length];
                
                for (int i = 0; i < result.materialRewards.Length; i++)
                {
                    materialRewardIDs[i] = result.materialRewards[i]?.data?.id ?? "";
                    materialRewardQuantities[i] = result.materialRewards[i]?.quantity ?? 0;
                }
            }
            else
            {
                materialRewardIDs = new string[0];
                materialRewardQuantities = new int[0];
            }
        }

        /// <summary>
        /// JSON 역직렬화용 기본 생성자
        /// </summary>
        public AdventureResultSaveData() { }

        /// <summary>
        /// 저장 데이터에서 AdventureResultData 복원
        /// </summary>
        public AdventureResultData ToAdventureResultData(CustomerInstance customer, MaterialInstance[] materials)
        {
            var result = new AdventureResultData
            {
                customer = customer,
                isSuccess = isSuccess,
                isWeaponRecovered = isWeaponRecovered,
                heroConverted = heroConverted,
                goldReward = goldReward,
                materialRewards = materials ?? new MaterialInstance[0],
                resultID = resultID,
                completionDate = completionDate
            };

            return result;
        }
    }
}