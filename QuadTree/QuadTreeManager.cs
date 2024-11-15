using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class QuadTreeManager : SingletonManager<QuadTreeManager>
{
    [SerializeField] Tilemap tilemap;
    List<MyCollider> colliders = new List<MyCollider>();
    private QuadTree quadTree;
    public Rect bounds { get; private set; }

    private void Start()
    {
        BoundsInt tilemapBounds = tilemap ? tilemap.cellBounds : new BoundsInt();
        bounds = new Rect((Vector2Int)tilemapBounds.min, (Vector2Int)tilemapBounds.size); // 예시로 설정한 초기 범위
        quadTree = new QuadTree(bounds);
    }

    private void Update()
    {
        DrawQuadTreeNodes();
        if (colliders.Count > 0)
            UpdateQuadTree();
    }

    public bool Insert(MyCollider collider)
    {
        colliders.Add(collider);
        return quadTree.Insert(collider);
    }

    public void Remove(MyCollider collider)
    {
        colliders.Remove(collider);
        quadTree.Remove(collider);
    }

    public void UpdateQuadTree()
    {
        List<MyCollider> colliders = Retrieve(bounds);
        for (int i = 0; i < colliders.Count; i++)
        {
            var collider = colliders[i];
            if (quadTree.Remove(collider))
                quadTree.Insert(collider);
            else Debug.Log("Remove is Failed");
        }
    }

    public void UpdateObject(MyCollider collider)
    {
        if (quadTree.Remove(collider))
        {
            quadTree.Insert(collider);
        }
        else
        {
            Debug.Log("Remove is Failed");
        }
    }
    public List<MyCollider> Retrieve()
    {
        return new List<MyCollider>(colliders);
    }

    public List<MyCollider> Retrieve(Rect area)
    {
        List<MyCollider> retrieves = quadTree.RetrieveObjects(area);
        return retrieves;
    }
    public List<MyCollider> Retrieve(Rect area, bool drawRect = false)
    {
        List<MyCollider> retrieves = quadTree.RetrieveObjects(area, drawRect);
        Culling(retrieves);
        return retrieves;
    }

    public void Culling(List<MyCollider> retrieves)
    {
        foreach (var collider in colliders)
        {
            collider.UnCulling();
        }
        foreach (var collider in retrieves)
        {
            collider.OnCulling();
        }
    }

    public void DrawQuadTreeNodes()
    {
        quadTree.DrawQuadTree();
    }
}
