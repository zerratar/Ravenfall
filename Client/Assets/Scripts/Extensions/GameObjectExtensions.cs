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
        public static int GetObjectId(this GameObject obj)
        {
            if (obj.name.IndexOf("::") > 0)
            {
                return int.Parse(obj.name.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries)[1]);
            }
            return -1;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetObjectIndex(this GameObject obj)
        {
            if (obj.name.IndexOf("::") > 0)
            {
                return int.Parse(obj.name.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries)[2]);
            }
            return -1;
        }

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
