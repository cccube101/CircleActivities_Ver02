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
    // ---------------------------- SerializeField
    [SerializeField] TMP_Text _text;
    [SerializeField] private float _duration;
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _cancelBtn;

    // ---------------------------- Field
    CancellationTokenSource _cts = new();
    private GameObject Obj => gameObject;
    bool _canStart = false;


    // ---------------------------- UnityMessage
    private async UniTaskVoid Start()
    {
        // �I�u�W�F�N�g���f�X�g���C���ꂽ�ۂɃL�����Z�������g�[�N��

        // ���̃X�N���v�g���R���|�[�l���g�Ƃ��ĕt����ꂽ�I�u�W�F�N�g���Ď�
        // �f�X�g���C�̏��������s�����ɂȂ��Ă���̂ŕ����̃g�[�N�����g�p����ۂɂ͒���
        // ��FOnDestroy��葁���L�����Z���̏��������s���ꂽ
        var dct = destroyCancellationToken;
        var objCt = _text.GetCancellationTokenOnDestroy(); // �C�ӂ̃I�u�W�F�N�g���Ď�

        ObserveBtn();   //  �{�^���Ď�

        await WaitStart(dct);   //  �{�^�����������܂őҋ@



        // ��ʓI�ȗ�O�������@
        Debug.Log("�J�n�FNormal");
        try
        {
            await Count(_cts.Token);
            Debug.Log("�I���FNormal");
        }
        catch (OperationCanceledException)
        {
            Debug.Log("�L�����Z���FNormal");
        }



        await WaitStart(dct);



        // try-catch���g��Ȃ����@
        // OperationCanceledException �� bool�^(UniTask<bool>)�ŕԂ��Ă���܂�
        Debug.Log("�J�n�FSuppress");
        if (await Count(_cts.Token).SuppressCancellationThrow())
        {
            Debug.Log("�L�����Z���FSuppress");
        }
        Debug.Log("�I���FSuppress");



        await WaitStart(dct);



        Debug.Log("�J�n�FSuppress");
        await Count(_cts.Token).SuppressCancellationThrow();
        Debug.Log("�I���FSuppress");
    }

    private void OnDestroy()
    {
        Debug.Log("�f�X�g���C");
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
        await UniTask.WaitUntil(() => _canStart, cancellationToken: ct).SuppressCancellationThrow();
        _canStart = false;
    }

    /// <summary>
    /// �J�E���g
    /// </summary>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�J�E���g</returns>
    private async UniTask Count(CancellationToken ct)
    {
        // �J�E���g
        await DOVirtual.Float(0, _duration, _duration,
            (value) =>
            {
                _text.text = $"{value:0.0}";
            })
            .SetEase(Ease.Linear)
            .SetLink(Obj)
            .ToUniTask(Tasks.TCB, ct);
    }
}
