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
        // Cinemachine Virtual Camera'dan Framing Transposer bileþenini al
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        // Eðer playerController baðlanmadýysa sahneden otomatik olarak bulmaya çalýþ
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        // Kamera hýzý baþlangýçta ayarlanýyor
        if (framingTransposer != null)
        {
            framingTransposer.m_XDamping = cameraSpeedX;
        }
    }

    void Update()
    {
        if (framingTransposer != null && playerController != null)
        {
            // IsFacingRight deðerine göre kameranýn X ofsetini ayarla
            if (playerController.IsFacingRight)
            {
                // Sað yöne bakarken ofset pozitif
                framingTransposer.m_TrackedObjectOffset = new Vector3(offsetMultiplier, 0, 0);
            }
            else
            {
                // Sol yöne bakarken ofset negatif
                framingTransposer.m_TrackedObjectOffset = new Vector3(-offsetMultiplier, 0, 0);
            }
        }
    }

    // Kamera hýzýný dinamik olarak deðiþtirmek için bir metot ekle
    public void SetCameraSpeedX(float newSpeed)
    {
        if (framingTransposer != null)
        {
            framingTransposer.m_XDamping = newSpeed;
        }
    }
}
