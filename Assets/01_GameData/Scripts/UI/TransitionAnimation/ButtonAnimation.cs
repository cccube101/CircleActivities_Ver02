
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
    /// ノーマルイベント
    /// </summary>
    public void Normal()
    {
        UpdateAnimation(_normalColor);
    }

    /// <summary>
    /// ハイライトイベント
    /// </summary>
    public void Highlighted()
    {
        UpdateAnimation(_highlightColor);

    }

    /// <summary>
    /// プレスイベント
    /// </summary>
    public void Pressed()
    {
        UpdateAnimation(_pressColor);

    }

    /// <summary>
    /// セレクトイベント
    /// </summary>
    public void Selected()
    {
        UpdateAnimation(_normalColor);
    }

    /// <summary>
    /// ディサブルイベント
    /// </summary>
    public void Disabled()
    {

    }

    #endregion

    // ---------------------------- PrivateMethod
    /// <summary>
    /// アニメーション更新
    /// </summary>
    /// <param name="color">画像色</param>
    private void UpdateAnimation(Color color)
    {
        _img.color = color;
    }



}
