using UnityEngine;
using System.Collections.Generic;


public class NightModel : DayPhaseModel
{
    public List<DayResult> DayResults { get; private set; }
    public bool IsPanelActive { get; set; }

    public NightModel() : base()
    {
        DayResults = new List<DayResult>();
    }

    public void AddDayResult(DayResult result)
    {
        DayResults.Add(result);
        GameManager.Instance.NotifyDayResultsChanged();
    }

    public void ClearDayResults()
    {
        DayResults.Clear();
        GameManager.Instance.NotifyDayResultsChanged();
    }
}
