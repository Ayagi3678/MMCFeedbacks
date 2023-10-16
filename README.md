# MMCFeedbacks とは？
- 色々なものをTweenさせてフィードバック（ゲームの手触り）を作るツール
- 有能アセットFEELを真似て作ったのでmimic(真似をする)という命名になっている

※UniRx,UniTask,DoTweenの導入が必須
## 目次
- [MMCFeedbackのつかいかた](https://github.com/Ayagi3678/MMCFeedbacks#mmcfeedback%E3%81%AE%E3%81%A4%E3%81%8B%E3%81%84%E3%81%8B%E3%81%9F))
- [Feedback Player再生オプション](https://github.com/Ayagi3678/MMCFeedbacks/edit/main/README.md#feedback-player%E5%86%8D%E7%94%9F%E3%82%AA%E3%83%97%E3%82%B7%E3%83%A7%E3%83%B3)
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
- IFeedbackを継承したクラスを作成し、Play()に実行したい処理を書く。
- Stateを実行時にRunning, 完了時にCompletedにする(※必須)
## UPMを使った導入方法
- Window => Package Manager => Add package from git URL...に
```text
https://github.com/Ayagi3678/MMCFeedbacks.git?path=/Assets/MMCFeedbacks
```
