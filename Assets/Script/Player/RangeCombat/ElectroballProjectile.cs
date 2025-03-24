using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroballProjectile : MonoBehaviour
{
    [SerializeField] public int damage = 1;
    public GameObject explosionPrefab; // Patlama efekti prefab'ý
    public float lifetime = 2f;        // Merminin ömrü (otomatik yok olma süresi)
    Enemy enemy;

    private void Start()
    {
        Invoke(nameof(SelfDestruction), lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Çarpýþma anýnda yapýlacak iþlemler
        if (collision.CompareTag("Ground")) // Engeller veya zemin (collision.CompareTag("Obstacle") || collision.CompareTag("Ground"))
        {
            SelfDestruction();
        }

        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }

    }

    private void CreateExplosion()
    {
        if (explosionPrefab != null)
        {
            // Patlama efektini ateþ mermisinin pozisyonunda oluþtur
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
    private void SelfDestruction()
    {
        // Patlama efektini oluþtur
        CreateExplosion();

        // Ateþ mermisini yok et
        Destroy(gameObject);
    }
}