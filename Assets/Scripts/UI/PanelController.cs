using UnityEngine;

public class PanelController : MonoBehaviour
{
    [Header("Panels")]
    public MorningUI morningUI;
    public DayUI dayUI;
    public NightUI nightUI;

    public GameObject morningPanel;
    public GameObject dayPanel;
    public GameObject nightPanel;
    public GameObject heroRosterPanel;
    public GameObject blacksmithMenuPanel;
    public GameObject blacksmithForgePanel;
    public GameObject blacksmithRecipePanel;
    public GameObject weaponShopPanel;
    public GameObject heroMenuPanel;
    public GameObject heroCreationPanel;
    public GameObject weaponSelectionPanel;

    public void BindUI()
    {
        morningUI = UIManager.Instance.morningUI;
        dayUI = UIManager.Instance.dayUI;
        nightUI = UIManager.Instance.nightUI;

        morningPanel = morningUI.gameObject;
        dayPanel = dayUI.gameObject;
        nightPanel = nightUI.gameObject;

        blacksmithMenuPanel = morningUI.blacksmithMenuPanel;
        blacksmithForgePanel = morningUI.blacksmithForgePanel;
        blacksmithRecipePanel = morningUI.blacksmithRecipePanel;
        weaponShopPanel = morningUI.weaponShopPanel;
        heroRosterPanel = dayUI.heroRosterPanel;
        heroMenuPanel = dayUI.heroMenuPanel;
        heroCreationPanel = dayUI.heroCreationPanel;
        weaponSelectionPanel = dayUI.weaponSelectionPanel;
    }

    public void InitUI()
    {
        heroMenuPanel.SetActive(false);
        heroCreationPanel.SetActive(false);
        heroRosterPanel.SetActive(false);
        weaponShopPanel.SetActive(false);
        blacksmithMenuPanel.SetActive(false);
        blacksmithForgePanel.SetActive(false);
        blacksmithRecipePanel.SetActive(false);
        weaponSelectionPanel.SetActive(false);
    }

    public void ShowMorningUI()
    {
        HideAllPanels();
        morningPanel.SetActive(true);
    }
    public void ShowDayUI()
    {
        HideAllPanels();
        dayPanel.SetActive(true);
    }
    public void ShowNightUI()
    {
        HideAllPanels();
        nightPanel.SetActive(true);
    }
    public void ShowBlacksmithForgePanel()
    {
        HideAllPanels();
        blacksmithRecipePanel.SetActive(true);
        blacksmithForgePanel.SetActive(true);
    }
    public void ShowBlacksmithRecipePanel()
    {
        HideAllPanels();
        blacksmithRecipePanel.SetActive(true);
    }
    public void ShowHeroCreationPanel()
    {
        HideAllPanels();
        heroCreationPanel.SetActive(true);
    }
    public void ShowHeroRosterPanel()
    {
        HideAllPanels();
        heroRosterPanel.SetActive(true);
    }
    public void ShowWeaponSelectionPanel()
    {
        HideAllPanels();
        weaponSelectionPanel.SetActive(true);
    }
    public void ShowWeaponShopPanel()
    {
        HideAllPanels();
        weaponShopPanel.SetActive(true);
    }

    void HideAllPanels()
    {
        heroRosterPanel.SetActive(false);
        blacksmithMenuPanel.SetActive(false);
        blacksmithForgePanel.SetActive(false);
        blacksmithRecipePanel.SetActive(false);
        weaponShopPanel.SetActive(false);
        heroMenuPanel.SetActive(false);
        heroCreationPanel.SetActive(false);
        weaponSelectionPanel.SetActive(false);
    }
}
