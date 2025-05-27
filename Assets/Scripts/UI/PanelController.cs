using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [Header("Panels")]
<<<<<<< HEAD
<<<<<<< HEAD
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
=======
=======
>>>>>>> parent of 0773a29 (mvp2)
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
>>>>>>> parent of 0773a29 (mvp2)

    public void BindUI()
    {
        morningPanel = GetComponent<MorningUI>().morningPanel;
        dayPanel = GetComponent<DayUI>().dayPanel;
        nightPanel = GetComponent<NightUI>().nightPanel;

<<<<<<< HEAD
<<<<<<< HEAD
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
=======
=======
>>>>>>> parent of 0773a29 (mvp2)
        heroRosterPanel = UIBinder.Instance.heroRosterPanel;
        blacksmithMenuPanel = UIBinder.Instance.blacksmithMenuPanel;
        blacksmithForgePanel = UIBinder.Instance.blacksmithForgePanel;
        blacksmithRecipePanel = UIBinder.Instance.blacksmithRecipePanel;
        heroMenuPanel = UIBinder.Instance.heroMenuPanel;
        heroCreationPanel = UIBinder.Instance.heroCreationPanel;
        WeaponSelectionPanel = UIBinder.Instance.WeaponSelectionPanel;
<<<<<<< HEAD
>>>>>>> parent of 0773a29 (mvp2)
=======
>>>>>>> parent of 0773a29 (mvp2)
    }

    public void InitUI()
    {
        heroMenuPanel.SetActive(false);
        heroCreationPanel.SetActive(false);
        heroRosterPanel.SetActive(false);
<<<<<<< HEAD
<<<<<<< HEAD
        weaponShopPanel.SetActive(false);
        blacksmithMenuPanel.SetActive(false);
        blacksmithForgePanel.SetActive(false);
        blacksmithRecipePanel.SetActive(false);
        weaponSelectionPanel.SetActive(false);
=======
        blacksmithMenuPanel.SetActive(false);
        blacksmithForgePanel.SetActive(false);
        blacksmithRecipePanel.SetActive(false);
>>>>>>> parent of 0773a29 (mvp2)
=======
        blacksmithMenuPanel.SetActive(false);
        blacksmithForgePanel.SetActive(false);
        blacksmithRecipePanel.SetActive(false);
>>>>>>> parent of 0773a29 (mvp2)
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
<<<<<<< HEAD
<<<<<<< HEAD
    public void ShowWeaponShopPanel()
    {
        HideAllPanels();
        weaponShopPanel.SetActive(true);
=======

    public void CloseWeaponSelectionPanel() // 아이템 선택 팝업 닫기
    {
        WeaponSelectionPanel.SetActive(false);
>>>>>>> parent of 0773a29 (mvp2)
=======

    public void CloseWeaponSelectionPanel() // 아이템 선택 팝업 닫기
    {
        WeaponSelectionPanel.SetActive(false);
>>>>>>> parent of 0773a29 (mvp2)
    }

    void HideAllPanels()
    {
        heroRosterPanel.SetActive(false);
        blacksmithMenuPanel.SetActive(false);
        blacksmithForgePanel.SetActive(false);
        blacksmithRecipePanel.SetActive(false);
<<<<<<< HEAD
<<<<<<< HEAD
        weaponShopPanel.SetActive(false);
=======
>>>>>>> parent of 0773a29 (mvp2)
=======
>>>>>>> parent of 0773a29 (mvp2)
        heroMenuPanel.SetActive(false);
        heroCreationPanel.SetActive(false);
        weaponSelectionPanel.SetActive(false);
    }
}
