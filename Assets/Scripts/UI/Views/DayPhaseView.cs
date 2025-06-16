using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public interface IDayPhaseView
{
    void Initialize(DayPhaseModel commonModel, DayPhaseModel phaseModel);
    void Show();
    void Hide();
    void UpdateDay(int day);
    void UpdateGold(int gold);
    void UpdateCustomerList(List<CustomerInstance> customers);
    void UpdateInventoryUI();
    void OnNextPhaseClicked();
}

public abstract class DayPhaseView : MonoBehaviour, IDayPhaseView
{
    protected DayPhaseModel commonModel;
    protected DayPhaseModel phaseModel;
    protected CommonUIView commonUI;

    public virtual void Initialize(DayPhaseModel commonModel, DayPhaseModel phaseModel)
    {
        this.commonModel = commonModel;
        this.phaseModel = phaseModel;
        commonUI.Initialize(commonModel);
        SetupButtonListeners();
    }

    protected virtual void SetupButtonListeners()
    {
        // 기본 버튼 리스너 설정
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        UpdateUI();
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    protected virtual void UpdateUI()
    {
        // 각 View에서 구현
    }

    public virtual void UpdateDay(int day)
    {
        commonUI.UpdateDay(day, phaseModel.CurrentPhase);
    }

    public virtual void UpdateGold(int gold)
    {
        commonUI.UpdateGold(gold);
    }

    public virtual void UpdateCustomerList(List<CustomerInstance> customers)
    {
        // 자식 클래스에서 구현
    }

    public virtual void UpdateInventoryUI()
    {
        commonUI.UpdateInventoryUI();
    }

    public virtual void OnNextPhaseClicked()
    {
        DayPhase nextPhase = (DayPhase)(((int)phaseModel.CurrentPhase + 1) % 3);
        phaseModel.UpdatePhase(phaseModel.CurrentDay, nextPhase);
    }
} 