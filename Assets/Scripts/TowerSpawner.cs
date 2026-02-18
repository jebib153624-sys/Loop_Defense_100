using Mono.Cecil;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private EnemySpawner enemySpawner; // 적정보 받아오려고 


    [SerializeField]
    private Gold gold; // 골드 정보 받아오려고

    public List<UpgradeJudge> towerList = new List<UpgradeJudge>();

    public void SpawnTower(Transform position, SpawnPosition sp)
    {



        /*   // 위치 넣는걸 함수 실행하기 전에 검사할 예정 //갯수 제한하려고 만든 스크립트 (맵에 깔려있는거)에서 가져와서 갯수 제한함 
        if(spawnPosition.IsBuildTower >= 2)
        {
            return;
        }*/

        sp.IsBuildTower++;

        GameObject clone = Instantiate(towerPrefab, position.position, Quaternion.identity);
        towerList.Add(clone.GetComponent<UpgradeJudge>());
        clone.GetComponent<TowerMover>().GetSpawnPosition(sp);//타워무버 스크립트에 스폰포지션 정보 넣기
        //clone.GetComponent<TowerMover>().synthesisButton.SetActive(false);
        TowerType towerTypeComp = clone.GetComponent<TowerType>();//타워타입 스크립트 가져오기
        TowerTypes randomType = (TowerTypes)Random.Range(0, System.Enum.GetValues(typeof(TowerTypes)).Length);//타워타입 랜덤으로 정하기
        towerTypeComp.towerType = randomType;//타워타입 스크립트에 랜덤으로 정한 타입 넣기

        towerTypeComp.ApplyStats(); //타워 스탯 적용

        clone.GetComponent<TowerVisual>().UpdateVisual(); //타워 비주얼 업데이트

        TowerWeapon towerWeapon = clone.GetComponent<TowerWeapon>();
        towerWeapon.Setup(enemySpawner, sp);
    }
}
