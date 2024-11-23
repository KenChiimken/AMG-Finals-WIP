using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedKnife = Instantiate(weaponData.Prefab);
        spawnedKnife.transform.position = transform.position; //Assign the position to be the same as this object which is parented to the player
        spawnedKnife.GetComponent<PenBehaviour>().DirectionChecker(pm.lastMovedVector);   //Reference and set the direction
    }
}
