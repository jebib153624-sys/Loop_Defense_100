using UnityEngine;
using System;
using System.Collections;

public class TowerMover : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField]
    public bool IsDragging = false;

    private Vector3 originalPosition;
    private Vector3 dragOffset;

    private Collider2D col;

    public Vector3 clickJudge;

    void Awake()
    {
        mainCam = Camera.main;
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    // ---------------- PC 테스트용 ----------------
    void HandleMouse()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            TryPick(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Drag(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Drop();
        }
    }

    // ---------------- 모바일 실제용 ----------------
    void HandleTouch()
    {
        
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            TryPick(touch.position);
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            Drag(touch.position);
        }
        else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            Drop();
        }
    }

    // ---------------- 공통 로직 ----------------

    void TryPick(Vector2 screenPos)
    {
        Vector2 worldPos = mainCam.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            IsDragging = true;
            originalPosition = transform.position;
            dragOffset = transform.position - (Vector3)worldPos;

            // 이동 중 충돌 비활성 (중요)
            col.enabled = false;
            Debug.Log("잡음!!");
        }

    }

    void Drag(Vector2 screenPos)
    {
        if (!IsDragging) return;
        
        Vector3 worldPos = mainCam.ScreenToWorldPoint(screenPos);
        worldPos.z = transform.position.z;

        transform.position = worldPos + dragOffset;
    }

    void Drop()
    {
        if (!IsDragging) return;

        col.enabled = true;
        Debug.Log("드롭!");

        // 1. 주변 타워 탐색 (반경 0.5f는 타워 크기에 맞춰 조절하세요)
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + clickJudge, 0.1f);

        TowerMover bestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            // 나 자신은 제외
            if (hit.gameObject == gameObject) continue;

            // TowerMover 컴포넌트가 있는지 확인 (타워인지 확인)
            TowerMover otherMover = hit.GetComponent<TowerMover>();
            if (otherMover != null)
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    bestTarget = otherMover;
                }
            }
        }

        // 2. 가장 가까운 타워가 있다면 합성 시도
        if (bestTarget != null)
        {
            UpgradeJudge judge = GetComponent<UpgradeJudge>();
            // 합성 성공 여부 반환 (타입/랭크가 맞는지)
            bool success = judge.TryUpgrade(bestTarget.gameObject);

            if (success)
            {
                IsDragging = false; // 합성 성공 시 복귀 방지
                return;
            }
        }

        // 3. 합성 실패 시에만 원래 위치로 복귀
        StartCoroutine(PickCooldownRoutine());
    }

    IEnumerator PickCooldownRoutine()
    {
        yield return new WaitForSeconds(0.01f);
        IsDragging = false;
        gameObject.transform.position = originalPosition;
    }
    private void OnDrawGizmosSelected()
    {
        // Gizmos 색상을 녹색으로 설정
        Gizmos.color = Color.green;
        // OverlapCircle과 동일한 위치와 크기로 원을 그림
        Gizmos.DrawWireSphere(transform.position + clickJudge, 0.15f);
    }
}