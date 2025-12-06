using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;

public class TalkMng : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private SpriteRenderer talkingNPCGraphic;
    [SerializeField]
    private ChoiceMng choiceMng;

    [SerializeField]
    private TalkDataBase talkDataBase;
    [SerializeField]
    private ChoiceDataBase choiceDataBase;
    [SerializeField]
    private CutScene cutScene;
    [SerializeField]
    private TextMeshPro talkText;
    [SerializeField] private AudioSource source;

    [ShowInInspector, LabelText("再生開始位置")]
    [PropertyRange(0, nameof(MaxPlaybackTime))]
    private float sourceStartPos;
    private float MaxPlaybackTime => source != null && source.clip != null ? source.clip.length : 0f;
    private IEnumerator talking = null;
    private bool isClicked = false;
    private TalkData talkData;//StartTalkでリストから検索して取ってきたデータを入れる。
    
    public Sprite defaultGraphic, blinkGraphic;
    private IEnumerator blinkTimer;
    void Start()
    {
        gameObject.SetActive(false);
    }
    void OnEnable()
    {
        blinkTimer = BlinkTimer();
        StartCoroutine(blinkTimer);
    }
    private NPCCard talkingNPC;
    // void OnMouseDown()
    // {
    //     //isClicked = true;
    // }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isClicked = true;

        }
    }
    public void StartTalk(string talkName, NPCCard _talkingNPC)
    {
        talkingNPC = _talkingNPC;
        GameObject.Find("Cards").GetComponent<CardMng>().cardsClickable = false;
        GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(true);
        //Debug.Log(talkDataBase);
        talkData = talkDataBase.GetTalkDataByName(talkName);
        //Debug.Log($"talkname={talkName},talkData={talkData}");
        talking = Talking();
        //Debug.Log(talking);
        StartCoroutine(talking);
        //会話の開始
    }
    public void ProceedTalk()
    {
        isClicked = true;
    }
    public void StopTalk()
    {
        if (talking != null)
        {
            StopCoroutine(talking);
        }
    }
    IEnumerator Talking()//*/TODOメモ参照
    {
        foreach (var talk in talkData.GetLineList())
        {
            isClicked = false;
            talkText.text = talk.lineText;
            if (talk.faseSprite)
                talkingNPCGraphic.sprite = talk.faseSprite;
            if (talk.choiceID != "")//選択肢が発生中なら
            {
                //Debug.Log($"{talk.choiceID}の選択肢を表示します");
                GetComponent<BoxCollider2D>().enabled = false;
                choiceMng.SetChoice(talk.choiceID);//選択の諸々はChoiceMng側で処理
                yield return new WaitUntil(() => choiceMng.isClicked);
                yield return new WaitUntil(() => isClicked);
                source.time = sourceStartPos;
                source.Play();
                if (choiceMng.isAllChoiced)
                {
                    talkingNPC.ChangeTalkThema(choiceDataBase.GetChoice(talk.choiceID).GetChoiceEndTalkThema());
                }
                StopCoroutine(talking);
                StartTalk(choiceMng.NextTalkName, talkingNPC);
            }

            //Debug.Log("テキスト変更");
            //yield return new WaitForSeconds(1);
            yield return new WaitUntil(() => isClicked);
            source.time = sourceStartPos;
            source.Play();
            if (talk.cutSceneID != "")//イベントが発生中なら
            {
                //Debug.Log($"event:{talk.cutSceneID}");
                cutScene.LaunchEvent(talk.cutSceneID);
            }
            //?TODO 一部のカードのクリック判定がTalkObjよりも優先されている。（動かすとこれよりも下になる）何故？
        }
        gameObject.SetActive(false);
        GameObject.Find("Cards").GetComponent<CardMng>().cardsClickable = true;
    }
    IEnumerator BlinkTimer()//*まばたき。会話設定側(NPCCards.csのtalkStartらへん)でデフォルト画像と目瞑り画像を入れてあげる
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(2.5f, 3.0f));
            //Debug.Log($"defaultGraphic = {defaultGraphic},nowGraphic = {talkingNPCGraphic.sprite}");

            if (talkingNPCGraphic.sprite == defaultGraphic)
            {
                talkingNPCGraphic.sprite = blinkGraphic;
                yield return new WaitForSeconds(0.08f);
                talkingNPCGraphic.sprite = defaultGraphic;
                talkingNPCGraphic.sprite = defaultGraphic;

            }
        }
    }
    public bool IsTalkDataExist(string talkName)
    {
        if (talkDataBase.GetTalkDataByName(talkName) != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
