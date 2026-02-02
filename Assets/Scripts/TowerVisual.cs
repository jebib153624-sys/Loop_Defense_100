using UnityEngine;

public class TowerVisual : MonoBehaviour
{
    [SerializeField] private TowerType towerType;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TowerSpriteSet[] spriteSets;

    private void Awake()
    {
        if (!towerType)
            towerType = GetComponent<TowerType>();

        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateVisual()
    {
        foreach (var set in spriteSets)
        {
            if (set.type == towerType.towerType && set.rank == towerType.towerRank)
            {
                spriteRenderer.sprite = set.sprite;
                return;
            }
        }

        Debug.LogWarning("해당 Tower 타입/랭크에 맞는 Sprite 없음");
    }
}

[System.Serializable]
public class TowerSpriteSet
{
    public TowerTypes type;
    public TowerRank rank;
    public Sprite sprite;
}