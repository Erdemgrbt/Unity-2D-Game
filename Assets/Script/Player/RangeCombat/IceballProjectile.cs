using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceballProjectile : MonoBehaviour
{
    #region Ayarlar
    [SerializeField] public int damage = 1;                    // Verilecek hasar
    public float slowAmount = 2f;                              // Ne kadar yavaslatacak
    public float slowDuration = 1f;                            // Yavaslatma suresi
    public GameObject freezeEffectPrefab;                      // Buz patlama efekti prefab'i
    public float lifetime = 2f;                                // Merminin omru (otomatik yok olma)
    #endregion

    #region Unity Fonksiyonlari
    private void Start()
    {
        // Belirli bir sure sonra otomatik yok ol
        Invoke(nameof(SelfDestruction), lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Eger yere carparsa yok ol
        if (collision.CompareTag("Ground"))
        {
            SelfDestruction();
        }

        // Eger dusmana carparsa
        if (collision.CompareTag("Enemy"))
        {
            // Hasar ver
            collision.GetComponent<Health>().TakeDamage(damage);

            // Dusmanin parent'indan EnemyPatrol script'ini al
            EnemyPatrol enemyPatrol = collision.GetComponentInParent<EnemyPatrol>();
            FlyingEnemy flyingEnemy = collision.GetComponentInParent<FlyingEnemy>();

            // Eger varsa, yavaslat
            if (enemyPatrol != null)
            {
                enemyPatrol.SlowDown(slowAmount, slowDuration);
            }

            if (flyingEnemy != null)
            {
                flyingEnemy.SlowDown(slowAmount, slowDuration);
            }

            // Mermiyi yok et
            Destroy(gameObject);
        }
    }
    #endregion

    #region Yok Olma ve Efekt
    private void SelfDestruction()
    {
        // Buz efekti varsa oynat
        if (freezeEffectPrefab != null)
        {
            Instantiate(freezeEffectPrefab, transform.position, Quaternion.identity);
        }

        // Mermiyi yok et
        Destroy(gameObject);
    }
    #endregion
}
