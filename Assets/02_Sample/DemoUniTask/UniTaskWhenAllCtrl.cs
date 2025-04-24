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
        // �L�����Z��
        _cancelBtn.onClick.AddListener(() =>
        {
            Tasks.Cancel(ref _cts);
        });

        // ���������^�X�N
        try
        {
            await WhenAllTask(CT);
        }
        catch
        {
            Debug.Log("�L�����Z������܂���");
        }

        Tasks.Cancel(ref _cts);

        ObserveBtn();   //  �{�^���Ď�
    }

    private void OnDestroy()
    {
        Tasks.Cancel(ref _cts);
    }

    // ---------------------------- PrivateMethod
    /// <summary>
    /// �{�^���Ď�
    /// </summary>
    private void ObserveBtn()
    {
        // R3���g�p�����{�^���Ď�
        _retryBtn.OnClickAsObservable().SubscribeAwait(async (_, ct) =>
        {
            await WhenAllTask(ct);

        }, AwaitOperation.Switch)
        .RegisterTo(destroyCancellationToken);

        var f = false; // ���g�p�̌x������
        if (f)
        {
            _retryBtn // �Ď��Ώۂ̃{�^��
            .OnClickAsObservable() // �{�^���̃N���b�N���Ď�
            .SubscribeAwait(async (_, ct) => // �N���b�N�̎��s����
            {
                await WhenAllTask(ct); // �C�ӂ̃^�X�N

            }, AwaitOperation.Switch) // �L�����Z���������ꂽ�ۂ̎��s�E�I�����@���w��
            .RegisterTo(destroyCancellationToken); // �L�����Z���g�[�N���̓o�^
        }
    }

    /// <summary>
    /// ���������^�X�N
    /// </summary>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�^�X�N����</returns>
    private async UniTask WhenAllTask(CancellationToken ct)
    {
        // �������ƃ^�X�N�J�n�𓯎��ɍs��
        await UniTask.WhenAll(
            Count(Text01, 1.0f, ct),
            Count(Text02, 2.0f, ct),
            Count(Text03, 3.0f, ct));

        // �^�X�N�p�̃��X�g��p�ӂ��ǉ����Ă���s��
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
    /// �J�E���g����
    /// </summary>
    /// <param name="item">�e�L�X�g�A�C�e��</param>
    /// <param name="volume">�J�E���g��</param>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�^�X�N����</returns>
    private async UniTask Count(TextItem item, float volume, CancellationToken ct)
    {
        // �J�E���g
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
