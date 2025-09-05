# TJAPlayer3-f
最終更新日:2025/09/06(Mr-Ojii)

このReadmeはTJAPlayer3のReadmeを基に作成いたしました。

## はじめに
このソフトウェアは、アーケード/家庭用ゲーム「太鼓の達人」シリーズ用、譜面ビューアーです。  
このソフトウェアは、Mr-OjiiがTJAPlayer3というシミュレーターを改造したものです。  

* 太鼓さん次郎・TJAPlayer等で使われている.tjaファイル
* Koioto等に使われている.tcc .tcm .tciファイル

を読み込み、再生することができます。  
(すべての対応ファイルが読み込めるわけではありません。)  

**もともとはシミュレーターとして開発していましたが、現在は、譜面ビューアーとして開発を続行しています。**  


## 使用上の注意
* TJAPlayer3-fはオープンソースソフトウェアです。このソフトウェア・スキンはすべてMITライセンスに準拠します。
* プログラムの制作者(Mr-Ojii)は、TJAPlayer3-f本体(GitHubからのダウンロード)とデフォルトのスキンのサポートのみ行います。
* すべての環境で動作確認はできないので、動いたら運がいい、程度でお願いします。
* 常時60fpsを保てないPCでの動作は期待できません。
* このプログラムを使用し発生した、いかなる不具合・損失に対しても、一切の責任を負いません。  
  このソフトウェアを使用する場合は、**全て自己責任**でお願いします。


## 動画、配信等でのご利用について
TJAPlayer3-fを動画共有サイトやライブ配信サービス、ウェブサイトやブログ等でご利用になられる場合、  
バンダイナムコエンターテインメント公式のものでないこと、他のソフトウェアと混同しないよう配慮をお願いいたします。  
また、タグ機能のあるサイトの場合、「TJAPlayer3-f」「TJAP3-f」といったタグを付けることで、  
他のソフトウェアとの誤解を防ぐとともに、関連動画として出やすくなるメリットがあるため、推奨します。 


## TJAPlayer3-fの改造・再配布(二次配布)を行う場合について
TJAPlayer3-f、デフォルトスキンはMITライセンスで制作されています。  
MITライセンスのルールのもと、改造・再配布を行うことは自由ですが、**全て自己責任**でお願いします。  
また、使用しているライブラリのライセンス上、**必ず**「ThirdPartyLicenses」フォルダを同梱の上、改造・再配布をお願いします。  
外部スキンや、譜面パッケージを同梱する場合は、それぞれの制作者のルールや規約を守ってください。  
これらにTJAPlayer3-fのライセンスは適用されません。


## 質問をする前に
質問をする前に、

1. 調べる前に考える
2. 人に聞く前に調べる
3. 過去に同じような質問がなかったか調べる
4. 使用しているパソコンの環境、どういう動作をしたら不具合を起こしたかの過程等を添えて連絡する

この4つのルールを守ってくださいますよう、よろしくお願いいたします。


## バグ報告について 
不具合を発見した場合はGitHub Issuesにてご報告いただくようお願い申し上げます。
また、プログラムが落ちるようなエラーである場合、情報を開発者に送信するような仕様になっております。ご了承ください。


## 追加機能について
「AdditionalFeatures.md」で説明いたします。


## 推奨動作環境
#### OS
* Windows 10以降のWindows (x86, x64)
* macOS 10.15 "Catalina"以降のmacOS (x64, arm64)
* デスクトップ環境構築済みの Linux ディストリビューション 最新安定版 (x64, arm64)

#### CPU
* マルチスレッド対応
* x86,x64の場合、SSE対応(BASS)


## 実行方法
### Windows環境
ダウンロード後、zipファイルを解凍し、フォルダ内に入っているTJAPlayer3-f.exeを実行してください。

### macOS環境
ダウンロード後、zipファイルを解凍し、フォルダ内に入っているTJAPlayer3-fを実行してください。

### Linux環境
TJAPlayer3-fのダウンロードごとに、zipファイルを解凍し、  
TJAPlayer3-fが存在するディレクトリをカレントディレクトリとしたターミナルで  
```sh
chmod +x TJAPlayer3-f.AppImage
```

をしてから、TJAPlayer3-f.AppImageを実行してください。


## 開発環境(動作確認環境)
#### OS
* Windows 11(Ver.24H2) (x64)
* macOS 15.6.1 (arm64)

#### Editor
* Visual Studio Community 2022
* Visual Studio Code


## 開発体制について
masterブランチでほぼすべての開発を行います。  
(基本的なものはです。大規模なテスト実装などは、別のブランチに移行するかもしれません。)


## 開発状況
|バージョン |日付(JST) |                                        実装内容                                        |
|:---------:|:--------:|:---------------------------------------------------------------------------------------|
|Ver.2.0.0.0|??????????|未定                                                                                    |


## デフォルトスキンについて
一部画像は、TJAPlayer3のデフォルトスキンから流用しています。


## ライセンス関係
Fork元より使用しているライブラリ
* [bass](https://www.un4seen.com/bass.html)
* FDK21(改造しているため、FDKとは呼べないライブラリと化しています)

以下のライブラリを追加いたしました。
* [ReadJEnc](https://github.com/hnx8/ReadJEnc)
* [FFmpeg.AutoGen](https://github.com/Ruslan-B/FFmpeg.AutoGen)
* [SDL3](https://www.libsdl.org/)
* [ppy/SDL3-CS](https://github.com/ppy/SDL3-CS)
* [discord-rpc-csharp](https://github.com/Lachee/discord-rpc-csharp)
* [M+ FONTS](https://osdn.net/projects/mplus-fonts/)
* [managed-midi](https://github.com/atsushieno/managed-midi)
* [ManagedBass](https://github.com/ManagedBass/Home)
* [SkiaSharp](https://github.com/mono/SkiaSharp)
* [Tomlyn](https://github.com/xoofx/Tomlyn)

また、フレームワークに[.NET](https://dotnet.microsoft.com/)を使用しています。

ライセンスは「ThirdPartyLicenses」に追加されています。


## FFmpegについて
`TJAPlayer3-f`と同じフォルダに`FFmpeg`フォルダを作成し、  
その中にOSとTJAPlayer3-fのアーキテクチャに対応したフォルダを作成し、
`TJAPlayer3-f`のアーキテクチャに対応したFFmpeg 7.1バイナリ(Shared)を置くことにより、

+ FFmpegが対応している動画ファイルの再生
+ FFmpegが対応している音声ファイルの再生

ができるようになります。

### OSとTJAPlayer3-fのアーキテクチャ数に対応したフォルダ名

+ Windows
  - x86   : `win-x86`
  - x64   : `win-x64`
+ macOS
  - x64   : `osx-x64`
  - arm64 : `osx-arm64`
+ Linux
  - x64   : `linux-x64`
  - arm64 : `linux-arm64`


## BASSについて
このリポジトリにはあらかじめBASSライブラリが同梱されています。

## 謝辞
このTJAPlayer3-fのもととなるソフトウェアを作成・メンテナンスしてきた中でも  
主要な方々に感謝の意を表し、お名前を上げさせていただきたいと思います。

- ＦＲＯＭ様
- yyagi様
- kairera0467様
- AioiLight様

また、他のTJAPlayer関係のソースコードを参考にさせていただいている箇所がございます。  
ありがとうございます。
