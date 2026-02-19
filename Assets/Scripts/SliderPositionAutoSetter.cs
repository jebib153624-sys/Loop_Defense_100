using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distance = new Vector3(0f, 70f, 0f);

    private Transform targetTransform;
    private RectTransform rectTransform;

    public void Setup(Transform target)
    {
        Setup(target, distance);
    }

    public void Setup(Transform target, Vector3 customDistance)
    {
        targetTransform = target;
        rectTransform = GetComponent<RectTransform>();
        distance = customDistance;
    }

    private void LateUpdate()
    {
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        rectTransform.position = screenPosition + distance;
    }
}
