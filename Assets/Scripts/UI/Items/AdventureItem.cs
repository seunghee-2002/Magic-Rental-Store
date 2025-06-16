using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AdventureItem : MonoBehaviour
{
    [Header("Text Fields")]
    public TextMeshProUGUI customerNameText;
    public TextMeshProUGUI remainingDaysText;
    public TextMeshProUGUI dungeonNameText;
    public Image customerIcon;  // 고객 아이콘을 위한 Image 컴포넌트

    public void Setup(string customerName, string dungeonName, 
        int remainingDays, Sprite customerIconSprite)
    {
        this.customerNameText.text = customerName;
        this.dungeonNameText.text = dungeonName;
        this.remainingDaysText.text = $"남은 일수: {remainingDays}일";
        this.customerIcon.sprite = customerIconSprite;
    }
}