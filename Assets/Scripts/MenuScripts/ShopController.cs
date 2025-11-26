using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private ShopableItem[] listOfItem;
    [SerializeField]
    GameObject visualContent;
    [SerializeField]
    GameObject objectToBuyPrefab;

    // Start is called before the first frame update
    void OnEnable()
    {
        foreach (Transform supprElement in visualContent.transform)
        {
            Destroy(supprElement.gameObject);
        }

        foreach (var item in listOfItem)
        {
            GameObject buyObject = Instantiate(objectToBuyPrefab, visualContent.transform);
            buyObject.name = "ObjectToBuy_" + item.itemData.GetName();

            ObjectToSell objectToSell = buyObject.GetComponent<ObjectToSell>();

            if (objectToSell != null)
            {
                objectToSell.Initialise(item);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

[System.Serializable]
public class ShopableItem
{
    public ItemData itemData;
    public bool haveCondition;
    public UnlockCondition unlockCondition;
    public Success success;
    public ItemData buyingObject;
}

public enum UnlockCondition
{
    Success,
    BuyingObject
}

[CreateAssetMenu(fileName = "Success", menuName = "success/New success")]
public class Success
{
    public string name;
    public string item;
    public Action action;
    public int iteration;
}

public enum Action
{
    Buy,
    Sell,
    Harvest,
    Plant,
    Take,
    Water
}