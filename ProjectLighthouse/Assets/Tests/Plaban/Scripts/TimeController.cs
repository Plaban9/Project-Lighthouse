using System;

using TMPro;

using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private float _timeMultiplier;
    [SerializeField] private float _startHour;
    [SerializeField] private TextMeshProUGUI _currentTimeText;
    [SerializeField] private float _sunriseHour;
    [SerializeField] private float _sunsetHour;
    [SerializeField] private float _fogStartHour;
    [SerializeField] private float _fogEndHour;


    [Header("Directional Light")]
    [SerializeField] private Light _sunlight;
    [SerializeField] private Light _moonlight;

    private TimeSpan _sunriseTime;
    private TimeSpan _sunsetTime;
    private TimeSpan _fogStartTime;
    private TimeSpan _fogEndTime;

    private DateTime _currentTime; // To make calculation easier

    // Ambient Light
    [Header("Ambient Light")]
    [SerializeField] private Color _dayTimeAmbientColor;
    [SerializeField] private Color _nightTimeAmbientColor;
    [SerializeField] private AnimationCurve _lightChangeCurve;
    [SerializeField] private float _maxSunlightIntensity;
    [SerializeField] private float _maxMoonlightIntensity;

    // Fog
    [Header("Fog")]
    [SerializeField] private float _maxFogIntensity;
    [SerializeField] private Gradient _fogColor;
    [SerializeField] private AnimationCurve _fogIntensityCurve;

    private DayNightCycle _dayNightCycle;


    // defines the "pattern" of the event
    public delegate void DayNightEventHandler(DayNightCycle dayNightCycle);
    // the event itself
    public static event DayNightEventHandler dayNightCycleStartNotifier;

    private bool _frozenTime = false;

    // Start is called before the first frame update
    void Start()
    {
        //DEBUG
        //var now = DateTime.Now;
        //_currentTime = new DateTime(now.Year, now.Month, now.Day, 19, 0, 0);
        //


        _currentTime = DateTime.Now + TimeSpan.FromHours(_startHour);
        _sunriseTime = TimeSpan.FromHours(_sunriseHour);
        _sunsetTime = TimeSpan.FromHours(_sunsetHour);
        _fogStartTime = TimeSpan.FromHours(_fogStartHour);
        _fogEndTime = TimeSpan.FromHours(_fogEndHour);

        _dayNightCycle = IsDay() ? DayNightCycle.DAY : DayNightCycle.NIGHT;
        NotifyDayNightCycleChange();
    }

    //void Update()
    //{
    //    UpdateTimeOfDay();
    //    RotateSun();
    //    UpdateLightSettings();
    //    UpdateFogSettings();
    //}

    void FixedUpdate()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
        UpdateFogSettings();
    }

    private void UpdateTimeOfDay()
    {
        if (_frozenTime) { return; }
        _currentTime = _currentTime.AddSeconds(Time.fixedDeltaTime + _timeMultiplier);

        if (_currentTimeText != null)
        {
            _currentTimeText.text = _currentTime.ToString("HH:mm");
        }
    }

    private bool IsDay()
    {
        return _currentTime.TimeOfDay > _sunriseTime && _currentTime.TimeOfDay < _sunsetTime;
    }

    private void RotateSun()
    {
        float sunlightRotation;

        if (IsDay()) // Day
        {
            if (_dayNightCycle != DayNightCycle.DAY)
            {
                _dayNightCycle = DayNightCycle.DAY;
                NotifyDayNightCycleChange();
            }

            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(_sunriseTime, _sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(_sunriseTime, _currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;
            sunlightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else // Night
        {
            if (_dayNightCycle != DayNightCycle.NIGHT)
            {
                _dayNightCycle = DayNightCycle.NIGHT;
                NotifyDayNightCycleChange();
            }

            TimeSpan sunsetToSunrise = CalculateTimeDifference(_sunsetTime, _sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(_sunsetTime, _currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunrise.TotalMinutes;
            sunlightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        _sunlight.transform.rotation = Quaternion.AngleAxis(sunlightRotation, Vector3.right); // To rotate around x - axis
        
        
        //Quaternion currentRotation = Quaternion.AngleAxis(sunlightRotation, Vector3.right); // Workaround for shadow
        //Vector3 rotationInEulerAngles = currentRotation.eulerAngles;
        ////rotationInEulerAngles.y = -135f;
        ////rotationInEulerAngles.z = 0f;
        //_sunlight.transform.localRotation = Quaternion.Euler(rotationInEulerAngles);

    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(_sunlight.transform.forward, Vector3.down); //-1 is vertically down, 0 is horizontal, +1 is vertically up

        _sunlight.intensity = Mathf.Lerp(0, _maxSunlightIntensity, _lightChangeCurve.Evaluate(dotProduct));
        _moonlight.intensity = Mathf.Lerp(_maxMoonlightIntensity, 0, _lightChangeCurve.Evaluate(dotProduct));

        RenderSettings.ambientLight = Color.Lerp(_nightTimeAmbientColor, _dayTimeAmbientColor, _lightChangeCurve.Evaluate(dotProduct));
    }

    private void UpdateFogSettings()
    {
        RenderSettings.fog = _currentTime.TimeOfDay > _fogStartTime || _currentTime.TimeOfDay < _fogEndTime;
        if (RenderSettings.fog)
        {
            TimeSpan _fogDuration = CalculateTimeDifference(_fogStartTime, _fogEndTime);
            TimeSpan timeSinceFogStart = CalculateTimeDifference(_fogStartTime, _currentTime.TimeOfDay);

            double percentage = timeSinceFogStart.TotalMinutes / _fogDuration.TotalMinutes;

            //RenderSettings.fogColor = Color.Lerp(_fogColor.Evaluate(0f), _fogColor.Evaluate(1f), _fogIntensityCurve.Evaluate((float)percentage));
            RenderSettings.fogColor = RenderSettings.ambientLight;
            RenderSettings.fogDensity = Mathf.Lerp(0, _maxFogIntensity, _fogIntensityCurve.Evaluate((float)percentage));
        }
    }

    private void NotifyDayNightCycleChange()
    {
        if (dayNightCycleStartNotifier != null)
        {
            dayNightCycleStartNotifier(_dayNightCycle);
        }
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if (difference.TotalSeconds < 0) // Add 24 hrs
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }

    public void FreezeTime(bool frozenTime)
    {
        _frozenTime = frozenTime;
    }
}
