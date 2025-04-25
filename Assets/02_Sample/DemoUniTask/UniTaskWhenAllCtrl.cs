using Cysharp.Threading.Tasks;
using DG.Tweening;
using Helper;
using R3;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UniTaskWhenAllCtrl : MonoBehaviour
{
    private struct TextItem
    {
        public TextItem(GameObject obj) => Obj = obj;
        public GameObject Obj;
        public readonly TMP_Text Text => Obj.GetComponent<TMP_Text>();
    }

    // ---------------------------- SerializeField
    [SerializeField] GameObject _textObj01;
    [SerializeField] GameObject _textObj02;
    [SerializeField] GameObject _textObj03;
    [SerializeField] private float _duration;
    [SerializeField] private Button _retryBtn;

    // ---------------------------- Field
    private TextItem Text01 => new(_textObj01);
    private TextItem Text02 => new(_textObj02);
    private TextItem Text03 => new(_textObj03);



    // ---------------------------- UnityMessage
    private async UniTaskVoid Start()
    {
        // 同時処理タスク
        await WhenAllTask(destroyCancellationToken).SuppressCancellationThrow();

        ObserveBtn();   //  ボタン監視
    }

    // ---------------------------- PrivateMethod
    /// <summary>
    /// ボタン監視
    /// </summary>
    private void ObserveBtn()
    {
        // R3を使用したボタン監視

        // 監視対象のボタン
        _retryBtn
            // クリックの監視
            .OnClickAsObservable()
            // クリックされた際に通知を出力
            // _ の部分に受け取る値（今回は空なので _）
            // ct の部分に下記で指定したキャンセルトークンを設定
            .SubscribeAwait(async (_, ct) =>
        {
            await WhenAllTask(ct); // 任意の処理

            // キャンセルされた際の実行挙動を指定
            // Switchなら実行中のタスクをキャンセルして新しいタスクを優先する
        }, AwaitOperation.Switch)
        // トークンを指定
        .RegisterTo(destroyCancellationToken);
    }

    /// <summary>
    /// 同時処理タスク
    /// </summary>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>タスク処理</returns>
    private async UniTask WhenAllTask(CancellationToken ct)
    {
        // 初期化とタスク開始を同時に行う
        await UniTask.WhenAll(
            Count(Text01, 1.0f, ct),
            Count(Text02, 2.0f, ct),
            Count(Text03, 3.0f, ct));

        // タスク用のリストを用意し追加してから行う
        var textArray = new[]
        {
            Text01,Text02,Text03
        };
        var tasks = new List<UniTask>();
        int count = 5;
        foreach (var item in textArray)
        {
            tasks.Add(Count(item, count, ct));
            count++;
        }
        await UniTask.WhenAll(tasks);
    }

    /// <summary>
    /// カウント処理
    /// </summary>
    /// <param name="item">テキストアイテム</param>
    /// <param name="volume">カウント数</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>タスク処理</returns>
    private async UniTask Count(TextItem item, float volume, CancellationToken ct)
    {
        // カウント
        await DOVirtual.Float(0, volume, _duration,
            (value) =>
            {
                item.Text.text = $"{value:0.0}";
            })
            .SetEase(Ease.Linear)
            .SetLink(item.Obj)
            .ToUniTask(Tasks.TCB, ct);
    }
}
