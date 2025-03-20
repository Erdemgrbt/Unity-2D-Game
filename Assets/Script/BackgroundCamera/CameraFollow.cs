using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public PlayerController playerController; // PlayerController scriptine referans
    public float offsetMultiplier = 3f; // Kameran�n kayma miktar�
    public float cameraSpeedX = 1f; // X eksenindeki takip h�z�

    private CinemachineFramingTransposer framingTransposer;

    void Start()
    {
        // Cinemachine Virtual Camera'dan Framing Transposer bile�enini al
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        // E�er playerController ba�lanmad�ysa sahneden otomatik olarak bulmaya �al��
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        // Kamera h�z� ba�lang��ta ayarlan�yor
        if (framingTransposer != null)
        {
            framingTransposer.m_XDamping = cameraSpeedX;
        }
    }

    void Update()
    {
        if (framingTransposer != null && playerController != null)
        {
            // IsFacingRight de�erine g�re kameran�n X ofsetini ayarla
            if (playerController.IsFacingRight)
            {
                // Sa� y�ne bakarken ofset pozitif
                framingTransposer.m_TrackedObjectOffset = new Vector3(offsetMultiplier, 0, 0);
            }
            else
            {
                // Sol y�ne bakarken ofset negatif
                framingTransposer.m_TrackedObjectOffset = new Vector3(-offsetMultiplier, 0, 0);
            }
        }
    }

    // Kamera h�z�n� dinamik olarak de�i�tirmek i�in bir metot ekle
    public void SetCameraSpeedX(float newSpeed)
    {
        if (framingTransposer != null)
        {
            framingTransposer.m_XDamping = newSpeed;
        }
    }
}
