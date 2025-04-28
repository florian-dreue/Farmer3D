using System;
using System.IO;
using UnityEngine;

//Classe permettant de gérer les fichiers de sauvegarde
public static class FileManager
{
    //Fonction permettant de sauvegarder les données en les inscrivant dans un fichier
    public static bool WriteToFile(string a_FileName, string a_FileContents)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, a_FileName);

        try
        {
            File.WriteAllText(fullPath, a_FileContents);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to {fullPath} with exception {e}");
            return false;
        }
    }

    //Fonction permettant de charger les données depuis le fichier de sauvegarde
    public static bool LoadFromFile(string a_FileName, out string result)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, a_FileName);

        try
        {
            result = File.ReadAllText(fullPath);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log($"Pas de fichier de sauvegarde {e}");
            result = "";
            return false;
        }
    }
}