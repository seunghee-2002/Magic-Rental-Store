using System;
using UnityEngine;

using MagicRentalShop.Core;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 모험 진행 상황을 관리하는 런타임 인스턴스
    /// </summary>
    [System.Serializable]
    public class AdventureInstance
    {
        [Header("모험 참가자")]
        [Tooltip("모험에 참가하는 Customer 인스턴스")]
        public CustomerInstance customer;
        
        [Header("모험 정보")]        
        [Tooltip("모험 완료까지 남은 일수")]
        public int remainingDays;
        
        [Header("고유 식별자")]
        [Tooltip("모험 고유 ID")]
        public string adventureID;
        
        [Tooltip("모험 시작 날짜")]
        public int startDate;

        /// <summary>
        /// 모험 생성자
        /// </summary>
        public AdventureInstance(CustomerInstance customer, int duration)
        {
            this.customer = customer;
            this.remainingDays = duration;
            this.adventureID = GenerateAdventureID();
            this.startDate = GameController.Instance != null ? GameController.Instance.CurrentDay : 0;
        }

        /// <summary>
        /// 기본 생성자 (역직렬화용)
        /// </summary>
        public AdventureInstance()
        {
            adventureID = GenerateAdventureID();
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
        /// 모험 참가자 레벨 반환
        /// </summary>
        public int GetCustomerLevel() => customer?.level ?? 1;

        /// <summary>
        /// 모험 참가자 등급 반환
        /// </summary>
        public Grade GetCustomerGrade() => customer?.GetGrade() ?? Grade.Common;

        /// <summary>
        /// 모험 참가자 속성 반환
        /// </summary>
        public Element GetCustomerElement() => customer?.GetElement() ?? Element.None;

        /// <summary>
        /// 모험 참가자 아이콘 반환
        /// </summary>
        public Sprite GetCustomerIcon() => customer?.GetIcon() ?? null;

        /// <summary>
        /// 모험 참가자 설명 반환
        /// </summary>
        public string GetCustomerDescription() => customer?.GetDescription() ?? "no description";

        /// <summary>
        /// 모험 참가자 인스턴스 ID 반환 (저장/로드용)
        /// </summary>
        public string GetCustomerInstanceID() => customer?.instanceID ?? "";

        /// <summary>
        /// 던전 데이터 반환 (CustomerInstance에서 가져옴)
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
        public string GetWeaponName() => customer.rentedWeapon?.GetDisplayName() ?? "unknown Weapon";

        /// <summary>
        /// 무기 아이콘 반환
        /// </summary>
        public Sprite GetWeaponIcon() => customer.rentedWeapon?.GetIcon() ?? null;

        /// <summary>
        /// 하루 경과 처리
        /// </summary>
        public void ProcessDayPassed()
        {
            if (remainingDays > 0)
            {
                remainingDays--;
            }
        }

        /// <summary>
        /// 모험이 완료되었는지 확인
        /// </summary>
        public bool IsCompleted()
        {
            return remainingDays <= 0;
        }

        /// <summary>
        /// 모험 진행률 반환 (0.0 ~ 1.0)
        /// 총 기간은 AdventureManager에서 계산되므로 여기서는 단순 계산
        /// </summary>
        public float GetProgress()
        {
            if (remainingDays <= 0) return 1.0f;
            
            // 시작일부터 현재까지의 경과 일수 계산
            int currentDay = GameController.Instance != null ? GameController.Instance.CurrentDay : startDate;
            int elapsedDays = currentDay - startDate;
            int totalDays = elapsedDays + remainingDays;
            
            if (totalDays <= 0) return 1.0f;
            return Mathf.Clamp01((float)elapsedDays / totalDays);
        }

        /// <summary>
        /// 모험 데이터 유효성 검사
        /// </summary>
        public bool IsValid()
        {
            // Customer 유효성 검사
            bool hasValidCustomer = customer != null && customer.IsValid();
            
            // 던전은 CustomerInstance에서 관리되므로 Customer 유효성에 포함됨
            
            return hasValidCustomer;
        }

        /// <summary>
        /// 고유 모험 ID 생성
        /// </summary>
        private string GenerateAdventureID()
        {
            return "adventure_" + Guid.NewGuid().ToString("N")[..8];
        }

        /// <summary>
        /// 디버그용 문자열 표현
        /// </summary>
        public override string ToString()
        {
            string customerInfo = GetCustomerName();
            string dungeonInfo = GetDungeonName();
            string weaponInfo = GetWeaponName();
            
            return $"Adventure[{adventureID}] {customerInfo} -> {dungeonInfo} with {weaponInfo} ({remainingDays}일 남음)";
        }
    }

    /// <summary>
    /// 저장/로드용 AdventureInstance 데이터
    /// Unity가 직렬화할 수 없는 데이터를 ID로 변환하여 저장
    /// </summary>
    [System.Serializable]
    public class AdventureInstanceSaveData
    {
        [Tooltip("참가 Customer의 인스턴스 ID")]
        public string customerInstanceID;
        
        [Tooltip("대여한 무기의 인스턴스 ID")]
        public string weaponInstanceID;
        
        [Tooltip("모험 완료까지 남은 일수")]
        public int remainingDays;
        
        [Tooltip("모험 고유 ID")]
        public string adventureID;
        
        [Tooltip("모험 시작 날짜")]
        public int startDate;

        /// <summary>
        /// AdventureInstance에서 저장 데이터 생성
        /// </summary>
        public AdventureInstanceSaveData(AdventureInstance adventure)
        {
            customerInstanceID = adventure.GetCustomerInstanceID();
            remainingDays = adventure.remainingDays;
            adventureID = adventure.adventureID;
            startDate = adventure.startDate;
        }

        /// <summary>
        /// JSON 역직렬화용 기본 생성자
        /// </summary>
        public AdventureInstanceSaveData() { }

        /// <summary>
        /// 저장 데이터에서 AdventureInstance 복원
        /// </summary>
        public AdventureInstance ToAdventureInstance(CustomerInstance customer, WeaponInstance weapon)
        {
            var adventure = new AdventureInstance
            {
                customer = customer,
                remainingDays = remainingDays,
                adventureID = adventureID,
                startDate = startDate
            };

            return adventure;
        }
    }
}