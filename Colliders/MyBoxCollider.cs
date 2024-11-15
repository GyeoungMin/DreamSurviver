using System.Collections.Generic;
using UnityEngine;

public class MyBoxCollider : MyCollider
{
    public Vector2 size = new Vector2(1, 1);
    private Vector2 Size { get { return size * range; } }
    private Vector2 HalfSize { get { return Size / 2; } }
    private Rect rect { get { return new Rect(position - Size / 2, Size); } }

    public override Rect Rect { get { return rect; } }

    public override Vector2 GetCenter()
    {
        return rect.center;
    }

    public override float GetRadius()
    {
        float length = Size.x < Size.y ? size.x : size.y;
        return length / 2;
    }
    public Vector2[] GetCorners()
    {
        Vector2 halfSize = Size / 2;
        Vector2[] corners = new Vector2[4];
        corners[0] = RotatePoint(-halfSize, Vector2.zero, transform.eulerAngles.z) + position;
        corners[1] = RotatePoint(new Vector2(-halfSize.x, halfSize.y), Vector2.zero, transform.eulerAngles.z) + position;
        corners[2] = RotatePoint(halfSize, Vector2.zero, transform.eulerAngles.z) + position;
        corners[3] = RotatePoint(new Vector2(halfSize.x, -halfSize.y), Vector2.zero, transform.eulerAngles.z) + position;
        return corners;
    }

    protected override bool CalculateCollision(MyCollider other)
    {
        Vector2[] myCorners = GetCorners();
        Vector2 otherCenter = other.GetCenter();

        foreach (var corner in myCorners)
        {
            if (IsPointInOBB(otherCenter, myCorners))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsPointInOBB(Vector2 point, Vector2[] corners)
    {
        Vector2 AB = corners[1] - corners[0];
        Vector2 AM = point - corners[0];
        Vector2 BC = corners[2] - corners[1];
        Vector2 BM = point - corners[1];

        float AB_AB = Vector2.Dot(AB, AB);
        float AB_AM = Vector2.Dot(AB, AM);
        float BC_BC = Vector2.Dot(BC, BC);
        float BC_BM = Vector2.Dot(BC, BM);

        return 0 <= AB_AM && AB_AM <= AB_AB && 0 <= BC_BM && BC_BM <= BC_BC;
    }

    protected override void DrawGizmo(Color color)
    {
        Gizmos.color = color;

        Vector2[] corners = GetCorners();
        for (int i = 0; i < corners.Length; i++)
        {
            Gizmos.DrawLine(corners[i], corners[(i + 1) % corners.Length]);
        }
    }
}
