using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemsController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Destroy(this.gameObject);
        }
    }
}
