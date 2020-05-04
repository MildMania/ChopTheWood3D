using UnityEngine;

public class DistanceBasedScaleSetter : MonoBehaviour
{
    public RectTransform Center;

    public float MaxDistance;
    public float MinScale;
    public float MaxScale;

    private float _timeFactor = 5.0f;

    private void OnEnable()
    {
        Update();
    }

    void Update()
    {
        // get current distance
        float currentDistance = Vector3.Distance(Center.position, transform.position);
        float scaleFactor = Mathf.Lerp(transform.localScale.x, CalculateScaleFactor(currentDistance), Time.deltaTime * _timeFactor);
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
    }
    
    private float CalculateScaleFactor(float distance)
    {
        if (distance == 0)
            return MaxScale;
        else if (distance > MaxDistance)
            return MinScale;
        else
            return MinScale + Utilities.Normalize(MaxDistance - distance, MaxDistance, 0, MaxScale - MinScale, 0);
    }
}
