using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefab;

    public Transform barPos;

    Transform UIbar;

    Image healthSlider;

    Transform cam;


    private void Awake()
    {
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;
        foreach (var canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                UIbar = Instantiate(healthUIPrefab, canvas.transform).transform;
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();
            }
        }
        EventHandler.UpdateHealthUI += OnUpdateEnemyHealthUI;
    }
    private void OnDisable()
    {
        EventHandler.UpdateHealthUI -= OnUpdateEnemyHealthUI;
    }

    private void OnUpdateEnemyHealthUI(CharacterStats targetStats)
    {
        if (gameObject.GetComponent<CharacterStats>() != targetStats) return;
        if(targetStats.CurrentHealth <= 0){
            Destroy(UIbar.gameObject);
        }

        float sliderPercent = (float)targetStats.CurrentHealth / targetStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }
    private void LateUpdate()
    {
        if (UIbar != null)
        {
            UIbar.position = barPos.position;
            UIbar.forward = -cam.forward;
        }
    }
}
