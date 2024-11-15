using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ExpOrb : ItemController
{
    public override void Use()
    {
        FindAnyObjectByType<DungeonPlayer>().EXPUp();
        Pool.Release(gameObject);
    }
}
