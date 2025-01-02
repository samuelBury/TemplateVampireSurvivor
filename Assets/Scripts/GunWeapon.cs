using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWeapon : WeaponBase
{
    [SerializeField] GameObject bulletPrefabs;
    public override void Attack()
    {
        UpdateVectorOfAttack();

        for (int i = 0; i < weaponStats.numberOfAttacks; i++)
        {
            GameObject thrownKnife = Instantiate(bulletPrefabs);

            Vector3 newKnifePosition = transform.position;


            thrownKnife.transform.position = newKnifePosition;

            ThrowingKnifeProjectile throwingDaggerProjectile = thrownKnife.GetComponent<ThrowingKnifeProjectile>();
            throwingDaggerProjectile.SetDirection(vectorOfAttack.x, vectorOfAttack.y);

            throwingDaggerProjectile.damage = GetDamage();
        }

    }
}
