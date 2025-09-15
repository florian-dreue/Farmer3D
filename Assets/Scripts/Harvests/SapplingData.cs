using UnityEngine;

// Classe contenant les donn�es des jeunes pousses
[CreateAssetMenu(fileName = "sappling", menuName = "sappling/New sappling")]
public class SapplingData : ScriptableObject
{
    [SerializeField]
    private GameObject[] statesOfGrowth;
    [SerializeField]
    private string typeOfSappling;
    [SerializeField]
    private int dayBeforeGrowth;

    //Fonction permettant de r�cup�rer le nombre de jours avant la croissance
    public int getDayBeforeGrowth()
    {
        return dayBeforeGrowth;
    }

    //Fonction permettant de r�cup�rer le type de jeune pousse
    public string getTypeOfSappling() { return typeOfSappling; }

    //Fonction permettant de r�cup�rer tout les �tats de croissance
    public GameObject[] getAllStatesOfGrowth()
    {
        return statesOfGrowth;
    }

    //Fonction permettant de r�cup�rer un �tat de croissance particulier
    public GameObject getStatesOfGrowth(int index)
    {
        return statesOfGrowth[index];
    }

    //Fonction permettant de r�cup�rer le nombre d'�tats de croissance
    public int getNumberOfStates()
    {
        return statesOfGrowth.Length;
    }
}
