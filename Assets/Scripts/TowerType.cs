using UnityEngine;

public class TowerType : MonoBehaviour
{
    private const float MinAttackInterval = 0.35f;

    public TowerTypes towerType;
    public TowerRank towerRank;

    public States[] warriorRankStates = new States[5];
    public States[] icerankStates = new States[5];

    public States states;

    private void Start()
    {
        ApplyStats();
    }

    public void ApplyStats()
    {
        int idx = Mathf.Clamp((int)towerRank, 0, 4);

        States baseStats = (towerType == TowerTypes.WarriorTower)
            ? warriorRankStates[idx]
            : icerankStates[idx];

        States bonus = new States();
        if (TowerRankUpgradeSystem.Instance != null)
        {
            bonus = TowerRankUpgradeSystem.Instance.GetBonus(towerType, idx);
        }

        states.damage = baseStats.damage + bonus.damage;
        states.rate = Mathf.Max(MinAttackInterval, baseStats.rate + bonus.rate);
        states.Range = baseStats.Range + bonus.Range;
        states.sell = baseStats.sell + bonus.sell;
        states.slow = baseStats.slow + bonus.slow;
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
    public float damage;
    public float rate;
    public float Range;
    public int sell;
    public float slow;
}

