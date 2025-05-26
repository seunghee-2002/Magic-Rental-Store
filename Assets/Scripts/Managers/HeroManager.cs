using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class HeroData
{
    public string heroName;
    public string description;
    public int level;
    public Grade grade;
    public Element element;
    public Sprite icon;
}

public class HeroManager : MonoBehaviour
{
    public static HeroManager Instance { get; private set; }

    [Header("고용된 용사 명단")]
    public List<HeroData> heroRoster = new List<HeroData>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    float[] GetGradeWeights(int cost)
    {
        float baseCommon = Mathf.Max(0, 1000 - cost) / 1000f;
        float baseUncommon = Mathf.Clamp(cost / 1000f, 0f, 0.5f);
        float baseRare = Mathf.Clamp((cost - 1000) / 2000f, 0f, 0.3f);
        float baseEpic = Mathf.Clamp((cost - 2000) / 3000f, 0f, 0.15f);
        float baseLegend = Mathf.Clamp((cost - 4000) / 5000f, 0f, 0.05f);

        float sum = baseCommon + baseUncommon + baseRare + baseEpic + baseLegend;
        return new float[] {
            baseCommon   / sum,
            baseUncommon / sum,
            baseRare     / sum,
            baseEpic     / sum,
            baseLegend   / sum
        };
    }

    Grade RollRandomGrade(int cost)
    {
        float[] weights = GetGradeWeights(cost);
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

    public void CreateHero(string name, string desc, Element element, Sprite icon, int cost)
    {
        if (!GameManager.Instance.SpendGold(cost)) return;

        Grade rolledGrade = RollRandomGrade(cost);

        HeroData newHero = new HeroData
        {
            heroName = name,
            description = desc,
            level = SetLevel(cost),
            grade = rolledGrade,
            element = element,
            icon = icon
        };

        heroRoster.Add(newHero);
        CommonUI.Instance.DisplayResult($"용사 {name} (등급: {rolledGrade})를 제작했습니다!");
        UIManager.Instance.dayUI.UpdateHeroUI();
    }

    int SetLevel(int cost)
    {
        int baseLevel = Mathf.Clamp(cost / 1000, 1, 99);
        return baseLevel + Random.Range(-1, 2);
    }
}
