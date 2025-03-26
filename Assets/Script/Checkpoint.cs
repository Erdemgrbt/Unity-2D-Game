using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("SpawnPoint Ayarý")]
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
        // Eðer oyuncu (Player tag'ine sahip) tabelaya temas ederse
        if (other.CompareTag("Player"))
        {
            // Eðer SpawnPoint zaten bu konumdaysa, efekt ve bildirim tetiklenmez
            if (spawnPoint.position == transform.position)
            {
                Debug.Log("SpawnPoint zaten burada, efekt oynatýlmadý.");
                return; // Ýþlemi burada bitir
            }

            // SpawnPoint'i yeni konuma ayarla
            spawnPoint.position = transform.position;
            Debug.Log("Yeni Spawn Noktasý Ayarlandý: " + spawnPoint.position);

            // Efekt oynat (Eðer efekt tanýmlýysa)
            if (spawnEffect != null)
            {
                Instantiate(spawnEffect, transform.position, Quaternion.identity);
            }

            // Ses efekti çal (Eðer ses tanýmlýysa)
            if (spawnSound != null)
            {
                audioSource.PlayOneShot(spawnSound);
            }

            // UI geri bildirim eklemek istersen burada ekleyebilirsin.
        }
    }
}