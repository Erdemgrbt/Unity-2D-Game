using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;           // Ok firlatma bekleme suresi
    [SerializeField] private Transform firePoint;            // Okun cikacagi nokta
    [SerializeField] private GameObject[] arrows;            // Havuzlanmis oklar
    private float cooldownTimer;                             // Gecen zaman

    [SerializeField] private AudioClip arrowSound;
    

    
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        
        if (cooldownTimer >= attackCooldown)
            Attack();
    }
    

   
    private void Attack()
    {
        cooldownTimer = 0;

        GetComponent<SFXSound>().PlaySound(arrowSound);

        int index = FindArrow(); // Kullanilmayan oku bul
        arrows[index].transform.position = firePoint.position;
        arrows[index].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }

        // Hepsi aktifse ilk oku kullan (opsiyonel: havuz buyutulebilir)
        return 0;
    }
    
}
