using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon;


public class SawTrap : MonoBehaviour
{
    public PlayerStatus player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerStatus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.ReceiveDamage(10);
        }
    }

    void playerDamage(int damageAmount)
    {
       
    }
}
