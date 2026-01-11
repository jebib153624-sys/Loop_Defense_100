using UnityEngine;

public class TowerDrag : MonoBehaviour
{

    private Camera mainCamera;
    private bool isDragging;
    private Vector3 offset;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();

        // 클릭 시작
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                offset = transform.position - mouseWorldPos;
            }
        }

        // 드래그 중
        if (Input.GetMouseButton(0) && isDragging)
        {
            transform.position = mouseWorldPos + offset;
        }

        // 클릭 종료
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        return pos;
    }
}
