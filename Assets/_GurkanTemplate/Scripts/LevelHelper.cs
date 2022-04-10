using UnityEngine.SceneManagement;

namespace _GurkanTemplate.Scripts
{
    public static class LevelHelper
    {
        private static bool _playMode;
        private static int _levelNo;

        public static int LevelNo => _levelNo + 1;

        public static void GameStarted()
        {
            _playMode = true;
        }
        
        public static void GameEnded(bool win)
        {
            if(!_playMode) return;
            _playMode = false;
            if (win) _levelNo++;
        }


        public static void LoadLevel()
        {
            if(_playMode) return;
            SceneManager.LoadScene(_levelNo);
        }
    }
}