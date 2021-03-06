﻿
using UnityEngine;
using System.Collections.Generic;

//------------------------------------------------------------
// NOTICE:
// ミニゲームの処理を司る抽象クラス
// 派生クラスに詳細なゲームの処理を実装する
//
//------------------------------------------------------------
// TIPS:
// コンポーネントとして使いますが、AR モデルには設定しないでください
// 基本的に単体で動作できるような仕組みにしています
//
// ゲーム用のインスタンスとして生成しています
// Awake()、Start() などでリソースの生成、初期化を行ってください
//
// ゲームループに入る直前に、ゲーム用の初期化メソッドがあります
// ループするたびに初期化しないといけない要素を初期化してください
//
//
// void Action()
// * ミニゲームのメインとなる処理を記述してください
// * マーカーが認識されてない場合は動作しません
// * 必ずモデルが２体表示されていないと問題がある処理を実装してください
//
// virtual void EarlyUpdate()
// virtual void LastUpdate()
// * マーカーが認識されてなくても動作するメソッドです
// * 必要に応じて override してください
// * それぞれ、Action() の前後で動作します
//
//
// virtual void GameStart()
// * Action() など、メインループが始まる直前に実行されます
// * サドンデスなど、ゲームループを繰り返す際に必要な初期化に使用できます
//
// virtual void SuddenDeathAction()
// * サドンデス突入時にのみ実行されます
// * サドンデスが発生するゲームは override してください
// * GameStart() の直後に実行されます
//
//
// bool IsFinish()
// * ゲーム終了の判定に使用します
// * ゲーム終了状態になったら true を返すようにしてください
//
// virtual bool IsDraw()
// * 引き分けの判定に使用します
// * 引き分け状態になるゲームの場合のみ override してください
//
// Transform GetWinner()
// * 勝利プレイヤーを返すようにしてください
//
//
// string gameRule { get; }
// * ゲーム開始前のルール説明で使用します
// * 派生クラスの Start() メソッドなどで文字列を入力してください
//
//------------------------------------------------------------

public abstract class AbstractGame : MonoBehaviour
{
  // マーカー認識が必須の処理用
  public abstract void Action();
  // マーカーが認識されてなくても動作させたいオブジェクト用（Action() の前）
  public virtual void EarlyUpdate() { }
  // マーカーが認識されてなくても動作させたいオブジェクト用（Action() の後）
  public virtual void LastUpdate() { }


  // ゲームループの始まる直前の処理用
  public virtual void GameStart() { }
  // サドンデス時の特殊な処理用
  public virtual void SuddenDeathAction() { }


  // ゲームが終了したとき true を返す
  public abstract bool IsFinish();
  // ゲームが引き分けなら true を返す
  // TIPS: 引き分けにならないゲームは override しなくて大丈夫です
  public virtual bool IsDraw() { return false; }


  // ゲームの勝者
  public abstract Transform GetWinner();


  /// <summary> ゲームルールの説明 </summary>
  public string gameRule { get; protected set; }


  /// <summary> プレイヤーの情報を取り出す </summary>
  public GameManager gameManager { get; set; }

  public ARModel player1 { get; protected set; }
  public ARModel player2 { get; protected set; }

  // TIPS: プレイヤーの入力取得用プロパティ
  static GameController controller { get { return GameController.instance; } }
  protected static IEnumerable<KeyCode> inputP1 { get { return controller.player1; } }
  protected static IEnumerable<KeyCode> inputP2 { get { return controller.player2; } }
}
