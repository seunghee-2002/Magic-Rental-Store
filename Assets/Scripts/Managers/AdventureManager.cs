using System.Collections.Generic;
using UnityEngine;

public class AdventureManager : MonoBehaviour
{
    public static AdventureManager Instance { get; private set; }

    [Header("Dungeon Lists")]
    [SerializeField] List<DungeonData> commonDungeons;
    [SerializeField] List<DungeonData> uncommonDungeons;
    [SerializeField] List<DungeonData> rareDungeons;
    [SerializeField] List<DungeonData> epicDungeons;
    [SerializeField] List<DungeonData> legendaryDungeons;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public (DungeonData dungeonData, int dungeonLevel) SetDungeon(Grade customerGrade, int customerLevel)
    {
        List<DungeonData>[] dungeonLists =
        {
            commonDungeons, uncommonDungeons, rareDungeons, epicDungeons, legendaryDungeons
        };

        (float threshold, int offset)[] gradeChances =
        {
            (0.03f, +2), // 3% 2단계 상승
            (0.10f, +1), // 7% 1단계 상승
            (0.25f, -1), // 15% 1단계 하락
            (0.30f, -2), // 5% 2단계 하락
            (1.00f,  0)  // 70% 등급 유지
        };

        int current = (int)customerGrade;
        float rand = Random.value;
        int newGradeIdx = current;

        foreach (var (threshold, offset) in gradeChances)
        {
            int candidate = current + offset;
            if (rand < threshold &&
                candidate >= (int)Grade.Common &&
                candidate <= (int)Grade.Legendary)
            {
                newGradeIdx = candidate;
                break;
            }
        }

        // 던전 레벨 범위 설정
        int minLevel, maxLevel;
        switch ((Grade)newGradeIdx)
        {
            case Grade.Common:
                minLevel = customerLevel - 5; maxLevel = customerLevel + 10; break;
            case Grade.Uncommon:
                minLevel = customerLevel - 5; maxLevel = customerLevel + 5; break;
            case Grade.Rare:
                minLevel = customerLevel - 5; maxLevel = customerLevel + 3; break;
            case Grade.Epic:
                minLevel = customerLevel - 7; maxLevel = customerLevel + 3; break;
            case Grade.Legendary:
                minLevel = customerLevel - 10; maxLevel = customerLevel; break;
            default:
                minLevel = 1; maxLevel = 1; break;
        }

        minLevel = Mathf.Clamp(minLevel, 1, 100);
        maxLevel = Mathf.Clamp(maxLevel, 1, 100);
        if (minLevel >= maxLevel) maxLevel = Mathf.Min(minLevel + 1, 100);

        int dungeonLevel = Random.Range(minLevel, maxLevel);

        List<DungeonData> dungeonList = dungeonLists[newGradeIdx];
        if (dungeonList == null || dungeonList.Count == 0) return (null, 1);

        int idx = Random.Range(0, dungeonList.Count);
        return (dungeonList[idx], dungeonLevel);
    }

    public DayResult SimulateRun(CustomerInstance customer)
    {
        float baseRate = customer.rentedWeapon == null
            ? 0
            : 0.5f + (customer.customerData.level - GameManager.Instance.playerLevel) * 0.05f;

        bool isSuccess = Random.value < Mathf.Clamp01(baseRate);
        int reward = isSuccess ? 300 : 0;

        if (isSuccess) GameManager.Instance.AddGold(reward);

        return new DayResult { customer = customer, isSuccess = isSuccess, reward = reward };
    }
}
