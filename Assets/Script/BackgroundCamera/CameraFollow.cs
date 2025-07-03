using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public PlayerController playerController; // PlayerController scriptine referans
    public float offsetMultiplier = 3f; // Kameranýn kayma miktarý
    public float cameraSpeedX = 1f; // X eksenindeki takip hýzý

    private CinemachineFramingTransposer framingTransposer;

    void Start()
    {
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        if (framingTransposer != null)
        {
            framingTransposer.m_XDamping = cameraSpeedX;
        }
    }

    void Update()
    {
        if (framingTransposer != null && playerController != null)
        {
            if (playerController.IsFacingRight)
            {
                framingTransposer.m_TrackedObjectOffset = new Vector3(offsetMultiplier, 0, 0);
            }
            else
            {
                framingTransposer.m_TrackedObjectOffset = new Vector3(-offsetMultiplier, 0, 0);
            }
        }
    }

    public void SetCameraSpeedX(float newSpeed)
    {
        if (framingTransposer != null)
        {
            framingTransposer.m_XDamping = newSpeed;
        }
    }
}
