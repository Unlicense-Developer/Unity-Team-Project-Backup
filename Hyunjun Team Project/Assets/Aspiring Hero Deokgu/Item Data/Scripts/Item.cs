using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Potion, Equipment, Etc
}

[CreateAssetMenu(fileName = "New Item",menuName ="Item/Create New Item")]
public class Item : ScriptableObject
{
    public ItemType type;
    public string itemName;
    public int value;
    public int atk;
    public int def;
    public Sprite icon;
    public string itemInfo;
}
