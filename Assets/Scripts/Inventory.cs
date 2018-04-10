using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    GameObject inventoryPanel;
    GameObject slotPanel;
    ItemDatabase database;

    public GameObject panel;
    public GameObject buttonMenu;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    int slotAmount;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    void Start() {

        database = GetComponent<ItemDatabase>();
        slotAmount = 20;

    }
 
    public void AddItem(int id) {
        Item itemToAdd = database.FetchItemByID(id);

        if (itemToAdd.Stackable && CheckIfItemIsInInventory(itemToAdd)){
            for(int i = 0; i < items.Count; i++) {
                if (items[i].ID == id) {
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    break;
                }
            }
        }
        else {
            for (int i = 0; i < items.Count; i++) {
                if (items[i].ID == -1) {
                    items[i] = itemToAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().item = itemToAdd;
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                    itemObj.name = itemToAdd.Title;
                    itemObj.transform.localPosition = new Vector3(0, 0, 0);
                    itemObj.transform.localScale = new Vector3(1, 1, 1);
                    break;
                }
            }
        }
            
    }

    public void DeployInventory() {
        panel.SetActive(true);
        buttonMenu.SetActive(false);
        inventoryPanel = GameObject.Find("Inventory Panel");
        slotPanel = GameObject.Find("Slot Panel");
        for (int i = 0; i < slotAmount; i++) {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform, false);
        }
        AddItem(0);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);

    }

    bool CheckIfItemIsInInventory(Item item) {
        for (int i = 0; i < items.Count; i++){
            if (items[i].ID == item.ID) {
                return true;
            }
        }
        return false;
    }
}
