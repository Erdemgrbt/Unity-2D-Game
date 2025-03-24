using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    #region Ayarlar
    [SerializeField] public int damage = 1;                // Verilecek hasar
    public GameObject explosionPrefab;                     // Patlama efekti prefab'i
    public float lifetime = 2f;                            // Merminin omru (otomatik yok olma suresi)
    #endregion

    #region Dahili
    Enemy enemy; // Opsiyonel, kullanilmiyor su an
    #endregion

    #region Unity Fonksiyonlari
    private void Start()
    {
        // Belirtilen sure sonunda kendini yok et
        Invoke(nameof(SelfDestruction), lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Eger yere carparsa yok ol
        if (collision.CompareTag("Ground"))
        {
            SelfDestruction();
        }

        // Eger dusmana carparsa hasar ver ve yok ol
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Health>().TakeDamage(damage);
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
