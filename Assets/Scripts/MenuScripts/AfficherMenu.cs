using UnityEngine;

// Classe permettant d'afficher le menu du jeu lorsque A est press�
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
        // V�rifier si la touche "A" est press�e
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            inventaire.SetActive(false);
            inGameMenu.SetActive(true);
            //On d�sactive le mouvement de la cam�ra et on r�active la souris
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
