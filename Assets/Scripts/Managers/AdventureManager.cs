using UnityEngine;
using System.Collections.Generic;

public class AdventureManager : MonoBehaviour
{
    public static AdventureManager Instance { get; private set; }

    [Header("Adventure Settings")]
    public int baseAdventureDays = 3;
    public float successRateMultiplier = 1.0f;

    private List<Adventure> activeAdventures = new List<Adventure>();
    private GameManager gameManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            gameManager = GameManager.Instance;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartAdventure(CustomerInstance customer)
    {
        if (customer == null || customer.weapon == null) return;

        // 모험 정보 생성
        Adventure adventure = new Adventure
        {
            customer = customer,
            startDay = gameManager.CurrentDay,
            remainingDays = CalculateAdventureDays(customer),
            isCompleted = false
        };

        activeAdventures.Add(adventure);
    }

    public void ProcessDayEnd()
    {
        List<DayResult> dayResults = new List<DayResult>();

        // 각 모험의 진행 상태 업데이트
        for (int i = activeAdventures.Count - 1; i >= 0; i--)
        {
            Adventure adventure = activeAdventures[i];
            adventure.remainingDays--;

            if (adventure.remainingDays <= 0)
            {
                // 모험 완료 처리
                bool isSuccess = CalculateSuccess(adventure);
                DayResult result = new DayResult
                {
                    customerName = adventure.customer.customerData.customerName,
                    dungeonName = adventure.customer.dungeon.dungeonData.dungeonName,
                    isSuccess = isSuccess,
                    reward = CalculateReward(adventure, isSuccess)
                };

                dayResults.Add(result);
                activeAdventures.RemoveAt(i);
            }
        }

        // 결과를 GameManager에 전달
        foreach (var result in dayResults)
        {
            gameManager.AddDayResult(result);
            if (result.isSuccess)
            {
                gameManager.AddGold(result.reward);
            }
        }
    }

    public List<Adventure> GetActiveAdventures()
    {
        return activeAdventures;
    }

    public bool IsCustomerOnAdventure(CustomerInstance customer)
    {
        return activeAdventures.Exists(a => a.customer == customer);
    }

    private int CalculateAdventureDays(CustomerInstance customer)
    {
        // 기본 일수에 고객 레벨과 던전 난이도를 반영
        float levelMultiplier = 1.0f + (customer.level * 0.1f);
        float dungeonMultiplier = 1.0f + ((int)customer.dungeon.dungeonData.grade * 0.2f);
        return Mathf.CeilToInt(baseAdventureDays * levelMultiplier * dungeonMultiplier);
    }

    private bool CalculateSuccess(Adventure adventure)
    {
        // 기본 성공률 계산
        float baseSuccessRate = gameManager.baseSuccessRate;
        
        // 고객 레벨과 무기 등급에 따른 보정
        float levelBonus = adventure.customer.level * 0.05f;
        float weaponBonus = ((int)adventure.customer.weapon.data.grade * 0.1f);
        
        // 최종 성공률 계산
        float finalSuccessRate = (baseSuccessRate + levelBonus + weaponBonus) * successRateMultiplier;
        finalSuccessRate = Mathf.Clamp(finalSuccessRate, 0f, 0.95f); // 최대 95%로 제한

        return Random.value <= finalSuccessRate;
    }

    private int CalculateReward(Adventure adventure, bool isSuccess)
    {
        if (!isSuccess) return 0;

        // 기본 보상 계산
        int baseReward = 1000;
        
        // 고객 레벨과 던전 등급에 따른 보상 증가
        float levelMultiplier = 1.0f + (adventure.customer.level * 0.2f);
        float dungeonMultiplier = 1.0f + ((int)adventure.customer.dungeon.dungeonData.grade * 0.3f);
        
        return Mathf.RoundToInt(baseReward * levelMultiplier * dungeonMultiplier);
    }

    public class Adventure
    {
        public CustomerInstance customer;
        public int startDay;
        public int remainingDays;
        public bool isCompleted;
    }
} 