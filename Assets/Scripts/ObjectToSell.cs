using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectToSell : MonoBehaviour
{
    private GameController gameController;

    [SerializeField]
    private Image sprite;

    [SerializeField]
    private TextMeshProUGUI objectName;

    [SerializeField]
    private TextMeshProUGUI price;

    [SerializeField]
    private Button button;

    [SerializeField]
    private GameObject notEnoughMoney;

    [SerializeField]
    private GameObject alreadyBuy;

    private ItemData itemData;

    private ShopableItem shopableItem;

    private int objectPrice;

    private void Start()
    {
        gameController = GetComponent<GameController>();
    }

    private void Update()
    {
        List<ItemData> itemList = MainManager.Instance.GetItemUnlock();

        if (shopableItem.haveCondition)
        {
            switch (shopableItem.unlockCondition)
            {
                case UnlockCondition.Success: break;
                case UnlockCondition.BuyingObject:
                    ItemData itemToUnlock = shopableItem.buyingObject;
                    if (!itemList.Contains(itemToUnlock))
                    {
                        alreadyBuy.SetActive(true);
                        notEnoughMoney.SetActive(false);
                        button.GetComponent<Image>().color = Color.red;
                    }
                    else
                    {
                        ManageVisual();
                    }
                    break;
                default: break;
            }
        }
        else
        {
            ManageVisual();
        }
    }

    private void ManageVisual()
    {
        List<ItemData> itemList = MainManager.Instance.GetItemUnlock();

        if (itemList != null && itemList.Contains(itemData))
        {
            alreadyBuy.SetActive(true);
            notEnoughMoney.SetActive(false);
            button.GetComponent<Image>().color = Color.red;
        }
        else
        {
            if (MainManager.Instance.GetMoney() >= objectPrice)
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

    public void Initialise(ShopableItem shopableItem)
    {
        this.shopableItem = shopableItem;
        this.itemData = shopableItem.itemData;
        sprite.sprite = itemData.GetVisuel();

        if(itemData.GetItemType() == ItemType.Purchasable)
        {
            var name = itemData.GetName().Split('-');
            objectName.text = LanguageManager.Instance.GetTranslation(name[0].ToLower()) + name[1];
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
        if (itemData.GetItemType() == ItemType.Destroyable)
        {
            DestroyZone();
        }
        else
        {
            UnlockZone();
        }
        
        MainManager.Instance.SpendMoney(objectPrice);
    }

    public void UnlockZone()
    {
        GameObject[] listeOfLockItem = GameObject.FindGameObjectsWithTag("LockZone");
        foreach (GameObject lockItem in listeOfLockItem)
        {
            DisabledZone script = lockItem.GetComponent<DisabledZone>();

            if (script != null && script.GetItem().GetName() == itemData.GetName())
            {
                script.UnlockZone();
            }
        }
    }

    public void DestroyZone()
    {
        GameObject[] listeOfDestroyItem = GameObject.FindGameObjectsWithTag("DestroyZone");
        foreach (GameObject destroyItem in listeOfDestroyItem)
        {
            DestroyZone script = destroyItem.GetComponent<DestroyZone>();

            if (script != null && script.GetItem().GetName() == itemData.GetName())
            {
                script.DestroyObject();
            }
        }
    }

}
