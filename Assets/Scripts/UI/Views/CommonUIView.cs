using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CommonUIView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI phaseText;
    [SerializeField] private Button heroButton;
    [SerializeField] private GameObject heroButtonObject;
    [SerializeField] private Transform inventoryPanelParent;
    [SerializeField] private GameObject inventoryButtonPrefab;

    private GameManager gameManager;

    public void Initialize(GameManager manager)
    {
        gameManager = manager;
        SetupButtonListeners();
        UpdateUI();
    }

    private void SetupButtonListeners()
    {
        if (heroButton != null)
        {
            heroButton.onClick.AddListener(() => {
                if (gameManager.IsHeroSystemUnlocked)
                {
                    UIManager.Instance.ShowHeroCreation();
                }
                else
                {
                    UIManager.Instance.ShowConfirmation(
                        $"용사 제작 시스템을 해금하시겠습니까? ({gameManager.heroUnlockGold} 골드)",
                        () => gameManager.TryUnlockHeroSystem()
                    );
                }
            });
        }
    }

    public void UpdateUI()
    {
        UpdateDay(gameManager.CurrentDay, gameManager.CurrentPhase);
        UpdateGold(gameManager.Gold);
        UpdatePhase(gameManager.CurrentPhase);
        UpdateHeroButton();
        UpdateInventory(gameManager.WeaponInventory);
        UpdateHeroes(gameManager.Heroes);
    }

    public void UpdateDay(int day, DayPhase phase)
    {
        if (dayText != null)
        {
            dayText.text = $"Day {day}";
        }
        if (phaseText != null)
        {
            phaseText.text = phase.ToString();
        }
    }

    public void UpdateGold(int gold)
    {
        if (goldText != null)
        {
            goldText.text = $"{gold:N0}";
        }
    }

    public void UpdatePhase(DayPhase phase)
    {
        if (phaseText != null)
        {
            phaseText.text = phase.ToString();
        }
    }

    public void UpdateHeroButton()
    {
        if (heroButtonObject != null)
        {
            heroButtonObject.SetActive(gameManager.IsHeroSystemUnlocked);
        }
    }

    public void UpdateInventory(List<WeaponInstance> inventory)
    {
        if (inventoryPanelParent == null || inventoryButtonPrefab == null) return;

        // 기존 인벤토리 버튼들 제거
        foreach (Transform child in inventoryPanelParent)
        {
            Destroy(child.gameObject);
        }

        // 현재 인벤토리의 무기들 표시
        foreach (WeaponInstance weapon in inventory)
        {
            GameObject buttonObj = Instantiate(inventoryButtonPrefab, inventoryPanelParent);
            InventoryButton button = buttonObj.GetComponent<InventoryButton>();
            button.Initialize(weapon.data, weapon.quantity);
        }
    }

    public void UpdateHeroes(List<HeroInstance> heroes)
    {
        // 영웅 목록 업데이트 로직
    }

    public void OnHeroSystemUnlocked()
    {
        UpdateHeroButton();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
} 