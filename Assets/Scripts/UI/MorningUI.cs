using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class MorningUI : MonoBehaviour
{
    PanelController panelController;
    [Header("Blacksmith Menu")]
    public GameObject blacksmithMenuPanel;
    [SerializeField] Button blacksmithMenuButton;
    [SerializeField] Button blacksmithForgeButton;
    [SerializeField] Button blacksmithRecipeButton;
    public Button blacksmithUnlockButton;
    [SerializeField] TextMeshProUGUI blacksmithUnlockText;
    [Header("Blacksmith Forge")]
    public GameObject blacksmithForgePanel;
    [SerializeField] Transform forgeButtonParent;
    [SerializeField] Button blacksmithForgeCancelButton;
    [SerializeField] GameObject forgeButtonPrefab;
    [Header("Blacksmith Recipe")]
    public GameObject blacksmithRecipePanel;
    [SerializeField] Button blacksmithRecipeCancelButton;
    [Header("Weapon Shop")]
    public GameObject weaponShopPanel;
    [SerializeField] Transform weaponShopButtonParent;
    [SerializeField] GameObject weaponShopButtonPrefab;
    [SerializeField] Button weaponShopButton;
    [SerializeField] Button weaponShopCancelButton;

    void Awake()
    {
        panelController = UIManager.Instance.panelController;
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
        weaponShopButton.onClick.RemoveAllListeners();
        weaponShopButton.onClick.AddListener(() =>
        {
            ShowWeaponShop();
        });
        weaponShopCancelButton.onClick.RemoveAllListeners();
        weaponShopCancelButton.onClick.AddListener(() =>
        {
            weaponShopPanel.SetActive(false);
        });
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

    public void UpdateBlacksmithUI()
    {        
        // 이전 버튼 삭제
        foreach (Transform t in forgeButtonParent) Destroy(t.gameObject);

        // 레시피마다 버튼 생성
        for (int i = 0; i < BlacksmithManager.Instance.recipes.Count; i++)
        {
            int idx = i;
            RecipeData recipe = BlacksmithManager.Instance.recipes[i];

            GameObject btn = Instantiate(forgeButtonPrefab, forgeButtonParent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{recipe.resultWeapon.weaponName} ({recipe.cost}G)";

            btn.GetComponent<Button>()
               .onClick.AddListener(() => {
                    BlacksmithManager.Instance.ForgeWeapon(idx);
               });
        }
    }

    void ShowBlacksmithForgePanel()
    {
        // 레시피 UI 업데이트
        UpdateBlacksmithUI();

        panelController.ShowBlacksmithForgePanel();
    }

    void ShowBlacksmithRecipePanel()
    {
        panelController.ShowBlacksmithRecipePanel();
    }

    public void ShowWeaponShop()
    {
        panelController.ShowWeaponShopPanel();
    }

    public void GenerateWeaponShop()
    {
        // 기존 버튼 제거 및 상태 초기화
        for (int i = 0; i < weaponShopButtonParent.childCount; i++)
        {
            Transform t = weaponShopButtonParent.GetChild(i);
            Destroy(t.gameObject);
        }

        WeaponShopManager.Instance.ResetPurchaseStatus();

        // 버튼 생성
        for (int i = 0; i < WeaponShopManager.Instance.currentStock.Count; i++)
        {
            int capturedIndex = i;
            WeaponData capturedWeapon = WeaponShopManager.Instance.currentStock[capturedIndex];

            GameObject go = Instantiate(weaponShopButtonPrefab, weaponShopButtonParent);
            go.GetComponent<Button>().onClick
                .AddListener(() => OnWeaponClicked(capturedWeapon, capturedIndex));
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
            () => { /* 취소 시 아무것도 안 함 */ }
        );
    }
}
