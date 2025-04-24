using Alchemy.Inspector;
using System.ComponentModel.DataAnnotations;
using UnityEngine;
//  �g���Ă��Ȃ� using �͂ł��邾���폜���悤

public class CodingRule : MonoBehaviour
{
    // ---------------------------- Enum


    // ---------------------------- SerializeField


    // ---------------------------- Field


    // ---------------------------- Property



    // ---------------------------- UnityMessage



    // ---------------------------- PublicMethod




    // ---------------------------- PrivateMethod



    #region ------ Template



    #endregion



    /////////////////////////////////////////////////////////////////
    //////////////////      �R�[�f�B���O�K��       //////////////////
    /////////////////////////////////////////////////////////////////
    #region ------ CodingRule
    /*
     * �R�~�b�g�E�}�[�W����O�ɃR�����g�͂�����Ɠ��͂���
     */

    /*
     * �A�N�Z�X�C���q�͕K���t����
     *  private public protected
     */

    /*
     * �X�N���v�g���� ���� �� �ڔ��� �̏�
     *  EnemySpawner , PlayerSwitcher
     *
     *
     * �C���^�[�t�F�[�X�͓��ɑ啶���� I
     *  IItem , IEnemy
     *
     *
     * ���\�b�h���� ���� �� ���� �̏�
     *  PlayClip() , GetScore()
     *
     *
     *  �ǂ����O����
     */

    /*
     * ���̂͋��ʂŕ����镨�݂̂ɂ���
     * ���������P�ꂪ����Ƃ��͗v���k
     *
     *  Controller �� ctrl
     *
     *  GameObject �� obj
     *  RigidBody �� rb
     *  Transform �� tr
     *  RectTransform �� rect // Rect �Ƃ����^������̂Ŏg���ۂɂ͗v���k
     *
     *  Button �� btn
     *  Position �� pos
     *  Direction �� dir
     *
     *  CancellationToken �� ct
     *  CancellationTokenSource �� cts
     */

    /*
     * Input�n Button�n�̃��\�b�h�͓��� On ��t����
     *  OnMove()
     *  OnSceneTransition()
     */


    /* ------ �萔 ------
     * �A�b�p�[�X�l�[�N�P�[�X
     * �ϐ��̏ꍇ���� CONST ��t����
     * _ �ŋ�؂�
     */

    private enum State
    {
        NORMAL, ERROR
    }

    private const int CONST_SET_SCORE = 1000;



    /* ------ �ϐ� ------
     * �L�������P�[�X
     * ���� _ ��t����
     * �啶���ŋ�؂�
     */

    //  Alchemy������
    [SerializeField, Required, BoxGroup("��b�p�����[�^")] private State _state;

    private float _score = 0;
    private GameObject _playerObj = null;


    //  �z�񓙂̃R���N�V�����͕����`
    private Transform[] _transforms = null;

    //  bool �̓��ɂ� is has can �̂����ꂩ��t����
    private bool _isPlaying = false;    //  ���̏�Ԃ�
    private bool _hasBall = false;      //  �ێ����Ă��邩
    private bool _canPlay = true;       //  �\��

    //  �v���p�e�B�̓p�X�J���P�[�X
    public bool IsPlaying { get { return _isPlaying; } set { _isPlaying = value; } }
    public bool HasBall { get => _hasBall; set => _hasBall = value; }
    public bool CanPlay => _canPlay;



    /* ------ ���\�b�h�� ------
     * �p�X�J���P�[�X
     * ���� �� ���� �̏�
     * XML�R�����g��ϐ��A���ʂ܂ŕK������
     */

    /// <summary>
    /// �N���b�v�Đ�
    /// </summary>
    private void PlayClip()
    {

    }

    /// <summary>
    /// �X�R�A�擾
    /// </summary>
    /// <param name="addValue">���Z�l</param>
    /// <returns>�X�R�A�l</returns>
    private float GetScore(float addValue)
    {
        _score += addValue;
        return _score;
    }





    private void Start()
    {
        //  ���g�p��Ԃ̌x������
        if (_isPlaying && _hasBall && _canPlay)
        {
            _transforms = new[] { transform };

            _transforms[0].position = Vector3.zero;

            _isPlaying = false;
            _hasBall = false;
            _canPlay = false;

            _playerObj = gameObject;
            _playerObj.SetActive(false);
            PlayClip();

            _score = GetScore(CONST_SET_SCORE);
        }
    }

    #endregion
}
