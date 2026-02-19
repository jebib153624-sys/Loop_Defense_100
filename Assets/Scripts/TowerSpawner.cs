using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private EnemySpawner enemySpawner; // ??? ?????


    [SerializeField]
    private Gold gold; // ?? ?? ?????

    public List<UpgradeJudge> towerList = new List<UpgradeJudge>();

    public void SpawnTower(Transform position, SpawnPosition sp)
    {



        /*   // ?? ??? ?? ???? ?? ??? ?? //?? ????? ?? ???? (?? ?????)?? ???? ?? ???
        if(spawnPosition.IsBuildTower >= 2)
        {
            return;
        }*/

        // ?? ???? 0/1? ??
        sp.IsBuildTower = 1;

        GameObject clone = Instantiate(towerPrefab, position.position, Quaternion.identity);
        towerList.Add(clone.GetComponent<UpgradeJudge>());
        clone.GetComponent<TowerMover>().GetSpawnPosition(sp);//???? ????? ????? ?? ??
        //clone.GetComponent<TowerMover>().synthesisButton.SetActive(false);
        TowerType towerTypeComp = clone.GetComponent<TowerType>();//???? ???? ????
        TowerTypes randomType = (TowerTypes)Random.Range(0, System.Enum.GetValues(typeof(TowerTypes)).Length);//???? ???? ???
        towerTypeComp.towerType = randomType;//???? ????? ???? ?? ?? ??
        AudioManager.instance.PlaySfx(10);
        towerTypeComp.ApplyStats(); //?? ?? ??

        clone.GetComponent<TowerVisual>().UpdateVisual(); //?? ??? ????

        TowerWeapon towerWeapon = clone.GetComponent<TowerWeapon>();
        towerWeapon.Setup(enemySpawner, sp);
    }
}
