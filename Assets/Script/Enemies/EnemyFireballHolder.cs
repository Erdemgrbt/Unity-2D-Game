using UnityEngine;

public class EnemyFireballHolder : MonoBehaviour
{
    [SerializeField] private Transform enemy;

    private void Update()
    {
        if (enemy != null)
            transform.localScale = enemy.localScale;

        if (enemy == null)
            Destroy(gameObject);
    }
}