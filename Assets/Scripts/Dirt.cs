using UnityEngine;

//Classe permettant de g�rer les pr�fab de terre
public class Dirt : MonoBehaviour
{
    [SerializeField]
    public GameObject dirtObject;
    [SerializeField]
    public GameObject dirtPlowedObject;

    public bool plowed = false;

    //Fonction pour labourr� la terre
    public void isGettingPlowed()
    {
        plowed = true;
        //On cative le prefab de la terre labourr� et on d�sactive le pr�fab de terre non labour�
        dirtPlowedObject.SetActive(true);
        dirtObject.SetActive(false);
    }

    //Fonction permettant de remettre la terre � son �tat initial
    public void Reinisialised()
    {
        plowed = false;
        dirtPlowedObject.SetActive(false);
        dirtObject.SetActive(true);
    }
}
