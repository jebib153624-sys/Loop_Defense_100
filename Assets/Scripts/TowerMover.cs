using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerMover : MonoBehaviour
{
    private Camera mainCam;
    private TowerType towerType;

    [Header("Pick Zone")]
    [SerializeField] private Transform basicZoneBlue;
    [SerializeField] private Vector3 warriorZoneScale = new Vector3(7.5f, 7.5f, 7.5f);
    [SerializeField] private Vector3 freezeZoneScale = new Vector3(11f, 11f, 11f);

    [SerializeField]
    public bool IsDragging = false;
    public GameObject synthesisButton; // 합성 버튼 오브젝트

    public bool isClicking;

    private Vector3 originalPosition;
    private Vector3 dragOffset;

    private Collider2D col;

    public float searchRadius;
    public Vector3 offsetY;


    private SpawnPosition currentSlot; // 현재 점유 중인 슬롯
    public SpawnPosition CurrentSlot => currentSlot;

    void Awake()
    {
        mainCam = Camera.main;
        towerType = GetComponent<TowerType>();

        if (basicZoneBlue == null)
            basicZoneBlue = FindChildRecursive(transform, "BasicZoneBlue");

        col = GetComponent<Collider2D>();
        SetClicking(false);
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
        // Pick만 UI 위에서 막기
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryPick(Input.mousePosition);
            }
        }

        // 드래그 & 드롭은 항상 허용
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
            SetClicking(true);
            Debug.Log(isClicking);
            originalPosition = transform.position;
            dragOffset = transform.position - (Vector3)worldPos;
            // 이동 중 충돌 비활성 (중요)
            col.enabled = false;
        }
        else
        {
            SetClicking(false);
        }

    }

    void Drag(Vector2 screenPos)
    {
        //synthesisButton.SetActive(false); // 합성 버튼 숨기기
        if (!IsDragging) return;

        Vector3 worldPos = mainCam.ScreenToWorldPoint(screenPos);
        worldPos.z = transform.position.z;

        transform.position = worldPos + dragOffset;
    }

    private void SetCurrentSlot(SpawnPosition nextSlot)
    {
        if (currentSlot == nextSlot)
        {
            SyncWeaponSpawnPosition();
            return;
        }

        if (currentSlot != null)
            currentSlot.IsBuildTower = 0;

        currentSlot = nextSlot;

        if (currentSlot != null)
            currentSlot.IsBuildTower = 1;

        SyncWeaponSpawnPosition();
    }

    private void SyncWeaponSpawnPosition()
    {
        TowerWeapon weapon = GetComponent<TowerWeapon>();
        if (weapon != null)
            weapon.spawnPosition = currentSlot;
    }

    public void ReleaseCurrentSlot()
    {
        if (currentSlot != null)
            currentSlot.IsBuildTower = 0;

        currentSlot = null;
        SyncWeaponSpawnPosition();
    }

    [SerializeField] private LayerMask slotLayerMask;
    void Drop()
    {
        if (!IsDragging) return;

        col.enabled = true;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + offsetY, searchRadius);

        SpawnPosition closestSlot = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            SpawnPosition sp = hit.GetComponent<SpawnPosition>();
            if (sp == null) continue;

            if (sp.IsBuildTower != 0) continue; // 이미 타워가 있는 슬롯은 건너뜁니다.

            float dist = Vector2.Distance(transform.position, hit.transform.position);

            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestSlot = sp;
            }
        }

        if (closestSlot != null)
        {
            transform.position = closestSlot.transform.position;
            SetCurrentSlot(closestSlot);
            SetClicking(false);
        }
        else
        {
            transform.position = originalPosition;
        }

        // 버튼 표시 상태와 별개로, 범위 가이드는 드롭 시 항상 숨김
        SetPickZoneVisible(false);
        IsDragging = false;
    }
    public void GetSpawnPosition(SpawnPosition sp) // 현재 점유 중인 슬롯 정보 받기
    {
        SetCurrentSlot(sp);
    }

    public void SetClicking(bool value)
    {
        isClicking = value;
        if (synthesisButton != null)
            synthesisButton.SetActive(value);

        SetPickZoneVisible(value);
    }

    private void SetPickZoneVisible(bool visible)
    {
        if (basicZoneBlue == null)
            return;

        if (visible)
            UpdatePickZoneScaleByType();

        basicZoneBlue.gameObject.SetActive(visible);
    }

    private void UpdatePickZoneScaleByType()
    {
        if (basicZoneBlue == null)
            return;

        if (towerType == null)
            towerType = GetComponent<TowerType>();

        if (towerType == null)
            return;

        basicZoneBlue.localScale = (towerType.towerType == TowerTypes.WarriorTower)
            ? warriorZoneScale
            : freezeZoneScale;
    }

    private Transform FindChildRecursive(Transform parent, string childName)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.name == childName)
                return child;

            Transform found = FindChildRecursive(child, childName);
            if (found != null)
                return found;
        }

        return null;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offsetY, searchRadius);
    }

    private void OnDestroy()
    {
        ReleaseCurrentSlot();
    }
}
