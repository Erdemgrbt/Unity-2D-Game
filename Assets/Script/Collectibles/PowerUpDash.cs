using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDash : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            // Dash hakkini yenile
            player.ResetDash();

            // Boncugu devre disi birak
            Destroy(gameObject);

        }
    }
}
