using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "backpack", menuName = "item/New backpack")]
public class BackpackData : ItemData
{
    [SerializeField]
    private int capacity;
    [SerializeField]
    private int maxWeight;
    private int actualWeight = 0;
    private List<ItemInInventory> content = new List<ItemInInventory>();
    [SerializeField]
    private int maxSize = 5;

    BackpackData()
    {
        type = ItemType.Backpack;
        stackable = false;
        sellPrice = 0;
        weight = 0;
    }

    public void AddItem(ItemData item)
    {
        //Si l'objet est une ressource on l'ajoute à l'inventaire normal
        if (item.GetItemType() == ItemType.Ressource)
        {
            //On cherche si cet objet est déjà présent dans l'inventaire
            ItemInInventory itemInInventory = content.Where(element => element.itemData == item).FirstOrDefault();

            //Si l'objet est présent et qu'il est stackable on incrémente le nombre d'objet et on met à jour le poids
            if (itemInInventory != null && item.IsStackable())
            {
                itemInInventory.count++;
                actualWeight += item.GetWeight();
            }
            //Sinon, on l'ajoute dans un nouvel espace
            else
            {
                content.Add(new ItemInInventory { itemData = item, count = 1 });
                actualWeight += item.GetWeight();
            }
        }
    }

    public void SubstractItem(ItemData item, int amount)
    {
        if (item.GetItemType() == ItemType.Ressource)
        {
            ItemInInventory itemInInventory = content.Where(element => element.itemData == item).FirstOrDefault();

            if (itemInInventory.count > amount)
            {
                itemInInventory.count -= amount;
            }
            else
            {
                content.Remove(itemInInventory);
            }
        }
    }

    public bool HaveSpace(ItemData item)
    {
        //Si c'est une ressource, on peut le stocker
        if (item.GetItemType() == ItemType.Ressource)
        {
            ItemInInventory itemInInventory = content.Where(element => element.itemData == item).FirstOrDefault();

            //Si l'objet est présent et qu'il est stackable
            if (itemInInventory != null && item.IsStackable())
            {
                //On regarde si le futur poids n'est pas au dessus de la capacité du joueur
                if (actualWeight + item.GetWeight() <= maxWeight)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //S'il n'est pas présent on regarde s'il y a assez de poids disponible et assez de slots disponible
            else
            {
                if (actualWeight + item.GetWeight() <= maxWeight && content.Count + 1 <= maxSize)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
    }

    public bool AlreadyInBackpack(ItemData item)
    {
        ItemInInventory existInContent = content.Find(x => x.itemData == item);
        if (existInContent != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<ItemInInventory> GetContent()
    {
        return content;
    }

    public int GetSize()
    {
        return capacity;
    }

    public BackpackSaveData ToSavable()
    {
        return new BackpackSaveData
        {
            capacity = capacity,
            maxWeight = maxWeight,
            actualWeight = actualWeight,
            content = content,
            maxSize = maxSize
        };
    }

    public void LoadFromSave(BackpackSaveData saveData)
    {
        this.capacity = saveData.capacity;
        this.maxWeight = saveData.maxWeight;
        this.actualWeight = saveData.actualWeight;
        this.content = new List<ItemInInventory>(saveData.content);
        this.maxSize = saveData.maxSize;
    }
}

[System.Serializable]
public class BackpackSaveData
{
    public int capacity;
    public int maxWeight;
    public int actualWeight;
    public List<ItemInInventory> content;
    public int maxSize;
}
