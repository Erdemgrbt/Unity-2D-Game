using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    #region Ayarlar
    [SerializeField] private float speed;         // Okun hizi
    [SerializeField] private float resetTime;     // Ne kadar sure sonra yok olacak
    private float lifetime;                       // Gecen sure
    #endregion

    #region Aktivasyon
    public void ActivateProjectile()
    {
        lifetime = 0;
        gameObject.SetActive(true); // Obje havuzdan aktif edilir
    }
    #endregion

    #region Unity Fonksiyonlari
    private void Update()
    {
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0); // Ileri dogru hareket

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false); // Sure dolduysa havuza geri don
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);     // EnemyDamage icindeki hasar fonksiyonu
        gameObject.SetActive(false);          // Temas edince havuza geri don
    }
    #endregion
}
