README
============================================================

Unityから「[EM-uNetPi](https://github.com/KONAMI/EM-uNetPi)」を制御するためのEditor拡張。

導入方法
------------------------------------------------------------

Assets以下の適当なフォルダにぶち込めばOK。

[EditorUtils](https://github.com/karinharp/EditorUtils)に依存しているので、こちらを先に導入しておくこと。

> ReleaseTabにおいてあるPackageには同梱してるので、そちらを使うなら一括導入OK

使い方
------------------------------------------------------------

制御ポートからコントロールした方がいいので、適当にNICを追加して、「192.168.31.*」を設定しておくと吉。

定義ファイルは、CreateAssets > TNP > Setting で生成（ScriptableObject）。

設定値を定義して、ScriptableObjectのInspectorから設定してもよし、

```
am.TNPSetting.Apply();
```

でスクリプトから呼んでもよし。


