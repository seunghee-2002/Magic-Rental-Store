using UnityEngine;
using System;

[System.Serializable]
public class HeroInstance
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Element Element { get; private set; }
    public Sprite Icon { get; private set; }
    public Grade Grade { get; private set; }
    public int Level { get; private set; }
    public int DungeonCount { get; private set; }
    public WeaponData EquippedWeapon { get; private set; }

    public HeroInstance(string name, string description, Element element, Sprite icon, Grade grade)
    {
        Name = name;
        Description = description;
        Element = element;
        Icon = icon;
        Grade = grade;
        Level = 1;
        DungeonCount = 0;
        EquippedWeapon = null;
    }

    public void EquipWeapon(WeaponData weapon)
    {
        EquippedWeapon = weapon;
    }

    public void UnequipWeapon()
    {
        EquippedWeapon = null;
    }

    public void CompleteDungeon()
    {
        DungeonCount++;
        // TODO: 던전 수에 따른 레벨업 로직 구현
        // 예: 3번의 던전마다 레벨업
        if (DungeonCount % 3 == 0)
        {
            Level++;
        }
    }
} 