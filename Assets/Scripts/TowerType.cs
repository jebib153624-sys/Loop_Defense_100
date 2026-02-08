using UnityEngine;

public class TowerType : MonoBehaviour
{
    public TowerTypes towerType;
    public TowerRank towerRank;
    public States[] warriorRankStates = new States[5];
    public States[] icerankStates = new States[5];
    public States states;

  

    public void ApplyStats()
    {
        int index = (int)towerRank;
        if (towerType == TowerTypes.WarriorTower)
        {
            states = warriorRankStates[index];
        }
        else
        {
            states = icerankStates[index];
        }
    }
}

public enum TowerTypes
{
    WarriorTower,
    //AssassinTower,
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
    public int sell;//판매가격
    public float slow;//감속퍼센트
}