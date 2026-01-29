using System;
using System.Collections.Generic;
using UnityEngine;

public class SummonTower : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner; // 소환할 타워

    public List<GameObject> towerPosition = new List<GameObject>(); // 타워 소환 위치 리스트

    private int index = 0; // 랜덤 인덱스 변수
    public void Summon()
    {
        if (index >= towerPosition.Count)
        {
            Debug.Log("더 이상 소환할 타워 위치가 없습니다.");
            return;
        }

        SpawnPosition sp = towerPosition[index].GetComponent<SpawnPosition>();

        if (sp.IsBuildTower < 1)
        {
            towerSpawner.SpawnTower(towerPosition[index].transform);
            index++;
            return;
        }

        index++; // 현재 위치가 막혀있으면 다음 위치로 이동
    }

}
