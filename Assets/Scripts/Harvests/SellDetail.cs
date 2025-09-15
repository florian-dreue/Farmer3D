using System;
using System.Runtime.CompilerServices;
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
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private Button plusButton;
    [SerializeField]
    private Button minusButton;

    private int initialPrice = 0;
    private int newPrice = 0;
    private int maxQuantity = 0;
    private int quantity = 0;

    public void Start()
    {
        inputField.onValueChanged.AddListener(UpdateQuantityValue);
        plusButton.onClick.AddListener(addQuantity);
        minusButton.onClick.AddListener(subtractQuantity);
        inputField.text = "0";
    }

    //Attribut la bonne valeur au détail de la vente
    public void setDetail(string detail)
    {
        sellDetail.text = detail;
        sellDetail.enabled = true;
    }

    //Attribut la bonne valeur au prix du détail de la vente
    public void setPrice(int price)
    {
        /*sellDetailPrice.text = price;
        sellDetailPrice.enabled = true;
        coinImage.enabled = true;*/

        initialPrice = price;
    }

    public void setQuantityInInventory(int quantity)
    {
        maxQuantity = quantity;
    }

    //Réinitialise l'affichage
    public void clearDetail()
    {
        sellDetail.text = "";
        sellDetail.enabled = false;
        sellDetailPrice.text = "";
        sellDetailPrice.enabled = false;
        coinImage.enabled = false;
    }

    //Change le prix affiché en fonction de la quantité entrée
    private void UpdateQuantityValue(string value)
    {
        if (int.TryParse(value, out int quantity))
        {
            if (quantity < 0)
            {
                quantity = 0;
                inputField.text = quantity.ToString();
            }
            if(quantity > maxQuantity)
            {
                quantity = maxQuantity;
                inputField.text = quantity.ToString();
            }
            this.quantity = quantity;
            newPrice = quantity * initialPrice;
            sellDetailPrice.text = newPrice.ToString();
        }
        else
        {
            Console.WriteLine("Invalid input for quantity.");
        }
        /*
        else
        {
            inputField.text = "0";
            newPrice = 0;
            sellDetailPrice.text = newPrice.ToString();
        }*/
    }

    public int getPrice()
    {
        return newPrice;
    }

    public int getQuantity()
    {
        return quantity;
    }

    private void addQuantity()
    {
        int currentQuantity = quantity;
        if (currentQuantity < maxQuantity)
        {
            currentQuantity++;
            inputField.text = currentQuantity.ToString();
            UpdateQuantityValue(currentQuantity.ToString());
        }
    }

    private void subtractQuantity()
    {
        int currentQuantity = quantity;
        if (currentQuantity > 0)
        {
            currentQuantity--;
            inputField.text = currentQuantity.ToString();
            UpdateQuantityValue(currentQuantity.ToString());
        }
    }

}
