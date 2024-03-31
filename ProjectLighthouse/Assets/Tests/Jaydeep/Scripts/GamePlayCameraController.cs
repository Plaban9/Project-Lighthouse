using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LighthouseGames.MainMenu
{
    public class GamePlayCameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCameraBase gameplayCam;

        private CinemachineBrain cinemachineBrain;

        private bool canRotateCam;

        private void Awake()
        {
            cinemachineBrain = GetComponentInParent<CinemachineBrain>();
        }

        private void Start()
        {
            cinemachineBrain.ManualUpdate();
        }

        private void Update()
        {
            //canRotateCam = Input.GetMouseButton(1);
            if (canRotateCam)
            {
                cinemachineBrain.ManualUpdate();
            }
        }

        public void ToggleCameraRot(bool value)
        {
            canRotateCam = value;
        }
    }
}
