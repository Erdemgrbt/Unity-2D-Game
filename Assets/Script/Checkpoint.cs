using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("SpawnPoint Ayari")]
    [SerializeField] private Transform spawnPoint; // SpawnPoint objesi
    [SerializeField] private ParticleSystem spawnEffect; // Efekt (Particle System)
    [SerializeField] private AudioClip spawnSound; // Ses efekti
    private AudioSource audioSource; // Ses oynatýcý

    private void Start()
    {
        // Oyun objesinde ses kaynaðý yoksa ekle
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eger oyuncu (Player tag'ine sahip) tabelaya temas ederse
        if (other.CompareTag("Player"))
        {
            // SpawnPoint objesi atanmýþ mý kontrol et
            if (spawnPoint != null)
            {
                spawnPoint.position = transform.position; // SpawnPoint tabelanýn konumuna çekilir
                Debug.Log("Yeni Spawn Noktasý Ayarlandý: " + spawnPoint.position);

                // Efekt oynat
                if (spawnEffect != null)
                {
                    Instantiate(spawnEffect, transform.position, Quaternion.identity);
                }

                // Ses efekti çal
                if (spawnSound != null)
                {
                    audioSource.PlayOneShot(spawnSound);
                }

                // UI geri bildirim eklemek istersen burada ekleyebilirsin.
                // Örneðin, bir Text UI ile "Yeni Spawn Noktasý Ayarlandý!" mesajý gösterebilirsin.
            }
            else
            {
                Debug.LogError("SpawnPoint objesi atanmadý! Lütfen Inspector'dan atayýn.");
            }
        }
    }
}