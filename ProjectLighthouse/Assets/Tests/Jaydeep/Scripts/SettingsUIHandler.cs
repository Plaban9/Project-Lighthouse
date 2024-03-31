using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUIHandler : MonoBehaviour
{
    [Header("Settings UI")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxPercentageText;

    public void OnSFXValueChanged(float value)
    {
        if (value < 10)
            sfxSlider.value = 0;
        sfxPercentageText.text = $"{sfxSlider.value}\n%";
    }
}
