using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerClown : MonoBehaviour
{
    
    public dirArrowManagerClown arrowMC;

    private levelManager LVM;

    public Vector2 dir;

    public GameObject bowlingBall;
    public player_bowling_ball _bowlingBallRef;
    public bool bowlingBallActive;

    private void Start() {
        if(LVM == null) {
            LVM = GameObject.FindGameObjectWithTag("LVM").GetComponent<levelManager>();
        }
        if(bowlingBall == null) {
            bowlingBall = GameObject.FindGameObjectWithTag("bowlingBall");
            _bowlingBallRef = bowlingBall.GetComponent<player_bowling_ball>();
        }
    }
    private void Update() {
        if (!LVM.clownActive) {
            return;
        }
        ProcessInputs();
    }
    //TODO
    //----------------------------------------------------
    //Add pushing mechanics to other pawns like the bowling ball
    private void ProcessInputs() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (arrowMC.arrowsOut) {
                arrowMC.toggleChildren(false);
                arrowMC.arrowsOut = false;
            } else {
                arrowMC.toggleChildren(true);
                arrowMC.arrowsOut = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (arrowMC.arrowsOut) {
                arrowMC.toggleBallMode();
            }
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            jumpToBall();
        }
    }
    public void moveOrSpawn(bool S) {
        if (S) {
            if (bowlingBall.gameObject.activeSelf) {
                bowlingBall.transform.position = transform.position;//reset the bowling ball to us
                bowlingBall.SetActive(false);
            }
            if (!bowlingBall.gameObject.activeSelf) {
                bowlingBall.SetActive(true);
                bowlingBall.transform.position = transform.position;//reset our position again, just to be safe.
                _bowlingBallRef.tilesToTravel = 1;
                _bowlingBallRef.doMove(dir);
                LVM.updateMoveMode();//flip us over to ball controls
                //Todo, play a SFX here
            }
        }
        else if (!S) {
            //domove
            transform.Translate((Vector3)dir * 2);
        }
    }
    public void jumpToBall() {
        if (bowlingBall.gameObject.activeSelf) {
            transform.position = bowlingBall.transform.position;
            bowlingBall.SetActive(false);
        } else {
            Debug.LogWarning("pClown tried to jump to an inactive ball");
        }
        
        //todo make this cost move currency with the LVM
    }
}
