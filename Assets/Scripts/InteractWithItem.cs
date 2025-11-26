using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Fonction permettant l'interaction avec les objets
public class InteractWithItem : MonoBehaviour
{

    private Dictionary<string, Action<RaycastHit>> tagActions;

    [SerializeField]
    private float range = 1.5f;

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Text text;

    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private GameObject ColorBox;

    [SerializeField]
    private EventSystem eventSystem;

    // Start is called before the first frame update
    private void Start()
    {
        //Création du Dictionnaire liant les tags aux actions
        tagActions = new Dictionary<string, Action<RaycastHit>>
        {
            { "Item", PickUpItem },
            { "Harvestable", HarvestItem },
            { "CapsuleDirt", SeedItem },
            { "Door", Sleep },
            { "Pickable", ShakeTree },
            { "TreeLand", PlantTree },
            { "Market", OpenMarket },
            { "Water", FillCan },
            { "LockZone", ManageLock },
            { "Shop", OpenShop }
        };
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        text.text = "";
        ColorBox.SetActive(false);

        // Appuyez sur R pour lâcher l'outil équipé
        if (Input.GetKeyDown(KeyCode.R))
        {
            DropEquippedTool();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            eventSystem.openMenu();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            eventSystem.clickBack();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            eventSystem.openBackpack();
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit, range, layerMask))
        {
            string tag = hit.transform.tag;
            if (tagActions.ContainsKey(tag))
            {
                // Appel de la méthode associée au tag
                tagActions[tag].Invoke(hit);
            }
            else
            {
                Debug.Log("Tag inconnu " + tag);
            }
        }
    }

    //Fonction pour le drop des outils
    private void DropEquippedTool()
    {
        // Vérifie si un outil est équipé
        if (inventory.GetToolEquipped() != null)
        {
            // Instancie le prefab de l'outil
            GameObject droppedTool = Instantiate(inventory.GetToolEquipped().GetPrefab());

            // Positionne l'objet juste devant le joueur
            Vector3 dropPosition = transform.position + transform.forward * 1.0f; // Position devant le joueur
            dropPosition.y = transform.position.y - 1.0f; // Légèrement au-dessus du sol
            droppedTool.transform.position = dropPosition;

            // Ajoute un Rigidbody pour que l'objet tombe au sol
            if (droppedTool.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = droppedTool.AddComponent<Rigidbody>();
                rb.mass = 1.0f; // Vous pouvez ajuster la masse si nécessaire
                rb.isKinematic = false;
            } 
            else
            {
                Rigidbody rb = droppedTool.GetComponent<Rigidbody>();
                rb.mass = 1.0f; // Vous pouvez ajuster la masse si nécessaire
                rb.isKinematic = false;
            }

            Item droppedItem = droppedTool.GetComponent<Item>();
            if (droppedItem != null)
            {
                droppedItem.Initialize(inventory.GetToolEquipped()); // injecte la copie modifiée
            }

            // Retire l'outil de l'inventaire
            inventory.EmptyTool();
        }
        else
        {
            Debug.Log("Aucun outil équipé à lâcher.");
        }
    }

    //Fonction pour l'intéraction avec les items
    public void PickUpItem(RaycastHit hit)
    {
        //Si c'est un item et qu'on à de la place, on donne la possibilité de le ramasser avec E
        ItemData itemSee = hit.transform.gameObject.GetComponent<Item>().item;
        Item item = hit.transform.gameObject.GetComponent<Item>();

        if (inventory.HaveSpace(itemSee))
        {
            text.text = LanguageManager.Instance.GetTranslation("pressToPickUp") + LanguageManager.Instance.GetTranslation(itemSee.GetName().ToLower() + "Gender");
            ColorBox.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                inventory.AddItem(hit.transform.gameObject.GetComponent<Item>());
                Destroy(hit.transform.gameObject);
            }
        }
        else
        {
            text.text = LanguageManager.Instance.GetTranslation("inventoryFull");
            ColorBox.SetActive(true);
        }
    }

    //Fonction pour l'interaction avec les plans à maturité
    public void HarvestItem(RaycastHit hit)
    {
        FullGrownItem fullGrownItem = hit.transform.gameObject.GetComponent<FullGrownItem>();

        //Si c'est un harvestable, on regarde si il � besoin d'un objet pour �tre ramass� et si, le cas pr�sent, l'objet n�cessaire est l'objet �quip�
        if (fullGrownItem.GetToolRequired() == null || inventory.GetToolEquipped() == fullGrownItem.GetToolRequired())
        {
            //Si on as la Faucille on donne la possibilit� de r�colter
            text.text = LanguageManager.Instance.GetTranslation("pressToHarvest");
            ColorBox.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Harvestable harvestable = hit.transform.GetComponentInParent<Harvestable>();

                //On boucle sur chaque objet différent que peut dropper le plant
                for (int i = 0; i < fullGrownItem.harvestableItems.Length; i++)
                {
                    Ressource ressource = fullGrownItem.harvestableItems[i];

                    //Pour chaque ressource, on génère un nombre aléatoire entre le minimum et le maximum de ressources possible
                    for (int j = 0; j < UnityEngine.Random.Range(ressource.minRessource, ressource.maxRessource); j++)
                    {
                        GameObject prefab = ressource.itemData.GetPrefab();
                        //On instancie un objet
                        GameObject instantiatedRessource = GameObject.Instantiate(prefab);

                        if (harvestable.GetPlantType() == PlantType.Plant)
                        {
                            //On modifie l�g�rement sa position pour qu'il soit ramassable
                            Vector3 newPos = harvestable.transform.position;
                            newPos.z += prefab.transform.position.z;
                            newPos.y += prefab.transform.position.y;
                            newPos.x += 0.5f;
                            instantiatedRessource.transform.position = newPos;
                        }
                        else
                        {
                            Vector3 newPos = harvestable.transform.position;
                            newPos.y += 0.2f;
                            instantiatedRessource.transform.position = newPos;
                        }

                    }
                }

                harvestable.isPickedUp();
            }
        }
        //Si on as pas l'objet ad�quat on affiche le text n�cessaire
        else
        {
            text.text = LanguageManager.Instance.GetTranslation("needTool") + LanguageManager.Instance.GetTranslation(fullGrownItem.GetToolRequired().GetName().ToLower()) + LanguageManager.Instance.GetTranslation("toHarvest");
            ColorBox.SetActive(true);
        }
    }

    //Fonction pour l'interaction avec le marché
    private void OpenMarket(RaycastHit hit)
    {
        text.text = LanguageManager.Instance.GetTranslation("pressToMarket");
        ColorBox.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            eventSystem.openMarketMenu();
        }
    }

    //Fonction pour l'interaction avec zones arboricolles
    private void PlantTree(RaycastHit hit)
    {
        TreeLand treeLand = hit.transform.gameObject.GetComponent<TreeLand>();
        TreeDirt dirtSee = hit.transform.gameObject.GetComponent<TreeDirt>();
        if (treeLand.isPlanted())
        {
            if (!treeLand.isPickable())
            {
                if (!dirtSee.getWatered())
                {
                    if (inventory.GetToolEquipped()?.GetName() == "Watercan")
                    {
                        if(inventory.GetToolCapicity() >= 20)
                        {
                            text.text = LanguageManager.Instance.GetTranslation("pressToWater");
                            ColorBox.SetActive(true);
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                dirtSee.isGettingWatered();
                                inventory.DrainTool(20);
                            }
                        }
                        else
                        {
                            text.text = LanguageManager.Instance.GetTranslation("needWater");
                            ColorBox.SetActive(true);
                        }
                    }
                    else
                    {
                        text.text = LanguageManager.Instance.GetTranslation("toolToWater");
                        ColorBox.SetActive(true);
                    }
                }
                else
                {
                    text.text = LanguageManager.Instance.GetTranslation(treeLand.getTreeName().ToLower()) + LanguageManager.Instance.GetTranslation("plantSince") + (treeLand.daySincePlantation() == 0 ? LanguageManager.Instance.GetTranslation("today") : treeLand.daySincePlantation() + (treeLand.daySincePlantation() > 1 ? LanguageManager.Instance.GetTranslation("days") : LanguageManager.Instance.GetTranslation("day")));
                    ColorBox.SetActive(true);
                }   
            }
            else
            {
                text.text = LanguageManager.Instance.GetTranslation(treeLand.getTreeName().ToLower()) + LanguageManager.Instance.GetTranslation("harvestable");
                ColorBox.SetActive(true);
            }
        }
        else
        {
            SapplingData sappling = inventory.GetToolEquipped() as SapplingData;
            if (sappling != null)
            {
                text.text = LanguageManager.Instance.GetTranslation("pressToPlant");
                ColorBox.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    treeLand.Plant(sappling);
                }
            }
            else if (inventory.GetToolEquipped()?.GetName() == "Watercan" && !dirtSee.getWatered())
            {
                if (inventory.GetToolCapicity() >= 20)
                {
                    text.text = LanguageManager.Instance.GetTranslation("pressToWater");
                    ColorBox.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        dirtSee.isGettingWatered();
                        inventory.DrainTool(20);
                    }
                }
                else
                {
                    text.text = LanguageManager.Instance.GetTranslation("needWater");
                    ColorBox.SetActive(true);
                }
            }
            else
            {
                text.text = LanguageManager.Instance.GetTranslation("takeSappling");
                ColorBox.SetActive(true);
            }
        }
    }

    //Fonction pour l'interaction avec les arbres ayant des fruits
    private void ShakeTree(RaycastHit hit)
    {
        //Si on as la Houe on donne la possibilit� de r�colter
        text.text = LanguageManager.Instance.GetTranslation("pressToShake");
        ColorBox.SetActive(true);
        if (Input.GetKeyDown(KeyCode.E))
        {
            FullGrownItem fullGrownItem = hit.transform.gameObject.GetComponent<FullGrownItem>();
            TreeLand treeLand = hit.transform.GetComponentInParent<TreeLand>();

            //On boucle sur chaque objet diff�rent que peut dropper l'arbre
            for (int i = 0; i < fullGrownItem.harvestableItems.Length; i++)
            {
                Ressource ressource = fullGrownItem.harvestableItems[i];

                //Pour chaque ressource, on g�n�re un nombre al�atoire entre le minimum et le maximum de ressources possible
                for (int j = 0; j < UnityEngine.Random.Range(ressource.minRessource, ressource.maxRessource); j++)
                {
                    //On instancie un objet
                    GameObject instantiatedRessource = GameObject.Instantiate(ressource.itemData.GetPrefab());
                    float xRand = (float)UnityEngine.Random.Range(4, 8) / 10;
                    int xSigne = UnityEngine.Random.Range(0, 2);
                    float zRand = (float)UnityEngine.Random.Range(4, 8) / 10;
                    int zSigne = UnityEngine.Random.Range(0, 2);
                    Vector3 newPos = fullGrownItem.transform.position;
                    if (xSigne == 0)
                    {
                        newPos.x += xRand;
                    }
                    else
                    {
                        newPos.x -= xRand;
                    }

                    if (zSigne == 0)
                    {
                        newPos.z += zRand;
                    }
                    else
                    {
                        newPos.z -= zRand;
                    }
                    instantiatedRessource.transform.position = newPos;
                }
            }

            treeLand.PickUp();
        }
    }

    //Fonction pour l'interaction avec la ferme
    private void Sleep(RaycastHit hit)
    {
        text.text = LanguageManager.Instance.GetTranslation("pressToSleep");
        ColorBox.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            eventSystem.openNightMenu();
        }
    }

    //Fonction pour l'interaction avec zones cultivables
    private void SeedItem(RaycastHit hit)
    {
        //Si c'est une parcelle de terre on regarde si elle est labourée
        Dirt dirtSee = hit.transform.gameObject.GetComponent<Dirt>();

        if (!dirtSee.getPlowed())
        {
            //Si elle n'est pas labour� on regarde si on as la houe pour donner la possibilit� de labourer
            if (inventory.GetToolEquipped()?.GetName() == "Hoe")
            {
                text.text = LanguageManager.Instance.GetTranslation("pressToPlow");
                ColorBox.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    dirtSee.isGettingPlowed();
                }
            }
            else
            {
                text.text = LanguageManager.Instance.GetTranslation("toolToPlow");
                ColorBox.SetActive(true);
            }
        }
        else
        {
            Harvestable harvestableSee = hit.transform.gameObject.GetComponent<Harvestable>();

            //Si la terre à été labourré on regarde si des graines ont été plantées
            if (!harvestableSee.isSeedPlanted())
            {
                //Si on as pas déjà de graine plantées on en plante si on as des graines dans l'inventaire
                SeedData seed = inventory.GetToolEquipped() as SeedData;
                if (seed != null)
                {
                    text.text = LanguageManager.Instance.GetTranslation("pressToSeed");
                    ColorBox.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        harvestableSee.isSeedeed(seed);
                    }
                }
                else if(inventory.GetToolEquipped()?.GetName() == "Watercan" && !dirtSee.getWatered())
                {
                    if (inventory.GetToolCapicity() >= 10)
                    {
                        text.text = LanguageManager.Instance.GetTranslation("pressToWater");
                        ColorBox.SetActive(true);
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            dirtSee.isGettingWatered();
                            inventory.DrainTool(10);
                        }
                    }
                    else
                    {
                        text.text = LanguageManager.Instance.GetTranslation("needWater");
                        ColorBox.SetActive(true);
                    }
                }
                else
                {
                    text.text = LanguageManager.Instance.GetTranslation("seedToSeed");
                    ColorBox.SetActive(true);
                }

            }
            else
            {
                //Si on a déjà planté quelque chose on regarde si on peut ramasser
                if (!harvestableSee.isCultureHarvestable())
                {
                    if (!dirtSee.getWatered())
                    {
                        if (inventory.GetToolEquipped()?.GetName() == "Watercan")
                        {
                            if (inventory.GetToolCapicity() >= 20)
                            {
                                text.text = LanguageManager.Instance.GetTranslation("pressToWater");
                                ColorBox.SetActive(true);
                                if (Input.GetKeyDown(KeyCode.E))
                                {
                                    dirtSee.isGettingWatered();
                                    inventory.DrainTool(10);
                                }
                            }
                            else
                            {
                                text.text = LanguageManager.Instance.GetTranslation("needWater");
                                ColorBox.SetActive(true);
                            }
                        }
                        else
                        {
                            text.text = LanguageManager.Instance.GetTranslation("toolToWater");
                            ColorBox.SetActive(true);
                        }
                    }
                    else
                    {
                        text.text = LanguageManager.Instance.GetTranslation(harvestableSee.GetTypeOfSeed().ToLower()) + LanguageManager.Instance.GetTranslation("plantSince") + (harvestableSee.GetTimeSincePlanted() == 0 ? LanguageManager.Instance.GetTranslation("today") : harvestableSee.GetTimeSincePlanted() + (harvestableSee.GetTimeSincePlanted() > 1 ? LanguageManager.Instance.GetTranslation("days") : LanguageManager.Instance.GetTranslation("day")));
                        ColorBox.SetActive(true);
                    }
                }
                else
                {
                    
                    text.text = LanguageManager.Instance.GetTranslation(harvestableSee.GetTypeOfSeed().ToLower()) + LanguageManager.Instance.GetTranslation("harvestable");
                    ColorBox.SetActive(true);
                        
                }

            }

        }
    }

    private void FillCan(RaycastHit hit)
    {
        ItemData toolEquiped = inventory.GetToolEquipped();
        FillableData fillable = toolEquiped as FillableData;

        if (fillable != null && fillable.GetName() == "Watercan" && fillable.GetFilling() != 100)
        {
            text.text = LanguageManager.Instance.GetTranslation("pressToFillWater");
            ColorBox.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                inventory.FillTool();
            }
        }
    }

    private void ManageLock(RaycastHit hit)
    {
        DisabledZone disabledZone = hit.transform.gameObject.GetComponent<DisabledZone>();
        text.text = LanguageManager.Instance.GetTranslation("pressToUnlock");
        ColorBox.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            disabledZone.UnlockZone();
        }
    }

    private void OpenShop(RaycastHit hit)
    {
        text.text = LanguageManager.Instance.GetTranslation("pressToShop");
        ColorBox.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            eventSystem.openShopMenu();
        }
    }
}
