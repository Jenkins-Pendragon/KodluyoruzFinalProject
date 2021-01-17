using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!PlayerData.Instance.IsPlayerDead)
        {
            IPlayerKillable playerKillable = collision.gameObject.GetComponent<IPlayerKillable>();
            if (playerKillable != null && playerKillable.IsDeadly && !PlayerData.Instance.IsImmune)
            {
                EventManager.OnLevelFailed.Invoke();
            }
        }
        
    }
}
