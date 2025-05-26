using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerInfoPanel : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI descText;
    public Image iconImage;

    public void SetInfo(CustomerInstance customer)
    {
        nameText.text = customer.customerData.name;
        levelText.text = $"레벨: {customer.customerData.level}";
        gradeText.text = $"등급: {customer.customerData.grade}";
        descText.text = customer.customerData.desc;
        iconImage.sprite = customer.customerData.icon;
    }
}
