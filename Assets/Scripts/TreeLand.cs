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
        //On regarde si quelque chose est planté
        if (Planted)
        {
            dayTracker++;
            //On regarrde si le nombre de jour modulo le temps entre deux étape de pousse est égale à 0 pour chager le modèle
            if (dayTracker % sappling.getDayBeforeGrowth() == 0)
            {

                if (state == 0)
                {
                    //On initialise le modèle correspondant à l'étape actuelle
                    actualPrefab = Instantiate(sappling.getStatesOfGrowth(state), gameObject.transform);
                    state++;
                }
                else
                {
                    //Si la plantation n'est pas arrivé à terme on la fait avancée
                    if (state < sappling.getNumberOfStates())
                    {
                        //Si on augmente la culture on détruit le model actuel avant de mettre le nouveau
                        Destroy(actualPrefab);
                        actualPrefab = Instantiate(sappling.getStatesOfGrowth(state), gameObject.transform);
                        state++;
                    }
                    if (state == sappling.getNumberOfStates())
                    {
                        //Si on arrive à la dernière étape on dit que la culture est récoltable
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

    //Fonction pour enlever la culture et réinitialiser la terre
    public void Reinitialised()
    {
        dayTracker = 0;
        Planted = false;
        Pickable = false;
        Destroy(actualPrefab);
    }

    //Fonction permettant de savoir si l'arbe est secouable
    public bool isPickable() {  return Pickable; }

    //Fonction permettant de savoir si quelque chose est planté
    public bool isPlanted() { return Planted; }

    //Fonction permettant de récupérer le nom de la pousse
    public string getTreeName() { return sappling.getTypeOfSappling(); }

    //Fonction permettant de récupérer le nombre de jours depuis la plantation
    public int daySincePlantation() {  return dayTracker; }

}
