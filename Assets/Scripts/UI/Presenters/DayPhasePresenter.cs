using UnityEngine;
using System.Collections.Generic;

public interface IDayPhasePresenter
{
    DayPhaseModel Model { get; }
    void UpdateView();
    void OnPhaseStart();
    void OnPhaseEnd();
}

public abstract class DayPhasePresenter : IDayPhasePresenter
{
    protected DayPhaseModel model;
    protected DayPhaseView view;

    public DayPhaseModel Model => model;

    protected DayPhasePresenter(DayPhaseModel model, DayPhaseView view)
    {
        this.model = model;
        this.view = view;
    }

    protected virtual void UpdateUI()
    {
        view.UpdateGold(model.Gold);
        view.UpdateDay(model.CurrentDay);
    }

    public void UpdateGold(int gold)
    {
        model.Gold = gold;
        view.UpdateGold(gold);
    }

    public void UpdateDay(int day)
    {
        model.CurrentDay = day;
        view.UpdateDay(day);
    }

    public virtual void UpdateView()
    {
        view.UpdateCustomerList(model.CurrentCustomers);
        view.UpdateInventoryUI();
    }

    public virtual void OnPhaseStart()
    {
        model.IsPanelActive = true;
        view.Show();
        UpdateView();
    }

    public virtual void OnPhaseEnd()
    {
        model.IsPanelActive = false;
        view.Hide();
    }
} 