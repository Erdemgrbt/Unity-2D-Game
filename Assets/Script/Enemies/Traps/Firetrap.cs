using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    #region Ayarlar
    [SerializeField] private float damage;               // Verilecek hasar

    [Header("Firetrap Timer")]
    [SerializeField] private float activationDelay;      // Tuzagin tetiklenme gecikmesi
    [SerializeField] private float activeTime;           // Tuzagin aktif kalma suresi

    [Header("Ses")]
    [SerializeField] private AudioClip firetrapSound;
    #endregion

    #region Dahili
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private bool triggered = false;  // Tuzak tetiklendi mi
    private bool active = false;     // Tuzak aktif mi (hasar verebilir mi)
    #endregion

    #region Unity Fonksiyonlari
    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!triggered)
                StartCoroutine(ActivateFiretrap());

            if (active)
                collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
    #endregion

    #region Tuzak Mekanigi
    private IEnumerator ActivateFiretrap()
    {
        triggered = true;

        // Renk degistirerek uyar
        spriteRenderer.color = Color.red;

        // Beklemeden sonra tuzagi aktif et
        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(firetrapSound);

        spriteRenderer.color = Color.white;
        active = true;
        anim.SetBool("activated", true);

        // Belirli bir sure aktif kal
        yield return new WaitForSeconds(activeTime);

        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
    #endregion
}
