using UnityEngine;

//Permet de créer des instances d'ItemData
[CreateAssetMenu(fileName = "item", menuName = "item/New item")]
public class ItemData : ScriptableObject
{
    [SerializeField]
    protected string nameItem;
    [SerializeField]
    protected string description;
    [SerializeField]
    protected ItemType type;
    [SerializeField]
    protected Sprite visuel;
    [SerializeField]
    protected GameObject prefab;
    [SerializeField]
    protected bool stackable;
    [SerializeField]
    protected int weight;
    [SerializeField]
    protected int sellPrice;
    [SerializeField]
    protected int buyingPrice;

    /*
    [SerializeField] private string guid;
    public string Guid => guid;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(guid))
        {
            guid = System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }*/

    public string GetName() {  return nameItem; }
    public string GetDescription() { return description; }
    public ItemType GetItemType() { return type; }
    public Sprite GetVisuel() { return visuel; }
    public GameObject GetPrefab() { return prefab; }
    public bool IsStackable() { return stackable; }
    public int GetPrice() { return sellPrice; }
    public int GetWeight() { return weight; }
    public int GetBuyingPrice() { return buyingPrice; }

}

public enum ItemType
{
    Tool,
    Ressource,
    Seed,
    Sappling,
    Backpack
}