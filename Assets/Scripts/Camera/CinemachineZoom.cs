using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineZoom : MonoBehaviour
{
    private CinemachineFreeLook freeLookCamera;

    private float zoomSpeed = 5f;

    private float zoomAcceleration = 2f;

    private float zoomInnerRange = 3f;

    private float zoomOutRange = 15f;

    private float currentMiddleRigRadius;

    private float newMiddleRigRadius;

    private float zoomYAxis=0f;


    // Start is called before the first frame update
    void Start()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        currentMiddleRigRadius = freeLookCamera.m_Orbits[1].m_Radius;
        newMiddleRigRadius = currentMiddleRigRadius;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        zoomYAxis = Input.GetAxis("Mouse ScrollWheel");
        AdjustCameraZoomIndex(zoomYAxis);
        UpdateZoomLevel();
    }

    private void UpdateZoomLevel()
    {
        if (currentMiddleRigRadius == newMiddleRigRadius) return;
        currentMiddleRigRadius = Mathf.Lerp(currentMiddleRigRadius, newMiddleRigRadius, zoomAcceleration * Time.deltaTime);
        currentMiddleRigRadius = Mathf.Clamp(currentMiddleRigRadius, zoomInnerRange, zoomOutRange);

        freeLookCamera.m_Orbits[1].m_Radius = currentMiddleRigRadius;
        freeLookCamera.m_Orbits[0].m_Height = freeLookCamera.m_Orbits[1].m_Radius;
    }

    public void AdjustCameraZoomIndex(float zoomYAxis)
    {
        if (zoomYAxis == 0) return;
        else if (zoomYAxis < 0)
        {
            newMiddleRigRadius = currentMiddleRigRadius + zoomSpeed;
        }
        else if(zoomYAxis>0)
        {
            newMiddleRigRadius = currentMiddleRigRadius - zoomSpeed;
        }
    }
}
