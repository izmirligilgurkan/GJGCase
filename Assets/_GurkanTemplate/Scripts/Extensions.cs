using System;
using UnityEngine;

namespace _GurkanTemplate.Scripts
{
    public static class Extensions
    {
        public static void Store<T1, T2>(this T1 data, T2 key)
            where T1 : Component
        {
            ReferenceHelper<T2, T1>.RefDictionary.Add(key, data);
        }
        public static void Destore<T1, T2>(this T1 _, T2 key)
            where T1 : Component
        {
            ReferenceHelper<T2, T1>.RefDictionary.Remove(key);
        }
    }
}