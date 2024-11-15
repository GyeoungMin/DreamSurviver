using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class ItemController : MonoBehaviour, IPoolableObject
{
    public IObjectPool<GameObject> Pool { get; set; }

    public void Init()
    {
    }

    public abstract void Use();

    public void OnGet()
    {
        Init();
        //gameObject.SetActive(true);
    }

    public void OnRelease()
    {
        //gameObject.SetActive(false);
        //Use();
    }
}
