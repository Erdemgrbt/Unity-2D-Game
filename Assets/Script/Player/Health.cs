using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class Health : MonoBehaviour
{
    [Header("Saðlýk")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    public bool dead;

    [Header("Yenilmezlik Durumu")]
    public float invincibilityDuration; // Hasar aldýktan sonra kaç saniye hasar alamaz hale geçicek.
    private bool isInvincible = false; // Hasar alamaz durumun kontrolü

    private Vector3 originalScale; // Karakterin orijinal ölçeði

    public Transform spawnPoint;
    public RangeAttack rangeAttack;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();

        originalScale = transform.localScale; // Baþlangýçta ölçeði kaydet
    }

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

                //Oynanan Karakter
                if (gameObject.CompareTag("Player"))
                {
                    if (GetComponent<PlayerController>() != null)
                    {
                        GetComponent<PlayerController>().enabled = false;
                    }

                    if (GetComponent<PlayerInput>() != null)
                    {
                        GetComponent<PlayerInput>().enabled = false;
                    }

                    originalScale = transform.localScale;
                    transform.localScale = originalScale;

                    if (rangeAttack != null)
                    {
                        rangeAttack.active = 0; // KAPAT
                        Debug.Log("RangeAttack kapandý!");
                    }
                }

                //Düþman
                if (gameObject.CompareTag("Enemy"))
                {
                    if (GetComponentInParent<EnemyPatrol>() != null)
                    {
                        GetComponentInParent<EnemyPatrol>().enabled = false;
                    }

                    if (GetComponent<MeleeEnemy>() != null)
                    {
                        GetComponent<MeleeEnemy>().enabled = false;
                    }

                    Collider2D enemyCollider = GetComponent<Collider2D>();
                    if (enemyCollider != null)
                    {
                        enemyCollider.enabled = false;
                    }

                    // **Cesedin fizik motoruyla etkileþimini kapat**
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.bodyType = RigidbodyType2D.Static; // Fizik motorunu devre dýþý býrak
                    }

                    // **2 saniye sonra cesedi yok et (opsiyonel)**
                    Destroy(gameObject, 5f); // 5 saniye sonra ceset kaybolacak
                }
                dead = true;
            }
        }
    }

    //Hasar almaz durumun sayacý
    IEnumerator InvincibilityTimer() 
    {
        isInvincible = true; // Yenilmezliði aç
        yield return new WaitForSeconds(invincibilityDuration); // Belirtilen süreyi bekle
        isInvincible = false; // Yenilmezliði kapat
    }

    private void Update()
    {

        // Test için karaktere 1 Hasar verme
        if (Input.GetKeyDown(KeyCode.E) && gameObject.CompareTag("Player")) 
        {
            TakeDamage(1);
        }

        // R tuþuna basýldýðýnda karakterin canlandýrýlmasý.
        if (currentHealth <= 0 && Input.GetKeyDown(KeyCode.R) && gameObject.CompareTag("Player"))
        {
            dead = false;
            anim.SetTrigger("revive");
            currentHealth = startingHealth;
            GetComponent<PlayerController>().enabled = true;
            GetComponent<PlayerInput>().enabled = true;

            if (rangeAttack != null)
            {
                rangeAttack.active = 1; // AÇ
                Debug.Log("RangeAttack açýldý!");
            }
            else
            {
                Debug.LogError("RangeAttack bileþeni bulunamadý!");
            }

            //Karakterin canlandýrýlacak konumunun ayarlanmasý.
            if (spawnPoint != null) // SpawnPoint atandýysa ýþýnla
            {
                transform.position = spawnPoint.position;
            }
            else
            {
                Debug.LogWarning("SpawnPoint atanmadý! Lütfen bir SpawnPoint nesnesi seçin.");
            }

        }
    }


}
