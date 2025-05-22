using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public class MorningUI : MonoBehaviour
{
    PanelController panelController;
    [Header("Morning UI")]
    public GameObject morningPanel;
    [Header("Blacksmith Menu")]
    GameObject blacksmithMenuPanel;
    Button blacksmithMenuButton;
    Button blacksmithForgeButton;
    Button blacksmithRecipeButton;
    public Button blacksmithUnlockButton;
    TextMeshProUGUI blacksmithUnlockText;
    [Header("Blacksmith Forge")]
    GameObject blacksmithForgePanel;
    Transform forgeButtonParent;
    Button blacksmithForgeCancelButton;

    [Header("Blacksmith Recipe")]
    GameObject blacksmithRecipePanel;
    Button blacksmithRecipeCancelButton;
    [Header("Weapon Shop")]
    Transform weaponShopButtonParent;
    GameObject weaponShopButtonPrefab;
    List<bool> isPurchaseWeapon; // 구매 여부

    void Start()
    {
        panelController = GetComponent<PanelController>();
        isPurchaseWeapon = new List<bool>(WeaponShopManager.Instance.stockCount);
    }

    public void InitPanel()
    {
        morningPanel = Instantiate(UIBinder.Instance.morningPanelPrefab, UIBinder.Instance.panelParent);
        morningPanel.SetActive(false);
    }

    public void BindUI()
    {
        blacksmithMenuButton = UIBinder.Instance.blacksmithMenuButton;
        forgeButtonParent = UIBinder.Instance.forgeButtonParent;
        blacksmithMenuPanel = UIBinder.Instance.blacksmithMenuPanel;
        blacksmithForgePanel = UIBinder.Instance.blacksmithForgePanel;
        blacksmithRecipePanel = UIBinder.Instance.blacksmithRecipePanel;
        blacksmithForgeButton = UIBinder.Instance.blacksmithForgeButton;
        blacksmithRecipeButton = UIBinder.Instance.blacksmithRecipeButton;
        blacksmithForgeCancelButton = UIBinder.Instance.blacksmithForgeCancelButton;
        blacksmithRecipeCancelButton = UIBinder.Instance.blacksmithRecipeCancelButton;
        blacksmithUnlockButton = UIBinder.Instance.blacksmithUnlockButton;
        blacksmithUnlockText = UIBinder.Instance.blacksmithUnlockText;
    }

    public void InitUI()
    {
        blacksmithMenuButton.interactable = false;
        blacksmithMenuButton.onClick.RemoveAllListeners();
        blacksmithMenuButton.onClick.AddListener(() =>
        {
            blacksmithMenuPanel.SetActive(!blacksmithMenuPanel.activeSelf);
        });
        blacksmithForgeButton.onClick.RemoveAllListeners();
        blacksmithForgeButton.onClick.AddListener(ShowBlacksmithForgePanel);
        blacksmithRecipeButton.onClick.RemoveAllListeners();
        blacksmithRecipeButton.onClick.AddListener(ShowBlacksmithRecipePanel);
        blacksmithForgeCancelButton.onClick.RemoveAllListeners();
        blacksmithForgeCancelButton.onClick.AddListener(() =>
        {
            blacksmithForgePanel.SetActive(false);
        });
        blacksmithRecipeCancelButton.onClick.RemoveAllListeners();
        blacksmithRecipeCancelButton.onClick.AddListener(() =>
        {
            blacksmithRecipePanel.SetActive(false);
        });
        blacksmithUnlockButton.onClick.RemoveAllListeners();
        blacksmithUnlockButton.onClick.AddListener(() =>
        {
            GameManager.Instance.PurchaseBlacksmithSystemUnlock();
        });
        blacksmithUnlockText.text = $"대장장이 시스템 해금 비용: {GameManager.Instance.blacksmithUnlockGold}G";
    }

    public void ShowMorningUI() // 아침 패널 표시
    {
        panelController.ShowMorningUI();
    }

    public void OnBlacksmithUnlocked()
    {
        blacksmithMenuButton.interactable = true;
        CommonUI.Instance.DisplayResult("대장장이 고용이 해금되었습니다!");
    }


    void ShowBlacksmithForgePanel()
    {
        panelController.ShowBlacksmithForgePanel();

        // 레시피 UI 업데이트
        UpdateBlacksmithUI();
    }

    void ShowBlacksmithRecipePanel()
    {
        panelController.ShowBlacksmithRecipePanel();
    }

    public void UpdateBlacksmithUI()
    {
        BlacksmithManager.Instance.UpdateUI();
    }


    public void GenerateWeaponShop(List<WeaponData> stock)
    {
        int index = 0;
        foreach (Transform t in weaponShopButtonParent)
        {
            isPurchaseWeapon[index] = false;
            index++;
            Destroy(t.gameObject);
        }

        index = 0;
        // 버튼 생성
        foreach (WeaponData weaponData in stock)
        {
            GameObject go = Instantiate(weaponShopButtonPrefab, weaponShopButtonParent);
            go.GetComponent<Image>().sprite = weaponData.icon;
            go.GetComponent<Button>().onClick
              .AddListener(() => OnWeaponClicked(weaponData, index));
            index++;
        }
    }
    
    void onWeaponClicked(WeaponData weapon, int index)
    {
        
        if (isPurchaseWeapon[index])
        {
            CommonUI.Instance.DisplayResult("이미 구매한 무기입니다.");
            return;
        }

        string msg = $"{weapon.weaponName}\n등급: {weapon.grade}\n가격: {weapon.cost}\n\n{weapon.description}";
        popup.ShowConfirmation(
            msg,
            () => Purchase(weapon, index),
            () => { /* 취소 시 아무것도 안 함 */ }
        );
    }
}
