using UnityEngine;

// Classe contenant les données des graines
[CreateAssetMenu(fileName = "seed", menuName = "seed/New seed")]
public class SeedData : ScriptableObject
{
    [SerializeField]
    private GameObject[] statesOfGrowth;
    [SerializeField]
    private string typeOfSeed;
    [SerializeField]
    private int dayBeforeGrowth;
    [SerializeField]
    private PlantType plantType;

    //Fonction pour récupérer le type de plante
    public PlantType GetPlantType()
    {
        return plantType;
    }

    //Fonction pour récupérer le nombre de jour avant la pousse
    public int GetDayBeforeGrowth()
    {
        return dayBeforeGrowth;
    }

    //Fonction pour récupérer le type de graine
    public string GetTypeOfSeed()
    {
        return typeOfSeed;
    }

    //Fonction permettant de récupérer tout les états de croissance
    public GameObject[] GetAllStatesOfGrowth()
    {
        return statesOfGrowth;
    }

    //Fonction permettant de récupérer un état de croissance particulier
    public GameObject GetStatesOfGrowth(int index)
    {
        return statesOfGrowth[index];
    }

    //Fonction permettant de récupérer le nombre d'états de croissance
    public int getNumberOfStates()
    {
        return statesOfGrowth.Length;
    }
}

public enum PlantType
{
    Plant,
    Tearable
}