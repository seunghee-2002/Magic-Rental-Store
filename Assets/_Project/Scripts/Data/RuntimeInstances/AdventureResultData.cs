using UnityEngine;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 모험 결과 데이터 클래스
    /// </summary>
    [System.Serializable]
    public class AdventureResultData
    {
        public string customerInstanceID; // Customer 또는 Hero의 인스턴스 ID
        public string weaponInstanceID;   // 무기 인스턴스 ID
        public string dungeonID;         // 던전 ID
        public bool isSuccess;           // 성공 여부
        public bool isWeaponRecovered;   // 무기 회수 여부
        public bool isHero;              // Hero 모험이었는지
        public bool heroConverted;       // Customer가 Hero로 전환되었는지
        public bool heroLevelUp;         // Hero가 레벨업했는지
        public int goldReward;           // 골드 보상
        public int completionDate;       // 완료 날짜
        
        public AdventureResultData()
        {
            isSuccess = false;
            isWeaponRecovered = false;
            isHero = false;
            heroConverted = false;
            heroLevelUp = false;
            goldReward = 0;
        }
    }
}
