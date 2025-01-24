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
    // ---------------------------- SerializeField
    [SerializeField] private Button _btn;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject[] _groupObjects;
    [SerializeField] private float _duration;
    [SerializeField] private float _addDuration;


    // ---------------------------- Field
    private const float _zero = 0.0f;
    private const float _one = 1.0f;

    private readonly List<Tasks.GroupItem> _items = new();
    private bool _canAddTime = false;
    private float _time = 0.0f;

    // ---------------------------- UnityMessage
    /// <summary>
    /// �X�^�[�g
    /// </summary>
    void IStarter.Start()
    {
        foreach (var obj in _groupObjects)
        {
            _items.Add(new Tasks.GroupItem(obj, obj.GetComponent<CanvasGroup>()));
        }

        //  �{�^���Ď�
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
    /// �A�b�v�f�[�g
    /// </summary>
    /// <returns>�X�V����</returns>
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
    /// ���������^�X�N
    /// </summary>
    /// <param name="endValue">�ڕW�l</param>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�t�F�[�h����</returns>
    private async UniTask WhenAllFade(float endValue, CancellationToken ct)
    {
        //  ���������p���X�g
        var tasks = new List<UniTask>();
        //  �^�X�N�ۑ�
        int i = 0;  //  �����o�����߂ɉ��Z����ϐ���p��
        foreach (var item in _items)
        {
            tasks.Add(Fade());
            async UniTask Fade()
            {
                await item.Group.DOFade(endValue, _duration + i * _addDuration)
                    .SetEase(Ease.Linear)
                    .SetLink(item.Obj)
                    .ToUniTask(Tasks.TCB, ct);
            }
            i++;
        }
        //  �^�X�N���s
        await UniTask.WhenAll(tasks);
    }




}
