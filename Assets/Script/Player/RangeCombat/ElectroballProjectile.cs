using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroballProjectile : MonoBehaviour
{
    #region Ayarlar
    [SerializeField] public int damage = 1;                  // Verilecek hasar
    public float stunDuration = 1f;                          // Sersemletme suresi
    public GameObject explosionPrefab;                       // Patlama efekti prefab'i
    public float lifetime = 2f;                              // Merminin omru (otomatik yok olma)
    #endregion

    #region Dahili
    Enemy enemy; // Su anda kullanilmiyor ama tanimli
    #endregion

    #region Unity Fonksiyonlari
    private void Start()
    {
        // Belirli bir sure sonra kendini yok et
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

            // Dusmanin parent'indan EnemyPatrol script'ini bul
            EnemyPatrol enemyPatrol = collision.GetComponentInParent<EnemyPatrol>();

            if (enemyPatrol != null)
            {
                // Sersemletme uygula
                enemyPatrol.Stun(stunDuration);
            }
            else
            {
                Debug.Log("Enemy does NOT have EnemyPatrol Script!");
            }

            // Mermiyi yok et
            Destroy(gameObject);
        }
    }
    #endregion

    #region Patlama ve Yok Olma
    private void CreateExplosion()
    {
        if (explosionPrefab != null)
        {
            // Patlama efektini mevcut konumda olustur
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    private void SelfDestruction()
    {
        // Patlama efektini olustur
        CreateExplosion();

        // Mermiyi yok et
        Destroy(gameObject);
    }
    #endregion
}
