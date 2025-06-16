using UnityEngine;
using System.Collections.Generic;

public class MorningPresenter : DayPhasePresenter
{
    private new MorningModel model;
    private new MorningView view;

    public new MorningModel Model => model;

    public MorningPresenter(MorningView view, MorningModel model) : base(model, view)
    {
        this.model = model;
        this.view = view;
        view.Initialize(this);
    }

    public override void OnPhaseStart()
    {
        base.OnPhaseStart();
        view.ShowMorningUI();
        UpdateUI();
    }

    public override void OnPhaseEnd()
    {
        base.OnPhaseEnd();
    }

    protected override void UpdateUI()
    {
        base.UpdateUI();  // 부모 클래스의 기본 UI 업데이트 실행
        view.UpdateBlacksmithUnlockButton(model.IsBlacksmithUnlocked, model.BlacksmithUnlockCost);
    }

    public void OnBlacksmithMenuClicked()
    {
        view.ShowBlacksmithMenu();
    }

    public void OnBlacksmithForgeClicked()
    {
        view.ShowBlacksmithForge();
    }

    public void OnBlacksmithRecipeClicked()
    {
        view.ShowBlacksmithRecipe();
    }

    public void OnBlacksmithForgeCancelClicked()
    {
        view.ShowMorningUI();
    }

    public void OnBlacksmithRecipeCancelClicked()
    {
        view.ShowMorningUI();
    }

    public void OnBlacksmithUnlockClicked()
    {
        if (model.TryUnlockBlacksmith())
        {
            UIManager.Instance.ShowMessage("대장장이 시스템이 해금되었습니다!");
            view.OnBlacksmithUnlocked();
        }
        else
        {
            UIManager.Instance.ShowMessage("골드가 부족합니다!");
        }
    }

    public void OnWeaponShopPanelClicked()  // 무기 상점 패널을 여는 메서드
    {
        view.ShowWeaponShop();
        UpdateWeaponShopUI();
    }

    public void OnWeaponShopCancelClicked()
    {
        view.ShowMorningUI();
    }

    private void UpdateWeaponShopUI()
    {
        // 무기 상점 UI 업데이트 로직
    }

    public void OnWeaponClicked(WeaponData weapon, int index)
    {
        // 무기 클릭 처리 로직
        OnForgeButtonClicked(index);
    }

    public void OnWeaponPurchaseClicked(WeaponData weapon, int index)  // 무기 구매 버튼 클릭 시
    {
        OnWeaponClicked(weapon, index);
    }

    public void ShowMorningUI()
    {
        view.ShowMorningUI();
    }
} 