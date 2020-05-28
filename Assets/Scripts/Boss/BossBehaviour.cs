﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ***FASE DE SPAWN***
// *FUNCIONAMIENTO LITERAL*
// Tomando los enemigos previamente colocados en el editor, en función de la oleada correspondiente
// van siendo teletransportados al lado del Boss. 
// Una vez eliminados, se inicia la siguiente oleada.
// Se repite este comportamiento hasta acabar con todas oleadas.
//-----------------------------------------------------------------
// *INFORMACIÓN DE EDITOR*
// En el editor se introduce el número de oleadas (array de structs Wave[])  
// Dentro de cada oleada, el número de enemigos que invocará (array de structs Enemy[])
// Por último, se rellenan los datos del enemigo (struct Enemy) con la referencia y la posición relativa al jugador
//-----------------------------------------------------------------
// *IMPORTANTE*
// Los enemigos requieren del script SPAWNCONTROL para ser identificados como enemigos de la oleada y ESTAR COLOCADOS EN LA ESCENA

public class BossBehaviour : MonoBehaviour
{
    TransformList enemiesOnScreen = new TransformList();
    Animator[] bossAnimator;
    
    private int actualWave;
   
    [System.Serializable]
    private struct Wave{
        public Enemy[] enemyRound; // Enemigos en una oleada
    }

    [System.Serializable]   
    private struct Enemy{
        public Vector2 relativePos; // Posición respecto al Boss
        public GameObject enemyRef; // Referencia de los enemigos en escena
    }

    [SerializeField] 
    Wave[] waves = null; // Array de oleadas de Spawn

    [SerializeField]
    GameObject[] crystals = null;

    private void Start()
    {
        actualWave = 0;
        bossAnimator = GetComponentsInChildren<Animator>();
        Instance();
        //

    }

    private void Instance() //Invocación de los enemigos (tp de transform)
    {
        bossAnimator[1].SetTrigger("AttakBoss");
        AudioManager.instance.Play(AudioManager.ESounds.Bastonazo);
        if (waves != null) 
            for (int i = 0; i < waves[actualWave].enemyRound.Length; i++){
                waves[actualWave].enemyRound[i].enemyRef.transform.position = GetRelativePos(i);
                enemiesOnScreen.InsertInEnd(waves[actualWave].enemyRound[i].enemyRef.transform);
            }
    }



    private void UpdateWave() //LLamado por SpawnControl para hacer la nueva invocación si no quedan enemigos vivos
    {
        if (actualWave < waves.Length && enemiesOnScreen.Lenght() == 0){
            crystals[actualWave].GetComponent<BossCrystal>().SetActive();
        }

    }

    public void UpdateEnemies(Transform e)
    {

        enemiesOnScreen.DeleteElement(e);
        Debug.LogWarning(enemiesOnScreen.Lenght());
        UpdateWave();

    }

    public void UpdateCrystal()
    {
        if (actualWave < waves.Length - 1)
        {
            actualWave++;
            Instance();
        }
        else
            Destroy(this.gameObject);

    }

    private Vector2 GetRelativePos(int i) //Método auxiliar para calcular la posición con respecto al Boss de los enemigos
    {
        Vector2 newRelativePos;
        newRelativePos.x = waves[actualWave].enemyRound[i].relativePos.x + this.transform.position.x;
        newRelativePos.y = waves[actualWave].enemyRound[i].relativePos.y + this.transform.position.y;
        return newRelativePos;
    }

}
