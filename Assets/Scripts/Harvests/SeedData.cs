using UnityEngine;

// Classe contenant les donn�es des graines
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

    //Fonction pour r�cup�rer le type de plante
    public PlantType GetPlantType()
    {
        return plantType;
    }

    //Fonction pour r�cup�rer le nombre de jour avant la pousse
    public int GetDayBeforeGrowth()
    {
        return dayBeforeGrowth;
    }

    //Fonction pour r�cup�rer le type de graine
    public string GetTypeOfSeed()
    {
        return typeOfSeed;
    }

    //Fonction permettant de r�cup�rer tout les �tats de croissance
    public GameObject[] GetAllStatesOfGrowth()
    {
        return statesOfGrowth;
    }

    //Fonction permettant de r�cup�rer un �tat de croissance particulier
    public GameObject GetStatesOfGrowth(int index)
    {
        return statesOfGrowth[index];
    }

    //Fonction permettant de r�cup�rer le nombre d'�tats de croissance
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