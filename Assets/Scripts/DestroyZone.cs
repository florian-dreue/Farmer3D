using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    [SerializeField]
    GameObject objectToDestroy;
    [SerializeField]
    ItemData itemData;

    public void DestroyObject()
    {
        Destroy(objectToDestroy);
        Destroy(gameObject);
    }

    public ItemData GetItem()
    {
        return itemData;
    }
}
