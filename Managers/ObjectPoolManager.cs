using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : SingletonManager<ObjectPoolManager>
{
    private Dictionary<string, IObjectPool<GameObject>> poollingObjectDict = new Dictionary<string, IObjectPool<GameObject>>();

    public bool CreatePoolling<T>(GameObject parent, GameObject prefab, int initialSize, int maxSize) where T : IPoolableObject
    {
        string keyName = prefab.name;
        if (poollingObjectDict.ContainsKey(keyName))
        {
            Debug.LogWarning("Pool with id " + keyName + " already exists.");
            return false;
        }

        var pool = new ObjectPool<GameObject>(() => CreateObject<T>(parent, prefab), OnGetPool, OnReleasePool, OnDestroyPool, true, initialSize, maxSize * 10);
        poollingObjectDict.Add(keyName, pool);

        for (int i = 0; i < initialSize; i++)
        {
            var obj = CreateObject<T>(parent, prefab);
            pool.Release(obj);
        }
        return true;
    }

    private GameObject CreateObject<T>(GameObject parent, GameObject prefab) where T : IPoolableObject
    {
        GameObject obj = GameObject.Instantiate(prefab);
        obj.SetActive(false);
        obj.transform.SetParent(parent.transform, false);
        obj.GetComponent<T>().Pool = poollingObjectDict[prefab.name];

        return obj;
    }

    private void OnGetPool(GameObject obj)
    {
        obj.SetActive(true);
        var poolable = obj.GetComponent<IPoolableObject>();
        poolable?.OnGet();
    }

    private void OnReleasePool(GameObject obj)
    {
        obj.SetActive(false);
        var poolable = obj.GetComponent<IPoolableObject>();
        poolable?.OnRelease();
    }

    private void OnDestroyPool(GameObject obj)
    {
        Destroy(obj);
    }

    public int GetPoolSize(string poolId)
    {
        if (poollingObjectDict.TryGetValue(poolId, out var pool))
        {
            return pool.CountInactive;
        }
        else
        {
            Debug.LogWarning("Pool with id " + poolId + " does not exist.");
            return 0;
        }
    }

    public int GetPoolSize(GameObject obj)
    {
        return GetPoolSize(obj.name);
    }

    public IObjectPool<GameObject> GetPoolling(string poolId)
    {
        if (poollingObjectDict.TryGetValue(poolId, out var pool))
        {
            return pool;
        }
        else
        {
            Debug.LogWarning("Pool with id " + poolId + " does not exist.");
            return null;
        }
    }

    public IObjectPool<GameObject> GetPoolling(GameObject obj)
    {
        return GetPoolling(obj.name);
    }

    public void ResetPool(string poolId)
    {
        if (poollingObjectDict.TryGetValue(poolId, out var pool))
        {
            while (pool.CountInactive > 0)
            {
                var go = pool.Get();
                Destroy(go);
            }
            poollingObjectDict.Remove(poolId);
        }
        else
        {
            Debug.LogWarning("Pool with id " + poolId + " does not exist.");
        }
    }
    public void ResetPool(GameObject obj)
    {
        ResetPool(obj.name);
    }
}
