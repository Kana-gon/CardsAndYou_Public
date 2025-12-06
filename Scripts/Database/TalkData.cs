using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data/Talk")]
public class TalkData : ScriptableObject
{
    [SerializeField]
    private string talkName;
    [Serializable]
    public struct Line
    {
        public Sprite faseSprite;
        public string choiceID;
        public string cutSceneID;
        public string Comment;
        [TextArea] public string lineText;

    }
    [SerializeField]
    private List<Line> lineList = new List<Line>();

    public string GetTalkName()
    {
        return talkName;
    }

    public List<Line> GetLineList()
    {
        return lineList;
    }
}
