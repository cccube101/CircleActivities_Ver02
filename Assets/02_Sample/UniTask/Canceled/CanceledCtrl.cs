using Cysharp.Threading.Tasks;
using DG.Tweening;
using Helper;
using R3;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CanceledCtrl : MonoBehaviour, IMessenger, IAwaitStarter
{
    // ---------------------------- SerializeField
    [SerializeField] private Button _btn;
    [SerializeField] private GameObject _groupObj;
    [SerializeField] private float _duration;


    // ---------------------------- Field
    private const float _zero = 0.0f;
    private const float _one = 1.0f;

    private CanvasGroup _group = null;


    // ---------------------------- UnityMessage
    /// <summary>
    /// スタート
    /// </summary>
    /// <returns>スタート処理</returns>
    async UniTask IAwaitStarter.Start()
    {
        _group = _groupObj.GetComponent<CanvasGroup>();

        //  フェード
        //  呼び出し元となる始めの await には Canceled を使用
        await Tasks.Canceled(StartEvent01(destroyCancellationToken));
        //  上と全く同じ処理
        await Tasks.Canceled(StartEvent02(destroyCancellationToken));

        //  ボタン監視
        _btn.OnClickAsObservable().SubscribeAwait(async (_, ct) =>
        {
            //  下の
            //
            //}, AwaitOperation.Switch)
            //.RegisterTo(destroyCancellationToken);
            //
            //  の部分が キャンセル の代わりに指定した挙動をしてくれるので Canceled の必要はない
            await StartEvent02(ct);

        }, AwaitOperation.Switch)
        .RegisterTo(destroyCancellationToken);
    }




    // ---------------------------- PrivateMethod
    /// <summary>
    /// スタートイベント
    /// </summary>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>イベント処理</returns>
    private async UniTask StartEvent01(CancellationToken ct)
    {
        //  フェード
        //  一度呼び出し元で Canceled されていれば以降の処理で Canceled する必要はない
        await FadeGroup(_group, _zero, _duration, Ease.Linear, _groupObj, ct);
        await FadeGroup(_group, _one, _duration, Ease.Linear, _groupObj, ct);
    }

    /// <summary>
    /// グループフェード処理
    /// </summary>
    /// <param name="group">キャンバスグループ</param>
    /// <param name="end">α目標値</param>
    /// <param name="duration">処理時間</param>
    /// <param name="ease">イース</param>
    /// <param name="obj">オブジェクト</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>フェード処理タスク</returns>
    private async UniTask FadeGroup
    (CanvasGroup group
    , float end
    , float duration
    , Ease ease
    , GameObject obj
    , CancellationToken ct)
    {
        await group.DOFade(end, duration)
             .SetEase(ease)
             .SetLink(obj)
             .ToUniTask(Tasks.TCB, ct);
    }

    /// <summary>
    /// スタートイベント02
    /// </summary>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>イベント処理</returns>
    private async UniTask StartEvent02(CancellationToken ct)
    {
        await Fade(_zero);
        await Fade(_one);

        //  エラー無く処理をこなすには
        //  共通化する際引数が重なることが多く
        //  このように引数を大幅に減らした状態で表記できるので
        //  タスクを全て共通化するのは得策でもないことが多い
        async UniTask Fade(float endValue)
        {
            await _group.DOFade(endValue, _duration)
                .SetEase(Ease.Linear)
                .SetLink(_groupObj)
                .ToUniTask(Tasks.TCB, ct);
        }

        //  ではどんなときに共通化するべき？
        //  → ほとんどの引数が変わらないとき
    }
}
