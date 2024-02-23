namespace Menu.MenuSelectionController
{
    using Menu.ClickableMenuObjects;
    using Menu.MenuCameraControl;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    [Serializable]
    public class SoundEntry
    {
        public string sfxName;
        public AudioClip clip;
    } 

    [RequireComponent(typeof(MenuCameraControl))]
    public class MenuSelectionController : MonoBehaviour
    {
        [SerializeField] private LayerMask _clickableLayer;
        private MenuCameraControl _cameraControl;
        [Header("Sounds")]
        [SerializeField] private AudioSource _buttonSoundsSource;
        [SerializeField] public List<SoundEntry> _soundList;
        


        // Start is called before the first frame update
        void Start()
        {
            _cameraControl = GetComponent<MenuCameraControl>();

        }

        private GameObject _currentMouseOver = null;

        // Update is called once per frame
        void Update()
        {
            MouseControl();
            KeyboardControl();
        }

        private void KeyboardControl()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _cameraControl.Back();
            }
        }

        private void MouseControl()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, _clickableLayer))
            {
                if (_currentMouseOver != hit.collider.gameObject)
                {
                    if (_currentMouseOver != null)
                    {
                        if (_currentMouseOver.TryGetComponent<ClickableMenuObject>
                        (out ClickableMenuObject oldhighlightedObj))
                        {
                            oldhighlightedObj.DisableHighlight();
                        }
                    }
                    _currentMouseOver = hit.collider.gameObject;
                    if (hit.transform.gameObject
                        .TryGetComponent<ClickableMenuObject>
                        (out ClickableMenuObject highlightedObj))
                    {
                        MouseoverSound();
                        highlightedObj.Highlight();
                    }
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.gameObject
                        .TryGetComponent<ClickableMenuObject>
                        (out ClickableMenuObject clickedObject))
                    {
                        MouseClickSound();
                        clickedObject.Activate();
                    }
                }
            }
            else
            {
                if(_currentMouseOver != null)
                {
                    if (_currentMouseOver.TryGetComponent<ClickableMenuObject>
                            (out ClickableMenuObject oldhighlightedObj))
                    {
                        oldhighlightedObj.DisableHighlight();
                    }
                    _currentMouseOver = null;
                }
            }
        }

        private void MouseoverSound()
        {
            _buttonSoundsSource.clip = _soundList.Find( o => o.sfxName == "Mouseover").clip;
            _buttonSoundsSource.time = 0.1f;
            _buttonSoundsSource.Play();
        }

        private void MouseClickSound()
        {
            _buttonSoundsSource.clip = _soundList.Find(o => o.sfxName == "Click").clip;
            _buttonSoundsSource.Play();
        }

    }





}
