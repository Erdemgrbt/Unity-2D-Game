using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroballProjectile : MonoBehaviour
{
    [SerializeField] public int damage = 1;
    public GameObject explosionPrefab; // Patlama efekti prefab'�
    public float lifetime = 2f;        // Merminin �mr� (otomatik yok olma s�resi)
    Enemy enemy;

    private void Start()
    {
        Invoke(nameof(SelfDestruction), lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �arp��ma an�nda yap�lacak i�lemler
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
            // Patlama efektini ate� mermisinin pozisyonunda olu�tur
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
    private void SelfDestruction()
    {
        // Patlama efektini olu�tur
        CreateExplosion();

        // Ate� mermisini yok et
        Destroy(gameObject);
    }
}