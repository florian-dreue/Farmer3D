using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Classe permettant de sauvegarder les données de l'inventaire
public static class SaveInventoryManager
{
    // Fonction permettant de sauvegarder les données en JSON
    public static void SaveJsonData(ISaveable a_Saveables, List<HarvestableData> b_saveables)
    {
        // Créer un conteneur de données
        InventoryData data = new InventoryData();

        a_Saveables.PopulateInventory(data);

        SaveFile saveFile = new SaveFile();
        saveFile.inventory = data;
        saveFile.harvestables = b_saveables;
        // Sérialiser et écrire
        string json = JsonUtility.ToJson(saveFile);
        if (FileManager.WriteToFile("Inventory.dat", json))
        {
            Debug.Log("Save successful");
            Debug.Log(data.content?.Count ?? 0);
            Debug.Log(json);
        }
    }

    // Fonction permettant de charger les données en JSON
    public static void LoadJsonData(Inventory a_Saveables, List<Harvestable> b_saveables)
    {
        if (FileManager.LoadFromFile("Inventory.dat", out var json))
        {
            Debug.Log("json: "+ json);
            SaveFile saveFile = JsonUtility.FromJson<SaveFile>(json);
            InventoryData data = saveFile.inventory;

            a_Saveables.LoadFromInventory(data);
            a_Saveables.RefreshContent();

            List<HarvestableData> harvestables = saveFile.harvestables;

            foreach (var hData in saveFile.harvestables)
            {
                var harvestable = b_saveables.FirstOrDefault(h => h.UniqueId == hData.uniqueId);
                
                if (harvestable != null)
                {
                    harvestable.LoadFromHarvestable(hData);
                }
            }

            /*
            for (int i = 0; i < harvestables.Count; i++)
            {
                b_saveables[i].LoadFromHarvestable(harvestables[i]);
            }*/

            Debug.Log("Load complete");
        }
    }
}

[System.Serializable]
public class SaveFile
{
    public InventoryData inventory;
    public List<HarvestableData> harvestables;
}