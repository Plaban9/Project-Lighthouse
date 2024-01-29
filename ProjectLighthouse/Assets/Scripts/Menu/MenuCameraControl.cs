namespace Menu.MenuCameraControl
{
    using Cinemachine;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using static UnityEngine.Rendering.DebugUI;

    public class MenuCameraControl : MonoBehaviour
    {
        // Start is called before the first frame update
        [Header("Cameras")]
        [SerializeField] CinemachineVirtualCamera mainMenuCamera;
        [SerializeField] CinemachineVirtualCamera settMenuCamera;
        [SerializeField] CinemachineVirtualCamera diffMenuCamera;
        [SerializeField] CinemachineVirtualCamera gameCamera;

        [Header("Settings")]
        [SerializeField] float timeInMsToLift = 20f;

        [Header("Buttons & Images")]
        [SerializeField] GameObject logo;
        [SerializeField] Material gameMaterial;
        [SerializeField] AnimationCurve logoAnimation;

        private CameraPositions _currentCameraPosition = CameraPositions.MAIN_MENU;

        void Start()
        {
            StartCoroutine(StartCamera());
            StartCoroutine(LogoFadeInFadeOut());
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

        private enum CameraPositions
        {
            MAIN_MENU,
            SETTINGS,
            DIFFICULTY,
            GAME
        }

        private void SetCameraPosition(CameraPositions cameraPosition)
        {
            DeactivateAllCameras();
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
