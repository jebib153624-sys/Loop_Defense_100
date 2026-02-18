using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] wayPoints;

    [SerializeField]
    private float enemySpeed;

    [SerializeField]
    private GameObject eliteEnemyPrefab;

    [SerializeField]
    private List<Enemy> enemyList;

    public List<Enemy> EnemyList => enemyList;

    [SerializeField]
    private GameObject enemyHPSliderPrefab;

    [SerializeField]
    private Transform canvasTransform;

    [SerializeField]
    private Gold gold;

    [SerializeField]
    private PlayerHP playerHP;

    private Wava currenWave;

    private void Awake()
    {
        enemyList = new List<Enemy>();
    }

    public void StartWave(Wava wave)
    {
        currenWave = wave;
        StartCoroutine("SpawnEnemy");
    }

    public void SpawnEliteEnemy()
    {
        if (eliteEnemyPrefab == null)
        {
            Debug.LogWarning("Elite enemy prefab is not assigned on EnemySpawner.");
            return;
        }

        SpawnEnemy(eliteEnemyPrefab);
    }

    private IEnumerator SpawnEnemy()
    {
        int spawnEnemyCount = 0;

        while (spawnEnemyCount < currenWave.maxEnemyCount)
        {
            int enemyIndex = Random.Range(0, currenWave.enemyPrefab.Length);

            if (SpawnEnemy(currenWave.enemyPrefab[enemyIndex]) != null)
            {
                spawnEnemyCount++;
            }

            yield return new WaitForSeconds(currenWave.spawnTime);
        }
    }

    private Enemy SpawnEnemy(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Enemy prefab is null.");
            return null;
        }

        GameObject clone = Instantiate(prefab);
        Enemy enemy = clone.GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogWarning($"Spawned object '{clone.name}' has no Enemy component.");
            Destroy(clone);
            return null;
        }

        enemy.Setup(this, wayPoints);
        enemyList.Add(enemy);
        enemy.moveSpeed = enemySpeed;

        SpawnEnemyHPSlider(clone);
        playerHP.EnemyCountUpdate();

        EnemyVisual visual = clone.GetComponent<EnemyVisual>();
        if (visual != null)
            visual.EnemyWalk();

        return enemy;
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        sliderClone.transform.SetParent(canvasTransform);
        sliderClone.transform.localScale = Vector3.one;

        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }

    public void DestroyEnemy(Enemy enemy, int gold , int energy)
    {
        this.gold.CurrentGold += gold;
        this.gold.CurrentEnergy += energy;
        enemyList.Remove(enemy);
        playerHP.EnemyCountUpdate();
        Destroy(enemy.gameObject);
    }
}