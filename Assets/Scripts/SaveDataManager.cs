using System.Collections.Generic;
using UnityEngine;

// Classe permettant de sauvegarder les données de l'inventaire
public static class SaveInventoryManager
{
    // Fonction permettant de sauvegarder les données en JSON
    public static void SaveJsonData(IEnumerable<ISaveable> a_Saveables)
    {
        Inventory sd = new Inventory();
        foreach (var saveable in a_Saveables)
        {
            saveable.PopulateInventory(sd);
        }

        if (FileManager.WriteToFile("Inventory.dat", sd.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    // Fonction permettant de charger les données en JSON
    public static void LoadJsonData(IEnumerable<ISaveable> a_Saveables)
    {
        if (FileManager.LoadFromFile("Inventory.dat", out var json))
        {
            Inventory sd = new Inventory();
            sd.LoadFromJson(json);

            foreach (var saveable in a_Saveables)
            {
                saveable.LoadFromInventory(sd);
            }

            Debug.Log("Load complete");
        }
    }
}
