using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectToSell : MonoBehaviour
{
    GameController gameController;

    [SerializeField]
    Image sprite;

    [SerializeField]
    TextMeshProUGUI objectName;

    [SerializeField]
    TextMeshProUGUI price;

    [SerializeField]
    Button button;

    [SerializeField]
    GameObject notEnoughMoney;

    [SerializeField]
    GameObject alreadyBuy;


    ItemData itemData;
    int objectPrice;

    private void Start()
    {
        gameController = GetComponent<GameController>();
    }

    private void Update()
    {
        List<ItemData> itemList = MainManager.Instance.GetItemUnlock();
        if(itemList != null && itemList.Contains(itemData))
        {
            alreadyBuy.SetActive(true);
            notEnoughMoney.SetActive(false);
            button.GetComponent<Image>().color = Color.red;
        }
        else
        {
            if(MainManager.Instance.GetMoney() >= objectPrice)
            {
                alreadyBuy.SetActive(false);
                notEnoughMoney.SetActive(false);
                button.GetComponent<Image>().color = Color.green;
            }
            else
            {
                alreadyBuy.SetActive(false);
                notEnoughMoney.SetActive(true);
                button.GetComponent<Image>().color = Color.red;
            }
        }
    }

    public void Initialise(ItemData itemData, int itemPrice)
    {
        this.itemData = itemData;
        sprite.sprite = itemData.GetVisuel();
        objectName.text = itemData.GetName();
        objectPrice = itemPrice;
        price.text = itemPrice.ToString();
    }

    public void BuyItem()
    {
        MainManager.Instance.AddItem(itemData);
        alreadyBuy.SetActive(true);
        UnlockZone(itemData);
    }

    public void UnlockZone(ItemData itemData)
    {
        GameObject[] listeOfLockItem = GameObject.FindGameObjectsWithTag("LockZone");
        foreach (GameObject lockItem in listeOfLockItem)
        {
            DisabledZone script = lockItem.GetComponent<DisabledZone>();
            //Si on as quelque chose de planté, on ajoute un jour à la culture.
            if (script != null && script.GetItem().GetName() == itemData.GetName())
            {
                script.UnlockZone();
            }
        }
    }

}
