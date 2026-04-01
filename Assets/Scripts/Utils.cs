using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightAdventure.Utils
{
    public static class Utils
    {
        //скрипт для получения рандомного направления  

        public static Vector3 GetRandomDir() 
        {
            return new Vector3(Random.Range(-1f, 1f), /* это x*/ Random.Range(-1f, 1f) /* это y*/).normalized; //а z тут нет
        }

        public static float GetRandomDistance(float distanceMin, float distanceMax)
        {
            return Random.Range(distanceMax, distanceMin); 
        }
    }
}