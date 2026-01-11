using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private EnemySpawner enemySpawner; // 적정보 받아오려고 

    public void SpawnTower(Transform position)
    {

        SpawnPosition spawnPosition = position.GetComponent<SpawnPosition>();

        //갯수 제한하려고 만든 스크립트 (맵에 깔려있는거)에서 가져와서 갯주 제한함 
        if(spawnPosition.IsBuildTower >= 2)
        {
            return;
        }

        spawnPosition.IsBuildTower++;

        GameObject clone = Instantiate(towerPrefab, position.position, Quaternion.identity);
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
    }
}
