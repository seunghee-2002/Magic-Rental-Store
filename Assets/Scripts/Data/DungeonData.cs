using UnityEngine;

[CreateAssetMenu(fileName = "NewDungeon", menuName = "Data/Dungeon")]
public class DungeonData : ScriptableObject
{
    public string dungeonName; // 던전 이름
    public string desc; // 던전 설명
    public Sprite icon; // 던전 아이콘
    public Grade grade; // 던전 등급
    public Element element; // 던전 속성
    public int difficulty; // 보정용
}
