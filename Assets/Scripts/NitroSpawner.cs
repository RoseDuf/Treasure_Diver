using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroSpawner : MonoBehaviour
{
    //desired spawn delay
    [SerializeField]
    float spawnDelay;

    //nitro prefab
    [SerializeField]
    GameObject nitroCylinder;

    //Positions
    [SerializeField]
    float minY;
    [SerializeField]
    float maxY;
    [SerializeField]
    float minX;
    [SerializeField]
    float maxX;

    float nextTimeToSpawn = 0f;
    bool oneNitro; //1 nitro on screen at a time checker
    public static bool destroyNitro;

    private void Start()
    {
        oneNitro = false;
        destroyNitro = false;
        nextTimeToSpawn = Time.time + spawnDelay;
    }

    private void Update()
    {
        NitroTimer();
    }

    //Timer function
    void NitroTimer()
    {
        if (nextTimeToSpawn <= Time.time) //Time.time is the number of seconds elapsed since the start of the game
        {
            nextTimeToSpawn = Time.time + spawnDelay;

            // check if player doesn't already have a nitro and if there isn't already a nitro in the game
            if (!Player.nitroCylinderObtained && !oneNitro) 
            {
                SpawnNitro();
                oneNitro = true;
                return;
            }
            //Destroy nitro after spawnDelay seconds
            if (!Player.nitroCylinderObtained && oneNitro)
            {
                destroyNitro = true;
                oneNitro = false;
            }
        }
        // once the player activates the nitro, update the nexttimeToSpawn and say that there is currently no nitro in the game
        if (Player.nitroCylinderActivate) 
        {
            nextTimeToSpawn = Time.time + spawnDelay;
            oneNitro = false;
        }
    }

    void SpawnNitro()
    {
        float coordX = Random.Range(minX, maxX);
        float coordY = Random.Range(minY, maxY);

        Instantiate(nitroCylinder, new Vector2(coordX, coordY), Quaternion.identity);

    }
}
