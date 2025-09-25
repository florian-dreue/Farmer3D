using UnityEngine;

//Permet de créer des instances d'ItemData
[CreateAssetMenu(fileName = "item", menuName = "item/New item")]
public class ItemData : ScriptableObject
{
    public string nameItem;
    public string description;
    public ItemType type;
    public Sprite visuel;
    public GameObject prefab;
    public bool stackable;
    public int weight;
    public int price;
    public SeedData seed;
    public SapplingData sappling;
    public int filling = 0;
}

public enum ItemType
{
    Tool,
    Ressource,
    Seed,
    Sappling
}