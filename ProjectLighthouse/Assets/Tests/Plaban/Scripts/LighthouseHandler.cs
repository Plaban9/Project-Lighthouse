using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LighthouseHandler : MonoBehaviour
{
    [Header("Spotlights")]
    [SerializeField] private GameObject _spotlightContainer;
    [SerializeField] private Material _lighthouseMaterial;

    [Header("Audio")]
    [SerializeField] private AudioClip _waves;
    [SerializeField] private AudioClip _seagulls;
    [SerializeField] private AudioSource _audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        TimeController.dayNightCycleStartNotifier += DayNightEventHandler;
    }

    private void Start()
    {
        if (_waves != null)
        {
            AudioSource.PlayClipAtPoint(_waves, transform.position);
        }
    }

    private void DayNightEventHandler(DayNightCycle dayNightCycle)
    {
        switch (dayNightCycle)
        {
            case DayNightCycle.DAY:
                OnDayStarted();
                break;
            case DayNightCycle.NIGHT:
                OnNightStarted();
                break;
        }
    }

    private void OnDayStarted()
    {
        d("Day Started");

        if (_spotlightContainer != null)
        {
            _spotlightContainer.SetActive(false);
        }

        if (_lighthouseMaterial != null)
        {
            _lighthouseMaterial.DisableKeyword("_EMISSION");
        }

        if (_seagulls != null)
        {
            AudioSource.PlayClipAtPoint(_seagulls, transform.position);
        }
    }

    private void OnNightStarted()
    {
        d("Night Started");

        if (_spotlightContainer != null)
        {
            _spotlightContainer.SetActive(true);
        }

        if (_lighthouseMaterial != null)
        {
            _lighthouseMaterial.EnableKeyword("_EMISSION");
        }

        if (_seagulls != null)
        {
            
        }
    }

    private static void d(string message)
    {
        Debug.Log("<<LighthouseHandler>> " + message);
    }
}
