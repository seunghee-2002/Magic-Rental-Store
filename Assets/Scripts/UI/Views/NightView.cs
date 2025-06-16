using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class NightView : DayPhaseView
{
    public NightPresenter presenter;

    [Header("Adventure List")]
    [SerializeField] private Transform adventureListParent;    // 모험 목록이 표시될 부모 오브젝트
    [SerializeField] private GameObject adventureItemPrefab;   // 모험 항목 프리팹

    [Header("Panels")]
    [SerializeField] private GameObject nightPanel;            // 밤 시간대 메인 패널
    [SerializeField] private GameObject resultPanel;           // 결과 패널

    [Header("Result")]
    [SerializeField] private Transform resultContent;          // 결과 항목들이 생성될 부모 오브젝트
    [SerializeField] private GameObject resultItemPrefab;      // 결과 항목 프리팹

    [Header("Buttons")]
    [SerializeField] private Button nextPhaseButton;

    private NightModel model;

    public void Initialize(NightModel model)
    {
        this.model = model;
        SetupButtonListeners();
    }

    protected override void SetupButtonListeners()
    {
        if (nextPhaseButton != null)
        {
            nextPhaseButton.onClick.AddListener(() => {
                GameManager.Instance.AdvanceToNextPhase();
            });
        }
    }

    public void UpdateResults(List<DayResult> results)
    {
        // 결과 목록 업데이트 로직
    }

    public override void Show()
    {
        nightPanel.SetActive(true);
    }

    public override void Hide()
    {
        nightPanel.SetActive(false);
    }

    public void ShowNightUI()
    {
        Show();
        UpdateUI();
    }

    protected override void UpdateUI()
    {
        ShowActiveAdventures();
        ShowResults(presenter.GetDayResults());
    }

    private void ShowActiveAdventures()
    {
        ClearAdventureList();

        List<AdventureManager.Adventure> activeAdventures = AdventureManager.Instance.GetActiveAdventures();
        foreach (AdventureManager.Adventure adventure in activeAdventures)
        {
            AddAdventureItem(
                adventure.customer.customerData.customerName,
                adventure.customer.dungeon.dungeonData.dungeonName,
                adventure.remainingDays,
                adventure.customer.customerData.icon
            );
        }
    }

    private void AddAdventureItem(string customerName, string dungeonName, int remainingDays, Sprite customerIcon)
    {
        GameObject item = Instantiate(adventureItemPrefab, adventureListParent);
        AdventureItem adventureItem = item.GetComponent<AdventureItem>();
        adventureItem.Setup(customerName, dungeonName, remainingDays, customerIcon);
    }

    private void ClearAdventureList()
    {
        foreach (Transform child in adventureListParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowResults(List<DayResult> dayResults)
    {
        ClearResultList();

        foreach (DayResult result in dayResults)
        {
            AddResultItem(result);
        }

        ShowResultPanel();
    }

    private void AddResultItem(DayResult result)
    {
        GameObject resultObj = Instantiate(resultItemPrefab, resultContent);
        ResultItem resultItem = resultObj.GetComponent<ResultItem>();
        resultItem.Setup(
            result.customer.customerData.customerName,
            result.reward,
            result.expectedMaterial?.materialName ?? "없음",
            result.materialCount,
            result.isSuccess
        );
    }

    public void ClearResultList()
    {
        foreach (Transform child in resultContent)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowResultPanel()
    {
        HideAllPanels();
        resultPanel.SetActive(true);
    }

    private void HideAllPanels()
    {
        nightPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    public override void OnNextPhaseClicked()
    {
        base.OnNextPhaseClicked();
        Hide();
    }

    public void AddResultItem(string customerName, string dungeonName, int remainingDays, Sprite customerIcon)
    {
        GameObject item = Instantiate(adventureItemPrefab, adventureListParent);
        AdventureItem adventureItem = item.GetComponent<AdventureItem>();
        adventureItem.Setup(customerName, dungeonName, remainingDays, customerIcon);
    }
} 