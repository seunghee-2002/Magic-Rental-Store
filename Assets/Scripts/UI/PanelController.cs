using UnityEngine;

public class PanelController : MonoBehaviour
{
    [Header("Panels")]
    MorningUI morningUI;
    DayUI dayUI;
    NightUI nightUI;
    GameObject morningPanel;
    GameObject dayPanel;
    GameObject nightPanel;
    GameObject heroRosterPanel;
    GameObject blacksmithMenuPanel;
    GameObject blacksmithForgePanel;
    GameObject blacksmithRecipePanel;
    GameObject WeaponShopPanel;
    GameObject heroMenuPanel;
    GameObject heroCreationPanel;
    GameObject WeaponSelectionPanel;

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
        WeaponShopPanel = morningUI.weaponShopPanel;
        heroRosterPanel = dayUI.heroRosterPanel;
        heroMenuPanel = dayUI.heroMenuPanel;
        heroCreationPanel = dayUI.heroCreationPanel;
        WeaponSelectionPanel = dayUI.WeaponSelectionPanel;
       
    }

    public void InitUI()
    {
        heroMenuPanel.SetActive(false);
        heroCreationPanel.SetActive(false);
        heroRosterPanel.SetActive(false);
        WeaponShopPanel.SetActive(false);
        blacksmithMenuPanel.SetActive(false);
        blacksmithForgePanel.SetActive(false);
        blacksmithRecipePanel.SetActive(false);
        WeaponSelectionPanel.SetActive(false);
    }

    public void ShowMorningUI()
    {
        HideAllPanels();
        morningPanel.SetActive(true);
        dayPanel.SetActive(false);
        nightPanel.SetActive(false);
    }

    public void ShowDayUI()
    {
        HideAllPanels();
        morningPanel.SetActive(false);
        dayPanel.SetActive(true);
        nightPanel.SetActive(false);
    }

    public void ShowNightUI()
    {
        HideAllPanels();
        morningPanel.SetActive(false);
        dayPanel.SetActive(false);
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
        WeaponSelectionPanel.SetActive(true);
    }

    public void ShowWeaponShopPanel()
    {
        HideAllPanels();
        WeaponShopPanel.SetActive(true);
    }

    void HideAllPanels()
    {
        heroRosterPanel.SetActive(false);
        blacksmithMenuPanel.SetActive(false);
        blacksmithForgePanel.SetActive(false);
        blacksmithRecipePanel.SetActive(false);
        WeaponShopPanel.SetActive(false);
        heroMenuPanel.SetActive(false);
        heroCreationPanel.SetActive(false);
        WeaponSelectionPanel.SetActive(false);
    }
}
