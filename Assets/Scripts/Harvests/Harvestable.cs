using System;
using System.Collections.Generic;
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

    [SerializeField] private string uniqueId;
    public string UniqueId => uniqueId;

    private void OnValidate()
    {
            Transform current = transform;
            string path = current.name;

            // Remonter toute la hiérarchie jusqu'à la racine
            while (current.parent != null)
            {
                current = current.parent;
                path = current.name + "/" + path;
                if (current.name.Contains("ZoneCultivable"))
                {
                    break;
                }
            }

            uniqueId = path;
    }

    //Fonction pour ajouter un jour à la plantation
    public void AddDay()
    {
        //On regarde si quelque chose est planté
        if (isPlanted)
        {
            dayTracker++;
            if (dirt.getWatered())
            {
                effectiveDays++;
                //On regarde si le nombre de jour modulo le temps entre deux étape de pousse est égale à 0 pour changer le modèle
                if (effectiveDays % seedData.GetDayBeforeGrowth() == 0)
                {
                    if (state == 0)
                    {
                        //On initialise le modèle correspondant à l'étape actuelle
                        actualPrefab = Instantiate(seedData.GetStatesOfGrowth(state), gameObject.transform);
                        state++;
                    }
                    else
                    {
                        //Si la plantation n'est pas arrivé à terme on la fait avancée
                        if (state < seedData.getNumberOfStates())
                        {
                            //Si on augmente la culture on détruit le model actuel avant de mettre le nouveau
                            Destroy(actualPrefab);
                            actualPrefab = Instantiate(seedData.GetStatesOfGrowth(state), gameObject.transform);
                            state++;
                        }
                        if (state == seedData.getNumberOfStates())
                        {
                            //Si on arrive à la dernière étape on dit que la culture est récoltable
                            isHarvestable = true;
                        }
                    }
                }

                dirt.isGettingDrained();
            }
            
        }
    }

    //Fonction pour la plantation de graines -> On renseigne tout les champ nécessaire
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

    //Fonction pour enlever la culture et réinitialiser la terre
    public void Reinitialised()
    {
        dayTracker = 0;
        effectiveDays = 0;
        isPlanted = false;
        dirt.Reinisialised();
    }

    //Fonction pour récupérer le type de plante
    public PlantType GetPlantType()
    {
        return seedData.GetPlantType();
    }

    //Fonction pour récupérer le type de graine
    public string GetTypeOfSeed()
    {
        return seedData.GetTypeOfSeed();
    }

    //Fonction pour savoir si une graine est plantée
    public bool isSeedPlanted()
    {
        return isPlanted;
    }

    //Fonction pour savoir si une culture est récoltable
    public bool isCultureHarvestable()
    {
        return isHarvestable;
    }

    //Fonction pour récupérer le temps passé depuis la plantation
    public int GetTimeSincePlanted()
    {
        return dayTracker;
    }

    public void PopulateHarvestable(HarvestableSaveData saveContainer)
    {
        saveContainer.uniqueId = this.uniqueId;
        saveContainer.state = this.state;
        saveContainer.dayTracker = this.dayTracker;
        saveContainer.effectiveDays = this.effectiveDays;
        saveContainer.isPlanted = this.isPlanted;
        saveContainer.isHarvestable = this.isHarvestable;
        saveContainer.dirtPlowed = this.dirt.getPlowed();
        saveContainer.dirtWatered = this.dirt.getWatered();
        saveContainer.seedData = this.seedData;
    }

    public void LoadFromHarvestable(HarvestableSaveData harvestableSaved)
    {
        this.state = harvestableSaved.state;
        this.dayTracker = harvestableSaved.dayTracker;
        this.effectiveDays = harvestableSaved.effectiveDays;
        this.isPlanted = harvestableSaved.isPlanted;
        this.isHarvestable = harvestableSaved.isHarvestable;
        this.seedData = harvestableSaved.seedData;
        if (harvestableSaved.dirtPlowed)
        {
            this.dirt.isGettingPlowed();
        }
        if(harvestableSaved.dirtWatered)
        {
            this.dirt.isGettingWatered();
        }

        if(seedData != null)
        {
            if (effectiveDays % seedData.GetDayBeforeGrowth() == 0)
            {
                if (state != 0)
                {
                    //On initialise le modèle correspondant à l'étape actuelle
                    actualPrefab = Instantiate(seedData.GetStatesOfGrowth(state-1), gameObject.transform);
                }
            }
        }
    }
}

[System.Serializable]
public class HarvestableSaveData
{
    public string uniqueId;
    public int state;
    public int dayTracker;
    public int effectiveDays;
    public bool isPlanted;
    public bool isHarvestable;
    public bool dirtPlowed;
    public bool dirtWatered;
    public SeedData seedData;
}