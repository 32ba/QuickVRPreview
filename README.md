# QuickVRPreview

Unity Editor の Play モードで、SteamVR ヘッドセットを使ってシーンを VR プレビューできるツールです。

シーンに配置 → Play モード → ボタンを押すだけで VR 確認を開始できます。

## インストール

### Unity Package Manager (Git URL)

1. Unity Editor で **Window > Package Manager** を開く
2. **+** > **Add package from git URL...**
3. 以下を入力:

```
https://github.com/32ba/QuickVRPreview.git
```

### 前提条件

以下のパッケージが必要です（`package.json` の依存で自動インストールされます）:

- `com.unity.xr.management` (4.3.3+)
- `com.unity.xr.openxr` (1.8.2+)
- `com.unity.inputsystem` (1.7.0+)

## セットアップ（初回のみ）

1. **Edit > Project Settings > XR Plug-in Management** で **OpenXR** を有効化（PC, Mac & Linux Standalone）
2. OpenXR の **Interaction Profiles** に使用するコントローラーを追加（Valve Index, HTC Vive 等）
3. **「Initialize XR on Startup」のチェックを外す**（通常の作業に影響させないため）
4. SteamVR がインストール済みであること

## 使い方

1. メニューから **GameObject > QuickVRPreview** でシーンに配置
2. **Play モード**に入る
3. Inspector の **「Start VR」** ボタンを押す → SteamVR が起動
4. ヘッドセットを被ってシーンを確認
5. 確認が終わったら **「Stop VR」** ボタンを押す、または Play モードを停止

## 操作方法

| 入力 | 動作 |
|------|------|
| 左スティック | 前後左右の自由移動（頭の向き基準） |
| 右スティック 左右 | 水平回転 |
| 右スティック 上下 | 上下移動 |

Inspector から `moveSpeed`、`turnSpeed`、`verticalSpeed` を調整できます。

## 活用例

- VRChat アバターを Gesture Manager / Av3Emulator と併用して VR 視点で確認
- VR 向けシーンのスケール感チェック
- ライティングやシェーダーの VR での見え方確認

## License

[MIT](LICENSE)
