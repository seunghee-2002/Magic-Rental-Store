using UnityEngine;

[CreateAssetMenu(fileName = "NewCustomer", menuName = "Data/Customer")]
public class CustomerData : ScriptableObject
{
    public string customerName;// 고객 이름
    public string description; // 고객 설명
    public Grade grade; // 고객 등급
    public Element element; // 고객 속성
    public Sprite icon; // 고객 아이콘
    public GameObject prefab; // 고객 프리팹
}