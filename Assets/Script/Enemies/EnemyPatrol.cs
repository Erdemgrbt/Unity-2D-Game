using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    #region Patrol Noktalari
    [Header("Patrol Noktalari")]
    [SerializeField] private Transform leftEdge;   // Sol sinir
    [SerializeField] private Transform rightEdge;  // Sag sinir
    #endregion

    #region Dusman Referansi
    [Header("Dusman")]
    [SerializeField] private Transform enemy; // Hareket edecek dusman objesi
    #endregion

    #region Hareket Ayarlari
    [Header("Hareket Ayarlari")]
    [SerializeField] public float speed;       // Mevcut hiz
    public bool isSlowed = false;              // Yavaslatildi mi?
    public bool isStunned = false;             // Sersemletildi mi?
    private float originalSpeed;               // Orijinal hiz
    private bool movingLeft;                   // Hangi yone gidiyor?
    #endregion

    #region Kenarda Bekleme
    [Header("Kenarda Bekleme")]
    [SerializeField] private float idleDuration;  // Kenarda bekleme suresi
    private float idleTimer;                      // Bekleme sayaci
    #endregion

    #region Animasyon
    [Header("Animasyon")]
    [SerializeField] private Animator anim; // Animator referansi
    #endregion

    #region Unity Fonksiyonlari
    private void Awake()
    {
        originalSpeed = speed; // Oyuna baslarken orijinal hizi kaydet
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false); // Script disable oldugunda animasyonu durdur
    }

    private void Update()
    {
        // Sersemletilmisse hicbir sey yapma
        if (isStunned)
        {
            anim.SetBool("moving", false);
            return;
        }

        // Patrol hareketi
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }
    #endregion

    #region Hareket Mantigi
    private void DirectionChange()
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);

        // Sprite'i yone gore dondur
        enemy.localScale = new Vector3(Mathf.Abs(enemy.localScale.x) * _direction,
            enemy.localScale.y,
            enemy.localScale.z);

        // Dusmani hareket ettir
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y,
            enemy.position.z);
    }
    #endregion

    #region Yavaslatma ve Sersemletme

    // Dusmani yavaslat
    public void SlowDown(float slowAmount, float slowDuration)
    {
        if (isSlowed || isStunned)
            return;

        isSlowed = true;
        speed -= slowAmount;

        StartCoroutine(RestoreSpeed(slowDuration));
    }

    // Dusmani sersemlet
    public void Stun(float stunDuration)
    {
        if (isStunned)
            return;

        isStunned = true;
        speed = 0;
        anim.SetBool("moving", false);

        StartCoroutine(RemoveStun(stunDuration));
    }

    private IEnumerator RestoreSpeed(float slowDuration)
    {
        yield return new WaitForSeconds(slowDuration);
        speed = originalSpeed;
        isSlowed = false;
    }

    private IEnumerator RemoveStun(float duration)
    {
        Debug.Log("Stunned");

        yield return new WaitForSeconds(duration);

        isStunned = false;
        anim.SetBool("moving", true);
        speed = originalSpeed;
    }
    #endregion
}
