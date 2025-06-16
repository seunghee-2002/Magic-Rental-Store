using UnityEngine;

[CreateAssetMenu(fileName = "New Customer", menuName = "Game/Customer Data")]
public class CustomerData : ScriptableObject
{
    public string customerName; // 고객 이름
    public string desc; // 고객 설명
    public string request;
    public Element element; // 고객 속성
    public Sprite icon; // 고객 아이콘
    public Grade grade; // 고객 등급
    public int baseLevel; // 기본 레벨
    public int baseReward; // 기본 보상
}