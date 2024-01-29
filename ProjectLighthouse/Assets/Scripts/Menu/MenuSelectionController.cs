namespace Menu.MenuSelectionController
{
    using Menu.ClickableMenuObjects;
    using Menu.MenuCameraControl;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(MenuCameraControl))]
    public class MenuSelectionController : MonoBehaviour
    {
        private MenuCameraControl _cameraControl;
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
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log($"Mouse over {hit.collider.gameObject.name}");
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
                        highlightedObj.Highlight();
                    }
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.gameObject
                        .TryGetComponent<ClickableMenuObject>
                        (out ClickableMenuObject clickedObject))
                    {
                        clickedObject.Activate();
                    }
                }
            }
            else
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

}
