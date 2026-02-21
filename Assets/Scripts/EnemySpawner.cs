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

    [Header("Wave HP Boost")]
    [SerializeField] private float hpIncreasePerWave ;

    private Coroutine spawnRoutine;
    private int currentWaveIndex = 0;

    public Transform path;

    private void Awake()
    {
        enemyList = new List<Enemy>();
    }

    public void StartWave(Wava wave, int waveIndex)
    {
        currentWaveIndex = Mathf.Max(0, waveIndex);

        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }

        spawnRoutine = StartCoroutine(SpawnEnemy(wave));
    }

    public void SpawnEliteEnemy()
    {
        if (eliteEnemyPrefab == null)
        {
            Debug.LogWarning("Elite enemy prefab is not assigned on EnemySpawner.");
            return;
        }

        if (gold.CurrentEnergy >= 100f)
        {
            AudioManager.instance.PlaySfx(8);
            gold.CurrentEnergy -= 100;
            SpawnEnemy(eliteEnemyPrefab);
        }
    }

    private IEnumerator SpawnEnemy(Wava wave)
    {
        int spawnEnemyCount = 0;
        float elapsed = 0f;
        float spawnInterval = Mathf.Max(0.01f, wave.spawnTime);

        while (spawnEnemyCount < wave.maxEnemyCount && elapsed < 40f)
        {
            if (wave.enemyPrefab == null || wave.enemyPrefab.Length == 0)
            {
                Debug.LogWarning("Wave enemyPrefab array is empty.");
                break;
            }

            int enemyIndex = Random.Range(0, wave.enemyPrefab.Length);

            if (SpawnEnemy(wave.enemyPrefab[enemyIndex]) != null)
                spawnEnemyCount++;

            yield return new WaitForSeconds(spawnInterval);
            elapsed += spawnInterval;
        }

        spawnRoutine = null;
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

        EnemyHP enemyHP = clone.GetComponent<EnemyHP>();
        if (enemyHP != null)
        {
            float hpMultiplier = 1f + (currentWaveIndex * hpIncreasePerWave);
            enemyHP.ApplyWaveHpMultiplier(hpMultiplier);
        }

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

        sliderClone.transform.SetParent(path, false);
        sliderClone.transform.localScale = Vector3.one;

        SliderPositionAutoSetter positionSetter = sliderClone.GetComponent<SliderPositionAutoSetter>();
        Enemy enemyComp = enemy.GetComponent<Enemy>();

        if (positionSetter != null)
        {
            if (enemyComp != null)
                positionSetter.Setup(enemy.transform, enemyComp.HpBarScreenOffset);
            else
                positionSetter.Setup(enemy.transform);
        }

        EnemyHPViewer hpViewer = sliderClone.GetComponent<EnemyHPViewer>();
        if (hpViewer != null)
            hpViewer.Setup(enemy.GetComponent<EnemyHP>());
    }

    public void NotifyEnemyDead(Enemy enemy, int rewardGold, int rewardEnergy)
    {
        gold.CurrentGold += rewardGold;
        gold.CurrentEnergy += rewardEnergy;
        enemyList.Remove(enemy);
        playerHP.EnemyCountUpdate();
    }

    public void FinalizeEnemyDestroy(Enemy enemy)
    {
        if (enemy == null)
            return;

        Destroy(enemy.gameObject);
    }

    public void DestroyEnemy(Enemy enemy, int rewardGold, int rewardEnergy)
    {
        NotifyEnemyDead(enemy, rewardGold, rewardEnergy);
        FinalizeEnemyDestroy(enemy);
    }
}

