using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeed : MonoBehaviour
{
    [SerializeField] private float speedBoostTime = 1f;
    [SerializeField] private float speedBoost = 2f;
    [SerializeField] private float airspeedBoost = 2f;
    [SerializeField] private AudioClip collectSound;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();
        SoundManager.instance.PlaySound(collectSound);

        if (playerController != null)
        {
            StartCoroutine(SpeedBoost(playerController));
            // gameObject.SetActive(false); // Gizle
            GetComponent<Collider2D>().enabled = false; // Çakýþmayý engelle
            GetComponent<SpriteRenderer>().enabled = false; // Görünmesin
        }
    }

    private IEnumerator SpeedBoost(PlayerController player)
    {
        float originalSpeed = player.walkSpeed;
        float originalAirSpeed = player.airWalkSpeed;
        player.walkSpeed += speedBoost;
        player.airWalkSpeed += airspeedBoost;

        Debug.Log("Speed boosted to: " + player.walkSpeed);

        yield return new WaitForSeconds(speedBoostTime);

        player.walkSpeed = originalSpeed;
        player.airWalkSpeed = originalAirSpeed;
        Debug.Log("Speed reset to: " + player.walkSpeed);

        Destroy(gameObject); // Artýk güvenli þekilde silebiliriz
    }
}