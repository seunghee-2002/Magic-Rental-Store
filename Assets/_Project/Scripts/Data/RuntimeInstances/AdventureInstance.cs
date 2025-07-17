using UnityEngine;

namespace MagicRentalShop.Data
{   
    /// <summary>
    /// 모험 인스턴스 클래스
    /// </summary>
    [System.Serializable]
    public class AdventureInstance
    {
        public string customerInstanceID; // Customer 또는 Hero의 인스턴스 ID
        public string weaponInstanceID;   // 무기 인스턴스 ID
        public string dungeonID;         // 던전 ID
        public int remainingDays;        // 남은 모험 일수
        public bool isHero;              // Hero 모험인지 구분
        public int startDate;            // 모험 시작 날짜
        
        public AdventureInstance()
        {
            remainingDays = 3; // 기본 모험 기간
        }
        
        public bool IsComplete()
        {
            return remainingDays <= 0;
        }
        
        public void ProcessDay()
        {
            remainingDays = Mathf.Max(0, remainingDays - 1);
        }
    }
}