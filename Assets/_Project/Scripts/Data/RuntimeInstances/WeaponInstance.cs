using System;
using UnityEngine;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 무기의 런타임 인스턴스 클래스
    /// StaticData(WeaponData)와 동적 정보(강화 레벨, 고유 ID)를 결합
    /// </summary>
    [System.Serializable]
    public class WeaponInstance
    {
        [Header("정적 데이터 참조")]
        [Tooltip("원본 무기 데이터 참조")]
        public WeaponData data;
        
        [Header("인스턴스 고유 정보")]
        [Tooltip("이 무기 인스턴스의 고유 식별자")]
        public string instanceID;
        
        [Header("동적 속성")]
        [Tooltip("무기가 현재 대여 중인지 여부")]
        public bool isRented = false;
        
        [Tooltip("대여 중인 경우 대여자 ID")]
        public string renterID = "";
        
        [Tooltip("총 대여 횟수 (사용 이력)")]
        public int rentalCount = 0;

        /// <summary>
        /// 생성자 - WeaponData로부터 인스턴스 생성
        /// </summary>
        public WeaponInstance(WeaponData weaponData)
        {
            data = weaponData;
            instanceID = System.Guid.NewGuid().ToString();
            isRented = false;
            renterID = "";
            rentalCount = 0;
        }
        
        /// <summary>
        /// 기본 생성자 (JSON 역직렬화용)
        /// </summary>
        public WeaponInstance()
        {
            instanceID = System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 무기의 기본 가격 반환
        /// </summary>
        public int GetPrice()
        {
            return data?.basePrice ?? 0;
        }
        
        /// <summary>
        /// 무기 판매 가격 계산 (GameConfig.weaponSellRate 반영)
        /// </summary>
        public int GetSellPrice(float sellRate = 0.5f)
        {
            return Mathf.RoundToInt(GetPrice() * sellRate);
        }
        
        /// <summary>
        /// 무기 이름 반환
        /// </summary>
        public string GetDisplayName()
        {
            return data?.weaponName ?? "Unknown Weapon";
        }
        
        /// <summary>
        /// 무기를 대여 상태로 설정
        /// </summary>
        public void SetRented(string renterInstanceID)
        {
            isRented = true;
            renterID = renterInstanceID;
            rentalCount++; // 대여 횟수 증가
        }
        
        /// <summary>
        /// 무기 대여 해제
        /// </summary>
        public void SetReturned()
        {
            isRented = false;
            renterID = "";
        }
        
        /// <summary>
        /// 디버그용 문자열 표현
        /// </summary>
        public override string ToString()
        {
            if (data == null) return $"WeaponInstance [Invalid] ID: {instanceID}";
            
            return $"WeaponInstance [{GetDisplayName()}] " +
                   $"Grade: {data.grade} | Element: {data.element} | " +
                   $"Price: {GetPrice()}G | Rentals: {rentalCount} | " +
                   $"Rented: {isRented} | ID: {instanceID}";
        }
    }
    
    /// <summary>
    /// 저장/로드용 WeaponInstance 데이터
    /// Unity가 직렬화할 수 없는 데이터를 ID로 변환하여 저장
    /// </summary>
    [System.Serializable]
    public class WeaponInstanceSaveData
    {
        public string staticDataID;      // WeaponData의 ID
        public string instanceID;        // 인스턴스 고유 ID
        public bool isRented;            // 대여 상태
        public string renterID;          // 대여자 ID
        public int rentalCount;          // 총 대여 횟수
        
        /// <summary>
        /// WeaponInstance에서 저장 데이터 생성
        /// </summary>
        public WeaponInstanceSaveData(WeaponInstance weapon)
        {
            staticDataID = weapon.data?.id ?? "";
            instanceID = weapon.instanceID;
            isRented = weapon.isRented;
            renterID = weapon.renterID;
            rentalCount = weapon.rentalCount;
        }
        
        /// <summary>
        /// JSON 역직렬화용 기본 생성자
        /// </summary>
        public WeaponInstanceSaveData() { }
        
        /// <summary>
        /// 저장 데이터에서 WeaponInstance 복원
        /// DataManager를 통해 WeaponData 참조 복원
        /// </summary>
        public WeaponInstance ToWeaponInstance(WeaponData weaponData)
        {
            var weapon = new WeaponInstance
            {
                data = weaponData,
                instanceID = instanceID,
                isRented = isRented,
                renterID = renterID,
                rentalCount = rentalCount
            };
            
            return weapon;
        }
    }
}