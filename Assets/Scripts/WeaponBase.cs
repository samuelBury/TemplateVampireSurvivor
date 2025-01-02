using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionOfAttack
{
    None,
    Forward,
    LeftRight,
    UpDown
}
public abstract class WeaponBase : MonoBehaviour
{
    PlayerMove playerMove;

    public WeaponData weaponData;

    public WeaponStats weaponStats;

    
    float timer;

    Character wielder;

    public Vector2 vectorOfAttack;
    [SerializeField] DirectionOfAttack attackDirection;
    private void Awake()
    {
        playerMove = GetComponentInParent<PlayerMove>();
    }

    public void Update()
    {
        timer -= Time.deltaTime;
        if(timer< 0f)
        {
            Attack();
            timer = weaponStats.timeToAttack;
        }
    }
    public void ApplyDamage(Collider2D[] colliders)
    {
        int damage = GetDamage();
        for (int i = 0; i < colliders.Length; i++)
        {
            IDamageable e = colliders[i].GetComponent<IDamageable>();
            if (e != null)
            {
                PostDamage(damage, colliders[i].transform.position);
                e.TakeDamage(damage);
            }

        }
    }

    public virtual void SetData(WeaponData wd)
    {
        weaponData = wd;
        

        weaponStats = new WeaponStats(wd.stats.damage, wd.stats.timeToAttack, wd.stats.numberOfAttacks);
    }
    public abstract void Attack();

    public int GetDamage()
    {
        int damage = (int)(weaponData.stats.damage * wielder.damageBonus) ;
        return damage;
    }

    public virtual void PostDamage(int damage, Vector3 targetPosition)
    {
        MessageSystem.instance.PostMessage(damage.ToString(), targetPosition);
    }

    public void AddOwnerCharacter(Character character)
    {
        wielder = character;
    }

    public void Upgrade(UpgradeData upgradeData)
    {
        weaponStats.Sum(upgradeData.weaponUpgradeStats);
    }

    public void UpdateVectorOfAttack()
    {
        if(attackDirection == DirectionOfAttack.None)
        {
            vectorOfAttack = Vector2.zero;
            return;
        }

        switch (attackDirection)
        {
            case DirectionOfAttack.Forward:
                vectorOfAttack.x = playerMove.lastHorizontalCoupledVector;
                vectorOfAttack.y = playerMove.lastVerticalCoupledVector;
                break;
            case DirectionOfAttack.LeftRight:
                vectorOfAttack.x = playerMove.lastHorizontalDeCoupledVector;
                vectorOfAttack.y = 0f;
                break;
            case DirectionOfAttack.UpDown:
                vectorOfAttack.x = 0f;
                vectorOfAttack.y = playerMove.lastVerticalDeCoupledVector;
                break;
        }
        vectorOfAttack = vectorOfAttack.normalized;
    }
}
