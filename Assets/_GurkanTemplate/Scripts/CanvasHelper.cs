using System;
using UnityEngine;

namespace _GurkanTemplate.Scripts
{
    public class CanvasHelper: MonoBehaviour
    {
        [SerializeField] private bool mainCanvas;
        [SerializeField] private int canvasIndexKey;
        
        private bool _mainCanvas;
        private int _canvasIndex;
        
        private Canvas _canvas;
        private void OnEnable()
        {
            if (!TryGetComponent(out Canvas component))
            {
                Debug.Log("No canvas on CanvasHelper object!");
                return;
            }
            _canvas = component;

            _mainCanvas = mainCanvas;
            _canvasIndex = canvasIndexKey;
            
            if (mainCanvas)
            {
                UIManager.MainCanvas = _canvas;
            }
            else
            {
                _canvas.Store(_canvasIndex);
            }
        }

        private void OnDisable()
        {
            if(!_canvas) return;
            
            if (_mainCanvas)
            {
                UIManager.MainCanvas = null;
            }
            else
            {
                _canvas.Destore(_canvasIndex);
            }
        }
    }
}