using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Classe contenant les données du menu in-game et les fonctions de ses boutons
public class InGameMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject menuInGame;
    [SerializeField]
    private GameObject inventaire;
    [SerializeField]
    private GameObject menuOption;
    [SerializeField]
    private TextMeshProUGUI saveText;
    [SerializeField]
    private Button btnResume;
    [SerializeField]
    private Button btnSave;
    [SerializeField]
    private Button btnOption;
    [SerializeField]
    private Button btnQuit;
    [SerializeField]
    private GameController gameController;

    void Start()
    {
        saveText.text = string.Empty;
        changeLanguage();
    }

    // Fonction permettant de fermer le menu in-game
    public void onClickResume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        inventaire.SetActive(true);
        menuInGame.SetActive(false);
    }

    //Fonction permettant d'ouvrir le menu option
    public void onClickOption()
    {
        menuOption.SetActive(true);
        menuInGame.SetActive(false);
    }

    //Fonction permettant de sauvegarder les données du joueur
    public void onClickSave()
    {
        Inventory inventaire = FindAnyObjectByType<Inventory>();
        SaveInventoryManager.SaveJsonData(new List<ISaveable> { inventaire });
        saveText.text = LanguageManager.Instance.GetTranslation("saveSuccess");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerPrefs.SetFloat("playerX", player.transform.position.x);
        PlayerPrefs.SetFloat("playerY", player.transform.position.y);
        PlayerPrefs.SetFloat("playerZ", player.transform.position.z);
        PlayerPrefs.SetFloat("playerRotationY", player.transform.rotation.y);
        PlayerPrefs.SetInt("money", MainManager.Instance.GetMoney());
        PlayerPrefs.SetInt("actualDays", gameController.GetDays());
        PlayerPrefs.SetInt("moneyWin", gameController.GetMoneyWin());
    }

    //Fonction permettant de revenir au menu principal
    public void onClickQuit()
    {
        SceneManager.LoadScene("MenuScene");
    }

    //Fonction permettant d'adapter le texte en fonction de la langue choisie
    private void changeLanguage()
    {
        var textQuitter = btnQuit.GetComponentInChildren<TextMeshProUGUI>();
        var textRetourJeu = btnResume.GetComponentInChildren<TextMeshProUGUI>();
        var textOption = btnOption.GetComponentInChildren<TextMeshProUGUI>();
        var textSauvegarder = btnSave.GetComponentInChildren<TextMeshProUGUI>();

        if (LanguageManager.Instance != null)
        {
            textOption.text = LanguageManager.Instance.GetTranslation("option");
            textRetourJeu.text = LanguageManager.Instance.GetTranslation("resume");
            textQuitter.text = LanguageManager.Instance.GetTranslation("quit");
            textSauvegarder.text = LanguageManager.Instance.GetTranslation("save");
        }
    }

}
