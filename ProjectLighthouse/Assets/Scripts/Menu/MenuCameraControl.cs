namespace Menu.MenuCameraControl
{
    using Cinemachine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using static UnityEngine.Rendering.DebugUI;

    public class MenuCameraControl : MonoBehaviour
    {
        // Start is called before the first frame update
        [Header("Cameras")]
        [SerializeField] private CinemachineVirtualCamera mainMenuCamera;
        [SerializeField] private CinemachineVirtualCamera settMenuCamera;
        [SerializeField] private CinemachineVirtualCamera diffMenuCamera;
        [SerializeField] private CinemachineVirtualCamera gameCamera;
        [SerializeField] private CinemachineBrain cinemachineBrain;


        [Header("Menus")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject settMenu;
        [SerializeField] private GameObject diffMenu;
        [SerializeField] private AnimationCurve fadeAnim;
        [SerializeField] private float fadeDuration;


        [Header("Settings")]
        [SerializeField] private float timeInMsToLift = 20f;

        [Header("Buttons & Images")]
        [SerializeField] private GameObject logo;
        [SerializeField] private Material gameMaterial;
        [SerializeField] private AnimationCurve logoAnimation;

        private enum CameraPositions
        {
            LOGO,
            MAIN_MENU,
            SETTINGS,
            DIFFICULTY,
            GAME
        }

        private bool isShowingMenu;
        private CameraPositions _currentCameraPosition = CameraPositions.LOGO;


        void Start()
        {
            StartCoroutine(StartCamera());
            StartCoroutine(LogoFadeInFadeOut());
        }

        private void Update()
        {
            if(!isShowingMenu)
            {
                if (!cinemachineBrain.IsBlending)
                {
                    ShowMenu(_currentCameraPosition);
                }
            }
        }

        IEnumerator StartCamera()
        {
            for (int i = 0; i < timeInMsToLift; i++)
            {
                yield return null;
            }
            ToMainMenu();
        }

        IEnumerator LogoFadeInFadeOut()
        {
            for (float i = 0; i < timeInMsToLift + 1; i++)
            {
                float value = logoAnimation.Evaluate(i / timeInMsToLift);
                logo.GetComponent<Renderer>().material.SetFloat("_Visibility", value);
                yield return null;
            }
        }

        private void HideMenu(CameraPositions currentCameraPosition, Action callback)
        {
            Debug.Log("Aie!");
            Action action = () => { callback.Invoke(); isShowingMenu = false; };
            switch (currentCameraPosition)
            {
                case CameraPositions.MAIN_MENU:
                    FadeOut(mainMenu, action);
                    break;
                case CameraPositions.SETTINGS:
                    FadeOut(settMenu, action);
                    break;
                case CameraPositions.DIFFICULTY:
                    FadeOut(diffMenu, action);
                    break;
                case CameraPositions.LOGO:
                    FadeOut(logo, action);
                    break;
            }
        }

        private void ShowMenu(CameraPositions currentCameraPos)
        {
            isShowingMenu = true;
            switch (currentCameraPos)
            {
                case CameraPositions.MAIN_MENU:
                    FadeIn(mainMenu);
                    break;
                case CameraPositions.SETTINGS:
                    FadeIn(settMenu);
                    break;
                case CameraPositions.DIFFICULTY:
                    FadeIn(diffMenu);
                    break;
            }
        }

        private void FadeIn(GameObject gb)
        {
            gb.SetActive(true);
            StartCoroutine(FadeFromCoroutine(gb, 0, 1, fadeAnim, fadeDuration, () => { }));
        }

        private void FadeOut(GameObject gb, Action callback)
        {
            Debug.Log("Is fading out");
            StartCoroutine(FadeFromCoroutine(gb, 1, 0, fadeAnim, fadeDuration, 
                () => {
                    callback?.Invoke();
                    Debug.Log("callbackcalled");
                    gb.SetActive(false); 
                     }));
        }

        IEnumerator FadeFromCoroutine(GameObject gb, float from, float to, AnimationCurve animCurve, float time, Action callback)
        {
            foreach (Transform t in gb.transform){
                if(t.TryGetComponent<CanvasRenderer>(out CanvasRenderer render))
                {
                    render.SetAlpha(0);
                }
            }
            Debug.Log("Before");

            for (float i = 0f; i < 1f; i+= 1.0f/time)
            {
                float progress =  animCurve.Evaluate(i);
                float val = (from * (1-progress) ) + to * animCurve.Evaluate(i);
                foreach (Transform t in gb.transform)
                {
                    if (t.TryGetComponent<CanvasRenderer>(out CanvasRenderer render))
                    {
                        render.SetAlpha(val);
                    }
                }
                Debug.Log($"Fading... {val}");

                yield return null;
            }
            Debug.Log("After");

            foreach (Transform t in gb.transform)
            {
                if (t.TryGetComponent<CanvasRenderer>(out CanvasRenderer render))
                {
                    render.SetAlpha(to);
                }
            }
            callback.Invoke();
        }


        private void SetCameraPosition(CameraPositions cameraPosition)
        {
            DeactivateAllCameras();
            Action callback = () =>
            {
                _currentCameraPosition = cameraPosition;
                switch (cameraPosition)
                {
                    case CameraPositions.MAIN_MENU:
                        mainMenuCamera.gameObject.SetActive(true);
                        break;
                    case CameraPositions.SETTINGS:
                        settMenuCamera.gameObject.SetActive(true);
                        break;
                    case CameraPositions.DIFFICULTY:
                        diffMenuCamera.gameObject.SetActive(true);
                        break;
                    case CameraPositions.GAME:
                        gameCamera.gameObject.SetActive(true);
                        break;
                }
            };
            HideMenu(_currentCameraPosition, callback);
            
        }

        private void DeactivateAllCameras()
        {
            mainMenuCamera.gameObject.SetActive(false);
            settMenuCamera.gameObject.SetActive(false);
            diffMenuCamera.gameObject.SetActive(false);
            gameCamera.gameObject.SetActive(false);

        }

        public void ToSettings()
        {
            SetCameraPosition(CameraPositions.SETTINGS);
        }

        public void ToDifficulty()
        {
            SetCameraPosition(CameraPositions.DIFFICULTY);
        }

        public void ToMainMenu()
        {
            SetCameraPosition(CameraPositions.MAIN_MENU);
        }

        public void ToGame()
        {
            SetCameraPosition(CameraPositions.GAME);
        }

        public void Back()
        {
            switch (_currentCameraPosition)
            {
                case CameraPositions.MAIN_MENU:
                    break;
                case CameraPositions.GAME:
                    break;
                case CameraPositions.DIFFICULTY:
                case CameraPositions.SETTINGS:
                    ToMainMenu();
                    break;
            }
        }
    }

}
