using UnityEngine;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 고객의 런타임 인스턴스 클래스
    /// StaticData(CustomerData)와 동적 정보(레벨, 던전 배정)를 결합
    /// </summary>
    [System.Serializable]
    public class CustomerInstance
    {
        [Header("정적 데이터 참조")]
        [Tooltip("원본 고객 데이터 참조")]
        public CustomerData data;
        
        [Header("인스턴스 고유 정보")]
        [Tooltip("이 고객 인스턴스의 고유 식별자")]
        public string instanceID;
        
        [Header("동적 속성")]
        [Tooltip("고객의 현재 레벨")]
        public int level;

        [Tooltip("대여한 무기")]
        public WeaponInstance rentedWeapon;
        
        [Tooltip("고객에게 배정된 던전")]
        public DungeonData assignedDungeon;

        [Header("Hero 시스템")]
        [Tooltip("Hero 상태 여부")]
        public bool isHero = false;
        
        [Tooltip("Hero로 전환된 날짜")]
        public int heroConvertedDay = 0;

        /// <summary>
        /// 생성자 - CustomerData로부터 인스턴스 생성
        /// </summary>
        public CustomerInstance(CustomerData customerData, int customerLevel, DungeonData dungeon)
        {
            data = customerData;
            instanceID = System.Guid.NewGuid().ToString();
            level = customerLevel;
            assignedDungeon = dungeon;
            rentedWeapon = null;
        }
        
        /// <summary>
        /// 기본 생성자 (JSON 역직렬화용)
        /// </summary>
        public CustomerInstance()
        {
            instanceID = System.Guid.NewGuid().ToString();
            level = 1;
        }

        /// <summary>
        /// Customer를 Hero로 전환
        /// </summary>
        public void ConvertToHero(int currentDay)
        {
            isHero = true;
            heroConvertedDay = currentDay;
            Debug.Log($"{GetDisplayName()} converted to Hero on day {currentDay}!");
        }

        /// <summary>
        /// Hero 상태인지 확인
        /// </summary>
        public bool IsHero()
        {
            return isHero;
        }

        /// <summary>
        /// 고객 이름 반환
        /// </summary>
        public string GetDisplayName()
        {
            string baseName = data?.customerName ?? "Unknown Customer";
            return isHero ? $"{baseName}★" : baseName;
        }
        
        /// <summary>
        /// 고객 아이콘 반환
        /// </summary>
        public Sprite GetIcon()
        {
            return data?.customerIcon;
        }

        /// <summary>
        /// 고객 레벨 반환
        /// </summary>
        public int GetLevel()
        {
            return level <= 0 ? 1 : level;
        }
        
        /// <summary>
        /// 고객 등급 반환
        /// </summary>
        public Grade GetGrade()
        {
            return data?.grade ?? Grade.Common;
        }
        
        /// <summary>
        /// 고객 속성 반환
        /// </summary>
        public Element GetElement()
        {
            return data?.element ?? Element.None;
        }
        
        /// <summary>
        /// 고객 설명 반환
        /// </summary>
        public string GetDescription()
        {
            return data?.description ?? "";
        }
        
        /// <summary>
        /// 배정된 던전 이름 반환
        /// </summary>
        public string GetAssignedDungeonName()
        {
            return assignedDungeon?.dungeonName ?? "No Dungeon";
        }

        /// <summary>
        /// 배정된 던전 아이콘 반환
        /// </summary>
        public Sprite GetAssignedDungeonIcon()
        {
            return assignedDungeon?.icon ?? null;
        }
        
        /// <summary>
        /// 고객이 유효한지 확인
        /// </summary>
        public bool IsValid()
        {
            return data != null && !string.IsNullOrEmpty(instanceID) && assignedDungeon != null;
        }
        
        /// <summary>
        /// 디버그용 문자열 표현
        /// </summary>
        public override string ToString()
        {
            if (data == null) return $"CustomerInstance [Invalid] ID: {instanceID}";
            
            return $"CustomerInstance [{GetDisplayName()}] " +
                   $"Lv.{level} | Grade: {GetGrade()} | Element: {GetElement()} | " +
                   $"Dungeon: {GetAssignedDungeonName()} | ID: {instanceID}";
        }
    }
    
    /// <summary>
    /// 저장/로드용 CustomerInstance 데이터
    /// Unity가 직렬화할 수 없는 데이터를 ID로 변환하여 저장
    /// </summary>
    [System.Serializable]
    public class CustomerInstanceSaveData
    {
        public string staticDataID;      // CustomerData의 ID
        public string instanceID;        // 인스턴스 고유 ID
        public int level;                // 고객 레벨
        public string assignedDungeonID; // 배정된 던전 ID

        public bool isHero;             // Hero 상태 여부   
        public int heroConvertedDay;    // Hero로 전환된 날

        /// <summary>
        /// CustomerInstance에서 저장 데이터 생성
        /// </summary>
        public CustomerInstanceSaveData(CustomerInstance customer)
        {
            staticDataID = customer.data?.id ?? "";
            instanceID = customer.instanceID;
            level = customer.level;
            assignedDungeonID = customer.assignedDungeon?.id ?? "";

            isHero = customer.isHero;
            heroConvertedDay = customer.heroConvertedDay;
        }
        
        /// <summary>
        /// JSON 역직렬화용 기본 생성자
        /// </summary>
        public CustomerInstanceSaveData() { }
        
        /// <summary>
        /// 저장 데이터에서 CustomerInstance 복원
        /// DataManager를 통해 CustomerData, DungeonData 참조 복원
        /// </summary>
        public CustomerInstance ToCustomerInstance(CustomerData customerData, DungeonData dungeonData)
        {
            var customer = new CustomerInstance
            {
                data = customerData,
                instanceID = instanceID,
                level = level,
                assignedDungeon = dungeonData,
                isHero = isHero,
                heroConvertedDay = heroConvertedDay
            };
            
            return customer;
        }
    }
}