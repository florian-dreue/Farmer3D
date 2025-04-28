using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellDetail : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI sellDetail;
    [SerializeField]
    private TextMeshProUGUI sellDetailPrice;
    [SerializeField]
    private Image coinImage;

    public void setDetail(string detail)
    {
        sellDetail.text = detail;
        sellDetail.enabled = true;
    }

    public void setPrice(string price)
    {
        sellDetailPrice.text = price;
        sellDetailPrice.enabled = true;
        coinImage.enabled = true;
    }

    public void clearDetail()
    {
        sellDetail.text = "";
        sellDetail.enabled = false;
        sellDetailPrice.text = "";
        sellDetailPrice.enabled = false;
        coinImage.enabled = false;
    }
}
