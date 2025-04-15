using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("SpawnPoint Ayar�")]
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
        // E�er oyuncu (Player tag'ine sahip) tabelaya temas ederse
        if (other.CompareTag("Player"))
        {
            // E�er SpawnPoint zaten bu konumdaysa, efekt ve bildirim tetiklenmez
            if (spawnPoint.position == transform.position)
            {
                Debug.Log("SpawnPoint zaten burada, efekt oynat�lmad�.");
                return; // ��lemi burada bitir
            }

            // SpawnPoint'i yeni konuma ayarla
            spawnPoint.position = transform.position;
            Debug.Log("Yeni Spawn Noktas� Ayarland�: " + spawnPoint.position);

            // Efekt oynat (E�er efekt tan�ml�ysa)
            if (spawnEffect != null)
            {
                Instantiate(spawnEffect, transform.position, Quaternion.identity);
            }

            // Ses efekti �al (E�er ses tan�ml�ysa)
            if (spawnSound != null)
            {
                audioSource.PlayOneShot(spawnSound);
            }

            // UI geri bildirim eklemek istersen burada ekleyebilirsin.
        }
    }
}