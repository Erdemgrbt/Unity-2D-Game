using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroballProjectile : MonoBehaviour
{
    [SerializeField] public int damage = 1;
    public float stunDuration = 1f;
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

        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Health>().TakeDamage(damage);

            // EnemyPatrol scriptini d��man�n parent'�nda ara
            EnemyPatrol enemyPatrol = collision.GetComponentInParent<EnemyPatrol>();

            if (enemyPatrol != null)
            {
                enemyPatrol.Stun(stunDuration); // Yava�latmay� ba�lat�yoruz
            }
            else
            {
                Debug.Log("Enemy does NOT have EnemyPatrol Script!");
            }

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