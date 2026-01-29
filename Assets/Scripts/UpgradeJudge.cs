using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UpgradeJudge : MonoBehaviour
{
    private TowerType myTowerType;
    private TowerMover towerMover;

    private void Start()
    {
        myTowerType = GetComponent<TowerType>();
        towerMover = GetComponent<TowerMover>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("충돌 왜안하지 시발1");
        if (towerMover.IsPickupCooldown) return;
        Debug.Log("충돌 왜안하지 시발2");
        TowerType other = collision.GetComponent<TowerType>();

        if (other.towerType == myTowerType.towerType && other.towerRank == myTowerType.towerRank)
        {
            if (other.towerRank < TowerRank.Rank5)
            {
                other.towerRank = (TowerRank)((int)other.towerRank + 1);
            }

            Debug.Log("같은 타입 타워 충돌 → 랭크 업");

            Destroy(this.gameObject);
        }
    }

}
