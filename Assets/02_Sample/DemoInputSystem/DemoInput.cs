using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DemoInput : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField] private PlayerInput _input;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _addForce;

    // ---------------------------- Fields
    private PlayerAction _action = null;
    private PlayerAction.PlayerActions PlayerAct => _action.Player;

    private Transform _tr = null;
    private Rigidbody2D _rb = null;

    // ---------------------------- Unity Message
    private void Awake()
    {
        _action = new PlayerAction(); // �A�N�V�����̏�����
        _tr = GetComponent<Transform>();   //  Transform�̎擾
        _rb = GetComponent<Rigidbody2D>();  //  RigidBody2D�̎擾
    }

    private void OnEnable()
    {
        _action?.Enable();      // ���̓A�N�V�����̗L����
        EnableControllers();    //  ���̗͂L����
    }

    private void OnDisable()
    {
        DisableControllers();   //  ���̖͂�����
        _action?.Disable();     // ���̓A�N�V�����̖�����
    }

    private void OnDestroy()
    {
        _action?.Dispose();    // ���\�[�X�̉��
    }

    // ---------------------------- Public Methods


    // ---------------------------- Private Methods
    #region Input Event Handlers

    //  ------ Set InputAct

    /// <summary>
    /// ���̗͂L����
    /// </summary>
    private void EnableControllers()
    {
        // ���͋@��̕ύX
        _input.onControlsChanged += input => OnControlsChanged();

        // �v���C���[�A�N�V����
        EnableAct(PlayerAct.Move, OnMove);
        EnableAct(PlayerAct.Aim, OnAim);
        EnableAct(PlayerAct.Fire, OnFire);

        //  �A�N�V�����̗L����
        static void EnableAct
            (InputAction input
            , Action<InputAction.CallbackContext> act)
        {
            input.started += act;
            input.performed += act;
            input.canceled += act;
        }
    }

    /// <summary>
    /// ���̖͂�����
    /// </summary>
    private void DisableControllers()
    {
        // ���͋@��̕ύX
        _input.onControlsChanged -= input => OnControlsChanged();

        // �v���C���[�A�N�V����
        DisableAct(PlayerAct.Move, OnMove);
        DisableAct(PlayerAct.Aim, OnAim);
        DisableAct(PlayerAct.Fire, OnFire);

        //  �A�N�V�����̖�����
        static void DisableAct
            (InputAction input
            , Action<InputAction.CallbackContext> act)
        {
            input.started -= act;
            input.performed -= act;
            input.canceled -= act;
        }
    }

    //  ------ Event Handlers

    /// <summary>
    /// ���͋@��̕ύX
    /// </summary>
    private void OnControlsChanged()
    {
        Debug.Log($"Control scheme changed:{_input.currentControlScheme}");
    }

    //  ------ Player Actions

    /// <summary>
    /// �ړ�����
    /// </summary>
    /// <param name="ctx">�R���e�L�X�g</param>
    private void OnMove(InputAction.CallbackContext ctx)
    {
        var dir = ctx.ReadValue<Vector2>().x;
        _rb.velocity = new Vector2(dir, _rb.velocity.y) * _speed;
    }

    /// <summary>
    /// ���b�N����
    /// </summary>
    /// <param name="ctx">�R���e�L�X�g</param>
    private void OnAim(InputAction.CallbackContext ctx)
    {
        if (Camera.main != null)
        {
            var camera = Camera.main.ScreenToWorldPoint(
                    (Vector3)ctx.ReadValue<Vector2>());
            camera.z = 0;

            var dir = _tr.position - (_tr.position - camera).normalized;
            _muzzle.position = Vector3.Lerp(_muzzle.position, dir, 0.8f);
        }
    }

    /// <summary>
    /// ���Γ���
    /// </summary>
    /// <param name="ctx">�R���e�L�X�g</param>
    private void OnFire(InputAction.CallbackContext ctx)
    {
        if (ctx.phase != InputActionPhase.Started) return;

        var bullet = Instantiate(_bullet, _muzzle.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce
            ((_muzzle.position - _tr.position).normalized * _addForce
            , ForceMode2D.Impulse);
    }

    #endregion
}
