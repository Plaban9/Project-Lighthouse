using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayCameraController : MonoBehaviour
{
    private CinemachineBrain cinemachineBrain;

    private void Awake()
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
    }

    private void Update()
    {
        if(Input.GetMouseButton(1))
        {
            cinemachineBrain.ManualUpdate();
        }
    }
}
