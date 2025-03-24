using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Deðiþkenleri")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Düþman")]
    [SerializeField] private Transform enemy;

    [Header("Hareket Deðiþkenleri")]
    [SerializeField] public float speed;
    public bool isSlowed = false; // Yavaþlatýldý mý?
    private float originalSpeed; // Orijinal hýz
    private bool movingLeft;

    [Header("Kenarlarda Bekleme")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Düþman Animasyonu")]
    [SerializeField] private Animator anim;

    private void Awake()
    {
        originalSpeed = speed; // Orijinal hýzý kaydediyoruz
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

    // Yavaþlatma coroutine'i
    public void SlowDown(float slowAmount, float slowDuration)
    {
        // Eðer zaten yavaþlatýldýysa, hýz geri yüklenmeyecek
        if (isSlowed)
            return;

        isSlowed = true; // Yavaþlatma baþladý
        speed -= slowAmount; // Hýzý yavaþlat

        // Yavaþlatma süresi sonrasý hýzý geri al
        StartCoroutine(RestoreSpeed(slowDuration));
    }

    private IEnumerator RestoreSpeed(float slowDuration)
    {
        yield return new WaitForSeconds(slowDuration); // Yavaþlatma süresi kadar bekle
        speed = originalSpeed; // Hýzý eski haline getir
        isSlowed = false; // Yavaþlatma bitmiþ olarak iþaretle
    }
}