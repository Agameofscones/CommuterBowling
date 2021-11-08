using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class dirArrowButton : MonoBehaviour
{
    [SerializeField]//since these are (ideally) setup in the prefab, we won't need to actually use the get function in the awake function, and just start off with a reference.
    private dirArrowsManager managerObject;
    [SerializeField]
    private Sprite altState;
    [SerializeField]
    private Sprite defaultState;

    private Renderer rendo;
    private SpriteRenderer sprendo;
    private bool scaled = false;
    [SerializeField]
    private Vector2 dir;

    public bool slowMode = false;

    [SerializeField]
    private Tilemap underTiles;//to-do, retrieve these if nulled
    [SerializeField]
    private Tilemap wallTiles;

    private void Awake() {
        if(underTiles == null) {
            underTiles = GameObject.FindGameObjectWithTag("tiles_UNDER").GetComponent<Tilemap>();
        }
        if(wallTiles == null) {
            wallTiles = GameObject.FindGameObjectWithTag("tiles_W").GetComponent<Tilemap>();
        }
        if(rendo == null) {
            rendo = gameObject.GetComponent<Renderer>();
        }
        if (managerObject == null) {
            managerObject = GetComponentInParent<dirArrowsManager>();//this'll grab our reference if we forget to set it
        }
        if (sprendo == null) {
            sprendo = gameObject.GetComponent<SpriteRenderer>();
        }
        rendo.enabled = false;//hide use til we're ready
        
    }
    private void OnEnable() {
        checkForValidPlacement(dir);
        updateSprite();
    }
    public void updateSprite() {
        if (slowMode) {
            sprendo.sprite = altState;
        }
        else {
            sprendo.sprite = defaultState;
        }
    }
    private void checkForValidPlacement(Vector2 direction) {
        Debug.LogWarning("checking for new dir");
        Vector3Int gridPosition = underTiles.WorldToCell(managerObject.transform.position + (Vector3)direction * 2);
        if (!underTiles.HasTile(gridPosition) || wallTiles.HasTile(gridPosition)) {
            this.gameObject.SetActive(false);
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
        managerObject.returnableHeading = dir;//set our dir to the heading
        Debug.Log("Clicked on DIR arrow");
        managerObject.toggleChildren(false);
        managerObject.bowlingBall.heading = dir;
        if (slowMode) {
            managerObject.bowlingBall.initialMove(dir, true);
        } else {
            managerObject.bowlingBall.initialMove(dir, false);
        }
        
    }
}
