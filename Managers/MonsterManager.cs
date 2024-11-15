using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterManager : SingletonManager<MonsterManager>
{
    [SerializeField] GameObject prefabMonster;
    MonsterController monsterController;
    public IObjectPool<GameObject> pool;

    void Start()
    {
        Init();
    }

    void Init()
    {
        ObjectPoolManager.Instance.CreatePoolling<MonsterController>(gameObject, prefabMonster, 100, 300);
        pool = ObjectPoolManager.Instance.GetPoolling(prefabMonster);
    }

    public void SpawnMonster(Vector2 position)
    {
        var go = pool.Get();
        go.transform.position = position;
    }

    public void DeSpawnMonster(GameObject go)
    {
        go.GetComponent<MonsterController>().OnDamaged(1f);
        //pool.Release(go);
    }
}
