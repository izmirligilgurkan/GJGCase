using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _GurkanTemplate.Scripts
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject floatingMessagePrefab;
        [SerializeField] private GameObject currencyPrefab;
        public static Canvas MainCanvas;
        public static Dictionary<Canvas, int> OtherCanvases = new Dictionary<Canvas, int>();
        private Camera _camera;
        
        private void Awake()
        {
            _camera = Camera.main;
        }
        
        private void FloatingMessage(string message, Vector3 worldPos, float moveDuration = 1f)
        {
            var messageInstance = Instantiate(floatingMessagePrefab, MainCanvas.transform);
            var rectTransform = messageInstance.GetComponent<RectTransform>();
            var textMeshPro = messageInstance.GetComponent<TextMeshProUGUI>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;
            rectTransform.anchoredPosition = WorldPositionToAnchoredPosition(worldPos);
            rectTransform.DOLocalMoveY(300f, moveDuration).SetRelative();
            textMeshPro.text = message;
            textMeshPro.DOFade(0f, moveDuration).OnComplete(() => Destroy(messageInstance));
        }
        private Vector2 WorldPositionToAnchoredPosition(Vector3 worldPos)
        {
            return _camera.WorldToScreenPoint(worldPos) / MainCanvas.scaleFactor;
        }
    }
}
