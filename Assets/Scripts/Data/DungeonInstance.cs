using System.Collections.Generic;
using UnityEngine;

public class DungeonInstance : MonoBehaviour
{
    public DungeonData dungeonData; // 던전 데이터
    public int level; // 던전 레벨
    public int reward; // 던전 보상

    public string description => dungeonData?.desc ?? "던전 정보 없음";

    public DungeonInstance(Grade customerGrade, int customerLevel)
    {
        dungeonData = SetDungeonData(customerGrade, customerLevel);
        level = SetLevel(customerLevel);
        reward = SetReward();
    }

    DungeonData SetDungeonData(Grade customerGrade, int customerLevel)
    {
        Dictionary<int, float> gradeProbabilities = new Dictionary<int, float>
        {
            { -1 , 0.15f }, // 15% 고객의 등급보다 한 단계 낮은 등급의 던전
            { 0, 0.7f }, // 70% 고객의 등급과 같은 등급의 던전
            { +1, 0.10f }, // 10% 고객의 등급보다 한 단계 높은 등급의 던전
            { +2, 0.04f }, // 4% 고객의 등급보다 두 단계 높은 등급의 던전
            { +3, 0.01f } // 1% 고객의 등급보다 세 단계 높은 등급의 던전
        };

        float randomValue = Random.Range(0f, 1f);
        float cumulativeProbability = 0f;
        int selectedGradeOffset = 0;

        // 확률에 따라 등급 오프셋 결정
        foreach (var kvp in gradeProbabilities)
        {
            cumulativeProbability += kvp.Value;
            if (randomValue <= cumulativeProbability)
            {
                selectedGradeOffset = kvp.Key;
                break;
            }
        }

        // 고객 등급에 오프셋을 적용하여 최종 던전 등급 결정
        int finalGrade = (int)customerGrade + selectedGradeOffset;
        
        // DataManager를 통해 해당 등급의 랜덤 던전 가져오기
        return DataManager.Instance.GetRandomDungeonByGrade((Grade)finalGrade);
    }

    int SetLevel(int customerLevel)
    {
        int weight = Random.Range(-10, 10);
        return customerLevel + weight;
        // 레벨 추후 설정
    }

    int SetReward()
    {
        const int BASE_REWARD = 300; // 보상 추후 설정
        float rewardMultiplier = 1f;

        switch (dungeonData.grade)
        {
            case Grade.Common:
                rewardMultiplier = 1f;
                break;
            case Grade.Uncommon:
                rewardMultiplier = 1.2f;
                break;
            case Grade.Rare:
                rewardMultiplier = 1.5f;
                break;
            case Grade.Epic:
                rewardMultiplier = 2.5f;
                break;
            case Grade.Legendary:
                rewardMultiplier = 10f;
                break;
        }

        return Mathf.RoundToInt(BASE_REWARD * rewardMultiplier);
    }
}
