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
        // ���������^�X�N
        await WhenAllTask(destroyCancellationToken).SuppressCancellationThrow();

        ObserveBtn();   //  �{�^���Ď�
    }

    // ---------------------------- PrivateMethod
    /// <summary>
    /// �{�^���Ď�
    /// </summary>
    private void ObserveBtn()
    {
        // R3���g�p�����{�^���Ď�

        // �Ď��Ώۂ̃{�^��
        _retryBtn
            // �N���b�N�̊Ď�
            .OnClickAsObservable()
            // �N���b�N���ꂽ�ۂɒʒm���o��
            // _ �̕����Ɏ󂯎��l�i����͋�Ȃ̂� _�j
            // ct �̕����ɉ��L�Ŏw�肵���L�����Z���g�[�N����ݒ�
            .SubscribeAwait(async (_, ct) =>
        {
            await WhenAllTask(ct); // �C�ӂ̏���

            // �L�����Z�����ꂽ�ۂ̎��s�������w��
            // Switch�Ȃ���s���̃^�X�N���L�����Z�����ĐV�����^�X�N��D�悷��
        }, AwaitOperation.Switch)
        // �g�[�N�����w��
        .RegisterTo(destroyCancellationToken);
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
