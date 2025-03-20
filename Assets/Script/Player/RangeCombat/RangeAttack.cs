using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    public Transform player;
    public float orbitRadius = 1.5f;
    public GameObject firePrefab;
    public float fireSpeed = 10f;
    public Transform fireSpawnPoint;

    [Header("Engeller Icin LayerMask")]
    public LayerMask obstacleLayer;

    private Vector3 mousePosition;
    private Vector3 direction;
    private float angle;

    public int active = 1;

    [Header("Ates Etme Cooldown Suresi")]
    public float fireCooldown = 0.5f; // 0.5 saniye cooldown
    private float lastFireTime; // Son ates zamani

    void Update()
    {
        if (active == 1)
        {
            FollowMouse();
            RotateAroundPlayer();
            ShootOnClick();
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
        if (Input.GetMouseButtonDown(0) && Time.time >= lastFireTime + fireCooldown) // Sol tik ve cooldown kontrol
        {
            float detectionRadius = 0.1f;
            Collider2D hit = Physics2D.OverlapCircle(fireSpawnPoint.position, detectionRadius, obstacleLayer);

            if (hit == null)
            {
                GameObject fire = Instantiate(firePrefab, fireSpawnPoint.position, Quaternion.identity);
                Rigidbody2D rb = fire.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    rb.velocity = direction.normalized * fireSpeed;
                    float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    fire.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
                }

                lastFireTime = Time.time; // Son ates zamanini guncelle
            }
            else
            {
                Debug.Log($"Ates noktasi iceride: {hit.gameObject.name}, ates edilemiyor!");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(fireSpawnPoint.position, 0.1f);
    }
}