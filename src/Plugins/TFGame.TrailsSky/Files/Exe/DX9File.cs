using System;
using System.Collections.Generic;

namespace TFGame.TrailsSky.Files.Exe
{
    public class DX9File : File
    {
        protected override long FontTableOffset => 0x117DB8;

        protected override Dictionary<int, List<int>> StringOffsets => new Dictionary<int, List<int>>()
        {
            {0x005163fc, new List<int>() {0x0040216f, }}, // Surprise attack!
            {0x00516410, new List<int>() {0x00402176, }}, // 背後をとられた！
            {0x00516424, new List<int>() {0x0040220f, }}, // Preemptive attack!
            {0x00516438, new List<int>() {0x00402216, }}, // 先制攻撃！
            {0x00516444, new List<int>() {0x00403208, }}, // Battle Entry Failed\n.
            {0x0051645c, new List<int>() {0x004042f4, }}, // Move
            {0x00516464, new List<int>() {0x004042fb, }}, //    移  動   
            {0x00516474, new List<int>() {0x004043b7, }}, // Attack
            {0x0051647c, new List<int>() {0x004043be, }}, // 　 攻　撃 　
            {0x005164a4, new List<int>() {0x0043bbd5, }}, // Party was defeated...
            {0x005164bc, new List<int>() {0x0043bbdc, }}, // パーティは全滅しました・・・
            {0x005164dc, new List<int>() {0x0043bbb5, }}, // Enemy fled...
            {0x005164ec, new List<int>() {0x0043bbbc, }}, // 敵に逃げられました・・・
            {0x00516508, new List<int>() {0x004084af, }}, // Party fled...
            {0x00516518, new List<int>() {0x004084b6, }}, // パーティは退却しました・・・
            {0x00516538, new List<int>() {0x0042c500, 0x0042c47f, 0x0040957f, }}, // Arts
            {0x00516540, new List<int>() {0x0040958c, }}, //    アーツ   
            {0x00516550, new List<int>() {0x00478e8b, 0x00409597, }}, // Crafts
            {0x00516558, new List<int>() {0x004095a4, }}, //   クラフト  
            {0x00516568, new List<int>() {0x004095b0, 0x004e2764, }}, // Items
            {0x00516570, new List<int>() {0x004095bd, }}, //    道  具   
            {0x00516580, new List<int>() {0x0040b4ff, }}, // Set %c%c%s%c%c as S-Break?
            {0x0051659c, new List<int>() {0x0040b506, }}, // %c%c%s%c%cをＳブレイク用に設定しますか？
            {0x005165c8, new List<int>() {0x0040b535, }}, // Tactics
            {0x005165d0, new List<int>() {0x0040b55c, }}, // Set %c%c%s%c%c as S-Break.\n\t\t\t\t\t\t\tSelect [%c%c%s%c%c] in main menu to change S-Break.
            {0x00516628, new List<int>() {0x0040b578, }}, // %c%c%s%c%cをＳブレイク用に設定しました。\n\t\t\t\t\t\t\t\tキャンプ画面の[%c%c%s%c%c]で変更出来ます。
            {0x00516690, new List<int>() {0x0041038a, }}, // %s is preparing to use arts.\n
            {0x005166b0, new List<int>() {0x0041039d, }}, // %sはアーツを使う準備をしている。\n
            {0x005166d4, new List<int>() {0x004112d7, }}, // %s fled.
            {0x005166e0, new List<int>() {0x004112e3, }}, // %sは逃げ出した。
            {0x005169ac, new List<int>() {0x00423798, }}, // %s was destroyed!\n
            {0x005169c0, new List<int>() {0x004237a4, }}, // アイテム「%s」を壊された！\n
            {0x00516a74, new List<int>() {0x0042bfbb, }}, // Battle Order Bar:
            {0x00516a88, new List<int>() {0x0042bfc3, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\tIndicates who attacks first.\n\t\t\t\t\t\t\t\t\tIt starts from the top and moves down.
            {0x00516aec, new List<int>() {0x0042bfca, }}, // ＡＴ(Action Time)バー:
            {0x00516b08, new List<int>() {0x0042bfd2, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\t行動順を表すバーです。\n\t\t\t\t\t\t\t\t\t上から順に行動が回ってきます。
            {0x00516b5c, new List<int>() {0x0042c05c, }}, // Attack:
            {0x00516b68, new List<int>() {0x0042c064, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\tAttack an enemy.\n\t\t\t\t\t\t\t\t\tYou may also use it to move, if you are\n\t\t\t\t\t\t\t\t\tusing a mouse and click an empty location.
            {0x00516bf4, new List<int>() {0x0042c07b, }}, // 攻撃：
            {0x00516c00, new List<int>() {0x0042c083, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\t敵を攻撃します。\n\t\t\t\t\t\t\t\t\t敵のいない場所をクリックすると\n\t\t\t\t\t\t\t\t\tそこまで移動することができます（マウス操作時）。
            {0x00516c88, new List<int>() {0x0042c0c0, }}, // The highlighted area indicates the distance\n\t\t\t\t\t\t\t\t\ta character can move. Selecting a target in\n\t\t\t\t\t\t\t\t\tthis area will move the character to attack.
            {0x00516d20, new List<int>() {0x0042c0d7, }}, // #0C表示される範囲は移動攻撃の届く距離です。\n\t\t\t\t\t\t\t\t\tこの範囲内にいる目標を選択すると\n\t\t\t\t\t\t\t\t\t自動的に移動して攻撃をしかけます。
            {0x00516da8, new List<int>() {0x0042c114, }}, // When an enemy is out of range,\n\t\t\t\t\t\t\t\t\t\tan     icon will appear on your cursor.\n\t\t\t\t\t\t\t\t\t\tSelecting an out of range target will\n\t\t\t\t\t\t\t\t\t\twill move the character as close to it as\n\t\t\t\t\t\t\t\t\t\tas possible, but no attack will be performed.
            {0x00516e98, new List<int>() {0x0042c11b, }}, // #0Cカーソルを重ねたときに\n\t\t\t\t\t\t\t\t\t　　が表示される目標は攻撃範囲外です。\n\t\t\t\t\t\t\t\t\t選択すると最も接近できる地点まで\n\t\t\t\t\t\t\t\t\t移動しますが、攻撃はできません。
            {0x00516f38, new List<int>() {0x0042c201, }}, // Battle Order Bonus:
            {0x00516f50, new List<int>() {0x0042c209, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\tThese icons indicate the bonuses alloted\n\t\t\t\t\t\t\t\t\tto the battle order. If a bonus icon appears\n\t\t\t\t\t\t\t\t\tnext to a character's icon, they will receive\n\t\t\t\t\t\t\t\t\tthat bonus.
            {0x00517010, new List<int>() {0x0042c220, }}, // ＡＴボーナス(行動順ボーナス)：
            {0x00517030, new List<int>() {0x0042c228, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\t　行動順についたボーナスを示すアイコンです。\n\t\t\t\t\t\t\t\t\t　これらのアイコンがあるときに\n\t\t\t\t\t\t\t\t\t　行動順が回ってくると各種のボーナスがつきます。
            {0x005170d8, new List<int>() {0x0042c265, }}, //   : Heal HP,    : Sepith Up, etc.\n\t\t\t\t\t\t\t\t\tindicate the effects of each icon.
            {0x00517128, new List<int>() {0x0042c26c, }}, // #0C   ：ＨＰ回復、   ：セピスボーナスなど\n\t\t\t\t\t\t\t\t\tアイコンごとに特定の効果があります。
            {0x00517180, new List<int>() {0x0042c3dd, }}, // Arts:
            {0x00517188, new List<int>() {0x0042c3e5, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\tArts are effective against foes which\n\t\t\t\t\t\t\t\t\tare difficult to hit with a weapon or those\n\t\t\t\t\t\t\t\t\ton which physical attacks have little effect.
            {0x00517230, new List<int>() {0x0042c3fc, }}, // アーツ：
            {0x00517240, new List<int>() {0x0042c404, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\t武器による攻撃が当たりにくい敵や\n\t\t\t\t\t\t\t\t\t攻撃の効果が薄い敵にはアーツが有効です。
            {0x005172a8, new List<int>() {0x0042c464, 0x0047d4ad, 0x0047d7df, }}, // EP
            {0x005172b0, new List<int>() {0x0042c488, }}, // It takes time before %c%c%s%c%c can be cast.\n\t\t\t\t\t\t\t\t\tAlso, %c%c%s%c%c is consumed when arts are cast.
            {0x00517318, new List<int>() {0x0042c52d, 0x0042c49f, }}, // アーツ
            {0x00517320, new List<int>() {0x0042c4a8, }}, // %c%c%s%c%cは発動までに時間（駆動時間）が掛かります。\n\t\t\t\t\t\t\t\t\tまた、使用すると%c%c%s%c%cを消費します。
            {0x00517388, new List<int>() {0x0042c4f3, }}, // Element
            {0x00517390, new List<int>() {0x0042c509, }}, // All %c%c%s%c%c have an %c%c%s%c%c. Determine the element\n\t\t\t\t\t\t\t\t\tmost effective against your foe and use it.
            {0x00517400, new List<int>() {0x0042c520, }}, // 属性
            {0x00517408, new List<int>() {0x0042c536, }}, // %c%c%s%c%cには%c%c%s%c%cがあります。\n\t\t\t\t\t\t\t\t\t相手に有効な属性を見極めて使いましょう。
            {0x00517460, new List<int>() {0x0042c5e6, }}, // Crafts:
            {0x00517468, new List<int>() {0x0042c5ee, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\tCrafts are character-specific skills which not\n\t\t\t\t\t\t\t\t\tonly deal out damage, but also have a broad range\n\t\t\t\t\t\t\t\t\tof effects.
            {0x005174fc, new List<int>() {0x0042c605, }}, // クラフト：
            {0x00517508, new List<int>() {0x0042c60d, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\tクラフトはキャラクターごとに固有の技です。\n\t\t\t\t\t\t\t\t\t攻撃だけでなく、様々な効果のものがあります。
            {0x00517580, new List<int>() {0x0042c737, 0x0042c78a, 0x0042c66d, 0x0042c67a, 0x0047d4c4, 0x0047d7f6, }}, // CP
            {0x00517584, new List<int>() {0x0042c695, }}, // crafts
            {0x00517590, new List<int>() {0x0042c69e, }}, // Using %c%c%s%c%c consumes %c%c%s%c%c.\n\t\t\t\t\t\t\t\t\t%c%c%s%c%c is gradually gained by dealing out\n\t\t\t\t\t\t\t\t\tor receiving damage in battle.
            {0x00517618, new List<int>() {0x0042c6b5, }}, // クラフト
            {0x00517628, new List<int>() {0x0042c6be, }}, // %c%c%s%c%cを使うと%c%c%s%c%cを消費します。\n\t\t\t\t\t\t\t\t\t%c%c%s%c%cは攻撃したり、ダメージを受けたりすることで\n\t\t\t\t\t\t\t\t\t戦闘中に少しずつ溜まっていきます。
            {0x005176c0, new List<int>() {0x0042c7f7, 0x0042c744, 0x00478f51, }}, // S-Crafts
            {0x005176cc, new List<int>() {0x0042c751, }}, // S-Breaks:
            {0x005176d8, new List<int>() {0x0042c759, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\tThese are actions which allow %c%c%s%c%c to be\n\t\t\t\t\t\t\t\t\timmediately unleashed while ignoring the battle\n\t\t\t\t\t\t\t\t\torder once the %c%c%s%c%c gauge has reached 0.
            {0x0051778c, new List<int>() {0x0042ca64, 0x0042c8ae, 0x0042c824, 0x0042c770, }}, // Ｓブレイク
            {0x00517798, new List<int>() {0x0042c817, 0x0042c77d, }}, // Ｓクラフト
            {0x005177a4, new List<int>() {0x0042c797, }}, // Ｓブレイク：
            {0x005177b8, new List<int>() {0x0042c79f, }}, // %c%c%s%c%c\n\t\t\t\t\t\t\t\t\t%c%c%s%c%cが100以上溜まると、ＡＴバー（行動順）を\n\t\t\t\t\t\t\t\t\t無視して%c%c%s%c%cを使用することが出来ます。\n\t\t\t\t\t\t\t\t\tこれが%c%c%s%c%cです。
            {0x00517854, new List<int>() {0x0042c7ea, }}, // S-Breaks
            {0x00517860, new List<int>() {0x0042c800, }}, // %c%c%s%c%c which will be used as %c%c%s%c%c can be changed by\n\t\t\t\t\t\t\t\t\tgoing to [Tactics] and then [Set S-Break] within the\n\t\t\t\t\t\t\t\t\tmain menu.
            {0x005178f0, new List<int>() {0x0042c82d, }}, // %c%c%s%c%cとして発動する%c%c%s%c%cの変更は\n\t\t\t\t\t\t\t\t\tキャンプ[Tactics]内の［Ｓブレイク登録］で行います。
            {0x00517958, new List<int>() {0x0042ca47, 0x0042c891, 0x0048b196, }}, // S-Break
            {0x00517960, new List<int>() {0x0042ca54, 0x0042c89e, }}, // Break Button
            {0x00517970, new List<int>() {0x0042c8a7, }}, // Press the        %c%c%s%c%c to unleash an %c%c%s%c%c.\n\t\t\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t\t\tAn S-Break cannot be unleashed\n\t\t\t\t\t\t\t\t\tunder the        condition.
            {0x00517a00, new List<int>() {0x0042ca71, 0x0042c8bb, }}, // ブレイクボタン
            {0x00517a10, new List<int>() {0x0042c8c4, }}, // %c%c%s%c%c　　　をクリックすると%c%c%s%c%cが発動します。\n\t\t\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t\t\t※ブレイクボタンが　　　になっている間は\n\t\t\t\t\t\t\t\t\t  Ｓブレイクを発動することはできません。
            {0x00517ab8, new List<int>() {0x0042ca5d, }}, // Now, press the         %c%c%s%c%c and try unleashing an %c%c%s%c%c.\n\t\t\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t\t\tIf you are using a keyboard, you may use the 1-4 number\n\t\t\t\t\t\t\t\t\tkeys, or the arrow keys to select it before unleashing it.
            {0x00517b90, new List<int>() {0x0042ca7a, }}, // #0Cでは、実際に%c%c%s%c%c　　　をクリックし\n\t\t\t\t\t\t\t\t\t%c%c%s%c%cを発動してみましょう。\n\t\t\t\t\t\t\t\t\t※キーボード操作の場合は\n\t\t\t\t\t\t\t\t\t　パーティメンバーに対応した[1][2][3][4]キーを押すか\n\t\t\t\t\t\t\t\t\t　矢印キーの左右[←][→]で選択します。
            {0x00517c78, new List<int>() {0x0042cbd2, }}, // Protect all NPCs!
            {0x00517c8c, new List<int>() {0x0042cbe9, }}, // #0CNPCを守れ！
            {0x00517c9c, new List<int>() {0x0042cc26, }}, // If an NPC's HP reaches 0, the game is over.
            {0x00517cc8, new List<int>() {0x0042cc3d, }}, // #0CNPCのHPが0になるとゲームオーバーになります。
            {0x0051818c, new List<int>() {0x00435591, }}, // Displays all items.
            {0x005181a0, new List<int>() {0x0043559b, }}, // 全てのアイテムを表示します。
            {0x005181c0, new List<int>() {0x004355be, }}, // Displays weapons.
            {0x005181d4, new List<int>() {0x004355c8, }}, // 武器を表示します。
            {0x005181e8, new List<int>() {0x004355ec, }}, // Displays armor.
            {0x005181f8, new List<int>() {0x004355f6, }}, // 防具とアクセサリーを表示します。
            {0x0051821c, new List<int>() {0x0043561a, }}, // Displays medicine.
            {0x00518230, new List<int>() {0x00435624, }}, // 薬と携帯食糧を表示します。
            {0x0051824c, new List<int>() {0x00435648, }}, // Displays quartz.
            {0x00518260, new List<int>() {0x0043564f, }}, // クオーツを表示します。
            {0x00518278, new List<int>() {0x00435670, }}, // Displays key items.
            {0x0051828c, new List<int>() {0x00435677, }}, // イベントアイテムを表示します。
            {0x005182ac, new List<int>() {0x00435698, }}, // Displays ingredients.
            {0x005182c4, new List<int>() {0x0043569f, }}, // 食材を表示します。
            {0x005182d8, new List<int>() {0x004356b8, }}, // Displays books.
            {0x005182e8, new List<int>() {0x004356c5, }}, // 書物を表示します。
            {0x005182fc, new List<int>() {0x004356ee, }}, // Removes equipment.
            {0x00518310, new List<int>() {0x00435700, }}, // 装備をはずします。
            {0x00518324, new List<int>() {0x00435788, }}, // Quartz can be installed.
            {0x00518340, new List<int>() {0x004357a3, }}, // クオーツを装着できます。
            {0x0051835c, new List<int>() {0x0043580d, }}, // Weapon can be equipped.
            {0x00518374, new List<int>() {0x00435814, }}, // 武器を装備できます。
            {0x0051838c, new List<int>() {0x0043582a, }}, // Clothing can be equipped.
            {0x005183a8, new List<int>() {0x00435831, }}, // 衣服を装備できます。
            {0x005183c0, new List<int>() {0x00435847, }}, // Footwear can be equipped.
            {0x005183dc, new List<int>() {0x0043584e, }}, // 靴を装備できます。
            {0x005183f0, new List<int>() {0x00435855, }}, // Accessory can be equipped.
            {0x0051840c, new List<int>() {0x00435862, }}, // 装飾品を装備できます。
            {0x00518524, new List<int>() {0x00435d67, }}, // Lv
            {0x00518528, new List<int>() {0x00435dc4, }}, // ◆━━━━━━━━━━━━━━━━━━━━━◆
            {0x00518558, new List<int>() {0x00435e03, }}, // Exp
            {0x00518564, new List<int>() {0x00435ea2, 0x005312bc, }}, // Next
            {0x00518574, new List<int>() {0x0043631f, }}, //   Item
            {0x00518580, new List<int>() {0x0043633d, 0x00436459, }}, // ◆━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━◆
            {0x005185e0, new List<int>() {0x0043638e, }}, // Got 
            {0x005185e8, new List<int>() {0x004363aa, }}, // Got
            {0x005185ec, new List<int>() {0x00436402, }}, //  を手に入れた
            {0x005185fc, new List<int>() {0x0043641d, }}, //   Craft
            {0x00518604, new List<int>() {0x00436424, }}, //   S-Craft
            {0x00518610, new List<int>() {0x00436431, }}, //   SCraft
            {0x0051861c, new List<int>() {0x00436497, }}, // %s learned 
            {0x0051862c, new List<int>() {0x0043655d, }}, // %sは 
            {0x00518634, new List<int>() {0x004365fb, }}, //  を習得した。
            {0x00518644, new List<int>() {0x00436885, 0x004a026f, }}, //   Sepith
            {0x00518650, new List<int>() {0x004368a6, }}, // ◆━━━━━━━◆
            {0x005187cc, new List<int>() {0x0043af81, 0x0043b447, 0x0043b818, 0x004a3fea, 0x004a43a1, 0x004a44fa, }}, // ◆━━━━━━━━━━━━━━◆
            {0x005187f0, new List<int>() {0x0043afa2, }}, // HP:
            {0x00518808, new List<int>() {0x0043b024, }}, // Condition:
            {0x00518814, new List<int>() {0x0043b0e1, 0x0043b6fb, 0x004929d5, 0x00492588, 0x00492583, 0x004929d0, }}, // なし
            {0x0051881c, new List<int>() {0x0043b0fc, 0x0043b716, }}, // None
            {0x00518824, new List<int>() {0x0043b170, }}, // Elemental Efficacy (%):
            {0x0051883c, new List<int>() {0x0043b177, }}, // 属性攻撃有効率(%):
            {0x00518854, new List<int>() {0x0043b379, }}, // CP:
            {0x00518860, new List<int>() {0x0043b4a6, }}, // Exp:
            {0x00518870, new List<int>() {0x0043b580, }}, // Sepith:
            {0x0051887c, new List<int>() {0x0043b68e, }}, // Item:
            {0x005189a4, new List<int>() {0x00590c6c, }}, // Title Screen
            {0x005189b4, new List<int>() {0x00590c68, }}, // Retry
            {0x005189bc, new List<int>() {0x00590c64, }}, // タイトルに戻る
            {0x005189cc, new List<int>() {0x00590c60, }}, // リトライ
            {0x00519bd0, new List<int>() {0x0046f7a8, }}, // Save failed. Check your save folder.\nCurrent save folder path:\n%s
            {0x00519c18, new List<int>() {0x0046f7c0, }}, // セーブに失敗しました。セーブフォルダを確認して下さい。\nセーブファイルのパスは以下の通り\n%s
            {0x00519c7c, new List<int>() {0x004710b2, }}, // Learned recipe for [%s].
            {0x00519c98, new List<int>() {0x004710b9, 0x004c44b2, }}, // 『%s』のレシピを覚えた。
            {0x00519d14, new List<int>() {0x00531244, 0x00591278, }}, // Discard
            {0x00519d1c, new List<int>() {0x00531240, 0x00591274, }}, // Use
            {0x00519d20, new List<int>() {0x00591270, }}, // 　捨てる　
            {0x00519d2c, new List<int>() {0x0059126c, }}, // 　使　う　
            {0x00519d3c, new List<int>() {0x00473983, }}, // Select a save file.
            {0x00519d50, new List<int>() {0x0047399c, }}, // Select a file to load.
            {0x00519d68, new List<int>() {0x004739b5, }}, // Erase save data.
            {0x00519d7c, new List<int>() {0x004739ce, }}, // Exit game.
            {0x00519d88, new List<int>() {0x004739e6, }}, // セーブするファイルを選んでください。
            {0x00519db0, new List<int>() {0x004739ff, }}, // ロードするファイルを選んでください。
            {0x00519dd8, new List<int>() {0x00473a18, }}, // 削除するファイルを選んでください。
            {0x00519dfc, new List<int>() {0x00473a31, }}, // ゲームを終了します。
            {0x00519e14, new List<int>() {0x004745ef, }}, // %cTactics mode cannot currently be utilized.
            {0x00519e44, new List<int>() {0x00475776, }}, // %cPosition cannot currently be changed.
            {0x00519ea4, new List<int>() {0x004762bc, }}, // %cAuto Save slot. Slot cannot be used.
            {0x00519ecc, new List<int>() {0x004762f9, }}, // Overwrite?
            {0x00519ed8, new List<int>() {0x004761cb, }}, // Load clear\ndata file?
            {0x00519ef0, new List<int>() {0x00476215, }}, // Load file?
            {0x00519efc, new List<int>() {0x00476148, }}, // Delete this file?
            {0x00519f10, new List<int>() {0x00476ecc, }}, // Select who will use the item.
            {0x00519f30, new List<int>() {0x00476ed3, }}, // アイテムを使う相手を選んでください。
            {0x00519f58, new List<int>() {0x00476fb8, }}, // Discard this item?
            {0x00519f6c, new List<int>() {0x00477024, }}, // このアイテムを捨てますか？
            {0x00519f88, new List<int>() {0x004770ac, 0x00477131, }}, // Select on whom the arts will be used.
            {0x00519fb0, new List<int>() {0x004770b3, 0x00477138, }}, // アーツを使う相手を選んでください。
            {0x00519fd4, new List<int>() {0x00478ed3, 0x004a4807, }}, // ◆━━━━
            {0x00519fe0, new List<int>() {0x00478f0c, 0x004a4830, }}, // ━━━━◆
            {0x00519fec, new List<int>() {0x004a3e78, 0x0047d207, 0x0047d3d8, 0x0047e8f8, 0x004a4ad2, 0x00478f91, 0x00498585, }}, // ◆━━━
            {0x00519ff8, new List<int>() {0x004a3ebe, 0x0047d229, 0x0047d3fa, 0x0047e91a, 0x004a4af8, 0x00478fcc, 0x004985a7, }}, // ━━━◆
            {0x0051a004, new List<int>() {0x0047900b, }}, //       Crafts
            {0x0051a014, new List<int>() {0x00479029, 0x0047e9b0, }}, // ◆━━━            ━━━━◆
            {0x0051a034, new List<int>() {0x0047904a, }}, //      S-Crafts
            {0x0051a044, new List<int>() {0x00479068, }}, // ◆━━                ━━━◆
            {0x0051a708, new List<int>() {0x0047c474, }}, // HP      /%4d
            {0x0051a720, new List<int>() {0x0047c4d7, }}, // EP  %4d/%4d
            {0x0051a7d0, new List<int>() {0x0047d17a, }}, // Name
            {0x0051a7d8, new List<int>() {0x0047d34b, 0x004e28ed, }}, // Status
            {0x0051a7e0, new List<int>() {0x0047d481, 0x0047d7b3, 0x0048769b, }}, // Level
            {0x0051a7e8, new List<int>() {0x0047d496, 0x0047d7c8, 0x004a811a, }}, // HP
            {0x0051a814, new List<int>() {0x0047d593, }}, // Parameters
            {0x0051a820, new List<int>() {0x0047d620, 0x0047e04a, 0x00498748, 0x00498a61, }}, // ◆━━
            {0x0051a828, new List<int>() {0x0047d642, 0x0047e078, 0x0049876a, 0x00498a83, }}, // ━━◆
            {0x0051a830, new List<int>() {0x0047d6cb, }}, //       Name
            {0x0051a83c, new List<int>() {0x0047d6e9, }}, // ◆━━━        ━━━◆
            {0x0051a858, new List<int>() {0x0047d774, }}, //      Status
            {0x0051a864, new List<int>() {0x0047d792, 0x0047d8f2, 0x0047e19b, 0x0049896a, }}, // ◆━                ━◆
            {0x0051a880, new List<int>() {0x0047d8d2, }}, //    Parameter
            {0x0051a890, new List<int>() {0x0047dae7, 0x004a403d, }}, // STR
            {0x0051a894, new List<int>() {0x0047daff, 0x004a4056, }}, // DEF
            {0x0051a898, new List<int>() {0x0047db19, 0x004a406f, }}, // ATS
            {0x0051a89c, new List<int>() {0x0047db33, 0x004a4088, }}, // ADF
            {0x0051a8a0, new List<int>() {0x0047db4d, 0x004a40a1, }}, // SPD
            {0x0051a8a4, new List<int>() {0x0047db67, 0x004a40b7, }}, // DEX
            {0x0051a8a8, new List<int>() {0x0047db81, 0x004a40cd, }}, // AGL
            {0x0051a8ac, new List<int>() {0x0047db9b, 0x004a40e6, }}, // MOV
            {0x0051a8b0, new List<int>() {0x0047dbb5, 0x004a40ff, }}, // RNG
            {0x0051a8c4, new List<int>() {0x0047df78, }}, // Experience
            {0x0051a8d0, new List<int>() {0x0047e133, }}, //    Experience
            {0x0051a8e0, new List<int>() {0x0047e1f7, 0x0047e273, }}, // EXP
            {0x0051a8e4, new List<int>() {0x0047e217, 0x0047e293, }}, // NEXT
            {0x0051a96c, new List<int>() {0x0047e86b, 0x004e27e7, }}, // Equipment
            {0x0051a978, new List<int>() {0x0047e992, }}, //       Equips
            {0x0051a988, new List<int>() {0x0047f449, }}, // Unequip
            {0x0051aa94, new List<int>() {0x004853e3, }}, // Autosave
            {0x0051aaa0, new List<int>() {0x004853fc, }}, // Page %2d
            {0x0051aaac, new List<int>() {0x00485615, }}, // ◆━━━━━━━━━━━━━━━━━━━━━━━━━━━━━◆
            {0x0051ab5c, new List<int>() {0x0048578b, 0x0048577f, 0x005311f8, }}, // Clear Data - Trails in the Sky
            {0x0051ab7c, new List<int>() {0x0048578b, 0x00485786, 0x005311e0, }}, // クリアデータ 空の軌跡ＦＣ
            {0x0051aba4, new List<int>() {0x004877a6, 0x004857a8, 0x004e2a01, 0x004e29fc, }}, // ノーマル
            {0x0051abb0, new List<int>() {0x004877ae, 0x004857b0, 0x004e29e7, 0x004e29e2, }}, // ハード
            {0x0051abb8, new List<int>() {0x004877b6, 0x004857b8, 0x004e29c4, 0x004e29bf, }}, // ナイトメア
            {0x0051abc4, new List<int>() {0x004877be, 0x004857c0, 0x004e2a01, 0x004e29a6, }}, // イージー
            {0x0051abd0, new List<int>() {0x004877c6, 0x004857c8, 0x004e2a01, 0x004e29ef, }}, // Normal
            {0x0051abd8, new List<int>() {0x004877ce, 0x004857d0, 0x004e29e7, 0x004e29d5, }}, // Hard
            {0x0051abe0, new List<int>() {0x004877d6, 0x004857d8, 0x004e29c4, 0x004e29b8, }}, // Nightmare
            {0x0051abec, new List<int>() {0x004877de, 0x004857e0, 0x004e2a01, 0x004e299f, }}, // Easy
            {0x0051abf4, new List<int>() {0x004857fd, 0x004857f1, }}, // Unknown
            {0x0051ac28, new List<int>() {0x004873b0, }}, // ◆━━━━━━━━━━━━━━━━━━━━━━━━━━━━◆
            {0x0051ac68, new List<int>() {0x004873e9, }}, // SaveTime: 
            {0x0051ac74, new List<int>() {0x004873f0, }}, // SaveTime：
            {0x0051ac80, new List<int>() {0x0048741b, }}, // PlayTime: 
            {0x0051ac8c, new List<int>() {0x00487422, }}, // PlayTime：
            {0x0051ac98, new List<int>() {0x00487824, }}, // Unknown:
            {0x0051ad1c, new List<int>() {0x00487f66, }}, // Are you sure\nyou wish to finish\nsaving Clear Data?
            {0x0051ad50, new List<int>() {0x0048807f, }}, // %cAuto save slot.\nYou can't save here.
            {0x0051ad78, new List<int>() {0x004880a6, }}, // Overwrite file?
            {0x0051af34, new List<int>() {0x00488606, }}, // OK
            {0x0051af38, new List<int>() {0x00488629, }}, // CANCEL
            {0x0051afe4, new List<int>() {0x00531db4, }}, // Set S-Break
            {0x0051aff0, new List<int>() {0x00531db0, }}, // Battle Position
            {0x0051b000, new List<int>() {0x00531dac, }}, // Character Position
            {0x0051b014, new List<int>() {0x00531da8, }}, // Ｓブレイク登録
            {0x0051b024, new List<int>() {0x00531da4, }}, // 戦闘隊列変更
            {0x0051b034, new List<int>() {0x00531da0, }}, // 移動隊列変更
            {0x0051b044, new List<int>() {0x0048a3af, }}, // Change character position.
            {0x0051b060, new List<int>() {0x0048a3c3, }}, // 移動時の隊列を変更します。
            {0x0051b07c, new List<int>() {0x0048a388, }}, // Change battle formation.
            {0x0051b098, new List<int>() {0x0048a397, }}, // 戦闘用の隊列を変更します。
            {0x0051b0b4, new List<int>() {0x0048a35d, }}, // Set S-Break.
            {0x0051b0c4, new List<int>() {0x0048a36c, }}, // Ｓブレイクの登録をします。
            {0x0051b0e0, new List<int>() {0x0048a6d7, }}, // ◆━━━━━
            {0x0051b0f0, new List<int>() {0x0048a6fd, }}, // ━━━━━◆
            {0x0051b100, new List<int>() {0x0048a77b, }}, // Party Order
            {0x0051b10c, new List<int>() {0x0048a805, }}, // ◆━━━━                   ━━━━◆
            {0x0051b134, new List<int>() {0x0048a826, }}, //         Walk Place
            {0x0051b148, new List<int>() {0x0048bb01, 0x0048b0f2, }}, // ◆━━━━━━━━━━━
            {0x0051b164, new List<int>() {0x0048bb27, 0x0048b118, }}, // ━━━━━━━━━━━◆
            {0x0051b180, new List<int>() {0x0048b220, 0x0048bc39, }}, // ◆━━━━━━━━━                   ━━━━━━━━━◆
            {0x0051b1bc, new List<int>() {0x0048b241, }}, //                 S-Break
            {0x0051b1f0, new List<int>() {0x0048bba5, }}, // Formation
            {0x0051b1fc, new List<int>() {0x0048bc5a, }}, //                Formation
            {0x0051b4b4, new List<int>() {0x00531d80, }}, // Cafe
            {0x0051b4bc, new List<int>() {0x00531d7c, }}, // Septian Church
            {0x0051b4cc, new List<int>() {0x00531d78, }}, // Restaurant/Inn
            {0x0051b4dc, new List<int>() {0x00531d74, }}, // Orbment Factory
            {0x0051b4ec, new List<int>() {0x00531d70, }}, // Bracer Guild
            {0x0051b4fc, new List<int>() {0x00531d6c, }}, // Lodgings
            {0x0051b508, new List<int>() {0x00531d68, }}, // General Goods
            {0x0051b518, new List<int>() {0x00531d64, }}, // Arms & Guards
            {0x0051b530, new List<int>() {0x00531d40, }}, // 飲食・喫茶
            {0x0051b53c, new List<int>() {0x00531d3c, }}, // 七耀教会
            {0x0051b548, new List<int>() {0x00531d38, }}, // 食事・休憩
            {0x0051b554, new List<int>() {0x00531d34, }}, // オーブメント
            {0x0051b564, new List<int>() {0x00531d30, }}, // 遊撃士協会
            {0x0051b570, new List<int>() {0x00531d2c, }}, // 休憩・宿泊
            {0x0051b57c, new List<int>() {0x00531d28, }}, // 薬・雑貨・食材
            {0x0051b58c, new List<int>() {0x00531d24, }}, // 武器・防具
            {0x0051b598, new List<int>() {0x0048f720, 0x0048f71b, }}, // Julia
            {0x0051b5a0, new List<int>() {0x0048f749, }}, // Captain Schwarz
            {0x0051b5b0, new List<int>() {0x0048f773, 0x0048f76e, }}, // Mueller
            {0x0051b5b8, new List<int>() {0x0048f79c, }}, // Major Vander
            {0x0051b684, new List<int>() {0x00492366, }}, // ■Status
            {0x0051b690, new List<int>() {0x0049236d, }}, // ■ステータス
            {0x0051b6a0, new List<int>() {0x004923a1, 0x00492395, }}, // Visible
            {0x0051b6a8, new List<int>() {0x004923a1, 0x0049239c, }}, // 表示する
            {0x0051b6b4, new List<int>() {0x00492460, 0x004923bb, 0x004923af, 0x0049244e, }}, // Invisible
            {0x0051b6c0, new List<int>() {0x00492460, 0x004923bb, 0x004923b6, 0x0049245b, }}, // 表示しない
            {0x0051b6cc, new List<int>() {0x004923e9, }}, // ■Mini Map
            {0x0051b6d8, new List<int>() {0x004923f0, }}, // ■ミニマップ
            {0x0051b6e8, new List<int>() {0x00492427, 0x00492415, }}, // Fixed north
            {0x0051b6f4, new List<int>() {0x00492427, 0x00492422, }}, // 北を上で固定
            {0x0051b704, new List<int>() {0x00492446, 0x00492434, }}, // Rotate
            {0x0051b70c, new List<int>() {0x00492446, 0x00492441, }}, // 画面回転と同期
            {0x0051b71c, new List<int>() {0x0049248e, }}, // ■Camera
            {0x0051b728, new List<int>() {0x00492495, }}, // ■カメラ回転
            {0x0051b738, new List<int>() {0x004924c8, 0x004924bc, }}, // 45 degree
            {0x0051b744, new List<int>() {0x004924c8, 0x004924c3, }}, // 45度単位
            {0x0051b750, new List<int>() {0x004924e3, 0x004924d7, }}, // Free rotation
            {0x0051b760, new List<int>() {0x004924e3, 0x004924de, }}, // フリー回転
            {0x0051b76c, new List<int>() {0x00492511, }}, // ■Menu Button
            {0x0051b77c, new List<int>() {0x00492518, }}, // ■ボタン機能
            {0x0051b78c, new List<int>() {0x0049254f, 0x0049253d, }}, // Click
            {0x0051b794, new List<int>() {0x0049254f, 0x0049254a, }}, // クリック
            {0x0051b7a0, new List<int>() {0x0049256e, 0x0049255c, }}, // Double click
            {0x0051b7b0, new List<int>() {0x0049256e, 0x00492569, }}, // ダブルクリック
            {0x0051b7c0, new List<int>() {0x00492abb, 0x004929d5, 0x00492a32, 0x0049291b, 0x00492885, 0x0049286a, 0x004926e3, 0x00492588, 0x00492576, 0x004926d7, 0x0049277d, 0x0049285e, 0x0049290f, 0x004929c9, 0x00492a26, 0x00492aaf, }}, // Off
            {0x0051b7c8, new List<int>() {0x004e2a3c, 0x004925c3, 0x00492724, }}, // ◆━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━◆
            {0x0051b80c, new List<int>() {0x004925e0, }}, // ■Frame Rate
            {0x0051b81c, new List<int>() {0x004925e7, }}, // ■フレームレート
            {0x0051b830, new List<int>() {0x0049261b, 0x0049260f, }}, // 60 FPS Limit
            {0x0051b840, new List<int>() {0x0049261b, 0x00492616, }}, // 60FPS固定
            {0x0051b84c, new List<int>() {0x0049263e, 0x0049262c, }}, // 30 FPS Limit
            {0x0051b85c, new List<int>() {0x0049263e, 0x00492639, }}, // 30FPS固定
            {0x0051b868, new List<int>() {0x00492658, 0x00492646, }}, // Automatic
            {0x0051b874, new List<int>() {0x00492658, 0x00492653, }}, // 自動
            {0x0051b87c, new List<int>() {0x00492689, }}, // ■Transparency
            {0x0051b88c, new List<int>() {0x00492690, }}, // ■キャラ透過
            {0x0051b89c, new List<int>() {0x00492ad5, 0x00492a4d, 0x00492936, 0x00492885, 0x004926c9, 0x004926bd, 0x00492879, 0x0049292a, 0x00492a41, 0x00492ac9, }}, // On
            {0x0051b8a0, new List<int>() {0x004926c9, 0x004926c4, }}, // 透過する
            {0x0051b8ac, new List<int>() {0x004926e3, 0x004926de, }}, // 透過しない
            {0x0051b8b8, new List<int>() {0x00492741, }}, // ■Background Music
            {0x0051b8cc, new List<int>() {0x00492748, }}, // ■ＢＧＭ
            {0x0051b8d8, new List<int>() {0x00492885, 0x0049291b, 0x00492a32, 0x0049286a, 0x00492788, 0x00492865, 0x00492916, 0x00492a2d, }}, // オフ
            {0x0051b8e0, new List<int>() {0x004927ae, 0x004927a2, }}, // Original
            {0x0051b8ec, new List<int>() {0x004927ae, 0x004927a9, }}, // オリジナル
            {0x0051b928, new List<int>() {0x0049283e, 0x0049282a, }}, // Unnamed Pack %d
            {0x0051b938, new List<int>() {0x0049283e, 0x00492831, }}, // ＢＧＭセット%d
            {0x0051b948, new List<int>() {0x00492a4d, 0x00492936, 0x00492885, 0x00492880, 0x00492931, 0x00492a48, }}, // オン
            {0x0051b950, new List<int>() {0x004928b6, }}, // 　BGM Volume
            {0x0051b960, new List<int>() {0x004928bd, }}, // 　ＢＧＭ音量
            {0x0051b970, new List<int>() {0x004928de, }}, // ■Sound Effects
            {0x0051b980, new List<int>() {0x004928e5, }}, // ■効果音
            {0x0051b98c, new List<int>() {0x00492967, }}, // 　SE Volume
            {0x0051b998, new List<int>() {0x0049296e, }}, // 　効果音音量
            {0x0051b9a8, new List<int>() {0x0049298f, }}, // ■Voices
            {0x0051b9b4, new List<int>() {0x00492996, }}, // ■戦闘ボイス
            {0x0051b9c4, new List<int>() {0x00492a4d, 0x004929ed, }}, // Japanese
            {0x0051b9d0, new List<int>() {0x00492a4d, 0x004929f4, }}, // 日本語
            {0x0051b9d8, new List<int>() {0x00492a0e, 0x00492a02, }}, // English
            {0x0051b9e0, new List<int>() {0x00492a0e, 0x00492a09, }}, // 英語
            {0x0051b9e8, new List<int>() {0x00492a7e, }}, // ■Retry Offset
            {0x0051b9f8, new List<int>() {0x00492a85, }}, // ■リトライ補正
            {0x0051ba08, new List<int>() {0x00492abb, 0x00492ab6, }}, // 補正しない
            {0x0051ba14, new List<int>() {0x00492ad5, 0x00492ad0, }}, // 補正する
            {0x0051ba20, new List<int>() {0x00492b03, }}, // ■Orbment Style
            {0x0051ba30, new List<int>() {0x00492b0a, }}, // ■導力器スタイル
            {0x0051ba44, new List<int>() {0x00492b49, 0x00492b37, }}, // Colored Lines
            {0x0051ba54, new List<int>() {0x00492b49, 0x00492b44, }}, // カラーライン
            {0x0051ba64, new List<int>() {0x00492b67, 0x00492b55, }}, // Powered Lines
            {0x0051ba74, new List<int>() {0x00492b67, 0x00492b62, }}, // グローライン
            {0x0051ba84, new List<int>() {0x00492b81, 0x00492b6f, }}, // Classic
            {0x0051ba8c, new List<int>() {0x00492b81, 0x00492b7c, }}, // クラシック
            {0x0051ba98, new List<int>() {0x004933d3, }}, // Change status display shown on the game screen.
            {0x0051bac8, new List<int>() {0x004933e8, }}, // ゲーム画面のステータス表示の切り替えをします。
            {0x0051baf8, new List<int>() {0x0049340b, }}, // Change mini map display shown on the game screen.
            {0x0051bb2c, new List<int>() {0x00493420, }}, // ゲーム画面のミニマップ表示の切り替えをします。
            {0x0051bb5c, new List<int>() {0x00493443, }}, // Change game camera settings.
            {0x0051bb7c, new List<int>() {0x00493458, }}, // ゲーム画面の回転方式の切り替えをします。
            {0x0051bba8, new List<int>() {0x0049347b, }}, // Change main menu button.
            {0x0051bbc4, new List<int>() {0x00493490, }}, // キャンプボタンの切り替えをします。
            {0x0051bbe8, new List<int>() {0x004934b3, }}, // Change frame rate.
            {0x0051bbfc, new List<int>() {0x004934c8, }}, // フレームレートの固定化を切り替えます。
            {0x0051bc24, new List<int>() {0x004934eb, }}, // Change character transparency.
            {0x0051bc44, new List<int>() {0x00493500, }}, // キャラの透過処理を切り替えます。
            {0x0051bc68, new List<int>() {0x00493527, }}, // Turn background music on or off.
            {0x0051bc90, new List<int>() {0x0049353f, }}, // Change BGM pack. Pack folders start from BGM1 onward.\nPack names can be set by adding a packname.txt file.
            {0x0051bcfc, new List<int>() {0x00493597, }}, // Change background music pack.
            {0x0051bd1c, new List<int>() {0x004935b3, }}, // ＢＧＭのＯＮ／ＯＦＦを切り替えます。
            {0x0051bd44, new List<int>() {0x004935d6, }}, // Change background music volume.
            {0x0051bd64, new List<int>() {0x004935eb, }}, // ＢＧＭの音量を変更します。
            {0x0051bd80, new List<int>() {0x0049360e, }}, // Turn sound effects on or off.
            {0x0051bda0, new List<int>() {0x00493623, }}, // 効果音のＯＮ／ＯＦＦを切り替えます。
            {0x0051bdc8, new List<int>() {0x00493646, }}, // Change sound effect volume.
            {0x0051bde4, new List<int>() {0x0049365b, }}, // 効果音の音量を変更します。
            {0x0051be00, new List<int>() {0x00493687, }}, // Change battle voice language.
            {0x0051be20, new List<int>() {0x0049369c, }}, // Turn battle voices on or off.
            {0x0051be40, new List<int>() {0x004936ba, }}, // 戦闘ボイス言語の切り替えをします。
            {0x0051be64, new List<int>() {0x004936cf, }}, // 戦闘ボイスオンオフの切り替えをします。
            {0x0051be90, new List<int>() {0x004936f2, }}, // Enemies become weaker based on number of retries.\nFeel free to use this function if you can't win a battle.
            {0x0051bf00, new List<int>() {0x00493707, }}, // 戦闘をリトライした回数に応じて敵が弱体化します。\nどうしても勝てない時にどうぞ。
            {0x0051bf50, new List<int>() {0x0049372a, }}, // Change orbment line display style.
            {0x0051bf74, new List<int>() {0x0049373f, }}, // 導力器ライン表示の切り替えをします。
            {0x0051bf9c, new List<int>() {0x00495982, }}, // ◆━━━━━━━━
            {0x0051bfb0, new List<int>() {0x004959a8, }}, // ━━━━━━━━◆
            {0x0051bfc4, new List<int>() {0x00495a26, }}, // Slots
            {0x0051bfcc, new List<int>() {0x00495ab0, }}, // ◆━━━━━━              ━━━━━━◆
            {0x0051bff8, new List<int>() {0x00495ad1, }}, //            Slots
            {0x0051c00c, new List<int>() {0x00495bc5, }}, // ◆━━━━━━
            {0x0051c01c, new List<int>() {0x00495bf1, }}, // ━━━━━━◆
            {0x0051c02c, new List<int>() {0x00495ca7, }}, // Orbal Arts
            {0x0051c038, new List<int>() {0x00495d6d, }}, // ◆━━━━━                  ━━━━━◆
            {0x0051c064, new List<int>() {0x00495dcc, }}, //          Orbal Arts
            {0x0051c078, new List<int>() {0x00495ea2, }}, // %3dEP
            {0x0051c080, new List<int>() {0x00496ec0, }}, // Remove quartz
            {0x0051c090, new List<int>() {0x00496ecd, }}, // クオーツをはずす
            {0x0051c0a4, new List<int>() {0x00497a0d, }}, // Line %d
            {0x0051c24c, new List<int>() {0x004984f8, 0x004989d4, }}, // Mira
            {0x0051c254, new List<int>() {0x004986bb, }}, // PlayTime
            {0x0051c278, new List<int>() {0x004988d2, }}, //      Mira  
            {0x0051c284, new List<int>() {0x004988f0, }}, // ◆━━            ━━◆
            {0x0051c2a8, new List<int>() {0x0049894c, }}, //    PlayTime
            {0x0051c2c0, new List<int>() {0x00498b1a, }}, //     Mira  
            {0x0051c2cc, new List<int>() {0x00498b38, }}, // ◆━          ━◆
            {0x0051c47c, new List<int>() {0x005914d8, }}, // [Nightmare] It's your funeral. May Aidios be with you.
            {0x0051c4b8, new List<int>() {0x005914d4, }}, // [Hard] A difficult setting. For those who find normal a bit lacking.
            {0x0051c500, new List<int>() {0x005914d0, }}, // [Normal] Average difficulty. For those wanting a little challenge.
            {0x0051c548, new List<int>() {0x005914cc, }}, // 【ナイトメア】まさに悪夢。各種データを引き継がないと厳しいバランスです。
            {0x0051c594, new List<int>() {0x005914c8, }}, // 【ハード】やや厳しいバランスです。ノーマルでは物足りない方に。
            {0x0051c5d4, new List<int>() {0x005914c4, }}, // 【ノーマル】標準的なバランスです。適度な刺激を楽しみたい方に。
            {0x0051c6e8, new List<int>() {0x0049dac6, }}, // 　 Bracer Guild\n　 Lodgings\n　 Orbal Factory\n　 Septian Church\n　 Various Shops
            {0x0051c738, new List<int>() {0x0049dacd, }}, // 　 遊撃士協会\n　 ホテル、宿酒場\n　 オーブメント工房\n　 七耀教会\n　 各種ショップ
            {0x0051c8e4, new List<int>() {0x00531c08, }}, // Mirage sepith
            {0x0051c8f4, new List<int>() {0x00531c04, }}, // Space sepith
            {0x0051c904, new List<int>() {0x00531c00, 0x00531c0c, 0x00531c10, }}, // Time sepith
            {0x0051c910, new List<int>() {0x00531bfc, }}, // Wind sepith
            {0x0051c91c, new List<int>() {0x00531bf8, }}, // Fire sepith
            {0x0051c928, new List<int>() {0x00531bf4, }}, // Water sepith
            {0x0051c938, new List<int>() {0x00531bf0, }}, // Earth sepith
            {0x0051c948, new List<int>() {0x00531be4, }}, // 幻のセピス
            {0x0051c954, new List<int>() {0x00531be0, }}, // 空のセピス
            {0x0051c960, new List<int>() {0x00531bdc, 0x00531be8, 0x00531bec, }}, // 時のセピス
            {0x0051c96c, new List<int>() {0x00531bd8, }}, // 風のセピス
            {0x0051c978, new List<int>() {0x00531bd4, }}, // 火のセピス
            {0x0051c984, new List<int>() {0x00531bd0, }}, // 水のセピス
            {0x0051c990, new List<int>() {0x00531bcc, }}, // 地のセピス
            {0x0051c99c, new List<int>() {0x005915a0, 0x00591620, 0x00591644, }}, // Quit
            {0x0051c9a4, new List<int>() {0x0059159c, 0x00591640, }}, // Trade
            {0x0051c9ac, new List<int>() {0x00591598, }}, // Quartz
            {0x0051c9b4, new List<int>() {0x00591594, }}, // Slot
            {0x0051c9bc, new List<int>() {0x00591590, }}, // や め る
            {0x0051c9c8, new List<int>() {0x0059158c, }}, // 換    金
            {0x0051c9d4, new List<int>() {0x00591588, }}, // クオーツ
            {0x0051c9e0, new List<int>() {0x00591584, }}, // スロット
            {0x0051c9ec, new List<int>() {0x0049e4c1, }}, // Open a slot.
            {0x0051c9fc, new List<int>() {0x0049e4d0, }}, // スロットの開封をします。
            {0x0051ca18, new List<int>() {0x0049e49a, }}, // Synthesize quartz.
            {0x0051ca2c, new List<int>() {0x0049e4a9, }}, // クオーツの合成をします。
            {0x0051ca48, new List<int>() {0x0049e474, }}, // Exchange sepith for mira.
            {0x0051ca64, new List<int>() {0x0049e483, 0x004a1964, }}, // セピスをお金に換金します。
            {0x0051ca80, new List<int>() {0x0049e44e, }}, // End transaction.
            {0x0051ca94, new List<int>() {0x0049e45d, 0x004a19bc, 0x004a32af, }}, // 何をしますか？
            {0x0051caa4, new List<int>() {0x0049eb94, 0x0049eb8e, 0x00531248, }}, // 開封済み
            {0x0051cab0, new List<int>() {0x0049ebfa, 0x0049ebf4, 0x0053124c, }}, // 開封可能
            {0x0051cabc, new List<int>() {0x0049eae5, 0x0049eadf, 0x00531250, }}, // 開封不可
            {0x0051cac8, new List<int>() {0x00472fe6, 0x00531254, }}, // 未開封
            {0x0051cadc, new List<int>() {0x0049eb94, 0x0049eb7c, }}, // Already Open
            {0x0051caec, new List<int>() {0x0049ebfa, 0x0049ebe2, }}, // Openable
            {0x0051caf8, new List<int>() {0x0049eae5, 0x0049eacd, }}, // Not Openable
            {0x0051cb08, new List<int>() {0x0049f5c0, }}, // %cOpened slot [%d].
            {0x0051cb1c, new List<int>() {0x0049f5d4, }}, // %cスロット[ %d ]を開封しました。
            {0x0051cb40, new List<int>() {0x0049f601, 0x0049f817, }}, // %cInsufficient sepith.
            {0x0051cb58, new List<int>() {0x0049f60d, 0x0049f823, }}, // %cセピスが足りません。
            {0x0051cb70, new List<int>() {0x0049f6ee, }}, // %c#0CSynthesize #3C%s#0C?
            {0x0051cb8c, new List<int>() {0x0049f6f5, }}, // %c#3C%s#0Cを合成します。よろしいですか？
            {0x0051cbb8, new List<int>() {0x0049f7af, }}, // %c#0CSynthesized #3C%s#0C.
            {0x0051cbd4, new List<int>() {0x0049f7c8, }}, // %c#3C%s#0Cを合成しました。
            {0x0051cbf0, new List<int>() {0x0049f8c1, }}, // %cTrade sepith for %d mira?
            {0x0051cc0c, new List<int>() {0x0049f8cd, }}, // %cセピスを%dミラに換金します。よろしいですか？
            {0x0051cc3c, new List<int>() {0x0049fa07, }}, // %cTraded sepith for %d mira.
            {0x0051cc5c, new List<int>() {0x0049fa13, }}, // %cセピスを%dミラに換金しました。
            {0x0051cc80, new List<int>() {0x0049fcbe, }}, // Choose a slot to open.
            {0x0051cc98, new List<int>() {0x0049fcca, }}, // 開封したいスロットを選択してください。
            {0x0051ccc0, new List<int>() {0x0049fc2f, }}, // Slot [%d] can be opened.
            {0x0051cce0, new List<int>() {0x0049fc3d, }}, // Slot [%d] cannot be opened because the circuit\nis not connected.
            {0x0051cd24, new List<int>() {0x0049fc4b, }}, // Slot [%d] has already been opened.
            {0x0051cd48, new List<int>() {0x0049fc61, }}, // スロット[ %d ]の開封を行えます。
            {0x0051cd70, new List<int>() {0x0049fc6f, }}, // スロット[ %d ]は動力回路が接続していないため、\n開封できません。
            {0x0051cdb0, new List<int>() {0x0049fc7d, }}, // スロット[ %d ]はすでに開封済みです。
            {0x0051cdd8, new List<int>() {0x0049fd35, }}, // Select a character.
            {0x0051cdec, new List<int>() {0x0049fd44, }}, // キャラクターを選択してください。
            {0x0051ce10, new List<int>() {0x0049fdac, 0x004a1d2f, }}, // Inventory: ------
            {0x0051ce24, new List<int>() {0x0049fdb3, }}, // 現在の所持数：------
            {0x0051ce3c, new List<int>() {0x0049fdd8, }}, // Choose a quartz to synthesize.
            {0x0051ce5c, new List<int>() {0x0049fdf9, }}, // 合成したいクオーツを選択してください。
            {0x0051ce84, new List<int>() {0x0049fe38, 0x004a1dc7, }}, // Inventory: %4d
            {0x0051ce94, new List<int>() {0x0049fe48, 0x004a1dd7, }}, // 現在の所持数：%4d個
            {0x0051cea8, new List<int>() {0x0049fef3, }}, // Select the sepith to trade.
            {0x0051cec4, new List<int>() {0x0049fefa, }}, // 換金したいセピスを選択してください。
            {0x0051ceec, new List<int>() {0x0049ff1b, }}, // Trade %s for mira.
            {0x0051cf00, new List<int>() {0x0049ff3c, }}, // %sを、換金します。
            {0x0051cf14, new List<int>() {0x0049ff5a, }}, // Trade sepith for mira.
            {0x0051cf2c, new List<int>() {0x0049ff61, }}, // セピスの換金を行います。
            {0x0051cf48, new List<int>() {0x0049ffd9, }}, // Trade %d sepith for %d mira.
            {0x0051cf68, new List<int>() {0x0049ffe0, }}, // %d個を%dミラに換金します。
            {0x0051cf84, new List<int>() {0x004a01a3, 0x004e7787, 0x004e77b2, }}, // Sepith
            {0x0051cf8c, new List<int>() {0x004a01f0, }}, // ◆━
            {0x0051cf94, new List<int>() {0x004a022d, }}, // ━◆
            {0x0051cf9c, new List<int>() {0x004a0289, }}, // ◆            ◆
            {0x0051cfb0, new List<int>() {0x004a029d, }}, // Sepith - Rate
            {0x0051cfc0, new List<int>() {0x004a02a7, }}, //  Sepith - Rate
            {0x0051cfd0, new List<int>() {0x004a02e4, }}, // ◆                     ◆
            {0x0051cfec, new List<int>() {0x004a02b6, }}, // Sepith - Cost
            {0x0051cffc, new List<int>() {0x004a02c0, }}, //  Sepith - Cost
            {0x0051d03c, new List<int>() {0x004a0dae, }}, // Total
            {0x0051d044, new List<int>() {0x004a0e00, }}, // Trade sepith
            {0x0051d054, new List<int>() {0x004a0e07, }}, // 合計
            {0x0051d05c, new List<int>() {0x004a0e59, }}, // セピスを換金する
            {0x0051d170, new List<int>() {0x0059163c, }}, // Rest
            {0x0051d178, new List<int>() {0x00591628, }}, //  休　む
            {0x0051d180, new List<int>() {0x0059161c, }}, // Convert
            {0x0051d188, new List<int>() {0x00591618, }}, // Sell
            {0x0051d190, new List<int>() {0x00591614, }}, // Buy
            {0x0051d194, new List<int>() {0x0059160c, 0x00591630, }}, //  やめる
            {0x0051d19c, new List<int>() {0x00591608, 0x0059162c, }}, //  換  金
            {0x0051d1a4, new List<int>() {0x00591604, }}, //  売  却
            {0x0051d1ac, new List<int>() {0x00591600, }}, //  購  入
            {0x0051d1b4, new List<int>() {0x004a18fd, }}, // Buy items.
            {0x0051d1c0, new List<int>() {0x004a190c, }}, // 商品を購入します。
            {0x0051d1d4, new List<int>() {0x004a1929, }}, // Sell items.
            {0x0051d1e0, new List<int>() {0x004a1938, }}, // 所持品を売却します。
            {0x0051d1f8, new List<int>() {0x004a1955, }}, // Cash in Sepith.
            {0x0051d208, new List<int>() {0x004a1981, }}, // Leave shop.
            {0x0051d214, new List<int>() {0x004a1990, }}, // お店を出ます。
            {0x0051d224, new List<int>() {0x004a19ad, }}, // What will you do?
            {0x0051d238, new List<int>() {0x004a1d59, }}, // What will you purchase?
            {0x0051d250, new List<int>() {0x004a1d60, }}, // 何を購入しますか？
            {0x0051d264, new List<int>() {0x004a2299, }}, // Sale price: ------------------
            {0x0051d284, new List<int>() {0x004a22a0, }}, // 売却価格：------------------
            {0x0051d2a4, new List<int>() {0x004a22c4, }}, // What will you sell?
            {0x0051d2b8, new List<int>() {0x004a22da, }}, // 何を売却しますか？
            {0x0051d2cc, new List<int>() {0x004a2331, }}, // Sale price: %5dx%2d = %7d
            {0x0051d2e8, new List<int>() {0x004a2338, }}, // 売却価格：%5dx%2d = %7d
            {0x0051d300, new List<int>() {0x004a23a9, }}, // Buy %d for %d mira.
            {0x0051d314, new List<int>() {0x004a23b0, }}, // %d個を%dミラで購入します。
            {0x0051d330, new List<int>() {0x004a2419, }}, // Sell %d for %d mira.
            {0x0051d348, new List<int>() {0x004a2425, }}, // %d個を%dミラで売却します。
            {0x0051d364, new List<int>() {0x004a2745, }}, // %c#0CCannot carry any more #1C%s#0C.
            {0x0051d38c, new List<int>() {0x004a274c, }}, // %c#1C%s#0Cは、もうこれ以上持つことはできません。
            {0x0051d3c0, new List<int>() {0x004a28ec, }}, // %c#0CBuy %d #1C%s#0C for %d mira?
            {0x0051d3e4, new List<int>() {0x004a290f, }}, // %c#1C%s#0Cを%d個、%dミラで購入します。よろしいですか？
            {0x0051d41c, new List<int>() {0x004a29bd, }}, // %c#0CBought %d #1C%s#0C for %d mira.
            {0x0051d444, new List<int>() {0x004a29e0, }}, // %c#1C%s#0Cを%d個、%dミラで購入しました。
            {0x0051d470, new List<int>() {0x004a353e, 0x004a2a35, }}, // %cInsufficient mira.
            {0x0051d488, new List<int>() {0x004a354a, 0x004a2a3f, }}, // %cミラが足りません。
            {0x0051d4a0, new List<int>() {0x004a3078, }}, // %c#0CSell %d #1C%s#0C for %d mira?
            {0x0051d4c4, new List<int>() {0x004a30ae, }}, // %c#1C%s#0Cを%d個、%dミラで売却します。よろしいですか？
            {0x0051d4fc, new List<int>() {0x004a3171, }}, // %c#0CSold %d #1C%s#0C for %d mira.
            {0x0051d520, new List<int>() {0x004a31a7, }}, // %c#1C%s#0Cを%d個、%dミラで売却しました。
            {0x0051d54c, new List<int>() {0x004a32c7, }}, // Rest and recover.
            {0x0051d560, new List<int>() {0x004a32d6, }}, // 休憩して回復をします。
            {0x0051d578, new List<int>() {0x004a32a0, }}, // Select an option.
            {0x0051d590, new List<int>() {0x004a34fd, }}, // %c#0CIt will be #3C%d#0C for the night.\nWill you spend the night?
            {0x0051d5d4, new List<int>() {0x004a3509, }}, // %c#0C宿泊料は#3C%dミラ#0Cとなります。よろしいですか？
            {0x0051d60c, new List<int>() {0x004a3f04, }}, // Equip Status
            {0x0051d61c, new List<int>() {0x004a3f44, }}, // ◆━                        ━◆
            {0x0051d640, new List<int>() {0x004a3f65, }}, //      Equip Status
            {0x0051d654, new List<int>() {0x004a4376, }}, // Ingredients
            {0x0051d660, new List<int>() {0x004a4383, }}, // 材料一覧
            {0x0051d674, new List<int>() {0x004a4462, }}, // Recipe not learned
            {0x0051d688, new List<int>() {0x004a4474, }}, // 未修得
            {0x0051d690, new List<int>() {0x004a44d5, }}, // Used in:
            {0x0051d69c, new List<int>() {0x004a44dc, }}, // 以下の料理で使用
            {0x0051d6b0, new List<int>() {0x004a4632, }}, // (Need %2d)
            {0x0051d6c4, new List<int>() {0x004a4e08, 0x004cc4b7, 0x004a475e, }}, // ９
            {0x0051d6c8, new List<int>() {0x004a4e10, 0x004cc4bf, 0x004a4766, }}, // ８
            {0x0051d6cc, new List<int>() {0x004a4e18, 0x004cc4c7, 0x004a476e, }}, // ７
            {0x0051d6d0, new List<int>() {0x004a4e20, 0x004cc4cf, 0x004a4776, }}, // ６
            {0x0051d6d4, new List<int>() {0x004a4e28, 0x004cc4d7, 0x004a477e, }}, // ５
            {0x0051d6d8, new List<int>() {0x004a4e30, 0x004cc4df, 0x004a4786, }}, // ４
            {0x0051d6dc, new List<int>() {0x004a4e38, 0x004cc4e7, 0x004a478e, }}, // ３
            {0x0051d6e0, new List<int>() {0x004a4e40, 0x004cc4ef, 0x004a4796, }}, // ２
            {0x0051d6e4, new List<int>() {0x004a4e48, 0x004cc4f7, 0x004a479e, }}, // １
            {0x0051d6e8, new List<int>() {0x004a4e50, 0x004cc4ff, 0x004a47a6, }}, // 9th
            {0x0051d6ec, new List<int>() {0x004a4e58, 0x004cc507, 0x004a47ae, }}, // 8th
            {0x0051d6f0, new List<int>() {0x004a4e60, 0x004cc50f, 0x004a47b6, }}, // 7th
            {0x0051d6f4, new List<int>() {0x004a4e68, 0x004cc517, 0x004a47be, }}, // 6th
            {0x0051d6f8, new List<int>() {0x004a4e70, 0x004cc51f, 0x004a47c6, }}, // 5th
            {0x0051d6fc, new List<int>() {0x004a4e78, 0x004cc527, 0x004a47ce, }}, // 4th
            {0x0051d700, new List<int>() {0x004a4e80, 0x004cc52f, 0x004a47d6, }}, // 3rd
            {0x0051d704, new List<int>() {0x004a4e88, 0x004cc537, 0x004a47de, }}, // 2nd
            {0x0051d708, new List<int>() {0x004a4e90, 0x004cc53f, 0x004a47e6, }}, // 1st
            {0x0051d70c, new List<int>() {0x004a48b1, }}, // Bracer Status
            {0x0051d71c, new List<int>() {0x004a4951, }}, // Rank: Junior Bracer - %s Class
            {0x0051d73c, new List<int>() {0x004a497e, }}, // Total BP: %4d
            {0x0051d74c, new List<int>() {0x004a49a5, }}, // Jobs Completed: %4d
            {0x0051d760, new List<int>() {0x004a49bd, 0x004a4c00, }}, // ◆━━                        ━━◆
            {0x0051d788, new List<int>() {0x004a49de, }}, //      Bracer Status
            {0x0051d79c, new List<int>() {0x004a4a03, }}, // ランク    準遊撃士・%s級
            {0x0051d7b8, new List<int>() {0x004a4a2f, }}, // 獲得累積ＢＰ        % 4d
            {0x0051d7d4, new List<int>() {0x004a4a56, }}, // 依頼達成数          % 4d
            {0x0051d7f0, new List<int>() {0x004a4b76, }}, // Guild Bonus
            {0x0051d7fc, new List<int>() {0x004a4c21, }}, //       Guild Bonus
            {0x0051d810, new List<int>() {0x004a4e9f, }}, // Report
            {0x0051d818, new List<int>() {0x004a4ea6, }}, // 依頼の報告
            {0x0051d824, new List<int>() {0x004a4f5e, }}, // Received payment for [%c%c%s%c%c].
            {0x0051d848, new List<int>() {0x004a4f65, }}, // Reported results for [%c%c%s%c%c].
            {0x0051d86c, new List<int>() {0x004a4f72, }}, // 『%c%c%s%c%c』の報酬を取得しました。
            {0x0051d894, new List<int>() {0x004a4f79, }}, // 『%c%c%s%c%c』の報告を行いました。
            {0x0051d8b8, new List<int>() {0x004a505f, }}, // %cPayment in mira: %c%c%5d (%+5d)%c%c\nGained BP: %c%c%5d (%+5d)%c%c
            {0x0051d900, new List<int>() {0x004a5096, }}, // %c取得ミラ  ：  %c%c%5d (%+5d)%c%c\n取得ＢＰ  ：  %c%c%5d (%+5d)%c%c
            {0x0051d944, new List<int>() {0x004a50f6, }}, // Currently, there is nothing to report.
            {0x0051d96c, new List<int>() {0x004a50fd, }}, // 報告できる依頼は特にありません。
            {0x0051d990, new List<int>() {0x004a511d, }}, // Rank status
            {0x0051d99c, new List<int>() {0x004a5124, }}, // ランクの確認
            {0x0051d9ac, new List<int>() {0x004a5197, }}, // Rank advancement to [Junior Bracer - %s Class].
            {0x0051d9dc, new List<int>() {0x004a51a8, }}, // ランクが『準遊撃士・%s級』に上がりました。
            {0x0051da08, new List<int>() {0x004a5233, }}, // Received [%c%c%s%c%c] quartz as a perk.
            {0x0051da30, new List<int>() {0x004a524e, }}, // 特典として『%c%c%s%c%c』#5Cのクオーツをもらいました。
            {0x0051da68, new List<int>() {0x004a526c, }}, // Received [%c%c%s%c%c] accessory as a perk.
            {0x0051da94, new List<int>() {0x004a5273, }}, // 特典として、装備品『%c%c%s%c%c』#5Cをもらいました。
            {0x0051dac8, new List<int>() {0x004a52bf, }}, // Current rank is [Junior Bracer - %s Class].
            {0x0051daf4, new List<int>() {0x004a52d0, }}, // 現在のランクは『準遊撃士・%s級』です。
            {0x0051dcf0, new List<int>() {0x004a6856, }}, // Cannot hold any more.
            {0x0051dd08, new List<int>() {0x004a685d, 0x004a768d, }}, // これ以上持てません。
            {0x0051dd20, new List<int>() {0x004a6bb0, }}, // %cGot %d of #2C[%s]#0C.
            {0x0051dd38, new List<int>() {0x004a6c0c, }}, // %c#2C【%s】#5Cを、%d個手に入れた。
            {0x0051dd5c, new List<int>() {0x004a6e63, }}, // %cCooked and ate #2C[%s]#0C.\n%s got #2Cfood-poisoning#0C.
            {0x0051dd98, new List<int>() {0x004a6e97, }}, // %c#2C【%s】#0Cを料理して食べた。\n%sは#2Cアタリ#0Cを引いてしまった。
            {0x0051dde0, new List<int>() {0x004a6f62, }}, // %cCooked and ate #2C[%s]#0C.\nEveryone recovered #12C%d#0C HP and #12C%d#0C CP.
            {0x0051de30, new List<int>() {0x004a6f9d, }}, // %c#2C【%s】#5Cを料理して食べた。\n全員のＨＰが%d#5C、ＣＰが%d#5C回復した。
            {0x0051de7c, new List<int>() {0x004a6fd8, }}, // %cCooked and ate #2C[%s]#0C.\nEveryone recovered #12C%d#0C HP.
            {0x0051debc, new List<int>() {0x004a6fdf, }}, // %c#2C【%s】#5Cを料理して食べた。\n全員のＨＰが%d#5C回復した。
            {0x0051defc, new List<int>() {0x004a767b, }}, // Can't hold any more.
            {0x0051df14, new List<int>() {0x004a76af, }}, // Make how many #2C[%s]#0C?
            {0x0051df30, new List<int>() {0x004a76c5, }}, // #2C【%s】#0Cを、何個作りますか？
            {0x0051df54, new List<int>() {0x004a7826, }}, // Make #2C[%s]#0C?
            {0x0051df68, new List<int>() {0x004a783c, }}, // #2C【%s】#0Cを作りますか？
            {0x0051dfa8, new List<int>() {0x004a815b, }}, // MAX
            {0x0051f25c, new List<int>() {0x004c37bd, }}, // %cCannot hold any more of this equipment so cannot unequip.
            {0x0051f298, new List<int>() {0x004c37c9, }}, // %c現在の装備品をこれ以上持てないので、装備をはずせません。
            {0x0051f2d4, new List<int>() {0x004c3863, }}, // %cCannot hold any more of this equipment so cannot change.
            {0x0051f310, new List<int>() {0x004c386f, }}, // %c現在の装備品をこれ以上持てないので、装備を変更できません。
            {0x0051f350, new List<int>() {0x004c3b76, 0x004c3d0e, }}, // %cCannot hold any more of this quartz so cannot remove.
            {0x0051f388, new List<int>() {0x004c3b82, 0x004c3d1a, }}, // %c装備しているクオーツをこれ以上持てないので、\nオーブメントからはずせません。
            {0x0051f3d8, new List<int>() {0x004c3c83, }}, // %cCannot install two of the same type of quartz.
            {0x0051f40c, new List<int>() {0x004c3c92, }}, // %c同系統のクオーツは、同時に装備できません。
            {0x0051f43c, new List<int>() {0x004c3c57, }}, // %cQuartz does not match elemental slot so cannot install.
            {0x0051f478, new List<int>() {0x004c3ca1, }}, // %cそのクオーツは、スロットの属性と合わないため装備できません。
            {0x0051f4b8, new List<int>() {0x004c3ff9, 0x004c4258, }}, // %c%s recovered %d HP.
            {0x0051f4d0, new List<int>() {0x004c4010, 0x004c426b, }}, // %c%8sのＨＰを、%4d回復した。
            {0x0051f4f0, new List<int>() {0x004c4074, }}, // %c%s recovered %d HP.\n%8s recovered %d HP.
            {0x0051f51c, new List<int>() {0x004c408b, }}, // %c%8sのＨＰを、%4d回復した。\n%8sのＨＰを、%4d回復した。
            {0x0051f558, new List<int>() {0x004c410c, }}, // %c%s recovered %d HP.\n%8s recovered %d HP.\n%8s recovered %d HP.
            {0x0051f598, new List<int>() {0x004c4123, }}, // %c%8sのＨＰを、%4d回復した。\n%8sのＨＰを、%4d回復した。\n%8sのＨＰを、%4d回復した。
            {0x0051f5f0, new List<int>() {0x004c41bd, }}, // %c%s recovered %d HP.\n%8s recovered %d HP.\n%8s recovered %d HP.\n%8s recovered %d HP.
            {0x0051f648, new List<int>() {0x004c41d4, }}, // %c%8sのＨＰを、%4d回復した。\n%8sのＨＰを、%4d回復した。\n%8sのＨＰを、%4d回復した。\n%8sのＨＰを、%4d回復した。
            {0x0051f6b8, new List<int>() {0x004c42b7, }}, // Charged %d EP.
            {0x0051f6c8, new List<int>() {0x004c42ca, }}, // %dＥＰをチャージした。
            {0x0051f6e0, new List<int>() {0x004c4321, }}, // Restored %d CP.
            {0x0051f6f0, new List<int>() {0x004c4334, }}, // %dＣＰが上昇した。
            {0x0051f704, new List<int>() {0x004c44ab, }}, // Learned recipe for "%s".
            {0x0051fbc0, new List<int>() {0x005312b8, }}, // Ending
            {0x0051fbc8, new List<int>() {0x005312b4, }}, // Opening
            {0x0051fbd0, new List<int>() {0x005312b0, }}, //  ＮＥＸＴ
            {0x0051fbdc, new List<int>() {0x005312ac, }}, //  ＥＮＤＩＮＧ 
            {0x0051fbec, new List<int>() {0x005312a8, }}, //  ＯＰＥＮＩＮＧ 
            {0x0051fc00, new List<int>() {0x005312a4, }}, // Mirage quartz only
            {0x0051fc14, new List<int>() {0x005312a0, }}, // Space quartz only
            {0x0051fc28, new List<int>() {0x0053129c, }}, // Time quartz only
            {0x0051fc3c, new List<int>() {0x00531298, }}, // Wind quartz only
            {0x0051fc50, new List<int>() {0x00531294, }}, // Fire quartz only
            {0x0051fc64, new List<int>() {0x00531290, }}, // Water quartz only
            {0x0051fc78, new List<int>() {0x0053128c, }}, // Earth quartz only
            {0x0051fc8c, new List<int>() {0x00531288, }}, // All quartz OK
            {0x0051fc9c, new List<int>() {0x00531284, }}, // 幻属性装着可
            {0x0051fcac, new List<int>() {0x00531280, }}, // 空属性装着可
            {0x0051fcbc, new List<int>() {0x0053127c, }}, // 時属性装着可
            {0x0051fccc, new List<int>() {0x00531278, }}, // 風属性装着可
            {0x0051fcdc, new List<int>() {0x00531274, }}, // 火属性装着可
            {0x0051fcec, new List<int>() {0x00531270, }}, // 水属性装着可
            {0x0051fcfc, new List<int>() {0x0053126c, }}, // 地属性装着可
            {0x0051fd0c, new List<int>() {0x00531268, }}, // 全属性装着可
            {0x0051fd1c, new List<int>() {0x00472fe6, 0x00531264, }}, // Slot is sealed
            {0x0051fd2c, new List<int>() {0x00531260, }}, // Slot cannot be opened
            {0x0051fd44, new List<int>() {0x0053125c, }}, // Slot can be opened
            {0x0051fd58, new List<int>() {0x00531258, }}, // Slot already open
            {0x0051fd6c, new List<int>() {0x0053123c, }}, // 捨てる
            {0x0051fd74, new List<int>() {0x00531238, }}, // 使  う
            {0x0051fd7c, new List<int>() {0x00531234, }}, // Exit Game
            {0x0051fd88, new List<int>() {0x00531230, }}, // Return to Title
            {0x0051fd98, new List<int>() {0x0053122c, }}, //  ゲームを終了する  
            {0x0051fdac, new List<int>() {0x00531228, }}, //   タイトルに戻る   
            {0x0051fdc0, new List<int>() {0x00531224, }}, // Exit
            {0x0051fdc8, new List<int>() {0x00531220, }}, // Erase
            {0x0051fdd0, new List<int>() {0x0053121c, }}, // Load
            {0x0051fdd8, new List<int>() {0x00531218, }}, // Save
            {0x0051fde0, new List<int>() {0x00531214, }}, //  ＥＸＩＴ  
            {0x0051fdec, new List<int>() {0x00531210, }}, //  ＥＲＡＳＥ
            {0x0051fdf8, new List<int>() {0x0053120c, }}, //  ＬＯＡＤ  
            {0x0051fe04, new List<int>() {0x00531208, }}, //  ＳＡＶＥ  
            {0x0051fe10, new List<int>() {0x005311f4, }}, // Turmoil in the Royal City
            {0x0051fe2c, new List<int>() {0x005311f0, }}, // The Black Orbment
            {0x0051fe40, new List<int>() {0x005311ec, }}, // Madrigal of the White Magnolia
            {0x0051fe60, new List<int>() {0x005311e8, }}, // Disappearance of the Linde
            {0x0051fe7c, new List<int>() {0x005311e4, }}, // A Father's Love, a New Beginning
            {0x0051fea0, new List<int>() {0x005311dc, }}, // 　 　王都撩乱     
            {0x0051feb4, new List<int>() {0x005311d8, }}, //  黒のオーブメント 
            {0x0051fec8, new List<int>() {0x005311d4, }}, // 白き花のマドリガル
            {0x0051fedc, new List<int>() {0x005311d0, }}, //   消えた飛行客船  
            {0x0051fef0, new List<int>() {0x005311cc, }}, // 　　父、旅立つ    
            {0x0051ff04, new List<int>() {0x00486b02, 0x005311bc, }}, // Clear Data
            {0x0051ff10, new List<int>() {0x005311b8, }}, // Final Chapter
            {0x0051ff20, new List<int>() {0x005311b4, }}, // Chapter 3
            {0x0051ff2c, new List<int>() {0x005311b0, }}, // Chapter 2
            {0x0051ff38, new List<int>() {0x005311ac, }}, // Chapter 1
            {0x0051ff44, new List<int>() {0x005311a8, }}, // Prologue
            {0x0051ff50, new List<int>() {0x00486b02, 0x004860b5, 0x00486617, 0x005311a4, }}, // クリアデータ
            {0x0051ff60, new List<int>() {0x005311a0, }}, // 終  章
            {0x0051ff68, new List<int>() {0x0053119c, }}, // 第三章
            {0x0051ff70, new List<int>() {0x00531198, }}, // 第二章
            {0x0051ff78, new List<int>() {0x00531194, }}, // 第一章
            {0x0051ff80, new List<int>() {0x00531190, }}, // 序  章
            {0x00520040, new List<int>() {0x004c878d, }}, // ◆━━━━━━━━━━━━━━━━━━━━━━━━◆
            {0x00520094, new List<int>() {0x004c8e80, }}, // 　Mira:
            {0x0052009c, new List<int>() {0x004c8e98, }}, // 　BP:
            {0x005200a4, new List<int>() {0x004c8ea7, }}, // 　Status:
            {0x005200b0, new List<int>() {0x004c8eae, }}, // 　取得ミラ：
            {0x005200c0, new List<int>() {0x004c8ec6, }}, // 　取得ＢＰ：
            {0x005200d0, new List<int>() {0x004c8ed5, }}, // 　状況　　：
            {0x00520148, new List<int>() {0x004c903a, 0x004c9004, }}, // Reported
            {0x00520154, new List<int>() {0x004c903a, 0x004c900f, }}, // Finished
            {0x00520160, new List<int>() {0x004c903a, 0x004c901a, }}, // Failed
            {0x00520168, new List<int>() {0x004c903a, 0x004c9025, }}, // Expired
            {0x00520170, new List<int>() {0x004c903a, 0x004c902e, }}, // In Progress
            {0x0052018c, new List<int>() {0x004c9094, 0x004c905e, }}, // 報告済み
            {0x00520198, new List<int>() {0x004c9094, 0x004c9069, }}, // 達成済み
            {0x005201a4, new List<int>() {0x004c9094, 0x004c9074, }}, // 失敗
            {0x005201ac, new List<int>() {0x004c9094, 0x004c907f, 0x004caf5b, }}, // 期限切れ
            {0x005201b8, new List<int>() {0x004c9094, 0x004c9088, }}, // 請負中
            {0x005201c0, new List<int>() {0x004c91b6, }}, // ◆━━━━━━━━━━━━━━━━━━━━━━━◆
            {0x005201f4, new List<int>() {0x004cadda, }}, // new!
            {0x005201fc, new List<int>() {0x004cae14, }}, // Reported!
            {0x00520208, new List<int>() {0x004cae23, }}, // Report!
            {0x00520210, new List<int>() {0x004cae3f, }}, // Clear!
            {0x00520218, new List<int>() {0x004caebe, }}, // Term (Short)
            {0x00520228, new List<int>() {0x004caeda, }}, // Term (Medium)
            {0x00520238, new List<int>() {0x004caefa, }}, // Term (Long)
            {0x00520244, new List<int>() {0x004caf0c, }}, // 期限(短)
            {0x00520250, new List<int>() {0x004caf1f, }}, // 期限(中)
            {0x0052025c, new List<int>() {0x004caf32, }}, // 期限(長)
            {0x00520268, new List<int>() {0x004caf54, }}, // Term (Failed)
            {0x00520278, new List<int>() {0x004cc558, }}, // Junior Bracer - %s Class
            {0x00520294, new List<int>() {0x004cc564, }}, // 準遊撃士・%s級
            {0x00520910, new List<int>() {0x004e26e3, }}, // Mira/Sepith
            {0x0052091c, new List<int>() {0x004e26ea, }}, // ミラ・セピス
            {0x0052092c, new List<int>() {0x004e2927, 0x004e28a4, 0x004e2821, 0x004e279e, 0x004e271b, 0x004e270f, 0x004e2792, 0x004e2815, 0x004e2898, 0x004e291b, }}, // Carry over
            {0x00520938, new List<int>() {0x004e271b, 0x004e279e, 0x004e2821, 0x004e28a4, 0x004e2927, 0x004e2716, 0x004e2799, 0x004e281c, 0x004e289f, 0x004e2922, }}, // 引き継ぐ
            {0x00520944, new List<int>() {0x004e2942, 0x004e28bf, 0x004e283c, 0x004e27b9, 0x004e2736, 0x004e272a, 0x004e27ad, 0x004e2830, 0x004e28b3, 0x004e2936, }}, // Don't carry over
            {0x00520958, new List<int>() {0x004e2942, 0x004e28bf, 0x004e283c, 0x004e27b9, 0x004e2736, 0x004e2731, 0x004e27b4, 0x004e2837, 0x004e28ba, 0x004e293d, }}, // 引き継がない
            {0x00520968, new List<int>() {0x004e276b, 0x004e6d29, 0x004e6e95, 0x004e6f72, }}, // アイテム
            {0x00520974, new List<int>() {0x004e27ee, }}, // 装備品
            {0x0052097c, new List<int>() {0x004e286a, }}, // Information
            {0x00520988, new List<int>() {0x004e2871, }}, // 手帳
            {0x00520990, new List<int>() {0x004e28f4, }}, // ステータス
            {0x0052099c, new List<int>() {0x004e2970, }}, // Enemy Strength
            {0x005209ac, new List<int>() {0x004e2977, }}, // 敵の強さ
            {0x005209b8, new List<int>() {0x004e2a59, }}, // Decide
            {0x005209c0, new List<int>() {0x004e2a60, }}, // 決定
            {0x005209c8, new List<int>() {0x004e30f4, }}, // Carry over mira and sepith.
            {0x005209e4, new List<int>() {0x004e3103, }}, // ミラとセピスとメダルを引き継ぎます。
            {0x00520a0c, new List<int>() {0x004e3120, }}, // Carry over usable inventory items.
            {0x00520a30, new List<int>() {0x004e312f, }}, // 各種アイテム(回復、食材、本、釣具など)を引き継ぎます。
            {0x00520a68, new List<int>() {0x004e314c, }}, // Carry over equipment and quartz (~10 per).\n※Quest-related items not included.
            {0x00520ab8, new List<int>() {0x004e315b, }}, // 装備品とクオーツを引き継ぎます(各アイテム10個まで)。\n※クエスト関連アイテムを除きます。
            {0x00520b10, new List<int>() {0x004e3178, }}, // Carry over recipe book and monster guide.
            {0x00520b3c, new List<int>() {0x004e3187, }}, // レシピ手帳、魔獣手帳、\n王国地図の内容を引き継ぎます。
            {0x00520b74, new List<int>() {0x004e31a4, }}, // Carry over levels and opened orbment slots.
            {0x00520ba0, new List<int>() {0x004e31b3, }}, // レベルとスロットの強化状況を引き継ぎます。
            {0x00520bd0, new List<int>() {0x004e31e2, }}, // Set enemy difficulty.\n[Normal] Average difficulty. For those wanting a little challenge.
            {0x00520c30, new List<int>() {0x004e31f6, }}, // Set enemy difficulty.\n[Hard] A difficult setting. For those who find normal a bit lacking.
            {0x00520c90, new List<int>() {0x004e320a, }}, // Set enemy difficulty.\n[Nightmare] It's your funeral. May Aidios be with you.
            {0x00520ce0, new List<int>() {0x004e321e, }}, // Set enemy difficulty.\n[Easy] A gentle balance for easy progression. For beginners.
            {0x00520d38, new List<int>() {0x004e3242, }}, // 敵の強さを変更できます。\n【ノーマル】標準的なバランスです。適度な刺激を楽しみたい方に。
            {0x00520d90, new List<int>() {0x004e3256, }}, // 敵の強さを変更できます。\n【ハード】やや厳しいバランスです。ノーマルでは物足りない方に。
            {0x00520de8, new List<int>() {0x004e326a, }}, // 敵の強さを変更できます。\n【ナイトメア】まさに悪夢。各種データを引き継がないと厳しいバランスです。
            {0x00520e50, new List<int>() {0x004e327e, }}, // 敵の強さを変更できます。\n【イージー】サクサク進める易しいバランスです。時間のない方や初心者向け。
            {0x00520eb4, new List<int>() {0x004e32a0, }}, // Start game with the above settings.
            {0x00520ed8, new List<int>() {0x004e32af, }}, // 以上の設定でゲームを開始します。
            {0x00520fbc, new List<int>() {0x004e66e7, }}, // Lv.%d  HP:%d
            {0x00520fcc, new List<int>() {0x004e6830, }}, // STR:%3d  DEF:%3d
            {0x00520fe0, new List<int>() {0x004e686f, }}, // ATS:%3d  ADF:%3d
            {0x00520ff4, new List<int>() {0x004e68b5, }}, // SPD:%3d  EXP:%3d
            {0x00521008, new List<int>() {0x004e68e8, }}, // STR:???  DEF:???
            {0x0052101c, new List<int>() {0x004e68fb, }}, // ATS:???  ADF:???
            {0x00521030, new List<int>() {0x004e690e, }}, // SPD:???  EXP:???
            {0x00521068, new List<int>() {0x004e6a20, }}, // Elemental
            {0x00521074, new List<int>() {0x004e6a2a, }}, // Efficacy (%)
            {0x00521084, new List<int>() {0x004e6a31, }}, // 属性攻撃
            {0x00521090, new List<int>() {0x004e6a3b, }}, // 有効率(％)
            {0x0052109c, new List<int>() {0x004e6d1c, 0x004e6e88, 0x004e6f61, }}, // Item
            {0x005210a8, new List<int>() {0x004e74f1, 0x004e7562, }}, // StatGuard
            {0x005210b4, new List<int>() {0x004e74fb, 0x004e7573, }}, // 異常耐性
            {0x005210c0, new List<int>() {0x004e7791, 0x004e780b, }}, // セピス
            {0x005210c8, new List<int>() {0x004e7997, }}, // Location
            {0x005210d4, new List<int>() {0x004e799e, }}, // 出現場所
            {0x00530e4c, new List<int>() {0x00530e5c, }}, //  は  い
            {0x00530e54, new List<int>() {0x00530e60, }}, //  いいえ
            {0x00530e64, new List<int>() {0x00530e70, }}, //  Yes
            {0x00530e6c, new List<int>() {0x00530e74, }}, //  No
        };

        protected override List<Tuple<int, byte[]>> Patches => new List<Tuple<int, byte[]>>
        {
            // Search for CMP AL,0x5C
            new Tuple<int, byte[]>(0x080C82, new byte[] {0xEB, 0x4C}),
            new Tuple<int, byte[]>(0x080E66, new byte[] {0x3C, 0xE0}),
        };

        public DX9File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }
    }
}
