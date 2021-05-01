# PolyominoSolver
It's a program for solving two-dimensional polyomino puzzle. Sorry but its UI is only in Japanese.

同じ大きさの正方形が複数繋がって構成される「ポリオミノ」で特定の盤面を埋めるパズル「ポリオミノパズル」のソルバープログラムです。  
正方形以外の形状が繋がっている種類のポリオミノには対応していません。

開発言語はVB.NET
動作環境はWindows OSかつ.NET Framework 4.6以上です。  
動作検証はWindows 10 64bit バージョン20H2で行っています。

以下の機能を持ちます。   
・盤面およびポリオミノをGUI操作で設定する機能（サイズはコンフィグファイルの編集により任意に可変）  
・同一形状のポリオミノを自動判別する機能（解答探索・および解答表示にて利用）  
・解答探索方法を、ポリオミノの回転・反転あり、回転のみ可、回転・反転とも不可、の3つから選択する機能  
・ポリオミノの埋め方を全通り探索する機能（マルチスレッド/シングルスレッド選択式）   
・解答をポリオミノごとに色分けおよび境界線を引くことで表示する機能   
・盤面およびポリオミノの設定をファイルに保存およびそのファイルから読込する機能
