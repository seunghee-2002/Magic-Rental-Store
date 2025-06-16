using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HeroButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;

    private HeroInstance hero;
    private Action onClick;

    public void Initialize(HeroInstance hero, Action onClick)
    {
        this.hero = hero;
        this.onClick = onClick;

        nameText.text = hero.Name;
        levelText.text = $"Lv.{hero.Level}";
        gradeText.text = hero.Grade.ToString();
        iconImage.sprite = hero.Icon;

        button.onClick.AddListener(() => onClick?.Invoke());
    }
} 