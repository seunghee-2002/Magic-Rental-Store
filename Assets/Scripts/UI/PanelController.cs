using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [Header("Panels")]
    GameObject morningPanel;
    GameObject dayPanel;
    GameObject nightPanel;
    GameObject heroRosterPanel;
    GameObject blacksmithMenuPanel;
    GameObject blacksmithForgePanel;
    GameObject blacksmithRecipePanel;
    GameObject heroMenuPanel;
    GameObject heroCreationPanel;
    GameObject WeaponSelectionPanel;

    public void BindUI()
    {
        morningPanel = GetComponent<MorningUI>().morningPanel;
        dayPanel = GetComponent<DayUI>().dayPanel;
        nightPanel = GetComponent<NightUI>().nightPanel;

        heroRosterPanel = UIBinder.Instance.heroRosterPanel;
        blacksmithMenuPanel = UIBinder.Instance.blacksmithMenuPanel;
        blacksmithForgePanel = UIBinder.Instance.blacksmithForgePanel;
        blacksmithRecipePanel = UIBinder.Instance.blacksmithRecipePanel;
        heroMenuPanel = UIBinder.Instance.heroMenuPanel;
        heroCreationPanel = UIBinder.Instance.heroCreationPanel;
        WeaponSelectionPanel = UIBinder.Instance.WeaponSelectionPanel;
    }

    public void InitUI()
    {
        heroMenuPanel.SetActive(false);
        heroCreationPanel.SetActive(false);
        heroRosterPanel.SetActive(false);
        blacksmithMenuPanel.SetActive(false);
        blacksmithForgePanel.SetActive(false);
        blacksmithRecipePanel.SetActive(false);
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

    public void CloseWeaponSelectionPanel() // 아이템 선택 팝업 닫기
    {
        WeaponSelectionPanel.SetActive(false);
    }

    void HideAllPanels()
    {
        heroRosterPanel.SetActive(false);
        blacksmithMenuPanel.SetActive(false);
        blacksmithForgePanel.SetActive(false);
        blacksmithRecipePanel.SetActive(false);
        heroMenuPanel.SetActive(false);
        heroCreationPanel.SetActive(false);
        WeaponSelectionPanel.SetActive(false);
    }
}
