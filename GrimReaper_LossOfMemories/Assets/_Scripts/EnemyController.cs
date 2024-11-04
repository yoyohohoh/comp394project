using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float damageAmount = 10f;
    public float attackRange = 5f;
    public float moveSpeed = 3f;

    [SerializeField] private GameObject player;
    private bool isPlayerInRange;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayerInRange();

        if (isPlayerInRange)
        {
            MoveTowardsPlayer();
        }
    }

    private void CheckForPlayerInRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        isPlayerInRange = distanceToPlayer <= attackRange;
    }

    private void MoveTowardsPlayer()
    {
        transform.LookAt(player.transform.position);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            player.GetComponent<PlayerController>().health -= 10f;
        }
    }

}
