using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LighthouseHandler : MonoBehaviour
{
    [Header("Spotlights")]
    [SerializeField] private GameObject _spotlightContainer;
    [SerializeField] private Material _lighthouseMaterial;

    [Header("Audio")]
    [SerializeField] private AudioSource _waves;
    [SerializeField] private AudioClip _siren;
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
            _waves.Play();
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

        if (_audioSource != null)
        {
            _audioSource.Play();
        }
    }

    private void OnNightStarted()
    {
        d("Night Started");

        if (_audioSource != null)
        {
            _audioSource.Stop();
        }

        if (_siren != null)
        {
            AudioSource.PlayClipAtPoint(_siren, transform.position, 5f);

            StartCoroutine(nameof(IlluminateLighthouse), _siren.length);
        }
    }

    private IEnumerator IlluminateLighthouse(float time)
    {
        yield return new WaitForSeconds(time);

        if (_spotlightContainer != null)
        {
            _spotlightContainer.SetActive(true);
        }

        if (_lighthouseMaterial != null)
        {
            _lighthouseMaterial.EnableKeyword("_EMISSION");
        }
    }

    private static void d(string message)
    {
        Debug.Log("<<LighthouseHandler>> " + message);
    }
}
