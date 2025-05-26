using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class MorningUI : MonoBehaviour
{
    PanelController panelController;
    [Header("Blacksmith Menu")]
    public GameObject blacksmithMenuPanel;
    public Button blacksmithMenuButton;
    public Button blacksmithForgeButton;
    public Button blacksmithRecipeButton;
    public Button blacksmithUnlockButton;
    public TextMeshProUGUI blacksmithUnlockText;
    [Header("Blacksmith Forge")]
    public GameObject blacksmithForgePanel;
    public Transform forgeButtonParent;
    public Button blacksmithForgeCloseButton;
    public GameObject forgeButtonPrefab;
    [Header("Blacksmith Recipe")]
    public GameObject blacksmithRecipePanel;
    public Button blacksmithRecipeCloseButton;
    [Header("Weapon Shop")]
    public GameObject weaponShopPanel;
    public Transform weaponShopButtonParent;
    public GameObject weaponShopButtonPrefab;
    public Button weaponShopButton;
    public Button weaponShopCloseButton;

    void Awake() { panelController = UIManager.Instance.panelController; }

    public void InitUI()
    {
        blacksmithMenuButton.interactable = false;
        blacksmithMenuButton.onClick.RemoveAllListeners();
        blacksmithMenuButton.onClick.AddListener(() => blacksmithMenuPanel.SetActive(!blacksmithMenuPanel.activeSelf));
        blacksmithForgeButton.onClick.RemoveAllListeners();
        blacksmithForgeButton.onClick.AddListener(ShowBlacksmithForgePanel);
        blacksmithRecipeButton.onClick.RemoveAllListeners();
        blacksmithRecipeButton.onClick.AddListener(ShowBlacksmithRecipePanel);
        blacksmithForgeCloseButton.onClick.RemoveAllListeners();
        blacksmithForgeCloseButton.onClick.AddListener(() => blacksmithForgePanel.SetActive(false));
        blacksmithRecipeCloseButton.onClick.RemoveAllListeners();
        blacksmithRecipeCloseButton.onClick.AddListener(() => blacksmithRecipePanel.SetActive(false));
        blacksmithUnlockButton.onClick.RemoveAllListeners();
        blacksmithUnlockButton.onClick.AddListener(() => GameManager.Instance.PurchaseBlacksmithSystemUnlock());
        blacksmithUnlockText.text = $"대장장이 시스템 해금 비용: {GameManager.Instance.blacksmithUnlockGold}G";
        weaponShopButton.onClick.RemoveAllListeners();
        weaponShopButton.onClick.AddListener(ShowWeaponShop);
        weaponShopCloseButton.onClick.RemoveAllListeners();
        weaponShopCloseButton.onClick.AddListener(() => weaponShopPanel.SetActive(false));
    }

    public void ShowMorningUI() { panelController.ShowMorningUI(); }
    public void OnBlacksmithUnlocked()
    {
        blacksmithMenuButton.interactable = true;
        CommonUI.Instance.DisplayResult("대장장이 고용이 해금되었습니다!");
    }
    public void UpdateBlacksmithUI()
    {
        foreach (Transform t in forgeButtonParent) Destroy(t.gameObject);
        for (int i = 0; i < BlacksmithManager.Instance.recipes.Count; i++)
        {
            int idx = i;
            RecipeData recipe = BlacksmithManager.Instance.recipes[i];
            GameObject btn = Instantiate(forgeButtonPrefab, forgeButtonParent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{recipe.resultWeapon.weaponName} ({recipe.cost}G)";
            btn.GetComponent<Button>().onClick.AddListener(() => BlacksmithManager.Instance.ForgeWeapon(idx));
        }
    }
    void ShowBlacksmithForgePanel()
    {
        panelController.ShowBlacksmithForgePanel();

        // 레시피 UI 업데이트
        UpdateBlacksmithUI();
    }
    void ShowBlacksmithRecipePanel() { panelController.ShowBlacksmithRecipePanel(); }
    public void ShowWeaponShop() { panelController.ShowWeaponShopPanel(); }
    public void GenerateWeaponShop()
    {
        for (int i = 0; i < weaponShopButtonParent.childCount; i++)
            Destroy(weaponShopButtonParent.GetChild(i).gameObject);

        WeaponShopManager.Instance.ResetPurchaseStatus();
        for (int i = 0; i < WeaponShopManager.Instance.currentStock.Count; i++)
        {
            int capturedIndex = i;
            WeaponData capturedWeapon = WeaponShopManager.Instance.currentStock[capturedIndex];
            GameObject go = Instantiate(weaponShopButtonPrefab, weaponShopButtonParent);
            go.GetComponent<Button>().onClick.AddListener(() => OnWeaponClicked(capturedWeapon, capturedIndex));
        }
    }
    void OnWeaponClicked(WeaponData weapon, int index)
    {
        if (WeaponShopManager.Instance.isPurchaseWeapon[index])
        {
            CommonUI.Instance.DisplayResult("이미 구매한 무기입니다.");
            return;
        }
        string msg = $"{weapon.weaponName}\n등급: {weapon.grade}\n가격: {weapon.cost}\n\n{weapon.description}";
        CommonUI.Instance.ShowConfirmation(
            msg,
            () => WeaponShopManager.Instance.Purchase(weapon, index),
            () => { }
        );
    }
}
