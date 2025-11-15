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

    public void Initialise(ItemData itemData)
    {
        this.itemData = itemData;
        sprite.sprite = itemData.GetVisuel();

        if(itemData.GetItemType() == ItemType.Purchasable)
        {
            var name = itemData.GetName().Split('-');
            objectName.text = LanguageManager.Instance.GetTranslation(name[0].ToLower())+ name[1];
        }
        else
        {
            objectName.text = LanguageManager.Instance.GetTranslation(itemData.GetName().ToLower());
        }

        objectPrice = itemData.GetBuyingPrice();
        price.text = itemData.GetBuyingPrice().ToString();
    }

    public void BuyItem()
    {
        MainManager.Instance.AddItem(itemData);
        alreadyBuy.SetActive(true);
        UnlockZone(itemData);
        MainManager.Instance.SpendMoney(objectPrice);
    }

    public void UnlockZone(ItemData itemData)
    {
        GameObject[] listeOfLockItem = GameObject.FindGameObjectsWithTag("LockZone");
        foreach (GameObject lockItem in listeOfLockItem)
        {
            DisabledZone script = lockItem.GetComponent<DisabledZone>();

            if (script != null && script.GetItem().GetName() == itemData.GetName())
            {
                /*
                if(itemData.GetItemType() == ItemType.Purchasable)
                {
                    var itemName = itemData.GetName().Split('-');

                    if (lockItem.name.Contains(itemName[1]))
                    {
                        script.UnlockZone();
                    }

                }
                else
                {*/
                    script.UnlockZone();
                //}
            }
        }
    }

}
