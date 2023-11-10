using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<Item> items = new ();
    [SerializeField] private int index = 1;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI description;

    
    private void Start()
    {
        index = 1;
        
        items.Add(new Helmet(){name = "Heaume", price = 150, description = "Un casque des plus classiques..."});
        items.Add(new Helmet(){name = "Casque d'Or", price = 500, description = "Casque ultra résistant, vous donne un style incroyable !!"});
        items.Add(new LovePotion(){name = "Séductor3000", price = 90});
        items.Add(new CheeseChestPlate(){name = "Plastron Cheddar", price = 5});
        
        name.text = items[0].name;
        description.text = items[0].description;
    }

    public void LeftClick()
    {
        if (index > 1)
        {
            index--;
            UpdateUI();
        }
        else
        {
            index = items.Count;
            UpdateUI();
        }
    }

    public void RightClick()
    {
        if (index < items.Count)
        {
            index++;
            UpdateUI();
        }
        else
        {
            index = 1;
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        name.text = items[index - 1].name;
        description.text = items[index - 1].description;
    }
}




[Serializable]
public class Item
{
    public string name;
    public string description;
    public int price;
}

public class Helmet : Item, IWearable
{
    
}

public class LovePotion : Item, IEatable, IInspectable
{
    public void ConsumeItem()
    {
        Debug.Log("I'm in love with you now");
    }
}

public class CheeseChestPlate : Item, IEatable, IWearable
{
    public void ConsumeItem()
    {
        Debug.Log("Mmmmh, so good !");
    }
}
    

