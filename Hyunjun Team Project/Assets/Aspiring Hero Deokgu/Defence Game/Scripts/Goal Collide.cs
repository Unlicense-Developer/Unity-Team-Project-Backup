using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCollide : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if ( !DefenceGameManager.Instance.IsPlaying() )
            return;

        DefenceGameManager.Instance.playerLife.DecreaseLife();
        Destroy(other.gameObject);
    }
}
