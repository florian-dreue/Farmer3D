using UnityEngine;
using UnityEngine.UI;

// Classe contenant les données des slots de l'inventaire
public class Slot : MonoBehaviour
{
    private ItemData item;
    [SerializeField]
    private Image itemVisual;
    [SerializeField]
    private Text countText;
    [SerializeField]
    private Sprite emptySlot;

    public void SetSlot(ItemData item)
    {
        this.item = item;
        itemVisual.sprite = item.visuel;
        if(item.stackable)
        {
            countText.enabled = true;
            countText.text = "1";
        }
        else
        {
            countText.enabled = false;
        }
    }

    public void SetText(string numberOfItem)
    {
        countText.text = numberOfItem;
    }

    public ItemData GetItem()
    {
        return item;
    }

    public void EmptySlot()
    {
        item = null;
        countText.text = "";
        countText.enabled = false;
        itemVisual.sprite = emptySlot;
    }

    public bool ItemStackable()
    {
        return item.stackable;
    }

}
