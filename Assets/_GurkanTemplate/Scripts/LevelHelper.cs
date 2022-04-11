using UnityEngine;
using UnityEngine.SceneManagement;

namespace _GurkanTemplate.Scripts
{
    public static class LevelHelper
    {
        private static bool _playMode;

        public static int LevelNo => PlayerPrefs.HasKey("LevelNo")? PlayerPrefs.GetInt("LevelNo"): 1;
        private static bool _lastStateWin;

        public static void GameStarted()
        {
            _playMode = true;
        }
        
        public static void GameEnded(bool win)
        {
            if(!_playMode) return;
            _playMode = false;
            _lastStateWin = win;
        }


        public static void LoadLevel()
        {
            if(_playMode) return;
            if (_lastStateWin) PlayerPrefs.SetInt("LevelNo", LevelNo + 1);
            SceneManager.LoadScene(LevelNo % SceneManager.sceneCount);
        }
    }
}