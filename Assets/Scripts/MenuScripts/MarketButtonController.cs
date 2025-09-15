using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

//Classe pour le contr�le du bouton de la fen�tre de vente
public class MarketButtonController : MonoBehaviour
{
    [SerializeField]
    public GameObject menuMarket;
    [SerializeField]
    private GameObject menuInventaire;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private TextMeshProUGUI amountText;
    private int amount = 0;
    [SerializeField]
    private Button validButton;
    [SerializeField]
    private Button sellAllButton;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private GameObject detailContainer;

    private SellDetail[] listOfDetail;

    [SerializeField]
    private Transform scrollViewContent;
    [SerializeField]
    private GameObject SellDetailPrefab;

    [SerializeField]
    private GameController gameController;

    private List<GameObject> SellDetailsList = new List<GameObject>();

    //Fonction pour le d�marrage de la fen�tre de vente
    public void Start()
    {
        foreach (GameObject obj in SellDetailsList)
        {
            Destroy(obj);
        }

        SellDetailsList.Clear();

        //On change les textes en fonction de la langue et on affiche le montant total de la vente
        //amountText.text = LanguageManager.Instance.GetTranslation("sellInventory") + inventory.GetSellAmount();
        var textValid = validButton.GetComponentInChildren<TextMeshProUGUI>();
        var textSellAll = sellAllButton.GetComponentInChildren<TextMeshProUGUI>();
        var textCancel = cancelButton.GetComponentInChildren<TextMeshProUGUI>();
        textValid.text = LanguageManager.Instance.GetTranslation("sell");
        textCancel.text = LanguageManager.Instance.GetTranslation("cancel");
        textSellAll.text = LanguageManager.Instance.GetTranslation("sellAll");

        // var listOfDetail = detailContainer.GetComponentsInChildren<SellDetail>();
        //this.listOfDetail = listOfDetail;
        var listOfItem = inventory.GetContent();
        /*for (int i = 0; i < listOfDetail.Length; i++)
        {
            if (i >= listOfItem.Count)
            {
                listOfDetail[i].clearDetail();
            }
            else
            {
                var nameItem = listOfItem[i].count > 1 ? LanguageManager.Instance.GetTranslation(listOfItem[i].itemData.name.ToLower()+"Plural") : LanguageManager.Instance.GetTranslation(listOfItem[i].itemData.name.ToLower());
                listOfDetail[i].setDetail(listOfItem[i].count + " " + nameItem);
                listOfDetail[i].setPrice(listOfItem[i].itemData.price);
                listOfDetail[i].setQuantityInInventory(listOfItem[i].count);
            }
        }*/

        for (int i = 0; i < listOfItem.Count; i++)
        {
            GameObject instance = Instantiate(SellDetailPrefab, scrollViewContent);
            SellDetailsList.Add(instance);
            SellDetail detail = instance.GetComponent<SellDetail>();

            var nameItem = listOfItem[i].count > 1 ? LanguageManager.Instance.GetTranslation(listOfItem[i].itemData.name.ToLower() + "Plural") : LanguageManager.Instance.GetTranslation(listOfItem[i].itemData.name.ToLower());
            detail.setDetail(listOfItem[i].count + " " + nameItem);
            detail.setPrice(listOfItem[i].itemData.price);
            detail.setQuantityInInventory(listOfItem[i].count);
        }
    }

    public void Update()
    {
        int amount = 0;

        for (int i = 0; i < SellDetailsList.Count; i++)
        {
            SellDetail detail = SellDetailsList[i].GetComponent<SellDetail>();
            amount += detail.getPrice();
        }

        amountText.text = LanguageManager.Instance.GetTranslation("sellInventory") + amount;
        this.amount = amount;
    }

    public void OnEnable()
    {
        foreach (GameObject obj in SellDetailsList)
        {
            Destroy(obj);
        }

        SellDetailsList.Clear();

        //var listOfDetail = detailContainer.GetComponentsInChildren<SellDetail>();
        //this.listOfDetail = listOfDetail;
        var listOfItem = inventory.GetContent();
        /*for (int i = 0; i < listOfDetail.Length; i++)
        {
            if (i >= listOfItem.Count)
            {
                listOfDetail[i].clearDetail();
            }
            else
            {
                var nameItem = listOfItem[i].count > 1 ? LanguageManager.Instance.GetTranslation(listOfItem[i].itemData.name.ToLower() + "Plural") : LanguageManager.Instance.GetTranslation(listOfItem[i].itemData.name.ToLower());
                listOfDetail[i].setDetail(listOfItem[i].count + " " + nameItem);
                listOfDetail[i].setPrice(listOfItem[i].itemData.price);
                listOfDetail[i].setQuantityInInventory(listOfItem[i].count);
            }
        }*/

        for (int i = 0; i < listOfItem.Count; i++)
        {
            GameObject instance = Instantiate(SellDetailPrefab, scrollViewContent);
            SellDetailsList.Add(instance);
            SellDetail detail = instance.GetComponent<SellDetail>();

            var nameItem = listOfItem[i].count > 1 ? LanguageManager.Instance.GetTranslation(listOfItem[i].itemData.name.ToLower() + "Plural") : LanguageManager.Instance.GetTranslation(listOfItem[i].itemData.name.ToLower());
            detail.setDetail(listOfItem[i].count + " " + nameItem);
            detail.setPrice(listOfItem[i].itemData.price);
            detail.setQuantityInInventory(listOfItem[i].count);
        }
    }

    //Fonction pour le click du bouton vendre
    public void ValidateButtonClick()
    {
        gameController.AddMoney(amount);
        var listOfItem = inventory.GetContent();
        for (int i = 0; i < SellDetailsList.Count; i++)
        {
            SellDetail detail = SellDetailsList[i].GetComponent<SellDetail>();

            if (i >= listOfItem.Count)
            {
                detail.clearDetail();
            }
            else
            {
                inventory.SubstractItem(listOfItem[i].itemData, detail.getQuantity());
            }
        }

        //inventory.Sell();
        //On d�sactive le menu de vente et on r�active l'affichage de l'inventaire
        menuMarket.SetActive(false);
        menuInventaire.SetActive(true);
        //On d�sactive le curseur et on ractive le mouvement de cam�ra
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    //Fonction pour le click du bouton vendre tout
    public void SellAllButtonClick()
    {
        inventory.Sell();
        //On d�sactive le menu de vente et on r�active l'affichage de l'inventaire
        menuMarket.SetActive(false);
        menuInventaire.SetActive(true);
        //On d�sactive le curseur et on ractive le mouvement de cam�ra
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    //Fonction pour le click du bouton annuler
    public void CancelButtonClick()
    {
        menuMarket.SetActive(false);
        menuInventaire.SetActive(true);
        //On d�sactive le curseur et on ractive le mouvement de cam�ra
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
}
