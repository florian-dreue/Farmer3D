using UnityEngine;

//Classe permettant de gérer les préfab de terre
public class Dirt : MonoBehaviour
{
    [SerializeField]
    private GameObject dirtObject;
    [SerializeField]
    private GameObject dirtPlowedObject;
    [SerializeField]
    private GameObject dirtWateredObject;

    private bool plowed = false;
    private bool watered = false;

    //Fonction pour labourrer la terre
    public void isGettingPlowed()
    {
        plowed = true;
        //On active le prefab de la terre labourré et on désactive le préfab de terre non labouré
        dirtPlowedObject.SetActive(true);
        dirtObject.SetActive(false);
    }

    //Fonction pour arroser la terre
    public void isGettingWatered()
    {
        watered = true;
        //On active le prefab de la terre humide et on désactive le préfab de terre sèche
        dirtWateredObject.SetActive(true);
        dirtPlowedObject.SetActive(false);
    }

    //Fonction pour assécher la terre
    public void isGettingDrained()
    {
        watered = false;
        //On active le prefab de la terre sèche et on désactive le préfab de terre humide
        dirtWateredObject.SetActive(false);
        dirtPlowedObject.SetActive(true);
    }

    //Fonction permettant de remettre la terre à son état initial
    public void Reinisialised()
    {
        plowed = false;
        dirtPlowedObject.SetActive(false);
        dirtObject.SetActive(true);
    }


    public bool getPlowed() { return plowed; }

    public bool getWatered() { return watered; }

}
