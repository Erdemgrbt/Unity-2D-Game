using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect;
    public float bufferZone = 2f; // Tampon de�eri (arka plan�n tekrar ekranda g�r�nmesi i�in ek mesafe)

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // Arka plan�n kopyalanma zaman�n� daha erken tetikle
        if (movement > startPos + length - bufferZone)
        {
            startPos += length;
        }
        else if (movement < startPos - length + bufferZone)
        {
            startPos -= length;
        }
    }
}