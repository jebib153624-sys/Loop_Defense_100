using UnityEngine;

public class WavaSystem : MonoBehaviour
{
    [SerializeField]
    public Wava[] wavas; // 웨이브 배열
    [SerializeField]
    private EnemySpawner enemySpawner; // 적 스포너
    public int currentWavaIndex = -1; // 현재 웨이브 인덱스

    public void StartWave()
    {
        if (enemySpawner.EnemyList.Count == 0 && currentWavaIndex < wavas.Length - 1) // 적이 모두 처치되고 다음 웨이브가 존재할 때
        {
            currentWavaIndex++; // 웨이브 인덱스 증가 (시작 인덱스가 -1이기 떄문)
            enemySpawner.StartWave(wavas[currentWavaIndex]);
        }
    }
}
[System.Serializable]
public struct Wava
{
    public float spawnTime; // 현제 웨이브 적 생성 주기
    public int maxEnemyCount; // 현제 웨이브 적 최대 생성 수
    public GameObject[] enemyPrefab; // 현제 웨이브 적 프리팹
}
