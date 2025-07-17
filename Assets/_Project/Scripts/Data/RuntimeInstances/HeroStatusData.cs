using UnityEngine;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// Hero 상태를 관리하는 데이터 클래스
    /// </summary>
    [System.Serializable]
    public class HeroStatusData
    {
        public bool isHero;
        public int heroConvertedDay;
        public InjuryType currentInjury;
        public int injuryStartDay;
        public int returnDay;
        
        // 게임오버 시 승계용 데이터
        public bool shouldInherit;
    }
}