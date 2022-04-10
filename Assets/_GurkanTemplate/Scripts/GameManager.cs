using System;
using UnityEngine;

namespace _GurkanTemplate.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static event Action GameStarted;
        public static event Action<bool> GameEnded;
        public static event Action TransitionToStarted;
        public static event Action ReadiedForTransition;
        public static event Action TransitionCompleted;

        public static void GameStart()
        {
            LevelHelper.GameStarted();
            InputManager.InputEnabled = true;
            GameStarted?.Invoke();
        }

        public static void GameEnd(bool win)
        {
            LevelHelper.GameEnded(win);
            InputManager.InputEnabled = false;
            GameEnded?.Invoke(win);
        }

        public static void ContinueRestart()
        {
            LevelHelper.LoadLevel();
        }

        public static void TransitionButtonPressed()
        {
            TransitionToStarted?.Invoke();
        }

        public static void ReadyForTransition()
        {
            ReadiedForTransition?.Invoke();
        }

        public static void TransitionComplete()
        {
            TransitionCompleted?.Invoke();
        }
    }
}

