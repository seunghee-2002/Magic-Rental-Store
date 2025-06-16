using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryButton : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI quantityText;
    public Image weaponIcon;
    public Button button;

    private WeaponData weaponData;
    private int quantity;

    public void Initialize(WeaponData data, int quantity)
    {
        this.weaponData = data;
        this.quantity = quantity;

        weaponNameText.text = data.weaponName;
        quantityText.text = $"x{quantity}";
        weaponIcon.sprite = data.icon;

        button.onClick.AddListener(() => OnClick());
    }

    public void OnClick()
    {
        // 버튼 클릭 시 처리할 로직
        // 예: 무기 상세 정보 표시
    }
}