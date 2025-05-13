using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider Slider;

    public void SetMaxHealth(float health)
    {
        Slider.maxValue = health;
        Slider.value = health;
    }
    
    public void SetHealth(float health)
    {
        Slider.value = health;
    }
}
