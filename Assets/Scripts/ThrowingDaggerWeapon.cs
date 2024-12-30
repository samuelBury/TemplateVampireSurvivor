using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingDaggerWeapon : WeaponBase
{
    
    PlayerMove playerMove;

    [SerializeField] GameObject knifePrefab;

    private void Awake()
    {
        playerMove = GetComponentInParent<PlayerMove>();
    }
   

   

    public override void Attack()
    {

        GameObject thrownKnife = Instantiate(knifePrefab);
        thrownKnife.transform.position = transform.position;
        ThrowingDaggerProjectile throwingDaggerProjectile = thrownKnife.GetComponent<ThrowingDaggerProjectile>();
        throwingDaggerProjectile.SetDirection(playerMove.lastHorizontalVector, 0f);

        throwingDaggerProjectile.damage = weaponStats.damage;
    }
}
