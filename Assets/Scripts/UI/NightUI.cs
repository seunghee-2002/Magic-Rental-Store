using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class NightUI : MonoBehaviour
{
    PanelController panelController;
    [Header("Night UI")]
    public GameObject nightPanel;
    [Header("Result List")]
    GameObject resultEntryPrefab;
    Transform resultListParent;

<<<<<<< HEAD
    void Awake() { panelController = UIManager.Instance.panelController; }

    public void ShowNightUI(List<DayResult> dayResults)
=======
    void Awake()
    {
        panelController = GetComponent<PanelController>();
    }

    public void InitPanel()
    {
        nightPanel = Instantiate(UIBinder.Instance.nightPanelPrefab, UIBinder.Instance.panelParent);
        nightPanel.SetActive(false);
    }

    public void BindUI()
    {
        resultEntryPrefab = UIBinder.Instance.resultEntryPrefab;
        resultListParent = UIBinder.Instance.resultListParent;
    }

    public void InitUI()
    {
        
    }

    public void ShowNightUI(List<DayResult> dayResults) // 저녁 패널 표시
>>>>>>> parent of 0773a29 (mvp2)
    {
        panelController.ShowNightUI();
        foreach (Transform t in resultListParent) Destroy(t.gameObject);
        foreach (DayResult r in dayResults)
        {
            GameObject entry = Instantiate(resultEntryPrefab, resultListParent);
            entry.GetComponentInChildren<TextMeshProUGUI>().text =
                r.customer.rentedWeapon == null
                    ? $"{r.customer.customerData.name} 아이템 없음"
                    : r.isSuccess
                        ? $"{r.customer.customerData.name} 성공! +{r.reward}G"
                        : $"{r.customer.customerData.name} 실패…";
        }
    }
}
