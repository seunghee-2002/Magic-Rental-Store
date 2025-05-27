using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NightUI : MonoBehaviour
{
    PanelController panelController;
    [Header("Result List")]
    [SerializeField] GameObject resultEntryPrefab;
    [SerializeField] Transform resultListParent;

    void Awake()
    {
        panelController = UIManager.Instance.panelController;
    }
    
    public void InitUI()
    {

    }

    public void ShowNightUI(List<DayResult> dayResults) // 저녁 패널 표시
    {
        panelController.ShowNightUI();

        foreach (Transform t in resultListParent) Destroy(t.gameObject);
        foreach (DayResult r in dayResults)
        {
            GameObject entry = Instantiate(resultEntryPrefab, resultListParent);
            entry.GetComponentInChildren<TextMeshProUGUI>().text =
                r.weapon == null
                    ? $"{r.customer.name} 아이템 없음"
                    : r.isSuccess
                        ? $"{r.customer.name} 성공! +{r.reward}G"
                        : $"{r.customer.name} 실패…";
        }
    }
}
