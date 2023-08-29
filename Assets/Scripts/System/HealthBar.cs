using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar instance;

    public Slider slider;
    public Gradient gradient;
    [SerializeField] Image fill;

    void Awake()
    {
        instance=this;
    }

    public void SetMaxHealth(float maxhealth)
    {
        slider.maxValue = maxhealth;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        if (slider.value <= 0)
        {
            slider.value = 0;
        }
    }

    
}
