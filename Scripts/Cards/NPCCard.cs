using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class MyDictionary<TKey, TValue>
{
    [SerializeField] private TKey _key;
    [SerializeField] private TValue _value;

    public TKey Key => _key;
    public TValue Value => _value;
}
public class NPCCard : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject talkObj;
    float nowChildCount;
    [Serializable]
    public struct talkElement
    {
        public string talkName;
        public bool isEndless;
    }
    [SerializeField] Sprite defaultGraphic, blinkGraphic;
    [SerializeField] public AudioSource clickCardAudio;

    [ShowInInspector, LabelText("再生開始位置_C")]
    [PropertyRange(0, nameof(MaxPlaybackTime_C))]
    public float sourceStartPos_C;
    private float MaxPlaybackTime_C => clickCardAudio != null && clickCardAudio.clip != null ? clickCardAudio.clip.length : 0f;
    private string talkThema = "";
    public Action talkedEventDelegate = null;//!個別スクリプト(Event)側でこれへの代入を忘れないこと！
    public Action talkedEventDelegate_withoutPlayer = null;//!こっちも
    //TODO? 正直個別クラスを共通クラスからの継承にして関数をオーバーライドするほうが綺麗かも そのうちね……
    private int talkIndex = 1;
    void Start()
    {
        nowChildCount = transform.childCount;
        GetComponent<Renderer>().material.shader = Shader.Find("Sprites/Default");
        talkObj = GameObject.Find("Parent_of_InactiveObjects").transform.Find("TalkManager").gameObject;
    }
    void OnTransformChildrenChanged()//上にカードが乗ったら
    {
        if (transform.childCount > nowChildCount)
        {
            if (GetComponent<CardBase>().childCard.GetComponent<CardBase>().cardID == "player")
            {
                StartTalk();
            }
        }
        else if (transform.childCount < nowChildCount)
        {
            //Debug.Log(name + "childrenの減少を検知");
        }
        nowChildCount = transform.childCount;//nowChildCountは一連の判定・処理が終わってから更新
    }
    private void OnMouseOver()
    {
        if (GetComponent<CardBase>().objBaseTransform.GetComponent<CardMng>().cardsClickable)
        {
            if (Input.GetMouseButtonDown(1) && StaticData.IsPlayerLiving == false)//プレイヤー死亡中、クリックされたら
            {
                StartTalk();
            }

        }
    }
    public void ChangeTalkThema(string talkThemaName)
    {
        talkThema = talkThemaName;
        talkIndex = 1;
    }
    public void ChangeTalkThemaIndex(int i)
    {
        talkIndex = i;
    }
    public void StartTalk()
    {
        if (StaticData.IsPlayerLiving)
        {
            ExecuteStartTalk();

        }
        else
        {
            ExecuteStartTalk_withoutPlayer();
        }
    }
    private void ExecuteStartTalk()
    {
        talkObj.GetComponent<TalkMng>().defaultGraphic = defaultGraphic;
        talkObj.GetComponent<TalkMng>().blinkGraphic = blinkGraphic;
        talkedEventDelegate();
        talkObj.GetComponent<TalkMng>().StartTalk(talkThema + "_" + talkIndex.ToString(), this);//"Guid_noramal1_1" "Guid_choice3_1"など
        if (talkObj.GetComponent<TalkMng>().IsTalkDataExist(talkThema + "_" + (talkIndex + 1).ToString()))
            talkIndex++;
    }
    private void ExecuteStartTalk_withoutPlayer()
    {
        clickCardAudio.time = sourceStartPos_C;
        clickCardAudio.Play();
        talkObj.GetComponent<TalkMng>().defaultGraphic = defaultGraphic;
        talkObj.GetComponent<TalkMng>().blinkGraphic = blinkGraphic;
        talkedEventDelegate_withoutPlayer();
        talkObj.GetComponent<TalkMng>().StartTalk(talkThema + "_" + talkIndex.ToString(), this);//"Guid_noramal1_1" "Guid_choice3_1"など
        if (talkObj.GetComponent<TalkMng>().IsTalkDataExist(talkThema + "_" + (talkIndex + 1).ToString()))
            talkIndex++;
    }
}
