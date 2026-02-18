using Spine.Unity;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeleton;
    [SerializeField] private string walkAnimationName = "Slime_Walking";
    [SerializeField] private string attackAnimationName = "Attack";

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

    public void PlayAttack()
    {
        PlayAnimation(attackAnimationName, false);
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