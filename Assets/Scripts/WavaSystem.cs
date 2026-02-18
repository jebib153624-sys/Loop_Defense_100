using System.Collections;
using UnityEngine;

public class WavaSystem : MonoBehaviour
{
    [SerializeField]
    public Wava[] wavas;

    [SerializeField]
    private EnemySpawner enemySpawner;

    public int currentWavaIndex = -1;

    public float timeBetweenWaves = 10f; // 웨이브 간 대기 시간
    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        yield return new WaitForSeconds(10f);
        while (currentWavaIndex < wavas.Length - 1)
        {
            currentWavaIndex++;

            Wava currentWave = wavas[currentWavaIndex];

            // 다음 웨이브 duration (없으면 0)
            float nextWaveDuration = 0f;
            if (currentWavaIndex + 1 < wavas.Length)
            {
                nextWaveDuration = wavas[currentWavaIndex + 1].waveDuration;
            }

            enemySpawner.StartWave(currentWave);

            timeBetweenWaves = currentWave.waveDuration + nextWaveDuration;

            yield return new WaitForSeconds(currentWave.waveDuration);
        }

        Debug.Log("모든 웨이브 종료!");
    }
}

[System.Serializable]
public struct Wava
{
    public float spawnTime;        // 적 생성 주기
    public int maxEnemyCount;      // 최대 생성 수
    public GameObject[] enemyPrefab;

    public float waveDuration;     // 웨이브 지속 시간 (추가됨)
}
