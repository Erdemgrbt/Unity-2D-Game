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
            // Dash hakkýný yenile
            player.ResetDash();

            // Boncuðu geçici olarak kapat
            StartCoroutine(DisableTemporarily());
        }
    }

    private System.Collections.IEnumerator DisableTemporarily()
    {
        // Görseli ve collider'ý kapat
        spriteRenderer.enabled = false;
        col.enabled = false;

        // Belirli bir süre bekle
        yield return new WaitForSeconds(respawnTime);

        // Yeniden görünür ve kullanýlabilir hale getir
        spriteRenderer.enabled = true;
        col.enabled = true;
    }
}