using UnityEngine;

[CreateAssetMenu(fileName = "NewDungeon", menuName = "Data/Dungeon")]
public class DungeonData : ScriptableObject
{
    public string dungeonName; // 던전 이름
    public int recommendedLevel; // 추천 레벨
    public Element element; // 던전 속성
    public int difficulty; // 보정용
}
