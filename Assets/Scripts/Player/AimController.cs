﻿using UnityEngine;
using UnityEngine.Audio;

public class AimController : MonoBehaviour
{
    Animator[] anim;
    SpriteRenderer[] neon;
    int attackIndicator;
    AnimatorStateInfo animState;
    CircleCollider2D attackCollider;
    Sprite playerSpr;
    bool attackBool, blockBool;
    Bloqueo parry;

    private void Awake()
    {
        Cursor.visible = false;
        //anim[0] player //anim[1] sword
        anim = GetComponentsInChildren<Animator>();
        neon = GetComponentsInChildren<SpriteRenderer>();
        //neon[1].color = Color.red;
        attackCollider = GetComponent<CircleCollider2D>();
        parry = GetComponent<Bloqueo>();
    }

    void Update()
    {
        //Cogemos el estado de la animacion en cada frame
        animState = anim[0].GetCurrentAnimatorStateInfo(0);
        //Hacemos que el booleano se active solo cuando se activa la animación
        attackBool = animState.IsName("Player_Attack");
        blockBool = animState.IsName("Player_Block");
        playerSpr = GetComponent<SpriteRenderer>().sprite;
        //63 left, 15 down, 34 up, 70 right
        if (playerSpr.name == "Player_70" || playerSpr.name == "Player_63" || playerSpr.name == "Player_15" || playerSpr.name == "Player_34" || blockBool)
        {
            attackCollider.enabled = true;

        }
        else
        {
            attackCollider.enabled = false;
        }

        Vector2 mov;

        // Mientras no estén activas las animaciones de ataque o bloqueo, el 
        // jugador apunta en la dirección en la que se mueve
        if (!attackBool && !blockBool)
        {
            mov = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            // Si se mueve el joystick en el eje horizontal:
            if (Mathf.Abs(mov.x) >= Mathf.Abs(mov.y) && mov != Vector2.zero && !GameManager.instance.gameIsPaused)
            {
                anim[0].SetFloat("PosY", 0);
                anim[1].SetFloat("PosY", 0);
                if (mov.x >= 0) // Mirar Derecha
                {
                    anim[0].SetFloat("PosX", 1);
                    anim[1].SetFloat("PosX", 1);
                    attackIndicator = 1;
                }
                else // Mirar Izquierda
                {
                    anim[0].SetFloat("PosX", -1);
                    anim[1].SetFloat("PosX", -1);
                    attackIndicator = 2;
                }
            }

            else if (mov != Vector2.zero && !GameManager.instance.gameIsPaused)
            {
                anim[0].SetFloat("PosX", 0);
                anim[1].SetFloat("PosX", 0);
                if (mov.y >= 0) // Mirar Arriba
                {
                    anim[0].SetFloat("PosY", 1);
                    anim[1].SetFloat("PosY", 1);
                    attackIndicator = 3;

                }
                else // Mirar Abajo
                {
                    anim[0].SetFloat("PosY", -1);
                    anim[1].SetFloat("PosY", -1);
                    attackIndicator = 4;
                    //Debug.Log(indicadorAtaque);

                }
            }
        }

        if (Input.GetKeyDown("joystick button 5") && !attackBool && !blockBool && !GameManager.instance.gameIsPaused)
        {
            GetComponent<Sword_Attack>().enabled = true;
            Attack(attackIndicator, ref attackCollider, anim);
        }

        else if (Input.GetKeyDown("joystick button 4") && !attackBool && !blockBool && !GameManager.instance.gameIsPaused)
        {

            mov = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            // Si se mueve el joystick en el eje horizontal:
            if (Mathf.Abs(mov.x) >= Mathf.Abs(mov.y) && mov != Vector2.zero && !GameManager.instance.gameIsPaused)
            {
                anim[0].SetFloat("PosY", 0);
                anim[1].SetFloat("PosY", 0);
                if (mov.x >= 0) // Mirar Derecha
                {
                    anim[0].SetFloat("PosX", 1);
                    anim[1].SetFloat("PosX", 1);
                    attackIndicator = 1;
                }
                else // Mirar Izquierda
                {
                    anim[0].SetFloat("PosX", -1);
                    anim[1].SetFloat("PosX", -1);
                    attackIndicator = 2;
                }
            }

            else if (mov != Vector2.zero && !GameManager.instance.gameIsPaused)
            {
                anim[0].SetFloat("PosX", 0);
                anim[1].SetFloat("PosX", 0);
                if (mov.y >= 0) // Mirar Arriba
                {
                    anim[0].SetFloat("PosY", 1);
                    anim[1].SetFloat("PosX", 1);
                    attackIndicator = 3;

                }
                else // Mirar Abajo
                {
                    anim[0].SetFloat("PosY", -1);
                    anim[1].SetFloat("PosX", -1);
                    attackIndicator = 4;
                    //Debug.Log(indicadorAtaque);

                }
            }

            GetComponent<Sword_Attack>().enabled = false;
            anim[0].SetTrigger("Block");
            anim[1].SetTrigger("Block");
            switch (attackIndicator)
            {
                case 1:  //Block Right
                    attackCollider.offset = new Vector2(0.18f, 0);
                    attackCollider.radius = 0.35f;
                    break;
                case 2:   //Block Left
                    attackCollider.offset = new Vector2(-0.19f, 0);
                    attackCollider.radius = 0.33f;
                    break;
                case 3:   //Block Up
                    attackCollider.offset = new Vector2(0, 0.15f);
                    attackCollider.radius = 0.3f;
                    break;
                case 4:   //Block Down
                    attackCollider.offset = new Vector2(0.04f, -0.125f);
                    attackCollider.radius = 0.3f;
                    break;
            }
            parry.enabled = true;
        }
    }

    static void Attack(int attackIndicator, ref CircleCollider2D attackCollider, Animator[] anim)
    {
        //attackCollider.enabled = true;
        Vector2 mov = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // Si se mueve el joystick en el eje horizontal:
        if (Mathf.Abs(mov.x) >= Mathf.Abs(mov.y) && mov != Vector2.zero && !GameManager.instance.gameIsPaused)
        {
            anim[0].SetFloat("PosY", 0);
            anim[1].SetFloat("PosY", 0);
            if (mov.x >= 0) // Mirar Derecha
            {
                anim[0].SetFloat("PosX", 1);
                anim[1].SetFloat("PosY", 1);
                attackIndicator = 1;
            }
            else // Mirar Izquierda
            {
                anim[0].SetFloat("PosX", -1);
                anim[1].SetFloat("PosY", -1);
                attackIndicator = 2;
            }
        }

        else if (mov != Vector2.zero && !GameManager.instance.gameIsPaused)
        {
            anim[0].SetFloat("PosX", 0);
            anim[1].SetFloat("PosY", 0);
            if (mov.y >= 0) // Mirar Arriba
            {
                anim[0].SetFloat("PosY", 1);
                anim[1].SetFloat("PosY", 1);
                attackIndicator = 3;

            }
            else // Mirar Abajo
            {
                anim[0].SetFloat("PosY", -1);
                anim[1].SetFloat("PosY", -1);
                attackIndicator = 4;
                //Debug.Log(indicadorAtaque);

            }
        }

        anim[0].SetTrigger("Attack");
        anim[1].SetTrigger("Attack");
        AudioManager.instance.Play(AudioManager.ESounds.Swing); // Hace que suene el sonido asociado al ataque
        switch (attackIndicator)
        {
            case 1:   //Right Attack
                attackCollider.offset = new Vector2(0.3f, 0.12f);
                attackCollider.radius = 0.4f;
                break;
            case 2:   //Left Attack
                attackCollider.offset = new Vector2(-0.3f, 0.12f);
                attackCollider.radius = 0.4f;
                break;
            case 3:   //Up Attack
                attackCollider.offset = new Vector2(-0.07f, 0.22f);
                attackCollider.radius = 0.42f;
                break;
            case 4:   //Down Attack
                attackCollider.offset = new Vector2(0, -0.3f);
                attackCollider.radius = 0.42f;
                break;
        }
    }
}

