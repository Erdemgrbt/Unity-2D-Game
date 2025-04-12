using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUIText : MonoBehaviour
{
    public GameObject uiTextObject; // Ekranda g�r�necek yaz� objesi

    private void Start()
    {
        if (uiTextObject != null)
            uiTextObject.SetActive(false); // Ba�ta gizli olsun
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            uiTextObject.SetActive(true); // Oyuncu girerse yaz�y� g�ster
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            uiTextObject.SetActive(false); // Oyuncu ��karsa yaz�y� gizle
    }
}