using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Saldýrý Parametreleri")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Collider Parametreleri")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Karakter Parametreleri")]
    [SerializeField] private LayerMask playerLayer;
    private Animator anim;

    //Referanslar
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSýght())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }

        if(enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSýght();
    }

    private bool PlayerInSýght()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right *range *transform.localScale.x *colliderDistance, 
            new Vector3(boxCollider.bounds.size.x *range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 
            0, Vector2.left, 0, playerLayer);

        if(hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right *range *transform.localScale.x *colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    
    private void DamagePlayer()
    {
        if (PlayerInSýght())
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
