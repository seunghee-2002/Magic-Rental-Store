using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MorningView : DayPhaseView
{
    [Header("Panels")]
    public GameObject morningPanel;          // 아침 시간대 메인 패널
    public GameObject blacksmithMenuPanel;   // 대장장이 메뉴 패널
    public GameObject blacksmithForgePanel;  // 대장장이 제작 패널
    public GameObject blacksmithRecipePanel; // 대장장이 레시피 패널
    public GameObject weaponShopPanel;       // 무기 상점 패널

    [Header("Blacksmith")]
    public Button blacksmithUnlockButton;    // 대장장이 해금 버튼
    public Button blacksmithMenuButton;      // 대장장이 메뉴 버튼
    public Button blacksmithForgeButton;     // 대장장이 제작 버튼
    public Button blacksmithRecipeButton;    // 대장장이 레시피 버튼
    public Button blacksmithForgeCancelButton; // 대장장이 제작 취소 버튼
    public Button blacksmithRecipeCancelButton; // 대장장이 레시피 취소 버튼

    [Header("Weapon Shop")]
    public Transform weaponShopContent;
    public GameObject weaponButtonPrefab;
    public Button weaponShopButton;
    public Button weaponShopCancelButton;

    public override void Initialize(DayPhaseModel commonModel, DayPhaseModel phaseModel)
    {
        base.Initialize(commonModel, phaseModel);
        this.presenter = new MorningPresenter(this, (MorningModel)phaseModel);
    }

    protected override void SetupButtonListeners()
    {
        blacksmithUnlockButton.onClick.AddListener(() => presenter.OnBlacksmithUnlockClicked());
        blacksmithMenuButton.onClick.AddListener(() => presenter.OnBlacksmithMenuClicked());
        blacksmithForgeButton.onClick.AddListener(() => presenter.OnBlacksmithForgeClicked());
        blacksmithRecipeButton.onClick.AddListener(() => presenter.OnBlacksmithRecipeClicked());
        blacksmithForgeCancelButton.onClick.AddListener(() => presenter.OnBlacksmithForgeCancelClicked());
        blacksmithRecipeCancelButton.onClick.AddListener(() => presenter.OnBlacksmithRecipeCancelClicked());
        weaponShopButton.onClick.AddListener(() => presenter.OnWeaponShopPanelClicked());
        weaponShopCancelButton.onClick.AddListener(() => presenter.OnWeaponShopCancelClicked());
    }

    public void ShowMorningUI()
    {
        HideAllPanels();
        morningPanel.SetActive(true);
        Show();  // 부모 클래스의 Show 호출
    }

    public void ShowBlacksmithMenu()
    {
        HideAllPanels();
        blacksmithMenuPanel.SetActive(true);
    }

    public void ShowBlacksmithForge()
    {
        HideAllPanels();
        blacksmithForgePanel.SetActive(true);
    }

    public void ShowBlacksmithRecipe()
    {
        HideAllPanels();
        blacksmithRecipePanel.SetActive(true);
    }

    public void ShowWeaponShop()
    {
        HideAllPanels();
        weaponShopPanel.SetActive(true);
    }

    private void HideAllPanels()
    {
        morningPanel.SetActive(false);
        blacksmithMenuPanel.SetActive(false);
        blacksmithForgePanel.SetActive(false);
        blacksmithRecipePanel.SetActive(false);
        weaponShopPanel.SetActive(false);
    }

    public void OnBlacksmithUnlocked()
    {
        blacksmithUnlockButton.gameObject.SetActive(false);
        blacksmithMenuButton.gameObject.SetActive(true);
    }

    public void UpdateBlacksmithUnlockButton(bool isUnlocked, int cost)
    {
        blacksmithUnlockButton.gameObject.SetActive(!isUnlocked);
        blacksmithMenuButton.gameObject.SetActive(isUnlocked);
        if (!isUnlocked)
        {
            blacksmithUnlockButton.GetComponentInChildren<TextMeshProUGUI>().text = $"대장장이 해금 ({cost}G)";
        }
    }
} 