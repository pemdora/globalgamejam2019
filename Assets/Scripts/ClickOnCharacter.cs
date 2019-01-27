using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnCharacter : MonoBehaviour {

    [SerializeField]
    private MonsterController monster;
    //Script to drag an object in world space using the mouse
    private void OnMouseDown()
    {
        monster.OpenCharacterCharac();
    }

}
