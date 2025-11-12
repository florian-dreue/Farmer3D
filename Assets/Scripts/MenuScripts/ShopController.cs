using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private ItemToBuy[] listOfItem;
    [SerializeField]
    GameObject visualContent;
    [SerializeField]
    GameObject objectToBuyPrefab;

    // Start is called before the first frame update
    void OnEnable()
    {
        foreach (var item in listOfItem)
        {
            GameObject buyObject = Instantiate(objectToBuyPrefab, visualContent.transform);
            buyObject.name = "ObjectToBuy_" + item.itemData.GetName();

            ObjectToSell objectToSell = buyObject.GetComponent<ObjectToSell>();

            if (objectToSell != null)
            {
                objectToSell.Initialise(item.itemData, item.price);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnlockZone(ItemData itemData)
    {
        GameObject[] listeOfLockItem = GameObject.FindGameObjectsWithTag("LockZone");
        foreach (GameObject lockItem in listeOfLockItem)
        {
            DisabledZone script = lockItem.GetComponent<DisabledZone>();
            //Si on as quelque chose de planté, on ajoute un jour à la culture.
            if (script != null && script.GetItem().GetName() == itemData.GetName())
            {
                script.UnlockZone();
            }
        }
    }

}

[System.Serializable]
public class ItemToBuy
{
    public ItemData itemData;
    public int price;
}
