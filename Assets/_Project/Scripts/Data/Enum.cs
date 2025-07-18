using System;

namespace MagicRentalShop
{
    /// <summary>
    /// 게임 시간대 구분
    /// </summary>
    [Serializable]
    public enum GamePhase
    {
        Morning,
        Day,
        Night
    }
    
    /// <summary>
    /// 아이템 등급
    /// </summary>
    [Serializable]
    public enum Grade
    {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
        Epic = 3,
        Legendary = 4
    }

    /// <summary>
    /// 속성
    /// </summary>
    [Serializable]
    public enum Element
    { 
        None,
        Fire,
        Water,
        Thunder,
        Earth,
        Air,
        Ice,
        Dark,
        Light
    }

    /// <summary>
    /// 인벤토리 모드
    /// </summary>
    [Serializable]
    public enum InventoryMode
    {
        Normal,      // 기본 모드 (정보 확인만)
        Selection,   // 선택 모드 (무기 장착용)
        Management   // 관리 모드 (무기 판매용)
    }
}