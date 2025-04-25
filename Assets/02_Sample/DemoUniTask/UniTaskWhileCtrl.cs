using Cysharp.Threading.Tasks;
using DG.Tweening;
using Helper;
using System.Threading;
using TMPro;
using UnityEngine;

public class UniTaskWhileCtrl : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField] GameObject _textObj;
    [SerializeField] private float _duration;

    // ---------------------------- Field
    private TMP_Text Text => _textObj.GetComponent<TMP_Text>();



    // ---------------------------- UnityMessage
    private async UniTaskVoid Start()
    {
        //  コルーチンのように繰り返し処理をする際に用いる await処理
        await WhileTask(destroyCancellationToken).SuppressCancellationThrow();
    }

    // ---------------------------- PrivateMethod
    /// <summary>
    /// 繰り返し処理
    /// </summary>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>繰り返し処理</returns>
    private async UniTask WhileTask(CancellationToken ct)
    {
        while (true)
        {
            // カウント
            await DOVirtual.Float(0, _duration, _duration,
                (value) =>
                {
                    Text.text = $"{value:0.0}";
                })
                .SetEase(Ease.Linear)
                .SetLink(_textObj)
                .ToUniTask(Tasks.TCB, ct);

            await UniTask.Yield(cancellationToken: ct); //  yield return null と同じ作用
        }
    }
}
