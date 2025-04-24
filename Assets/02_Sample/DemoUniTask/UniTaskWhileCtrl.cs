using Cysharp.Threading.Tasks;
using DG.Tweening;
using Helper;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UniTaskWhileCtrl : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField] GameObject _textObj;
    [SerializeField] private float _duration;
    [SerializeField] private Button _cancelBtn;

    // ---------------------------- Field
    CancellationTokenSource _cts = new();
    CancellationToken CT => _cts.Token;
    private TMP_Text Text => _textObj.GetComponent<TMP_Text>();



    // ---------------------------- UnityMessage
    private async UniTaskVoid Start()
    {
        ObserveBtn();   //  �{�^���Ď�

        //  �R���[�`���̂悤�ɌJ��Ԃ�����������ۂɗp���� await����
        try
        {
            await WhileTask(CT);
        }
        catch
        {
            Debug.Log("�L�����Z������܂���");
        }
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
        // �L�����Z��
        _cancelBtn.onClick.AddListener(() =>
        {
            Tasks.Cancel(ref _cts);
        });
    }

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
                .ToUniTask(Tasks.TCB, CT);

            await UniTask.Yield(cancellationToken: CT); //  yield return null �Ɠ�����p
        }
    }
}
