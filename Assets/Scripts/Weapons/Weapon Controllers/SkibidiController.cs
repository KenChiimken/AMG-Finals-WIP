using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkibidiController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedSkibidi = Instantiate(weaponData.Prefab);
        spawnedSkibidi.transform.position = transform.position; //Assign the position to be the same as this object which is parented to the player
        spawnedSkibidi.transform.parent = transform;
    }
}
