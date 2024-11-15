using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;
using TMPro;

public class MonsterController : MonoBehaviour, IPoolableObject
{
    [SerializeField] private MyCollider col;
    public float hp = 10f;
    float speed = 1.5f;
    DungeonPlayer player;
    BoxCollider2D boxCollider;
    SpriteRenderer render;
    public TMP_Text damageText;//대미지 표시 텍스트

    public IObjectPool<GameObject> Pool { get; set; }
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        render = GetComponent<SpriteRenderer>();
    }
    //풀에서 몬스터를 꺼낼 때
    private void OnEnable()
    {
        boxCollider.enabled = true;
        render.DOFade(1, 0.1f);
        
        hp = 10f;
    }
    public void Init()
    {
        player = FindAnyObjectByType<DungeonPlayer>();
        StartCoroutine(TraceTarget());
    }

    void Update()
    {
        //QuadTreeManager.Instance.UpdateObject(col);
    }

    IEnumerator TraceTarget()
    {
        while (true)
        {
            Vector3 targetPos = player.transform.position;
            Vector2 dir = targetPos - transform.position;
            transform.Translate(dir.normalized * Time.deltaTime * speed);
            yield return null;
        }
    }

    public void OnGet()
    {
        Init();
    }

    public void OnRelease()
    {

    }

    public void OnDamaged(float damage)
    {
        hp -= damage;
        StartCoroutine(CoDamageText(damage));
        if (hp <= 0)
        {
            if (gameObject.activeSelf)
            {
                //몬스터 사망 시
                boxCollider.enabled = false;
                render.DOFade(0, 1);
                ItemManager.Instance.RandomSpawnItem(transform.position);
                //Invoke("ReleaseMonster", 1);
                StartCoroutine(CoRelease(1));
            }
        }
    }
    private IEnumerator CoRelease(float releaseTime)
    {
        yield return new WaitForSeconds(releaseTime);
        Pool.Release(gameObject);
    }
    private IEnumerator CoDamageText(float damage)
    {
        damageText.text = damage.ToString();
        damageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        damageText.gameObject.SetActive(false);
    }
}
