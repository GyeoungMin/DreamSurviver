using System.Collections.Generic;
using UnityEngine;

public enum ColliderType
{
    None = -1, Player, Monster, Skill, Item
}
//public delegate void OnCollisionEvent(MyCollider other);
public abstract class MyCollider : MonoBehaviour
{
    //public event OnCollisionEvent OnCollisionEnterEvent;
    public ColliderType colliderType = ColliderType.None;
    public Color Color = Color.white;
    public bool isTrigger;
    public Vector2 center;
    private bool FlipX
    {
        get
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                return spriteRenderer.flipX;
            }
            return false;
        }
    }

    [HideInInspector]
    public float range = 1.0f;

    // ȸ���� �߽��� ���
    protected Vector2 Center { get { return RotatePoint(center, Vector2.zero, transform.rotation.eulerAngles.z); } }

    public Vector2 position => (Vector2)transform.position + (FlipX ? new Vector2(-Center.x, Center.y) : Center);

    public abstract Rect Rect { get; }

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    protected virtual void Awake()
    {
        if (colliderType == ColliderType.None)
        {
            Debug.LogError($"{gameObject.name} has ColliderType.None and will not be used in collision detection.", gameObject);
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    public void OnCulling()
    {
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        if (animator != null) animator.enabled = true;
    }

    public void UnCulling()
    {
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (animator != null) animator.enabled = false;
    }

    private void Update()
    {
        if (!isTrigger)
        {
            ApplySeparation();
        }
    }

    //private int frameCount = 0;
    //private int frameInterval = 2;
    //private void FixedUpdate()
    //{
    //    frameCount++;
    //    if (frameCount >= frameInterval)
    //    {
    //        frameCount = 0;
    //        if (!isTrigger) ApplySeparation();
    //        else CheckForCollisions();
    //    }
    //}
    //private void CheckForCollisions()
    //{
    //    MyCollider[] colliders = FindObjectsOfType<MyCollider>();
    //    foreach (MyCollider other in colliders)
    //    {
    //        if (other != this && IsColliding(other))
    //        {
    //            OnCollisionEnterEvent?.Invoke(other);
    //        }
    //    }
    //}

    public abstract Vector2 GetCenter();
    public abstract float GetRadius();

    public virtual bool IsColliding(MyCollider other)
    {
        if (IsCollisionTypeCompatible(other))
        {
            return false;
        }

        if (other is MyBoxCollider && (transform.eulerAngles.z % 360 != 0 || other.transform.eulerAngles.z % 360 != 0))
        {
            CalculateCollision(other);
        }

        float distance = Vector2.Distance(GetCenter(), other.GetCenter());
        return distance < (GetRadius() + other.GetRadius());
    }

    private bool IsCollisionTypeCompatible(MyCollider other)
    {
        if (colliderType == other.colliderType) return true;
        if (colliderType == ColliderType.Player && other.colliderType == ColliderType.Skill)
        {
            return true;
        }
        return false;
    }

    protected abstract bool CalculateCollision(MyCollider other);

    protected virtual void ApplySeparation()
    {
        //MyCollider[] colliders = FindObjectsOfType<MyCollider>();

        List<MyCollider> colliders = QuadTreeManager.Instance.Retrieve();
        foreach (var other in colliders)
        {
            if (other != this)
            {
                ResolveCollision(other);
            }
        }
    }

    protected void ResolveCollision(MyCollider other)
    {
        Vector2 direction = GetCenter() - other.GetCenter();
        float distance = direction.magnitude;
        float overlap = GetRadius() + other.GetRadius() - distance;

        if (overlap > 0)
        {
            direction.Normalize();

            transform.position += (Vector3)(direction * overlap * 0.5f);
            other.transform.position -= (Vector3)(direction * overlap * 0.5f);
        }
    }

    protected void OnDrawGizmos()
    {
        DrawGizmo(Color);
    }

    protected void OnDrawGizmosSelected()
    {
        DrawGizmo(Color.green);
    }

    protected abstract void DrawGizmo(Color color);

    protected Vector2 RotatePoint(Vector2 point, Vector2 pivot, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        Vector2 rotated = new Vector2(
            cos * (point.x - pivot.x) - sin * (point.y - pivot.y) + pivot.x,
            sin * (point.x - pivot.x) + cos * (point.y - pivot.y) + pivot.y
        );
        return rotated;
    }
}
