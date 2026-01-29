using UnityEngine;
using System;
using System.Collections;

public class TowerMover : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField]
    public bool IsPickupCooldown = false;
    public bool IsDragging = false;

    private Vector3 originalPosition;
    private Vector3 dragOffset;

    private Collider2D col;



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
        if (IsPickupCooldown) return;
        
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
        if (IsPickupCooldown) return;
        
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
        IsPickupCooldown = true;
        col.enabled = true;
        IsDragging = false;
        Debug.Log("드롭!");
        gameObject.transform.position = originalPosition;
        
        StartCoroutine(PickCooldownRoutine());
    }

    IEnumerator PickCooldownRoutine()
    {
        yield return new WaitForSeconds(0.1f);

        IsPickupCooldown = false;
    }

}