using UnityEngine;

[System.Serializable]
public class CustomerInstance
{
    public CustomerData customerData; // 고객 데이터
    public int level; // 고객 레벨
    public DungeonInstance dungeon; // 고객이 가려는 던전
    public WeaponInstance weapon; // 고객이 대여한 무기
    public int reward; // 고객이 모험에서 얻는 보상

    public CustomerInstance(CustomerData data)
    {
        this.customerData = data;
        this.level = data.baseLevel;
        this.reward = data.baseReward;
        dungeon = SetDungeon();
        weapon = null;
    }

    DungeonInstance SetDungeon()
    {
        return null;
        // 던전 설정 로직 추가 예정
    }
}
