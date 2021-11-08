using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class levelManager : MonoBehaviour
{
    public int fastMoves;
    public int slowMoves;
    public int clownRepositions;
    public int continuedBowls;

    public int score;
    public int levelID;

    public bool infiniteFastMoves = false;
    public bool infiniteSlowMoves = false;
    public bool infiniteClownRepo = false;
    public bool infiniteContBowl = false;

    public bool clownActive = false;//sets the clown as the first active object

    public player_bowling_ball bowlingBall;
    public GameObject _BBREF;
    public playerClown _playerClown;

    public TextMeshProUGUI slowAMT;
    public TextMeshProUGUI fastAMT;
    public TextMeshProUGUI clownRepoAMT;
    public TextMeshProUGUI contBowlAMT;
    public Image modeImage;
    public Sprite modeImageClown;
    public Sprite modeImageBall;
    public GameObject helpMenuHolder;
    public GameObject gameMenuHolder;

    private void Start() {
        if (slowAMT == null) {
            slowAMT = GameObject.FindGameObjectWithTag("LVM-SlowAMT").GetComponent<TextMeshProUGUI>();
        }
        if (fastAMT == null) {
            fastAMT = GameObject.FindGameObjectWithTag("LVM-FastAMT").GetComponent<TextMeshProUGUI>();
        }
        if (clownRepoAMT == null) {
            clownRepoAMT = GameObject.FindGameObjectWithTag("LVM-ClownAMT").GetComponent<TextMeshProUGUI>();
        }
        if (contBowlAMT == null) {
            contBowlAMT = GameObject.FindGameObjectWithTag("LVM-ContAMT").GetComponent<TextMeshProUGUI>();
        }
        if (bowlingBall == null) {
            bowlingBall = GameObject.FindGameObjectWithTag("bowlingBall").GetComponent<player_bowling_ball>();
        }
        if (_playerClown == null) {
            _playerClown = GameObject.FindGameObjectWithTag("Player").GetComponent<playerClown>();
        }
        if (_BBREF == null) {
            _BBREF = GameObject.FindGameObjectWithTag("bowlingBall");
        }
        updateHUDValues();
        updateMoveModeInitial();
        _BBREF.SetActive(false);//disable the bowling ball, this leaves the rest to the playerClown.cs
        gameMenuHolder.SetActive(false);
        helpMenuHolder.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {//todo, prevent player input if the help or game menu is active
            if(clownActive && !_BBREF.activeSelf) {
                return;
            }
            updateMoveMode();
            //play toggle SFX
        }
    }
    public void updateMoveModeInitial() {
        bowlingBall.arrowInputManager.toggleChildren(false);
        bowlingBall.readyToRoll = false;
        _playerClown.arrowMC.toggleChildren(false);
        _playerClown.arrowMC.arrowsOut = false;
        modeImage.sprite = modeImageClown;
        modeImage.SetNativeSize();
        modeImage.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
    }
    public void updateMoveMode() {
        clownActive = !clownActive;
        if (clownActive) {
            bowlingBall.arrowInputManager.toggleChildren(false);
            bowlingBall.readyToRoll = false;
            _playerClown.arrowMC.toggleChildren(false);
            _playerClown.arrowMC.arrowsOut = false;
            modeImage.sprite = modeImageClown;
            modeImage.SetNativeSize();
            modeImage.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
        } else if (!clownActive) {
            bowlingBall.arrowInputManager.toggleChildren(false);
            bowlingBall.readyToRoll = false;
            _playerClown.arrowMC.toggleChildren(false);
            _playerClown.arrowMC.arrowsOut = false;
            modeImage.sprite = modeImageBall;
            modeImage.SetNativeSize();
            modeImage.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
        }
    }
    public void updateHUDValues() {
        if (infiniteSlowMoves) {
            slowAMT.text = "∞";
        } else if (!infiniteSlowMoves) {
            slowAMT.text = slowMoves.ToString();
        }

        if (infiniteFastMoves) {
            fastAMT.text = "∞";
        }
        else if (!infiniteFastMoves) {
            fastAMT.text = fastMoves.ToString();
        }

        if (infiniteClownRepo) {
            clownRepoAMT.text = "∞";
        }
        else if (!infiniteClownRepo) {
            clownRepoAMT.text = clownRepositions.ToString();
        }

        if (infiniteContBowl) {
            contBowlAMT.text = "∞";
        }
        else if (!infiniteContBowl) {
            contBowlAMT.text = continuedBowls.ToString();
        } 
    }
    public void toggleHelpMenu() {

    }
    public void toggleGameMenu() {

    }
}
