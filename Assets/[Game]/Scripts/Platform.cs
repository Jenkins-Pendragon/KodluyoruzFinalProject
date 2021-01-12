using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [HideInInspector]
    public List<Enemy> enemyList = new List<Enemy>();
    public Transform pointA;
    public Transform pointB;


    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();


        if(enemy!=null && !enemyList.Contains(enemy))
        {
            enemyList.Add(enemy);
        }


    }


}
