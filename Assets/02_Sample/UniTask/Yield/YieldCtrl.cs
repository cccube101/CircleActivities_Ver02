using Cysharp.Threading.Tasks;
using DG.Tweening;
using Helper;
using System.Threading;
using UnityEngine;

public class YieldCtrl : MonoBehaviour, IMessenger, IAwaitStarter
{
    // ---------------------------- SerializeField

    [SerializeField] private GameObject _groupObj;
    [SerializeField] private float _duration;

    // ---------------------------- Field
    private const float _zero = 0.0f;
    private const float _one = 1.0f;

    private CanvasGroup _group = null;


    // ---------------------------- UnityMessage

    async UniTask IAwaitStarter.Start()
    {
        //  �L���b�V��
        _group = _groupObj.GetComponent<CanvasGroup>();

        await Tasks.Canceled(StartEvent(destroyCancellationToken));
    }

    // ---------------------------- PrivateMethod

    /// <summary>
    /// �X�^�[�g�C�x���g
    /// </summary>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�C�x���g����</returns>
    private async UniTask StartEvent(CancellationToken ct)
    {
        //  �J��Ԃ�����
        while (true)
        {
            await Fade(_zero, ct);
            await Fade(_one, ct);

            //  �R���[�`���� yield return null;�Ɠ���
            await UniTask.Yield(cancellationToken: ct);
        }
    }

    /// <summary>
    /// �t�F�[�h
    /// </summary>
    /// <param name="endValue">�ڕW�l</param>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�t�F�[�h����</returns>
    private async UniTask Fade(float endValue, CancellationToken ct)
    {
        //  �t�F�[�h����
        await _group.DOFade(endValue, _duration)
            .SetEase(Ease.Linear)
            .SetLink(_groupObj)
            .ToUniTask(Tasks.TCB, ct);
    }

}