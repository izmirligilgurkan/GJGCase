using System;
using _GurkanTemplate.Scripts;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;

namespace _GJGCase.Scripts
{
    public class BrushController : FingerControlled
    {
        public bool fingerDown;
        [SerializeField] private float moveSpeed = 1f;
        
        private Vector3 _childInitLocalPos;
        private Vector3 _initWorldPos;
        public bool peelMode;
        private bool _updateChildLocalPosition;
        private Camera _camera;
        public static event Action<Vector3> PeelOffFingerDown;
        public static event Action PeelOffFingerUp;
        public static event Action<LeanFinger> PeelOffFingerUpdate;

        private void Start()
        {
            _camera = Camera.main;
            _childInitLocalPos = transform.GetChild(0).localPosition;
            _initWorldPos = transform.position;
            transform.GetChild(0).position += Vector3.left * 2f;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.GameStarted += OnGameStart;
            GameManager.TransitionToStarted += OnTransitionToStarted;
            GameManager.TransitionCompleted += OnTransitionCompleted;
            GameManager.GameEnded += OnGameEnd;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameManager.GameStarted -= OnGameStart;
            GameManager.TransitionToStarted -= OnTransitionToStarted;
            GameManager.TransitionCompleted -= OnTransitionCompleted;
            GameManager.GameEnded -= OnGameEnd;
        }

        private void OnGameEnd(bool obj)
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }

        private void OnTransitionToStarted()
        {
            _updateChildLocalPosition = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }

        private void OnTransitionCompleted()
        {
            transform.position = _initWorldPos;
            peelMode = true;
        }

        private void OnGameStart()
        {
            transform.GetChild(0).DOLocalMove(_childInitLocalPos + Vector3.back * .1f, .3f).OnComplete(() => _updateChildLocalPosition = true);
        }



        public override void OnFingerDown(LeanFinger leanFinger)
        {
            fingerDown = true;
            var hit = peelMode? RaycastForArmFromScreen(leanFinger.ScreenPosition): RaycastForArm();
            if (peelMode && hit.HasValue)
            {
                transform.position = hit.Value.point;
                PeelOffFingerDown?.Invoke(hit.Value.point);
            }
            
        }

        public override void OnFingerUpdate(LeanFinger leanFinger)
        {
            Vector3 delta = leanFinger.ScaledDelta;
            delta = _camera.transform.TransformDirection(delta);
            var hit = RaycastForArm();
            var origin = hit?.point ?? transform.position;
            if (peelMode)
            {
                transform.position += delta * Time.deltaTime;
                PeelOffFingerUpdate?.Invoke(leanFinger);
            }
            else
            {
                transform.position = origin + delta * moveSpeed;
            }
            transform.forward = -hit?.normal ?? transform.forward;
        }

        public override void OnFingerUp(LeanFinger leanFinger)
        {
            fingerDown = false;
            if(peelMode) PeelOffFingerUp?.Invoke();
        }

        RaycastHit? RaycastForArm()
        {
            int layerMask = 1 << 6;
            if (Physics.Raycast(transform.position - transform.forward * 1f, transform.forward, out var hit, Mathf.Infinity, layerMask))
            {
                return hit;
            }
            return null;
        }
        RaycastHit? RaycastForArmFromScreen(Vector2 screenPos)
        {
            int layerMask = 1 << 6;
            if (Physics.Raycast(_camera.ScreenPointToRay(screenPos), out var hit, Mathf.Infinity, layerMask))
            {
                return hit;
            }
            return null;
        }

        private void Update()
        {
            if (_updateChildLocalPosition)
            {
                transform.GetChild(0).localPosition =
                    fingerDown ? _childInitLocalPos : _childInitLocalPos + Vector3.back * .1f;
                transform.GetChild(1).localPosition =
                    fingerDown ? _childInitLocalPos : _childInitLocalPos + Vector3.back * .1f;
            }
        
        }
    }
}
