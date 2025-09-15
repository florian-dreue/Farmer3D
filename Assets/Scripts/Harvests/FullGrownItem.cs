using UnityEngine;

//Classe pour les plants pouvant être récolté
public class FullGrownItem : MonoBehaviour
{
    //Variable contenant chaque Item pouvant être droppé par le plant ainsi que leurs quantité minimum et maximum
    [SerializeField]
    public Ressource[] harvestableItems;
    [SerializeField]
    private ItemData tool;

    //Fonction permettant de récupèrer l'outil nécessair à la récolte
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