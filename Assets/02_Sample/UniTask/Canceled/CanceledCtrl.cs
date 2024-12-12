using Cysharp.Threading.Tasks;
using DG.Tweening;
using Helper;
using R3;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CanceledCtrl : MonoBehaviour, IMessenger, IAwaitStarter
{
    // ---------------------------- SerializeField
    [SerializeField] private Button _btn;
    [SerializeField] private GameObject _groupObj;
    [SerializeField] private float _duration;


    // ---------------------------- Field
    private const float _zero = 0.0f;
    private const float _one = 1.0f;

    private CanvasGroup _group = null;


    // ---------------------------- UnityMessage
    /// <summary>
    /// �X�^�[�g
    /// </summary>
    /// <returns>�X�^�[�g����</returns>
    async UniTask IAwaitStarter.Start()
    {
        _group = _groupObj.GetComponent<CanvasGroup>();

        //  �t�F�[�h
        //  �Ăяo�����ƂȂ�n�߂� await �ɂ� Canceled ���g�p
        await Tasks.Canceled(StartEvent01(destroyCancellationToken));
        //  ��ƑS����������
        await Tasks.Canceled(StartEvent02(destroyCancellationToken));

        //  �{�^���Ď�
        _btn.OnClickAsObservable().SubscribeAwait(async (_, ct) =>
        {
            //  ����
            //
            //}, AwaitOperation.Switch)
            //.RegisterTo(destroyCancellationToken);
            //
            //  �̕����� �L�����Z�� �̑���Ɏw�肵�����������Ă����̂� Canceled �̕K�v�͂Ȃ�
            await StartEvent02(ct);

        }, AwaitOperation.Switch)
        .RegisterTo(destroyCancellationToken);
    }




    // ---------------------------- PrivateMethod
    /// <summary>
    /// �X�^�[�g�C�x���g
    /// </summary>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�C�x���g����</returns>
    private async UniTask StartEvent01(CancellationToken ct)
    {
        //  �t�F�[�h
        //  ��x�Ăяo������ Canceled ����Ă���Έȍ~�̏����� Canceled ����K�v�͂Ȃ�
        await FadeGroup(_group, _zero, _duration, Ease.Linear, _groupObj, ct);
        await FadeGroup(_group, _one, _duration, Ease.Linear, _groupObj, ct);
    }

    /// <summary>
    /// �O���[�v�t�F�[�h����
    /// </summary>
    /// <param name="group">�L�����o�X�O���[�v</param>
    /// <param name="end">���ڕW�l</param>
    /// <param name="duration">��������</param>
    /// <param name="ease">�C�[�X</param>
    /// <param name="obj">�I�u�W�F�N�g</param>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�t�F�[�h�����^�X�N</returns>
    private async UniTask FadeGroup
    (CanvasGroup group
    , float end
    , float duration
    , Ease ease
    , GameObject obj
    , CancellationToken ct)
    {
        await group.DOFade(end, duration)
             .SetEase(ease)
             .SetLink(obj)
             .ToUniTask(Tasks.TCB, ct);
    }

    /// <summary>
    /// �X�^�[�g�C�x���g02
    /// </summary>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�C�x���g����</returns>
    private async UniTask StartEvent02(CancellationToken ct)
    {
        await Fade(_zero);
        await Fade(_one);

        //  �G���[�������������Ȃ��ɂ�
        //  ���ʉ�����ۈ������d�Ȃ邱�Ƃ�����
        //  ���̂悤�Ɉ�����啝�Ɍ��炵����Ԃŕ\�L�ł���̂�
        //  �^�X�N��S�ċ��ʉ�����͓̂���ł��Ȃ����Ƃ�����
        async UniTask Fade(float endValue)
        {
            await _group.DOFade(endValue, _duration)
                .SetEase(Ease.Linear)
                .SetLink(_groupObj)
                .ToUniTask(Tasks.TCB, ct);
        }

        //  �ł͂ǂ�ȂƂ��ɋ��ʉ�����ׂ��H
        //  �� �قƂ�ǂ̈������ς��Ȃ��Ƃ�
    }
}
