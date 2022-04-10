using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

namespace _GurkanTemplate.Scripts
{
    public class InputManager : MonoBehaviour
    {
        public static bool InputEnabled;
        public static readonly List<FingerControlled> FingerControllers = new List<FingerControlled>();
        public void FingerDown(LeanFinger leanFinger)
        {
            if(!InputEnabled) return;
            foreach (var fingerController in FingerControllers)
            {
                fingerController.OnFingerDown(leanFinger);
            }
        }

        public void FingerUpdate(LeanFinger leanFinger)
        {
            if(!InputEnabled) return;
            foreach (var fingerController in FingerControllers)
            {
                fingerController.OnFingerUpdate(leanFinger);
            }
        }

        public void FingerUp(LeanFinger leanFinger)
        {
            if(!InputEnabled) return;
            foreach (var fingerController in FingerControllers)
            {
                fingerController.OnFingerUp(leanFinger);
            }
        }
    }
}