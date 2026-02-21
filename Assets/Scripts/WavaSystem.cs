using System.Collections;
using UnityEngine;

public class WavaSystem : MonoBehaviour
{
    [SerializeField]
    public Wava[] wavas;

    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private float initialDelay = 10f;

    public int currentWavaIndex = -1;

    public float waveDuration = 40f;
    public float timeBetweenWaves { get; private set; }

    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        yield return StartCoroutine(Countdown(initialDelay));

        while (currentWavaIndex < wavas.Length - 1)
        {
            currentWavaIndex++;
            Wava currentWave = wavas[currentWavaIndex];

            enemySpawner.StartWave(currentWave, currentWavaIndex);

            yield return StartCoroutine(Countdown(waveDuration));
        }

        Debug.Log("모든 웨이브 종료!");
    }

    private IEnumerator Countdown(float duration)
    {
        timeBetweenWaves = Mathf.Max(0f, duration);

        while (timeBetweenWaves > 0f)
        {
            timeBetweenWaves -= Time.deltaTime;
            yield return null;
        }

        timeBetweenWaves = 0f;
    }
}

[System.Serializable]
public struct Wava
{
    public float spawnTime;        // 적 생성 주기
    public int maxEnemyCount;      // 최대 생성 수
    public GameObject[] enemyPrefab;

    //public float waveDuration;     // 웨이브 지속 시간
}

