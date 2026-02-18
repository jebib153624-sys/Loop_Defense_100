using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class TowerVisual : MonoBehaviour
{
    [SerializeField] private TowerType towerType;
    public SkeletonAnimation spine;
    [SerializeField] private TowerSpineSet[] spineSets;

    private SkeletonDataAsset currentSkeleton; // ★ 캐시

    private void Awake()
    {
        if (!towerType)
            towerType = GetComponentInParent<TowerType>();

        if (!spine)
            spine = GetComponentInChildren<SkeletonAnimation>();
    }
    private void Update()
    {
        //UpdateVisual();
    }
    public void UpdateVisual()
    {
        foreach (var set in spineSets)
        {
            if (set.type == towerType.towerType && set.rank == towerType.towerRank)
            {
                // 1. Skeleton이 바뀌었을 때만 교체
                if (currentSkeleton != set.skeletonData)
                {
                    currentSkeleton = set.skeletonData;
                    spine.skeletonDataAsset = currentSkeleton;
                    spine.Initialize(true);
                }

                // 2. 애니메이션은 항상 갱신
                spine.AnimationState.SetAnimation(0, set.idleAnimation, true);
                return;
            }
        }
    }
    public bool IsAttackPlaying { get; private set; }
    public void PlayAttackOnce(System.Action onHit)
    {
        if (spine == null || spine.AnimationState == null) return;
        if (IsAttackPlaying) return; // 이 줄이 핵심

        foreach (var set in spineSets)
        {
            if (set.type != towerType.towerType || set.rank != towerType.towerRank) continue;

            IsAttackPlaying = true;
            var entry = spine.AnimationState.SetAnimation(0, set.attackAnimation, false);

            bool hitCalled = false;
            entry.Event += (trackEntry, e) =>
            {
                if (!hitCalled && e.Data.Name == "Attack_Hit")
                {
                    hitCalled = true;
                    onHit?.Invoke();
                }
            };

            entry.Complete += _ =>
            {
                IsAttackPlaying = false;
                spine.AnimationState.SetAnimation(0, set.idleAnimation, true);
            };

            entry.End += _ => { IsAttackPlaying = false; }; // 안전장치
            return;
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