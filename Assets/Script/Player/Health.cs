using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class Health : MonoBehaviour
{
    [Header("Sa�l�k")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    public bool dead;

    [Header("Yenilmezlik Durumu")]
    public float invincibilityDuration; // Hasar ald�ktan sonra ka� saniye hasar alamaz hale ge�icek.
    private bool isInvincible = false; // Hasar alamaz durumun kontrol�

    private Vector3 originalScale; // Karakterin orijinal �l�e�i

    public Transform spawnPoint;
    public RangeAttack rangeAttack;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();

        originalScale = transform.localScale; // Ba�lang��ta �l�e�i kaydet
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
                        Debug.Log("RangeAttack kapand�!");
                    }
                }

                //D��man
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

                    // **Cesedin fizik motoruyla etkile�imini kapat**
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.bodyType = RigidbodyType2D.Static; // Fizik motorunu devre d��� b�rak
                    }

                    // **2 saniye sonra cesedi yok et (opsiyonel)**
                    Destroy(gameObject, 5f); // 5 saniye sonra ceset kaybolacak
                }
                dead = true;
            }
        }
    }

    //Hasar almaz durumun sayac�
    IEnumerator InvincibilityTimer() 
    {
        isInvincible = true; // Yenilmezli�i a�
        yield return new WaitForSeconds(invincibilityDuration); // Belirtilen s�reyi bekle
        isInvincible = false; // Yenilmezli�i kapat
    }

    private void Update()
    {

        // Test i�in karaktere 1 Hasar verme
        if (Input.GetKeyDown(KeyCode.E) && gameObject.CompareTag("Player")) 
        {
            TakeDamage(1);
        }

        // R tu�una bas�ld���nda karakterin canland�r�lmas�.
        if (currentHealth <= 0 && Input.GetKeyDown(KeyCode.R) && gameObject.CompareTag("Player"))
        {
            dead = false;
            anim.SetTrigger("revive");
            currentHealth = startingHealth;
            GetComponent<PlayerController>().enabled = true;
            GetComponent<PlayerInput>().enabled = true;

            if (rangeAttack != null)
            {
                rangeAttack.active = 1; // A�
                Debug.Log("RangeAttack a��ld�!");
            }
            else
            {
                Debug.LogError("RangeAttack bile�eni bulunamad�!");
            }

            //Karakterin canland�r�lacak konumunun ayarlanmas�.
            if (spawnPoint != null) // SpawnPoint atand�ysa ���nla
            {
                transform.position = spawnPoint.position;
            }
            else
            {
                Debug.LogWarning("SpawnPoint atanmad�! L�tfen bir SpawnPoint nesnesi se�in.");
            }

        }
    }


}
