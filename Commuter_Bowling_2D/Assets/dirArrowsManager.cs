using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dirArrowsManager : MonoBehaviour {
    public Vector2 returnableHeading;

    [SerializeField]
    private GameObject[] dirArrows;

    public player_bowling_ball bowlingBall;

    private void Start() {
        if (bowlingBall == null) {
            bowlingBall = GameObject.FindGameObjectWithTag("bowlingBall").GetComponent<player_bowling_ball>();
        }
    }
    public Vector2 retrieveHeading() {
        return returnableHeading;
    }

    public void toggleChildren(bool B) {
        for (int i = 0; i < dirArrows.Length; i++) {
            Debug.Log("Toggling children in arrow manager: " + B);  
            dirArrows[i].SetActive(B);
        }
    }
    public void toggleSlowMode(bool S) {
        for (int i = 0; i < dirArrows.Length; i++) {
            dirArrows[i].GetComponent<dirArrowButton>().slowMode = S;
            dirArrows[i].GetComponent<dirArrowButton>().updateSprite();
        }
    }
}
