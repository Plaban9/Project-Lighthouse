namespace Menu.MenuCameraControl
{
    using Cinemachine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using static UnityEngine.Rendering.DebugUI;

    public class MenuCameraControl : MonoBehaviour
    {
        // Start is called before the first frame update
        [Header("Game Controller")]
        [SerializeField] private TimeController timeController;

        [Header("Cameras")]
        [SerializeField] private CinemachineVirtualCamera logoCamera;
        [SerializeField] private CinemachineVirtualCamera mainMenuCamera;
        [SerializeField] private CinemachineVirtualCamera settMenuCamera;
        [SerializeField] private CinemachineVirtualCamera diffMenuCamera;
        [SerializeField] private CinemachineVirtualCameraBase gameCamera;
        [SerializeField] private CinemachineVirtualCamera creditCamera;
        [SerializeField] private CinemachineBrain cinemachineBrain;


        [Header("Menus")]
        [SerializeField] private GameObject settMenu;
        [SerializeField] private GameObject diffMenu;
        [SerializeField] private GameObject gameUI;
        [SerializeField] private AnimationCurve fadeAnim;
        [SerializeField] private float fadeDuration;

        [Header("When Game Starts")]
        [SerializeField] private List<GameObject> enable;
        [SerializeField] private List<GameObject> disable;


        [Header("Settings")]
        [SerializeField] private float timeInMsToLift = 20f;

        [Header("Buttons & Images")]
        [SerializeField] private GameObject logo;
        [SerializeField] private Material logoMaterial;
        [SerializeField] private AnimationCurve logoAnimation;
        [SerializeField] private GameObject title;
        [SerializeField] private float titleDuration;
        [SerializeField] private AnimationCurve titleAnimation;
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
            cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
            foreach (GameObject gb in enable)
            {
                gb?.SetActive(false);
            }
            DeactivateAllCameras();
            logoCamera.Priority = 15;
            title.GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 0f);
            timeController.FreezeTime(true);
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
            Action action = () => { callback.Invoke(); isShowingMenu = false; };
            switch (currentCameraPosition)
            {
                case CameraPositions.MAIN_MENU:
                    action();
                    break;
                case CameraPositions.SETTINGS:
                    StartCoroutine(FadeOutTitle());
                    FadeOut(settMenu, action);
                    break;
                case CameraPositions.DIFFICULTY:
                    FadeOut(diffMenu, action);
                    break;
                case CameraPositions.LOGO:
                    FadeOut(logo, action);
                    break;
                case CameraPositions.GAME:
                    StartCoroutine(FadeOutTitle());
                    action.Invoke();
                    break;
            }
        }

        private void ShowMenu(CameraPositions currentCameraPos)
        {
            isShowingMenu = true;
            switch (currentCameraPos)
            {
                case CameraPositions.MAIN_MENU:
                    StartCoroutine(FadeInTitle());
                    break;
                case CameraPositions.SETTINGS:
                    FadeIn(settMenu);
                    break;
                case CameraPositions.DIFFICULTY:
                    FadeIn(diffMenu);
                    break;
                case CameraPositions.GAME:
                    FadeIn(gameUI);
                    break;
            }
        }

        private IEnumerator FadeInTitle()
        {
            title.GetComponent<MeshRenderer>().material.EnableKeyword("GLOW_ON");
            for (float i = 0.0f; i < 1.0f; i+= 1.0f / titleDuration)
            {
                title.GetComponent<TextMeshPro>().color = new Color(1, 1, 1,
                    titleAnimation.Evaluate(i));
                title.GetComponent<MeshRenderer>().material.SetFloat("_GlowPower", i);
                yield return null;
            }
            title.GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 1);
        }

        private IEnumerator FadeOutTitle()
        {
            for (float i = 1.0f; i > 0.0f; i -= 1.0f / titleDuration)
            {
                title.GetComponent<TextMeshPro>().color = new Color(1, 1, 1,
                    titleAnimation.Evaluate(i));
                title.GetComponent<MeshRenderer>().material.SetFloat("_GlowPower", i);
                yield return null;
            }
            title.GetComponent<TextMeshPro>().color = new Color(0, 0, 0, 1);
            title.GetComponent<MeshRenderer>().material.SetFloat("_GlowPower", 0);
            title.SetActive(false);
        }



        private void FadeIn(GameObject gb)
        {
            if(gb != null) gb.SetActive(true);
            StartCoroutine(FadeFromCoroutine(gb, 0, 1, fadeAnim, fadeDuration, () => { }));
        }

        private void FadeOut(GameObject gb, Action callback)
        {
            StartCoroutine(FadeFromCoroutine(gb, 1, 0, fadeAnim, fadeDuration, 
                () => {
                    callback?.Invoke();
                    gb.SetActive(false); 
                     }));
        }

        IEnumerator FadeFromCoroutine(
            GameObject gb, float from, float to, 
            AnimationCurve animCurve, float time, Action callback)
        {
            if (gb != null)
            {
                foreach (Transform t in gb.transform)
                {
                    if (t.TryGetComponent<CanvasRenderer>(out CanvasRenderer render))
                    {
                        render.SetAlpha(0);
                    }

                }

                for (float i = 0f; i < 1f; i += 1.0f / time)
                {
                    float progress = animCurve.Evaluate(i);
                    float val = (from * (1 - progress)) + to * animCurve.Evaluate(i);
                    foreach (Transform t in gb.transform)
                    {
                        if (t.TryGetComponent<CanvasRenderer>(out CanvasRenderer render))
                        {
                            render.SetAlpha(val);
                        }
                    }

                    yield return null;
                }

                foreach (Transform t in gb.transform)
                {
                    if (t.TryGetComponent<CanvasRenderer>(out CanvasRenderer render))
                    {
                        render.SetAlpha(to);
                    }
                }
            }
            callback();
        }


        private void SetCameraPosition(CameraPositions cameraPosition)
        {
            DeactivateAllCameras();
            _currentCameraPosition = cameraPosition;

            Action callback = () =>
            {
                switch (cameraPosition)
                {
                    case CameraPositions.MAIN_MENU:
                        mainMenuCamera.Priority = 15;
                        break;
                    case CameraPositions.SETTINGS:
                        settMenuCamera.Priority = 15;
                        break;
                    case CameraPositions.DIFFICULTY:
                        diffMenuCamera.Priority = 15;
                        break;
                    case CameraPositions.GAME:
                        gameCamera.Priority = 15;
                        GameStartProcedure();
                        break;
                }
            };
            HideMenu(_currentCameraPosition, callback);
            
        }

        private void DeactivateAllCameras()
        {
            mainMenuCamera.Priority = 10;
            settMenuCamera.Priority = 10;
            diffMenuCamera.Priority = 10;
            gameCamera.Priority = 10;
            creditCamera.Priority = 10;
            logoCamera.Priority = 10;

        }

        public void ToSettings()
        {
            SetCameraPosition(CameraPositions.SETTINGS);
        }

        public void ToDifficulty()
        {
            SetCameraPosition(CameraPositions.GAME);
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

        private void GameStartProcedure()
        {
            foreach (GameObject gb in enable)
            {
                gb?.SetActive(true);
            }
            foreach(GameObject gb in disable)
            {
                gb?.SetActive(false);
            }
            timeController.FreezeTime(false);
        }
    }

}
