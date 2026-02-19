using System.Collections;
using UnityEngine;
using Spine.Unity;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] private float maxHP;
    private float currentHP;
    private bool isDie = false;

    private Enemy enemy;
    private SpriteRenderer spriteRenderer;         // 스프라이트용(호환)
    private SkeletonAnimation skeletonAnimation;    // 스파인용

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
        enemy = GetComponent<Enemy>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>(); // 스파인이 자식일 수 있음
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

        StopCoroutine(HitFlashAnimation());
        StartCoroutine(HitFlashAnimation());

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
            // 기본색 저장
            var baseColor = skeletonAnimation.Skeleton.GetColor();

            // 피격 틴트(살짝 빨강)
            skeletonAnimation.Skeleton.SetColor(new Color(1f, 0.65f, 0.65f, 1f));
            yield return new WaitForSeconds(0.06f);

            // 원복
            skeletonAnimation.Skeleton.SetColor(baseColor);
            yield break;
        }

        if (spriteRenderer != null)
        {
            Color baseColor = spriteRenderer.color;
            spriteRenderer.color = new Color(1f, 0.65f, 0.65f, 1f);
            yield return new WaitForSeconds(0.06f);
            spriteRenderer.color = baseColor;
        }
    }
}
