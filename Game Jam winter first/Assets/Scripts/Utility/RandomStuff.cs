using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bogadanul.Assets.Scripts.Utility
{
    public static class RandomStuff
    {
        public static float RandomNumber(float number, float range)
        {
            return Random.Range(number - range, number + range);
        }
    }
}