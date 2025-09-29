using UnityEngine;
using UnityEngine.UI;

// Classe contenant les données des slots d'outils
public class ToolSlot : MonoBehaviour
{
    private ItemData item;
    [SerializeField]
    private Image itemVisual;
    [SerializeField]
    private Sprite emptySlotVisual;
    [SerializeField]
    private Image baseCapacity;
    [SerializeField]
    private Image fillingVisual;

    public ItemData getItem()
    {
        return item;
    }

    public void setItem(ItemData item)
    {
        this.item = item;
        itemVisual.sprite = item.GetVisuel();

        FillableData fillableData = item as FillableData;
        if (fillableData != null)
        {
            baseCapacity.gameObject.SetActive(true);
            fillingVisual.gameObject.SetActive(true);

            /*RectTransform rt = fillingVisual.rectTransform;
            rt.sizeDelta = new Vector2(50, rt.sizeDelta.y);*/
            fillingVisual.fillAmount = (float)fillableData.GetFilling() / fillableData.GetMaxFilling();
        }
        else
        {
            baseCapacity.gameObject.SetActive(false);
            fillingVisual.gameObject.SetActive(false);
        }
    }

    public void updateItem(ItemData item)
    {
        FillableData fillableData = item as FillableData;
        if (fillableData != null)
        {
            fillingVisual.fillAmount = (float)fillableData.GetFilling() / fillableData.GetMaxFilling();
        }
    }

    public void EmptySlot()
    {
        item = null;
        itemVisual.sprite = emptySlotVisual;
        baseCapacity.gameObject.SetActive(false);
        fillingVisual.gameObject.SetActive(false);
    }

}
