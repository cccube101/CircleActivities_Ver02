
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : TransitionAnimatorBase, ITransitionAnimation
{
    // ---------------------------- SerializeField
    [SerializeField] private Image _img;


    [SerializeField] private Color _normalColor;


    [SerializeField] private Color _highlightColor;


    [SerializeField] private Color _pressColor;



    // ---------------------------- Field



    // ---------------------------- UnityMessage



    // ---------------------------- PublicMethod
    #region ------ StateAnimation
    /// <summary>
    /// �m�[�}���C�x���g
    /// </summary>
    public void Normal()
    {
        UpdateAnimation(_normalColor);
    }

    /// <summary>
    /// �n�C���C�g�C�x���g
    /// </summary>
    public void Highlighted()
    {
        UpdateAnimation(_highlightColor);

    }

    /// <summary>
    /// �v���X�C�x���g
    /// </summary>
    public void Pressed()
    {
        UpdateAnimation(_pressColor);

    }

    /// <summary>
    /// �Z���N�g�C�x���g
    /// </summary>
    public void Selected()
    {
        UpdateAnimation(_normalColor);
    }

    /// <summary>
    /// �f�B�T�u���C�x���g
    /// </summary>
    public void Disabled()
    {

    }

    #endregion

    // ---------------------------- PrivateMethod
    /// <summary>
    /// �A�j���[�V�����X�V
    /// </summary>
    /// <param name="color">�摜�F</param>
    private void UpdateAnimation(Color color)
    {
        _img.color = color;
    }



}
