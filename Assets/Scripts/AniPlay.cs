using Spine.Unity;
using UnityEngine;

public class AniPlay : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation spine;
    void Start()
    {
        spine = GetComponentInChildren<SkeletonAnimation>();
        spine.AnimationState.SetAnimation(0, "Warrior1_idle", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
