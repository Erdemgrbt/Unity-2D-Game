using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    #region Ayarlar
    public Transform player;                      // Oyuncu referansi
    public float orbitRadius = 1.5f;              // Oyuncunun etrafindaki donus yaricapi
    public GameObject firePrefab;                 // Atis prefab - ates
    public GameObject icePrefab;                  // Atis prefab - buz
    public GameObject electricPrefab;             // Atis prefab - elektrik
    public float fireSpeed = 10f;                 // Atisin hizi
    public Transform fireSpawnPoint;              // Merminin cikacagi nokta

    [Header("Engeller Icin LayerMask")]
    public LayerMask obstacleLayer;               // Mermi engel kontrolu icin

    [Header("Ates Etme Cooldown Suresi")]
    public float fireCooldown = 0.5f;             // Mermi atma bekleme suresi
    private float lastFireTime;                   // En son atis zamani
    #endregion

    #region Dahili Degiskenler
    private Vector3 mousePosition;
    private Vector3 direction;
    private float angle;

    public int active = 1;                        // Aktif mi kontrolu (1 = aktif)

    private SpriteRenderer spriteRenderer;

    private enum ElementType { Fire, Ice, Electric }
    private ElementType currentElement = ElementType.Fire;
    #endregion

    #region Unity Fonksiyonlari
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateElementAppearance(); // Baslangicta gorsel guncelle
    }

    void Update()
    {
        if (active == 1)
        {
            FollowMouse();         // Fareyi takip et
            RotateAroundPlayer();  // Oyuncunun etrafinda don
            ShootOnClick();        // Tiklandiginda ates et

            if (Input.GetKeyDown(KeyCode.Mouse1)) // Sag tik ile element degistir
            {
                ChangeElement();
            }
        }
    }
    #endregion

    #region Hareket Fonksiyonlari
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
    #endregion

    #region Ates Etme
    private void ShootOnClick()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastFireTime + fireCooldown)
        {
            // Mermi noktasi engel icerisinde mi kontrol et
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

                lastFireTime = Time.time;
            }
            else
            {
                Debug.Log($"Ates noktasi engel icerisinde: {hit.gameObject.name}, ates edilemez!");
            }
        }
    }
    #endregion

    #region Element Degisimi ve Gorsel
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
    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(fireSpawnPoint.position, 0.1f);
    }
    #endregion
}
