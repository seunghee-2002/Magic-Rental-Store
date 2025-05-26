using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class NightUI : MonoBehaviour
{
    PanelController panelController;
    [Header("Result List")]
    [SerializeField] GameObject resultEntryPrefab;
    [SerializeField] Transform resultListParent;

    void Awake() { panelController = UIManager.Instance.panelController; }

    public void ShowNightUI(List<DayResult> dayResults)
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
