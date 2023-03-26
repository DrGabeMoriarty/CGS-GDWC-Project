using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prop", menuName = "ScriptableObjects/Prop", order = 1)]
public class Prop : ScriptableObject
{
    [Header("Prop data:")]
    public Sprite PropSprite;
    [Range(0,2)]
    public int propType = 1;

    public Vector2Int PropSize = Vector2Int.one;

    [Space, Header("Placement type:")]
    public bool Corner = false;
    public bool NearWallUp = false;
    public bool NearWallDown = false;
    public bool NearWallLeft = false;
    public bool NearWallRight = false;
    public bool Inner = true;

    [Min(1)]
    public int PlacementQuantityMin = 1;
    [Min(1)]
    public int PlacementQuantityMax = 1;

    [Space, Header("Group Placement:")]
    public bool PlaceAsGroup = false;
    [Min(1)]
    public int GroupMinCount = 1;
    [Min(1)]
    public int GroupMaxCount = 1;
}
