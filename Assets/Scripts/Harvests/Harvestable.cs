using UnityEngine;

//Classe contenant chaque terrain cultivable
public class Harvestable : MonoBehaviour
{
    private int state;
    private int dayTracker = 0;
    private int effectiveDays = 0;
    private GameObject actualPrefab;
    private bool isPlanted = false;
    private bool isHarvestable = false;
    [SerializeField]
    private Dirt dirt;
    private SeedData seedData;

    //Fonction pour ajouter un jour � la plantation
    public void AddDay()
    {
        //On regarde si quelque chose est plant�
        if (isPlanted)
        {
            dayTracker++;
            if (dirt.getWatered())
            {
                effectiveDays++;
                //On regarde si le nombre de jour modulo le temps entre deux �tape de pousse est �gale � 0 pour changer le mod�le
                if (effectiveDays % seedData.GetDayBeforeGrowth() == 0)
                {
                    if (state == 0)
                    {
                        //On initialise le mod�le correspondant � l'�tape actuelle
                        actualPrefab = Instantiate(seedData.GetStatesOfGrowth(state), gameObject.transform);
                        state++;
                    }
                    else
                    {
                        //Si la plantation n'est pas arriv� � terme on la fait avanc�e
                        if (state < seedData.getNumberOfStates())
                        {
                            //Si on augmente la culture on d�truit le model actuel avant de mettre le nouveau
                            Destroy(actualPrefab);
                            actualPrefab = Instantiate(seedData.GetStatesOfGrowth(state), gameObject.transform);
                            state++;
                        }
                        if (state == seedData.getNumberOfStates())
                        {
                            //Si on arrive � la derni�re �tape on dit que la culture est r�coltable
                            isHarvestable = true;
                        }
                    }
                }

                dirt.isGettingDrained();
            }
            
        }
    }

    //Fonction pour la plantation de graines -> On renseigne tout les champ n�cessaire
    public void isSeedeed(SeedData seedData)
    {
        this.seedData = seedData;
        isPlanted = true;
    }

    //Fonction pour le ramassage de la culture
    public void isPickedUp()
    {
        isHarvestable = false;
        if (seedData.GetPlantType() == PlantType.Plant)
        {
            state--;
            Destroy(actualPrefab);
            actualPrefab = Instantiate(seedData.GetStatesOfGrowth(state - 1), gameObject.transform);
        }
        else
        {
            state = 0;
            Destroy(actualPrefab);
            Reinitialised();
        }
    }

    //Fonction pour enlever la culture et r�initialiser la terre
    public void Reinitialised()
    {
        dayTracker = 0;
        effectiveDays = 0;
        isPlanted = false;
        dirt.Reinisialised();
    }

    //Fonction pour r�cup�rer le type de plante
    public PlantType GetPlantType()
    {
        return seedData.GetPlantType();
    }

    //Fonction pour r�cup�rer le type de graine
    public string GetTypeOfSeed()
    {
        return seedData.GetTypeOfSeed();
    }

    //Fonction pour savoir si une graine est plant�e
    public bool isSeedPlanted()
    {
        return isPlanted;
    }

    //Fonction pour savoir si une culture est r�coltable
    public bool isCultureHarvestable()
    {
        return isHarvestable;
    }

    //Fonction pour r�cup�rer le temps pass� depuis la plantation
    public int GetTimeSincePlanted()
    {
        return dayTracker;
    }
}
