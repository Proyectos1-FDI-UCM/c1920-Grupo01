﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyVision))]
[RequireComponent(typeof(TurretAttack))]
[RequireComponent(typeof(Rigidbody2D))]

public class Turret : MonoBehaviour
{
    [SerializeField] float speed = 1f, moveIni = 0f, changePos = 2.5f, cadence = 0.5f;
    [SerializeField] Transform player = null;
    [SerializeField] Transform[] children = null;
    int nextPos;
    float cadenceAux = 0;
    bool comeback = false;
    TurretAttack attack;
    EnemyVision vision;
    Enemy_Death death;

    void Awake()
    {
        vision = GetComponent<EnemyVision>();
        attack = GetComponent<TurretAttack>();
        death = GetComponent<Enemy_Death>();
    }

    private void Start()
    {
        transform.DetachChildren();
        nextPos = 1;
    }


    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, children[nextPos].position, speed * Time.deltaTime);

        if (player != null)
        {
            if (Vector2.Distance(transform.position, children[nextPos].position) < 0.2f)
            {
                if (moveIni <= 0)
                {
                    moveIni = changePos;
                    GetNextPos();
                    cadenceAux = cadence;
                }
                else
                {
                    death.enabled = false;
                    moveIni -= Time.deltaTime;
                  

                    if (vision.Spotted(player))
                    {
                        if (cadenceAux <= 0){
                            attack.TurAttack(player);
                            cadenceAux = cadence;
                        }
                        else{
                            cadenceAux -= Time.deltaTime;
                        }
                    }
                }
            }
            else
            {
                death.enabled = true;
                attack.enabled = false;
            }
        }
    }

    void GetNextPos()
    {
        if (comeback)
        {
            if (nextPos > 0){
                nextPos--;
            }
            else{
                comeback = false;
                nextPos = 1;
            }
        }
        else
        {
            if (nextPos < children.Length - 1){
                nextPos++;
            }
            else{
                comeback = true;
                nextPos = children.Length - 2;
            }
        }
    }
}