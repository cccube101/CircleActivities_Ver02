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
    private enum CancelType
    {
        Normal,
        InMethod,
        Destroy,
    }
    // ---------------------------- SerializeField
    [SerializeField] GameObject _textObj;
    [SerializeField] private float _duration;
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _cancelBtn;


    // ---------------------------- Field
    CancellationTokenSource _cts = new();
    private TMP_Text Text => _textObj.GetComponent<TMP_Text>();
    bool _canStart = false;

    // ---------------------------- UnityMessage
    // Awakeでも await したいものがあれば以下のようにUniTaskVoidを使う
    //private async UniTaskVoid Awake()
    //{
    //    await UniTask.Yield(); // 任意のタスク
    //}

    private async UniTaskVoid Start()
    {
        // ------ キャンセル処理方法の例 ------


        //トークンの変数化
        // 任意のタイミングでキャンセルするトークン
        var ct = _cts.Token;
        // オブジェクトがデストロイされた際にキャンセルされるトークン
        // 任意のタイミングでキャンセルしない場合有効
        var dct = destroyCancellationToken;
        var objCt = _textObj.GetCancellationTokenOnDestroy();

        ObserveBtn();   //  ボタン監視



        await WaitStart(dct);   //  ボタンが押されるまで待機



        // 一般的な例外処理方法
        // キャンセル処理を任意のタイミングで行う場合に使用
        try
        {
            await NormalCount(ct);
            Debug.Log("一般的なタスクが終了しました");
        }
        catch
        {
            Debug.Log("一般的なタスクがキャンセルされました");
        }



        await WaitStart(dct);   //  ボタンが押されるまで待機



        // 呼び出し先の関数内でキャンセルする場合
        // 例外処理の呼び出し位置を調整することができる
        await InMethodCount(ct);



        await WaitStart(dct);   //  ボタンが押されるまで待機



        //  try-catchを使わない方法
        // 任意のタイミングでキャンセルしない場合有効
        if (await DestroyCount(objCt).SuppressCancellationThrow())
        {
            Debug.Log("デストロイ依存のタスクがキャンセルされました");
            return;
        }
        // 同上
        // await Tasks.Canceled(DestroyCount(objCt));

    }

    private void OnDestroy()
    {
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
        try
        {
            await UniTask.WaitUntil(() => _canStart, cancellationToken: ct);
        }
        catch
        {
        }
        _canStart = false;
    }

    /// <summary>
    /// カウント
    /// </summary>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>カウント終了 または キャンセル</returns>
    private async UniTask NormalCount(CancellationToken ct)
    {
        await Count(CancelType.Normal, ct);
    }

    /// <summary>
    /// カウント
    /// </summary>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>カウント終了 または キャンセル</returns>
    private async UniTask InMethodCount(CancellationToken ct)
    {
        try
        {
            await Count(CancelType.InMethod, ct);
            Debug.Log("関数内でキャンセルされるタスクが終了しました");
        }
        catch
        {
            Debug.Log("関数内でキャンセルされるタスクがキャンセルされました");
        }
    }

    /// <summary>
    /// カウント
    /// </summary>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>カウント終了 または キャンセル</returns>
    private async UniTask DestroyCount(CancellationToken ct)
    {
        await Count(CancelType.Destroy, ct);
    }

    /// <summary>
    /// カウント
    /// </summary>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>カウント</returns>
    private async UniTask Count(CancelType type, CancellationToken ct)
    {
        // 種類
        var text = type switch
        {
            CancelType.Normal => "Normal",
            CancelType.InMethod => "InMethod",
            CancelType.Destroy => "Destroy",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        // カウント
        await DOVirtual.Float(0, _duration, _duration,
            (value) =>
            {
                Text.text = $"{text}:{value:0.0}";
            })
            .SetEase(Ease.Linear)
            .SetLink(_textObj)
            .ToUniTask(Tasks.TCB, ct);
    }
}
