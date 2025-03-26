using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("SpawnPoint Ayari")]
    [SerializeField] private Transform spawnPoint; // SpawnPoint objesi
    [SerializeField] private ParticleSystem spawnEffect; // Efekt (Particle System)
    [SerializeField] private AudioClip spawnSound; // Ses efekti
    private AudioSource audioSource; // Ses oynat�c�

    private void Start()
    {
        // Oyun objesinde ses kayna�� yoksa ekle
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eger oyuncu (Player tag'ine sahip) tabelaya temas ederse
        if (other.CompareTag("Player"))
        {
            // SpawnPoint objesi atanm�� m� kontrol et
            if (spawnPoint != null)
            {
                spawnPoint.position = transform.position; // SpawnPoint tabelan�n konumuna �ekilir
                Debug.Log("Yeni Spawn Noktas� Ayarland�: " + spawnPoint.position);

                // Efekt oynat
                if (spawnEffect != null)
                {
                    Instantiate(spawnEffect, transform.position, Quaternion.identity);
                }

                // Ses efekti �al
                if (spawnSound != null)
                {
                    audioSource.PlayOneShot(spawnSound);
                }

                // UI geri bildirim eklemek istersen burada ekleyebilirsin.
                // �rne�in, bir Text UI ile "Yeni Spawn Noktas� Ayarland�!" mesaj� g�sterebilirsin.
            }
            else
            {
                Debug.LogError("SpawnPoint objesi atanmad�! L�tfen Inspector'dan atay�n.");
            }
        }
    }
}