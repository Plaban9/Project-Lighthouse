using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace LighthouseGames.UI.Effects
{
    public class LightBeamColorChanger : MonoBehaviour
    {
        [Header("Text Settings")]
        [SerializeField] private string _textToDisplay;
        [SerializeField] private TextMeshProUGUI _lightCommencementText;

        [Header("Light Beam Settings")]
        [SerializeField] private bool _showLightBeam;
        [SerializeField] private Image _lightRay;
        [SerializeField] private Mask _lightRayMask;

        [Header("Light Source Settings")]
        [SerializeField] private Image _lightSource;

        [Header("Global Settings")]
        [SerializeField] Color _themeColor;
        [SerializeField] Animator _lightAnimator;

        private void Awake()
        {
            if (_lightCommencementText != null)
            {
                _lightCommencementText.color = _themeColor;
                _lightCommencementText.text = _textToDisplay;
            }

            if (_lightRayMask != null)
            {
                _lightRayMask.showMaskGraphic = _showLightBeam;
            }

            if (_lightRay != null)
            {
                _lightRay.color = new Color(_themeColor.r, _themeColor.g, _themeColor.b);
            }

            if (_lightSource != null)
            {
                _lightSource.color = new Color(_themeColor.r, _themeColor.g, _themeColor.b);
            }

            if (_lightAnimator == null)
            {
                _lightAnimator = GetComponent<Animator>();
            }
        }

        private void Update()
        {
            NotifyColorChange();
        }

        private void NotifyColorChange()
        {
            if (_lightCommencementText != null && _lightRay != null && _lightSource != null && _lightRayMask != null)
            {
                if (_themeColor != _lightCommencementText.color)
                {
                    D("Color Changed - New: " + _themeColor + ", Old: " + _lightCommencementText.color);

                    _lightCommencementText.color = _themeColor;
                    _lightRay.color = new Color(_themeColor.r, _themeColor.g, _themeColor.b);
                    _lightSource.color = new Color(_themeColor.r, _themeColor.g, _themeColor.b);

                }

                if (_lightRayMask.showMaskGraphic != _showLightBeam)
                {
                    _lightRayMask.showMaskGraphic = _showLightBeam;
                }

                if (!_lightCommencementText.text.Equals(_textToDisplay))
                {
                    _lightCommencementText.text = _textToDisplay;
                }

                return;
            }

            D("Error in changing color. One of the references are not assigned");
        }


        public void TriggerEffect()
        {
            _lightAnimator.SetTrigger("Enable");
        }

        private static void D(string message)
        {
            Debug.Log("<<LightBeamColorChanger>> " + message);
        }
    }
}
