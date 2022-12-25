using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PropPlacementManager : MonoBehaviour
{
    public float propPlacementChance;

    [SerializeField]
    private List<Prop> propsToPlace;

    [SerializeField]
    private GameObject propPrefab;

    public UnityEvent OnFinished;

    Room room;
    int once = 0;

    private void Awake()
    {
        room = FindObjectOfType<Room>();
    }

    public void ProcessRooms()
    {
        if (room == null)
            return;

        PlaceProps(room, propsToPlace);
    }

    private void PlaceProps(Room room, List<Prop> propsToPlace)
    {
        
        float tempChance = propPlacementChance;

        System.Random random = new System.Random();
        Vector2Int Tile = room.FloorTiles.ElementAt(random.Next(room.FloorTiles.Count));

        PlacePropGameObjectAt(room, Tile, propsToPlace[0]); //Temporary

        /* foreach(Vector2Int Tile in room.FloorTiles)
         {
             if(UnityEngine.Random.value < propPlacementChance && (once == 0))
             {
                 Prop propToPlace = propsToPlace[UnityEngine.Random.Range(0, propsToPlace.Count)];
                 PlacePropGameObjectAt(room, Tile, propToPlace);

                 tempChance = Mathf.Clamp01(tempChance+0.001f);
                 once = 1;
             }
         }*/
        OnFinished?.Invoke();
    }

    private GameObject PlacePropGameObjectAt(Room room, Vector2Int placementPosition, Prop propToPlace)
    {
        GameObject prop = Instantiate(propPrefab);
        SpriteRenderer propSpriteRenderer = prop.GetComponentInChildren<SpriteRenderer>();

        propSpriteRenderer.sprite = propToPlace.PropSprite;

        CapsuleCollider2D collider = propSpriteRenderer.gameObject.AddComponent<CapsuleCollider2D>();
        collider.offset = Vector2.zero;
        if(propToPlace.PropSize.x > propToPlace.PropSize.y)
        {
            collider.direction = CapsuleDirection2D.Horizontal;
        }
        collider.isTrigger = true;

        Vector2 size = new Vector2(propToPlace.PropSize.x * 0.8f,
            propToPlace.PropSize.y * 0.8f);
        collider.size = size;

        prop.transform.localPosition = (Vector2)placementPosition;

        propSpriteRenderer.transform.localPosition =
            (Vector2)propToPlace.PropSize * 0.5f;

        return prop;
    }
}
