using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LighthouseGames.MainMenu
{
    public class GamePlayCameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook gameplayCam;

        private CinemachineBrain cinemachineBrain;

        private bool canRotateCam;

        private void Awake()
        {
            cinemachineBrain = GetComponentInParent<CinemachineBrain>();
        }

        private void Start()
        {
            cinemachineBrain.ManualUpdate();

#if UNITY_ANDROID
            gameplayCam.m_YAxis.m_InputAxisName = "Mouse Y";
#else
            gameplayCam.m_YAxis.m_InputAxisName = "Mouse ScrollWheel";
#endif
        }

        private void Update()
        {
#if !UNITY_ANDROID
            canRotateCam = Input.GetMouseButton(1);
#else
            canRotateCam = Input.touchCount == 2;
#endif
            if (canRotateCam)
            {
                cinemachineBrain.ManualUpdate();
            }
        }
    }
}
