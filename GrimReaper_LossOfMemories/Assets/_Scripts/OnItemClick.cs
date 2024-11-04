using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnItemClick : MonoBehaviour
{
    [SerializeField] private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void UsePickupItems()
    {
        player.GetComponent<PlayerController>().health += 10f;
        Destroy(gameObject);
    }
}
