using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class GameObjectExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetActiveFast(this GameObject obj, bool state)
        {
            if (obj.activeSelf == state)
            {
                return;
            }

            obj.SetActive(state);
        }
    }
}
