using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Data Pool")]
    public List<WeaponData> testStartWeapons;
    [Header("Player State")]
    public int playerLevel = 1;
    public int gold = 1000;
    public int currentDay = 1;
    [Header("Game State")]
    public DayPhase currentPhase = DayPhase.Morning;
    [Header("Tax Settings")]
    public int taxAmount = 2000;
    int taxInterval = 7;
    [Header("Unlock Settings")]
    public int blacksmithUnlockGold = 5000;
    public int heroUnlockGold = 8000;
    [HideInInspector] public bool isBlacksmithUnlocked = false;
    [HideInInspector] public bool isHeroSystemUnlocked = false;

    [Range(0f, 1f)] public float baseSuccessRate = 0.5f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Invoke("GetTestWeapon", 0.1f);
        StartMorning();
    }

    void GetTestWeapon()
    {
        foreach (WeaponData weapon in testStartWeapons)
            InventoryManager.Instance.AddWeapon(weapon, Random.Range(1, 4));
    }

    public bool SpendGold(int amount)
    {
        if (gold < amount)
        {
            CommonUI.Instance.DisplayResult("골드가 부족합니다!");
            return false;
        }
        gold -= amount;
        CommonUI.Instance.UpdateGold(gold);
        return true;
    }

    public void AddGold(int amount)
    {
        gold += amount;
        CommonUI.Instance.UpdateGold(gold);
    }

    public void NextPhase()
    {
        switch (currentPhase)
        {
            case DayPhase.Morning: StartDay();   break;
            case DayPhase.Day:     StartNight(); break;
            case DayPhase.Night:
            {
                if (currentDay % taxInterval == 0)
                {
                    if (!SpendGold(taxAmount))
                    {
                        CommonUI.Instance.DisplayResult($"<color=red>월세 미납! 게임 오버</color>");
                        return;
                    }
                    CommonUI.Instance.DisplayResult($"<color=yellow>월세 {taxAmount}G 납부 완료</color>");
                }
                currentDay++;
                StartMorning();
                break;
            }
        }

        CommonUI.Instance.UpdatePhase(currentDay, currentPhase);
        CommonUI.Instance.UpdateInventoryUI();
    }

    void StartMorning()
    {
        currentPhase = DayPhase.Morning;
        WeaponShopManager.Instance.GenerateStock();
        UIManager.Instance.morningUI.GenerateWeaponShop();
        UIManager.Instance.morningUI.ShowMorningUI();
    }

    void StartDay()
    {
        currentPhase = DayPhase.Day;
        CustomerManager.Instance.SpawnCustomer();
    }

    void StartNight()
    {
        currentPhase = DayPhase.Night;
        CustomerManager.Instance.Result();
        CustomerManager.Instance.InitCustomer();
    }

    public void PurchaseHeroSystemUnlock()
    {
        if (!SpendGold(heroUnlockGold)) return;
        isHeroSystemUnlocked = true;
        CommonUI.Instance.DisplayResult("용사 제작 시스템이 해금되었습니다!");
        UIManager.Instance.dayUI.heroUnlockButton.gameObject.SetActive(false);
        UIManager.Instance.dayUI.OnHeroSystemUnlocked();
    }

    public void PurchaseBlacksmithSystemUnlock()
    {
        if (!SpendGold(blacksmithUnlockGold)) return;
        isBlacksmithUnlocked = true;
        CommonUI.Instance.DisplayResult("대장장이 시스템이 해금되었습니다!");
        UIManager.Instance.morningUI.blacksmithUnlockButton.gameObject.SetActive(false);
        UIManager.Instance.morningUI.OnBlacksmithUnlocked();
    }
}
