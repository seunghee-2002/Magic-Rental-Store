using UnityEngine;

public enum Grade { Common, Uncommon, Rare, Epic, Legendary } // 아이템 등급 
public enum Element { Fire, Water, Earth, Air, Lightning, Ice, Light, Dark,} // 아이템 속성

[CreateAssetMenu(fileName = "New Item", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName; // 아이템 이름
    public string description; // 아이템 설명
    public Grade grade; // 아이템 등급
    public Element element; // 아이템 속성
    public Sprite icon; // 아이템 아이콘
    public GameObject prefab; // 아이템 프리팹
}   
