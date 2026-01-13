using System.Collections.Generic;
using UnityEngine;

public class SummonEnemy : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner; // 소환할 타워

    public List<GameObject> towerPosition = new List<GameObject>(); // 타워 소환 위치 리스트

    private int randomIndex = -1; // 랜덤 인덱스 변수
    public void Summon()
    {
        int tryCount = 0;
        int maxTry = towerPosition.Count;

        SpawnPosition sp = towerPosition[randomIndex].GetComponent<SpawnPosition>(); // SpawnPosition 스크립트 참조   

        while (tryCount < maxTry * 2)// 최대 시도 횟수 설정
        {
            int randomIndex = Random.Range(0, towerPosition.Count);
           
           if (sp.IsBuildTower < 2)
            {
                // 조건 만족 → 소환
                towerSpawner.SpawnTower(towerPosition[randomIndex].transform);
                return;
            }

            tryCount++;
        }
        // 모든 위치에 타워가 2개 이상 있다는 뜻
        Debug.Log("모든 타워 위치가 가득 차서 소환할 수 없습니다.");
    }
}
