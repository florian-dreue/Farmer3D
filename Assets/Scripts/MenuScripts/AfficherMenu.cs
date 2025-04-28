using UnityEngine;

// Classe permettant d'afficher le menu du jeu lorsque A est pressé
public class AfficherMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject inventaire;

    [SerializeField]
    private GameObject inGameMenu;

    void Update()
    {
        if (inGameMenu.activeSelf)
        {
            return;
        }
        // Vérifier si la touche "A" est pressée
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            inventaire.SetActive(false);
            inGameMenu.SetActive(true);
            //On désactive le mouvement de la caméra et on réactive la souris
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
