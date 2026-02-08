using UnityEngine;

public class UpgradeJudge : MonoBehaviour
{
    private TowerType myTowerType;

    private TowerWeapon weaponState;

    private TowerSpawner towerSpawner;
    private void Awake()
    {
        towerSpawner = FindFirstObjectByType<TowerSpawner>();
    }

    private void Start()
    {
        myTowerType = GetComponent<TowerType>();
        weaponState = GetComponent<TowerWeapon>();
    }

    // 외부(TowerMover)에서 호출하는 합성 체크 함수
    public void TryUpgrade()
    {
        var list = towerSpawner.towerList;

        for (int i = 0; i < list.Count; i++)
        {
            UpgradeJudge baseTower = list[i];
            TowerType baseType = baseTower.GetComponent<TowerType>();

            // Rank5는 합성 불가
            if (baseType.towerRank == TowerRank.Rank5)
                continue;

            for (int j = i + 1; j < list.Count; j++)
            {
                UpgradeJudge targetTower = list[j];
                TowerType targetType = targetTower.GetComponent<TowerType>();

                // target도 Rank5면 스킵
                if (targetType.towerRank == TowerRank.Rank5)
                    continue;

                if (baseType.towerType == targetType.towerType &&
                    baseType.towerRank == targetType.towerRank)
                {
                    Debug.Log("합성 성공!");

                    // 랭크 +1 (안전하게)
                    baseType.towerRank =
                        (TowerRank)((int)baseType.towerRank + 1);

                    Destroy(targetTower.gameObject);
                    list.RemoveAt(j);

                    return;
                }
            }
        }

        Debug.Log("합성 가능한 타워 없음");
    }

}