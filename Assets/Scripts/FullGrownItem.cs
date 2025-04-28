using UnityEngine;

//Classe pour les plants pouvant �tre r�colt�
public class FullGrownItem : MonoBehaviour
{
    //Variable contenant chaque Item pouvant �tre dropp� par le plant ainsi que leurs quantit� minimum et maximum
    [SerializeField]
    public Ressource[] harvestableItems;
    [SerializeField]
    private ItemData tool;

    //Fonction permettant de r�cup�rer l'outil n�cessair � la r�colte
    public ItemData GetToolRequired()
    {
        return tool;
    }
}

[System.Serializable]
public class Ressource
{
    public ItemData itemData;
    public int minRessource;
    public int maxRessource;

}