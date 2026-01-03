using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos =
                mainCamera.ScreenToWorldPoint(Input.mousePosition);

            mouseWorldPos.z = 0f;

            spawnTower(mouseWorldPos);
        }
    }

    public void spawnTower(Vector3 position)
    {
        Instantiate(towerPrefab, position, Quaternion.identity);
    }
}
