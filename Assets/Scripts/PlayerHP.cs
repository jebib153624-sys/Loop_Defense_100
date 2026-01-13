using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner enemySpawner;

    public int currentEnemy = 0;

    public void EnemyCountUpdate()
    {
        currentEnemy = enemySpawner.EnemyList.Count;
    }
}
