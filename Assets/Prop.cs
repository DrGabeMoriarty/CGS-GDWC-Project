using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prop", menuName = "ScriptableObjects/Prop", order = 1)]
public class Prop : ScriptableObject
{
    [Header("Prop data:")]
    public Sprite PropSprite;

    public Vector2Int PropSize = Vector2Int.one;
    
}
