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
        if (myTowerType.towerRank >= TowerRank.Rank5)
            return;

        foreach (UpgradeJudge other in towerSpawner.towerList)
        {
            if (other == this)
                continue;

            TowerType otherType = other.GetComponent<TowerType>(); // 다른 타워의 TowerType 정보 가져오기

            // 같은 타워 + 같은 랭크 조건
            if (otherType.towerType != myTowerType.towerType)
                continue;

            if (otherType.towerRank != myTowerType.towerRank)
                continue;

            // ===== 합성 실행 =====

            // 랭크 +1
            myTowerType.towerRank++;

            // 리스트에서 제거
            towerSpawner.towerList.Remove(other);

            // 상대 파괴
            GetComponent<TowerVisual>().UpdateVisual();// 비주얼 업데이트
            other.GetComponent<TowerWeapon>().spawnPosition.IsBuildTower--;// 스폰 가능한 포지션으로 변경 
            Destroy(other.gameObject);
            Debug.Log("합성 성공!");
            return;
        }
    }
}