using UnityEngine;
using System.Collections.Generic;

public class NightPresenter : DayPhasePresenter
{
    private new NightModel model;
    private new NightView view;

    public new NightModel Model => model;

    public NightPresenter(NightView view, NightModel model) : base(model, view)
    {
        this.model = model;
        this.view = view;
        view.Initialize(this);
    }

    public override void OnPhaseStart()
    {
        base.OnPhaseStart();
        view.ShowNightUI();
        UpdateUI();
    }

    public override void OnPhaseEnd()
    {
        base.OnPhaseEnd();
        model.ClearDayResults();  // 완료된 모험 결과만 초기화
    }

    public List<DayResult> GetDayResults()
    {
        return model.DayResults;
    }

    public void ShowNightUI(List<DayResult> dayResults)
    {
        model.SetDayResults(dayResults);
        view.ShowNightUI();
        UpdateUI();
    }

    protected override void UpdateUI()
    {
        base.UpdateUI();  // 부모 클래스의 기본 UI 업데이트 실행

        // 진행 중인 모험 표시
        view.ClearResultList();
        List<AdventureManager.Adventure> activeAdventures = AdventureManager.Instance.GetActiveAdventures();
        model.SetActiveAdventures(activeAdventures);
        
        foreach (var adventure in activeAdventures)
        {
            view.AddResultItem(
                adventure.customer.customerData.customerName,
                adventure.customer.dungeon.dungeonData.dungeonName,
                adventure.remainingDays,
                adventure.customer.customerData.icon
            );
        }

        // 완료된 모험 결과 표시
        List<DayResult> dayResults = model.DayResults;
        if (dayResults != null && dayResults.Count > 0)
        {
            view.ShowResults(dayResults);
        }
    }

    // DayResults가 변경될 때 호출되는 메서드
    public void OnDayResultsUpdated()
    {
        UpdateUI();
    }
} 