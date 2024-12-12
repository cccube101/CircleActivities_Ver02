using UnityEngine;
using System.Collections.Generic;


public class ManagerBase : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField] protected GameObject[] _controllers;


    // ---------------------------- Field
    protected readonly List<IMessenger> _messengers = new();



    // ---------------------------- UnityMessage
    public virtual void Awake()
    {
        //  コントローラーインターフェースのキャッシュ
        foreach (var item in _controllers)
        {
            _messengers.Add(item.GetComponent<IMessenger>());
        }

        //  アウェイク処理
        foreach (var message in _messengers)
        {
            if (message is IAwaken awaken)
            {
                awaken.Awake();
            }
        }
    }

    public virtual async void Start()
    {
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 20000, sequencesCapacity: 200);

        //  スタート処理
        foreach (var message in _messengers)
        {
            if (message is IStarter starter)    //  通常
            {
                starter.Start();
            }
            else if (message is IAwaitStarter awaitStarter) //  UniTask使用
            {
                await awaitStarter.Start();
            }
        }
    }

    public virtual void Update()
    {
        //  オブジェクトが存在する限り更新
        _messengers.RemoveAll(message =>
        {
            if (message is IUpdater updater)
            {
                return updater.Update();
            }
            else
            {
                return false;
            }
        });
    }
}
