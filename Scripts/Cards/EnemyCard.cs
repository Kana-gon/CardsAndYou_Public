
using System.Collections;
using System.Collections.Generic;
using App.BaseSystem.DataStores.ScriptableObjects.Enemy;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

public class EnemyCard : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isHostile = true;
    private GameObject burrage;
    private BattleMng battleMng;
    [SerializeField] GameObject crumpledPaper;
    [SerializeField] EnemyData enemyData;
    [SerializeField] ProgressBar progressBar;
    List<EnemyData.dropItem> damagedDropItems = new List<EnemyData.dropItem>();
    List<EnemyData.dropItem> dropItems = new List<EnemyData.dropItem>();
    [SerializeField] public AudioSource audioSource;

    [ShowInInspector, LabelText("再生開始位置")]
    [PropertyRange(0, nameof(MaxPlaybackTime))]
    public float sourceStartPos;
    private float MaxPlaybackTime => audioSource != null && audioSource.clip != null ? audioSource.clip.length : 0f;
    [Title("パラメータ")]
    [SerializeField] float MAXHP;
    private float HP;
    [SerializeField] float DEF;
    [Title("ドロップアイテムパラメータ")]
    public Vector3 generateRandomness = new Vector3(3f, 3f, 0);
    public float moveDuration = 0.8f;
    void Start()
    {
        progressBar.progress = 100;
        battleMng = GameObject.Find("Parent_of_InactiveObjects").transform.Find("BattleManager").GetComponent<BattleMng>();
        burrage = Instantiate(enemyData.GetBurrage(), this.transform);
        burrage.transform.position = transform.position;
        dropItems = enemyData.GetDropItems();
        MAXHP = enemyData.GetHP();
        HP = MAXHP;
        DEF = enemyData.GetDEF();
        damagedDropItems = enemyData.GetDamagedDropItems();
    }
    [Button("弾幕スタート")]
    public void StartBurrage()
    {
        burrage.GetComponent<Burrage>().StartBurrage();
    }
    // Update is called once per frame
    void OnMouseEnter()
    {
        //battleMng.AddHP(-10);
    }
    public void Hit(float damage)
    {
        //Debug.Log($"Enemy damaged!!damage:{damage}");
        HP -= damage;
        progressBar.progress = HP / MAXHP * 100;
        if (HP < 0)
        {
            Dead();
        }
        else
        {

            audioSource.time = sourceStartPos;
            //audioSource.Play();
        }
        Drop(damagedDropItems);
    }
    private void Dead()
    {
        //Debug.Log("DEAD!");
        Drop(dropItems);
        Destroy(gameObject);
        Destroy(burrage);
    }
    void Drop(List<EnemyData.dropItem> dropItems)
    {
        foreach (var dropItem in dropItems)
        {
            int num = dropItem.num + Random.Range(-1, 2);

            for (int i = 0; i < num; i++)
            {
                var generatingTarget = crumpledPaper;
                generatingTarget.GetComponent<CrumpledPaper>().dropCard = dropItem.obj;
                var generatedTarget = Instantiate(generatingTarget, transform.position, Quaternion.identity, this.GetComponent<CardBase>().objBaseTransform);
                var targetPos = transform.TransformPoint(
                    new Vector3(
                        UnityEngine.Random.Range(0, generateRandomness.x) - generateRandomness.x / 2,
                        UnityEngine.Random.Range(0, generateRandomness.y) - generateRandomness.y / 2,
                        0.0f)
                        );

                targetPos.x = Mathf.Clamp(targetPos.x, -8.2f, 8.2f);
                targetPos.y = Mathf.Clamp(targetPos.y, -4.0f, 4.0f);
                generatedTarget.GetComponent<CrumpledPaper>().MovePaper(targetPos, moveDuration);
            }
        }
    }
}
