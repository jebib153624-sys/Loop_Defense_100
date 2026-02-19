using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    public Transform[] wayPoint;
    public int nextIndex = 0;

    public float moveSpeed;
    public float currentSpeed;
    public Vector3 moveDirection;

    private EnemySpawner enemySpawner;

    [SerializeField]
    private int gold;

    [SerializeField]
    private int energy;

    [SerializeField]
    private Vector3 hpBarScreenOffset = new Vector3(0f, 70f, 0f);

    private float baseScaleX;
    private bool isDead = false;
    private EnemyVisual enemyVisual;
    private Collider2D col;

    public Vector3 HpBarScreenOffset => hpBarScreenOffset;

    private void Awake()
    {
        enemyVisual = GetComponent<EnemyVisual>();
        if (enemyVisual == null)
            enemyVisual = GetComponentInChildren<EnemyVisual>();

        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        baseScaleX = Mathf.Abs(transform.localScale.x);
        SetDirectionToNextWaypoint();
        currentSpeed = moveSpeed;
        UpdateFacingByX();
    }

    private void Update()
    {
        if (isDead)
            return;

        transform.position += moveDirection * currentSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, wayPoint[nextIndex].position) < 0.02f * moveSpeed)
        {
            transform.position = wayPoint[nextIndex].position;
            MoveToNextWaypoint();
        }
    }

    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        this.wayPoint = wayPoints;
        this.enemySpawner = enemySpawner;

        transform.position = wayPoint[0].position;
        nextIndex++;
    }

    void MoveToNextWaypoint()
    {
        nextIndex++;

        if (nextIndex == wayPoint.Length)
            nextIndex = 0;

        SetDirectionToNextWaypoint();
    }

    void SetDirectionToNextWaypoint()
    {
        Vector3 dir = (wayPoint[nextIndex].position - transform.position).normalized;
        moveDirection = dir;
        UpdateFacingByX();
    }

    private void UpdateFacingByX()
    {
        if (Mathf.Abs(moveDirection.x) < 0.001f)
            return;

        Vector3 scale = transform.localScale;
        scale.x = moveDirection.x > 0f ? baseScaleX : -baseScaleX;
        transform.localScale = scale;
    }

    public void Ondie()
    {
        if (isDead)
            return;

        isDead = true;
        currentSpeed = 0f;
        moveSpeed = 0f;
        moveDirection = Vector3.zero;

        if (col != null)
            col.enabled = false;

        enemySpawner.NotifyEnemyDead(this, gold, energy);

        if (enemyVisual != null)
            enemyVisual.PlayDeadOnce(() => enemySpawner.FinalizeEnemyDestroy(this));
        else
            enemySpawner.FinalizeEnemyDestroy(this);
    }
}
