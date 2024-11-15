using UnityEngine;

public class MyCircleCollider : MyCollider
{
    public float radius = 1f;
    private Vector2 Size { get { return new Vector2(radius, radius) * range; } }
    private float Radius { get { return radius * range; } }
    public override Rect Rect { get { return new Rect(Center - Size, Size); } }
    public override Vector2 GetCenter()
    {
        return position;
    }

    public override float GetRadius()
    {
        return Radius;
    }

    protected override void DrawGizmo(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(position, Radius);
    }
    protected override bool CalculateCollision(MyCollider other)
    {
        return (other as MyBoxCollider).IsColliding(this);
    }
}
