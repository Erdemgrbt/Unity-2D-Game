using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUIText : MonoBehaviour
{
    public GameObject uiTextObject; // Ekranda görünecek yazý objesi

    private void Start()
    {
        if (uiTextObject != null)
            uiTextObject.SetActive(false); // Baþta gizli olsun
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            uiTextObject.SetActive(true); // Oyuncu girerse yazýyý göster
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            uiTextObject.SetActive(false); // Oyuncu çýkarsa yazýyý gizle
    }
}