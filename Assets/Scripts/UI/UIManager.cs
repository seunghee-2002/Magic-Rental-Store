using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Views")]
    public MorningView MorningView;
    public DayView DayView;
    public NightView NightView;
    public CommonUIView CommonUIView;

    [Header("Popups")]
    public GameObject messagePopup;
    public GameObject confirmationPopup;
    public GameObject heroCreationPopup;
    public GameObject weaponShopPanel;
    public GameObject blacksmithPanel;
    public GameObject overlayBlocker;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeUI()
    {
        // GameManager 이벤트 구독
        GameManager.Instance.OnGoldChanged += UpdateGoldUI;
        GameManager.Instance.OnDayChanged += UpdateDayUI;
        GameManager.Instance.OnPhaseChanged += UpdatePhaseUI;
        GameManager.Instance.OnInventoryChanged += UpdateInventoryUI;
        GameManager.Instance.OnHeroesChanged += UpdateHeroesUI;
        GameManager.Instance.OnCustomersChanged += UpdateCustomersUI;
        GameManager.Instance.OnDayResultsChanged += UpdateDayResultsUI;

        // 초기 UI 설정
        InitializeViews();
        HideAllPopups();
    }

    private void InitializeViews()
    {
        // CommonUIView 초기화
        CommonUIView.Initialize(GameManager.Instance);

        // 각 View 초기화
        MorningView.Initialize(GameManager.Instance.MorningModel);
        DayView.Initialize(GameManager.Instance.DayModel);
        NightView.Initialize(GameManager.Instance.NightModel);
    }

    private void UpdateGoldUI(int gold)
    {
        CommonUIView.UpdateGold(gold);
    }

    private void UpdateDayUI(int day)
    {
        CommonUIView.UpdateDay(day);
    }

    private void UpdatePhaseUI(DayPhase phase)
    {
        switch (phase)
        {
            case DayPhase.Morning:
                ShowMorningUI();
                break;
            case DayPhase.Day:
                ShowDayUI();
                break;
            case DayPhase.Night:
                ShowNightUI(GameManager.Instance.GetDayResults());
                break;
        }
    }

    private void UpdateInventoryUI()
    {
        CommonUIView.UpdateInventory(GameManager.Instance.WeaponInventory);
    }

    private void UpdateHeroesUI()
    {
        CommonUIView.UpdateHeroes(GameManager.Instance.Heroes);
    }

    private void UpdateCustomersUI()
    {
        switch (GameManager.Instance.CurrentPhase)
        {
            case DayPhase.Morning:
                MorningView.UpdateCustomers(GameManager.Instance.GetCurrentCustomers());
                break;
            case DayPhase.Day:
                DayView.UpdateCustomers(GameManager.Instance.GetCurrentCustomers());
                break;
        }
    }

    private void UpdateDayResultsUI()
    {
        if (GameManager.Instance.CurrentPhase == DayPhase.Night)
        {
            NightView.UpdateResults(GameManager.Instance.GetDayResults());
        }
    }

    public void ShowMorningUI()
    {
        HideAllViews();
        MorningView.Show();
        CommonUIView.Show();
    }

    public void ShowDayUI()
    {
        HideAllViews();
        DayView.Show();
        CommonUIView.Show();
    }

    public void ShowNightUI(List<DayResult> results)
    {
        HideAllViews();
        NightView.Show();
        CommonUIView.Show();
        NightView.UpdateResults(results);
    }

    private void HideAllViews()
    {
        MorningView.Hide();
        DayView.Hide();
        NightView.Hide();
        CommonUIView.Hide();
    }

    public void ShowMessage(string message)
    {
        messagePopup.SetActive(true);
        messagePopup.GetComponent<MessagePopup>().SetMessage(message);
    }

    public void ShowConfirmation(string message, System.Action onConfirm)
    {
        confirmationPopup.SetActive(true);
        confirmationPopup.GetComponent<ConfirmationPopup>().SetMessage(message, onConfirm);
    }

    public void ShowHeroCreation()
    {
        heroCreationPopup.SetActive(true);
    }

    public void ShowWeaponShop()
    {
        weaponShopPanel.SetActive(true);
    }

    public void ShowBlacksmith()
    {
        blacksmithPanel.SetActive(true);
    }

    public void HideAllPopups()
    {
        messagePopup.SetActive(false);
        confirmationPopup.SetActive(false);
        heroCreationPopup.SetActive(false);
        weaponShopPanel.SetActive(false);
        blacksmithPanel.SetActive(false);
        overlayBlocker.SetActive(false);
    }

    public void OnHeroSystemUnlocked()
    {
        CommonUIView.OnHeroSystemUnlocked();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            // GameManager 이벤트 구독 해제
            GameManager.Instance.OnGoldChanged -= UpdateGoldUI;
            GameManager.Instance.OnDayChanged -= UpdateDayUI;
            GameManager.Instance.OnPhaseChanged -= UpdatePhaseUI;
            GameManager.Instance.OnInventoryChanged -= UpdateInventoryUI;
            GameManager.Instance.OnHeroesChanged -= UpdateHeroesUI;
            GameManager.Instance.OnCustomersChanged -= UpdateCustomersUI;
            GameManager.Instance.OnDayResultsChanged -= UpdateDayResultsUI;
        }
    }
}
