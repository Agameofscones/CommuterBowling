using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class player_bowling_ball : MonoBehaviour
{
    [SerializeField]
    private Tilemap underTiles;
    [SerializeField]
    private Tilemap wallTiles;
    [SerializeField]
    private Tilemap uniqueTiles;

    public UniqueTileManager UTM;

    private Vector2 NORTH = new Vector2(0, 1);//these should be defined somewhere else, and probably staticly accesible, and not duplicated from the UTM
    private Vector2 SOUTH = new Vector2(0, -1);
    private Vector2 EAST = new Vector2(1, 0);
    private Vector2 WEST = new Vector2(-1, 0);

    public levelManager LVM;

    public int baseTileSpeed = 6;//The amount of tiles we move with each roll
    public int tilesToTravel = 0;//The amount of tiles we have left before we can roll again
    public int slowsAt = 2;//the tiles left to travel that we turn goingFast to false. Also doubles as our slowtilespeed
    public bool goingFast = false;//is the ball going fast? This changes the interaction behavior of different tiles
    public bool travelling = false;//refuse inputs if we're already moving

    public Vector2 heading;

    public bool readyToRoll = false;//used to toggle movement of the ball
    public bool slowMode = false;

    public GameObject ghostball;

    public dirArrowsManager arrowInputManager;//to-do toggle the gameobject, but also get a reference ot the script!

    private bool waitingForMove = false;
    public float moveDelay = 0.2f;

    private void Start() {
        heading = new Vector2(0, 0);//we aren't going any direction, yet!
        if(UTM == null) {
            UTM = GameObject.FindGameObjectWithTag("UTM").GetComponent<UniqueTileManager>();
        }
        arrowInputManager.toggleChildren(false);//disable our UI roll arrows

        if (underTiles == null) {
            underTiles = GameObject.FindGameObjectWithTag("tiles_UNDER").GetComponent<Tilemap>();
        }
        if (wallTiles == null) {
            wallTiles = GameObject.FindGameObjectWithTag("tiles_W").GetComponent<Tilemap>();
        }
        if (uniqueTiles == null) {
            uniqueTiles = GameObject.FindGameObjectWithTag("tiles_UT").GetComponent<Tilemap>();
        }
        if (LVM == null) {
            LVM = GameObject.FindGameObjectWithTag("LVM").GetComponent<levelManager>();
        }
    }

    private void Update() {
        if (LVM.clownActive) {
            return;
        }
        ProcessInputs();
    }
    
    private void ProcessInputs() {
        if (Input.GetKeyDown(KeyCode.Q)) {//R toggles us into slowmode
            slowMode = !slowMode;
            arrowInputManager.toggleSlowMode(slowMode);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (travelling) {
                Debug.Log("Tried to ready the ball, but ball was already moving");
                //play an SFX that says "Hey we're still moving, you can't ready up yet!
                return;
            }
            readyToRoll = !readyToRoll;//invert the ready check
            //todo play a sound right here based on the outcome!
            if (readyToRoll) {
                //do ready the sound
                arrowInputManager.toggleChildren(true);
                Debug.Log("Ball is readied");

                return;
            } else {
                //do the negation sound
                arrowInputManager.toggleChildren(false);
                Debug.Log("Ball is unreadied");
                return;
            }
        }

        if (!readyToRoll || travelling) {
            //do the negation sound
            //Debug.Log("Ball was travelling or unreadied and thus could not receive a new heading");
            return;
        }
        
    }
    public void initialMove(Vector2 direction, bool slowmode) {
        if (readyToRoll && !travelling) {
            if (slowmode) {
                tilesToTravel = slowsAt;
                goingFast = false;
                Debug.Log("Bowling ball initial goin' slow");
            }
            if (!slowmode) {
                tilesToTravel = baseTileSpeed;
                goingFast = true;
                Debug.Log("Bowling ball initial goin' fast AF boi");
            }
            arrowInputManager.toggleChildren(false);
            StartCoroutine(MoveWaitFor(moveDelay));
        }
        else {
            Debug.LogWarning("Something went wrong with the initial move method");
        }
    }
    private void LateUpdate() {
        if (travelling && !waitingForMove) {
            StartCoroutine(MoveWaitFor(moveDelay));
        }
    }


    public void doMove(Vector2 direction) {//this is for moving one tile!
        readyToRoll = false;
        travelling = true;
        if(tilesToTravel == slowsAt || tilesToTravel <= 0) {//why the second check? Well goingFast can get overriden by unique tiles
            goingFast = false;
            Debug.Log("Ball slowed down, it past the slowtile treshold");
            if (tilesToTravel <= 0) {//reset the tiles to travel if we're in the negatives
                tilesToTravel = 0;
                travelling = false;
                return;//also stop the ball!
            }
        }
        if (CanMove(direction)) {//check if we can actually move onto the next tile in the first place!
            Debug.Log("trying to translate ball");
            Instantiate(ghostball, transform.position, Quaternion.identity);//create a ghost marker of where we went
            transform.Translate((Vector3)direction * 2);//move towards where we're going, multiply by 2 because our gridsize is 2
            CheckRollOver();
        }
    }

    private void CheckRollOver() {
        Vector3Int gridposition = underTiles.WorldToCell(transform.position);//get the tile that we're on
        if (underTiles.HasTile(gridposition)) {
            tilesToTravel -= 1;
        }
        if (uniqueTiles.HasTile(gridposition)) {
            UTM.getUniqueTileAction(gridposition);
        }
        
    }
    private bool DebugCanMove(Vector2 direction) {
        Vector3Int gridposition = underTiles.WorldToCell(transform.position + (Vector3)direction * 2);
        if (wallTiles.HasTile(gridposition)) {
            Debug.Log("Ball is adjacent to a wall");
            return false;
        }
        return true;
    }
    private bool CanMove(Vector2 direction) {
        Vector3Int gridPosition = underTiles.WorldToCell(transform.position + (Vector3)direction * 2);//this math is bad, and breaks when moving south or west

        if (uniqueTiles.HasTile(gridPosition)) {
            Debug.Log("Ball encountered a unique tile!");
            return true;//we want to pass this early since the unique tiles are on the same 'layer' as the undertiles, but would cause it to error out if we didn't return early, since there's no undertile there!
        }

        if (!underTiles.HasTile(gridPosition) || wallTiles.HasTile(gridPosition)) {
            travelling = false;
            readyToRoll = false;
            tilesToTravel = 0;
            Debug.Log("Ball hit a wall and stopped");
            return false;
        }
        return true;
    }
    IEnumerator MoveWaitFor(float T) {
        waitingForMove = true;
        Debug.LogWarning("Doing the movewait for routine");
        if (!goingFast || slowMode) {
            yield return new WaitForSeconds(T*3);
        } else {
            yield return new WaitForSeconds(T);
        }
        doMove(heading);
        waitingForMove = false;
        yield return null;
    }
}
