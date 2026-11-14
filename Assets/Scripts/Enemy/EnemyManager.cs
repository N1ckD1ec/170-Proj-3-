using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] int linkCount = 0;
    [SerializeField] float linkSize = 0.25f;
    [SerializeField] GameObject linkPrefab;
    GameObject[] links;
    Vector3 targetPosition;
    [SerializeField] float angleConstraint = 60f;

    [SerializeField] float rotationSpeed = 2.5f;
    [SerializeField] float expansionSpeed = 0.025f;
    float currentAngle = 0f;
    float currentRadius = 0f;
    Vector3 centerPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        links = new GameObject[linkCount];
        for (int i = 0; i < linkCount; i++)
        {
            links[i] = Instantiate(linkPrefab, transform.position, transform.rotation, this.transform);
        }

        centerPosition = transform.position;
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentAngle += rotationSpeed * Time.deltaTime;
        currentRadius += expansionSpeed * Time.deltaTime;

        float offsetX = currentRadius * Mathf.Cos(currentAngle);
        float offsetY = currentRadius * Mathf.Sin(currentAngle);
        targetPosition = centerPosition + new Vector3(offsetX, offsetY, 0f);
        Resolve();
    }

    private void Resolve()
    {
        if (linkCount > 0)
        {
            Vector3 headPosition = links[0].transform.position;
            links[0].transform.rotation = Quaternion.Euler(0f, 0f, GetPointDirection(headPosition, targetPosition));
            links[0].transform.position = targetPosition;

            for (int i = 1; i < linkCount; i++)
            {
                Vector3 currentPosition = links[i].transform.position;
                Vector3 aheadPosition = links[i - 1].transform.position;
                float currentAngle = GetPointDirection(currentPosition, aheadPosition);
                float aheadAngle = links[i - 1].transform.eulerAngles.z;
                float constrainedAngle = ConstrainAngle(currentAngle, aheadAngle, angleConstraint);

                float newX = aheadPosition.x - LengthDirX(linkSize, constrainedAngle);
                float newY = aheadPosition.y - LengthDirY(linkSize, constrainedAngle);
                links[i].transform.position = new Vector3(newX, newY, 0f);
                links[i].transform.rotation = Quaternion.Euler(0f, 0f, constrainedAngle);
            }
        }
    }

    private float LengthDirX(float len, float dirDegrees)
    {
        return len * Mathf.Cos(dirDegrees * Mathf.Deg2Rad);
    }

    private float LengthDirY(float len, float dirDegrees)
    {
        return len * Mathf.Sin(dirDegrees * Mathf.Deg2Rad);
    }

    private float GetPointDirection(Vector2 p1, Vector2 p2)
    {
        return Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * Mathf.Rad2Deg;
    }

    private float SimplifyAngle(float angle)
    {
        while (angle >= 360f) angle -= 360f;
        while (angle < 0f) angle += 360f;
        return angle;
    }

    private float RelativeAngleDiff(float angle, float anchor)
    {
        angle = SimplifyAngle(angle + 180f - anchor);
        anchor = 180f;
        return anchor - angle;
    }

    private float ConstrainAngle(float angle, float anchor, float constraint)
    {
        float diff = RelativeAngleDiff(angle, anchor);

        if (Mathf.Abs(diff) <= constraint)
        {
            return SimplifyAngle(angle);
        }

        if (diff > constraint)
        {
            return SimplifyAngle(anchor - constraint);
        }

        return SimplifyAngle(anchor + constraint);
    }
}
