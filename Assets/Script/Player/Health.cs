using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour
{
    #region Saglik
    [Header("Saglik")]
    [SerializeField] private float startingHealth;       // Baslangic sagligi
    public float currentHealth { get; private set; }     // Mevcut saglik (sadece okunabilir)
    private Animator anim;
    public bool dead;                                    // Oldu mu?
    #endregion

    #region Yenilmezlik
    [Header("Yenilmezlik Durumu")]
    public float invincibilityDuration;                  // Hasar alinamayacak sure
    private bool isInvincible = false;                   // Yenilmezlik aktif mi?
    #endregion

    #region Diger Bilesenler
    private Vector3 originalScale;                       // Orijinal boyut
    public Transform spawnPoint;                         // Yeniden dogma noktasi
    public RangeAttack rangeAttack;                      // RangeAttack kontrolu
    #endregion

    #region Unity Fonksiyonlari
    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    private void Update()
    {
        // Test: E tusu ile hasar ver
        if (Input.GetKeyDown(KeyCode.E) && gameObject.CompareTag("Player"))
        {
            TakeDamage(1);
        }

        // R tusu ile canlandirma
        if (currentHealth <= 0 && Input.GetKeyDown(KeyCode.R) && gameObject.CompareTag("Player"))
        {
            dead = false;
            anim.SetTrigger("revive");
            currentHealth = startingHealth;

            GetComponent<PlayerController>().enabled = true;
            GetComponent<PlayerInput>().enabled = true;

            if (rangeAttack != null)
            {
                rangeAttack.active = 1;
                Debug.Log("RangeAttack acildi!");
            }
            else
            {
                Debug.LogError("RangeAttack bileseni bulunamadi!");
            }

            // Yeniden dogma noktasi varsa oraya git
            if (spawnPoint != null)
            {
                transform.position = spawnPoint.position;
            }
            else
            {
                Debug.LogWarning("SpawnPoint atanamadi!");
            }
        }
    }
    #endregion

    #region Hasar Islemleri
    public void TakeDamage(float _damage)
    {
        if (isInvincible) return;

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(InvincibilityTimer());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");

                // Oyuncu olduysa
                if (CompareTag("Player"))
                {
                    var pc = GetComponent<PlayerController>();
                    if (pc != null) pc.enabled = false;

                    var pi = GetComponent<PlayerInput>();
                    if (pi != null) pi.enabled = false;

                    transform.localScale = originalScale;

                    if (rangeAttack != null)
                    {
                        rangeAttack.active = 0;
                        Debug.Log("RangeAttack kapandi!");
                    }
                }

                // Dusman olduysa
                if (CompareTag("Enemy"))
                {
                    var patrol = GetComponentInParent<EnemyPatrol>();
                    if (patrol != null) patrol.enabled = false;

                    var melee = GetComponent<MeleeEnemy>();
                    if (melee != null) melee.enabled = false;

                    var col = GetComponent<Collider2D>();
                    if (col != null) col.enabled = false;

                    var rb = GetComponent<Rigidbody2D>();
                    if (rb != null) rb.bodyType = RigidbodyType2D.Static;

                    Destroy(gameObject, 5f); // 5 saniye sonra yok et
                }

                dead = true;
            }
        }
    }

    private IEnumerator InvincibilityTimer()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
    #endregion
}
