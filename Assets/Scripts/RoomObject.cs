using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour {
    public float time;
    [HideInInspector]
    public bool occupied;
    public Sprite spr;

    private void Start()
    {
        occupied = false;
    }
}
