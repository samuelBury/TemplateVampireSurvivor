using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityTools 
{
   public static Vector2 GenerateRandomPositionSquarePattern( Vector2 spawnArea)
    {
        Vector2 position = new Vector2();

        float f = UnityEngine.Random.value > 0.5f ? -1f : 1f;
        if (UnityEngine.Random.value > 0.5f)
        {
            position.x = UnityEngine.Random.Range(-spawnArea.x, spawnArea.x);
            position.y = spawnArea.y * f;
        }
        else
        {
            position.y = UnityEngine.Random.Range(-spawnArea.y, spawnArea.y);
            position.x = spawnArea.x * f;
        }


        
        return position;
    }
}
