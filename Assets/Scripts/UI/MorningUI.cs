using UnityEngine.UI;
using TMPro;
using UnityEngine;

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

    void Awake()
    {
        panelController = GetComponent<PanelController>();
    }

    public void Init()
    {
        morningPanel = Instantiate(UIBinder.Instance.morningPanelPrefab, UIBinder.Instance.panelParent);
        morningPanel.SetActive(false);
    }

    public void InitUI()
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

        blacksmithMenuButton.interactable = false;

        blacksmithMenuButton.onClick.AddListener(() => {
            blacksmithMenuPanel.SetActive(!blacksmithMenuPanel.activeSelf);
        });
        blacksmithForgeButton.onClick.AddListener(ShowBlacksmithForgePanel);
        blacksmithRecipeButton.onClick.AddListener(ShowBlacksmithRecipePanel);
        blacksmithForgeCancelButton.onClick.AddListener(() => {
            blacksmithForgePanel.SetActive(false);
        });
        blacksmithRecipeCancelButton.onClick.AddListener(() => {
            blacksmithRecipePanel.SetActive(false);
        });
        blacksmithUnlockButton.onClick.AddListener(() => {
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
}
