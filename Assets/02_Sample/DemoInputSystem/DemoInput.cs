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
        _action = new PlayerAction(); // アクションの初期化
        _tr = GetComponent<Transform>();   //  Transformの取得
        _rb = GetComponent<Rigidbody2D>();  //  RigidBody2Dの取得
    }

    private void OnEnable()
    {
        _action?.Enable();      // 入力アクションの有効化
        EnableControllers();    //  入力の有効化
    }

    private void OnDisable()
    {
        DisableControllers();   //  入力の無効化
        _action?.Disable();     // 入力アクションの無効化
    }

    private void OnDestroy()
    {
        _action?.Dispose();    // リソースの解放
    }

    // ---------------------------- Public Methods


    // ---------------------------- Private Methods
    #region Input Event Handlers

    //  ------ Set InputAct

    /// <summary>
    /// 入力の有効化
    /// </summary>
    private void EnableControllers()
    {
        // 入力機器の変更
        _input.onControlsChanged += input => OnControlsChanged();

        // プレイヤーアクション
        EnableAct(PlayerAct.Move, OnMove);
        EnableAct(PlayerAct.Aim, OnAim);
        EnableAct(PlayerAct.Fire, OnFire);

        //  アクションの有効化
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
    /// 入力の無効化
    /// </summary>
    private void DisableControllers()
    {
        // 入力機器の変更
        _input.onControlsChanged -= input => OnControlsChanged();

        // プレイヤーアクション
        DisableAct(PlayerAct.Move, OnMove);
        DisableAct(PlayerAct.Aim, OnAim);
        DisableAct(PlayerAct.Fire, OnFire);

        //  アクションの無効化
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
    /// 入力機器の変更
    /// </summary>
    private void OnControlsChanged()
    {
        Debug.Log($"Control scheme changed:{_input.currentControlScheme}");
    }

    //  ------ Player Actions

    /// <summary>
    /// 移動入力
    /// </summary>
    /// <param name="ctx">コンテキスト</param>
    private void OnMove(InputAction.CallbackContext ctx)
    {
        var dir = ctx.ReadValue<Vector2>().x;
        _rb.velocity = new Vector2(dir, _rb.velocity.y) * _speed;
    }

    /// <summary>
    /// ルック入力
    /// </summary>
    /// <param name="ctx">コンテキスト</param>
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
    /// 発火入力
    /// </summary>
    /// <param name="ctx">コンテキスト</param>
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
