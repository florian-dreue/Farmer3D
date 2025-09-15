using UnityEngine;

//Classe pour le contrôle des boutons du menu de nuit
public class ButtonController : MonoBehaviour
{
    [SerializeField]
    private GameObject menuDeNuit;
    [SerializeField]
    private GameObject inventaire;
    [SerializeField]
    private Transform playerBody;

    //Fonction pour le click du bouton quitter
    public void CloseButtonClick()
    {
        //On désactive le menu de nuit et on réactive l'affichage de l'inventaire
        menuDeNuit.SetActive(false);
        inventaire.SetActive(true);
        //On désactive le curseur et on ractive le mouvement de caméra
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;

        //On applique une rotation au joueur afin qu'il se retrouve dos à la ferme
        playerBody.rotation = Quaternion.Euler(new Vector3(0f, -120f, 0f)); ;
    }
}
