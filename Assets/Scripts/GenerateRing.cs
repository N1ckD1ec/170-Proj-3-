
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class RingGenerator : MonoBehaviour
{
    public float radius = 4f;
    public int segments = 360;
    public float lineWidth = 0.05f;
    [SerializeField] private Color ringColor = Color.cyan;
    private LineRenderer line;
    private EdgeCollider2D edgeCollider;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        int pointCount = Mathf.Max(16, segments);

        line.useWorldSpace = false;
        line.loop = true;
        line.positionCount = pointCount;

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        line.startColor = ringColor;
        line.endColor = ringColor;

        Vector2[] colliderPoints = new Vector2[pointCount + 1];

        for (int i = 0; i < pointCount; i++)
        {
            float angle = i * Mathf.PI * 2f / pointCount;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Vector2 point = new Vector2(x, y);

            line.SetPosition(i,new Vector3(x, y, 0f));
            colliderPoints[i] = point;
        }
        colliderPoints[pointCount] = colliderPoints[0];
        edgeCollider.points = colliderPoints;
        edgeCollider.isTrigger = false;
    }
}
