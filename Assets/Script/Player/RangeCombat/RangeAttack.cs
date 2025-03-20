using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    public Transform player;
    public float orbitRadius = 1.5f;
    public GameObject firePrefab;
    public GameObject icePrefab;
    public GameObject electricPrefab;
    public float fireSpeed = 10f;
    public Transform fireSpawnPoint;

    [Header("Engeller Ýçin LayerMask")]
    public LayerMask obstacleLayer;

    private Vector3 mousePosition;
    private Vector3 direction;
    private float angle;

    public int active = 1;

    [Header("Ateþ Etme Cooldown Süresi")]
    public float fireCooldown = 0.5f; // 0.5 saniye cooldown
    private float lastFireTime; // Son ateþ zamaný

    private SpriteRenderer spriteRenderer;

    private enum ElementType { Fire, Ice, Electric }
    private ElementType currentElement = ElementType.Fire;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateElementAppearance();
    }

    void Update()
    {
        if (active == 1)
        {
            FollowMouse();
            RotateAroundPlayer();
            ShootOnClick();

            if (Input.GetKeyDown(KeyCode.R))
            {
                ChangeElement();
            }
        }
    }

    private void FollowMouse()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        direction = mousePosition - player.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void RotateAroundPlayer()
    {
        Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * orbitRadius;
        transform.position = player.position + offset;
        float lookAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, lookAngle - 90f);
    }

    private void ShootOnClick()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastFireTime + fireCooldown) // Sol týk ve cooldown kontrolü
        {
            float detectionRadius = 0.1f;
            Collider2D hit = Physics2D.OverlapCircle(fireSpawnPoint.position, detectionRadius, obstacleLayer);

            if (hit == null)
            {
                GameObject projectilePrefab = GetCurrentProjectilePrefab();
                GameObject projectile = Instantiate(projectilePrefab, fireSpawnPoint.position, Quaternion.identity);
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    rb.velocity = direction.normalized * fireSpeed;
                    float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    projectile.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
                }

                lastFireTime = Time.time; // Son ateþ zamanýný güncelle
            }
            else
            {
                Debug.Log($"Ateþ noktasý içeride: {hit.gameObject.name}, ateþ edilemiyor!");
            }
        }
    }

    private void ChangeElement()
    {
        currentElement = (ElementType)(((int)currentElement + 1) % 3);
        UpdateElementAppearance();
    }

    private void UpdateElementAppearance()
    {
        if (spriteRenderer != null)
        {
            switch (currentElement)
            {
                case ElementType.Fire:
                    spriteRenderer.color = Color.red;
                    break;
                case ElementType.Ice:
                    spriteRenderer.color = Color.cyan;
                    break;
                case ElementType.Electric:
                    spriteRenderer.color = Color.yellow;
                    break;
            }
        }
    }

    private GameObject GetCurrentProjectilePrefab()
    {
        switch (currentElement)
        {
            case ElementType.Ice:
                return icePrefab;
            case ElementType.Electric:
                return electricPrefab;
            default:
                return firePrefab;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(fireSpawnPoint.position, 0.1f);
    }
}