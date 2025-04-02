using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class Health : MonoBehaviour
{
    [Header("Saglik")]
    [SerializeField] private float startingHealth; // Baslangic sagligi
    public float currentHealth { get; private set; } // Mevcut saglik (disaridan sadece okunabilir)
    private Animator anim; // Animator referansi
    public bool dead; // Karakterin olum durumu

    [Header("Yenilmezlik Durumu")]
    public float invincibilityDuration; // Hasar alindiktan sonra kac saniye yenilmez olunacak
    private bool isInvincible = false; // Yenilmezlik durumu

    private Vector3 originalScale; // Karakterin orijinal olcegi

    public Transform spawnPoint; // Yeniden dogma noktasi
    public RangeAttack rangeAttack; // Menzilli saldiri kontrolu

    private void Awake()
    {
        currentHealth = startingHealth; // Baslangic sagligini ayarla
        anim = GetComponent<Animator>(); // Animator bilesenini al
        originalScale = transform.localScale; // Baslangicta olcegi kaydet
    }

    public void TakeDamage(float _damage)
    {
        if (isInvincible) return; // Eger yenilmezse hasar alma

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth); // Sagligi sifirin altina dusmeyecek sekilde ayarla

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt"); // Yaralanma animasyonu tetikle
            StartCoroutine(InvincibilityTimer()); // Yenilmezlik zamanlayicisini baslat
        }
        else
        {
            if (!dead) // Eger zaten olum durumundaysa tekrar isleme
            {
                anim.SetTrigger("die"); // Olum animasyonunu tetikle

                // Karakter oyuncuysa
                if (gameObject.CompareTag("Player"))
                {
                    if (GetComponent<PlayerController>() != null)
                    {
                        GetComponent<PlayerController>().enabled = false; // PlayerController'i devre disi birak
                    }

                    if (GetComponent<PlayerInput>() != null)
                    {
                        GetComponent<PlayerInput>().enabled = false; // PlayerInput'u devre disi birak
                    }

                    originalScale = transform.localScale; // Mevcut olcegi kaydet
                    transform.localScale = originalScale; // Olcegi koru

                    if (rangeAttack != null)
                    {
                        rangeAttack.active = 0; // Menzilli saldiriyi kapat
                        Debug.Log("RangeAttack kapandi!");
                    }
                }

                // Karakter dusmansa
                if (gameObject.CompareTag("Enemy"))
                {
                    if (GetComponentInParent<EnemyPatrol>() != null)
                    {
                        GetComponentInParent<EnemyPatrol>().enabled = false; // Devriye davranisini devre disi birak
                    }

                    if (GetComponent<MeleeEnemy>() != null)
                    {
                        GetComponent<MeleeEnemy>().enabled = false; // Yakin dovus dusmanini devre disi birak
                    }

                    if (GetComponent<RangedEnemy>() != null)
                    {
                        GetComponent<RangedEnemy>().enabled = false; // menzilli dusmanini devre disi birak
                    }

                    Collider2D enemyCollider = GetComponent<Collider2D>(); // Dusmanin colliderini al
                    if (enemyCollider != null)
                    {
                        enemyCollider.enabled = false; // Collideri devre disi birak
                    }

                    Rigidbody2D rb = GetComponent<Rigidbody2D>(); // Dusmanin rigidbody'sini al
                    if (rb != null)
                    {
                        rb.bodyType = RigidbodyType2D.Static; // Fizik motorunu devre disi birak
                    }

                    Destroy(gameObject, 3f); // 5 saniye sonra nesneyi yok et
                }
                dead = true; // Olum durumunu aktif et
            }
        }
    }

    // Yenilmezlik zamanlayicisi
    IEnumerator InvincibilityTimer()
    {
        isInvincible = true; // Yenilmezligi ac
        yield return new WaitForSeconds(invincibilityDuration); // Belirtilen sure kadar bekle
        isInvincible = false; // Yenilmezligi kapat
    }

    private void Update()
    {
        // Test amacli, E tusuna basinca karaktere 1 hasar ver
        if (Input.GetKeyDown(KeyCode.E) && gameObject.CompareTag("Player"))
        {
            TakeDamage(1);
        }

        // R tusuna basildiginda karakterin canlandirilmasi
        if (currentHealth <= 0 && Input.GetKeyDown(KeyCode.R) && gameObject.CompareTag("Player"))
        {
            dead = false; // Olum durumunu kaldir
            anim.SetTrigger("revive"); // Canlanma animasyonunu tetikle
            currentHealth = startingHealth; // Sagligi baslangic seviyesine getir
            GetComponent<PlayerController>().enabled = true; // PlayerController'i etkinlestir
            GetComponent<PlayerInput>().enabled = true; // PlayerInput'u etkinlestir

            if (rangeAttack != null)
            {
                rangeAttack.active = 1; // Menzilli saldiriyi ac
                Debug.Log("RangeAttack acildi!");
            }
            else
            {
                Debug.LogError("RangeAttack bileseni bulunamadi!");
            }

            // Karakterin canlandirilacak konumunun ayarlanmasi
            if (spawnPoint != null) // SpawnPoint atandiysa oraya isik hizinda git
            {
                transform.position = spawnPoint.position;
            }
            else
            {
                Debug.LogWarning("SpawnPoint atanamadi! Lutfen bir SpawnPoint nesnesi secin.");
            }
        }
    }
}
