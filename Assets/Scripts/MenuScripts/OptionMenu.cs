using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Classe permettant de gérer le menu des options
public class OptionMenu : MonoBehaviour
{
    [SerializeField]
    private Button btnRetourMenu;

    [SerializeField]
    private TextMeshProUGUI memoCommandesText;

    [SerializeField]
    private TextMeshProUGUI forward;

    [SerializeField]
    private TextMeshProUGUI moveBack;

    [SerializeField]
    private TextMeshProUGUI left;

    [SerializeField]
    private TextMeshProUGUI right;

    [SerializeField]
    private TextMeshProUGUI showMenu;

    [SerializeField]
    private TextMeshProUGUI pickup;

    [SerializeField]
    private GameObject optionMenu;

    [SerializeField]
    private GameObject inGameMenu;


    void Start()
    {
        changeAffichage();
    }

    // Fonction permettant de retourner au menu in game
    public void retourMenu()
    {
        //Cacher les commandes
        if (optionMenu != null)
        {
            optionMenu.SetActive(false);
        }
        //Afficher le menu
        if (inGameMenu != null)
        {
            inGameMenu.SetActive(true);
        }
    }

    // Fonction permettant de changer l'affichage des commandes en fonction de la langue
    private void changeAffichage()
    {
        var textRetour = btnRetourMenu.GetComponentInChildren<TextMeshProUGUI>();
        
        if (LanguageManager.Instance != null)
        {
            textRetour.text = LanguageManager.Instance.GetTranslation("backMenu");
            memoCommandesText.text = LanguageManager.Instance.GetTranslation("commands");
            forward.text = LanguageManager.Instance.GetTranslation("forward");
            moveBack.text = LanguageManager.Instance.GetTranslation("moveBack");
            left.text = LanguageManager.Instance.GetTranslation("left");
            right.text = LanguageManager.Instance.GetTranslation("right");
            showMenu.text = LanguageManager.Instance.GetTranslation("showMenu");
            pickup.text = LanguageManager.Instance.GetTranslation("pickup");
        }
        else
        {
            print("Pas de LanguageManager");
        }
    }
}
