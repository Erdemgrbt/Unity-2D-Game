using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    #region Enum - AI Durumlarý
    private enum State
    {
        Patrolling,
        ChasingPlayer,
        ReturningToStart
    }

    private State currentState = State.Patrolling;
    #endregion

    #region Referanslar
    [Header("Patrol Noktalari")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Dusman")]
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform startPoint; // Baþlangýç noktasý

    [Header("Hareket Ayarlari")]
    [SerializeField] public float speed;
    public bool isSlowed = false;
    public bool isStunned = false;
    private float originalSpeed;
    private bool movingLeft;

    [Header("Kenarda Bekleme")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Animasyon")]
    [SerializeField] private Animator anim;

    [Header("Oyuncu Tespiti")]
    [SerializeField] private BoxCollider2D detectionRange;
    private Transform playerTarget;
    #endregion

    #region Unity Fonksiyonlari
    private void Awake()
    {
        originalSpeed = speed;
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }

    private void Update()
    {
        if (isStunned)
        {
            anim.SetBool("moving", false);
            return;
        }

        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                break;

            case State.ChasingPlayer:
                ChasePlayer();
                break;

            case State.ReturningToStart:
                ReturnToStart();
                break;
        }
    }
    #endregion

    #region AI Durum Fonksiyonlari
    private void Patrol()
    {
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

    private void ChasePlayer()
    {
        if (playerTarget == null) return;

        float direction = Mathf.Sign(playerTarget.position.x - enemy.position.x);
        MoveInDirection((int)direction);
    }

    private void ReturnToStart()
    {
        float direction = Mathf.Sign(startPoint.position.x - enemy.position.x);

        if (Mathf.Abs(enemy.position.x - startPoint.position.x) < 0.1f)
        {
            currentState = State.Patrolling;
            return;
        }

        MoveInDirection((int)direction);
    }
    #endregion

    #region Hareket & Yön
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

        enemy.localScale = new Vector3(Mathf.Abs(enemy.localScale.x) * _direction,
            enemy.localScale.y,
            enemy.localScale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y,
            enemy.position.z);
    }
    #endregion

    #region Trigger Etkileþimleri
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentState = State.ChasingPlayer;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentState = State.ReturningToStart;
        }
    }
    #endregion

    #region Yavaþlatma & Sersemletme
    public void SlowDown(float slowAmount, float slowDuration)
    {
        if (isSlowed || isStunned)
            return;

        isSlowed = true;
        speed -= slowAmount;

        StartCoroutine(RestoreSpeed(slowDuration));
    }

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
        yield return new WaitForSeconds(duration);

        isStunned = false;
        anim.SetBool("moving", true);
        speed = originalSpeed;
    }
    #endregion
}