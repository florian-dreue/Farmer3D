using UnityEngine;

// Classe pour les zones arboricoles
public class TreeLand : MonoBehaviour
{
    private int state = 0;
    private int dayTracker = 0;
    private GameObject actualPrefab;
    private bool Planted = false;
    private bool Pickable = false;
    private SapplingData sappling;

    //Fonction pour ajouter un jour
    public void AddDay()
    {
        //On regarde si quelque chose est plant�
        if (Planted)
        {
            dayTracker++;
            //On regarrde si le nombre de jour modulo le temps entre deux �tape de pousse est �gale � 0 pour chager le mod�le
            if (dayTracker % sappling.getDayBeforeGrowth() == 0)
            {

                if (state == 0)
                {
                    //On initialise le mod�le correspondant � l'�tape actuelle
                    actualPrefab = Instantiate(sappling.getStatesOfGrowth(state), gameObject.transform);
                    state++;
                }
                else
                {
                    //Si la plantation n'est pas arriv� � terme on la fait avanc�e
                    if (state < sappling.getNumberOfStates())
                    {
                        //Si on augmente la culture on d�truit le model actuel avant de mettre le nouveau
                        Destroy(actualPrefab);
                        actualPrefab = Instantiate(sappling.getStatesOfGrowth(state), gameObject.transform);
                        state++;
                    }
                    if (state == sappling.getNumberOfStates())
                    {
                        //Si on arrive � la derni�re �tape on dit que la culture est r�coltable
                        Pickable = true;
                    }
                }
            }
        }
    }

    //Fonction pour planter une pousse
    public void Plant(SapplingData newSappling)
    {
        sappling = newSappling;
        actualPrefab = Instantiate(sappling.getStatesOfGrowth(state), gameObject.transform);
        state = 1;
        Planted = true;
    }

    //Fonction pour secouer l'arbre
    public void PickUp()
    {
        Pickable = false;
        state--;
        Destroy(actualPrefab);
        actualPrefab = Instantiate(sappling.getStatesOfGrowth(state-1), gameObject.transform);
    }

    //Fonction pour enlever la culture et r�initialiser la terre
    public void Reinitialised()
    {
        dayTracker = 0;
        Planted = false;
        Pickable = false;
        Destroy(actualPrefab);
    }

    //Fonction permettant de savoir si l'arbe est secouable
    public bool isPickable() {  return Pickable; }

    //Fonction permettant de savoir si quelque chose est plant�
    public bool isPlanted() { return Planted; }

    //Fonction permettant de r�cup�rer le nom de la pousse
    public string getTreeName() { return sappling.getTypeOfSappling(); }

    //Fonction permettant de r�cup�rer le nombre de jours depuis la plantation
    public int daySincePlantation() {  return dayTracker; }

}
