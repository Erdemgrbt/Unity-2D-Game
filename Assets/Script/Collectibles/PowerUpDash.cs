using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDash : MonoBehaviour
{
    [SerializeField] private float respawnTime = 10f;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    [SerializeField] private AudioClip collectSound;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (collision.CompareTag("Player"))
            SoundManager.instance.PlaySound(collectSound);

        if (player != null)
        {
            // Dash hakk�n� yenile
            player.ResetDash();

            // Boncu�u ge�ici olarak kapat
            StartCoroutine(DisableTemporarily());
        }
    }

    private System.Collections.IEnumerator DisableTemporarily()
    {
        // G�rseli ve collider'� kapat
        spriteRenderer.enabled = false;
        col.enabled = false;

        // Belirli bir s�re bekle
        yield return new WaitForSeconds(respawnTime);

        // Yeniden g�r�n�r ve kullan�labilir hale getir
        spriteRenderer.enabled = true;
        col.enabled = true;
    }
}