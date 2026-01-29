using UnityEngine;

public class TowerType : MonoBehaviour
{
    public TowerTypes towerType;
    public TowerRank towerRank;
}

public enum TowerTypes
{
    BasicTower,
    SniperTower,
    CannonTower,
    FreezeTower
}
public enum TowerRank
{
    Rank1,
    Rank2,
    Rank3,
    Rank4,
    Rank5
}