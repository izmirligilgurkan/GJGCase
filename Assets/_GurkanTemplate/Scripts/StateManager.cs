using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _GurkanTemplate.Scripts
{
    public class StateManager : MonoBehaviour
    {
        private List<StateDependent> _stateDependents = new List<StateDependent>();

        private void OnEnable()
        {
            GameManager.GameStarted += OnGameStart;
            GameManager.GameEnded += OnGameEnd;
        }
        
        private void OnDisable()
        {
            GameManager.GameStarted -= OnGameStart;
            GameManager.GameEnded -= OnGameEnd;
        }


        private void Start()
        {
            _stateDependents = UIManager.MainCanvas.GetComponentsInChildren<StateDependent>(true).ToList();
            ChangeStates(0);
        }
        private void OnGameStart()
        {
            ChangeStates(1);
        }
        private void OnGameEnd(bool obj)
        {
            ChangeStates(2, obj);
        }

        

        private void ChangeStates(int index, bool win = true)
        {
            if(_stateDependents.Count < 1) return;
            foreach (var dependent in _stateDependents)
            {
                var state = index switch
                {
                    0 => dependent.tapStateBehaviour,
                    1 => dependent.playStateBehaviour,
                    2 => win? dependent.winStateBehaviour: dependent.loseStateBehaviour,
                    _ => new StateBehaviour()
                };
                StartCoroutine(StateChanger(state, dependent.gameObject));
            }
        }
        
        IEnumerator StateChanger(StateBehaviour stateBehaviour, GameObject go)
        {
            var duration = stateBehaviour.enabledAtThisState ? stateBehaviour.delayBeforeEnabled : stateBehaviour.delayBeforeDisabled;
            if (duration >= 0f) yield return new WaitForSeconds(duration);
            go.SetActive(stateBehaviour.enabledAtThisState);
        }
    }
}