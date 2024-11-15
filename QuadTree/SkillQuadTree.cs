using System.Collections.Generic;
using UnityEngine;

public class SkillQuadTree
{
    private const int MAX_OBJECTS = 100;
    private const int MAX_LEVELS = 5;

    private int level;
    private List<GameObject> objects;
    private Rect bounds;
    private SkillQuadTree[] nodes;

    public SkillQuadTree(int level, Rect bounds)
    {
        this.level = level;
        this.objects = new List<GameObject>();
        this.bounds = bounds;
        this.nodes = new SkillQuadTree[4];
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

        nodes[0] = new SkillQuadTree(level + 1, new Rect(x + subWidth, y, subWidth, subHeight));
        nodes[1] = new SkillQuadTree(level + 1, new Rect(x, y, subWidth, subHeight));
        nodes[2] = new SkillQuadTree(level + 1, new Rect(x, y + subHeight, subWidth, subHeight));
        nodes[3] = new SkillQuadTree(level + 1, new Rect(x + subWidth, y + subHeight, subWidth, subHeight));
    }

    private int GetIndex(GameObject gameObject)
    {
        int index = -1;
        Vector2 p = gameObject.transform.position;
        bool topQuadrant = (p.y > bounds.y + bounds.height / 2);
        bool bottomQuadrant = (p.y < bounds.y + bounds.height / 2);

        if (p.x < bounds.x + bounds.width / 2)
        {
            if (topQuadrant)
            {
                index = 2;
            }
            else if (bottomQuadrant)
            {
                index = 1;
            }
        }
        else if (p.x > bounds.x + bounds.width / 2)
        {
            if (topQuadrant)
            {
                index = 3;
            }
            else if (bottomQuadrant)
            {
                index = 0;
            }
        }

        return index;
    }

    public void Insert(GameObject gameObject)
    {
        if (nodes[0] != null)
        {
            int index = GetIndex(gameObject);
            if (index != -1)
            {
                nodes[index].Insert(gameObject);
                return;
            }
        }

        objects.Add(gameObject);

        if (objects.Count > MAX_OBJECTS && level < MAX_LEVELS)
        {
            if (nodes[0] == null)
            {
                Split();
            }

            int i = 0;
            while (i < objects.Count)
            {
                int index = GetIndex(objects[i]);
                if (index != -1)
                {
                    nodes[index].Insert(objects[i]);
                    objects.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
    }

    public List<GameObject> Retrieve(List<GameObject> returnObjects, GameObject gameObject)
    {
        int index = GetIndex(gameObject);
        if (index != -1 && nodes[0] != null)
        {
            nodes[index].Retrieve(returnObjects, gameObject);
        }

        returnObjects.AddRange(objects);

        return returnObjects;
    }
}
