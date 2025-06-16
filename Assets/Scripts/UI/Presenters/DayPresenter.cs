using UnityEngine;
using System.Collections.Generic;

public class DayPresenter : DayPhasePresenter
{
    private new DayModel model;
    private new DayView view;

    public new DayModel Model => model;

    public DayPresenter(DayView view, DayModel model) : base(model, view)
    {
        this.model = model;
        this.view = view;
        view.Initialize(this);
    }

    public override void OnPhaseStart()
    {
        base.OnPhaseStart();
        view.ShowDayUI();
        UpdateUI();
    }

    public override void OnPhaseEnd()
    {
        base.OnPhaseEnd();
    }

    protected override void UpdateUI()
    {
        base.UpdateUI();  // 부모 클래스의 기본 UI 업데이트 실행
        view.UpdateCustomerList(model.Customers);
    }

    public void OnHeroMenuClicked()
    {
        view.ShowHeroMenu();
    }

    public void OnHeroRosterClicked()
    {
        view.ShowHeroRoster();
    }

    public void OnHeroCreationClicked()
    {
        view.ShowHeroCreation();
    }

    public void OnHeroRosterCancelClicked()
    {
        view.ShowDayUI();
    }

    public void OnHeroCreationCancelClicked()
    {
        view.ShowDayUI();
    }

    public void OnCustomerInfoClicked(CustomerInstance customer)
    {
        view.ShowCustomerInfo(customer);
    }

    public void OnCustomerInfoCancelClicked()
    {
        view.ShowDayUI();
    }

    public void OnWeaponSelectionClicked(CustomerInstance customer)
    {
        view.ShowWeaponSelection(customer);
    }

    public void OnWeaponSelectionCancelClicked()
    {
        view.ShowDayUI();
    }

    public void OnDungeonInfoClicked(CustomerInstance customer)
    {
        view.ShowDungeonInfo(customer);
    }

    public void OnDungeonInfoCancelClicked()
    {
        view.ShowDayUI();
    }

    public void ShowDayUI()
    {
        view.ShowDayUI();
    }
} 