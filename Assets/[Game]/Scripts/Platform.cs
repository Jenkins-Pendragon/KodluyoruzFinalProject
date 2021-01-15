using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [HideInInspector]
    public List<Enemy> enemyList = new List<Enemy>();
    [HideInInspector]
    public List<IShootable> shootables = new List<IShootable>();
    [HideInInspector]
    public bool isPlatfromActive;
    public Transform moveTo;
    public Transform jumpTo;


    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();        
        IShootable shootable = other.GetComponent<IShootable>();

        if (enemy!=null && !enemyList.Contains(enemy))
        {
            if(enemy.NavMeshAgent !=null && enemy.canRun) enemy.NavMeshAgent.enabled = isPlatfromActive;
            enemyList.Add(enemy);
        }

        if (shootable != null && !shootables.Contains(shootable))
        {
            shootable.IsCanFire = isPlatfromActive;
            shootables.Add(shootable);
        }
    }


}
