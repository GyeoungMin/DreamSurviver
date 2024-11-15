using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum ItemType { None = -1, ExpOrb, Coin, Potion, Margnet }
public class ItemManager : SingletonManager<ItemManager>
{
    [SerializeField] GameObject[] prefabItems;
    ItemController[] items;
    public IObjectPool<GameObject>[] pools;

    void Start()
    {
        Init();
    }
    void Init()
    {
        items = new ItemController[prefabItems.Length];
        pools = new IObjectPool<GameObject>[prefabItems.Length];
        for (int i = 0; i < prefabItems.Length; i++)
        {
            items[i] = prefabItems[i].GetComponent<ItemController>();
            if (CreatePool(prefabItems[i]))
            {
                pools[i] = ObjectPoolManager.Instance.GetPoolling(prefabItems[i]);
            }
        }
    }

    bool CreatePool(GameObject prefab)
    {
        GameObject parent = new GameObject(prefab.name);
        parent.transform.SetParent(gameObject.transform);
        bool result = ObjectPoolManager.Instance.CreatePoolling<ItemController>(parent, prefab, 10, 20);
        Debug.LogFormat("Created Poolling {0} is {1}", prefab.name, result ? "Success" : "Failed");
        return result;
    }

    public void RandomSpawnItem(Vector2 position)
    {
        int rand = Random.Range(0, 100);
        ItemType type = ItemType.None;
        if (rand < 50)
        {
            type = ItemType.ExpOrb;
        }
        else if (rand < 54)
        {
            type = ItemType.Coin;
        }
        else if (rand < 58)
        {
            type = ItemType.Potion;
        }
        else if (rand < 60)
        {
            type = ItemType.Margnet;
        }

        Debug.Log($"SpawnItem : {type}");
        if (type == ItemType.None) return;
        //SpawnItem(position,type);
    }

    public void SpawnItem(Vector2 position, ItemType type)
    {
        //var item = items[(int)type].Pool.Get();
        var item = pools[(int)type].Get();
        item.transform.position = position;
    }

    public void TakeItem(GameObject item, ItemType type)
    {
        items[(int)type].Use();
    }

}
