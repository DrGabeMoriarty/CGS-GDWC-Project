using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap, wallTilemap;
    
    [Header("Walls")]
    [SerializeField] private TileBase wallTop;
    [SerializeField] private TileBase wallSideRight;
    [SerializeField] private TileBase wallSideLeft;
    [SerializeField] private TileBase wallBottom;
    [SerializeField] private TileBase wallFull;
    [SerializeField] private TileBase wallInnerCornerDownLeft;
    [SerializeField] private TileBase wallInnerCornerDownRight;
    [SerializeField] private TileBase wallDiagonalCornerDownRight;
    [SerializeField] private TileBase wallDiagonalCornerDownLeft;
    [SerializeField] private TileBase wallDiagonalCornerUpRight;
    [SerializeField] private TileBase wallDiagonalCornerUpLeft;


    [Header("Floor")]
    [SerializeField] private TileBase[] floorTiles;
    [SerializeField] private float[] floorTileweights;
    
    public void PaintfloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
       PaintTiles(floorPositions, floorTilemap, floorTiles,floorTileweights);
    }

    private TileBase GetRandomTile(TileBase[] floorTiles,float[] weights, System.Random random)
    {
        //TileBase selectedTile = floorTiles.First().Key; //default

        float total = weights.Sum();
        float num = (float)((float) total*random.NextDouble());
        float cumulative_sum = 1;
        
        for(int i = 0;i< weights.Length;i++)
        {
            if(cumulative_sum - weights[i] <= num) return floorTiles[i];
            else cumulative_sum -= weights[i];
        }
        
        return null;
    }

    internal void PaintSingleBasicWall(Vector2Int position,string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType,2);
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        if (tile != null) PaintSingleTile(wallTilemap, tile, position);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase[] tiles,float[] tileWeights)
    {
        System.Random random = new System.Random();
        

        foreach (var position in positions)
        {
            TileBase tile = GetRandomTile(tiles, tileWeights, random);
            PaintSingleTile(tilemap, tile, position);
        }
    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType,2);
        TileBase tile = null;

        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt)) tile = wallInnerCornerDownLeft;
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt)) tile = wallInnerCornerDownRight;
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt)) tile = wallInnerCornerDownRight;
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt)) tile = wallDiagonalCornerDownLeft;
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt)) tile = wallDiagonalCornerDownRight;
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt)) tile = wallDiagonalCornerUpLeft;
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt)) tile = wallDiagonalCornerUpRight;
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt)) tile = wallFull;
        else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt)) tile = wallBottom;
        if (tile != null) PaintSingleTile(wallTilemap,tile,position);

    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
}
