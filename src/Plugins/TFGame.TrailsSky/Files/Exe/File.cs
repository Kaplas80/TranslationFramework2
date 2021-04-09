using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using AsmResolver;
using TF.Core.Entities;
using TF.Core.Files;
using TF.Core.Helpers;
using TF.Core.TranslationEntities;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace TFGame.TrailsSky.Files.Exe
{
    public class File : BinaryTextFile
    {
        private CharacterInfo[] _charWidths;
        private FontTableView _ftView;

        protected override int ChangesFileVersion => 4;

        protected virtual long FontTableOffset => 0x15F140;

        protected virtual List<Tuple<int, byte[]>> Patches => new List<Tuple<int, byte[]>>
        {
            new Tuple<int, byte[]>(0x07AD92, new byte[] {0xEB, 0x4C}),
            new Tuple<int, byte[]>(0x07AF76, new byte[] {0x3C, 0xE0}),
        };

        protected virtual Dictionary<int, List<int>> StringOffsets => new Dictionary<int, List<int>>()
        {
            {0x0055d1d4, new List<int>() {0x004021af, }}, // Surprise attack!
            {0x0055d1e8, new List<int>() {0x004021b6, }}, // 背後をとられた！
            {0x0055d1fc, new List<int>() {0x0040224f, }}, // Preemptive attack!
            {0x0055d210, new List<int>() {0x00402256, }}, // 先制攻撃！
            {0x0055d21c, new List<int>() {0x00403248, }}, // Battle Entry Failed\n.
            {0x0055d234, new List<int>() {0x00404334, }}, // Move
            {0x0055d23c, new List<int>() {0x0040433b, }}, //    移  動   
            {0x0055d24c, new List<int>() {0x004043f7, }}, // Attack
            {0x0055d254, new List<int>() {0x004043fe, }}, // 　 攻　撃 　
            {0x0055d264, new List<int>() {0x00406e91, }}, // ATT GUARD
            {0x0055d270, new List<int>() {0x00406f87, }}, // CRITICAL
            {0x0055d27c, new List<int>() {0x0043bb95, }}, // Party was defeated...
            {0x0055d294, new List<int>() {0x0043bb9c, }}, // パーティは全滅しました・・・
            {0x0055d2b4, new List<int>() {0x0043bb75, }}, // Enemy fled...
            {0x0055d2c4, new List<int>() {0x0043bb7c, }}, // 敵に逃げられました・・・
            {0x0055d2e0, new List<int>() {0x004084ef, }}, // Party fled...
            {0x0055d2f0, new List<int>() {0x004084f6, }}, // パーティは退却しました・・・
            {0x0055d310, new List<int>() {0x0042c480, 0x0042c3ff, 0x0040963f, }}, // Arts
            {0x0055d318, new List<int>() {0x0040964c, }}, //    アーツ   
            {0x0055d328, new List<int>() {0x0047311b, 0x00409657, }}, // Crafts
            {0x0055d330, new List<int>() {0x00409664, }}, //   クラフト  
            {0x0055d340, new List<int>() {0x00409670, 0x004dc634, }}, // Items
            {0x0055d348, new List<int>() {0x0040967d, }}, //    道  具   
            {0x0055d358, new List<int>() {0x0040b5bf, }}, // Set %c%c%s%c%c as S-Break?
            {0x0055d374, new List<int>() {0x0040b5c6, }}, // %c%c%s%c%cをＳブレイク用に設定しますか？
            {0x0055d3a0, new List<int>() {0x0040b5f5, }}, // Tactics
            {0x0055d3a8, new List<int>() {0x0040b61c, }}, // Set %c%c%s%c%c as S-Break.\n\t\t\t\t\t\t\tSelect [%c%c%s%c%c] in main menu to change S-Break.
            {0x0055d400, new List<int>() {0x0040b638, }}, // %c%c%s%c%cをＳブレイク用に設定しました。\n\t\t\t\t\t\t\t\tキャンプ画面の[%c%c%s%c%c]で変更出来ます。
            {0x0055d468, new List<int>() {0x0041038a, }}, // %s is preparing to use arts.\n
            {0x0055d488, new List<int>() {0x0041039d, }}, // %sはアーツを使う準備をしている。\n
            {0x0055d4ac, new List<int>() {0x004112d7, }}, // %s fled.
            {0x0055d4b8, new List<int>() {0x004112e3, }}, // %sは逃げ出した。
            {0x0055d668, new List<int>() {0x00416693, 0x00422fb9, }}, // RESIST
            {0x0055d670, new List<int>() {0x00416ad3, 0x00416d55, }}, // DEF DOWN
            {0x0055d67c, new List<int>() {0x00416c76, }}, // MOV UP
            {0x0055d684, new List<int>() {0x00416c4d, }}, // ARTS GUARD
            {0x0055d690, new List<int>() {0x00416c23, }}, // CRAFT GUARD
            {0x0055d69c, new List<int>() {0x00416d0b, }}, // STR DOWN
            {0x0055d6a8, new List<int>() {0x00416d36, }}, // STR UP
            {0x0055d6b0, new List<int>() {0x00416cde, }}, // DEF UP
            {0x0055d6b8, new List<int>() {0x00416de3, }}, // SPD UP
            {0x0055d6c0, new List<int>() {0x00416dbd, }}, // SPD DOWN
            {0x0055d6cc, new List<int>() {0x00416d97, }}, // DEX UP
            {0x0055d6d4, new List<int>() {0x00416e09, }}, // DEX DOWN
            {0x0055d6e0, new List<int>() {0x00416ec8, }}, // AGL UP
            {0x0055d6e8, new List<int>() {0x00416ea5, }}, // AGL DOWN
            {0x0055d6f4, new List<int>() {0x00416e5d, }}, // MAX GUARD
            {0x0055d764, new List<int>() {0x00422a57, 0x00422b66, 0x00422c51, }}, // GUARD
            {0x0055d76c, new List<int>() {0x00422fdd, }}, // AT DELAY
            {0x0055d778, new List<int>() {0x004234a2, }}, // AT ADVANCE
            {0x0055d784, new List<int>() {0x00423728, }}, // %s was destroyed!\n
            {0x0055d798, new List<int>() {0x00423734, }}, // アイテム「%s」を壊された！\n
            {0x0055d84c, new List<int>() {0x0042bf3b, }}, // Battle Order Bar:
            {0x0055d860, new List<int>() {0x0042bf43, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\tIndicates who attacks first.\n\t\t\t\t\t\t\t\t\tIt starts from the top and moves down.
            {0x0055d8c4, new List<int>() {0x0042bf4a, }}, // ＡＴ(Action Time)バー:
            {0x0055d8e0, new List<int>() {0x0042bf52, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\t行動順を表すバーです。\n\t\t\t\t\t\t\t\t\t上から順に行動が回ってきます。
            {0x0055d934, new List<int>() {0x0042bfdc, }}, // Attack:
            {0x0055d940, new List<int>() {0x0042bfe4, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\tAttack an enemy.\n\t\t\t\t\t\t\t\t\tYou may also use it to move, if you are\n\t\t\t\t\t\t\t\t\tusing a mouse and click an empty location.
            {0x0055d9cc, new List<int>() {0x0042bffb, }}, // 攻撃：
            {0x0055d9d8, new List<int>() {0x0042c003, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\t敵を攻撃します。\n\t\t\t\t\t\t\t\t\t敵のいない場所をクリックすると\n\t\t\t\t\t\t\t\t\tそこまで移動することができます（マウス操作時）。
            {0x0055da60, new List<int>() {0x0042c040, }}, // The highlighted area indicates the distance\n\t\t\t\t\t\t\t\t\ta character can move. Selecting a target in\n\t\t\t\t\t\t\t\t\tthis area will move the character to attack.
            {0x0055daf8, new List<int>() {0x0042c057, }}, // #0C表示される範囲は移動攻撃の届く距離です。\n\t\t\t\t\t\t\t\t\tこの範囲内にいる目標を選択すると\n\t\t\t\t\t\t\t\t\t自動的に移動して攻撃をしかけます。
            {0x0055db80, new List<int>() {0x0042c094, }}, // When an enemy is out of range,\n\t\t\t\t\t\t\t\t\t\tan     icon will appear on your cursor.\n\t\t\t\t\t\t\t\t\t\tSelecting an out of range target will\n\t\t\t\t\t\t\t\t\t\twill move the character as close to it as\n\t\t\t\t\t\t\t\t\t\tas possible, but no attack will be performed.
            {0x0055dc70, new List<int>() {0x0042c09b, }}, // #0Cカーソルを重ねたときに\n\t\t\t\t\t\t\t\t\t　　が表示される目標は攻撃範囲外です。\n\t\t\t\t\t\t\t\t\t選択すると最も接近できる地点まで\n\t\t\t\t\t\t\t\t\t移動しますが、攻撃はできません。
            {0x0055dd10, new List<int>() {0x0042c181, }}, // Battle Order Bonus:
            {0x0055dd28, new List<int>() {0x0042c189, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\tThese icons indicate the bonuses alloted\n\t\t\t\t\t\t\t\t\tto the battle order. If a bonus icon appears\n\t\t\t\t\t\t\t\t\tnext to a character's icon, they will receive\n\t\t\t\t\t\t\t\t\tthat bonus.
            {0x0055dde8, new List<int>() {0x0042c1a0, }}, // ＡＴボーナス(行動順ボーナス)：
            {0x0055de08, new List<int>() {0x0042c1a8, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\t　行動順についたボーナスを示すアイコンです。\n\t\t\t\t\t\t\t\t\t　これらのアイコンがあるときに\n\t\t\t\t\t\t\t\t\t　行動順が回ってくると各種のボーナスがつきます。
            {0x0055deb0, new List<int>() {0x0042c1e5, }}, //   : Heal HP,    : Sepith Up, etc.\n\t\t\t\t\t\t\t\t\tindicate the effects of each icon.
            {0x0055df00, new List<int>() {0x0042c1ec, }}, // #0C   ：ＨＰ回復、   ：セピスボーナスなど\n\t\t\t\t\t\t\t\t\tアイコンごとに特定の効果があります。
            {0x0055df58, new List<int>() {0x0042c35d, }}, // Arts:
            {0x0055df60, new List<int>() {0x0042c365, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\tArts are effective against foes which\n\t\t\t\t\t\t\t\t\tare difficult to hit with a weapon or those\n\t\t\t\t\t\t\t\t\ton which physical attacks have little effect.
            {0x0055e008, new List<int>() {0x0042c37c, }}, // アーツ：
            {0x0055e018, new List<int>() {0x0042c384, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\t武器による攻撃が当たりにくい敵や\n\t\t\t\t\t\t\t\t\t攻撃の効果が薄い敵にはアーツが有効です。
            {0x0055e080, new List<int>() {0x0042c3e4, 0x0047770d, 0x00477a3f, }}, // EP
            {0x0055e088, new List<int>() {0x0042c408, }}, // It takes time before %c%c%s%c%c can be cast.\n\t\t\t\t\t\t\t\t\tAlso, %c%c%s%c%c is consumed when arts are cast.
            {0x0055e0f0, new List<int>() {0x0042c4ad, 0x0042c41f, }}, // アーツ
            {0x0055e0f8, new List<int>() {0x0042c428, }}, // %c%c%s%c%cは発動までに時間（駆動時間）が掛かります。\n\t\t\t\t\t\t\t\t\tまた、使用すると%c%c%s%c%cを消費します。
            {0x0055e160, new List<int>() {0x0042c473, }}, // Element
            {0x0055e168, new List<int>() {0x0042c489, }}, // All %c%c%s%c%c have an %c%c%s%c%c. Determine the element\n\t\t\t\t\t\t\t\t\tmost effective against your foe and use it.
            {0x0055e1d8, new List<int>() {0x0042c4a0, }}, // 属性
            {0x0055e1e0, new List<int>() {0x0042c4b6, }}, // %c%c%s%c%cには%c%c%s%c%cがあります。\n\t\t\t\t\t\t\t\t\t相手に有効な属性を見極めて使いましょう。
            {0x0055e238, new List<int>() {0x0042c566, }}, // Crafts:
            {0x0055e240, new List<int>() {0x0042c56e, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\tCrafts are character-specific skills which not\n\t\t\t\t\t\t\t\t\tonly deal out damage, but also have a broad range\n\t\t\t\t\t\t\t\t\tof effects.
            {0x0055e2d4, new List<int>() {0x0042c585, }}, // クラフト：
            {0x0055e2e0, new List<int>() {0x0042c58d, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\tクラフトはキャラクターごとに固有の技です。\n\t\t\t\t\t\t\t\t\t攻撃だけでなく、様々な効果のものがあります。
            {0x0055e358, new List<int>() {0x0042c6b7, 0x0042c70a, 0x0042c5ed, 0x0042c5fa, 0x00477724, 0x00477a56, }}, // CP
            {0x0055e35c, new List<int>() {0x0042c615, }}, // crafts
            {0x0055e368, new List<int>() {0x0042c61e, }}, // Using %c%c%s%c%c consumes %c%c%s%c%c.\n\t\t\t\t\t\t\t\t\t%c%c%s%c%c is gradually gained by dealing out\n\t\t\t\t\t\t\t\t\tor receiving damage in battle.
            {0x0055e3f0, new List<int>() {0x0042c635, }}, // クラフト
            {0x0055e400, new List<int>() {0x0042c63e, }}, // %c%c%s%c%cを使うと%c%c%s%c%cを消費します。\n\t\t\t\t\t\t\t\t\t%c%c%s%c%cは攻撃したり、ダメージを受けたりすることで\n\t\t\t\t\t\t\t\t\t戦闘中に少しずつ溜まっていきます。
            {0x0055e498, new List<int>() {0x0042c777, 0x0042c6c4, 0x004731e1, }}, // S-Crafts
            {0x0055e4a4, new List<int>() {0x0042c6d1, }}, // S-Breaks:
            {0x0055e4b0, new List<int>() {0x0042c6d9, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\tThese are actions which allow %c%c%s%c%c to be\n\t\t\t\t\t\t\t\t\timmediately unleashed while ignoring the battle\n\t\t\t\t\t\t\t\t\torder once the %c%c%s%c%c gauge has reached 0.
            {0x0055e564, new List<int>() {0x0042c9e4, 0x0042c82e, 0x0042c7a4, 0x0042c6f0, }}, // Ｓブレイク
            {0x0055e570, new List<int>() {0x0042c797, 0x0042c6fd, }}, // Ｓクラフト
            {0x0055e57c, new List<int>() {0x0042c717, }}, // Ｓブレイク：
            {0x0055e590, new List<int>() {0x0042c71f, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\t%c%c%s%c%cが100以上溜まると、ＡＴバー（行動順）を\n\t\t\t\t\t\t\t\t\t無視して%c%c%s%c%cを使用することが出来ます。\n\t\t\t\t\t\t\t\t\tこれが%c%c%s%c%cです。
            {0x0055e62c, new List<int>() {0x0042c76a, }}, // S-Breaks
            {0x0055e638, new List<int>() {0x0042c780, }}, // %c%c%s%c%c which will be used as %c%c%s%c%c can be changed by\n\t\t\t\t\t\t\t\t\tgoing to [Tactics] and then [Set S-Break] within the\n\t\t\t\t\t\t\t\t\tmain menu.
            {0x0055e6c8, new List<int>() {0x0042c7ad, }}, // %c%c%s%c%cとして発動する%c%c%s%c%cの変更は\n\t\t\t\t\t\t\t\t\tキャンプ[Tactics]内の［Ｓブレイク登録］で行います。
            {0x0055e730, new List<int>() {0x0042c9c7, 0x0042c811, 0x00485206, }}, // S-Break
            {0x0055e738, new List<int>() {0x0042c9d4, 0x0042c81e, }}, // Break Button
            {0x0055e748, new List<int>() {0x0042c827, }}, // Press the        %c%c%s%c%c to unleash an %c%c%s%c%c.\n\t\t\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t\t\tAn S-Break cannot be unleashed\n\t\t\t\t\t\t\t\t\tunder the        condition.
            {0x0055e7d8, new List<int>() {0x0042c9f1, 0x0042c83b, }}, // ブレイクボタン
            {0x0055e7e8, new List<int>() {0x0042c844, }}, // %c%c%s%c%c　　　をクリックすると%c%c%s%c%cが発動します。\n\t\t\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t\t\t※ブレイクボタンが　　　になっている間は\n\t\t\t\t\t\t\t\t\t  Ｓブレイクを発動することはできません。
            {0x0055e890, new List<int>() {0x0042c9dd, }}, // Now, press the         %c%c%s%c%c and try unleashing an %c%c%s%c%c.\n\t\t\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t\t\tIf you are using a keyboard, you may use the 1-4 number\n\t\t\t\t\t\t\t\t\tkeys, or the arrow keys to select it before unleashing it.
            {0x0055e968, new List<int>() {0x0042c9fa, }}, // #0Cでは、実際に%c%c%s%c%c　　　をクリックし\n\t\t\t\t\t\t\t\t\t%c%c%s%c%cを発動してみましょう。\n\t\t\t\t\t\t\t\t\t※キーボード操作の場合は\n\t\t\t\t\t\t\t\t\t　パーティメンバーに対応した[1][2][3][4]キーを押すか\n\t\t\t\t\t\t\t\t\t　矢印キーの左右[←][→]で選択します。
            {0x0055ea50, new List<int>() {0x0042cb52, }}, // Protect all NPCs!
            {0x0055ea64, new List<int>() {0x0042cb69, }}, // #0CNPCを守れ！
            {0x0055ea74, new List<int>() {0x0042cba6, }}, // If an NPC's HP reaches 0, the game is over.
            {0x0055eaa0, new List<int>() {0x0042cbbd, }}, // #0CNPCのHPが0になるとゲームオーバーになります。
            {0x0055ead0, new List<int>() {0x00430f04, 0x00430f29, }}, // MISS
            {0x0055ead8, new List<int>() {0x00430ec8, 0x00430eeb, }}, // DEAD
            {0x0055eae0, new List<int>() {0x00430e88, 0x00430eaf, 0x0043614c, 0x0043616f, }}, // LEVEL UP
            {0x0055ef64, new List<int>() {0x00435491, }}, // Displays all items.
            {0x0055ef78, new List<int>() {0x0043549b, }}, // 全てのアイテムを表示します。
            {0x0055ef98, new List<int>() {0x004354be, }}, // Displays weapons.
            {0x0055efac, new List<int>() {0x004354c8, }}, // 武器を表示します。
            {0x0055efc0, new List<int>() {0x004354ec, }}, // Displays armor.
            {0x0055efd0, new List<int>() {0x004354f6, }}, // 防具とアクセサリーを表示します。
            {0x0055eff4, new List<int>() {0x0043551a, }}, // Displays medicine.
            {0x0055f008, new List<int>() {0x00435524, }}, // 薬と携帯食糧を表示します。
            {0x0055f024, new List<int>() {0x00435548, }}, // Displays quartz.
            {0x0055f038, new List<int>() {0x0043554f, }}, // クオーツを表示します。
            {0x0055f050, new List<int>() {0x00435570, }}, // Displays key items.
            {0x0055f064, new List<int>() {0x00435577, }}, // イベントアイテムを表示します。
            {0x0055f084, new List<int>() {0x00435598, }}, // Displays ingredients.
            {0x0055f09c, new List<int>() {0x0043559f, }}, // 食材を表示します。
            {0x0055f0b0, new List<int>() {0x004355b8, }}, // Displays books.
            {0x0055f0c0, new List<int>() {0x004355c5, }}, // 書物を表示します。
            {0x0055f0d4, new List<int>() {0x004355ee, }}, // Removes equipment.
            {0x0055f0e8, new List<int>() {0x00435600, }}, // 装備をはずします。
            {0x0055f0fc, new List<int>() {0x00435688, }}, // Quartz can be installed.
            {0x0055f118, new List<int>() {0x004356a3, }}, // クオーツを装着できます。
            {0x0055f134, new List<int>() {0x0043570d, }}, // Weapon can be equipped.
            {0x0055f14c, new List<int>() {0x00435714, }}, // 武器を装備できます。
            {0x0055f164, new List<int>() {0x0043572a, }}, // Clothing can be equipped.
            {0x0055f180, new List<int>() {0x00435731, }}, // 衣服を装備できます。
            {0x0055f198, new List<int>() {0x00435747, }}, // Footwear can be equipped.
            {0x0055f1b4, new List<int>() {0x0043574e, }}, // 靴を装備できます。
            {0x0055f1c8, new List<int>() {0x00435755, }}, // Accessory can be equipped.
            {0x0055f1e4, new List<int>() {0x00435762, }}, // 装飾品を装備できます。
            {0x0055f2fc, new List<int>() {0x00435d57, }}, // Lv
            {0x0055f300, new List<int>() {0x00435db4, }}, // ◆━━━━━━━━━━━━━━━━━━━━━◆
            {0x0055f330, new List<int>() {0x00435df3, }}, // Exp
            {0x0055f33c, new List<int>() {0x00579cfc, 0x00435e92, }}, // Next
            {0x0055f34c, new List<int>() {0x0043630f, }}, //   Item
            {0x0055f358, new List<int>() {0x0043632d, 0x00436449, }}, // ◆━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━◆
            {0x0055f3b8, new List<int>() {0x0043637e, }}, // Got 
            {0x0055f3c0, new List<int>() {0x0043639a, }}, // Got
            {0x0055f3c4, new List<int>() {0x004363f2, }}, //  を手に入れた
            {0x0055f3d4, new List<int>() {0x0043640d, }}, //   Craft
            {0x0055f3dc, new List<int>() {0x00436414, }}, //   S-Craft
            {0x0055f3e8, new List<int>() {0x00436421, }}, //   SCraft
            {0x0055f3f4, new List<int>() {0x00436487, }}, // %s learned 
            {0x0055f404, new List<int>() {0x0043654d, }}, // %sは 
            {0x0055f40c, new List<int>() {0x004365eb, }}, //  を習得した。
            {0x0055f41c, new List<int>() {0x00436875, 0x0049a31f, }}, //   Sepith
            {0x0055f428, new List<int>() {0x00436896, }}, // ◆━━━━━━━◆
            {0x0055f5a4, new List<int>() {0x0043af41, 0x0043b407, 0x0043b7d8, 0x0049e0ba, 0x0049e471, 0x0049e5ca, }}, // ◆━━━━━━━━━━━━━━◆
            {0x0055f5c8, new List<int>() {0x0043af62, }}, // HP:
            {0x0055f5e0, new List<int>() {0x0043afe4, }}, // Condition:
            {0x0055f5ec, new List<int>() {0x0043b0a1, 0x0043b6bb, 0x0048ca75, 0x0048c628, 0x0048c623, 0x0048ca70, }}, // なし
            {0x0055f5f4, new List<int>() {0x0043b0bc, 0x0043b6d6, }}, // None
            {0x0055f5fc, new List<int>() {0x0043b130, }}, // Elemental Efficacy (%):
            {0x0055f614, new List<int>() {0x0043b137, }}, // 属性攻撃有効率(%):
            {0x0055f62c, new List<int>() {0x0043b339, }}, // CP:
            {0x0055f638, new List<int>() {0x0043b466, }}, // Exp:
            {0x0055f648, new List<int>() {0x0043b540, }}, // Sepith:
            {0x0055f654, new List<int>() {0x0043b64e, }}, // Item:
            {0x0055f77c, new List<int>() {0x005d96ac, }}, // Title Screen
            {0x0055f78c, new List<int>() {0x005d96a8, }}, // Retry
            {0x0055f794, new List<int>() {0x005d96a0, }}, // タイトルに戻る
            {0x0055f7a4, new List<int>() {0x005d96a0, }}, // リトライ
            {0x00560720, new List<int>() {0x004699e8, }}, // Save failed. Check your save folder.\nCurrent save folder path:\n%s
            {0x00560768, new List<int>() {0x00469a00, }}, // セーブに失敗しました。セーブフォルダを確認して下さい。\nセーブファイルのパスは以下の通り\n%s
            {0x005607cc, new List<int>() {0x0046b2f4, }}, // Learned recipe for [%s].
            {0x005607e8, new List<int>() {0x0046b2fb, 0x004be332, }}, // 『%s』のレシピを覚えた。
            {0x00560864, new List<int>() {0x00579c84, 0x005d99f8, }}, // Discard
            {0x0056086c, new List<int>() {0x00579c80, 0x005d99f4, }}, // Use
            {0x00560870, new List<int>() {0x005d99f0, }}, // 　捨てる　
            {0x0056087c, new List<int>() {0x005d99ec, }}, // 　使　う　
            {0x0056088c, new List<int>() {0x0046dba3, }}, // Select a save file.
            {0x005608a0, new List<int>() {0x0046dbbc, }}, // Select a file to load.
            {0x005608b8, new List<int>() {0x0046dbd5, }}, // Erase save data.
            {0x005608cc, new List<int>() {0x0046dbee, }}, // Exit game.
            {0x005608d8, new List<int>() {0x0046dc06, }}, // セーブするファイルを選んでください。
            {0x00560900, new List<int>() {0x0046dc1f, }}, // ロードするファイルを選んでください。
            {0x00560928, new List<int>() {0x0046dc38, }}, // 削除するファイルを選んでください。
            {0x0056094c, new List<int>() {0x0046dc51, }}, // ゲームを終了します。
            {0x00560964, new List<int>() {0x0046e80f, }}, // %cTactics mode cannot currently be utilized.
            {0x00560994, new List<int>() {0x0046f993, }}, // %cPosition cannot currently be changed.
            {0x005609f4, new List<int>() {0x004704d8, }}, // %cAuto Save slot. Slot cannot be used.
            {0x00560a1c, new List<int>() {0x00470512, }}, // Overwrite?
            {0x00560a28, new List<int>() {0x004703e7, }}, // Load clear\ndata file?
            {0x00560a40, new List<int>() {0x00470431, }}, // Load file?
            {0x00560a4c, new List<int>() {0x00470364, }}, // Delete this file?
            {0x00560a60, new List<int>() {0x004710ec, }}, // Select who will use the item.
            {0x00560a80, new List<int>() {0x004710f3, }}, // アイテムを使う相手を選んでください。
            {0x00560aa8, new List<int>() {0x004711dc, }}, // Discard this item?
            {0x00560abc, new List<int>() {0x00471249, }}, // このアイテムを捨てますか？
            {0x00560ad8, new List<int>() {0x004712dc, 0x00471377, }}, // Select on whom the arts will be used.
            {0x00560b00, new List<int>() {0x004712e3, 0x0047137e, }}, // アーツを使う相手を選んでください。
            {0x00560b24, new List<int>() {0x00473163, 0x0049e8d7, }}, // ◆━━━━
            {0x00560b30, new List<int>() {0x0047319c, 0x0049e900, }}, // ━━━━◆
            {0x00560b3c, new List<int>() {0x0049df48, 0x00473221, 0x00477467, 0x00477638, 0x00478b38, 0x0049ec12, 0x00492645, }}, // ◆━━━
            {0x00560b48, new List<int>() {0x0049df8e, 0x0047325c, 0x00477489, 0x0047765a, 0x00478b5a, 0x0049ec38, 0x00492667, }}, // ━━━◆
            {0x00560b54, new List<int>() {0x0047329b, }}, //       Crafts
            {0x00560b64, new List<int>() {0x004732b9, 0x00478bf0, }}, // ◆━━━            ━━━━◆
            {0x00560b84, new List<int>() {0x004732da, }}, //      S-Crafts
            {0x00560b94, new List<int>() {0x004732f8, }}, // ◆━━                ━━━◆
            {0x00561258, new List<int>() {0x004766ed, }}, // HP      /%4d
            {0x00561270, new List<int>() {0x00476750, }}, // EP  %4d/%4d
            {0x00561320, new List<int>() {0x004773da, }}, // Name
            {0x00561328, new List<int>() {0x004775ab, 0x004dc7bd, }}, // Status
            {0x00561330, new List<int>() {0x004776e1, 0x00477a13, 0x0048172b, }}, // Level
            {0x00561338, new List<int>() {0x004776f6, 0x00477a28, 0x004a218a, }}, // HP
            {0x00561364, new List<int>() {0x004777f3, }}, // Parameters
            {0x00561370, new List<int>() {0x00477880, 0x004782aa, 0x00492808, 0x00492b21, }}, // ◆━━
            {0x00561378, new List<int>() {0x004778a2, 0x004782d8, 0x0049282a, 0x00492b43, }}, // ━━◆
            {0x00561380, new List<int>() {0x0047792b, }}, //       Name
            {0x0056138c, new List<int>() {0x00477949, }}, // ◆━━━        ━━━◆
            {0x005613a8, new List<int>() {0x004779d4, }}, //      Status
            {0x005613b4, new List<int>() {0x004779f2, 0x00477b52, 0x004783fb, 0x00492a2a, }}, // ◆━                ━◆
            {0x005613d0, new List<int>() {0x00477b32, }}, //    Parameter
            {0x005613e0, new List<int>() {0x00477d47, 0x0049e10d, }}, // STR
            {0x005613e4, new List<int>() {0x00477d5f, 0x0049e126, }}, // DEF
            {0x005613e8, new List<int>() {0x00477d79, 0x0049e13f, }}, // ATS
            {0x005613ec, new List<int>() {0x00477d93, 0x0049e158, }}, // ADF
            {0x005613f0, new List<int>() {0x00477dad, 0x0049e171, }}, // SPD
            {0x005613f4, new List<int>() {0x00477dc7, 0x0049e187, }}, // DEX
            {0x005613f8, new List<int>() {0x00477de1, 0x0049e19d, }}, // AGL
            {0x005613fc, new List<int>() {0x00477dfb, 0x0049e1b6, }}, // MOV
            {0x00561400, new List<int>() {0x00477e15, 0x0049e1cf, }}, // RNG
            {0x00561414, new List<int>() {0x004781d8, }}, // Experience
            {0x00561420, new List<int>() {0x00478393, }}, //    Experience
            {0x00561430, new List<int>() {0x00478457, 0x004784d3, }}, // EXP
            {0x00561434, new List<int>() {0x00478477, 0x004784f3, }}, // NEXT
            {0x005614bc, new List<int>() {0x00478aab, 0x004dc6b7, }}, // Equipment
            {0x005614c8, new List<int>() {0x00478bd2, }}, //       Equips
            {0x005614d8, new List<int>() {0x00479669, }}, // Unequip
            {0x005615e4, new List<int>() {0x0047f5a3, }}, // Autosave
            {0x005615f0, new List<int>() {0x0047f5bc, }}, // Page %2d
            {0x005615fc, new List<int>() {0x0047f7d5, }}, // ◆━━━━━━━━━━━━━━━━━━━━━━━━━━━━━◆
            {0x005616ac, new List<int>() {0x0047f94b, 0x0047f93f, 0x00579c38, }}, // Clear Data - Trails in the Sky
            {0x005616cc, new List<int>() {0x0047f94b, 0x0047f946, 0x00579c20, }}, // クリアデータ 空の軌跡ＦＣ
            {0x005616f4, new List<int>() {0x00481836, 0x0047f968, 0x004dc8d1, 0x004dc8cc, }}, // ノーマル
            {0x00561700, new List<int>() {0x0048183e, 0x0047f970, 0x004dc8b7, 0x004dc8b2, }}, // ハード
            {0x00561708, new List<int>() {0x00481846, 0x0047f978, 0x004dc894, 0x004dc88f, }}, // ナイトメア
            {0x00561714, new List<int>() {0x0048184e, 0x0047f980, 0x004dc8d1, 0x004dc876, }}, // イージー
            {0x00561720, new List<int>() {0x00481856, 0x0047f988, 0x004dc8d1, 0x004dc8bf, }}, // Normal
            {0x00561728, new List<int>() {0x0048185e, 0x0047f990, 0x004dc8b7, 0x004dc8a5, }}, // Hard
            {0x00561730, new List<int>() {0x00481866, 0x0047f998, 0x004dc894, 0x004dc888, }}, // Nightmare
            {0x0056173c, new List<int>() {0x0048186e, 0x0047f9a0, 0x004dc8d1, 0x004dc86f, }}, // Easy
            {0x00561744, new List<int>() {0x0047f9bd, 0x0047f9b1, }}, // Unknown
            {0x00561778, new List<int>() {0x00481440, }}, // ◆━━━━━━━━━━━━━━━━━━━━━━━━━━━━◆
            {0x005617b8, new List<int>() {0x00481479, }}, // SaveTime: 
            {0x005617c4, new List<int>() {0x00481480, }}, // SaveTime：
            {0x005617d0, new List<int>() {0x004814ab, }}, // PlayTime: 
            {0x005617dc, new List<int>() {0x004814b2, }}, // PlayTime：
            {0x005617e8, new List<int>() {0x004818b4, }}, // Unknown:
            {0x0056186c, new List<int>() {0x00481ff6, }}, // Are you sure\nyou wish to finish\nsaving Clear Data?
            {0x005618a0, new List<int>() {0x0048210f, }}, // %cAuto save slot.\nYou can't save here.
            {0x005618c8, new List<int>() {0x00482133, }}, // Overwrite file?
            {0x00561a84, new List<int>() {0x00482696, }}, // OK
            {0x00561a88, new List<int>() {0x004826b9, }}, // CANCEL
            {0x00561b34, new List<int>() {0x0057a7f4, }}, // Set S-Break
            {0x00561b40, new List<int>() {0x0057a7f0, }}, // Battle Position
            {0x00561b50, new List<int>() {0x0057a7ec, }}, // Character Position
            {0x00561b64, new List<int>() {0x0057a7e8, }}, // Ｓブレイク登録
            {0x00561b74, new List<int>() {0x0057a7e4, }}, // 戦闘隊列変更
            {0x00561b84, new List<int>() {0x0057a7e0, }}, // 移動隊列変更
            {0x00561b94, new List<int>() {0x0048441f, }}, // Change character position.
            {0x00561bb0, new List<int>() {0x00484433, }}, // 移動時の隊列を変更します。
            {0x00561bcc, new List<int>() {0x004843f8, }}, // Change battle formation.
            {0x00561be8, new List<int>() {0x00484407, }}, // 戦闘用の隊列を変更します。
            {0x00561c04, new List<int>() {0x004843cd, }}, // Set S-Break.
            {0x00561c14, new List<int>() {0x004843dc, }}, // Ｓブレイクの登録をします。
            {0x00561c30, new List<int>() {0x00484737, }}, // ◆━━━━━
            {0x00561c40, new List<int>() {0x0048475d, }}, // ━━━━━◆
            {0x00561c50, new List<int>() {0x004847db, }}, // Party Order
            {0x00561c5c, new List<int>() {0x00484865, }}, // ◆━━━━                   ━━━━◆
            {0x00561c84, new List<int>() {0x00484886, }}, //         Walk Place
            {0x00561c98, new List<int>() {0x00485b71, 0x00485162, }}, // ◆━━━━━━━━━━━
            {0x00561cb4, new List<int>() {0x00485b97, 0x00485188, }}, // ━━━━━━━━━━━◆
            {0x00561cd0, new List<int>() {0x00485290, 0x00485ca9, }}, // ◆━━━━━━━━━                   ━━━━━━━━━◆
            {0x00561d0c, new List<int>() {0x004852b1, }}, //                 S-Break
            {0x00561d40, new List<int>() {0x00485c15, }}, // Formation
            {0x00561d4c, new List<int>() {0x00485cca, }}, //                Formation
            {0x00562004, new List<int>() {0x0057a7c0, }}, // Cafe
            {0x0056200c, new List<int>() {0x0057a7bc, }}, // Septian Church
            {0x0056201c, new List<int>() {0x0057a7b8, }}, // Restaurant/Inn
            {0x0056202c, new List<int>() {0x0057a7b4, }}, // Orbment Factory
            {0x0056203c, new List<int>() {0x0057a7b0, }}, // Bracer Guild
            {0x0056204c, new List<int>() {0x0057a7ac, }}, // Lodgings
            {0x00562058, new List<int>() {0x0057a7a8, }}, // General Goods
            {0x00562068, new List<int>() {0x0057a7a4, }}, // Arms & Guards
            {0x00562080, new List<int>() {0x0057a780, }}, // 飲食・喫茶
            {0x0056208c, new List<int>() {0x0057a77c, }}, // 七耀教会
            {0x00562098, new List<int>() {0x0057a778, }}, // 食事・休憩
            {0x005620a4, new List<int>() {0x0057a774, }}, // オーブメント
            {0x005620b4, new List<int>() {0x0057a770, }}, // 遊撃士協会
            {0x005620c0, new List<int>() {0x0057a76c, }}, // 休憩・宿泊
            {0x005620cc, new List<int>() {0x0057a768, }}, // 薬・雑貨・食材
            {0x005620dc, new List<int>() {0x0057a764, }}, // 武器・防具
            {0x005620e8, new List<int>() {0x00489820, 0x0048981b, }}, // Julia
            {0x005620f0, new List<int>() {0x00489849, }}, // Captain Schwarz
            {0x00562100, new List<int>() {0x00489873, 0x0048986e, }}, // Mueller
            {0x00562108, new List<int>() {0x0048989c, }}, // Major Vander
            {0x005621d4, new List<int>() {0x0048c406, }}, // ■Status
            {0x005621e0, new List<int>() {0x0048c40d, }}, // ■ステータス
            {0x005621f0, new List<int>() {0x0048c441, 0x0048c435, }}, // Visible
            {0x005621f8, new List<int>() {0x0048c441, 0x0048c43c, }}, // 表示する
            {0x00562204, new List<int>() {0x0048c500, 0x0048c45b, 0x0048c44f, 0x0048c4ee, }}, // Invisible
            {0x00562210, new List<int>() {0x0048c500, 0x0048c45b, 0x0048c456, 0x0048c4fb, }}, // 表示しない
            {0x0056221c, new List<int>() {0x0048c489, }}, // ■Mini Map
            {0x00562228, new List<int>() {0x0048c490, }}, // ■ミニマップ
            {0x00562238, new List<int>() {0x0048c4c7, 0x0048c4b5, }}, // Fixed north
            {0x00562244, new List<int>() {0x0048c4c7, 0x0048c4c2, }}, // 北を上で固定
            {0x00562254, new List<int>() {0x0048c4e6, 0x0048c4d4, }}, // Rotate
            {0x0056225c, new List<int>() {0x0048c4e6, 0x0048c4e1, }}, // 画面回転と同期
            {0x0056226c, new List<int>() {0x0048c52e, }}, // ■Camera
            {0x00562278, new List<int>() {0x0048c535, }}, // ■カメラ回転
            {0x00562288, new List<int>() {0x0048c568, 0x0048c55c, }}, // 45 degree
            {0x00562294, new List<int>() {0x0048c568, 0x0048c563, }}, // 45度単位
            {0x005622a0, new List<int>() {0x0048c583, 0x0048c577, }}, // Free rotation
            {0x005622b0, new List<int>() {0x0048c583, 0x0048c57e, }}, // フリー回転
            {0x005622bc, new List<int>() {0x0048c5b1, }}, // ■Menu Button
            {0x005622cc, new List<int>() {0x0048c5b8, }}, // ■ボタン機能
            {0x005622dc, new List<int>() {0x0048c5ef, 0x0048c5dd, }}, // Click
            {0x005622e4, new List<int>() {0x0048c5ef, 0x0048c5ea, }}, // クリック
            {0x005622f0, new List<int>() {0x0048c60e, 0x0048c5fc, }}, // Double click
            {0x00562300, new List<int>() {0x0048c60e, 0x0048c609, }}, // ダブルクリック
            {0x00562310, new List<int>() {0x0048cb5b, 0x0048ca75, 0x0048cad2, 0x0048c9bb, 0x0048c925, 0x0048c90a, 0x0048c783, 0x0048c628, 0x0048c616, 0x0048c777, 0x0048c81d, 0x0048c8fe, 0x0048c9af, 0x0048ca69, 0x0048cac6, 0x0048cb4f, }}, // Off
            {0x00562318, new List<int>() {0x004dc90c, 0x0048c663, 0x0048c7c4, }}, // ◆━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━◆
            {0x0056235c, new List<int>() {0x0048c680, }}, // ■Frame Rate
            {0x0056236c, new List<int>() {0x0048c687, }}, // ■フレームレート
            {0x00562380, new List<int>() {0x0048c6bb, 0x0048c6af, }}, // 60 FPS Limit
            {0x00562390, new List<int>() {0x0048c6bb, 0x0048c6b6, }}, // 60FPS固定
            {0x0056239c, new List<int>() {0x0048c6de, 0x0048c6cc, }}, // 30 FPS Limit
            {0x005623ac, new List<int>() {0x0048c6de, 0x0048c6d9, }}, // 30FPS固定
            {0x005623b8, new List<int>() {0x0048c6f8, 0x0048c6e6, }}, // Automatic
            {0x005623c4, new List<int>() {0x0048c6f8, 0x0048c6f3, }}, // 自動
            {0x005623cc, new List<int>() {0x0048c729, }}, // ■Transparency
            {0x005623dc, new List<int>() {0x0048c730, }}, // ■キャラ透過
            {0x005623ec, new List<int>() {0x0048cb75, 0x0048caed, 0x0048c9d6, 0x0048c925, 0x0048c769, 0x0048c75d, 0x0048c919, 0x0048c9ca, 0x0048cae1, 0x0048cb69, }}, // On
            {0x005623f0, new List<int>() {0x0048c769, 0x0048c764, }}, // 透過する
            {0x005623fc, new List<int>() {0x0048c783, 0x0048c77e, }}, // 透過しない
            {0x00562408, new List<int>() {0x0048c7e1, }}, // ■Background Music
            {0x0056241c, new List<int>() {0x0048c7e8, }}, // ■ＢＧＭ
            {0x00562428, new List<int>() {0x0048c925, 0x0048c9bb, 0x0048cad2, 0x0048c90a, 0x0048c828, 0x0048c905, 0x0048c9b6, 0x0048cacd, }}, // オフ
            {0x00562430, new List<int>() {0x0048c84e, 0x0048c842, }}, // Original
            {0x0056243c, new List<int>() {0x0048c84e, 0x0048c849, }}, // オリジナル
            {0x00562478, new List<int>() {0x0048c8de, 0x0048c8ca, }}, // Unnamed Pack %d
            {0x00562488, new List<int>() {0x0048c8de, 0x0048c8d1, }}, // ＢＧＭセット%d
            {0x00562498, new List<int>() {0x0048caed, 0x0048c9d6, 0x0048c925, 0x0048c920, 0x0048c9d1, 0x0048cae8, }}, // オン
            {0x005624a0, new List<int>() {0x0048c956, }}, // 　BGM Volume
            {0x005624b0, new List<int>() {0x0048c95d, }}, // 　ＢＧＭ音量
            {0x005624c0, new List<int>() {0x0048c97e, }}, // ■Sound Effects
            {0x005624d0, new List<int>() {0x0048c985, }}, // ■効果音
            {0x005624dc, new List<int>() {0x0048ca07, }}, // 　SE Volume
            {0x005624e8, new List<int>() {0x0048ca0e, }}, // 　効果音音量
            {0x005624f8, new List<int>() {0x0048ca2f, }}, // ■Voices
            {0x00562504, new List<int>() {0x0048ca36, }}, // ■戦闘ボイス
            {0x00562514, new List<int>() {0x0048caed, 0x0048ca8d, }}, // Japanese
            {0x00562520, new List<int>() {0x0048caed, 0x0048ca94, }}, // 日本語
            {0x00562528, new List<int>() {0x0048caae, 0x0048caa2, }}, // English
            {0x00562530, new List<int>() {0x0048caae, 0x0048caa9, }}, // 英語
            {0x00562538, new List<int>() {0x0048cb1e, }}, // ■Retry Offset
            {0x00562548, new List<int>() {0x0048cb25, }}, // ■リトライ補正
            {0x00562558, new List<int>() {0x0048cb5b, 0x0048cb56, }}, // 補正しない
            {0x00562564, new List<int>() {0x0048cb75, 0x0048cb70, }}, // 補正する
            {0x00562570, new List<int>() {0x0048cba3, }}, // ■Orbment Style
            {0x00562580, new List<int>() {0x0048cbaa, }}, // ■導力器スタイル
            {0x00562594, new List<int>() {0x0048cbe9, 0x0048cbd7, }}, // Colored Lines
            {0x005625a4, new List<int>() {0x0048cbe9, 0x0048cbe4, }}, // カラーライン
            {0x005625b4, new List<int>() {0x0048cc07, 0x0048cbf5, }}, // Powered Lines
            {0x005625c4, new List<int>() {0x0048cc07, 0x0048cc02, }}, // グローライン
            {0x005625d4, new List<int>() {0x0048cc21, 0x0048cc0f, }}, // Classic
            {0x005625dc, new List<int>() {0x0048cc21, 0x0048cc1c, }}, // クラシック
            {0x005625e8, new List<int>() {0x0048d473, }}, // Change status display shown on the game screen.
            {0x00562618, new List<int>() {0x0048d488, }}, // ゲーム画面のステータス表示の切り替えをします。
            {0x00562648, new List<int>() {0x0048d4ab, }}, // Change mini map display shown on the game screen.
            {0x0056267c, new List<int>() {0x0048d4c0, }}, // ゲーム画面のミニマップ表示の切り替えをします。
            {0x005626ac, new List<int>() {0x0048d4e3, }}, // Change game camera settings.
            {0x005626cc, new List<int>() {0x0048d4f8, }}, // ゲーム画面の回転方式の切り替えをします。
            {0x005626f8, new List<int>() {0x0048d51b, }}, // Change main menu button.
            {0x00562714, new List<int>() {0x0048d530, }}, // キャンプボタンの切り替えをします。
            {0x00562738, new List<int>() {0x0048d553, }}, // Change frame rate.
            {0x0056274c, new List<int>() {0x0048d568, }}, // フレームレートの固定化を切り替えます。
            {0x00562774, new List<int>() {0x0048d58b, }}, // Change character transparency.
            {0x00562794, new List<int>() {0x0048d5a0, }}, // キャラの透過処理を切り替えます。
            {0x005627b8, new List<int>() {0x0048d5c7, }}, // Turn background music on or off.
            {0x005627e0, new List<int>() {0x0048d5df, }}, // Change BGM pack. Pack folders start from BGM1 onward.\nPack names can be set by adding a packname.txt file.
            {0x0056284c, new List<int>() {0x0048d637, }}, // Change background music pack.
            {0x0056286c, new List<int>() {0x0048d653, }}, // ＢＧＭのＯＮ／ＯＦＦを切り替えます。
            {0x00562894, new List<int>() {0x0048d676, }}, // Change background music volume.
            {0x005628b4, new List<int>() {0x0048d68b, }}, // ＢＧＭの音量を変更します。
            {0x005628d0, new List<int>() {0x0048d6ae, }}, // Turn sound effects on or off.
            {0x005628f0, new List<int>() {0x0048d6c3, }}, // 効果音のＯＮ／ＯＦＦを切り替えます。
            {0x00562918, new List<int>() {0x0048d6e6, }}, // Change sound effect volume.
            {0x00562934, new List<int>() {0x0048d6fb, }}, // 効果音の音量を変更します。
            {0x00562950, new List<int>() {0x0048d727, }}, // Change battle voice language.
            {0x00562970, new List<int>() {0x0048d73c, }}, // Turn battle voices on or off.
            {0x00562990, new List<int>() {0x0048d75a, }}, // 戦闘ボイス言語の切り替えをします。
            {0x005629b4, new List<int>() {0x0048d76f, }}, // 戦闘ボイスオンオフの切り替えをします。
            {0x005629e0, new List<int>() {0x0048d792, }}, // Enemies become weaker based on number of retries.\nFeel free to use this function if you can't win a battle.
            {0x00562a50, new List<int>() {0x0048d7a7, }}, // 戦闘をリトライした回数に応じて敵が弱体化します。\nどうしても勝てない時にどうぞ。
            {0x00562aa0, new List<int>() {0x0048d7ca, }}, // Change orbment line display style.
            {0x00562ac4, new List<int>() {0x0048d7df, }}, // 導力器ライン表示の切り替えをします。
            {0x00562aec, new List<int>() {0x0048fa22, }}, // ◆━━━━━━━━
            {0x00562b00, new List<int>() {0x0048fa48, }}, // ━━━━━━━━◆
            {0x00562b14, new List<int>() {0x0048fac6, }}, // Slots
            {0x00562b1c, new List<int>() {0x0048fb50, }}, // ◆━━━━━━              ━━━━━━◆
            {0x00562b48, new List<int>() {0x0048fb71, }}, //            Slots
            {0x00562b5c, new List<int>() {0x0048fc65, }}, // ◆━━━━━━
            {0x00562b6c, new List<int>() {0x0048fc91, }}, // ━━━━━━◆
            {0x00562b7c, new List<int>() {0x0048fd47, }}, // Orbal Arts
            {0x00562b88, new List<int>() {0x0048fe0d, }}, // ◆━━━━━                  ━━━━━◆
            {0x00562bb4, new List<int>() {0x0048fe6c, }}, //          Orbal Arts
            {0x00562bc8, new List<int>() {0x0048ff42, }}, // %3dEP
            {0x00562bd0, new List<int>() {0x00490f60, }}, // Remove quartz
            {0x00562be0, new List<int>() {0x00490f6d, }}, // クオーツをはずす
            {0x00562bf4, new List<int>() {0x00491bed, }}, // Line %d
            {0x00562d9c, new List<int>() {0x004925b8, 0x00492a94, }}, // Mira
            {0x00562da4, new List<int>() {0x0049277b, }}, // PlayTime
            {0x00562dc8, new List<int>() {0x00492992, }}, //      Mira  
            {0x00562dd4, new List<int>() {0x004929b0, }}, // ◆━━            ━━◆
            {0x00562df8, new List<int>() {0x00492a0c, }}, //    PlayTime
            {0x00562e10, new List<int>() {0x00492bda, }}, //     Mira  
            {0x00562e1c, new List<int>() {0x00492bf8, }}, // ◆━          ━◆
            {0x00562fcc, new List<int>() {0x005d9c58, }}, // [Nightmare] It's your funeral. May Aidios be with you.
            {0x00563008, new List<int>() {0x005d9c54, }}, // [Hard] A difficult setting. For those who find normal a bit lacking.
            {0x00563050, new List<int>() {0x005d9c50, }}, // [Normal] Average difficulty. For those wanting a little challenge.
            {0x00563098, new List<int>() {0x005d9c4c, }}, // 【ナイトメア】まさに悪夢。各種データを引き継がないと厳しいバランスです。
            {0x005630e4, new List<int>() {0x005d9c48, }}, // 【ハード】やや厳しいバランスです。ノーマルでは物足りない方に。
            {0x00563124, new List<int>() {0x005d9c44, }}, // 【ノーマル】標準的なバランスです。適度な刺激を楽しみたい方に。
            {0x00563238, new List<int>() {0x00497be6, }}, // 　 Bracer Guild\n　 Lodgings\n　 Orbal Factory\n　 Septian Church\n　 Various Shops
            {0x00563288, new List<int>() {0x00497bed, }}, // 　 遊撃士協会\n　 ホテル、宿酒場\n　 オーブメント工房\n　 七耀教会\n　 各種ショップ
            {0x00563434, new List<int>() {0x0057a648, }}, // Mirage sepith
            {0x00563444, new List<int>() {0x0057a644, }}, // Space sepith
            {0x00563454, new List<int>() {0x0057a640, 0x0057a64c, 0x0057a650, }}, // Time sepith
            {0x00563460, new List<int>() {0x0057a63c, }}, // Wind sepith
            {0x0056346c, new List<int>() {0x0057a638, }}, // Fire sepith
            {0x00563478, new List<int>() {0x0057a634, }}, // Water sepith
            {0x00563488, new List<int>() {0x0057a630, }}, // Earth sepith
            {0x00563498, new List<int>() {0x0057a624, }}, // 幻のセピス
            {0x005634a4, new List<int>() {0x0057a620, }}, // 空のセピス
            {0x005634b0, new List<int>() {0x0057a61c, 0x0057a628, 0x0057a62c, }}, // 時のセピス
            {0x005634bc, new List<int>() {0x0057a618, }}, // 風のセピス
            {0x005634c8, new List<int>() {0x0057a614, }}, // 火のセピス
            {0x005634d4, new List<int>() {0x0057a610, }}, // 水のセピス
            {0x005634e0, new List<int>() {0x0057a60c, }}, // 地のセピス
            {0x005634ec, new List<int>() {0x005d9d20, 0x005d9da0, 0x005d9dc4 }}, // Quit
            {0x005634f4, new List<int>() {0x005d9d1c, 0x005d9dc0, }}, // Trade
            {0x005634fc, new List<int>() {0x005d9d18, }}, // Quartz
            {0x00563504, new List<int>() {0x005d9d14, }}, // Slot
            {0x0056350c, new List<int>() {0x005d9d10, }}, // や め る
            {0x00563518, new List<int>() {0x005d9d0c, }}, // 換    金
            {0x00563524, new List<int>() {0x005d9d08, }}, // クオーツ
            {0x00563530, new List<int>() {0x005d9d04, }}, // スロット
            {0x0056353c, new List<int>() {0x004985e1, }}, // Open a slot.
            {0x0056354c, new List<int>() {0x004985f0, }}, // スロットの開封をします。
            {0x00563568, new List<int>() {0x004985ba, }}, // Synthesize quartz.
            {0x0056357c, new List<int>() {0x004985c9, }}, // クオーツの合成をします。
            {0x00563598, new List<int>() {0x00498594, }}, // Exchange sepith for mira.
            {0x005635b4, new List<int>() {0x004985a3, 0x0049ba14, }}, // セピスをお金に換金します。
            {0x005635d0, new List<int>() {0x0049856e, }}, // End transaction.
            {0x005635e4, new List<int>() {0x0049857d, 0x0049d36f, 0x0049ba6c, }}, // 何をしますか？
            {0x005635f4, new List<int>() {0x00498cb4, 0x00498cae, 0x00579c88, }}, // 開封済み
            {0x00563600, new List<int>() {0x00498d1a, 0x00498d14, 0x00579c8c, }}, // 開封可能
            {0x0056360c, new List<int>() {0x00498c05, 0x00498bff, 0x00579c90, }}, // 開封不可
            {0x00563618, new List<int>() {0x0046d206, 0x00579c94, }}, // 未開封
            {0x0056362c, new List<int>() {0x00498cb4, 0x00498c9c, }}, // Already Open
            {0x0056363c, new List<int>() {0x00498d1a, 0x00498d02, }}, // Openable
            {0x00563648, new List<int>() {0x00498c05, 0x00498bed, }}, // Not Openable
            {0x00563658, new List<int>() {0x004996e0, }}, // %cOpened slot [%d].
            {0x0056366c, new List<int>() {0x004996f4, }}, // %cスロット[ %d ]を開封しました。
            {0x00563690, new List<int>() {0x00499721, 0x00499934, }}, // %cInsufficient sepith.
            {0x005636a8, new List<int>() {0x0049972d, 0x00499940, }}, // %cセピスが足りません。
            {0x005636c0, new List<int>() {0x0049980e, }}, // %c#0CSynthesize #3C%s#0C?
            {0x005636dc, new List<int>() {0x00499815, }}, // %c#3C%s#0Cを合成します。よろしいですか？
            {0x00563708, new List<int>() {0x004998cf, }}, // %c#0CSynthesized #3C%s#0C.
            {0x00563724, new List<int>() {0x004998e8, }}, // %c#3C%s#0Cを合成しました。
            {0x00563740, new List<int>() {0x004999d1, }}, // %cTrade sepith for %d mira?
            {0x0056375c, new List<int>() {0x004999dd, }}, // %cセピスを%dミラに換金します。よろしいですか？
            {0x0056378c, new List<int>() {0x00499b17, }}, // %cTraded sepith for %d mira.
            {0x005637ac, new List<int>() {0x00499b23, }}, // %cセピスを%dミラに換金しました。
            {0x005637d0, new List<int>() {0x00499dde, }}, // Choose a slot to open.
            {0x005637e8, new List<int>() {0x00499dea, }}, // 開封したいスロットを選択してください。
            {0x00563810, new List<int>() {0x00499d4f, }}, // Slot [%d] can be opened.
            {0x00563830, new List<int>() {0x00499d5d, }}, // Slot [%d] cannot be opened because the circuit\nis not connected.
            {0x00563874, new List<int>() {0x00499d6b, }}, // Slot [%d] has already been opened.
            {0x00563898, new List<int>() {0x00499d81, }}, // スロット[ %d ]の開封を行えます。
            {0x005638c0, new List<int>() {0x00499d8f, }}, // スロット[ %d ]は動力回路が接続していないため、\n開封できません。
            {0x00563900, new List<int>() {0x00499d9d, }}, // スロット[ %d ]はすでに開封済みです。
            {0x00563928, new List<int>() {0x00499e55, }}, // Select a character.
            {0x0056393c, new List<int>() {0x00499e64, }}, // キャラクターを選択してください。
            {0x00563960, new List<int>() {0x00499ecc, 0x0049bddf, }}, // Inventory: ------
            {0x00563974, new List<int>() {0x00499ed3, }}, // 現在の所持数：------
            {0x0056398c, new List<int>() {0x00499ef8, }}, // Choose a quartz to synthesize.
            {0x005639ac, new List<int>() {0x00499f19, }}, // 合成したいクオーツを選択してください。
            {0x005639d4, new List<int>() {0x00499f58, 0x0049be77, }}, // Inventory: %4d
            {0x005639e4, new List<int>() {0x00499f68, 0x0049be87, }}, // 現在の所持数：%4d個
            {0x005639f8, new List<int>() {0x0049a013, }}, // Select the sepith to trade.
            {0x00563a14, new List<int>() {0x0049a01a, }}, // 換金したいセピスを選択してください。
            {0x00563a3c, new List<int>() {0x0049a03b, }}, // Trade %s for mira.
            {0x00563a50, new List<int>() {0x0049a05c, }}, // %sを、換金します。
            {0x00563a64, new List<int>() {0x0049a07a, }}, // Trade sepith for mira.
            {0x00563a7c, new List<int>() {0x0049a081, }}, // セピスの換金を行います。
            {0x00563a98, new List<int>() {0x0049a0f9, }}, // Trade %d sepith for %d mira.
            {0x00563ab8, new List<int>() {0x0049a100, }}, // %d個を%dミラに換金します。
            {0x00563ad4, new List<int>() {0x0049a253, 0x004e1657, 0x004e1682, }}, // Sepith
            {0x00563adc, new List<int>() {0x0049a2a0, }}, // ◆━
            {0x00563ae4, new List<int>() {0x0049a2dd, }}, // ━◆
            {0x00563aec, new List<int>() {0x0049a339, }}, // ◆            ◆
            {0x00563b00, new List<int>() {0x0049a34d, }}, // Sepith - Rate
            {0x00563b10, new List<int>() {0x0049a357, }}, //  Sepith - Rate
            {0x00563b20, new List<int>() {0x0049a394, }}, // ◆                     ◆
            {0x00563b3c, new List<int>() {0x0049a366, }}, // Sepith - Cost
            {0x00563b4c, new List<int>() {0x0049a370, }}, //  Sepith - Cost
            {0x00563b8c, new List<int>() {0x0049ae5e, }}, // Total
            {0x00563b94, new List<int>() {0x0049aeb0, }}, // Trade sepith
            {0x00563ba4, new List<int>() {0x0049aeb7, }}, // 合計
            {0x00563bac, new List<int>() {0x0049af09, }}, // セピスを換金する
            {0x00563cc0, new List<int>() {0x005d9dbc, }}, // Rest
            {0x00563cc8, new List<int>() {0x005d9da8, }}, //  休　む
            {0x00563cd0, new List<int>() {0x005d9d9c, }}, // Convert
            {0x00563cd8, new List<int>() {0x005d9d98, }}, // Sell
            {0x00563ce0, new List<int>() {0x005d9d94, }}, // Buy
            {0x00563ce4, new List<int>() {0x005d9d8c, 0x005d9db0, }}, //  やめる
            {0x00563cec, new List<int>() {0x005d9d88, 0x005d9dac, }}, //  換  金
            {0x00563cf4, new List<int>() {0x005d9d84, }}, //  売  却
            {0x00563cfc, new List<int>() {0x005d9d80, }}, //  購  入
            {0x00563d04, new List<int>() {0x0049b9ad, }}, // Buy items.
            {0x00563d10, new List<int>() {0x0049b9bc, }}, // 商品を購入します。
            {0x00563d24, new List<int>() {0x0049b9d9, }}, // Sell items.
            {0x00563d30, new List<int>() {0x0049b9e8, }}, // 所持品を売却します。
            {0x00563d48, new List<int>() {0x0049ba05, }}, // Cash in Sepith.
            {0x00563d58, new List<int>() {0x0049ba31, }}, // Leave shop.
            {0x00563d64, new List<int>() {0x0049ba40, }}, // お店を出ます。
            {0x00563d74, new List<int>() {0x0049ba5d, }}, // What will you do?
            {0x00563d88, new List<int>() {0x0049be09, }}, // What will you purchase?
            {0x00563da0, new List<int>() {0x0049be10, }}, // 何を購入しますか？
            {0x00563db4, new List<int>() {0x0049c349, }}, // Sale price: ------------------
            {0x00563dd4, new List<int>() {0x0049c350, }}, // 売却価格：------------------
            {0x00563df4, new List<int>() {0x0049c374, }}, // What will you sell?
            {0x00563e08, new List<int>() {0x0049c38a, }}, // 何を売却しますか？
            {0x00563e1c, new List<int>() {0x0049c3e1, }}, // Sale price: %5dx%2d = %7d
            {0x00563e38, new List<int>() {0x0049c3e8, }}, // 売却価格：%5dx%2d = %7d
            {0x00563e50, new List<int>() {0x0049c459, }}, // Buy %d for %d mira.
            {0x00563e64, new List<int>() {0x0049c460, }}, // %d個を%dミラで購入します。
            {0x00563e80, new List<int>() {0x0049c4c9, }}, // Sell %d for %d mira.
            {0x00563e98, new List<int>() {0x0049c4d5, }}, // %d個を%dミラで売却します。
            {0x00563eb4, new List<int>() {0x0049c7f2, }}, // %c#0CCannot carry any more #1C%s#0C.
            {0x00563edc, new List<int>() {0x0049c7f9, }}, // %c#1C%s#0Cは、もうこれ以上持つことはできません。
            {0x00563f10, new List<int>() {0x0049c9a0, }}, // %c#0CBuy %d #1C%s#0C for %d mira?
            {0x00563f34, new List<int>() {0x0049c9c3, }}, // %c#1C%s#0Cを%d個、%dミラで購入します。よろしいですか？
            {0x00563f6c, new List<int>() {0x0049ca75, }}, // %c#0CBought %d #1C%s#0C for %d mira.
            {0x00563f94, new List<int>() {0x0049caa8, }}, // %c#1C%s#0Cを%d個、%dミラで購入しました。
            {0x00563fc0, new List<int>() {0x0049d5fe, 0x0049cae5, }}, // %cInsufficient mira.
            {0x00563fd8, new List<int>() {0x0049d60a, 0x0049caec, }}, // %cミラが足りません。
            {0x00563ff0, new List<int>() {0x0049d13a, }}, // %c#0CSell %d #1C%s#0C for %d mira?
            {0x00564014, new List<int>() {0x0049d170, }}, // %c#1C%s#0Cを%d個、%dミラで売却します。よろしいですか？
            {0x0056404c, new List<int>() {0x0049d235, }}, // %c#0CSold %d #1C%s#0C for %d mira.
            {0x00564070, new List<int>() {0x0049d26b, }}, // %c#1C%s#0Cを%d個、%dミラで売却しました。
            {0x0056409c, new List<int>() {0x0049d387, }}, // Rest and recover.
            {0x005640b0, new List<int>() {0x0049d396, }}, // 休憩して回復をします。
            {0x005640c8, new List<int>() {0x0049d360, }}, // Select an option.
            {0x005640e0, new List<int>() {0x0049d5bd, }}, // %c#0CIt will be #3C%d#0C for the night.\nWill you spend the night?
            {0x00564124, new List<int>() {0x0049d5c9, }}, // %c#0C宿泊料は#3C%dミラ#0Cとなります。よろしいですか？
            {0x0056415c, new List<int>() {0x0049dfd4, }}, // Equip Status
            {0x0056416c, new List<int>() {0x0049e014, }}, // ◆━                        ━◆
            {0x00564190, new List<int>() {0x0049e035, }}, //      Equip Status
            {0x005641a4, new List<int>() {0x0049e446, }}, // Ingredients
            {0x005641b0, new List<int>() {0x0049e453, }}, // 材料一覧
            {0x005641c4, new List<int>() {0x0049e532, }}, // Recipe not learned
            {0x005641d8, new List<int>() {0x0049e544, }}, // 未修得
            {0x005641e0, new List<int>() {0x0049e5a5, }}, // Used in:
            {0x005641ec, new List<int>() {0x0049e5ac, }}, // 以下の料理で使用
            {0x00564200, new List<int>() {0x0049e702, }}, // (Need %2d)
            {0x00564214, new List<int>() {0x0049e82e, 0x0049ef48, 0x004c6327, }}, // ９
            {0x00564218, new List<int>() {0x0049e836, 0x0049ef50, 0x004c632f, }}, // ８
            {0x0056421c, new List<int>() {0x0049e83e, 0x0049ef58, 0x004c6337, }}, // ７
            {0x00564220, new List<int>() {0x0049e846, 0x0049ef60, 0x004c633f, }}, // ６
            {0x00564224, new List<int>() {0x0049e84e, 0x0049ef68, 0x004c6347, }}, // ５
            {0x00564228, new List<int>() {0x0049e856, 0x0049ef70, 0x004c634f, }}, // ４
            {0x0056422c, new List<int>() {0x0049e85e, 0x0049ef78, 0x004c6357, }}, // ３
            {0x00564230, new List<int>() {0x0049e866, 0x0049ef80, 0x004c635f, }}, // ２
            {0x00564234, new List<int>() {0x0049e86e, 0x0049ef88, 0x004c6367, }}, // １
            {0x00564238, new List<int>() {0x0049e876, 0x0049ef90, 0x004c636f, }}, // 9th
            {0x0056423c, new List<int>() {0x0049e87e, 0x0049ef98, 0x004c6377, }}, // 8th
            {0x00564240, new List<int>() {0x0049e886, 0x0049efa0, 0x004c637f, }}, // 7th
            {0x00564244, new List<int>() {0x0049e88e, 0x0049efa8, 0x004c6387, }}, // 6th
            {0x00564248, new List<int>() {0x0049e896, 0x0049efb0, 0x004c638f, }}, // 5th
            {0x0056424c, new List<int>() {0x0049e89e, 0x0049efb8, 0x004c6397, }}, // 4th
            {0x00564250, new List<int>() {0x0049e8a6, 0x0049efc0, 0x004c639f, }}, // 3rd
            {0x00564254, new List<int>() {0x0049e8ae, 0x0049efc8, 0x004c63a7, }}, // 2nd
            {0x00564258, new List<int>() {0x0049e8b6, 0x0049efd0, 0x004c63af, }}, // 1st
            {0x0056425c, new List<int>() {0x0049e981, }}, // Bracer Status
            {0x0056426c, new List<int>() {0x0049ea21, }}, // Rank: Junior Bracer - %s Class
            {0x0056428c, new List<int>() {0x0049ea4e, }}, // Total BP: %4d
            {0x0056429c, new List<int>() {0x0049ea75, }}, // Jobs Completed: %4d
            {0x005642b0, new List<int>() {0x0049ea8d, 0x0049ed40, }}, // ◆━━                        ━━◆
            {0x005642d8, new List<int>() {0x0049eaae, }}, //      Bracer Status
            {0x005642ec, new List<int>() {0x0049ead3, }}, // ランク    準遊撃士・%s級
            {0x00564308, new List<int>() {0x0049eaff, }}, // 獲得累積ＢＰ        % 4d
            {0x00564324, new List<int>() {0x0049eb26, }}, // 依頼達成数          % 4d
            {0x00564340, new List<int>() {0x0049ecb6, }}, // Guild Bonus
            {0x0056434c, new List<int>() {0x0049ed61, }}, //       Guild Bonus
            {0x00564360, new List<int>() {0x0049efdf, }}, // Report
            {0x00564368, new List<int>() {0x0049efe6, }}, // 依頼の報告
            {0x00564374, new List<int>() {0x0049f09e, }}, // Received payment for [%c%c%s%c%c].
            {0x00564398, new List<int>() {0x0049f0a5, }}, // Reported results for [%c%c%s%c%c].
            {0x005643bc, new List<int>() {0x0049f0b2, }}, // 『%c%c%s%c%c』の報酬を取得しました。
            {0x005643e4, new List<int>() {0x0049f0b9, }}, // 『%c%c%s%c%c』の報告を行いました。
            {0x00564408, new List<int>() {0x0049f19c, }}, // %cPayment in mira: %c%c%5d (%+5d)%c%c\nGained BP: %c%c%5d (%+5d)%c%c
            {0x00564450, new List<int>() {0x0049f1d3, }}, // %c取得ミラ  ：  %c%c%5d (%+5d)%c%c\n取得ＢＰ  ：  %c%c%5d (%+5d)%c%c
            {0x00564494, new List<int>() {0x0049f230, }}, // Currently, there is nothing to report.
            {0x005644bc, new List<int>() {0x0049f237, }}, // 報告できる依頼は特にありません。
            {0x005644e0, new List<int>() {0x0049f254, }}, // Rank status
            {0x005644ec, new List<int>() {0x0049f25b, }}, // ランクの確認
            {0x005644fc, new List<int>() {0x0049f2ce, }}, // Rank advancement to [Junior Bracer - %s Class].
            {0x0056452c, new List<int>() {0x0049f2df, }}, // ランクが『準遊撃士・%s級』に上がりました。
            {0x00564558, new List<int>() {0x0049f367, }}, // Received [%c%c%s%c%c] quartz as a perk.
            {0x00564580, new List<int>() {0x0049f382, }}, // 特典として『%c%c%s%c%c』#5Cのクオーツをもらいました。
            {0x005645b8, new List<int>() {0x0049f3a0, }}, // Received [%c%c%s%c%c] accessory as a perk.
            {0x005645e4, new List<int>() {0x0049f3a7, }}, // 特典として、装備品『%c%c%s%c%c』#5Cをもらいました。
            {0x00564618, new List<int>() {0x0049f3f0, }}, // Current rank is [Junior Bracer - %s Class].
            {0x00564644, new List<int>() {0x0049f401, }}, // 現在のランクは『準遊撃士・%s級』です。
            {0x00564840, new List<int>() {0x004a0896, }}, // Cannot hold any more.
            {0x00564858, new List<int>() {0x004a089d, 0x004a16c1, }}, // これ以上持てません。
            {0x00564870, new List<int>() {0x004a0bf0, }}, // %cGot %d of #2C[%s]#0C.
            {0x00564888, new List<int>() {0x004a0c4c, }}, // %c#2C【%s】#5Cを、%d個手に入れた。
            {0x005648ac, new List<int>() {0x004a0ea3, }}, // %cCooked and ate #2C[%s]#0C.\n%s got #2Cfood-poisoning#0C.
            {0x005648e8, new List<int>() {0x004a0ed7, }}, // %c#2C【%s】#0Cを料理して食べた。\n%sは#2Cアタリ#0Cを引いてしまった。
            {0x00564930, new List<int>() {0x004a0f9e, }}, // %cCooked and ate #2C[%s]#0C.\nEveryone recovered #12C%d#0C HP and #12C%d#0C CP.
            {0x00564980, new List<int>() {0x004a0fd6, }}, // %c#2C【%s】#5Cを料理して食べた。\n全員のＨＰが%d#5C、ＣＰが%d#5C回復した。
            {0x005649cc, new List<int>() {0x004a1011, }}, // %cCooked and ate #2C[%s]#0C.\nEveryone recovered #12C%d#0C HP.
            {0x00564a0c, new List<int>() {0x004a1018, }}, // %c#2C【%s】#5Cを料理して食べた。\n全員のＨＰが%d#5C回復した。
            {0x00564a4c, new List<int>() {0x004a16b2, }}, // Can't hold any more.
            {0x00564a64, new List<int>() {0x004a16e0, }}, // Make how many #2C[%s]#0C?
            {0x00564a80, new List<int>() {0x004a16f6, }}, // #2C【%s】#0Cを、何個作りますか？
            {0x00564aa4, new List<int>() {0x004a1859, }}, // Make #2C[%s]#0C?
            {0x00564ab8, new List<int>() {0x004a186f, }}, // #2C【%s】#0Cを作りますか？
            {0x00564af8, new List<int>() {0x004a21cb, }}, // MAX
            {0x00565d4c, new List<int>() {0x004bd5cd, }}, // %cCannot hold any more of this equipment so cannot unequip.
            {0x00565d88, new List<int>() {0x004bd5d9, }}, // %c現在の装備品をこれ以上持てないので、装備をはずせません。
            {0x00565dc4, new List<int>() {0x004bd670, }}, // %cCannot hold any more of this equipment so cannot change.
            {0x00565e00, new List<int>() {0x004bd67c, }}, // %c現在の装備品をこれ以上持てないので、装備を変更できません。
            {0x00565e40, new List<int>() {0x004bd976, 0x004bdb0b, }}, // %cCannot hold any more of this quartz so cannot remove.
            {0x00565e78, new List<int>() {0x004bd982, 0x004bdb17, }}, // %c装備しているクオーツをこれ以上持てないので、\nオーブメントからはずせません。
            {0x00565ec8, new List<int>() {0x004bda80, }}, // %cCannot install two of the same type of quartz.
            {0x00565efc, new List<int>() {0x004bda8f, }}, // %c同系統のクオーツは、同時に装備できません。
            {0x00565f2c, new List<int>() {0x004bda54, }}, // %cQuartz does not match elemental slot so cannot install.
            {0x00565f68, new List<int>() {0x004bda9e, }}, // %cそのクオーツは、スロットの属性と合わないため装備できません。
            {0x00565fa8, new List<int>() {0x004bddf9, 0x004be0da, }}, // %c%s recovered %d HP.
            {0x00565fc0, new List<int>() {0x004bde1e, 0x004be0ed, }}, // %c%8sのＨＰを、%4d回復した。
            {0x00565fe0, new List<int>() {0x004bde94, }}, // %c%s recovered %d HP.\n%8s recovered %d HP.
            {0x0056600c, new List<int>() {0x004bdeb9, }}, // %c%8sのＨＰを、%4d回復した。\n%8sのＨＰを、%4d回復した。
            {0x00566048, new List<int>() {0x004bdf48, }}, // %c%s recovered %d HP.\n%8s recovered %d HP.\n%8s recovered %d HP.
            {0x00566088, new List<int>() {0x004bdf6d, }}, // %c%8sのＨＰを、%4d回復した。\n%8sのＨＰを、%4d回復した。\n%8sのＨＰを、%4d回復した。
            {0x005660e0, new List<int>() {0x004be015, }}, // %c%s recovered %d HP.\n%8s recovered %d HP.\n%8s recovered %d HP.\n%8s recovered %d HP.
            {0x00566138, new List<int>() {0x004be03a, }}, // %c%8sのＨＰを、%4d回復した。\n%8sのＨＰを、%4d回復した。\n%8sのＨＰを、%4d回復した。\n%8sのＨＰを、%4d回復した。
            {0x005661a8, new List<int>() {0x004be139, }}, // Charged %d EP.
            {0x005661b8, new List<int>() {0x004be14c, }}, // %dＥＰをチャージした。
            {0x005661d0, new List<int>() {0x004be1a3, }}, // Restored %d CP.
            {0x005661e0, new List<int>() {0x004be1b6, }}, // %dＣＰが上昇した。
            {0x005661f4, new List<int>() {0x004be32b, }}, // Learned recipe for "%s".
            {0x005666b0, new List<int>() {0x00579cf8, }}, // Ending
            {0x005666b8, new List<int>() {0x00579cf4, }}, // Opening
            {0x005666c0, new List<int>() {0x00579cf0, }}, //  ＮＥＸＴ
            {0x005666cc, new List<int>() {0x00579cec, }}, //  ＥＮＤＩＮＧ 
            {0x005666dc, new List<int>() {0x00579ce8, }}, //  ＯＰＥＮＩＮＧ 
            {0x005666f0, new List<int>() {0x00579ce4, }}, // Mirage quartz only
            {0x00566704, new List<int>() {0x00579ce0, }}, // Space quartz only
            {0x00566718, new List<int>() {0x00579cdc, }}, // Time quartz only
            {0x0056672c, new List<int>() {0x00579cd8, }}, // Wind quartz only
            {0x00566740, new List<int>() {0x00579cd4, }}, // Fire quartz only
            {0x00566754, new List<int>() {0x00579cd0, }}, // Water quartz only
            {0x00566768, new List<int>() {0x00579ccc, }}, // Earth quartz only
            {0x0056677c, new List<int>() {0x00579cc8, }}, // All quartz OK
            {0x0056678c, new List<int>() {0x00579cc4, }}, // 幻属性装着可
            {0x0056679c, new List<int>() {0x00579cc0, }}, // 空属性装着可
            {0x005667ac, new List<int>() {0x00579cbc, }}, // 時属性装着可
            {0x005667bc, new List<int>() {0x00579cb8, }}, // 風属性装着可
            {0x005667cc, new List<int>() {0x00579cb4, }}, // 火属性装着可
            {0x005667dc, new List<int>() {0x00579cb0, }}, // 水属性装着可
            {0x005667ec, new List<int>() {0x00579cac, }}, // 地属性装着可
            {0x005667fc, new List<int>() {0x00579ca8, }}, // 全属性装着可
            {0x0056680c, new List<int>() {0x0046d206, 0x00579ca4, }}, // Slot is sealed
            {0x0056681c, new List<int>() {0x00579ca0, }}, // Slot cannot be opened
            {0x00566834, new List<int>() {0x00579c9c, }}, // Slot can be opened
            {0x00566848, new List<int>() {0x00579c98, }}, // Slot already open
            {0x0056685c, new List<int>() {0x00579c7c, }}, // 捨てる
            {0x00566864, new List<int>() {0x00579c78, }}, // 使  う
            {0x0056686c, new List<int>() {0x00579c74, }}, // Exit Game
            {0x00566878, new List<int>() {0x00579c70, }}, // Return to Title
            {0x00566888, new List<int>() {0x00579c6c, }}, //  ゲームを終了する  
            {0x0056689c, new List<int>() {0x00579c68, }}, //   タイトルに戻る   
            {0x005668b0, new List<int>() {0x00579c64, }}, // Exit
            {0x005668b8, new List<int>() {0x00579c60, }}, // Erase
            {0x005668c0, new List<int>() {0x00579c5c, }}, // Load
            {0x005668c8, new List<int>() {0x00579c58, }}, // Save
            {0x005668d0, new List<int>() {0x00579c54, }}, //  ＥＸＩＴ  
            {0x005668dc, new List<int>() {0x00579c50, }}, //  ＥＲＡＳＥ
            {0x005668e8, new List<int>() {0x00579c4c, }}, //  ＬＯＡＤ  
            {0x005668f4, new List<int>() {0x00579c48, }}, //  ＳＡＶＥ  
            {0x00566900, new List<int>() {0x00579c34, }}, // Turmoil in the Royal City
            {0x0056691c, new List<int>() {0x00579c30, }}, // The Black Orbment
            {0x00566930, new List<int>() {0x00579c2c, }}, // Madrigal of the White Magnolia
            {0x00566950, new List<int>() {0x00579c28, }}, // Disappearance of the Linde
            {0x0056696c, new List<int>() {0x00579c24, }}, // A Father's Love, a New Beginning
            {0x00566990, new List<int>() {0x00579c1c, }}, // 　 　王都撩乱     
            {0x005669a4, new List<int>() {0x00579c18, }}, //  黒のオーブメント 
            {0x005669b8, new List<int>() {0x00579c14, }}, // 白き花のマドリガル
            {0x005669cc, new List<int>() {0x00579c10, }}, //   消えた飛行客船  
            {0x005669e0, new List<int>() {0x00579c0c, }}, // 　　父、旅立つ    
            {0x005669f4, new List<int>() {0x00480b92, 0x00579bfc, }}, // Clear Data
            {0x00566a00, new List<int>() {0x00579bf8, }}, // Final Chapter
            {0x00566a10, new List<int>() {0x00579bf4, }}, // Chapter 3
            {0x00566a1c, new List<int>() {0x00579bf0, }}, // Chapter 2
            {0x00566a28, new List<int>() {0x00579bec, }}, // Chapter 1
            {0x00566a34, new List<int>() {0x00579be8, }}, // Prologue
            {0x00566a40, new List<int>() {0x00480b92, 0x00480145, 0x004806a7, 0x00579be4, }}, // クリアデータ
            {0x00566a50, new List<int>() {0x00579be0, }}, // 終  章
            {0x00566a58, new List<int>() {0x00579bdc, }}, // 第三章
            {0x00566a60, new List<int>() {0x00579bd8, }}, // 第二章
            {0x00566a68, new List<int>() {0x00579bd4, }}, // 第一章
            {0x00566a70, new List<int>() {0x00579bd0, }}, // 序  章
            {0x00566b30, new List<int>() {0x004c260d, }}, // ◆━━━━━━━━━━━━━━━━━━━━━━━━◆
            {0x00566b84, new List<int>() {0x004c2d00, }}, // 　Mira:
            {0x00566b8c, new List<int>() {0x004c2d18, }}, // 　BP:
            {0x00566b94, new List<int>() {0x004c2d27, }}, // 　Status:
            {0x00566ba0, new List<int>() {0x004c2d2e, }}, // 　取得ミラ：
            {0x00566bb0, new List<int>() {0x004c2d46, }}, // 　取得ＢＰ：
            {0x00566bc0, new List<int>() {0x004c2d55, }}, // 　状況　　：
            {0x00566c38, new List<int>() {0x004c2eba, 0x004c2e84, }}, // Reported
            {0x00566c44, new List<int>() {0x004c2eba, 0x004c2e8f, }}, // Finished
            {0x00566c50, new List<int>() {0x004c2eba, 0x004c2e9a, }}, // Failed
            {0x00566c58, new List<int>() {0x004c2eba, 0x004c2ea5, }}, // Expired
            {0x00566c60, new List<int>() {0x004c2eba, 0x004c2eae, }}, // In Progress
            {0x00566c7c, new List<int>() {0x004c2f14, 0x004c2ede, }}, // 報告済み
            {0x00566c88, new List<int>() {0x004c2f14, 0x004c2ee9, }}, // 達成済み
            {0x00566c94, new List<int>() {0x004c2f14, 0x004c2ef4, }}, // 失敗
            {0x00566c9c, new List<int>() {0x004c2f14, 0x004c2eff, 0x004c4deb, }}, // 期限切れ
            {0x00566ca8, new List<int>() {0x004c2f14, 0x004c2f08, }}, // 請負中
            {0x00566cb0, new List<int>() {0x004c3036, }}, // ◆━━━━━━━━━━━━━━━━━━━━━━━◆
            {0x00566ce4, new List<int>() {0x004c4c6a, }}, // new!
            {0x00566cec, new List<int>() {0x004c4ca4, }}, // Reported!
            {0x00566cf8, new List<int>() {0x004c4cb3, }}, // Report!
            {0x00566d00, new List<int>() {0x004c4ccf, }}, // Clear!
            {0x00566d08, new List<int>() {0x004c4d4e, }}, // Term (Short)
            {0x00566d18, new List<int>() {0x004c4d6a, }}, // Term (Medium)
            {0x00566d28, new List<int>() {0x004c4d8a, }}, // Term (Long)
            {0x00566d34, new List<int>() {0x004c4d9c, }}, // 期限(短)
            {0x00566d40, new List<int>() {0x004c4daf, }}, // 期限(中)
            {0x00566d4c, new List<int>() {0x004c4dc2, }}, // 期限(長)
            {0x00566d58, new List<int>() {0x004c4de4, }}, // Term (Failed)
            {0x00566d68, new List<int>() {0x004c63c8, }}, // Junior Bracer - %s Class
            {0x00566d84, new List<int>() {0x004c63d4, }}, // 準遊撃士・%s級
            {0x00567400, new List<int>() {0x004dc5b3, }}, // Mira/Sepith
            {0x0056740c, new List<int>() {0x004dc5ba, }}, // ミラ・セピス
            {0x0056741c, new List<int>() {0x004dc7f7, 0x004dc774, 0x004dc6f1, 0x004dc66e, 0x004dc5eb, 0x004dc5df, 0x004dc662, 0x004dc6e5, 0x004dc768, 0x004dc7eb, }}, // Carry over
            {0x00567428, new List<int>() {0x004dc5eb, 0x004dc66e, 0x004dc6f1, 0x004dc774, 0x004dc7f7, 0x004dc5e6, 0x004dc669, 0x004dc6ec, 0x004dc76f, 0x004dc7f2, }}, // 引き継ぐ
            {0x00567434, new List<int>() {0x004dc812, 0x004dc78f, 0x004dc70c, 0x004dc689, 0x004dc606, 0x004dc5fa, 0x004dc67d, 0x004dc700, 0x004dc783, 0x004dc806, }}, // Don't carry over
            {0x00567448, new List<int>() {0x004dc812, 0x004dc78f, 0x004dc70c, 0x004dc689, 0x004dc606, 0x004dc601, 0x004dc684, 0x004dc707, 0x004dc78a, 0x004dc80d, }}, // 引き継がない
            {0x00567458, new List<int>() {0x004dc63b, 0x004e0bf9, 0x004e0d65, 0x004e0e42, }}, // アイテム
            {0x00567464, new List<int>() {0x004dc6be, }}, // 装備品
            {0x0056746c, new List<int>() {0x004dc73a, }}, // Information
            {0x00567478, new List<int>() {0x004dc741, }}, // 手帳
            {0x00567480, new List<int>() {0x004dc7c4, }}, // ステータス
            {0x0056748c, new List<int>() {0x004dc840, }}, // Enemy Strength
            {0x0056749c, new List<int>() {0x004dc847, }}, // 敵の強さ
            {0x005674a8, new List<int>() {0x004dc929, }}, // Decide
            {0x005674b0, new List<int>() {0x004dc930, }}, // 決定
            {0x005674b8, new List<int>() {0x004dcfc4, }}, // Carry over mira and sepith.
            {0x005674d4, new List<int>() {0x004dcfd3, }}, // ミラとセピスとメダルを引き継ぎます。
            {0x005674fc, new List<int>() {0x004dcff0, }}, // Carry over usable inventory items.
            {0x00567520, new List<int>() {0x004dcfff, }}, // 各種アイテム(回復、食材、本、釣具など)を引き継ぎます。
            {0x00567558, new List<int>() {0x004dd01c, }}, // Carry over equipment and quartz (~10 per).\n※Quest-related items not included.
            {0x005675a8, new List<int>() {0x004dd02b, }}, // 装備品とクオーツを引き継ぎます(各アイテム10個まで)。\n※クエスト関連アイテムを除きます。
            {0x00567600, new List<int>() {0x004dd048, }}, // Carry over recipe book and monster guide.
            {0x0056762c, new List<int>() {0x004dd057, }}, // レシピ手帳、魔獣手帳、\n王国地図の内容を引き継ぎます。
            {0x00567664, new List<int>() {0x004dd074, }}, // Carry over levels and opened orbment slots.
            {0x00567690, new List<int>() {0x004dd083, }}, // レベルとスロットの強化状況を引き継ぎます。
            {0x005676c0, new List<int>() {0x004dd0b2, }}, // Set enemy difficulty.\n[Normal] Average difficulty. For those wanting a little challenge.
            {0x00567720, new List<int>() {0x004dd0c6, }}, // Set enemy difficulty.\n[Hard] A difficult setting. For those who find normal a bit lacking.
            {0x00567780, new List<int>() {0x004dd0da, }}, // Set enemy difficulty.\n[Nightmare] It's your funeral. May Aidios be with you.
            {0x005677d0, new List<int>() {0x004dd0ee, }}, // Set enemy difficulty.\n[Easy] A gentle balance for easy progression. For beginners.
            {0x00567828, new List<int>() {0x004dd112, }}, // 敵の強さを変更できます。\n【ノーマル】標準的なバランスです。適度な刺激を楽しみたい方に。
            {0x00567880, new List<int>() {0x004dd126, }}, // 敵の強さを変更できます。\n【ハード】やや厳しいバランスです。ノーマルでは物足りない方に。
            {0x005678d8, new List<int>() {0x004dd13a, }}, // 敵の強さを変更できます。\n【ナイトメア】まさに悪夢。各種データを引き継がないと厳しいバランスです。
            {0x00567940, new List<int>() {0x004dd14e, }}, // 敵の強さを変更できます。\n【イージー】サクサク進める易しいバランスです。時間のない方や初心者向け。
            {0x005679a4, new List<int>() {0x004dd170, }}, // Start game with the above settings.
            {0x005679c8, new List<int>() {0x004dd17f, }}, // 以上の設定でゲームを開始します。
            {0x00567aac, new List<int>() {0x004e05b7, }}, // Lv.%d  HP:%d
            {0x00567abc, new List<int>() {0x004e0700, }}, // STR:%3d  DEF:%3d
            {0x00567ad0, new List<int>() {0x004e073f, }}, // ATS:%3d  ADF:%3d
            {0x00567ae4, new List<int>() {0x004e0785, }}, // SPD:%3d  EXP:%3d
            {0x00567af8, new List<int>() {0x004e07b8, }}, // STR:???  DEF:???
            {0x00567b0c, new List<int>() {0x004e07cb, }}, // ATS:???  ADF:???
            {0x00567b20, new List<int>() {0x004e07de, }}, // SPD:???  EXP:???
            {0x00567b58, new List<int>() {0x004e08f0, }}, // Elemental
            {0x00567b64, new List<int>() {0x004e08fa, }}, // Efficacy (%)
            {0x00567b74, new List<int>() {0x004e0901, }}, // 属性攻撃
            {0x00567b80, new List<int>() {0x004e090b, }}, // 有効率(％)
            {0x00567b8c, new List<int>() {0x004e0bec, 0x004e0d58, 0x004e0e31, }}, // Item
            {0x00567b9c, new List<int>() {0x004e13c1, 0x004e1432, }}, // StatGuard
            {0x00567ba8, new List<int>() {0x004e13cb, 0x004e1443, }}, // 異常耐性
            {0x00567bb4, new List<int>() {0x004e1661, 0x004e16db, }}, // セピス
            {0x00567bbc, new List<int>() {0x004e1867, }}, // Location
            {0x00567bc8, new List<int>() {0x004e186e, }}, // 出現場所
            {0x0057988c, new List<int>() {0x0057989c, }}, //  は  い
            {0x00579894, new List<int>() {0x005798a0, }}, //  いいえ
            {0x005798a4, new List<int>() {0x005798b0, }}, //  Yes
            {0x005798ac, new List<int>() {0x005798b4, }}, //  No
        };
        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        public override void Open(DockPanel panel)
        {
            _ftView = new FontTableView();
            _charWidths = GetFontTable();
            _ftView.LoadFontTable(_charWidths);
            _ftView.Show(panel, DockState.Document);

            base.Open(panel);
        }

        protected virtual CharacterInfo[] GetFontTable()
        {
            var result = new CharacterInfo[128];

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs))
            {
                input.Seek(FontTableOffset, SeekOrigin.Begin);

                for (var i = 0; i < 128; i++)
                {
                    var width = input.ReadInt32();

                    result[i] = new CharacterInfo((byte)i)
                    {
                        OriginalWidth = width,
                        Width = width,
                    };
                }
            }

            LoadFontTableChanges(result);

            for (var i = 0; i < 128; i++)
            {
                result[i].SetLoadedWidth();
                result[i].PropertyChanged += SubtitlePropertyChanged;
            }

            return result;
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            var peInfo = WindowsAssembly.FromFile(Path);

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.LittleEndian))
            {
                foreach (var kvp in StringOffsets)
                {
                    var section = this.GetSectionByVirtualAddress(peInfo, kvp.Key);
                    var rawOffset = (long)kvp.Key - (long)(peInfo.NtHeaders.OptionalHeader.ImageBase + (section.Header.VirtualAddress - section.Header.PointerToRawData));
                    
                    var sub = ReadSubtitle(input, rawOffset, false);
                    sub.Offset = kvp.Key;
                    sub.PropertyChanged += SubtitlePropertyChanged;

                    result.Add(sub);
                }
            }

            result.Sort();
            LoadChanges(result);

            return result;
        }

        public override void SaveChanges()
        {
            using (var fs = new FileStream(ChangesFile, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, System.Text.Encoding.Unicode))
            {
                output.Write(ChangesFileVersion);

                for (var i = 0; i < 128; i++)
                {
                    output.Write(_charWidths[i].Width);
                    _charWidths[i].SetLoadedWidth();
                }

                output.Write(_subtitles.Count);
                foreach (var subtitle in _subtitles)
                {
                    output.Write(subtitle.Offset);
                    output.WriteString(subtitle.Translation);

                    subtitle.Loaded = subtitle.Translation;
                }
            }

            NeedSaving = false;
            OnFileChanged();
        }

        protected override void LoadChanges(IList<Subtitle> subtitles)
        {
            if (HasChanges)
            {
                using (var fs = new FileStream(ChangesFile, FileMode.Open))
                using (var input = new ExtendedBinaryReader(fs, System.Text.Encoding.Unicode))
                {
                    var version = input.ReadInt32();

                    if (version != ChangesFileVersion)
                    {
                        //System.IO.File.Delete(ChangesFile);
                        return;
                    }

                    input.Skip(128 * 4);

                    var subtitleCount = input.ReadInt32();

                    for (var i = 0; i < subtitleCount; i++)
                    {
                        var offset = input.ReadInt64();
                        var text = input.ReadString();

                        var subtitle = subtitles.FirstOrDefault(x => x.Offset == offset);
                        if (subtitle != null)
                        {
                            subtitle.PropertyChanged -= SubtitlePropertyChanged;
                            subtitle.Translation = text;
                            subtitle.Loaded = subtitle.Translation;
                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                        }
                    }
                }
            }
        }

        private void LoadFontTableChanges(CharacterInfo[] data)
        {
            if (HasChanges)
            {
                using (var fs = new FileStream(ChangesFile, FileMode.Open))
                using (var input = new ExtendedBinaryReader(fs))
                {
                    var version = input.ReadInt32();

                    if (version != ChangesFileVersion)
                    {
                        //System.IO.File.Delete(ChangesFile);
                        return;
                    }

                    for (var i = 0; i < 128; i++)
                    {
                        data[i].Width = input.ReadInt32();
                    }
                }
            }
        }

        protected override void SubtitlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedSaving = _subtitles.Any(subtitle => subtitle.HasChanges) || _charWidths.Any(x => x.HasChanged);
            OnFileChanged();
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(outputFolder, RelativePath));
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            RebuildSubtitles(outputPath);
            RebuildFontTable(outputPath);
            RebuildPatches(outputPath);
        }

        private void RebuildSubtitles(string outputFile)
        {
            var data = GetSubtitles();

            if (data.Any(x => x.Text != x.Translation))
            {
                CreateExeFile(outputFile);

                var peInfo = WindowsAssembly.FromFile(outputFile);

                using (var outputFs = new FileStream(outputFile, FileMode.Open))
                using (var outputReader = new ExtendedBinaryReader(outputFs, FileEncoding))
                using (var output = new ExtendedBinaryWriter(outputFs, FileEncoding))
                {
                    var translationSection = peInfo.GetSectionByName(".trad\0\0\0");
                    var translationSectionBase = (long)(peInfo.NtHeaders.OptionalHeader.ImageBase +
                                                         (translationSection.Header.VirtualAddress -
                                                          translationSection.Header.PointerToRawData));

                    var outputOffset = (long)translationSection.Header.PointerToRawData;

                    foreach (var kvp in StringOffsets)
                    {
                        var subtitle = data.FirstOrDefault(x => x.Offset == kvp.Key);

                        if (subtitle != null)
                        {
                            if (subtitle.Text != subtitle.Translation)
                            {
                                var newOffset = outputOffset + translationSectionBase;
                                foreach (var reference in kvp.Value)
                                {
                                    var section = this.GetSectionByVirtualAddress(peInfo, reference);

                                    var rawOffset = (long)reference - (long)(peInfo.NtHeaders.OptionalHeader.ImageBase + (section.Header.VirtualAddress - section.Header.PointerToRawData));
                                    if (section.Header.Name == ".data\0\0\0")
                                    {
                                        output.Seek(rawOffset, SeekOrigin.Begin);
                                    }
                                    else
                                    {
                                        outputReader.Seek(rawOffset, SeekOrigin.Begin);
                                        var op = outputReader.ReadByte();
                                        switch (op)
                                        {
                                            case 0x50: // PUSH EAX
                                            case 0x89: 
                                                {
                                                    continue;
                                                }

                                            case 0x8B: // MOV EAX, ?
                                                {
                                                    output.Seek(rawOffset + 2, SeekOrigin.Begin);
                                                    break;
                                                }

                                            case 0x68: // PUSH
                                            case 0xA1: // MOV EAX, ?
                                            case 0xB8: // MOV EAX, ?
                                            case 0xB9: // MOV ECX, ?
                                            case 0xBB: // MOV EBX, ?
                                            case 0xBF: // MOV EDI, ?
                                                {
                                                    output.Seek(rawOffset + 1, SeekOrigin.Begin);
                                                    break;
                                                }

                                            case 0xC7: // MOV ptr, ?
                                                {
                                                    output.Seek(rawOffset + 4, SeekOrigin.Begin);
                                                    break;
                                                }
                                            default:
                                                {
                                                    throw new NotImplementedException($"Pos: 0x{kvp.Key:X8} - Op: 0x{op:X2}");
                                                }
                                        }
                                    }
                                    
                                    output.Write((int)newOffset);
                                }
                                outputOffset = WriteSubtitle(output, subtitle, outputOffset, false);
                            }
                        }
                    }

                }
            }
        }

        private void CreateExeFile(string outputFile)
        {
            var peInfo = WindowsAssembly.FromFile(Path);

            using (var inputFs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(inputFs, FileEncoding, Endianness.LittleEndian))
            using (var outputFs = new FileStream(outputFile, FileMode.Create))
            {
                var output = new BinaryStreamWriter(outputFs);
                var writingContext = new WritingContext(peInfo, new BinaryStreamWriter(outputFs));

                var dosHeader = input.ReadBytes((int)peInfo.NtHeaders.StartOffset);
                output.WriteBytes(dosHeader);

                var ntHeader = peInfo.NtHeaders;
                ntHeader.FileHeader.NumberOfSections++;
                ntHeader.OptionalHeader.SizeOfImage += 0x00010000;

                ntHeader.Write(writingContext);

                var newSection = CreateTFSection(peInfo.SectionHeaders[peInfo.SectionHeaders.Count - 1], ntHeader.OptionalHeader.FileAlignment,
                    ntHeader.OptionalHeader.SectionAlignment);
                peInfo.SectionHeaders.Add(newSection);

                foreach (var section in peInfo.SectionHeaders)
                {
                    section.Write(writingContext);
                }

                foreach (var section in peInfo.SectionHeaders)
                {
                    input.Seek(section.PointerToRawData, SeekOrigin.Begin);
                    outputFs.Seek(section.PointerToRawData, SeekOrigin.Begin);

                    var data = input.ReadBytes((int)section.SizeOfRawData);
                    output.WriteBytes(data);
                }

                var bytes = new byte[0x00010000];
                output.WriteBytes(bytes);
            }
        }

        private ImageSectionHeader CreateTFSection(ImageSectionHeader previous, uint fileAlignment, uint sectionAlignment)
        {
            var realAddress = previous.PointerToRawData + previous.SizeOfRawData;
            realAddress = Align(realAddress, fileAlignment);

            var virtualAddress = previous.VirtualAddress + previous.VirtualSize;
            virtualAddress = Align(virtualAddress, sectionAlignment);

            var sectionHeader = new ImageSectionHeader
            {
                Name = ".trad",
                Attributes = ImageSectionAttributes.MemoryRead |
                             ImageSectionAttributes.ContentInitializedData,
                PointerToRawData = realAddress,
                SizeOfRawData = 0x00010000,
                VirtualAddress = virtualAddress,
                VirtualSize = 0x00010000,
            };

            return sectionHeader;
        }

        private static uint Align(uint value, uint align)
        {
            align--;
            return (value + align) & ~align;
        }

        private void RebuildFontTable(string outputFile)
        {
            if (!System.IO.File.Exists(outputFile))
            {
                System.IO.File.Copy(Path, outputFile);
            }

            var data = GetFontTable();

            using (var fs = new FileStream(outputFile, FileMode.Open))
            using (var output = new ExtendedBinaryWriter(fs))
            {
                output.Seek(FontTableOffset, SeekOrigin.Begin);

                for (var i = 0; i < 128; i++)
                {
                    output.Write(data[i].Width);
                }
            }
        }

        private void RebuildPatches(string outputFile)
        {
            if (!System.IO.File.Exists(outputFile))
            {
                System.IO.File.Copy(Path, outputFile);
            }

            using (var fs = new FileStream(outputFile, FileMode.Open))
            using (var output = new ExtendedBinaryWriter(fs, FileEncoding))
            {
                foreach (var patch in Patches)
                {
                    output.Seek(patch.Item1, SeekOrigin.Begin);
                    output.Write(patch.Item2);
                }
            }
        }

        protected override void LoadBeforeImport()
        {
            _charWidths = GetFontTable();
            base.LoadBeforeImport();
        }

        private ImageSection GetSectionByVirtualAddress(WindowsAssembly peInfo, int address)
        {
            var sections = peInfo.GetSections();
            foreach (ImageSection section in sections)
            {
                var sectionStart = peInfo.NtHeaders.OptionalHeader.ImageBase + section.Header.VirtualAddress;
                if ((sectionStart <= (ulong)address) && ((ulong)address <= sectionStart + section.Header.VirtualSize))
                {
                    return section;
                }
            }

            return null;
        }

    }
}
