using UnityEngine;
using System.Collections.Generic;

public class DayPhaseModel
{
    protected GameManager gameManager;

    public DayPhaseModel()
    {
        gameManager = GameManager.Instance;
    }

    // GameManager의 상태를 읽기 전용으로 접근
    public int Gold => gameManager.Gold;
    public int CurrentDay => gameManager.CurrentDay;
    public DayPhase CurrentPhase => gameManager.CurrentPhase;
    public List<WeaponInstance> WeaponInventory => gameManager.WeaponInventory;
    public List<HeroInstance> Heroes => gameManager.Heroes;
    public bool IsHeroSystemUnlocked => gameManager.IsHeroSystemUnlocked;

    // 골드 관련 메서드
    public bool SpendGold(int amount) => gameManager.SpendGold(amount);
    public void AddGold(int amount) => gameManager.AddGold(amount);

    // 무기 관련 메서드
    public void AddWeapon(WeaponData data, int amount) => gameManager.AddWeapon(data, amount);
    public bool UseWeapon(WeaponData data) => gameManager.UseWeapon(data);

    // 페이즈 관련 메서드
    public void AdvanceToNextPhase() => gameManager.AdvanceToNextPhase();

    // 영웅 시스템 관련 메서드
    public bool TryUnlockHeroSystem() => gameManager.TryUnlockHeroSystem();
    public void AddHero(HeroInstance hero) => gameManager.AddHero(hero);
} 