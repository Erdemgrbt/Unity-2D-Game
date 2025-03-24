using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceballProjectile : MonoBehaviour
{
    [SerializeField] public int damage = 1;
    public float slowAmount = 2f; // H�z� ne kadar azaltacak
    public float slowDuration = 1f; // Yava�latma s�resi (saniye)
    public GameObject freezeEffectPrefab; // Buz patlama efekti prefab'�
    public float lifetime = 2f; // Merminin �mr�

    private void Start()
    {
        // Merminin belirli bir s�re sonra yok olmas� i�in
        Invoke(nameof(SelfDestruction), lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
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
                enemyPatrol.SlowDown(slowAmount, slowDuration); // Yava�latmay� ba�lat�yoruz
            }
            else
            {
                Debug.Log("Enemy does NOT have EnemyPatrol Script!");
            }

            Destroy(gameObject);
        }
    }

    private void SelfDestruction()
    {
        if (freezeEffectPrefab != null)
        {
            Instantiate(freezeEffectPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}