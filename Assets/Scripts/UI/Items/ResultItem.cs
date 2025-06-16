using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI customerNameText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private TextMeshProUGUI materialText;  // 보상 재료 텍스트
    [SerializeField] private TextMeshProUGUI resultText;    // 성공/실패 텍스트
    [SerializeField] private Image resultIcon;              // 성공/실패 아이콘

    public void Setup(string customerName, int reward, string materialName, int materialCount, bool isSuccess)
    {
        customerNameText.text = customerName;
        rewardText.text = $"{reward}G";
        materialText.text = $"{materialName} x{materialCount}";
        
        // 성공/실패 표시
        resultText.text = isSuccess ? "성공" : "실패";
        resultText.color = isSuccess ? Color.green : Color.red;
        resultIcon.sprite = isSuccess ? successIcon : failIcon;
    }

    [SerializeField] private Sprite successIcon;  // 성공 아이콘
    [SerializeField] private Sprite failIcon;     // 실패 아이콘
}