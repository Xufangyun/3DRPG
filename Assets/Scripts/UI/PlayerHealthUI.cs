using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : Singleton<PlayerHealthUI>
{
    Text levelText;

    Image healthSlider;

    Image expSlider;

    protected override void Awake()
    {
        base.Awake();
        levelText = transform.GetChild(2).GetComponent<Text>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        DontDestroyOnLoad(this);
    }
    private void OnEnable()
    {
        EventHandler.UpdatePlayerExpUI += OnUpdatePlayerExpUI;
        EventHandler.UpdateHealthUI += OnUpdatePlayerHealthUI;
    }
    private void OnDisable()
    {
        EventHandler.UpdatePlayerExpUI -= OnUpdatePlayerExpUI;
        EventHandler.UpdateHealthUI -= OnUpdatePlayerHealthUI;
    }

    private void OnUpdatePlayerExpUI(CharacterStats playerStats)
    {
        float sliderPercent_Exp = (float)playerStats.characterData.currentExp / playerStats.characterData.baseExp;
        expSlider.fillAmount = sliderPercent_Exp;
        levelText.text = "Level:" + playerStats.characterData.currentLevel.ToString("00");
    }

    private void OnUpdatePlayerHealthUI(CharacterStats playerStats)
    {
        if (playerStats != FindObjectOfType<PlayerController>().GetComponent<CharacterStats>()) return;
        float sliderPercent_HP = (float)playerStats.CurrentHealth / playerStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent_HP;
    }
}
