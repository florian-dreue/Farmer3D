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

        if (itemLock.GetItemType() != ItemType.Purchasable)
        {
            var rotation = transform.rotation;
            if (itemLock.GetItemType() == ItemType.Seed)
            {
                Vector3 euler = rotation.eulerAngles;
                euler.y += -89.417f; // tu peux mettre la valeur que tu veux
                rotation = Quaternion.Euler(euler);
            }
            Instantiate(itemToGenerate.GetPrefab(), spawnPos, rotation);
        }

        Destroy(gameObject);
    }

    public ItemData GetItem()
    {
        return itemLock;
    }
}
