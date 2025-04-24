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
    [SerializeField] private Button _cancelBtn;
    [SerializeField] private Button _retryBtn;

    // ---------------------------- Field
    CancellationTokenSource _cts = new();
    CancellationToken CT => _cts.Token;
    private TextItem Text01 => new(_textObj01);
    private TextItem Text02 => new(_textObj02);
    private TextItem Text03 => new(_textObj03);



    // ---------------------------- UnityMessage
    private async UniTaskVoid Start()
    {
        // キャンセル
        _cancelBtn.onClick.AddListener(() =>
        {
            Tasks.Cancel(ref _cts);
        });

        // 同時処理タスク
        try
        {
            await WhenAllTask(CT);
        }
        catch
        {
            Debug.Log("キャンセルされました");
        }

        Tasks.Cancel(ref _cts);

        ObserveBtn();   //  ボタン監視
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
        // R3を使用したボタン監視
        _retryBtn.OnClickAsObservable().SubscribeAwait(async (_, ct) =>
        {
            await WhenAllTask(ct);

        }, AwaitOperation.Switch)
        .RegisterTo(destroyCancellationToken);

        var f = false; // 未使用の警告避け
        if (f)
        {
            _retryBtn // 監視対象のボタン
            .OnClickAsObservable() // ボタンのクリックを監視
            .SubscribeAwait(async (_, ct) => // クリックの実行判定
            {
                await WhenAllTask(ct); // 任意のタスク

            }, AwaitOperation.Switch) // キャンセル処理された際の実行・終了方法を指定
            .RegisterTo(destroyCancellationToken); // キャンセルトークンの登録
        }
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
