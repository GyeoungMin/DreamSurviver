using System.Collections.Generic;
using UnityEngine;

public class QuadTreeNode
{
    private Rect bounds;
    private List<AABBCollider> objects;
    private QuadTreeNode[] children;
    private int maxObjects = 4;
    private bool divided = false;
    public QuadTreeNode[] Children { get { return children; } }
    public Rect Bounds { get { return bounds; } }
    public int ObjectsCount { get { return objects.Count; } }
    public bool Divided { get { return divided; } }
    public QuadTreeNode(Rect bounds)
    {
        this.bounds = bounds;
        objects = new List<AABBCollider>();
        children = new QuadTreeNode[4];
    }


    public void Insert(AABBCollider collider)
    {
        if (!bounds.Overlaps(collider.rect))
            return;

        if (objects.Count < maxObjects)
        {
            objects.Add(collider);
        }
        else
        {
            if (!divided)
                Subdivide();

            foreach (QuadTreeNode child in children)
            {
                child.Insert(collider);
            }
        }
    }

    public bool Remove(AABBCollider collider)
    {
        if (!bounds.Overlaps(collider.rect))
            return false;

        if (objects.Remove(collider))
            return true;

        if (divided)
        {
            int objectsCount = objects.Count;
            foreach (QuadTreeNode child in children)
            {
                objectsCount += child.objects.Count;
                child.Remove(collider);
            }
            if (objectsCount < maxObjects)
            {
                MergeNodes();
            }
        }
        return true;
    }

    private void MergeNodes()
    {
        foreach (QuadTreeNode child in children)
        {
            objects.AddRange(child.objects);
            child.objects.Clear();
        }
        for (int i = 0; i < children.Length; i++)
        {
            children[i] = null;
        }
    }

    private void Subdivide()
    {
        float halfWidth = bounds.width / 2f;
        float halfHeight = bounds.height / 2f;
        float x = bounds.x;
        float y = bounds.y;

        children[0] = new QuadTreeNode(new Rect(x, y, halfWidth, halfHeight));
        children[1] = new QuadTreeNode(new Rect(x + halfWidth, y, halfWidth, halfHeight));
        children[2] = new QuadTreeNode(new Rect(x, y + halfHeight, halfWidth, halfHeight));
        children[3] = new QuadTreeNode(new Rect(x + halfWidth, y + halfHeight, halfWidth, halfHeight));

        divided = true;
    }

    public List<AABBCollider> Retrieve(Rect area, List<AABBCollider> returnObjects)
    {
        if (!bounds.Overlaps(area))
            return returnObjects;

        returnObjects.AddRange(objects);

        if (divided)
        {
            foreach (QuadTreeNode child in children)
            {
                if (child != null && child.bounds.Overlaps(area))
                {
                    child.Retrieve(area, returnObjects);
                }
            }
        }
        else
        {
            DrawRect(Color.red);
        }

        return returnObjects;
    }

    //public void ObjectsPerNodes()
    //{
    //    if (divided)
    //    {
    //        foreach (QuadTreeNode child in children)
    //        {
    //            child.ObjectsPerNodes();
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log(objects.Count);
    //    }
    //}

    //public void DrawQuadTree()
    //{
    //    DrawRect(Color.white);

    //    if (divided)
    //    {
    //        foreach (QuadTreeNode child in children)
    //        {
    //            child.DrawQuadTree();
    //        }
    //    }
    //}

    void DrawRect(Color color)
    {
        Debug.DrawLine(bounds.min, new Vector2(bounds.min.x, bounds.max.y), color);
        Debug.DrawLine(bounds.min, new Vector2(bounds.max.x, bounds.min.y), color);
        Debug.DrawLine(bounds.max, new Vector2(bounds.min.x, bounds.max.y), color);
        Debug.DrawLine(bounds.max, new Vector2(bounds.max.x, bounds.min.y), color);
    }
}