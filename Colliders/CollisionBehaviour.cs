using UnityEngine;

public class CollisionBehaviour : MonoBehaviour
{
    public MyCollider MyCollider { get { return GetComponent<MyCollider>(); } }

    public void Start()
    {
        //if(MyCollider != null) MyCollider.OnCollisionEnterEvent += OnMyCollisionEnter2D;
    }
    protected virtual void OnMyCollisionEnter2D(MyCollider other) { }

    public void OnDestroy()
    {
        //if(MyCollider != null) MyCollider.OnCollisionEnterEvent -= OnMyCollisionEnter2D;
    }
}
