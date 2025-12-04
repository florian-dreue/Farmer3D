using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System;

//Inventaire du joueur séparé en deux partis:
//La partie inventaire unique pour un outil ou un sac de graines
//La partie inventaire classique pour les récoltes
public class Inventory : MonoBehaviour
{
    private List<ItemInInventory> content = new List<ItemInInventory>();

    [SerializeField]
    private GameObject inventoryPanel;

    private ItemData toolEquipped;
    public ItemData toolRef;

    [SerializeField]
    private ToolSlot toolSlot;

    [SerializeField]
    private Transform inventorySlotsParent;

    [SerializeField]
    private TextMeshProUGUI moneyText;

    [SerializeField]
    private GameController gameController;

    private BackpackData backpack;
    public BackpackData backpackRef;

    const int maxSize = 5;
    const int maxWeight = 10000;
    private int actualWeight = 0;

    //Fonction pour l'initialisation de l'inventaire
    public void Start()
    {
        /*
        SaveInventoryManager.LoadJsonData(this);
        RefreshContent();*/
    }

    //Fonction pour l'ajout d'un objet à l'inventaire tel qu'il soit
    public void AddItem(Item itemPass)
    {
        ItemData item = itemPass.item;
        if (HaveSpaceInInventory(item))
        {
            //Si l'objet est une ressource on l'ajoute à l'inventaire normal
            if(item.GetItemType() == ItemType.Ressource)
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
            //Si c'est un sack à dos on l'ajoute au sac
            else if( item.GetItemType() == ItemType.Backpack)
            {
                backpack = item as BackpackData;
                backpackRef = itemPass.globalItem as BackpackData;
            }
            //Sinon on ajoute l'objet dans l'inventaire de l'outil
            else
            {
                var fillable = item as FillableData;
                if(fillable != null)
                {
                    toolEquipped = item;
                    toolRef = itemPass.globalItem as FillableData;
                }
                else
                {
                    toolEquipped = itemPass.globalItem;
                }
                
            }

            //On rafraichît le visuel de l'inventaire.
            RefreshContent();
        }
        else
        {
            backpack.AddItem(item);
        }
        
    }

    //Fonction pour la suppression d'un objet à l'inventaire
    public void RemoveItem(ItemData item)
    {
        if (item.GetItemType() == ItemType.Ressource)
        {
            ItemInInventory itemInInventory = content.Where(element => element.itemData == item).FirstOrDefault();

            if (itemInInventory.count > 1)
            {
                itemInInventory.count--;
            }
            else
            {
                content.Remove(itemInInventory);
            }
        }
        else if(item.GetItemType() != ItemType.Backpack)
        {
            toolEquipped = null;
        }
        else
        {
            backpack = null;
        }

        //On rafraichît le visuel de l'inventaire.
        RefreshContent();
    }

    //Fonction pour la soustraction d'un objet de l'inventaire
    public void SubstractItem(ItemData item, int amount)
    {
        if (item.GetItemType() == ItemType.Ressource)
        {
            ItemInInventory itemInInventory = content.Find(element => element.itemData == item);

            if(itemInInventory != null)
            {
                if (itemInInventory.count > amount)
                {
                    itemInInventory.count -= amount;
                }
                else
                {
                    content.Remove(itemInInventory);
                }
            }
            else
            {
                backpack.SubstractItem(item, amount);
            }
        }

            
        else
        {
            toolEquipped = null;
        }

        //On rafraichît le visuel de l'inventaire.
        RefreshContent();
    }

    //Fonction permettant de récupérer le contenu de l'inventaire
    public List<ItemInInventory> GetContent()
    {
        return content;
    }

    //Fonction permettant la mise à jour à chaque Frame
    public void Update()
    {
        //Si on appuie sur la touche I on affiche ou cache la barre d'inventaire
        /*
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
        */
    }

    //Fonction permettant de mettre à jour le visuel de l'inventaire
    public void RefreshContent()
    {
        //On boucle sur chaque slot de l'inventaire et on remet l'affichage par défaut
        for (int i = 0; i < inventorySlotsParent.childCount; i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();

            currentSlot.EmptySlot();
        }
        //On met l'affichage par défaut sur l'inventaire d'outil 
        toolSlot.EmptySlot();

        //On boucle sur le contenu de l'inventaire
        for (int i = 0; i < content.Count; i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();

            if (content[i] != null)
            {
                //On ajoute les données nécessaire à l'affichage
                currentSlot.SetSlot(content[i].itemData);

                //Si l'objet est stackable on affiche également le compteur
                if (currentSlot.ItemStackable())
                {
                    currentSlot.SetText(content[i].count.ToString());
                }
                //inventorySlotsParent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = content[i].itemData.visuel;
            }
        }

        //Si on as un outil équipé, on change l'affichage
        if (toolEquipped)
        {
            toolSlot.setItem(toolEquipped);
        }
    }

    //Fonction permettant de vérifier si on as de la place dans l'inventaire ou dans le sac
    public bool HaveSpace(ItemData item)
    {
        //Si c'est une ressource, on regarde dans l'inventaire normal
        if(item.GetItemType() == ItemType.Ressource)
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
                    if(backpack && backpack.HaveSpace(item))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //S'il n'est pas dans l'inventaire on regarde s'il est déjà dans le sac à dos
            else if (backpack != null && AlreadyInBackpack(item) && backpack.HaveSpace(item) )
            {
                return true;
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
                    if (backpack && backpack.HaveSpace(item))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        //Sinon on regarde si c'est un sack à dos
        else if(item.GetItemType() == ItemType.Backpack)
        {
            if (!backpack)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Sinon on regarde l'inventaire d'équippement
        else
        {
            //Si on as pas déjà d'outil et que le joueur peut le porter on retourne true
            if (!toolEquipped && actualWeight + item.GetWeight() <= maxWeight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }

    //Fonction permettant de vérifier si on as de la place uniquement dans l'inventaire
    public bool HaveSpaceInInventory(ItemData item)
    {
        //Si c'est une ressource, on regarde dans l'inventaire normal
        if (item.GetItemType() == ItemType.Ressource)
        {
            if (!AlreadyInBackpack(item))
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
        //Sinon on regarde si c'est un sack à dos
        else if (item.GetItemType() == ItemType.Backpack)
        {
            if (!backpack)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Sinon on regarde l'inventaire d'équippement
        else
        {
            //Si on as pas déjà d'outil et que le joueur peut le porter on retourne true
            if (!toolEquipped && actualWeight + item.GetWeight() <= maxWeight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }


    //Fonction permettant de vendre l'intégralité de l'inventaire
    public void Sell()
    {
        gameController.AddMoney(GetSellAmount());
        this.content.Clear();
        RefreshContent();
    }

    //Fonction permettant de récupérer le montant total de l'inventaire
    public int GetSellAmount()
    {
        int price = 0;
        foreach (ItemInInventory itemInInventory in this.content)
        {
            price += itemInInventory.itemData.GetPrice() * itemInInventory.count;
        }

        return price;
    }

    //Fonction permettant de récupérer le poids total de l'inventaire
    public int GetWeight()
    {
        return actualWeight;
    }

    //Fonction permettant de récupérer l'outil équipé
    public ItemData GetToolEquipped()
    {
        return toolEquipped;
    }

    public void FillTool()
    {
        FillableData fillable = toolEquipped as FillableData;
        fillable.FillTool(100);
        toolSlot.updateItem(fillable);
    }

    public void DrainTool(int purcentDrain)
    {
        FillableData fillable = toolEquipped as FillableData;
        fillable.DrainTool(purcentDrain);
        toolSlot.updateItem(fillable);
    }

    public int GetToolCapicity()
    {
        FillableData fillable = toolEquipped as FillableData;
        return fillable.GetFilling();
    }

    public void EmptyTool()
    {
        toolEquipped = null;
        toolRef = null;
        toolSlot.EmptySlot();
    }

    public void EmptyBackpack()
    {
        backpack = null;
    }

    public List<ItemInInventory> GetBackpackContent()
    {
        if(backpack != null)
        {
            return backpack.GetContent();
        }
        else
        {
            return new List<ItemInInventory>();
        }
    }

    public int GetBackpackSize()
    {
        if(backpack != null)
        {
            return backpack.GetSize();
        }
        else
        {
            return 0;
        }
    }

    public bool HaveBackpack()
    {
        return backpack != null;
    }

    public bool AlreadyInInventory(ItemData item)
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

    public bool AlreadyInBackpack(ItemData item)
    {
        if(backpack != null)
        {
            ItemInInventory existInContent = backpack.GetContent().Find(x => x.itemData == item);
            if (existInContent != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }



    //Fonction permettant de sauvegarder l'inventaire
    public void PopulateInventory(InventorySaveData saveContainer)
    {
        saveContainer.content = this.content;
        saveContainer.actualWeight = this.actualWeight;
        saveContainer.maxSize = maxSize;
        saveContainer.maxWeight = maxWeight;
        saveContainer.hasBackpack = this.backpack != null;
        saveContainer.backpack = this.backpack?.ToSavable();
        saveContainer.backpackRef = this.backpackRef;

        var fillable = toolEquipped as FillableData;

        Debug.Log("fillable: " + fillable);

        if (fillable != null)
        {
            saveContainer.toolEquipped = this.toolRef;
            saveContainer.fillQuantity = fillable.GetFilling();
        }
        else
        {
            saveContainer.toolEquipped = this.toolEquipped;
        }
    }

    //Fonction permettant de charger un inventaire
    public void LoadFromInventory(InventorySaveData inventorySaved)
    {
        this.content = inventorySaved.content;
        this.actualWeight = inventorySaved.actualWeight;

        if (inventorySaved.hasBackpack)
        {
            this.backpackRef = inventorySaved.backpackRef;
            this.backpack = ScriptableObject.Instantiate(backpackRef); // clone de l’asset
            this.backpack.LoadFromSave(inventorySaved.backpack);                 // appliquer l’état sauvegardé
        }

        var fillable = inventorySaved.toolEquipped as FillableData;

        if (fillable != null)
        {
            this.toolEquipped = ScriptableObject.Instantiate(inventorySaved.toolEquipped) as FillableData;
            this.toolRef = inventorySaved.toolEquipped;
            FillableData insatnciedFillable = toolEquipped as FillableData;
            insatnciedFillable.FillTool(inventorySaved.fillQuantity);
            toolSlot.updateItem(insatnciedFillable);
        }
        else
        {
            this.toolEquipped = inventorySaved.toolEquipped;
        }
    }
}

//Objet contenant l'item et le nombre d'item stocké
[System.Serializable]
public class ItemInInventory
{
    public ItemData itemData;
    public int count;
}

[System.Serializable]
public class InventorySaveData
{
    public List<ItemInInventory> content;
    public ItemData toolEquipped;
    public int fillQuantity;
    public int actualWeight;
    public int maxSize;
    public int maxWeight;
    public bool hasBackpack;
    public BackpackSaveData backpack;
    public BackpackData backpackRef;
}
