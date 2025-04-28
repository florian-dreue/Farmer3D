using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Classe contenant les données du menu de la scène d'accueil
public class MenuScript : MonoBehaviour
{
    [SerializeField] 
    private Button btnJouer;

    [SerializeField]
    private Button btnCommandes;

    [SerializeField]
    private Button btnQuitter;

    [SerializeField]
    private Button btnRetourMenu;

    [SerializeField]
    private Button btnRetourMenuLanguage;

    [SerializeField]
    private Button btnLanguage;

    [SerializeField]
    private TextMeshProUGUI welcomeText;

    [SerializeField]
    private TextMeshProUGUI memoCommandesText;

    [SerializeField]
    private TextMeshProUGUI changeLanguage;

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
    private TextMeshProUGUI drop;

    [SerializeField]
    private GameObject memoCommandes;

    [SerializeField]
    private GameObject menuPrincipal;

    [SerializeField]
    private GameObject menuLanguage;

    public void Start()
    {
        changeAffichage();
    }

    // Fonction pour lancer le jeu
    public void jouer() {
        // Charger la scène "SampleScene"
        SceneManager.LoadScene("SampleScene");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    //Fonction pour afficher les commandes
    public void afficherMemoCommandes() {
        // Afficher le Canvas des commandes
        if (memoCommandes != null) {
            memoCommandes.SetActive(true);
        }
        //Cacher le menu
        if (menuPrincipal != null) {
            menuPrincipal.SetActive(false);
        }
    }

    //Fonction pour retourner au menu principal
    public void retourMenu() {
        //Cacher les commandes
        if (memoCommandes != null){
            memoCommandes.SetActive(false);
        }
        //Cacher les commandes
        if (menuLanguage != null)
        {
            menuLanguage.SetActive(false);
        }
        //Afficher le menu
        if (menuPrincipal != null){
            menuPrincipal.SetActive(true);
        }
    }

    //Fonction pour afficher le menu de changement de langue
    public void selectLanguage()
    {
        if (memoCommandes != null)
        {
            memoCommandes.SetActive(false);
        }
        //Afficher le menu
        if (menuLanguage != null)
        {
            menuLanguage.SetActive(true);
        }
    }

    //Fonction pour changer la langue en français
    public void selectFr()
    {
        LanguageManager.Instance.SetLanguage("fr");
        changeAffichage();
    }

    //Fonction pour changer la langue en anglais
    public void selectEn()
    {
        LanguageManager.Instance.SetLanguage("en");
        changeAffichage();
    }

    //Fonction pour changer la langue en espagnol
    public void selectEsp()
    {
        LanguageManager.Instance.SetLanguage("es");
        changeAffichage();
    }

    // Fonction pour quitter le jeu
    public void quitter() {
        // Quitter le jeu
        Application.Quit();

        // Note : La fonction Application.Quit() ne fonctionne pas dans l'éditeur Unity.
        // Pour tester dans l'éditeur, vous pouvez utiliser cette ligne :
        Debug.Log("Quitter le jeu !");
    }

    //Fonction pour changer l'affichage des textes des menus en fonction de la langue
    private void changeAffichage()
    {
        var textJouer = btnJouer.GetComponentInChildren<TextMeshProUGUI>();
        var textCommandes = btnCommandes.GetComponentInChildren<TextMeshProUGUI>();
        var textQuitter = btnQuitter.GetComponentInChildren<TextMeshProUGUI>();
        var textRetour = btnRetourMenu.GetComponentInChildren<TextMeshProUGUI>();
        var textRetourLanguage = btnRetourMenuLanguage.GetComponentInChildren<TextMeshProUGUI>();
        var textLanguage = btnLanguage.GetComponentInChildren<TextMeshProUGUI>();
        if (LanguageManager.Instance != null)
        {
            textJouer.text = LanguageManager.Instance.GetTranslation("play");
            textCommandes.text = LanguageManager.Instance.GetTranslation("option");
            textQuitter.text = LanguageManager.Instance.GetTranslation("exit");
            textRetour.text = LanguageManager.Instance.GetTranslation("backMenu");
            textRetourLanguage.text = LanguageManager.Instance.GetTranslation("backMenu");
            textLanguage.text = LanguageManager.Instance.GetTranslation("languageBtn");
            welcomeText.text = LanguageManager.Instance.GetTranslation("welcome");
            memoCommandesText.text = LanguageManager.Instance.GetTranslation("commands");
            changeLanguage.text = LanguageManager.Instance.GetTranslation("changeLanguage");
            forward.text = LanguageManager.Instance.GetTranslation("forward");
            moveBack.text = LanguageManager.Instance.GetTranslation("moveBack");
            left.text = LanguageManager.Instance.GetTranslation("left");
            right.text = LanguageManager.Instance.GetTranslation("right");
            showMenu.text = LanguageManager.Instance.GetTranslation("showMenu");
            pickup.text = LanguageManager.Instance.GetTranslation("pickup");
            drop.text = LanguageManager.Instance.GetTranslation("drop");
        }
        else
        {
            print("Pas de LanguageManager");
        }
    }
}
