using UnityEngine;

//Classe permettant de gérer les préfab de terre
public class TreeDirt : MonoBehaviour
{
    [SerializeField]
    private GameObject dirtObject;
    [SerializeField]
    private GameObject dirtWateredObject;

    private bool watered = false;


    //Fonction pour arroser la terre
    public void isGettingWatered()
    {
        watered = true;
        //On active le prefab de la terre humide et on désactive le préfab de terre sèche
        dirtWateredObject.SetActive(true);
        dirtObject.SetActive(false);
    }

    //Fonction pour assécher la terre
    public void isGettingDrained()
    {
        watered = false;
        //On active le prefab de la terre sèche et on désactive le préfab de terre humide
        dirtWateredObject.SetActive(false);
        dirtObject.SetActive(true);
    }

    //Fonction permettant de remettre la terre à son état initial
    public void Reinisialised()
    {
        watered = false;
        dirtObject.SetActive(false);
        dirtObject.SetActive(true);
    }

    public bool getWatered() { return watered; }

}
