using Cysharp.Threading.Tasks;
using DG.Tweening;
using Helper;
using System.Threading;
using TMPro;
using UnityEngine;

public class UniTaskWhileCtrl : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField] GameObject _textObj;
    [SerializeField] private float _duration;

    // ---------------------------- Field
    private TMP_Text Text => _textObj.GetComponent<TMP_Text>();



    // ---------------------------- UnityMessage
    private async UniTaskVoid Start()
    {
        //  �R���[�`���̂悤�ɌJ��Ԃ�����������ۂɗp���� await����
        await WhileTask(destroyCancellationToken).SuppressCancellationThrow();
    }

    // ---------------------------- PrivateMethod
    /// <summary>
    /// �J��Ԃ�����
    /// </summary>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�J��Ԃ�����</returns>
    private async UniTask WhileTask(CancellationToken ct)
    {
        while (true)
        {
            // �J�E���g
            await DOVirtual.Float(0, _duration, _duration,
                (value) =>
                {
                    Text.text = $"{value:0.0}";
                })
                .SetEase(Ease.Linear)
                .SetLink(_textObj)
                .ToUniTask(Tasks.TCB, ct);

            await UniTask.Yield(cancellationToken: ct); //  yield return null �Ɠ�����p
        }
    }
}
