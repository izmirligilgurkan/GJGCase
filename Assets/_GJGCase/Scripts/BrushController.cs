using System;
using _GurkanTemplate.Scripts;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;

public class BrushController : FingerControlled
{
    public bool shouldPaint;
    [SerializeField] private float moveSpeed = 1f;
    private Vector3 _childInitLocalPos;
    private Vector3 _initWorldPos;
    private bool _updateChildLocalPosition;

    private void Start()
    {
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
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(1).position += Vector3.left * 2f;
        transform.GetChild(1).DOLocalMove(_childInitLocalPos + Vector3.back * .1f, .3f).OnComplete(() => _updateChildLocalPosition = true);

    }

    private void OnGameStart()
    {
        transform.GetChild(0).DOLocalMove(_childInitLocalPos + Vector3.back * .1f, .3f).OnComplete(() => _updateChildLocalPosition = true);
    }



    public override void OnFingerDown(LeanFinger leanFinger)
    {
        shouldPaint = true;
    }

    public override void OnFingerUpdate(LeanFinger leanFinger)
    {
        Vector3 delta = leanFinger.ScaledDelta;
        delta = new Vector3(delta.x, 0, delta.y);
        var hit = RaycastForArm();
        var origin = hit?.point ?? transform.position;
        transform.position = origin + delta * moveSpeed;
        transform.forward = -hit?.normal ?? transform.forward;
    }

    public override void OnFingerUp(LeanFinger leanFinger)
    {
        shouldPaint = false;
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

    private void Update()
    {
        if (_updateChildLocalPosition)
        {
            transform.GetChild(0).localPosition =
                shouldPaint ? _childInitLocalPos : _childInitLocalPos + Vector3.back * .1f;
            transform.GetChild(1).localPosition =
                shouldPaint ? _childInitLocalPos : _childInitLocalPos + Vector3.back * .1f;
        }
        
    }
}
