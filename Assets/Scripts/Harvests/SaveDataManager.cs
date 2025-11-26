using System.Collections.Generic;
using UnityEngine;

// Classe permettant de sauvegarder les données de l'inventaire
public static class SaveInventoryManager
{
    // Fonction permettant de sauvegarder les données en JSON
    public static void SaveJsonData(IEnumerable<ISaveable> a_Saveables)
    {
        // Créer un conteneur de données
        InventoryData data = new InventoryData();

        // Demander aux saveables de remplir ce conteneur
        foreach (var saveable in a_Saveables)
        {
            saveable.PopulateInventory(data);
        }

        SaveFile saveFile = new SaveFile();
        saveFile.inventory = data;
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
    public static void LoadJsonData(IEnumerable<ISaveable> a_Saveables)
    {
        if (FileManager.LoadFromFile("Inventory.dat", out var json))
        {
            Debug.Log("json: "+ json);
            SaveFile saveFile = JsonUtility.FromJson<SaveFile>(json);
            InventoryData data = saveFile.inventory;

            foreach (var saveable in a_Saveables)
            {
                saveable.LoadFromInventory(data);
            }

            Debug.Log("Load complete");
        }
    }
}

[System.Serializable]
public class SaveFile
{
    public InventoryData inventory;
}