using Lean.Touch;
using UnityEngine;

namespace _GurkanTemplate.Scripts
{
    public abstract class FingerControlled: MonoBehaviour 
    {
        public abstract void OnFingerDown(LeanFinger leanFinger);
        public abstract void OnFingerUpdate(LeanFinger leanFinger);
        public abstract void OnFingerUp(LeanFinger leanFinger);

        protected virtual void OnEnable()
        {
            InputManager.FingerControllers.Add(this);
        }

        protected virtual void OnDisable()
        {
            InputManager.FingerControllers.Remove(this);
        }
    }
}