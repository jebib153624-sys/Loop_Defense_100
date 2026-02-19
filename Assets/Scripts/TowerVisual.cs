using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class TowerVisual : MonoBehaviour
{
    [SerializeField] private TowerType towerType;
    public SkeletonAnimation spine;
    [SerializeField] private TowerSpineSet[] spineSets;
    [SerializeField] private float animationSpeed = 1.5f;

    [Header("Floor Effect")]
    [SerializeField] private Transform floorEffectRoot;

    // Rank1 ~ Rank5: Gray, Blue, Purple, Yellow, Red
    [SerializeField] private Color[] rankColors = new Color[5]
    {
        new Color32(220, 220, 220, 255), // Gray (bright)
        new Color32(0, 149, 255, 255),   // Blue (vivid)
        new Color32(177, 0, 255, 255),   // Purple (vivid)
        new Color32(255, 214, 0, 255),   // Yellow (vivid)
        new Color32(255, 36, 0, 255)     // Red (vivid)
    };

    private SkeletonDataAsset currentSkeleton; // 캐시

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorId = Shader.PropertyToID("_Color");
    private static readonly int TintColorId = Shader.PropertyToID("_TintColor");

    private void Awake()
    {
        if (!towerType)
            towerType = GetComponentInParent<TowerType>();

        if (!spine)
            spine = GetComponentInChildren<SkeletonAnimation>();

        if (spine != null)
            spine.timeScale = animationSpeed;

        CacheFloorEffectRoot();
    }

    public void UpdateVisual()
    {
        ApplyRankEffectColor();

        foreach (var set in spineSets)
        {
            if (set.type == towerType.towerType && set.rank == towerType.towerRank)
            {
                // Skeleton 교체 시 공격 상태 꼬임 방지
                if (currentSkeleton != set.skeletonData)
                {
                    currentSkeleton = set.skeletonData;
                    IsAttackPlaying = false;

                    spine.skeletonDataAsset = currentSkeleton;
                    spine.Initialize(true);
                    spine.timeScale = animationSpeed;
                }

                spine.AnimationState.SetAnimation(0, set.idleAnimation, true);
                spine.timeScale = animationSpeed;
                return;
            }
        }
    }

    public bool IsAttackPlaying { get; private set; }

    public void PlayAttackOnce(System.Action onHit)
    {
        if (spine == null || spine.AnimationState == null) return;
        if (IsAttackPlaying) return;

        foreach (var set in spineSets)
        {
            if (set.type != towerType.towerType || set.rank != towerType.towerRank)
                continue;

            // 애니 이름이 비어있거나 없으면 콜백만 보장
            if (string.IsNullOrEmpty(set.attackAnimation) ||
                spine.Skeleton?.Data?.FindAnimation(set.attackAnimation) == null)
            {
                onHit?.Invoke();
                return;
            }

            IsAttackPlaying = true;
            var entry = spine.AnimationState.SetAnimation(0, set.attackAnimation, false);

            bool hitCalled = false;

            entry.Event += (trackEntry, e) =>
            {
                if (!hitCalled && e.Data.Name == "Attack_Hit")
                {
                    AudioManager.instance.PlaySfx(5);
                    hitCalled = true;
                    onHit?.Invoke();
                }
                else if (!hitCalled && e.Data.Name == "Spell_Trigger")
                {
                    AudioManager.instance.PlaySfx(7);
                    hitCalled = true;
                    onHit?.Invoke();
                }

            };

            entry.Complete += _ =>
            {
                // 이벤트가 빠져도 데미지 1회 보장
                if (!hitCalled)
                {
                    hitCalled = true;
                    onHit?.Invoke();
                }

                IsAttackPlaying = false;
                spine.AnimationState.SetAnimation(0, set.idleAnimation, true);
                spine.timeScale = animationSpeed;
            };

            entry.End += _ => { IsAttackPlaying = false; }; // 안전장치
            return;
        }
    }

    private void CacheFloorEffectRoot()
    {
        if (floorEffectRoot != null)
            return;

        Transform direct = transform.Find("FloorEffect");
        if (direct != null)
        {
            floorEffectRoot = direct;
            return;
        }

        Transform[] all = GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].name == "FloorEffect")
            {
                floorEffectRoot = all[i];
                return;
            }
        }
    }

    private void ApplyRankEffectColor()
    {
        CacheFloorEffectRoot();
        if (floorEffectRoot == null)
            return;

        int rankIndex = Mathf.Clamp((int)towerType.towerRank, 0, rankColors.Length - 1);
        Color rankColor = rankColors[rankIndex];

        // FloorEffect 포함 하위(PortalDust, GlowCircle 포함) 전체 적용
        ApplyColorToEffectTree(floorEffectRoot, rankColor);
    }

    private void ApplyColorToEffectTree(Transform root, Color color)
    {
        if (root == null)
            return;

        ParticleSystem[] particles = root.GetComponentsInChildren<ParticleSystem>(true);
        for (int i = 0; i < particles.Length; i++)
        {
            ParticleSystem ps = particles[i];
            var main = ps.main;
            main.startColor = color;
        }

        Renderer[] renderers = root.GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer r = renderers[i];
            if (r == null || r.sharedMaterial == null)
                continue;

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            r.GetPropertyBlock(block);

            bool hasColorProperty = false;

            if (r.sharedMaterial.HasProperty(BaseColorId))
            {
                block.SetColor(BaseColorId, color);
                hasColorProperty = true;
            }

            if (r.sharedMaterial.HasProperty(ColorId))
            {
                block.SetColor(ColorId, color);
                hasColorProperty = true;
            }

            if (r.sharedMaterial.HasProperty(TintColorId))
            {
                block.SetColor(TintColorId, color);
                hasColorProperty = true;
            }

            if (hasColorProperty)
                r.SetPropertyBlock(block);
        }
    }
}

[System.Serializable]
public class TowerSpineSet
{
    public TowerTypes type;
    public TowerRank rank;
    public SkeletonDataAsset skeletonData;
    public string idleAnimation;
    public string attackAnimation;
}


