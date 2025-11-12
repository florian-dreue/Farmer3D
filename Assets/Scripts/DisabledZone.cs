using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledZone : MonoBehaviour
{
    [SerializeField]
    ItemData itemLock;

    public void UnlockZone()
    {
        ItemData itemToGenerate = itemLock;

        Renderer rend = GetComponent<Renderer>();
        Vector3 spawnPos = transform.position;

        if (rend != null)
        {
            float bottomY = rend.bounds.min.y;
            spawnPos = new Vector3(transform.position.x, bottomY, transform.position.z);
        }

        Instantiate(itemToGenerate.GetPrefab(), spawnPos, transform.rotation);

        Destroy(gameObject);
    }

    public ItemData GetItem()
    {
        return itemLock;
    }
}
