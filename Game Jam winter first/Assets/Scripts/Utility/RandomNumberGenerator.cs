using UnityEngine;

namespace Bogadanul.Assets.Scripts.Utility
{
    public class RandomNumberGenerator
    {
        public RandomNumberGenerator (int _seed)
        {
            Seed = _seed;
            Random.InitState (Seed);
        }

        public RandomNumberGenerator ()
        {
            Seed = Random.Range (0, 10000);
            Random.InitState (Seed);
        }

        public int Seed { get; set; }
    }
}