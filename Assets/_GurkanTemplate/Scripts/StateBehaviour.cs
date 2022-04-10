using System;

namespace _GurkanTemplate.Scripts
{
    [Serializable]
    public class StateBehaviour
    {
        public bool enabledAtThisState = true;
        public float delayBeforeEnabled;
        public float delayBeforeDisabled;
    }
}