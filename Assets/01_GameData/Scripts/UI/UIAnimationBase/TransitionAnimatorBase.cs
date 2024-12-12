using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using R3;
using R3.Triggers;

public class TransitionAnimatorBase : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField] protected UnityEvent[] _event;


    // ---------------------------- Field
    protected Dictionary<string, UnityEvent> _actions;
    protected Animator _animator = null;


    // ---------------------------- UnityMessage
    public virtual void Awake()
    {
        StartEvent();
    }

    public virtual void OnEnable()
    {
        AnimatorStateObserve();
    }

    // ---------------------------- PublicMethod
    /// <summary>
    /// �J�n�C�x���g
    /// </summary>
    public void StartEvent()
    {
        //  �L���b�V��
        _animator = GetComponent<Animator>();

        //  ���C���[���擾
        var layer = _animator.GetLayerName(0);
        var clips = _animator.runtimeAnimatorController.animationClips;

        //  ���\�b�h�i�[
        _actions = new Dictionary<string, UnityEvent>(clips.Length);
        for (int i = 0; i < clips.Length; i++)
        {
            //  "���C���[.�X�e�[�g��"
            _actions.Add($"{layer}.{clips[i].name}", _event[i]);
        }
    }

    /// <summary>
    /// �A�j���[�^�[�X�e�[�g�̊Ď�
    /// </summary>
    public void AnimatorStateObserve()
    {
        //  null���葁�����^�[��
        if (_animator == null) return;

        //  �A�j���[�^�[�Ď�
        _animator.GetBehaviour<ObservableStateMachineTrigger>()
            .OnStateEnterAsObservable()
            .Subscribe(state =>
            {
                //  �A�N�V������������
                foreach (var item in _actions)
                {
                    //  �X�e�[�g���Ŕ���
                    if (state.StateInfo.IsName(item.Key))
                    {
                        _actions[item.Key]?.Invoke();   //  ���s
                    }
                }
            })
            .AddTo(this);
    }



}
