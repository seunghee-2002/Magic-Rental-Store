using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("UI Management")]
    PanelController panelController;
    public MorningUI morningUI;
    public DayUI dayUI;
    public NightUI nightUI;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        panelController = GetComponent<PanelController>();
        morningUI = GetComponent<MorningUI>();
        dayUI = GetComponent<DayUI>();
        nightUI = GetComponent<NightUI>();
        
        UIBinder.Instance.Init();
        morningUI.Init();
        dayUI.Init();
        nightUI.Init();
        UIBinder.Instance.BindUI(); // UI 바인딩
        morningUI.InitUI();
        dayUI.InitUI();
        nightUI.InitUI();
        panelController.InitPanel();
        CommonUI.Instance.Init();
    }
}
