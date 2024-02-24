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
        private CinemachineBrain.UpdateMethod _backupUpdateMethod;

        private void Awake()
        {
            cinemachineBrain = GetComponentInParent<CinemachineBrain>();
            _backupUpdateMethod = cinemachineBrain.m_UpdateMethod;
        }

        private void Start()
        {
            cinemachineBrain.ManualUpdate();
        }

        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                cinemachineBrain.ManualUpdate();
            }
        }

        private void SetUpGameplayCamera()
        {
            gameplayCam.Priority = 20;
            cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.ManualUpdate;
        }
    }

}
