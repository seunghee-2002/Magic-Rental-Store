namespace MagicRentalShop.Data
{
    /// <summary>
    /// Hero 컬렉션 데이터를 관리하는 런타임 인스턴스
    /// </summary>
    [System.Serializable]
    public class HeroCollectionData
    {
        public bool isAcquired;          // 획득 여부
        public int acquiredDay;          // 획득 날짜
        public bool shouldInherit;       // 게임오버 시 승계 여부
    }
}