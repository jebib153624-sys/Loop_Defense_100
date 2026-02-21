using System.Collections;
using UnityEngine;
using Spine.Unity;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] private float maxHP;
    private float currentHP;
    private bool isDie = false;

    private Enemy enemy;
    private SpriteRenderer spriteRenderer;
    private SkeletonAnimation skeletonAnimation;
    private Coroutine hitFlashRoutine;

    private Color defaultSpriteColor = Color.white;
    private Color defaultSkeletonColor = Color.white;
    private bool hasSpriteDefaultColor;
    private bool hasSkeletonDefaultColor;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
        enemy = GetComponent<Enemy>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();

        CacheDefaultColorsIfNeeded();
    }

    public void ApplyWaveHpMultiplier(float multiplier)
    {
        multiplier = Mathf.Max(1f, multiplier);
        maxHP *= multiplier;
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        if (isDie) return;

        currentHP -= damage;

        CacheDefaultColorsIfNeeded();
        ResetHitFlash();
        hitFlashRoutine = StartCoroutine(HitFlashAnimation());

        if (currentHP <= 0)
        {
            isDie = true;
            enemy.Ondie();
        }
    }

    private IEnumerator HitFlashAnimation()
    {
        if (skeletonAnimation != null && skeletonAnimation.Skeleton != null)
        {
            skeletonAnimation.Skeleton.SetColor(new Color(1f, 0.65f, 0.65f, 1f));
            yield return new WaitForSeconds(0.06f);
            skeletonAnimation.Skeleton.SetColor(defaultSkeletonColor);
            hitFlashRoutine = null;
            yield break;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 0.65f, 0.65f, 1f);
            yield return new WaitForSeconds(0.06f);
            spriteRenderer.color = defaultSpriteColor;
        }

        hitFlashRoutine = null;
    }

    private void CacheDefaultColorsIfNeeded()
    {
        if (!hasSpriteDefaultColor && spriteRenderer != null)
        {
            defaultSpriteColor = spriteRenderer.color;
            hasSpriteDefaultColor = true;
        }

        if (!hasSkeletonDefaultColor && skeletonAnimation != null && skeletonAnimation.Skeleton != null)
        {
            defaultSkeletonColor = skeletonAnimation.Skeleton.GetColor();
            hasSkeletonDefaultColor = true;
        }
    }

    private void ResetHitFlash()
    {
        if (hitFlashRoutine != null)
        {
            StopCoroutine(hitFlashRoutine);
            hitFlashRoutine = null;
        }

        if (skeletonAnimation != null && skeletonAnimation.Skeleton != null)
        {
            skeletonAnimation.Skeleton.SetColor(defaultSkeletonColor);
            return;
        }

        if (spriteRenderer != null)
            spriteRenderer.color = defaultSpriteColor;
    }
}
