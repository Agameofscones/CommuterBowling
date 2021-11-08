using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class dirArrowClown : MonoBehaviour
{
    [SerializeField]
    private dirArrowManagerClown objectManager;
    [SerializeField]
    private Sprite clownState;
    [SerializeField]
    private Sprite ballState;

    private Renderer rendo;
    private SpriteRenderer sprendo;
    private bool scaled = false;
    [SerializeField]
    private Vector2 dir;

    public bool ballMode = false;
    public bool touchingCollidable = false;

    [SerializeField]
    private Tilemap underTiles;//to-do, retrieve these if nulled
    [SerializeField]
    private Tilemap wallTiles;
    [SerializeField]
    private Tilemap uniqueTiles;

    public playerClown pClown;

    private void Awake() {//this is some shitty copy-paste code, good god.
        if (underTiles == null) {
            underTiles = GameObject.FindGameObjectWithTag("tiles_UNDER").GetComponent<Tilemap>();
        }
        if (wallTiles == null) {
            wallTiles = GameObject.FindGameObjectWithTag("tiles_W").GetComponent<Tilemap>();
        }
        if (uniqueTiles == null) {
            uniqueTiles = GameObject.FindGameObjectWithTag("tiles_UT").GetComponent<Tilemap>();
        }
        if (rendo == null) {
            rendo = gameObject.GetComponent<Renderer>();
        }
        if (objectManager == null) {
            objectManager = GetComponentInParent<dirArrowManagerClown>();//this'll grab our reference if we forget to set it
        }
        if (sprendo == null) {
            sprendo = gameObject.GetComponent<SpriteRenderer>();
        }
        if (pClown == null) {
            pClown = GameObject.FindGameObjectWithTag("Player").GetComponent<playerClown>();
        }
        rendo.enabled = false;//hide use til we're ready
    }
    private void OnEnable() {
        checkForValidPlacement(dir);
        updateSprite();
    }
    public void updateSprite() {
        if (ballMode) {
            sprendo.sprite = ballState;
        }
        else {
            sprendo.sprite = clownState;
        }
    }
    private void checkForValidPlacement(Vector2 direction) {
        Debug.LogWarning("checking for CLOWN ADJACENT dir");
        if (touchingCollidable) {
            gameObject.SetActive(false);
            return;
        }
        Vector3Int gridPosition = underTiles.WorldToCell(objectManager.GetComponentInParent<Transform>().position + (Vector3)direction * 2);
        if (!underTiles.HasTile(gridPosition) || wallTiles.HasTile(gridPosition) || uniqueTiles.HasTile(gridPosition)) {//we cant move onto unique tiles either
            gameObject.SetActive(false);
            return;
        }
        rendo.enabled = true;//since we passed the placement check, show us to the world!
    }
    private void OnMouseOver() {
        if (!scaled) {
            transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            scaled = true;
        }
    }
    private void OnMouseExit() {
        scaled = false;
        transform.localScale = new Vector3(1, 1, 1);
    }
    private void OnMouseDown() {
        pClown.dir = dir;
        pClown.moveOrSpawn(ballMode);//spawns the bowling ball or moves the clown
        objectManager.toggleChildren(false);
        objectManager.arrowsOut = false;
    }
}
