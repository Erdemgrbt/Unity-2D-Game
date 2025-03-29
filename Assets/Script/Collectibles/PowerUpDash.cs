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
            // Dash hakk�n� yenile
            player.ResetDash();

            // Boncu�u devre d��� b�rak

            // �stersen burada bir efekt veya ses de oynatabilirsin
        }
    }
}
