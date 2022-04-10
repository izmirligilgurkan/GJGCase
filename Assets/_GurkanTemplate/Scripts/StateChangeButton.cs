using System;
using UnityEngine;
using UnityEngine.UI;

namespace _GurkanTemplate.Scripts
{
    [RequireComponent(typeof(Button))]
    public class StateChangeButton : MonoBehaviour
    {
        private Button _button;
        [SerializeField] private ToWhere type;
        private enum ToWhere
        {
            Play,
            Restart
        }
        private void OnEnable()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            switch (type)
            {
                case ToWhere.Play:
                    GameManager.GameStart();
                    break;
                case ToWhere.Restart:
                    GameManager.ContinueRestart();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}