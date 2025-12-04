using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Classe permettant de sauvegarder les données de l'inventaire
public static class SaveInventoryManager
{
    // Fonction permettant de sauvegarder les données en JSON
    public static void SaveJsonData(Inventory inventory, List<HarvestableSaveData> harvestableList, List<TreeLandSaveData> treeLandList)
    {
        // Créer un conteneur de données
        InventorySaveData saveInventory = new InventorySaveData();

        inventory.PopulateInventory(saveInventory);

        List<ItemData> itemUnlock = MainManager.Instance.GetItemUnlock();

        SaveDatas saveDatas = new SaveDatas();
        saveDatas.inventory = saveInventory;
        saveDatas.harvestables = harvestableList;
        saveDatas.treeLands = treeLandList;
        saveDatas.itemsUnlock = itemUnlock;

        // Sérialiser et écrire
        string json = JsonUtility.ToJson(saveDatas);
        if (FileManager.WriteToFile("Inventory.dat", json))
        {
            Debug.Log("Save successful");
        }
    }

    // Fonction permettant de charger les données en JSON
    public static void LoadJsonData(Inventory inventory, List<Harvestable> harvestableList, List<TreeLand> treeLandList)
    {
        if (FileManager.LoadFromFile("Inventory.dat", out var json))
        {
            SaveDatas saveDatas = JsonUtility.FromJson<SaveDatas>(json);

            if (saveDatas != null)
            {
                InventorySaveData inventorySaved = saveDatas.inventory;

                inventory.LoadFromInventory(inventorySaved);
                inventory.RefreshContent();

                //Chargement des terres cultivables
                foreach (HarvestableSaveData harvestablePointer in saveDatas.harvestables)
                {
                    Harvestable harvestable = harvestableList.FirstOrDefault(h => h.UniqueId == harvestablePointer.uniqueId);

                    if (harvestable != null)
                    {
                        harvestable.LoadFromHarvestable(harvestablePointer);
                    }
                }

                //Chargement des terres d'arbres cultivables
                foreach (TreeLandSaveData treeLandPointer in saveDatas.treeLands)
                {
                    TreeLand treeLand = treeLandList.FirstOrDefault(tl => tl.UniqueId == treeLandPointer.uniqueId);

                    if (treeLand != null)
                    {
                        treeLand.LoadFromTreeLand(treeLandPointer);
                    }
                }

                //Chargement des objets achetés
                foreach (ItemData unlockItem in saveDatas.itemsUnlock)
                {
                    MainManager.Instance.AddItem(unlockItem);

                    if (unlockItem.GetItemType() == ItemType.Destroyable)
                    {
                        GameObject[] listeOfDestroyItem = GameObject.FindGameObjectsWithTag("DestroyZone");
                        foreach (GameObject destroyItem in listeOfDestroyItem)
                        {
                            DestroyZone script = destroyItem.GetComponent<DestroyZone>();

                            if (script != null && script.GetItem().GetName() == unlockItem.GetName())
                            {
                                script.DestroyObject();
                            }
                        }
                    }
                    else
                    {
                        GameObject[] listeOfLockItem = GameObject.FindGameObjectsWithTag("LockZone");
                        foreach (GameObject lockItem in listeOfLockItem)
                        {
                            DisabledZone script = lockItem.GetComponent<DisabledZone>();

                            if (script != null && script.GetItem().GetName() == unlockItem.GetName())
                            {
                                script.UnlockZone();
                            }
                        }
                    }
                }

            }

            Debug.Log("Load complete");
        }
    }
}

[System.Serializable]
public class SaveDatas
{
    public InventorySaveData inventory;
    public List<HarvestableSaveData> harvestables;
    public List<TreeLandSaveData> treeLands;
    public List<ItemData> itemsUnlock;
}