using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class generic_impactable : MonoBehaviour
{
    public Rigidbody2D rb2d;

    [SerializeField]
    private Tilemap underTiles;

    private player_bowling_ball bowlingBall;
    public levelManager LVM;

    private bool knockedOut = false;
    public bool pushable = false;


    private void Awake() {
        if(rb2d == null) {
            rb2d = GetComponent<Rigidbody2D>();
        }
        if(underTiles == null) {
            underTiles = GameObject.FindGameObjectWithTag("tiles_UNDER").GetComponent<Tilemap>();
        }
        if(bowlingBall == null) {
            bowlingBall = GameObject.FindGameObjectWithTag("bowlingBall").GetComponent<player_bowling_ball>();
        }
        if (LVM == null) {
            LVM = GameObject.FindGameObjectWithTag("LVM").GetComponent<levelManager>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.LogWarning("generic impactable was hit by an object trigger");
        if (bowlingBall.goingFast && !knockedOut) {
            LVM.score += 1;
            //rb2d.gameObject.SetActive(false);//turns off collissions init?
            transform.Rotate(0, 0, 90);//spaceman go horizontal
            knockedOut = true;
            Debug.Log("pawn went horizontal");
        }
        if (!bowlingBall.goingFast && !knockedOut && pushable) {
            transform.Translate((Vector3)bowlingBall.heading * 2);//todo, make this check for valid tiles and also stop the ball if it'd impact a wall.
        }
    }
}
