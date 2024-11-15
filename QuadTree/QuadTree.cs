using System.Collections.Generic;
using UnityEngine;

public class QuadTree
{
    private int maxObjectsPerNode = 8; // 각 노드당 최대 객체 수
    private int maxLevels = 5;         // 쿼드트리 최대 깊이

    private int level;
    private List<MyCollider> objects;
    private Rect bounds;
    private QuadTree[] nodes;
    private bool divided = false;
    public Rect Bounds { get { return bounds; } }
    public QuadTree[] Nodes { get { return nodes; } }
    public bool Divided { get { return divided; } }
    public QuadTree(Rect bounds, int level = 0)
    {
        this.level = level;
        this.bounds = bounds;
        objects = new List<MyCollider>();
        nodes = new QuadTree[4];
    }

    public void Clear()
    {
        objects.Clear();
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] != null)
            {
                nodes[i].Clear();
                nodes[i] = null;
            }
        }
    }

    private void Split()
    {
        float subWidth = bounds.width / 2;
        float subHeight = bounds.height / 2;
        float x = bounds.x;
        float y = bounds.y;

        nodes[0] = new QuadTree(new Rect(x + subWidth, y, subWidth, subHeight), level + 1);
        nodes[1] = new QuadTree(new Rect(x, y, subWidth, subHeight), level + 1);
        nodes[2] = new QuadTree(new Rect(x, y + subHeight, subWidth, subHeight), level + 1);
        nodes[3] = new QuadTree(new Rect(x + subWidth, y + subHeight, subWidth, subHeight), level + 1);

        divided = true;
    }

    public bool Insert(MyCollider collider)
    {
        // If there are subnodes, try to add the object to the corresponding node
        if (nodes[0] != null)
        {
            foreach (var node in nodes)
            {
                if (node.bounds.Overlaps(collider.Rect))
                {
                    return node.Insert(collider);
                }
            }
        }

        // Add the object to the current node
        objects.Add(collider);

        // If the number of objects exceeds the maximum and the current level is not the maximum, split the node
        if (objects.Count > maxObjectsPerNode && level < maxLevels)
        {
            if (nodes[0] == null)
            {
                Split();
            }

            // Redistribute objects to the appropriate subnodes
            int i = 0;
            while (i < objects.Count)
            {
                bool inserted = false;
                foreach (var node in nodes)
                {
                    if (node.bounds.Overlaps(objects[i].Rect))
                    {
                        node.Insert(objects[i]);
                        objects.RemoveAt(i);
                        inserted = true;
                        break;
                    }
                }

                if (!inserted)
                {
                    i++;
                }
            }
        }
        return true;
        //return bounds;
    }

    public bool Remove(MyCollider collider)
    {
        // 현재 노드에서 collider 제거 시도
        bool removed = objects.Remove(collider);

        // 하위 노드에서 collider 제거 시도
        if (nodes[0] != null)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null && nodes[i].Remove(collider))
                {
                    removed = true;
                }
            }
        }

        // 노드 병합 시도
        if (removed)
        {
            MergeNodes();
        }

        return removed;
    }

    private void MergeNodes()
    {
        if (nodes[0] == null) return;

        // 모든 하위 노드의 객체를 현재 노드로 병합
        List<MyCollider> allObjects = new List<MyCollider>(objects);
        bool canMerge = true;

        foreach (var node in nodes)
        {
            if (node == null) continue;

            if (node.nodes[0] != null)
            {
                canMerge = false;
                break;
            }

            allObjects.AddRange(node.objects);
        }

        if (canMerge && allObjects.Count <= maxObjectsPerNode)
        {
            objects = allObjects;
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = null;
            }
        }
    }

    public List<MyCollider> RetrieveObjects(Rect area, bool drawRect = false)
    {
        List<MyCollider> retrievedObjects = new List<MyCollider>();
        RetrieveObjectsInArea(area, retrievedObjects, drawRect);
        return retrievedObjects;
    }

    private void RetrieveObjectsInArea(Rect area, List<MyCollider> retrievedObjects, bool drawRect = false)
    {
        // 현재 노드와 겹치는 객체 추가
        if (bounds.Overlaps(area))
        {
            retrievedObjects.AddRange(objects);
        }

        // 하위 노드가 존재하면 재귀적으로 탐색
        if (nodes[0] != null)
        {
            foreach (var node in nodes)
            {
                //node.DrawRect(Color.white);
                if (node != null && node.bounds.Overlaps(area))
                {
                    node.RetrieveObjectsInArea(area, retrievedObjects, drawRect);
                }
            }
        }
        else
        {
            if (drawRect)
            {
                DrawRect(Color.red, 0.1f);
            }
        }
    }

    public void DrawQuadTree()
    {
        DrawRect(Color.white); // 현재 노드의 경계를 그립니다.

        if (nodes[0] != null)
        {
            foreach (var node in nodes)
            {
                if (node != null)
                {
                    node.DrawQuadTree(); // 하위 노드에 대해 재귀적으로 호출합니다.
                }
            }
        }
    }

    private void DrawRect(Color color, float duration = 0.1f)
    {
        Debug.DrawLine(bounds.min, new Vector2(bounds.min.x, bounds.max.y), color, duration);
        Debug.DrawLine(bounds.min, new Vector2(bounds.max.x, bounds.min.y), color, duration);
        Debug.DrawLine(bounds.max, new Vector2(bounds.min.x, bounds.max.y), color, duration);
        Debug.DrawLine(bounds.max, new Vector2(bounds.max.x, bounds.min.y), color, duration);
    }
}
