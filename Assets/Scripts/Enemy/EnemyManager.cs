using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] int linkCount = 0;
    [SerializeField] float linkSize = 0.25f;
    [SerializeField] GameObject linkPrefab;
    List<GameObject> links = new List<GameObject>();
    Vector3 targetPosition;
    [SerializeField] float angleConstraint = 60f;
    [SerializeField] float rotationSpeed = 2.5f;
    [SerializeField] float expansionSpeed = 0.025f;
    [SerializeField] Sprite headSprite;

    float currentAngle = 0f;
    float currentRadius = 0f;
    Vector3 centerPosition;

    bool initializedLinks = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!initializedLinks)
        {
            for (int i = 0; i < linkCount; i++)
            {
                GameObject enemyLinkObj = Instantiate(linkPrefab, transform.position, transform.rotation, this.transform);
                EnemyLink enemyLink = enemyLinkObj.GetComponent<EnemyLink>();
                links.Add(enemyLinkObj);
                enemyLink.origin = this;
            }

            centerPosition = transform.position;
            targetPosition = transform.position;

            initializedLinks = true;
            ApplyHeadSprite();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (links.Count <= 0)
        {
            Destroy(gameObject);
            return;
        }

        CheckIfSplit();

        if (links.Count > 0)
        {
            currentAngle += rotationSpeed * Time.deltaTime;
            currentRadius += expansionSpeed * Time.deltaTime;

            float offsetX = currentRadius * Mathf.Cos(currentAngle);
            float offsetY = currentRadius * Mathf.Sin(currentAngle);
            targetPosition = centerPosition + new Vector3(offsetX, offsetY, 0f);
            Resolve();
        }
    }

    private void Resolve()
    {
        links.RemoveAll(item => item == null);
        if (links.Count > 0)
        {
            Vector3 headPosition = links[0].transform.position;
            links[0].transform.rotation = Quaternion.Euler(0f, 0f, GetPointDirection(headPosition, targetPosition));
            links[0].transform.position = targetPosition;

            for (int i = 1; i < links.Count; i++)
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

    public bool IsHead(EnemyLink link)
    {
        return (links[0] == link);
    }

    public void DestroyEnemyChain()
    {
        foreach (var link in links)
        {
            link.GetComponent<EnemyLink>().touchedRing = true;
            Destroy(link);
        }
        Destroy(gameObject);
    }

    public void InitializeSplitChain(List<GameObject> tailLinks, Vector3 center, float rotSpeed, float expSpeed, float lSize, Sprite tempHeadSprite)
    {
        links = tailLinks;
        foreach (var link in links)
        {
            link.transform.SetParent(this.transform);
            link.GetComponent<EnemyLink>().origin = this;
        }
        centerPosition = center;
        rotationSpeed = rotSpeed;
        expansionSpeed = expSpeed;
        linkSize = lSize;
        headSprite = tempHeadSprite;

        Vector3 newHeadPos = links[0].transform.position;
        Vector3 offset = newHeadPos - centerPosition;
        currentAngle = Mathf.Atan2(offset.y, offset.x);
        currentRadius = offset.magnitude;

        initializedLinks = true;
        ApplyHeadSprite();
    }

    private void CheckIfSplit()
    {
        for (int i = 0; i < links.Count; i++)
        {
            if (links[i] == null && (i == 0 || i == links.Count - 1))
            {
                links.RemoveAt(i);
                ApplyHeadSprite();
                break;
            }
            if (links[i] == null)
            {
                List<GameObject> tailLinks = new List<GameObject>();
                for (int j = i + 1; j < links.Count; j++)
                {
                    if (links[j] != null)
                    {
                        tailLinks.Add(links[j]);
                    }
                }

                if (tailLinks.Count > 0)
                {
                    tailLinks.Reverse();
                    GameObject splitEnemyManagerObj = new GameObject("EnemyManager");
                    EnemyManager splitEnemyManager = splitEnemyManagerObj.AddComponent<EnemyManager>();
                    splitEnemyManager.InitializeSplitChain(tailLinks, centerPosition, (i == 0) ? rotationSpeed : -rotationSpeed, expansionSpeed, linkSize, headSprite);
                }

                links.RemoveRange(i, links.Count - i);
                break;
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
    private void ApplyHeadSprite()
    {
        if (headSprite == null || links.Count == 0)
            return;
        SpriteRenderer renderer = links[0].GetComponentInChildren<SpriteRenderer>(true);
        if (renderer != null)
            renderer.sprite = headSprite;
    }
}
