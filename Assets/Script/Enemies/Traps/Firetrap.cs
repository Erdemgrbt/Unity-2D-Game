using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("FireTrap Timer")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private bool triggered; // Tuzagin Tetiklenmesi
    private bool active; // Tuzagin aktif olup oyuncuya hasar vermesi

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!triggered)
                StartCoroutine(ActivateFiretrap());

            if (active)
                collision.GetComponent<Health>().TakeDamage(damage);
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        //Tuzagi kirmizi rengine cevirerek oyuncuya tetiklendigini bildiriyor
        triggered = true;
        spriteRenderer.color = Color.red;

        //Tuzagin tetiklenmesi icin biraz gecikme ve ardindan eski rengine donuyor
        yield return new WaitForSeconds(activationDelay);
        spriteRenderer.color = Color.white;
        active = true;
        anim.SetBool("activated", true);

        // X sure sonra eski haline donuyor ve resetleniyor
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}
