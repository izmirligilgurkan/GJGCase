using UnityEngine;
using UnityEngine.Serialization;

namespace _GurkanTemplate.Scripts
{
    public class StateDependent: MonoBehaviour
    {
        [SerializeField] public StateBehaviour tapStateBehaviour;
        [SerializeField] public StateBehaviour playStateBehaviour;
        [FormerlySerializedAs("winState")] [SerializeField] public StateBehaviour winStateBehaviour;
        [FormerlySerializedAs("loseState")] [SerializeField] public StateBehaviour loseStateBehaviour;
        
    }
}