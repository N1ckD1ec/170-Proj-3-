
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RingGenerator : MonoBehaviour
{
    public float radius = 4f;
    public int segments = 360;
    public float lineWidth = 0.05f;

    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();

        line.useWorldSpace = false;
        line.loop = true;
        line.positionCount = segments;

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            line.SetPosition(i, new Vector3(x, y, 0f));
        }
    }
}
