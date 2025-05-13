using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Hero
{
    public string heroName;       // 용사 이름
    public string description;    // 용사 설명
    public Grade grade;          // 용사 등급
    public Element element;      // 용사 속성
    public Sprite icon;          // 용사 아이콘
}

public class HeroManager : MonoBehaviour
{
    public static HeroManager Instance;

    [Header("고용된 용사 명단")]
    public List<Hero> heroRoster = new List<Hero>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    float[] GetGradeWeights(int cost) // 등급 가중치 계산
    {
        // cost 에 따라 가중치 배율을 조절
        // 예: cost가 높을수록 레어 이상 확률 상승
        float baseCommon   = Mathf.Max(0, 1000 - cost) / 1000f;   // cost가 작으면 common 우선
        float baseUncommon = Mathf.Clamp(cost / 1000f, 0f, 0.5f);
        float baseRare     = Mathf.Clamp((cost - 1000) / 2000f, 0f, 0.3f);
        float baseEpic     = Mathf.Clamp((cost - 2000) / 3000f, 0f, 0.15f);
        float baseLegend   = Mathf.Clamp((cost - 4000) / 5000f, 0f, 0.05f);

        // 총합 1.0 이 되도록 정규화
        float sum = baseCommon + baseUncommon + baseRare + baseEpic + baseLegend;
        return new float[] {
            baseCommon   / sum,
            baseUncommon / sum,
            baseRare     / sum,
            baseEpic     / sum,
            baseLegend   / sum
        };
    }

    Grade RollRandomGrade(int cost) // 랜덤 등급 결정
    {
        float[] weights = GetGradeWeights(cost);
        // Grade enum 순서가 Common, Uncommon, Rare, Epic, Legendary 이어야 합니다.
        float r = Random.value;
        float cumulative = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (r <= cumulative)
                return (Grade)i;
        }
        return Grade.Common;
    }

    
    public void CreateHero(string name, string desc, Element element, Sprite icon, int cost) // 용사 제작
    {
        if (!GameManager.Instance.SpendGold(cost)) return;

        Grade rolledGrade = RollRandomGrade(cost);

        Hero newHero = new Hero {
            heroName  = name,
            description = desc,
            grade     = rolledGrade,
            element   = element,
            icon      = icon
        };

        heroRoster.Add(newHero);
        UIManager.Instance.DisplayResult($"용사 {name} (등급: {rolledGrade})를 제작했습니다!");
        UIManager.Instance.UpdateHeroUI();
    }
}
