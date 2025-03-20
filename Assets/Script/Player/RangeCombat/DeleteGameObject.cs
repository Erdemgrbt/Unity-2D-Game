using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteGameObject : MonoBehaviour
{
    public float lifetime = 2f; // Nesnenin sahnede kalma süresi (saniye cinsinden)

    void Start()
    {
        // Belirtilen süre sonra bu nesneyi yok et
        Destroy(gameObject, lifetime);
    }
}