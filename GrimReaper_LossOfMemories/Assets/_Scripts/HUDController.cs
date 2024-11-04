using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public GameObject menu;

    public GameObject inventory;
    bool hasInventory;

    public Slider health;

    public GameObject item1;
    public GameObject item2;
    public GameObject item3;

    // Start is called before the first frame update
    void Start()
    {
        hasInventory = false;
        inventory.SetActive(false);
    }

    private void Update()
    {
        ManageHealth();
        ManagePickupItems();
    }

    void ManagePickupItems()
    {
        GameObject[] pickupItems = GameObject.FindGameObjectsWithTag("PickupItem");
        int pickupItemAmount = pickupItems.Length;

        switch(pickupItemAmount)
        {
            case 0:
                item1.SetActive(true);
                item2.SetActive(true);
                item3.SetActive(true);
                break;
            case 1:
                item1.SetActive(true);
                item2.SetActive(true);
                item3.SetActive(false);
                break;
            case 2:
                item1.SetActive(true);
                item2.SetActive(false);
                item3.SetActive(false);
                break;           
            case 3:
                item1.SetActive(false);
                item2.SetActive(false);
                item3.SetActive(false);
                break;

            default:
                Debug.Log($"Error! Pick up items: {pickupItemAmount}");
                break;
        }
    }

    void ManageHealth()
    {
        health.value = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().health;
        
        if(health.value <= 0f)
        {
            menu.GetComponent<MenuController>().LoseGame();
        }
    }
    public void ManageInventory()
    {
        if(hasInventory)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }

    }

    void OpenInventory()
    {
        hasInventory = true;
        inventory.SetActive(true);
    }

   void CloseInventory()
    {
        hasInventory = false;
        inventory.SetActive(false);
    }
}
