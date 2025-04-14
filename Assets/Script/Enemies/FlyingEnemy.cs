using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed;
    private float originalSpeed;
    public bool chase = false;
    public Transform startingPoint;
    private GameObject player;
    private Animator anim;
    public bool isSlowed = false;
    public bool isStunned = false;

    private void Awake()
    {
        originalSpeed = speed;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null)
            return;

        if (chase)
        {
            Chase();
        }
        else
        {
            ReturnStartPoint();
        }

        HandleAnimation(); // Animasyon durumu burada kontrol ediliyor
        Flip();
    }

    private void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void ReturnStartPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, speed * Time.deltaTime);
    }

    private void Flip()
    {
        if (chase)
        {
            if (transform.position.x > player.transform.position.x)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            if (transform.position.x > startingPoint.position.x)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void HandleAnimation()
    {
        bool isMoving = chase || Vector2.Distance(transform.position, startingPoint.position) > 0.05f;

        anim.SetBool("Fly", isMoving); // Fly açýk mý?
        // Idle'ý ayrýca setlemene gerek yok, çünkü Fly false olduðunda Idle otomatik oynar.
    }
    public void SlowDown(float slowAmount, float slowDuration)
    {
        if (isSlowed || isStunned)
            return;

        isSlowed = true;
        speed -= slowAmount;

        StartCoroutine(RestoreSpeed(slowDuration));
    }

    private IEnumerator RestoreSpeed(float slowDuration)
    {
        yield return new WaitForSeconds(slowDuration);
        speed = originalSpeed;
        isSlowed = false;
    }

    public void Stun(float stunDuration)
    {
        if (isStunned)
            return;

        isStunned = true;
        speed = 0;
        anim.SetBool("Fly", false);

        StartCoroutine(RemoveStun(stunDuration));
    }

    private IEnumerator RemoveStun(float duration)
    {
        yield return new WaitForSeconds(duration);

        isStunned = false;
        anim.SetBool("Fly", true);
        speed = originalSpeed;
    }
}