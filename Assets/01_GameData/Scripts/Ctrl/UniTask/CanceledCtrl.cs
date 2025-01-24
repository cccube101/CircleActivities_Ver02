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

    private Tasks.GroupItem _item;


    // ---------------------------- UnityMessage
    /// <summary>
    /// �X�^�[�g
    /// </summary>
    /// <returns>�X�^�[�g����</returns>
    async UniTask IAwaitStarter.Start()
    {
        _item = new Tasks.GroupItem(_groupObj, _groupObj.GetComponent<CanvasGroup>());

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
        await FadeGroup(_item, _zero, _duration, Ease.Linear, ct);
        await FadeGroup(_item, _one, _duration, Ease.Linear, ct);
    }

    /// <summary>
    /// �O���[�v�t�F�[�h����
    /// </summary>
    /// <param name="item">�O���[�v�A�C�e��</param>
    /// <param name="end">���ڕW�l</param>
    /// <param name="duration">��������</param>
    /// <param name="ease">�C�[�X</param>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�t�F�[�h�����^�X�N</returns>
    private async UniTask FadeGroup
    (Tasks.GroupItem item
    , float end
    , float duration
    , Ease ease
    , CancellationToken ct)
    {
        await item.Group.DOFade(end, duration)
             .SetEase(ease)
             .SetLink(item.Obj)
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
            await _item.Group.DOFade(endValue, _duration)
                .SetEase(Ease.Linear)
                .SetLink(_item.Obj)
                .ToUniTask(Tasks.TCB, ct);
        }

        //  �ł͂ǂ�ȂƂ��ɋ��ʉ�����ׂ��H
        //  �� �قƂ�ǂ̈������ς��Ȃ��Ƃ�
    }
}
