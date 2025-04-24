using Cysharp.Threading.Tasks;
using DG.Tweening;
using Helper;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UniTaskWhileCtrl : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField] GameObject _textObj;
    [SerializeField] private float _duration;
    [SerializeField] private Button _cancelBtn;

    // ---------------------------- Field
    CancellationTokenSource _cts = new();
    CancellationToken CT => _cts.Token;
    private TMP_Text Text => _textObj.GetComponent<TMP_Text>();



    // ---------------------------- UnityMessage
    private async UniTaskVoid Start()
    {
        ObserveBtn();   //  ボタン監視

        //  コルーチンのように繰り返し処理をする際に用いる await処理
        try
        {
            await WhileTask(CT);
        }
        catch
        {
            Debug.Log("キャンセルされました");
        }
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
        // キャンセル
        _cancelBtn.onClick.AddListener(() =>
        {
            Tasks.Cancel(ref _cts);
        });
    }

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
                .ToUniTask(Tasks.TCB, CT);

            await UniTask.Yield(cancellationToken: CT); //  yield return null と同じ作用
        }
    }
}
