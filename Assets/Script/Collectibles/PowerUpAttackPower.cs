using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAttackPower : MonoBehaviour
{
    [SerializeField] private float attackPowerBoostTime = 1f;
    [SerializeField] private int attackPowerBoost= 1;
    [SerializeField] private AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RangeAttack rangeAttack = collision.GetComponentInChildren<RangeAttack>();
        SoundManager.instance.PlaySound(collectSound);
        if (rangeAttack != null)
        {
            StartCoroutine(AttackPowerBoost(rangeAttack));
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private IEnumerator AttackPowerBoost(RangeAttack rangeAttack)
    {
        rangeAttack.isBoosted = true;
        rangeAttack.bonusDamage = attackPowerBoost;

        Debug.Log("Boost aktifleþtirildi!");

        yield return new WaitForSeconds(attackPowerBoostTime);

        rangeAttack.isBoosted = false;
        rangeAttack.bonusDamage = 0;

        Debug.Log("Boost sona erdi.");

        Destroy(gameObject);
    }
}
