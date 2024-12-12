using Cysharp.Threading.Tasks;
using DG.Tweening;
using Helper;
using System.Threading;
using UnityEngine;

public class YieldCtrl : MonoBehaviour, IMessenger, IAwaitStarter
{
    // ---------------------------- SerializeField

    [SerializeField] private GameObject _groupObj;
    [SerializeField] private float _duration;

    // ---------------------------- Field
    private const float _zero = 0.0f;
    private const float _one = 1.0f;

    private CanvasGroup _group = null;


    // ---------------------------- UnityMessage

    async UniTask IAwaitStarter.Start()
    {
        //  キャッシュ
        _group = _groupObj.GetComponent<CanvasGroup>();

        await Tasks.Canceled(StartEvent(destroyCancellationToken));
    }

    // ---------------------------- PrivateMethod

    /// <summary>
    /// スタートイベント
    /// </summary>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>イベント処理</returns>
    private async UniTask StartEvent(CancellationToken ct)
    {
        //  繰り返し処理
        while (true)
        {
            await Fade(_zero, ct);
            await Fade(_one, ct);

            //  コルーチンの yield return null;と同じ
            await UniTask.Yield(cancellationToken: ct);
        }
    }

    /// <summary>
    /// フェード
    /// </summary>
    /// <param name="endValue">目標値</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>フェード処理</returns>
    private async UniTask Fade(float endValue, CancellationToken ct)
    {
        //  フェード処理
        await _group.DOFade(endValue, _duration)
            .SetEase(Ease.Linear)
            .SetLink(_groupObj)
            .ToUniTask(Tasks.TCB, ct);
    }

}
