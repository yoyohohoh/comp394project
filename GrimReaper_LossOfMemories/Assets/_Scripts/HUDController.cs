using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public GameObject inventory;
    bool hasInventory;
    // Start is called before the first frame update
    void Start()
    {
        hasInventory = false;
        inventory.SetActive(false);
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
