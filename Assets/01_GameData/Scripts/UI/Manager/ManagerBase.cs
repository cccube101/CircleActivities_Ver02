using UnityEngine;
using System.Collections.Generic;


public class ManagerBase : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField] protected GameObject[] _controllers;


    // ---------------------------- Field
    protected readonly List<IMessenger> _messengers = new();



    // ---------------------------- UnityMessage
    public virtual void Awake()
    {
        //  �R���g���[���[�C���^�[�t�F�[�X�̃L���b�V��
        foreach (var item in _controllers)
        {
            _messengers.Add(item.GetComponent<IMessenger>());
        }

        //  �A�E�F�C�N����
        foreach (var message in _messengers)
        {
            if (message is IAwaken awaken)
            {
                awaken.Awake();
            }
        }
    }

    public virtual async void Start()
    {
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 20000, sequencesCapacity: 200);

        //  �X�^�[�g����
        foreach (var message in _messengers)
        {
            if (message is IStarter starter)    //  �ʏ�
            {
                starter.Start();
            }
            else if (message is IAwaitStarter awaitStarter) //  UniTask�g�p
            {
                await awaitStarter.Start();
            }
        }
    }

    public virtual void Update()
    {
        //  �I�u�W�F�N�g�����݂������X�V
        _messengers.RemoveAll(message =>
        {
            if (message is IUpdater updater)
            {
                return updater.Update();
            }
            else
            {
                return false;
            }
        });
    }
}
