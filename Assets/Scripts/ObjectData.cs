using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectData
{
    public GameObject obj;
    public enum REACTION { Love, Like, Dislike, Hate};
    public REACTION reaction;
    public Sprite emote;
    public float satisfactionScore;
}
