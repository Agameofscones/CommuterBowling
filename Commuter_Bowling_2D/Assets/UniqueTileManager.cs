using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UniqueTileManager : MonoBehaviour
{

    private Vector2 NORTH = new Vector2(0, 1);//these should be defined somewhere else, and probably staticly accesible
    private Vector2 SOUTH = new Vector2(0, -1);
    private Vector2 EAST = new Vector2(1, 0);
    private Vector2 WEST = new Vector2(-1, 0);

    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private List<uniqueTileData> uniqueTiles;

    private Dictionary<TileBase, uniqueTileData> dataFromTiles;

    public player_bowling_ball bowlingBall;//our ball that we're going to be relaying information to

    private void Awake() {
        dataFromTiles = new Dictionary<TileBase, uniqueTileData>();
        foreach (var uniqueTileData in uniqueTiles) {
            foreach(var tile in uniqueTileData.tileArray) {
                dataFromTiles.Add(tile, uniqueTileData);
            }
        }
    }
    private void Start() {
        if(bowlingBall == null) {
            bowlingBall = GameObject.FindGameObjectWithTag("bowlingBall").GetComponent<player_bowling_ball>();
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridposition = map.WorldToCell(mousePosition);

            TileBase _clickedTile = map.GetTile(gridposition);

            if(_clickedTile == null) {
                Debug.LogWarning("Clicked a null tile");
                return;
            }
            float bonusMoves = dataFromTiles[_clickedTile].additionalMoves;

            Debug.Log("Clicked tile at " + gridposition + " It was: " + _clickedTile + "It grants: " + bonusMoves);
        }
    }   

    public void getUniqueTileAction(Vector3Int worldPosition) {

        TileBase _Tile = map.GetTile(worldPosition);
        Debug.Log("Got the unique action of tile at " + worldPosition + " It was: " + _Tile);

        //do dictionary stuff here!
        bowlingBall.tilesToTravel += dataFromTiles[_Tile].additionalMoves;//add or remove additional moves from the bowling ball if they hit a tile of that type


        if (dataFromTiles[_Tile].deadStopsBall) {
            bowlingBall.tilesToTravel = 0;
            return;
        }
        if (dataFromTiles[_Tile].slowBall) {//slow the ball if we hit a slow tile
            bowlingBall.goingFast = false;
        }
        if (dataFromTiles[_Tile].speedBall) {//speed up the ball if it hits a speed tile
            bowlingBall.goingFast = true;
        }
        if (dataFromTiles[_Tile].invertsBall) {
            Vector2 newHeading = bowlingBall.heading * -1;
            bowlingBall.tilesToTravel += 1;//we lose one movement point because we 'enter' the tile of the bounce wall. By giving back 1 point we get the 'bounce' even at 2 move points
            bowlingBall.heading = newHeading;
        }
        if (dataFromTiles[_Tile].redirectsBall) {
            Vector2 newHeading = bowlingBall.heading - dataFromTiles[_Tile].angleDirection;
            bowlingBall.heading = newHeading;
            Debug.Log("changed balls heading to " + newHeading);
        }
    }
}
