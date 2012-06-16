Lost-In-Trek
============

PssProgram
-------------------------------
6.8 23:31 篤良
1.Debug类的FPS算法更新
-------------------------------
6.9 4:11 篤良
1.Debug类更新，增加了屏幕输出指令——WriteLine方法以及三个重载
2.MainMenu类更新，修正Bug以及做了Debug输出的单次信息和持续信息两个例子（有专门备注）。测试方法为按下几个按钮以及持续按住“X”键（PC上是S键）。
3.Debug信息在Release模式下不显示
-------------------------------
6.9 10:22 大神
1.修改 SpriteX类， 加入 NowDirection—— 角色面对方向。
2.在 Helper文件夹  里 加入Maths类——各种静态数学方法。
3.加入Battles文件夹，用于放战斗有关的类，里面加的类有：BattleScene，BattleMap，BEnemy，BPalyer，BWarship. 这些类都还没完成，类的具体作用在注释里。
4.在Content//Pic 里加入Battles文件夹，用于放战斗有关的图片。
5.加入 摄像机类 LITCamera。
6.修改mainMenu 为BattleScene，显示一个追踪演示。
-------------------------------
6.9 11:23 篤良
1.修改了Debug类，使Debug信息在所有模式下显示
-----------
6.9 20:27 大神
1.Battles 文件夹 移动到了GameBace项目，并修改里面的类的命名空间为PSVX.Battles
2.修改Maths的命名空间为 PSVX.Base
3.修正Debug类 的 186行超出索引的异常。
-------------------------------
6.10 2:26 篤良
1.更新了Debug和Progream项，显示CPU占用率
2.增加了GameUI类，管控所有UI
3.增加了AVGMode文件夹及其相关类，命名空间PSVX.Base.AVG还在制作中
3.更新了GameBase类、Lit.Game类，增加了更换场景的方法。
4.修改了GameScene,使得其循环方法一开始不注册，而在Game调用的时候才注册以节约效能。
5.修改了MainMenu及图片,新增了TestMenu以示例更换场景，请参考NewGame按钮的方法（注意更换的时候要反注册场景循环方法，不过估计你只要一个场景就够了）
-------------------------------
6.10 4:07 篤良
1.更新了SpriteX的几处小BUG，比如后几个构造方法没有设置完整目录和设置了帧数限制等
2.更新了GameScene和GameBase类，在GameScene增加了一个Start方法，使得场景可以先载入，但只有更换场景时才会执行Start方法。
----------
6.10 18:10 大神
1.删除LITCamera类
2.修改GameScene，增加 MoveCamera();
3.更新Battles，增加主角控制场景摄像机,主角由摇杆控制，电脑上用鼠标模拟摇杆，加入PSV/PC模式，两个模式只是角色速度不同，完善BattleMap类。
4.修改TouchManager类，让触摸数据可以传到外部，从此获取触摸数据不会掉FPS了。
5.篤良 修正界面卡死BUG，修正Debug，Program类，FPS固定在55～60！
---------------
21:27 2012/6/10 篤良
1.更新了Game和TouchManager，已解决卡帧问题。
--------------------------
8:22 2012/6/12 篤良
1.更新Game、MainMenu、GameBase、GameScene以解决场景更换问题。
<<<<<<< .mine
2.将GameDate类替换为GameData并拿出来，卧槽我英文真烂，老是写错字

sdhkfgsakdjfgjklashflashfuifwqdf-=============================
2.将GameDate类替换为GameData并拿出来，卧槽我英文真烂，老是写错字
-------------------------
这个文件可以废了>>>>>>> .r9
