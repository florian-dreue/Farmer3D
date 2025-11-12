using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private ItemData[] listOfItem;
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
            buyObject.name = "ObjectToBuy_" + item.GetName();

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
