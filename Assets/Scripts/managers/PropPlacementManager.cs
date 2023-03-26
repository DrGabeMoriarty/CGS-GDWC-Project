using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PropPlacementManager : MonoBehaviour
{
    [SerializeField, Range(0, 1)]
    private float cornerPropPlacementChance = 0.7f;

    [SerializeField] private Prop key;
    [SerializeField] private List<Prop> propsToPlace;

    [SerializeField]
    private GameObject Chest; //0
    [SerializeField] private GameObject DecorObject; //1
    [SerializeField] private GameObject Key; //2

    [SerializeField] private float colliderSize = 0.8f;
    [SerializeField] private int groupsMaxCount = 8;

    public UnityEvent OnFinished;

    DungeonData dungeonData;

    private void Awake()
    {
        dungeonData = FindObjectOfType<DungeonData>();
    }

    public void ProcessRooms()
    {
        if (dungeonData == null)
            return;

        //Place the unique key for the map
        Room keyRoom = dungeonData.Rooms[UnityEngine.Random.Range(2, dungeonData.Rooms.Count)];
        List<Prop> keyls = new List<Prop>();
        keyls.Add(key);
        PlaceProps(keyRoom, keyls, keyRoom.InnerTiles, PlacementOriginCorner.BottomLeft);

        //Others
        foreach (Room room in dungeonData.Rooms)
        {
            List<Prop> cornerProps = propsToPlace.Where(x => x.Corner).ToList();
            PlaceCornerProps(room, cornerProps);

            List<Prop> leftWallProps = propsToPlace
                .Where(x => x.NearWallLeft)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y)
                .ToList();

            PlaceProps(room,leftWallProps,room.NearWallTilesLeft,PlacementOriginCorner.BottomLeft);

            List<Prop> rightWallProps = propsToPlace
                .Where(x => x.NearWallRight)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y)
                .ToList();

            PlaceProps(room, rightWallProps, room.NearWallTilesRight, PlacementOriginCorner.TopRight);

            List<Prop> topWallProps = propsToPlace
                .Where(x => x.NearWallUp)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y)
                .ToList();

            PlaceProps(room, topWallProps, room.NearWallTilesUp, PlacementOriginCorner.TopLeft);

            List<Prop> downWallProps = propsToPlace
                .Where(x => x.NearWallDown)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y)
                .ToList();

            PlaceProps(room, downWallProps, room.NearWallTilesDown, PlacementOriginCorner.BottomLeft);

            List<Prop> innerProps = propsToPlace
                 .Where(x => x.Inner)
                 .OrderByDescending(x => x.PropSize.x * x.PropSize.y)
                 .ToList();
            
            PlaceProps(room, innerProps, room.InnerTiles, PlacementOriginCorner.BottomLeft);
        }
        OnFinished?.Invoke();
    }

    private void PlaceCornerProps(Room room, List<Prop> cornerProps)
    {
        if (cornerProps.Count == 0) return;

        float tempChance = cornerPropPlacementChance;
        HashSet<Vector2Int> tempPositions = new HashSet<Vector2Int>(room.CornerTiles);
        tempPositions.ExceptWith(dungeonData.Path);

        foreach (Vector2Int cornerTile in tempPositions)
        {
            int i = UnityEngine.Random.Range(0, cornerProps.Count);
            if (UnityEngine.Random.value < tempChance)
            {
                Prop propToPlace = cornerProps[i];

                PlacePropGameObjectAt(room, cornerTile, propToPlace);
                cornerProps.Remove(cornerProps[i]);

                if (propToPlace.PlaceAsGroup)
                {
                    PlaceGroupObject(room, cornerTile, propToPlace, 2);
                    cornerProps.Remove(cornerProps[i]);
                }
                break;
            }
            else
            {
                tempChance = Mathf.Clamp01(tempChance + 0.1f);
            }
        }
    }

    private void PlaceGroupObject(Room room, Vector2Int groupOriginPosition, Prop propToPlace, int searchOffset)
    {
        int count = UnityEngine.Random.Range(propToPlace.GroupMinCount, propToPlace.GroupMaxCount) - 1;
        count = Mathf.Clamp(count, 0, groupsMaxCount);

        List<Vector2Int> availableSpaces = new List<Vector2Int>();
        for (int xOffset = -searchOffset; xOffset <= searchOffset; xOffset++)
        {
            for (int yOffset = -searchOffset; yOffset <= searchOffset; yOffset++)
            {
                Vector2Int tempPos = groupOriginPosition + new Vector2Int(xOffset, yOffset);
                if(room.FloorTiles.Contains(tempPos) &&
                    !dungeonData.Path.Contains(tempPos) &&
                    !room.PropPositions.Contains(tempPos))
                {
                    availableSpaces.Add(tempPos);
                }
            }
        }

        availableSpaces.OrderBy(x => Guid.NewGuid());

        int tempCount = count < availableSpaces.Count ? count : availableSpaces.Count;
        for (int i = 0; i < tempCount; i++)
        {
            PlacePropGameObjectAt(room, availableSpaces[i], propToPlace);
        }
    }

    private void PlaceProps(Room room, List<Prop> wallProps,HashSet<Vector2Int> nearWallTiles,PlacementOriginCorner placement)
    {
        HashSet<Vector2Int> tempPositions = new HashSet<Vector2Int>(nearWallTiles);
        tempPositions.ExceptWith(dungeonData.Path);

        foreach (Prop propToPlace in wallProps)
        {
            int quantity = UnityEngine.Random.Range(propToPlace.PlacementQuantityMin, propToPlace.PlacementQuantityMax + 1);

            for (int i = 0; i < quantity; i++)
            {
                tempPositions.ExceptWith(room.PropPositions);

                List<Vector2Int> availablePositions = tempPositions.OrderBy(x => Guid.NewGuid()).ToList();

                if (TryPlacingPropBruteForce(room, propToPlace, availablePositions, placement) == false)
                    break;
            }
        }
    }

    private bool TryPlacingPropBruteForce(Room room, Prop propToPlace, List<Vector2Int> availablePositions, PlacementOriginCorner placement)
    {
        for (int i = 0; i < availablePositions.Count; i++)
        {
            Vector2Int position = availablePositions[i];
            if (room.PropPositions.Contains(position))
                continue;

            List<Vector2Int> freePositionsAround
                = TryToFitProp(propToPlace, availablePositions, position, placement);
            
            if(freePositionsAround.Count == propToPlace.PropSize.x * propToPlace.PropSize.y)
            {
                PlacePropGameObjectAt(room, position, propToPlace);

                foreach (Vector2Int pos in freePositionsAround)
                {
                    room.PropPositions.Add(pos);
                }

                if (propToPlace.PlaceAsGroup)
                {
                    PlaceGroupObject(room, position, propToPlace, 1);
                }
                return true;
            }
        }
        return false;
    }

    private List<Vector2Int> TryToFitProp(Prop prop, List<Vector2Int> availablePositions, Vector2Int originPosition, PlacementOriginCorner placement)
    {
        List<Vector2Int> freePositions = new List<Vector2Int>();

        if (placement == PlacementOriginCorner.BottomLeft)
        {
            for (int xOffset = 0; xOffset < prop.PropSize.x; xOffset++)
            {
                for (int yOffset = 0; yOffset < prop.PropSize.y; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                    if (availablePositions.Contains(tempPos))
                        freePositions.Add(tempPos);
                }
            }
        }
        else if (placement == PlacementOriginCorner.BottomRight)
        {
            for (int xOffset = -prop.PropSize.x +1; xOffset <= 0; xOffset++)
            {
                for (int yOffset = 0; yOffset < prop.PropSize.y; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                    if (availablePositions.Contains(tempPos))
                        freePositions.Add(tempPos);
                }
            }
        }
        else if(placement == PlacementOriginCorner.TopLeft)
        {
            for (int xOffset = 0; xOffset < prop.PropSize.x; xOffset++)
            {
                for (int yOffset = -prop.PropSize.y + 1; yOffset <= 0; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                    if (availablePositions.Contains(tempPos))
                        freePositions.Add(tempPos);
                }
            }
        }
        else if(placement == PlacementOriginCorner.TopRight)
        {
            for (int xOffset = -prop.PropSize.x + 1; xOffset <= 0; xOffset++)
            {
                for (int yOffset = -prop.PropSize.y + 1; yOffset <=  0; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                    if (availablePositions.Contains(tempPos))
                        freePositions.Add(tempPos);
                }
            }
        }
        return freePositions;
    }

    private GameObject PlacePropGameObjectAt(Room room, Vector2Int placementPosition, Prop propToPlace)
    {

        GameObject prop = null;

        if(propToPlace.propType == 0)
        {
            prop = Instantiate(Chest);
        }
        else if (propToPlace.propType == 1)
        {
            prop = Instantiate(DecorObject);
        }
        else if (propToPlace.propType == 2)
        {
            prop = Instantiate(Key);
        }

        SpriteRenderer propSpriteRenderer = prop.GetComponentInChildren<SpriteRenderer>();

        propSpriteRenderer.sprite = propToPlace.PropSprite;

        if (propToPlace.propType != 0)
        {
            CapsuleCollider2D collider = propSpriteRenderer.gameObject.AddComponent<CapsuleCollider2D>();
            collider.offset = Vector2.zero;
            if (propToPlace.PropSize.x > propToPlace.PropSize.y)
            {
                collider.direction = CapsuleDirection2D.Horizontal;
            }

            if (propToPlace.propType == 2)
                collider.isTrigger = true;

            Vector2 size = new Vector2(propToPlace.PropSize.x * colliderSize,
                propToPlace.PropSize.y * colliderSize);
            collider.size = size;
        }

        prop.transform.localPosition = (Vector2)placementPosition;

        propSpriteRenderer.transform.localPosition =
            (Vector2)propToPlace.PropSize * 0.5f;

        for (int i = 0; i < propToPlace.PropSize.x; i++)
        {
            room.PropPositions.Add(placementPosition + Vector2Int.right * (i - 1));
        }
        for (int i = 0; i < propToPlace.PropSize.y; i++)
        {
            room.PropPositions.Add(placementPosition + Vector2Int.up * (i - 1));
        }

        room.PropObjectReferences.Add(prop);

        return prop;
    }
}

public enum PlacementOriginCorner
{
    BottomLeft,
    BottomRight,
    TopLeft,
    TopRight
}
