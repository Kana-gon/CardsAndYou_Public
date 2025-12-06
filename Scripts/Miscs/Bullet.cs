using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 targetPos;
    public float charge;
    public float basicDamage = 5;
    public float Penetrate = 1;
    private Vector3 direction;
    [SerializeField]public  float maxSpeed = 4.0f;
    [SerializeField] AudioSource hit;
    private BattleMng battleMng;
    public bool CanMove = false;
    public Transform posBaseObject;
    void Awake()
    {
        //StartCoroutine(LifeTimer());
        GetComponent<SpriteRenderer>().enabled = true;
        battleMng = GameObject.Find("Parent_of_InactiveObjects").transform.Find("BattleManager").GetComponent<BattleMng>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {//親の名前で判定しても良いかもね
        // if (collision.name.Contains("wall"))
        //     Destroy(gameObject);
        // if (collision.name.Contains("Player"))
        // {
        //     Debug.Log($"{basicDamage},{basicDamage + (charge - 5) / 5}");
        //     collision.gameObject.GetComponent<EnemyCard>().Hit(basicDamage + (charge - 25) / 5);
        //     Penetrate -= 1;
        //     if (Penetrate == 0)
        //         Destroy(gameObject);
        // }
    }
    void OnMouseEnter()
    {
        //Debug.Log("マウスにヒット");
        battleMng.DamageHP(basicDamage);
        transform.position = posBaseObject.position;
        CanMove = false;
        gameObject.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        //_rigid.velocity = direction * speed * Time.deltaTime;void Update()
        if (CanMove)
        {
            transform.position += transform.up * maxSpeed * Time.deltaTime;
        }
    }
    IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(7);
        Destroy(gameObject);
    }
    void OnBecameInvisible()
    {
        if (posBaseObject)
        {
            transform.position = posBaseObject.position;
            CanMove = false;
            gameObject.SetActive(false);
        }
    }
}
