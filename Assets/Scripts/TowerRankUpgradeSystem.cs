using UnityEngine;

public class TowerRankUpgradeSystem : MonoBehaviour
{
    public static TowerRankUpgradeSystem Instance;

    [Header("골드")]
    [SerializeField] private Gold gold;

    [Header("랭크별 강화 레벨 (1=미강화)")]
    [SerializeField] private int[] rankLevel = new int[5] { 1, 1, 1, 1, 1 };
    [SerializeField] private int[] rankMaxLevel = new int[5] { 50, 50, 50, 50, 50 };

    [Header("랭크별 강화 비용")]
    [SerializeField] private int[] baseCost = new int[5] { 50, 80, 120, 170, 230 };
    [SerializeField] private int[] costStep = new int[5] { 10, 15, 20, 30, 40 };

    [Header("전사: 랭크별 1강당 증가치")]
    [SerializeField] private States[] warriorPerLevelBonus = new States[5];

    [Header("빙결: 랭크별 1강당 증가치")]
    [SerializeField] private States[] icePerLevelBonus = new States[5];

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public int GetRankLevel(int rankIndex)
    {
        return rankLevel[rankIndex];
    }

    public int GetUpgradeCost(int rankIndex)
    {
        int step = Mathf.Max(0, rankLevel[rankIndex] - 1);
        return baseCost[rankIndex] + (costStep[rankIndex] * step);
    }

    public States GetBonus(TowerTypes type, int rankIndex)
    {
        States per = (type == TowerTypes.WarriorTower) ? warriorPerLevelBonus[rankIndex] : icePerLevelBonus[rankIndex];
        int count = Mathf.Max(0, rankLevel[rankIndex] - 1);

        States bonus = new States();
        bonus.damage = per.damage * count;
        bonus.rate = per.rate * count;
        bonus.Range = per.Range * count;
        bonus.sell = Mathf.RoundToInt(per.sell * count);
        bonus.slow = per.slow * count;
        return bonus;
    }

    public void UpgradeRank1() { TryUpgradeRank(0); }
    public void UpgradeRank2() { TryUpgradeRank(1); }
    public void UpgradeRank3() { TryUpgradeRank(2); }
    public void UpgradeRank4() { TryUpgradeRank(3); }
    public void UpgradeRank5() { TryUpgradeRank(4); }

    public void TryUpgradeRank(int rankIndex)
    {
        if (rankIndex < 0 || rankIndex > 4)
            return;

        if (rankLevel[rankIndex] >= rankMaxLevel[rankIndex])
            return;

        if (gold == null)
            gold = FindFirstObjectByType<Gold>();

        if (gold == null)
            return;

        int cost = GetUpgradeCost(rankIndex);
        if (gold.CurrentGold < cost)
        {
            Debug.Log("골드 부족");
            return;
        }

        gold.CurrentGold -= cost;
        rankLevel[rankIndex]++;
        AudioManager.instance.PlaySfx(9);
        RefreshAllTowers();
        Debug.Log($"Rank {rankIndex + 1} 강화 성공. 현재 레벨: {rankLevel[rankIndex]}");
    }

    private void RefreshAllTowers()
    {
        TowerType[] towers = FindObjectsByType<TowerType>(FindObjectsSortMode.None);
        for (int i = 0; i < towers.Length; i++)
        {
            towers[i].ApplyStats();
        }
    }
}
