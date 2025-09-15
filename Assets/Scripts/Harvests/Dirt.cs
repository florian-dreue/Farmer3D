using UnityEngine;

//Classe permettant de g�rer les pr�fab de terre
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
        //On active le prefab de la terre labourr� et on d�sactive le pr�fab de terre non labour�
        dirtPlowedObject.SetActive(true);
        dirtObject.SetActive(false);
    }

    //Fonction pour arroser la terre
    public void isGettingWatered()
    {
        watered = true;
        //On active le prefab de la terre humide et on d�sactive le pr�fab de terre s�che
        dirtWateredObject.SetActive(true);
        dirtPlowedObject.SetActive(false);
    }

    //Fonction pour ass�cher la terre
    public void isGettingDrained()
    {
        watered = false;
        //On active le prefab de la terre s�che et on d�sactive le pr�fab de terre humide
        dirtWateredObject.SetActive(false);
        dirtPlowedObject.SetActive(true);
    }

    //Fonction permettant de remettre la terre � son �tat initial
    public void Reinisialised()
    {
        plowed = false;
        dirtPlowedObject.SetActive(false);
        dirtObject.SetActive(true);
    }


    public bool getPlowed() { return plowed; }

    public bool getWatered() { return watered; }

}
