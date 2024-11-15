using System.Collections;
using UnityEditor;
using UnityEngine;

// AABB �浹 ó���� ���� Ŭ����
public class AABBCollider : MonoBehaviour
{
    public ColliderType colliderType = ColliderType.None;
    public Vector2 size;
    public Vector2 center;
    [HideInInspector]
    public Vector2 min;
    [HideInInspector]
    public Vector2 max;
    public Rect rect { get { return new Rect((Vector2)transform.position + center - size / 2, size); } }
    private void Awake()
    {
        if (colliderType == ColliderType.None)
        {
            Debug.LogError($"{gameObject.name} has ColliderType.None and will not be used in QuadTree.", gameObject);
        }
    }

    private void Update()
    {
        ApplySeparation();
    }

    // AABB �浹 �˻� �Լ�
    public bool IsColliding(AABBCollider other)
    {
        return colliderType != other.colliderType && rect.Overlaps(other.rect);
    }
    private void ApplySeparation()
    {
        // ���� ���� ��� AABBCollider�� ������
        AABBCollider[] colliders = FindObjectsOfType<AABBCollider>();
        foreach (var other in colliders)
        {
            if (other != this && other.colliderType != ColliderType.Skill && rect.Overlaps(other.rect))
            {
                ResolveCollision(other);
            }
        }
    }

    private void ResolveCollision(AABBCollider other)
    {
        // �� �浹ü�� Rect�� ������
        Rect otherRect = other.rect;

        // �� �浹ü�� ��ġ�� ������ ���
        float overlapX = Mathf.Min(rect.xMax, otherRect.xMax) - Mathf.Max(rect.xMin, otherRect.xMin);
        float overlapY = Mathf.Min(rect.yMax, otherRect.yMax) - Mathf.Max(rect.yMin, otherRect.yMin);

        // X �������� �� ���� ������ ���, Y �������� �и�
        if (overlapX > overlapY)
        {
            float separationY = overlapY * 0.5f;

            if (rect.center.y > otherRect.center.y)
            {
                transform.position += new Vector3(0, separationY);
                other.transform.position -= new Vector3(0, separationY);
            }
            else
            {
                transform.position -= new Vector3(0, separationY);
                other.transform.position += new Vector3(0, separationY);
            }
        }
        // Y �������� �� ���� ������ ���, X �������� �и�
        else
        {
            float separationX = overlapX * 0.5f;

            if (rect.center.x > otherRect.center.x)
            {
                transform.position += new Vector3(separationX, 0);
                other.transform.position -= new Vector3(separationX, 0);
            }
            else
            {
                transform.position -= new Vector3(separationX, 0);
                other.transform.position += new Vector3(separationX, 0);
            }
        }
    }

    private void OnDrawGizmos()
    {
        DrawGizmo(Color.white);
    }
    private void OnDrawGizmosSelected()
    {
        DrawGizmo(Color.green);
    }

    private void DrawGizmo(Color color)
    {
        Vector2 min = rect.min;
        Vector2 max = rect.max;

        Gizmos.color = color;
        Gizmos.DrawLine(new Vector3(min.x, min.y), new Vector3(max.x, min.y));
        Gizmos.DrawLine(new Vector3(min.x, min.y), new Vector3(min.x, max.y));
        Gizmos.DrawLine(new Vector3(max.x, min.y), new Vector3(max.x, max.y));
        Gizmos.DrawLine(new Vector3(min.x, max.y), new Vector3(max.x, max.y));
    }

    public void DrawRect()
    {
        Vector2 min = rect.min;
        Vector2 max = rect.max;

        Color color = Color.red;
        Debug.DrawLine(min, new Vector2(max.x, min.y), color);
        Debug.DrawLine(min, new Vector2(min.x, max.y), color);
        Debug.DrawLine(max, new Vector2(max.x, min.y), color);
        Debug.DrawLine(max, new Vector2(min.x, max.y), color);
    }
}
