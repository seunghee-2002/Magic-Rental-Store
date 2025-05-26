using UnityEngine;

[System.Serializable]
public class CustomerInstance
{
    public CustomerData customerData;
    public WeaponData rentedWeapon; // 대여된 도구 (null 가능)
    public DungeonData dungeonData; // 가려는 던전 정보
    public int dungeonLevel; // 던전 레벨
    // ...추가 정보 (ex. 등급, 이름, 등)
}
