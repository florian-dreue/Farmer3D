using UnityEngine;

//Classe permettant de gérer les préfab de terre
public class Dirt : MonoBehaviour
{
    [SerializeField]
    public GameObject dirtObject;
    [SerializeField]
    public GameObject dirtPlowedObject;

    public bool plowed = false;

    //Fonction pour labourré la terre
    public void isGettingPlowed()
    {
        plowed = true;
        //On cative le prefab de la terre labourré et on désactive le préfab de terre non labouré
        dirtPlowedObject.SetActive(true);
        dirtObject.SetActive(false);
    }

    //Fonction permettant de remettre la terre à son état initial
    public void Reinisialised()
    {
        plowed = false;
        dirtPlowedObject.SetActive(false);
        dirtObject.SetActive(true);
    }
}
