using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    CommonUI commonUI;
    public PanelController panelController;
    [SerializeField] GameObject morningUIPrefab;
    [SerializeField] GameObject dayUIPrefab;
    [SerializeField] GameObject nightUIPrefab;
    [SerializeField] Transform panelParent;
    [Header("UI Management")]
    public MorningUI morningUI;
    public DayUI dayUI;
    public NightUI nightUI;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        commonUI = GetComponent<CommonUI>();
        panelController = GetComponent<PanelController>();
        morningUI = Instantiate(morningUIPrefab, panelParent).GetComponent<MorningUI>();
        dayUI = Instantiate(dayUIPrefab, panelParent).GetComponent<DayUI>();
        nightUI = Instantiate(nightUIPrefab, panelParent).GetComponent<NightUI>();
    
        panelController.BindUI();

        morningUI.InitUI(); // UI 초기화
        dayUI.InitUI();
        nightUI.InitUI();
        panelController.InitUI();
        commonUI.InitUI();
    }
}
