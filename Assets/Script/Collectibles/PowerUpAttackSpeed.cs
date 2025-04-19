using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAttackSpeed : MonoBehaviour
{
    [SerializeField] private float attackSpeedBoostTime = 1f;
    [SerializeField] private int attackSpeedBoost = 1;
    [SerializeField] private AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RangeAttack rangeAttack = collision.GetComponentInChildren<RangeAttack>();
        SoundManager.instance.PlaySound(collectSound);

        if (rangeAttack != null)
        {
            StartCoroutine(AttackSpeedBoost(rangeAttack));
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private IEnumerator AttackSpeedBoost(RangeAttack rangeAttack)
    {
        rangeAttack.isAttackSpeedBoosted = true;

        float originalCooldown = rangeAttack.fireCooldown;

        rangeAttack.fireCooldown = originalCooldown / attackSpeedBoost;

        Debug.Log("Attack speed boost aktif! Yeni cooldown: " + rangeAttack.fireCooldown);

        yield return new WaitForSeconds(attackSpeedBoostTime);

        rangeAttack.fireCooldown = rangeAttack.originalFireCooldown;
        rangeAttack.isAttackSpeedBoosted = false;

        Debug.Log("Attack speed boost sona erdi.");

        Destroy(gameObject);
    }
}
