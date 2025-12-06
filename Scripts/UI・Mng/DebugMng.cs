using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DebugMng : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] NPCCard npcCard;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    [Button("デバッグ：進行度調整：プレイヤー死亡会話")]
    private void debugButton_TalkThemaChangeToPlayerDie()
    {
        npcCard.ChangeTalkThema("Guid_bringSword");
        npcCard.ChangeTalkThemaIndex(1);
    }
}
