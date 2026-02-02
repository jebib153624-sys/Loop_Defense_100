using UnityEngine;

public class TowerType : MonoBehaviour
{
    public TowerTypes towerType;
    public TowerRank towerRank;
    public States states;
}

public enum TowerTypes
{
    WarriorTower,
    AssassinTower,
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
[System.Serializable]
public struct States
{
    public float damage;//공격력
    public float rate;//공격속도
    public float Range;//사거리
    public int cost;//가격
}