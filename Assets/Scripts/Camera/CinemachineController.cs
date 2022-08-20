using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineController : Singleton<CinemachineController>
{

    private CinemachineVirtualCamera virtualCamera;

    private CinemachineFreeLook freeLookCamera;

    private int index = 1;

    void Start()
    {
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        freeLookCamera = GetComponentInChildren<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            virtualCamera.Priority +=index;
            freeLookCamera.Priority += -index;
            index = -index;
        }
    }
}
