
using UnityEngine;


namespace RTS
{
    public enum Team
    {
        Player,
        Enemy,
        Neutral
    }

    public static class TeamColors
    {
        public static readonly Color Player = Color.darkGreen;
        public static readonly Color Enemy = Color.softRed;
        public static readonly Color Neutral = Color.lightGoldenRod;
    }
}
