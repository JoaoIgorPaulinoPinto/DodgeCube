using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DifficultyModifier
{
    public float wallSpeed;
    public float wallSpawnerWaitTime;
    public float timeToAct;
}
public class DificultManager : MonoBehaviour
{
   public int time;

    public GameObject[] walls;

    public WallsController wallsController;

    public List<DifficultyModifier> DificultyUpdates;

    private void Update()
    {
        time += (int)Time.time;
        print(time);
        foreach (var item in DificultyUpdates)
        {
            if(time > item.timeToAct)
            {
                wallsController.timewait += item.wallSpawnerWaitTime;
                for (int i = 0; i < walls.Length; i++)
                {
                    walls[i].GetComponent<WallMovimentController>().wallspeed += item.wallSpeed;
                
                }
                print("modificado");
                DificultyUpdates.Remove(item);
            }
        }
    }
}
