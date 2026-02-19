using System;
using Spine.Unity;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeleton;
    [SerializeField] private string walkAnimationName = "Slime_Walking";
    [SerializeField] private string deadAnimationName = "Slime_Dead";

    private void Awake()
    {
        if (skeleton == null)
            skeleton = GetComponent<SkeletonAnimation>();

        if (skeleton == null)
            skeleton = GetComponentInChildren<SkeletonAnimation>();
    }

    public void EnemyWalk()
    {
        PlayAnimation(walkAnimationName, true);
    }

    public void PlayDeadOnce(Action onComplete)
    {
        if (skeleton == null || skeleton.AnimationState == null)
        {
            onComplete?.Invoke();
            return;
        }

        if (string.IsNullOrEmpty(deadAnimationName))
        {
            onComplete?.Invoke();
            return;
        }

        if (skeleton.Skeleton?.Data?.FindAnimation(deadAnimationName) == null)
        {
            Debug.LogWarning($"{name}: animation '{deadAnimationName}' not found in {skeleton.skeletonDataAsset?.name}.");
            onComplete?.Invoke();
            return;
        }

        bool completed = false;
        var entry = skeleton.AnimationState.SetAnimation(0, deadAnimationName, false);

        entry.Complete += _ =>
        {
            if (completed) return;
            completed = true;
            onComplete?.Invoke();
        };

        entry.End += _ =>
        {
            if (completed) return;
            completed = true;
            onComplete?.Invoke();
        };
    }

    private void PlayAnimation(string animationName, bool loop)
    {
        if (skeleton == null || skeleton.AnimationState == null) return;
        if (string.IsNullOrEmpty(animationName)) return;

        if (skeleton.Skeleton?.Data?.FindAnimation(animationName) == null)
        {
            Debug.LogWarning($"{name}: animation '{animationName}' not found in {skeleton.skeletonDataAsset?.name}.");
            return;
        }

        skeleton.AnimationState.SetAnimation(0, animationName, loop);
    }
}
