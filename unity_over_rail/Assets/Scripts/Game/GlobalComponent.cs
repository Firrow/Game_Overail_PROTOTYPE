using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace overail.GlobalComponent_
{
    public static class GlobalComponent
    {
        public static Dictionary<string, string> OPPOSITE_DIRECTIONS = new Dictionary<string, string>() {
            { "N", "S" },
            { "S", "N" },
            { "E", "O" },
            { "O", "E" }
        }; //CONSTANTE GLOBALE AU JEU ENTIER
        public enum DirectionChoice
        {
            NO_DIRECTION,
            LEFT,
            RIGHT,
            RANDOM
        } //CONSTANTE GLOBALE AU JEU ENTIER



        public static int modulo(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}
