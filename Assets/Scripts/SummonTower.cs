using System;
using System.Collections.Generic;
using UnityEngine;

public class SummonTower : MonoBehaviour
{
    public int SummonEnergyCost = 20;

    [SerializeField]
    private TowerSpawner towerSpawner; // 소환할 타워

    [SerializeField]
    private Gold gold;

    public List<GameObject> towerPosition = new List<GameObject>(); // 타워 소환 위치 리스트

    public int towerPosindex = -1;

    public void Summon()
    {
        if (gold == null)
        {
            Debug.Log("Gold 컴포넌트를 찾지 못해 소환할 수 없습니다.");
            return;
        }

        if (gold.CurrentEnergy < SummonEnergyCost)
        {
            Debug.Log("에너지가 부족하여 소환할 수 없습니다.");
            return;
        }

        SummonEnergyCost = SummonEnergyCost + 2;

        for (int i = 0; i < towerPosition.Count; i++)
        {
            SpawnPosition sp = towerPosition[i].GetComponent<SpawnPosition>();

            if (sp.IsBuildTower < 1)
            {
                towerSpawner.SpawnTower(towerPosition[i].transform, sp);
                gold.CurrentEnergy -= SummonEnergyCost;
                towerPosindex = i;
                return;
            }
        }

        Debug.Log("더 이상 소환할 타워 위치가 없습니다.");
    }
}