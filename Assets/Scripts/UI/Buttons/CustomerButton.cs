using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CustomerButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;

    private CustomerInstance customer;
    private Action onClick;

    public void Initialize(CustomerInstance customer, Action onClick)
    {
        this.customer = customer;
        this.onClick = onClick;

        nameText.text = customer.customerData.customerName;
        gradeText.text = customer.customerData.grade.ToString();
        iconImage.sprite = customer.customerData.icon;

        button.onClick.AddListener(() => onClick?.Invoke());
    }
} 