using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DemoInput : MonoBehaviour
{
    private enum State
    {
        Enable, Disable
    }


    // ---------------------------- SerializeField
    [SerializeField] private PlayerInput _input;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _addForce;

    // ---------------------------- Fields
    private PlayerAction _action = null;
    private PlayerAction.PlayerActions PlayerAct => _action.Player;

    private Transform TR => GetComponent<Transform>();
    private Rigidbody2D RB => GetComponent<Rigidbody2D>();

    // ---------------------------- Unity Message
    private void Awake()
    {
        _action = new(); // アクションの初期化
    }

    private void OnEnable()
    {
        _action?.Enable();      // 入力アクションの有効化
        ChangeAct(State.Enable);   //  アクションの有効化
    }

    private void OnDisable()
    {
        ChangeAct(State.Disable);   //  アクションの無効化
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
    /// アクションの切換え
    /// </summary>
    /// <param name="state">アクション状態</param>
    private void ChangeAct(State state)
    {
        // 入力機器の変更
        switch (state)
        {
            case State.Enable:
                _input.onControlsChanged += input => OnControlsChanged();
                break;

            case State.Disable:
                _input.onControlsChanged -= input => OnControlsChanged();
                break;
        }

        // プレイヤーアクション
        Set(PlayerAct.Move, OnMove);
        Set(PlayerAct.Aim, OnAim);
        Set(PlayerAct.Fire, OnFire);
        void Set(InputAction input
            , Action<InputAction.CallbackContext> act)
        {
            switch (state)
            {
                case State.Enable:
                    input.started += act;
                    input.performed += act;
                    input.canceled += act;
                    break;

                case State.Disable:
                    input.started -= act;
                    input.performed -= act;
                    input.canceled -= act;
                    break;
            }
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
        RB.velocity = new Vector2(dir, RB.velocity.y) * _speed;
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

            var dir = TR.position - (TR.position - camera).normalized;
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
            ((_muzzle.position - TR.position).normalized * _addForce
            , ForceMode2D.Impulse);
    }

    #endregion
}
