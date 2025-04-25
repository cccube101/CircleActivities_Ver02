using Cysharp.Threading.Tasks;
using DG.Tweening;
using Helper;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UniTaskCancelCtrl : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField] TMP_Text _text;
    [SerializeField] private float _duration;
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _cancelBtn;

    // ---------------------------- Field
    CancellationTokenSource _cts = new();
    private GameObject Obj => gameObject;
    bool _canStart = false;


    // ---------------------------- UnityMessage
    private async UniTaskVoid Start()
    {
        // オブジェクトがデストロイされた際にキャンセルされるトークン

        // このスクリプトがコンポーネントとして付けられたオブジェクトを監視
        // デストロイの処理順が不透明になっているので複数のトークンを使用する際には注意
        // 例：OnDestroyより早くキャンセルの処理が実行された
        var dct = destroyCancellationToken;
        var objCt = _text.GetCancellationTokenOnDestroy(); // 任意のオブジェクトを監視

        ObserveBtn();   //  ボタン監視

        await WaitStart(dct);   //  ボタンが押されるまで待機



        // 一般的な例外処理方法
        Debug.Log("開始：Normal");
        try
        {
            await Count(_cts.Token);
            Debug.Log("終了：Normal");
        }
        catch (OperationCanceledException)
        {
            Debug.Log("キャンセル：Normal");
        }



        await WaitStart(dct);



        // try-catchを使わない方法
        // OperationCanceledException を bool型(UniTask<bool>)で返してくれます
        Debug.Log("開始：Suppress");
        if (await Count(_cts.Token).SuppressCancellationThrow())
        {
            Debug.Log("キャンセル：Suppress");
        }
        Debug.Log("終了：Suppress");



        await WaitStart(dct);



        Debug.Log("開始：Suppress");
        await Count(_cts.Token).SuppressCancellationThrow();
        Debug.Log("終了：Suppress");
    }

    private void OnDestroy()
    {
        Debug.Log("デストロイ");
        Tasks.Cancel(ref _cts);
    }





    // ---------------------------- PrivateMethod
    /// <summary>
    /// ボタン監視
    /// </summary>
    private void ObserveBtn()
    {
        // スタート
        _startBtn.onClick.AddListener(() =>
        {
            _canStart = true;
        });

        // キャンセル
        _cancelBtn.onClick.AddListener(() =>
        {
            _canStart = false;
            Tasks.Cancel(ref _cts);
            _cts = new();
        });
    }

    /// <summary>
    /// ボタンが押されるまで待機
    /// </summary>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>待機処理</returns>
    private async UniTask WaitStart(CancellationToken ct)
    {
        await UniTask.WaitUntil(() => _canStart, cancellationToken: ct).SuppressCancellationThrow();
        _canStart = false;
    }

    /// <summary>
    /// カウント
    /// </summary>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>カウント</returns>
    private async UniTask Count(CancellationToken ct)
    {
        // カウント
        await DOVirtual.Float(0, _duration, _duration,
            (value) =>
            {
                _text.text = $"{value:0.0}";
            })
            .SetEase(Ease.Linear)
            .SetLink(Obj)
            .ToUniTask(Tasks.TCB, ct);
    }
}
