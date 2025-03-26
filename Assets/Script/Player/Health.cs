using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class Health : MonoBehaviour
{
<<<<<<< Updated upstream
    [Header("Saglik")]
    [SerializeField] private float startingHealth; // Baslangic sagligi
    public float currentHealth { get; private set; } // Mevcut saglik (disaridan sadece okunabilir)
    private Animator anim; // Animator referansi
    public bool dead; // Karakterin olum durumu
=======
    #region Saðlýk Deðiþkenleri
    [Header("Saðlýk Ayarlarý")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    public bool IsDead { get; private set; }
    #endregion
>>>>>>> Stashed changes

    [Header("Yenilmezlik Durumu")]
<<<<<<< Updated upstream
    public float invincibilityDuration; // Hasar alindiktan sonra kac saniye yenilmez olunacak
    private bool isInvincible = false; // Yenilmezlik durumu

    private Vector3 originalScale; // Karakterin orijinal olcegi

    public Transform spawnPoint; // Yeniden dogma noktasi
    public RangeAttack rangeAttack; // Menzilli saldiri kontrolu

=======
    [SerializeField] private float invincibilityDuration;
    private bool isInvincible;
    #endregion

    #region Diðer Bileþenler
    private Animator anim;
    private Vector3 originalScale;
    public Transform spawnPoint;
    public RangeAttack rangeAttack;
    #endregion

    #region Unity Fonksiyonlarý
>>>>>>> Stashed changes
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

                    Destroy(gameObject, 5f); // 5 saniye sonra nesneyi yok et
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
<<<<<<< Updated upstream
        // Test amacli, E tusuna basinca karaktere 1 hasar ver
        if (Input.GetKeyDown(KeyCode.E) && gameObject.CompareTag("Player"))
=======
        if (Input.GetKeyDown(KeyCode.E) && CompareTag("Player"))
>>>>>>> Stashed changes
        {
            TakeDamage(1);
        }

<<<<<<< Updated upstream
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
=======
        if (currentHealth <= 0 && Input.GetKeyDown(KeyCode.R) && CompareTag("Player"))
        {
            Revive();
        }
    }
    #endregion

    #region Hasar Ýþlemleri
    public void TakeDamage(float damage)
    {
        if (isInvincible || IsDead) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(InvincibilityTimer());
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        if (IsDead) return;

        anim.SetTrigger("die");
        IsDead = true;

        if (CompareTag("Player"))
        {
            HandlePlayerDeath();
        }
        else if (CompareTag("Enemy"))
        {
            HandleEnemyDeath();
        }
    }

    private void HandlePlayerDeath()
    {
        GetComponent<PlayerController>()?.Disable();
        GetComponent<PlayerInput>()?.Disable();
        transform.localScale = originalScale;
        rangeAttack?.Disable();
    }

    private void HandleEnemyDeath()
    {
        GetComponentInParent<EnemyPatrol>()?.Disable();
        GetComponent<MeleeEnemy>()?.Disable();
        GetComponent<Collider2D>()?.Disable();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Destroy(gameObject, 2f);
    }

    private IEnumerator InvincibilityTimer()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
    #endregion

    #region Canlandýrma Ýþlemi
    private void Revive()
    {
        IsDead = false;
        anim.SetTrigger("revive");
        currentHealth = startingHealth;

        GetComponent<PlayerController>()?.Enable();
        GetComponent<PlayerInput>()?.Enable();
        rangeAttack?.Enable();

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
    }
    #endregion
}

public static class ComponentExtensions
{
    public static void Enable(this Behaviour component)
    {
        if (component != null) component.enabled = true;
    }

    public static void Disable(this Behaviour component)
    {
        if (component != null) component.enabled = false;
    }
>>>>>>> Stashed changes
}
