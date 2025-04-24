using Cysharp.Threading.Tasks;
using DG.Tweening;
using Helper;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UniTaskCancelCtrl : MonoBehaviour
{
    private enum CancelType
    {
        Normal,
        InMethod,
        Destroy,
    }
    // ---------------------------- SerializeField
    [SerializeField] GameObject _textObj;
    [SerializeField] private float _duration;
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _cancelBtn;


    // ---------------------------- Field
    CancellationTokenSource _cts = new();
    private TMP_Text Text => _textObj.GetComponent<TMP_Text>();
    bool _canStart = false;

    // ---------------------------- UnityMessage
    // Awake�ł� await ���������̂�����Έȉ��̂悤��UniTaskVoid���g��
    //private async UniTaskVoid Awake()
    //{
    //    await UniTask.Yield(); // �C�ӂ̃^�X�N
    //}

    private async UniTaskVoid Start()
    {
        // ------ �L�����Z���������@�̗� ------


        //�g�[�N���̕ϐ���
        // �C�ӂ̃^�C�~���O�ŃL�����Z������g�[�N��
        var ct = _cts.Token;
        // �I�u�W�F�N�g���f�X�g���C���ꂽ�ۂɃL�����Z�������g�[�N��
        // �C�ӂ̃^�C�~���O�ŃL�����Z�����Ȃ��ꍇ�L��
        var dct = destroyCancellationToken;
        var objCt = _textObj.GetCancellationTokenOnDestroy();

        ObserveBtn();   //  �{�^���Ď�



        await WaitStart(dct);   //  �{�^�����������܂őҋ@



        // ��ʓI�ȗ�O�������@
        // �L�����Z��������C�ӂ̃^�C�~���O�ōs���ꍇ�Ɏg�p
        try
        {
            await NormalCount(ct);
            Debug.Log("��ʓI�ȃ^�X�N���I�����܂���");
        }
        catch
        {
            Debug.Log("��ʓI�ȃ^�X�N���L�����Z������܂���");
        }



        await WaitStart(dct);   //  �{�^�����������܂őҋ@



        // �Ăяo����̊֐����ŃL�����Z������ꍇ
        // ��O�����̌Ăяo���ʒu�𒲐����邱�Ƃ��ł���
        await InMethodCount(ct);



        await WaitStart(dct);   //  �{�^�����������܂őҋ@



        //  try-catch���g��Ȃ����@
        // �C�ӂ̃^�C�~���O�ŃL�����Z�����Ȃ��ꍇ�L��
        if (await DestroyCount(objCt).SuppressCancellationThrow())
        {
            Debug.Log("�f�X�g���C�ˑ��̃^�X�N���L�����Z������܂���");
            return;
        }
        // ����
        // await Tasks.Canceled(DestroyCount(objCt));

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
        // �X�^�[�g
        _startBtn.onClick.AddListener(() =>
        {
            _canStart = true;
        });

        // �L�����Z��
        _cancelBtn.onClick.AddListener(() =>
        {
            _canStart = false;
            Tasks.Cancel(ref _cts);
            _cts = new();
        });
    }

    /// <summary>
    /// �{�^�����������܂őҋ@
    /// </summary>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�ҋ@����</returns>
    private async UniTask WaitStart(CancellationToken ct)
    {
        try
        {
            await UniTask.WaitUntil(() => _canStart, cancellationToken: ct);
        }
        catch
        {
        }
        _canStart = false;
    }

    /// <summary>
    /// �J�E���g
    /// </summary>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�J�E���g�I�� �܂��� �L�����Z��</returns>
    private async UniTask NormalCount(CancellationToken ct)
    {
        await Count(CancelType.Normal, ct);
    }

    /// <summary>
    /// �J�E���g
    /// </summary>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�J�E���g�I�� �܂��� �L�����Z��</returns>
    private async UniTask InMethodCount(CancellationToken ct)
    {
        try
        {
            await Count(CancelType.InMethod, ct);
            Debug.Log("�֐����ŃL�����Z�������^�X�N���I�����܂���");
        }
        catch
        {
            Debug.Log("�֐����ŃL�����Z�������^�X�N���L�����Z������܂���");
        }
    }

    /// <summary>
    /// �J�E���g
    /// </summary>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�J�E���g�I�� �܂��� �L�����Z��</returns>
    private async UniTask DestroyCount(CancellationToken ct)
    {
        await Count(CancelType.Destroy, ct);
    }

    /// <summary>
    /// �J�E���g
    /// </summary>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�J�E���g</returns>
    private async UniTask Count(CancelType type, CancellationToken ct)
    {
        // ���
        var text = type switch
        {
            CancelType.Normal => "Normal",
            CancelType.InMethod => "InMethod",
            CancelType.Destroy => "Destroy",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        // �J�E���g
        await DOVirtual.Float(0, _duration, _duration,
            (value) =>
            {
                Text.text = $"{text}:{value:0.0}";
            })
            .SetEase(Ease.Linear)
            .SetLink(_textObj)
            .ToUniTask(Tasks.TCB, ct);
    }
}
