using LighthouseGames.UI.Effects;

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
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _sfxAudioSource;

    [Header("UI")]
    [SerializeField] private LightBeamColorChanger _canvasNightNotifyEffect;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    private void OnEnable()
    {
        TimeController.dayNightCycleStartNotifier += DayNightEventHandler;
    }

    private void OnDisable()
    {
        TimeController.dayNightCycleStartNotifier -= DayNightEventHandler;
    }

    private void Start()
    {
        if (_waves != null)
        {
            _waves.Play();
        }

        if (_canvasNightNotifyEffect == null)
        {
            _canvasNightNotifyEffect = FindAnyObjectByType<LightBeamColorChanger>();
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

        if (_musicAudioSource != null)
        {
            _musicAudioSource.Play();
        }
    }

    private void OnNightStarted()
    {
        d("Night Started");

        if (_musicAudioSource != null)
        {
            _musicAudioSource.Stop();
        }

        if (_siren != null)
        {
            d("Playing Siren");
            _sfxAudioSource.PlayOneShot(_siren, 2f);
            ////_sfxAudioSource.Play();
            //AudioSource.PlayClipAtPoint(_siren, transform.position);

            if (_canvasNightNotifyEffect != null)
            {
                _canvasNightNotifyEffect.TriggerEffect();
            }

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
