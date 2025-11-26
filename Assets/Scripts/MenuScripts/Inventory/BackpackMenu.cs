using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private GameObject visualContent;
    [SerializeField]
    private Inventory inventory;

    private void OnEnable()
    {
        List<ItemInInventory> itemList = inventory.GetBackpackContent();

        foreach (Transform supprElement in visualContent.transform)
        {
            Destroy(supprElement.gameObject);
        }

        foreach (var item in itemList)
        {
            GameObject slot = Instantiate(slotPrefab, visualContent.transform);
            slot.name = "Slot_" + item.itemData.GetName();

            Slot slotObject = slot.GetComponent<Slot>();
            if (slotObject != null)
            {
                slotObject.SetSlot(item);
            }
        }

        for (int i = 0; i < inventory.GetBackpackSize() - itemList.Count; i++)
        {
            GameObject slot = Instantiate(slotPrefab, visualContent.transform);
            slot.name = "Slot_Vide_" + i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
