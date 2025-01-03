using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnifeWeapon : WeaponBase
{

    [SerializeField] GameObject knifePrefab;
    [SerializeField] float spread = 0.5f;

 
    public override void Attack()
    {

        UpdateVectorOfAttack();

        for(int i = 0; i< weaponStats.numberOfAttacks; i++)
        {
            GameObject thrownKnife = Instantiate(knifePrefab);

            Vector3 newKnifePosition = transform.position;

            if(weaponStats.numberOfAttacks > i)
            {
                newKnifePosition.y -= (spread * (weaponStats.numberOfAttacks-1)) / 2; // calculating offset
                newKnifePosition.y += i * spread;
            }
            

            thrownKnife.transform.position = newKnifePosition;

            ThrowingKnifeProjectile throwingDaggerProjectile = thrownKnife.GetComponent<ThrowingKnifeProjectile>();
            throwingDaggerProjectile.SetDirection(vectorOfAttack.x, vectorOfAttack.y);

            throwingDaggerProjectile.damage = GetDamage();
        }

        
    }
}
