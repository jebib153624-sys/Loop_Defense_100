using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;     // 적 프리팹

    [SerializeField]
    private float spawnTime;            // 적 생성 주기

    [SerializeField]
    private Transform[] wayPoints;      // 현재 스테이지의 이동 경로

    [SerializeField]
    private float enemySpeed; //적 속도 스폰 할떄 마다 적한테 넘겨줌 

    private List<Enemy> enemyList; //맵에 있는 적들의 정보 리스트 

    public List<Enemy> EnemyList => enemyList; // 프로퍼티 근데 set기능은 없음 lead onry

    private void Awake()
    {
        //적 리스트 메모리 할당
        enemyList = new List<Enemy>();
        // 적 생성 코루틴 함수 호출
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject clone = Instantiate(enemyPrefab);     // 적 오브젝트 생성
            Enemy enemy = clone.GetComponent<Enemy>();       // 방금 생성된 적의 Enemy 컴포넌트

            enemy.Setup(this , wayPoints);                           // wayPoint 정보를 매개변수로 Setup() 호출
            enemyList.Add(enemy);
            enemy.moveSpeed = enemySpeed;
            yield return new WaitForSeconds(spawnTime);       // spawnTime 시간 동안 대기
        }
    } 
    public void DestroyEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
}
