using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCollide : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        DefenceGameManager.instance.playerLife.DecreaseLife();
        Destroy(other.gameObject);
    }
}
