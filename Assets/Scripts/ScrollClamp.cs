using UnityEngine;
using UnityEngine.UI;

public class ScrollClamp : MonoBehaviour
{
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;

    private void Update()
    {
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, minX, maxX);
        transform.position = p;
    }
}
