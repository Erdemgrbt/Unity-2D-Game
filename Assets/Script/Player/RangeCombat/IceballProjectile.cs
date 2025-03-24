using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceballProjectile : MonoBehaviour
{
    [SerializeField] public int damage = 1;
    public float slowAmount = 2f; // Hýzý ne kadar azaltacak
    public float slowDuration = 1f; // Yavaþlatma süresi (saniye)
    public GameObject freezeEffectPrefab; // Buz patlama efekti prefab'ý
    public float lifetime = 2f; // Merminin ömrü

    private void Start()
    {
        // Merminin belirli bir süre sonra yok olmasý için
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

            // EnemyPatrol scriptini düþmanýn parent'ýnda ara
            EnemyPatrol enemyPatrol = collision.GetComponentInParent<EnemyPatrol>();

            if (enemyPatrol != null)
            {
                enemyPatrol.SlowDown(slowAmount, slowDuration); // Yavaþlatmayý baþlatýyoruz
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