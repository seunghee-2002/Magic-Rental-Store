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
<<<<<<< HEAD
<<<<<<< HEAD
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
=======
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

=======
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

>>>>>>> parent of 0773a29 (mvp2)
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
<<<<<<< HEAD
=======
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
>>>>>>> parent of 0773a29 (mvp2)
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
>>>>>>> parent of 0773a29 (mvp2)

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
<<<<<<< HEAD
<<<<<<< HEAD
        weaponShopButton.onClick.RemoveAllListeners();
        weaponShopButton.onClick.AddListener(ShowWeaponShop);
        weaponShopCloseButton.onClick.RemoveAllListeners();
        weaponShopCloseButton.onClick.AddListener(() => weaponShopPanel.SetActive(false));
=======
=======
>>>>>>> parent of 0773a29 (mvp2)
    }

    public void ShowMorningUI() // 아침 패널 표시
    {
        panelController.ShowMorningUI();
>>>>>>> parent of 0773a29 (mvp2)
    }

    public void ShowMorningUI() { panelController.ShowMorningUI(); }
    public void OnBlacksmithUnlocked()
    {
        blacksmithMenuButton.interactable = true;
        CommonUI.Instance.DisplayResult("대장장이 고용이 해금되었습니다!");
    }
<<<<<<< HEAD
<<<<<<< HEAD
    public void UpdateBlacksmithUI()
    {
        foreach (Transform t in forgeButtonParent) Destroy(t.gameObject);
        for (int i = 0; i < BlacksmithManager.Instance.recipes.Count; i++)
        {
            int idx = i;
            RecipeData recipe = BlacksmithManager.Instance.recipes[i];
            GameObject btn = Instantiate(forgeButtonPrefab, forgeButtonParent);
            TextMeshProUGUI txt = btn.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = $"{recipe.resultWeapon.weaponName} ({recipe.cost}G)";

            btn.GetComponent<Button>()
               .onClick.AddListener(() => {
                    BlacksmithManager.Instance.ForgeWeapon(idx);
               });
        }
    }
=======
=======

>>>>>>> parent of 0773a29 (mvp2)


>>>>>>> parent of 0773a29 (mvp2)
    void ShowBlacksmithForgePanel()
    {
        panelController.ShowBlacksmithForgePanel();

        // 레시피 UI 업데이트
        UpdateBlacksmithUI();
    }
<<<<<<< HEAD
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
=======

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
<<<<<<< HEAD
>>>>>>> parent of 0773a29 (mvp2)
=======
>>>>>>> parent of 0773a29 (mvp2)
        {
            CommonUI.Instance.DisplayResult("이미 구매한 무기입니다.");
            return;
        }
        string msg = $"{weapon.weaponName}\n등급: {weapon.grade}\n가격: {weapon.cost}\n\n{weapon.description}";
        popup.ShowConfirmation(
            msg,
<<<<<<< HEAD
<<<<<<< HEAD
            () => WeaponShopManager.Instance.Purchase(weapon, index),
            () => { }
=======
=======
>>>>>>> parent of 0773a29 (mvp2)
            () => Purchase(weapon, index),
            () => { /* 취소 시 아무것도 안 함 */ }
>>>>>>> parent of 0773a29 (mvp2)
        );
    }
}
