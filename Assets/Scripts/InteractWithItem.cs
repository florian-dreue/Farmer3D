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
    private GameObject menuDeNuit;

    [SerializeField]
    private GameObject menuMarket;

    [SerializeField]
    private TextMeshProUGUI textNumeroJour;

    [SerializeField]
    private TextMeshProUGUI textMoneyJour;

    [SerializeField]
    private Button endButton;

    [SerializeField]
    private GameObject inventaire;

    [SerializeField]
    private TextMeshProUGUI textJourInventaire;

    [SerializeField]
    private TextMeshProUGUI textAmountMoney;

    [SerializeField]
    private GameObject ColorBox;

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
            { "Market", OpenMarket }
        };
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        text.text = "";
        ColorBox.SetActive(false);
        if (menuDeNuit.activeSelf || menuMarket.activeSelf)
        {
            return;
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

        // Appuyez sur R pour lâcher l'outil équipé
        if (Input.GetKeyDown(KeyCode.R))
        {
            DropEquippedTool();
        }
    }

    //Fonction pour le drop des outils
    private void DropEquippedTool()
    {
        // Vérifie si un outil est équipé
        if (inventory.GetToolEquipped() != null)
        {
            // Instancie le prefab de l'outil
            GameObject droppedTool = Instantiate(inventory.GetToolEquipped().prefab);

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

        if (inventory.HaveSpace(itemSee))
        {
            text.text = LanguageManager.Instance.GetTranslation("pressToPickUp") + LanguageManager.Instance.GetTranslation(itemSee.nameItem.ToLower() + "Gender"); ;
            ColorBox.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                inventory.AddItem(hit.transform.gameObject.GetComponent<Item>().item);
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
                        //On instancie un objet
                        GameObject instantiatedRessource = GameObject.Instantiate(ressource.itemData.prefab);

                        if (harvestable.GetPlantType() == PlantType.Plant)
                        {
                            //On modifie l�g�rement sa position pour qu'il soit ramassable
                            Vector3 newPos = harvestable.transform.position;
                            newPos.z += ressource.itemData.prefab.transform.position.z;
                            newPos.y += ressource.itemData.prefab.transform.position.y;
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
            text.text = LanguageManager.Instance.GetTranslation("needTool") + LanguageManager.Instance.GetTranslation(fullGrownItem.GetToolRequired().nameItem.ToLower()) + LanguageManager.Instance.GetTranslation("toHarvest");
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
            Time.timeScale = 0;
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;

            inventaire.SetActive(false);
            menuMarket.SetActive(true);
            var amount = inventory.GetSellAmount();
            textAmountMoney.text = LanguageManager.Instance.GetTranslation("sellInventory") + amount;
        }
    }

    //Fonction pour l'interaction avec zones arboricolles
    private void PlantTree(RaycastHit hit)
    {
        TreeLand treeLand = hit.transform.gameObject.GetComponent<TreeLand>();
        if (treeLand.isPlanted())
        {
            if (!treeLand.isPickable())
            {
                text.text = LanguageManager.Instance.GetTranslation(treeLand.getTreeName().ToLower()) + LanguageManager.Instance.GetTranslation("plantSince") + (treeLand.daySincePlantation() == 0 ? LanguageManager.Instance.GetTranslation("today") : treeLand.daySincePlantation() + (treeLand.daySincePlantation() > 1 ? LanguageManager.Instance.GetTranslation("days") : LanguageManager.Instance.GetTranslation("day")));
                ColorBox.SetActive(true);
            }
            else
            {
                text.text = LanguageManager.Instance.GetTranslation(treeLand.getTreeName().ToLower()) + LanguageManager.Instance.GetTranslation("harvestable");
                ColorBox.SetActive(true);
            }
        }
        else
        {
            if (inventory.GetToolEquipped()?.type == ItemType.Sappling)
            {
                text.text = LanguageManager.Instance.GetTranslation("pressToPlant");
                ColorBox.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    SapplingData sappling = inventory.GetToolEquipped().sappling;
                    treeLand.Plant(sappling);
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
                    GameObject instantiatedRessource = GameObject.Instantiate(ressource.itemData.prefab);
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
            //On désactive l'affichage de l'inventaire et on active l'affichage du menu de nuit
            inventaire.SetActive(false);
            menuDeNuit.SetActive(true);
            //On désactive le mouvement de la caméra et on réactive la souris
            Time.timeScale = 0;
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            //On change le text pour afficher le jour et on lance le nouveau jour
            textNumeroJour.text = LanguageManager.Instance.GetTranslation("endDay") + gameController.GetDays();
            textMoneyJour.text = LanguageManager.Instance.GetTranslation("moneyWin") + gameController.GetMoneyWin();
            var textendButton = endButton.GetComponentInChildren<TextMeshProUGUI>();
            textendButton.text = LanguageManager.Instance.GetTranslation("endRecap");
            gameController.NewDay();
            textJourInventaire.text = gameController.GetDays().ToString();
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
            if (inventory.GetToolEquipped()?.nameItem == "Hoe")
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

                if (inventory.GetToolEquipped()?.type == ItemType.Seed)
                {
                    text.text = LanguageManager.Instance.GetTranslation("pressToSeed");
                    ColorBox.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        SeedData seed = inventory.GetToolEquipped().seed;
                        harvestableSee.isSeedeed(seed);
                    }
                }
                else if(inventory.GetToolEquipped()?.nameItem == "Watercan" && dirtSee.getWatered())
                {
                    text.text = LanguageManager.Instance.GetTranslation("pressToWater");
                    ColorBox.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        dirtSee.isGettingWatered();
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
                        if (inventory.GetToolEquipped()?.nameItem == "Watercan")
                        {
                            text.text = LanguageManager.Instance.GetTranslation("pressToWater");
                            ColorBox.SetActive(true);
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                dirtSee.isGettingWatered();
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
}
