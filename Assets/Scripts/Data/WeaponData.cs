using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Data/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName; // 도구 이름
    public string description; // 도구 설명
    public int cost; // 도구 가격
    public Grade grade; // 도구 등급
    public Element element; // 도구 속성
    public Sprite icon; // 도구 아이콘
    public GameObject prefab; // 도구 프리팹
}   
