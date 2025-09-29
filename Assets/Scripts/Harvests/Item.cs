using UnityEngine;

//Classe pour les items
public class Item : MonoBehaviour
{
    public ItemData globalItem; // référence originale
    public ItemData item;       // copie runtime

    public void Initialize(ItemData runtimeData)
    {
        item = runtimeData;
    }
    void Start()
    {
        if (item == null && globalItem != null)
        {
            item = Instantiate(globalItem); // clone à l’instanciation
        }
    }
}
