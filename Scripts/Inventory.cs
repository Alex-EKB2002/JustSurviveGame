using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryObject;
    public float distance = 2f;

    public Slot[] slots;

    public Slot[] equipSlots;

    void Start()
    {
        inventoryObject.SetActive(false);

        foreach(Slot i in slots)
        {
            i.CustomStart();
        }
        foreach (Slot i in equipSlots)
            i.CustomStart();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(inventoryObject.activeSelf == false)
            {
                inventoryObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                gameObject.GetComponent<FirstPersonController>().enabled = false;
            }
            else
            {
                inventoryObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                gameObject.GetComponent<FirstPersonController>().enabled = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.collider.gameObject.GetComponent<Item>())
                    AddItem(hit.collider.gameObject.GetComponent<Item>());

            }
        }

        foreach(Slot i in slots)
        {
            i.CheckForItem();
        }
        foreach (Slot i in equipSlots)
            i.CheckForItem();
    }

    public int GetItemAmount(int id)
    {
        int num = 0;
        foreach(Slot i in slots)
        {
            if (i.slotsItem)
            {
                Item z = i.slotsItem;
                if (z.itemID == id)
                    num += z.amountInStack;
            }
        }
        return num;
    }

    public void RemoveItemAmount(int id, int amountToRemove)
    {
        foreach(Slot i in slots)
        {
            if (amountToRemove <= 0)
                return;

            if (i.slotsItem)
            {
                Item z = i.slotsItem;
                if(z.itemID == id)
                {
                    int amountThatCanRemoved = z.amountInStack;
                    if(amountThatCanRemoved <= amountToRemove)
                    {
                        Destroy(z.gameObject);
                        amountToRemove -= amountThatCanRemoved;
                    }
                    else
                    {
                        z.amountInStack -= amountToRemove;
                        amountToRemove = 0;
                    }
                }
            }
        }
    }

    public void AddItem(Item itemToBeAdded, Item startingItem = null)
    {
        int amountInStack = itemToBeAdded.amountInStack;
        List<Item> stackableItems = new List<Item>();
        List<Slot> emptySlots = new List<Slot>();

        if (startingItem && startingItem.itemID == itemToBeAdded.itemID && startingItem.amountInStack < startingItem.maxStackSize)
            stackableItems.Add(startingItem);

        foreach (Slot i in slots)
        {
            if (i.slotsItem)
            {
                Item z = i.slotsItem;
                if (z.itemID == itemToBeAdded.itemID && z.amountInStack < z.maxStackSize && z != startingItem)
                    stackableItems.Add(z);
            }
            else
            {
                emptySlots.Add(i);
            }
        }

        foreach (Item i in stackableItems)
        {
            int amountThatCanbeAdded = i.maxStackSize - i.amountInStack;
            if(amountInStack <= amountThatCanbeAdded)
            {
                i.amountInStack += amountInStack;
                Destroy(itemToBeAdded.gameObject);
                return;
            }
            else
            {
                i.amountInStack = i.maxStackSize;
                amountInStack -= amountThatCanbeAdded;
            }
        }

        itemToBeAdded.amountInStack = amountInStack;
        if(emptySlots.Count > 0)
        {
            itemToBeAdded.transform.parent = emptySlots[0].transform;
            itemToBeAdded.gameObject.SetActive(false);
        }
    }
}
