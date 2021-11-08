    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class uniqueTileData : ScriptableObject
{
    public TileBase[] tileArray;

    public int additionalMoves;
    public bool speedBall;
    public bool slowBall;
    public bool redirectsBall;
    public bool invertsBall;

    public bool deadStopsBall;

    public Vector2 angleDirection;
}
