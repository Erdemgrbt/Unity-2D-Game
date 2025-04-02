using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    #region Saldiri Parametreleri
    [Header("Saldiri Parametreleri")]
    [SerializeField] private float attackCooldown;     // Saldiri arasindaki bekleme suresi
    [SerializeField] private float range;              // Saldiri menzili
    [SerializeField] private int damage;               // Verilecek hasar
    private float cooldownTimer = Mathf.Infinity;      // Gecen zaman

    [Header("Menzilli Saldiri Parametreleri")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;
    #endregion

    #region Collider Ayarlari
    [Header("Collider Ayarlari")]
    [SerializeField] private float colliderDistance;   // BoxCast uzakligi
    [SerializeField] private BoxCollider2D boxCollider;
    #endregion

    #region Karakter Ayarlari
    [Header("Karakter Ayarlari")]
    [SerializeField] private LayerMask playerLayer;    // Oyuncu layer'i
    private Animator anim;
    #endregion

    #region Referanslar
    private EnemyPatrol enemyPatrol;
    #endregion

    #region Unity Fonksiyonlari
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        // Oyuncu goruluyorsa ve bekleme suresi gectiyse saldir
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("rangeAttack");
            }
        }

        // Oyuncu goruluyorsa patrol durdur
        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }
    #endregion

    #region Oyuncu Tespiti
    private bool PlayerInSight()
    {
        // Sersemletilmisse oyuncuyu gorme
        if (enemyPatrol != null && enemyPatrol.isStunned)
        {
            return false;
        }

        // BoxCast ile oyuncu kontrolu
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }
    #endregion

    private void RangedAttack()
    {
        cooldownTimer = 0;
        fireballs[FindFireball()].transform.position = firepoint.position;
        fireballs[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    #region Debug
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    #endregion
}
