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
    private GameController gameController;

    private GameObject lastMenu;

    private void Start()
    {
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
        else
        {
            return inventory;
        }
    }
}
