
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Helper;
using R3;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WhenAllCtrl : MonoBehaviour, IMessenger, IStarter, IUpdater
{
    private class GroupParam
    {
        public GroupParam(CanvasGroup group, GameObject obj)
        {
            Group = group;
            Obj = obj;
        }

        public CanvasGroup Group;
        public GameObject Obj;
    }

    // ---------------------------- SerializeField
    [SerializeField] private Button _btn;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject[] _groupObjects;
    [SerializeField] private float _duration;
    [SerializeField] private float _addDuration;


    // ---------------------------- Field
    private const float _zero = 0.0f;
    private const float _one = 1.0f;

    private List<GroupParam> _params = new();
    private bool _canAddTime = false;
    private float _time = 0.0f;

    // ---------------------------- UnityMessage
    /// <summary>
    /// スタート
    /// </summary>
    void IStarter.Start()
    {
        foreach (var obj in _groupObjects)
        {
            _params.Add(new GroupParam(obj.GetComponent<CanvasGroup>(), obj));
        }

        //  ボタン監視
        _btn.OnClickAsObservable().SubscribeAwait(async (_, ct) =>
        {
            _canAddTime = true;

            _time = 0.0f;
            await WhenAllFade(_zero, ct);
            _time = 0.0f;
            await WhenAllFade(_one, ct);

            _canAddTime = false;

        }, AwaitOperation.Switch)
        .RegisterTo(destroyCancellationToken);
    }

    /// <summary>
    /// アップデート
    /// </summary>
    /// <returns>更新処理</returns>
    bool IUpdater.Update()
    {
        if (_canAddTime)
        {
            _time += Time.deltaTime;
        }
        _text.text = _time.ToString("0.00");
        return false;
    }

    // ---------------------------- PrivateMethod
    /// <summary>
    /// 同時処理タスク
    /// </summary>
    /// <param name="endValue">目標値</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>フェード処理</returns>
    private async UniTask WhenAllFade(float endValue, CancellationToken ct)
    {
        //  同時処理用リスト
        var tasks = new List<UniTask>();
        //  タスク保存
        int i = 0;  //  差を出すために加算する変数を用意
        foreach (var param in _params)
        {
            tasks.Add(Fade());
            async UniTask Fade()
            {
                await param.Group.DOFade(endValue, _duration + i * _addDuration)
                    .SetEase(Ease.Linear)
                    .SetLink(param.Obj)
                    .ToUniTask(Tasks.TCB, ct);
            }
            i++;
        }
        //  タスク実行
        await UniTask.WhenAll(tasks);
    }




}
