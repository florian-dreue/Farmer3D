using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Classe permettant d'afficher le menu du jeu lorsque A est pressé
public class EventSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject inventory;

    [SerializeField]
    private GameObject inGameMenu;

    [SerializeField]
    private GameObject nightMenu;

    [SerializeField]
    private GameObject marketMenu;

    [SerializeField]
    private GameObject commandMenu;

    [SerializeField]
    private GameObject backpackMenu;

    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private GameObject shopMenu;

    private GameObject lastMenu;

    private void Start()
    {
        Inventory inventaire = FindAnyObjectByType<Inventory>();
        List<Harvestable> saveables = FindObjectsOfType<Harvestable>().ToList();

        SaveInventoryManager.LoadJsonData(inventaire, saveables);
        lastMenu = inventory;
    }

    void Update()
    {
        /*
        // Vérifier si la touche "A" est pressée
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            //Si le menu est activé on le désactive sinon on active 
            if (inGameMenu.activeSelf)
            {
                inventory.SetActive(true);
                inGameMenu.SetActive(false);

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
            else
            {
                inventory.SetActive(false);
                inGameMenu.SetActive(true);
                //On désactive le mouvement de la caméra et on réactive la souris
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
                
        }*/
    }

    public void openMenu()
    {
        
        GameObject actualMenu = getActiveMenu();

        if (actualMenu == nightMenu || actualMenu == commandMenu)
        {
            return;
        }

        if(!inGameMenu.activeSelf)
        {
            lastMenu = getActiveMenu();

            lastMenu.SetActive(false);
            inGameMenu.SetActive(true);

            //On désactive le mouvement de la caméra et on réactive la souris
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
        }
        else
        {
            lastMenu.SetActive(true);
            inGameMenu.SetActive(false);

            if (lastMenu == inventory)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
        }
        
    }

    public void clickBack()
    {
        GameObject actualMenu = getActiveMenu();

        if (actualMenu == inventory)
        {
            lastMenu = getActiveMenu();

            lastMenu.SetActive(false);
            inGameMenu.SetActive(true);

            //On désactive le mouvement de la caméra et on réactive la souris
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (actualMenu == inGameMenu)
        {
            lastMenu.SetActive(true);
            inGameMenu.SetActive(false);

            if (lastMenu == inventory)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
        }
        else if (actualMenu == marketMenu)
        {
            inventory.SetActive(true);
            marketMenu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
        else if (actualMenu == shopMenu)
        {
            inventory.SetActive(true);
            shopMenu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            TextMeshProUGUI textJourInventaire = inventory.transform.Find("Top").transform.Find("TextArgentValue").GetComponent<TextMeshProUGUI>();
            textJourInventaire.text = MainManager.Instance.GetMoney().ToString();
        }
        else if (actualMenu == backpackMenu)
        {
            backpackMenu.SetActive(false);
            inventory.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
        else if (actualMenu == commandMenu)
        {
            commandMenu.SetActive(false);
            inGameMenu.SetActive(true);
        }
    }

    public void openNightMenu()
    {
        if (!nightMenu.activeSelf)
        {
            lastMenu = inventory;
            inventory.SetActive(false);

            TextMeshProUGUI[] listText = nightMenu.GetComponents<TextMeshProUGUI>();
            
            TextMeshProUGUI textNumeroJour = nightMenu.transform.Find("TextJour").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI textMoneyJour = nightMenu.transform.Find("TextMoney").GetComponent<TextMeshProUGUI>(); ;

            textNumeroJour.text = LanguageManager.Instance.GetTranslation("endDay") + gameController.GetDays();
            textMoneyJour.text = LanguageManager.Instance.GetTranslation("moneyWin") + gameController.GetMoneyWin();
            
            TextMeshProUGUI textendButton = nightMenu.transform.Find("CloseButton").GetComponentInChildren<TextMeshProUGUI>();
            textendButton.text = LanguageManager.Instance.GetTranslation("endRecap");
            gameController.NewDay();

            TextMeshProUGUI textJourInventaire = inventory.transform.Find("Top").transform.Find("TextNbJour").GetComponent<TextMeshProUGUI>();
            textJourInventaire.text = gameController.GetDays().ToString();

            nightMenu.SetActive(true);

            //On désactive le mouvement de la caméra et on réactive la souris
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
    }

    public void openMarketMenu()
    {
        if (!marketMenu.activeSelf)
        {
            lastMenu = inventory;
            inventory.SetActive(false);
            marketMenu.SetActive(true);

            //On désactive le mouvement de la caméra et on réactive la souris
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void openShopMenu()
    {
        if (!shopMenu.activeSelf)
        {
            lastMenu = inventory;
            inventory.SetActive(false);
            shopMenu.SetActive(true);

            //On désactive le mouvement de la caméra et on réactive la souris
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void openBackpack()
    {
        GameObject actualMenu = getActiveMenu();

        if (actualMenu != inventory && actualMenu != backpackMenu)
        {
            return;
        }

        if (!backpackMenu.activeSelf)
        {
            lastMenu = inventory;
            inventory.SetActive(false);

            TextMeshProUGUI textJourInventaire = backpackMenu.transform.Find("Top").transform.Find("TextNbJour").GetComponent<TextMeshProUGUI>();
            textJourInventaire.text = gameController.GetDays().ToString();

            TextMeshProUGUI textMoneyInventaire = backpackMenu.transform.Find("Top").transform.Find("TextArgentValue").GetComponent<TextMeshProUGUI>();
            textMoneyInventaire.text = gameController.GetMoney().ToString();

            backpackMenu.SetActive(true);

            //On désactive le mouvement de la caméra et on réactive la souris
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else
        {
            inventory.SetActive(true);
            backpackMenu.SetActive(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            Time.timeScale = 1;
        }
    }

    private GameObject getActiveMenu()
    {
        if (inventory.activeSelf)
        {
            return inventory;
        }
        else if (inGameMenu.activeSelf)
        {
            return inGameMenu;
        }
        else if (marketMenu.activeSelf)
        {
            return marketMenu;
        }
        else if (nightMenu.activeSelf)
        {
            return nightMenu;
        }
        else if (commandMenu.activeSelf)
        {
            return commandMenu;
        }
        else if (backpackMenu.activeSelf)
        {
            return backpackMenu;
        }
        else if (shopMenu.activeSelf)
        {
            return shopMenu;
        }
        else
        {
            return inventory;
        }
    }
}
