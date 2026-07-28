using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ぷかぷかとオブジェクトを上下(左右)に正弦波で自然に揺らす
public class FloatingObject : MonoBehaviour
{
    [Header("ゆれの速度(負の値で逆回転)")]
    [SerializeField] private float speedX = 2.0f;
    [SerializeField] private float speedY = 2.0f;
    [SerializeField] private float speedZ = 2.0f;

    [Header("ゆれの大きさ")]
    [SerializeField] private float amplitudeX = 0.5f;
    [SerializeField] private float amplitudeY = 0.5f;
    [SerializeField] private float amplitudeZ = 0.5f;

    [Header("動く方向(XYで円運動)")]
    [SerializeField] private bool movableX = false;
    [SerializeField] private bool movableY = true;
    [SerializeField] private bool movableZ = false;

    [Header("ランダムにずらすか否か")]
    [SerializeField] private bool isRandom;
    private float difference;   // 動作ずらし用変数

    private float timeFromStart;

    private Transform tf;

    void Start()
    {
        tf = GetComponent<Transform>();

        // 複数のオブジェクトが同時に動かないよう、違いをランダムで設定
        float rnd = Random.Range(0, 1.0f);
        // 条件演算子 isRandomがtrueなら 乱数を、そうじゃないなら0を代入
        difference = isRandom? rnd : 0;
    }

    void Update()
    {
        // Sin関数に増え続ける値を引数に与えると-1～+1の値が返ってくる
        // Time.time：シーン開始から経過した時間
        // 引数に渡す数を掛け算等で増やすと振幅速度が上がる
        // 戻り値の値そのものを掛け算等で増やすと振幅量が上がる
        timeFromStart += Time.deltaTime; // シーン再読み込み対策(Time.timeだとズレる為)
        float x = Mathf.Cos(timeFromStart * speedX + difference) * amplitudeX;
        float y = Mathf.Sin(timeFromStart * speedY + difference) * amplitudeY;
        float z = Mathf.Cos(timeFromStart * speedZ + difference) * amplitudeZ;

        // 「条件演算子」
        // 「条件? 真 : 偽」
        float moveX = movableX? x : 0; // movableXが真なら x、偽なら 0を返し、それを代入
        float moveY = movableY? y : 0;
        float moveZ = movableZ? z : 0;

        var move = new Vector3(moveX, moveY, moveZ);

        // 移動処理(キャラのRotationが傾いても移動方向が変わってほしくないので Space.World で)
        // 座標を直接上書きする方式ではないので、他の移動スクリプトとも恐らく併用可能
        tf.Translate(move * Time.deltaTime, Space.World);
    }
}
