using Alchemy.Inspector;
using System.ComponentModel.DataAnnotations;
using UnityEngine;
//  使っていない using はできるだけ削除しよう

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
    //////////////////      コーディング規約       //////////////////
    /////////////////////////////////////////////////////////////////
    #region ------ CodingRule
    /*
     * コミット・マージする前にコメントはきちんと入力する
     */

    /*
     * アクセス修飾子は必ず付ける
     *  private public protected
     */

    /*
     * スクリプト名は 名詞 → 接尾語 の順
     *  EnemySpawner , PlayerSwitcher
     *
     *
     * インターフェースは頭に大文字の I
     *  IItem , IEnemy
     *
     *
     * メソッド名は 動詞 → 名詞 の順
     *  PlayClip() , GetScore()
     *
     *
     *  どれも例外あり
     */

    /*
     * 略称は共通で分かる物のみにする
     * 略したい単語があるときは要相談
     *
     *  Controller → ctrl
     *
     *  GameObject → obj
     *  RigidBody → rb
     *  Transform → tr
     *  RectTransform → rect // Rect という型もあるので使う際には要相談
     *
     *  Button → btn
     *  Position → pos
     *  Direction → dir
     *
     *  CancellationToken → ct
     *  CancellationTokenSource → cts
     */

    /*
     * Input系 Button系のメソッドは頭に On を付ける
     *  OnMove()
     *  OnSceneTransition()
     */


    /* ------ 定数 ------
     * アッパースネークケース
     * 変数の場合頭に CONST を付ける
     * _ で区切る
     */

    private enum State
    {
        NORMAL, ERROR
    }

    private const int CONST_SET_SCORE = 1000;



    /* ------ 変数 ------
     * キャメルケース
     * 頭に _ を付ける
     * 大文字で区切る
     */

    //  Alchemy属性例
    [SerializeField, Required, BoxGroup("基礎パラメータ")] private State _state;

    private float _score = 0;
    private GameObject _playerObj = null;


    //  配列等のコレクションは複数形
    private Transform[] _transforms = null;

    //  bool の頭には is has can のいずれかを付ける
    private bool _isPlaying = false;    //  その状態か
    private bool _hasBall = false;      //  保持しているか
    private bool _canPlay = true;       //  可能か

    //  プロパティはパスカルケース
    public bool IsPlaying { get { return _isPlaying; } set { _isPlaying = value; } }
    public bool HasBall { get => _hasBall; set => _hasBall = value; }
    public bool CanPlay => _canPlay;



    /* ------ メソッド名 ------
     * パスカルケース
     * 動詞 → 名詞 の順
     * XMLコメントを変数、結果まで必ず書く
     */

    /// <summary>
    /// クリップ再生
    /// </summary>
    private void PlayClip()
    {

    }

    /// <summary>
    /// スコア取得
    /// </summary>
    /// <param name="addValue">加算値</param>
    /// <returns>スコア値</returns>
    private float GetScore(float addValue)
    {
        _score += addValue;
        return _score;
    }





    private void Start()
    {
        //  未使用状態の警告避け
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
