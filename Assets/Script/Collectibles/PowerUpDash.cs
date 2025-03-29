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
            // Dash hakkýný yenile
            player.ResetDash();

            // Boncuðu devre dýþý býrak

            // Ýstersen burada bir efekt veya ses de oynatabilirsin
        }
    }
}
