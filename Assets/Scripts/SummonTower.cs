using System;
using System.Collections.Generic;
using UnityEngine;

public class SummonTower : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner; // 소환할 타워

    public List<GameObject> towerPosition = new List<GameObject>(); // 타워 소환 위치 리스트

    public int towerPosindex = -1; 
    public void Summon()
    {
        for (int i = 0; i < towerPosition.Count; i++)
        {
            SpawnPosition sp = towerPosition[i].GetComponent<SpawnPosition>();

            if (sp.IsBuildTower < 1)
            {
                towerSpawner.SpawnTower(towerPosition[i].transform);
                towerPosindex = i;
                return;
            }
        }

        Debug.Log("더 이상 소환할 타워 위치가 없습니다.");
    }

}
