using UnityEngine;

// Classe contenant les données des jeunes pousses
[CreateAssetMenu(fileName = "sappling", menuName = "sappling/New sappling")]
public class SapplingData : ScriptableObject
{
    [SerializeField]
    private GameObject[] statesOfGrowth;
    [SerializeField]
    private string typeOfSappling;
    [SerializeField]
    private int dayBeforeGrowth;

    //Fonction permettant de récupérer le nombre de jours avant la croissance
    public int getDayBeforeGrowth()
    {
        return dayBeforeGrowth;
    }

    //Fonction permettant de récupérer le type de jeune pousse
    public string getTypeOfSappling() { return typeOfSappling; }

    //Fonction permettant de récupérer tout les états de croissance
    public GameObject[] getAllStatesOfGrowth()
    {
        return statesOfGrowth;
    }

    //Fonction permettant de récupérer un état de croissance particulier
    public GameObject getStatesOfGrowth(int index)
    {
        return statesOfGrowth[index];
    }

    //Fonction permettant de récupérer le nombre d'états de croissance
    public int getNumberOfStates()
    {
        return statesOfGrowth.Length;
    }
}
