using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol De�i�kenleri")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("D��man")]
    [SerializeField] private Transform enemy;

    [Header("Hareket De�i�kenleri")]
    [SerializeField] public float speed;
    public bool isSlowed = false; // Yava�lat�ld� m�?
    private float originalSpeed; // Orijinal h�z
    private bool movingLeft;

    [Header("Kenarlarda Bekleme")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("D��man Animasyonu")]
    [SerializeField] private Animator anim;

    private void Awake()
    {
        originalSpeed = speed; // Orijinal h�z� kaydediyoruz
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }

    private void Update()
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

    // Yava�latma coroutine'i
    public void SlowDown(float slowAmount, float slowDuration)
    {
        // E�er zaten yava�lat�ld�ysa, h�z geri y�klenmeyecek
        if (isSlowed)
            return;

        isSlowed = true; // Yava�latma ba�lad�
        speed -= slowAmount; // H�z� yava�lat

        // Yava�latma s�resi sonras� h�z� geri al
        StartCoroutine(RestoreSpeed(slowDuration));
    }

    private IEnumerator RestoreSpeed(float slowDuration)
    {
        yield return new WaitForSeconds(slowDuration); // Yava�latma s�resi kadar bekle
        speed = originalSpeed; // H�z� eski haline getir
        isSlowed = false; // Yava�latma bitmi� olarak i�aretle
    }
}