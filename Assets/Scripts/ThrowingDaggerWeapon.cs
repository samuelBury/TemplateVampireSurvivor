using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingDaggerWeapon : MonoBehaviour
{
    [SerializeField] float timeToAttack;
    float timer;
    PlayerMove playerMove;

    [SerializeField] GameObject knifePrefab;

    private void Awake()
    {
        playerMove = GetComponentInParent<PlayerMove>();
    }
    private void Update()
    {
        if( timer< timeToAttack)
        {
            timer += Time.deltaTime;
            return;
        }
        timer = 0;
        SpawnKnife();
    }

    private void SpawnKnife()
    {
        GameObject thrownKnife = Instantiate(knifePrefab);
        thrownKnife.transform.position = transform.position;
        thrownKnife.GetComponent<ThrowingDaggerProjectile>().SetDirection(playerMove.lastHorizontalVector, 0f);
    }
}
