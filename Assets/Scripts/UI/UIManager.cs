using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("UI Management")]
    CommonUI commonUI;
    PanelController panelController;
    public MorningUI morningUI;
    public DayUI dayUI;
    public NightUI nightUI;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        commonUI = GetComponent<CommonUI>();
        panelController = GetComponent<PanelController>();
        morningUI = GetComponent<MorningUI>();
        dayUI = GetComponent<DayUI>();
        nightUI = GetComponent<NightUI>();

        UIBinder.Instance.InitPanel(); // 패널 정보 가져오기
        morningUI.InitPanel(); // 패널 생성
        dayUI.InitPanel();
        nightUI.InitPanel();

        UIBinder.Instance.BindUI(); // UI 바인딩
        morningUI.BindUI();
        dayUI.BindUI();
        nightUI.BindUI();
        panelController.BindUI();
        commonUI.BindUI();

        morningUI.InitUI(); // UI 초기화
        dayUI.InitUI();
        nightUI.InitUI();
        panelController.InitUI();
        commonUI.InitUI();
    }
}
