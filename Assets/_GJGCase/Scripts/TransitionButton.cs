using System.Collections.Generic;
using System.Linq;
using _GurkanTemplate.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _GJGCase.Scripts
{
    public class TransitionButton : MonoBehaviour
    {
        private Button _button;
        private List<Image> _images;
        private void OnEnable()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClicked);
            GameManager.ReadiedForTransition += OnReadiedForTransition;
            _images = GetComponentsInChildren<Image>().ToList();
            foreach (var image in _images)
            {
                image.enabled = false;
            }
        }
    
        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
            GameManager.ReadiedForTransition -= OnReadiedForTransition;
        }
        private void OnReadiedForTransition()
        {
            transform.DOScale(1.2f, .3f).SetLoops(2, LoopType.Yoyo).SetRelative();
            foreach (var image in _images)
            {
                image.enabled = true;
                var imageColor = image.color;
                imageColor.a = 0;
                image.color = imageColor;
                image.DOFade(1f, .5f);
            }
        }
        private void OnClicked()
        {
            GameManager.TransitionButtonPressed();
            foreach (var image in _images)
            {
                image.enabled = false;
            }
        }
    }
}
