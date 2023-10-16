# MMCFeedbacks ってなぁに？
- 色々なものをTweenさせてフィードバック（ゲームの手触り）を作るツール
- 有能アセットFEELを真似て作ったのでmimic(真似をする)という命名になっている

※UniRx,UniTask,DoTweenの導入が必須
## フィードバックの作り方
- FeedbackPlayerコンポーネントをアタッチし、`AddFeedback▼`からフィードバックを追加する
- フィードバックは[menu]から、削除・複製・リセットを行う　
   ![Image](/Assets/Documentation/img2.png)
## 使い方
- FeedbackPlayerにはFeedbackPlay()・FeedbackStop()メソッドが定義されている
- Editor再生中は`Play` `Stop`のボタンを押して、フィードバックの動作を確認できる
- FeedbackPlayerの再生オプション
  - すべてのフィードバックを同時に再生する「Concurrent」
  - 順番にフィードバックを再生する「Sequence」
  - Sequenceを指定回数ループさせる「Loop」

   ![Image 1](/Assets/Documentation/img1.png)
## 拡張について
- IFeedbackを継承したクラスを作成し、Play()に実行したい処理を書く。
- Stateを実行時にRunning, 完了時にCompletedにする(※必須)
## UPMを使った導入方法
- Window => Package Manager => Add package from git URL...に
```text
https://github.com/Ayagi3678/MMCFeedbacks.git?path=/Assets/MMCFeedbacks
```
