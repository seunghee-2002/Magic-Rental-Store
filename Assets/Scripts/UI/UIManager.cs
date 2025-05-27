using UnityEngine;

public class UIManager : MonoBehaviour
{
<<<<<<< HEAD
<<<<<<< HEAD
    public static UIManager Instance { get; private set; }
    public PanelController panelController;
=======
=======
>>>>>>> parent of 0773a29 (mvp2)
    public static UIManager Instance;
    [Header("UI Management")]
    CommonUI commonUI;
    PanelController panelController;
<<<<<<< HEAD
>>>>>>> parent of 0773a29 (mvp2)
=======
>>>>>>> parent of 0773a29 (mvp2)
    public MorningUI morningUI;
    public DayUI dayUI;
    public NightUI nightUI;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
<<<<<<< HEAD
=======

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
>>>>>>> parent of 0773a29 (mvp2)
    }
}
