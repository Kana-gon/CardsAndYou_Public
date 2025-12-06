using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EventMng : MonoBehaviour
{
    // Start is called before the first frame update
    [Title("マネージャーたち")]
    [SerializeField] BattleMng battleMng;
    [SerializeField] BGMMng bgmMng;
    //[SerializeField] TalkMng talkMng;

    [Title("参照：チュートリアルイベント")]
    [SerializeField] GameObject enemyObject;
    [SerializeField] NPCCard guidCard;
    //
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void EnemyAppear_tutrial()//?チュートリアルバトル 君本当にここでいいの？
    {
        battleMng.AddEnemys(enemyObject);
        battleMng.battleType = BattleMng.BattleType.TUTRIAL;//?結局列挙型である必要はあるのか？
    }
    public void EnemyActivate()
    {
        enemyObject.GetComponent<Animator>().Play("Enemy01_Move");
        enemyObject.GetComponent<EnemyCard>().StartBurrage();
    }
    public void OnTutorialBattleEnd()
    {
        //Debug.Log("戦闘終了を検知");
        guidCard.ChangeTalkThema("Guid_tutorialBattleEnd");
        guidCard.ChangeTalkThemaIndex(1);
        guidCard.StartTalk();
        bgmMng.PlayBGM("BGM_none");
    }

}
