README
============================================================

Unityから「[EM-uNetPi](https://github.com/KONAMI/EM-uNetPi)」を制御するためのEditor拡張。

Unity2017.3以降、Scripting Runtime Version : .NET4.x で動作を確認している。

導入方法
------------------------------------------------------------

Assets以下の適当なフォルダにぶち込めばOK。

なお、以下のパッケージに依存しているので、先に導入しておくこと。

- [karinharp/EditorUtils](https://github.com/karinharp/EditorUtils) => [UnityPackage](https://github.com/karinharp/EditorUtils/releases)
- [neuecc/Utf8Json](https://github.com/neuecc/Utf8Json) => [UnityPackage](https://github.com/neuecc/Utf8Json/releases)

使い方
------------------------------------------------------------

制御ポートからコントロールした方がいいので、適当にNICを追加して「192.168.31.*」を設定しておき、制御ポートに接続しておくと吉。

定義ファイルは、CreateAssets > TNP > Setting で生成（ScriptableObject）。

設定値を定義して、ScriptableObjectのInspectorから設定してもよし、

```
am.ENPSetting.Apply();
```

でスクリプトから呼んでもよし。

![Inspector](https://user-images.githubusercontent.com/1039507/46513995-3551c100-c896-11e8-98a0-4b65451b99bf.png)

