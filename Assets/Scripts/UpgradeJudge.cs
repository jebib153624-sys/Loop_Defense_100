using UnityEngine;

public class UpgradeJudge : MonoBehaviour
{
    private TowerType myTowerType;

    public int myListNum = -1;

    private TowerWeapon weaponState;



    private void Start()
    {
        myTowerType = GetComponent<TowerType>();
        weaponState = GetComponent<TowerWeapon>();
    }

    // 외부(TowerMover)에서 호출하는 합성 체크 함수
    public bool TryUpgrade(GameObject target)
    {
        TowerType other = target.GetComponent<TowerType>();

        if (other != null && other.towerType == myTowerType.towerType && other.towerRank == myTowerType.towerRank)
        {
            if (other.towerRank < TowerRank.Rank5)
            {
                other.towerRank = (TowerRank)((int)other.towerRank + 1);

                // 랭크업 시 시각적 업데이트 (Sprite 변경 등)를 호출하는 코드가 여기 오면 좋습니다.
                target.GetComponent<TowerVisual>().UpdateVisual();
                // other.UpdateVisual(); 

                Debug.Log($"합성 성공! 현재 랭크: {other.towerRank}");

                weaponState.spawnPosition.IsBuildTower--; // 합성 후 드래그한 타워 위치의 빌드 상태 해제
                Destroy(this.gameObject); // 드래그하던 나 자신 삭제
                return true;
            }
        }
        return false;
    }
}