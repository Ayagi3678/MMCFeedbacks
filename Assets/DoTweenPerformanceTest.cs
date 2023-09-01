using AnnulusGames.LucidTools.Inspector;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

public class DoTweenPerformanceTest : MonoBehaviour
{
    [SerializeField] private bool isLoop;
    [SerializeField] private int count;
    [SerializeField] private bool ignoreTimeScale;
    [SerializeField] private GameObject target;
    [SerializeField] private bool isRelative = true;
    [SerializeField] private bool isReturn;
    [Header("Position")]
    [SerializeField] private Vector3 zero;
    [SerializeField] private Vector3 one;
    [SerializeField] private float duration=1;
    
    private TweenCallback _onCompleteCacheWorld;
    private TweenCallback _onKillCache;
    private DOGetter<Vector3> _getterCache;
    private DOSetter<Vector3> _setterCache;
    private Vector3 _initialPosition;
    private Tween _tween;

    private void Start()
    {
        _onCompleteCacheWorld = () =>
        {
            Debug.Log("Complete");
            if (isReturn) target.transform.position = _initialPosition;
        };
        _onKillCache = () =>
        {
            if (isReturn) target.transform.position = _initialPosition;
        };
        _getterCache = () => target.transform.position;
        _setterCache = x => target.transform.position = x;
    }

    private void Update()
    {
        if (isLoop)
        {
            for (int i = 0; i < count; i++)
            {
                Play();
            }
        }
    }

    [Button]
    private void Play()
    {
        _tween?.Kill();
        _initialPosition = target.transform.position;
       _tween = DOTween.To(_getterCache,_setterCache,one,duration)            
            .SetAutoKill(false)
            .From(zero,true, isRelative)
            .SetRelative(isRelative)
            .SetUpdate(ignoreTimeScale)
            .OnKill(_onKillCache)
            .OnComplete(_onCompleteCacheWorld);
    }
}
