using UnityEngine;
using UnityEngine.UI;

// Classe contenant les données des slots d'outils
public class ToolSlot : MonoBehaviour
{
    [SerializeField]
    private ItemData item;
    [SerializeField]
    private Image itemVisual;
    [SerializeField]
    private Sprite emptySlotVisual;

    public ItemData getItem()
    {
        return item;
    }

    public void setItem(ItemData item)
    {
        this.item = item;
        this.itemVisual.sprite = item.visuel;
    }

    public void EmptySlot()
    {
        this.item = null;
        this.itemVisual.sprite = emptySlotVisual;
    }

}
