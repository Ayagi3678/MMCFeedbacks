# MMCFeedbacks とは？
- 色々なものをTweenさせて簡単にフィードバック（ゲームの手触り）が作れるツール
- 有能アセットFEELを真似て作ったのでmimicという命名になっている

※[**UniRx**](https://github.com/neuecc/UniRx), [**UniTask**](https://github.com/Cysharp/UniTask), [**DoTween**](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676?locale=ja-JP)の導入が必須（無いとエラーがでます）

（追記：さらに機能が強化された[**MMMCFeedback**](https://github.com/Ayagi3678/MMMCFeedbacks)を公開！）
## MMCFeedbackのつかいかた
- #### フィードバックを追加する
  -  このライブラリを導入すると追加されるコンポーネント、`Feedback Player`を適当なObjectにつける
  -  `Feedback Player`内の`AddFeedback▼`というボタンから使うフィードバックを選択して追加
  - [Feedback一覧](https://github.com/Ayagi3678/MMCFeedbacks/wiki#feedback%E3%81%AE%E7%A8%AE%E9%A1%9E)
- #### フィードバックの設定
  - 追加したフィードバックは[menu]から、削除・複製・リセットを行う
   - インスペクターから、参照や数値を変更する
   -  Editor再生中は`Play` `Stop`のボタンを押して、フィードバックの動作を確認できる

    ![Image](/Assets/Documentation/img2.png)
- #### 注意点
  - Editorを再生しながら作業すると動作を確認しながらフィードバックを調整できる。
  - しかし再生を止めると値が戻ってしまうため、`Copy All`ボタンを押して`Copy`する
  - 再生を止めたあとに`Copy All`から`Paste Value`することで状態を保存できる
   
## Feedback Player再生オプション
  - すべてのフィードバックを同時に再生する`Concurrent`
  - 順番にフィードバックを再生する`Sequence`
  - Sequenceを指定回数ループさせる`Loop`

   ![Image 1](/Assets/Documentation/img1.png)
## 拡張について
- Feedbackを継承したクラスを作成し、overrideメソッドでいろいろする
- クラスには[Serializable]というアトリビュートをつける
- Stateを実行時にRunning, 完了時にCompletedにする(※必須)
## UPMを使った導入方法
- Window => Package Manager => Add package from git URL...に
```text
https://github.com/Ayagi3678/MMCFeedbacks.git?path=/Assets/MMCFeedbacks
```
