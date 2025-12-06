using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Burrage_01_core : Burrage
{
    // Start is called before the first frame update
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject burrageCore;
    IEnumerator bulletGenerate;
    private GameObject[] bullets = new GameObject[100];
    private int bulletNum = 0;
    float zRote_deg = 180;
    int zRote_bec = 1;
    [Title("パラメータ")]
    [SerializeField] float burrageDensity = 0.2f;
    [SerializeField] float burrageSpeed = 5.0f;
    [SerializeField] float burrageDamage = 5.0f;
    void Awake()
    {
        bulletGenerate = BulletGenerate();
    }

    // Update is called once per frame
    void Update()
    {
        zRote_deg += zRote_bec * 0.1f;
        if (zRote_deg > 270)
        {
            zRote_deg = 180;
        }
        //transform.rotation = Quaternion.Euler(0, 0, zRote_deg);
    }
    public override void StartBurrage()
    {
        for (int i = 0; i < 100; i++)
        {
            bullets[i] = Instantiate(bullet);
            bullets[i].SetActive(false);
            bullets[i].GetComponent<Bullet>().posBaseObject = gameObject.transform;
        }
        StartCoroutine(bulletGenerate);
    }
    IEnumerator BulletGenerate()//弾幕のメイン部分
    {
        float timer = 0;
        while (true)
        {

            yield return null;//TODO deltaTimeに修正
            timer += Time.deltaTime;
            if (timer > burrageDensity)
            {
                bullets[bulletNum].transform.rotation = Quaternion.Euler(0, 0, zRote_deg);
                bullets[bulletNum].transform.position = transform.position;
                bullets[bulletNum].SetActive(true);
                bullets[bulletNum].GetComponent<Bullet>().maxSpeed = burrageSpeed;
                bullets[bulletNum].GetComponent<Bullet>().basicDamage = burrageDamage;
                bullets[bulletNum].GetComponent<Bullet>().CanMove = true;
                bulletNum++;

                bullets[bulletNum].transform.rotation = Quaternion.Euler(0, 0, zRote_deg - 30);
                bullets[bulletNum].transform.position = transform.position;
                bullets[bulletNum].SetActive(true);
                bullets[bulletNum].GetComponent<Bullet>().maxSpeed = burrageSpeed;
                bullets[bulletNum].GetComponent<Bullet>().basicDamage = burrageDamage;
                bullets[bulletNum].GetComponent<Bullet>().CanMove = true;

                bulletNum++;
                bullets[bulletNum].transform.rotation = Quaternion.Euler(0, 0, zRote_deg - 60);
                bullets[bulletNum].transform.position = transform.position;
                bullets[bulletNum].SetActive(true);
                bullets[bulletNum].GetComponent<Bullet>().maxSpeed = burrageSpeed;
                bullets[bulletNum].GetComponent<Bullet>().basicDamage = burrageDamage;
                bullets[bulletNum].GetComponent<Bullet>().CanMove = true;
                bulletNum++;

                bullets[bulletNum].transform.rotation = Quaternion.Euler(0, 0, zRote_deg - 90);
                bullets[bulletNum].transform.position = transform.position;
                bullets[bulletNum].SetActive(true);
                bullets[bulletNum].GetComponent<Bullet>().maxSpeed = burrageSpeed;
                bullets[bulletNum].GetComponent<Bullet>().basicDamage = burrageDamage;
                bullets[bulletNum].GetComponent<Bullet>().CanMove = true;
                bulletNum++;

                if (bulletNum > 99)
                {
                    bulletNum = 0;
                }
                timer = 0f;
            }
        }
    }
    void OnDestroy()
    {
        for (int i = 0; i < 100; i++)
        {
            Destroy(bullets[i]);
        }
    }
}
