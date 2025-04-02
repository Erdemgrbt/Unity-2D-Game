using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    #region Ayarlar
    [SerializeField] private float speed;         // Okun hizi
    [SerializeField] private float resetTime;     // Ne kadar sure sonra yok olacak
    private float lifetime;                       // Gecen sure
    private Animator anim;
    private BoxCollider2D coll;

    private bool hit;
    #endregion

    #region Aktivasyon
    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true); // Obje havuzdan aktif edilir
        coll.enabled = true;
    }
    #endregion

    #region Unity Fonksiyonlari

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0); // Ileri dogru hareket

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false); // Sure dolduysa havuza geri don
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision);     // EnemyDamage icindeki hasar fonksiyonu
        coll.enabled = false;

        if (anim != null)
        {
            anim.SetTrigger("explode");
        }
        else
        {
            gameObject.SetActive(false);          // Temas edince havuza geri don
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
