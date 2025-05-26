using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public PanelController panelController;
    public MorningUI morningUI;
    public DayUI dayUI;
    public NightUI nightUI;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
