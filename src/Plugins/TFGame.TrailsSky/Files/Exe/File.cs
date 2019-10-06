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

        protected override int ChangesFileVersion => 3;

        protected virtual long FontTableOffset => 0x15F140;

        protected virtual List<Tuple<int, byte[]>> Patches => new List<Tuple<int, byte[]>>
        {
            new Tuple<int, byte[]>(0x07AD72, new byte[] {0xEB, 0x4C}),
            new Tuple<int, byte[]>(0x07AF56, new byte[] {0x3C, 0xE0}),
        };

        protected virtual Dictionary<int, List<int>> StringOffsets => new Dictionary<int, List<int>>()
        {
            {0x0015BFD4, new List<int>() {0x000015B0}},
            {0x0015BFE8, new List<int>() {0x000015B7}},
            {0x0015BFFC, new List<int>() {0x00001650}},
            {0x0015C010, new List<int>() {0x00001657}},
            {0x0015C01C, new List<int>() {0x00002649}},
            {0x0015C034, new List<int>() {0x00003735}},
            {0x0015C03C, new List<int>() {0x0000373C}},
            {0x0015C04C, new List<int>() {0x000037F8}},
            {0x0015C054, new List<int>() {0x000037FF}},
            {0x0015C07C, new List<int>() {0x0003AF96}},
            {0x0015C094, new List<int>() {0x0003AF9D}},
            {0x0015C0B4, new List<int>() {0x0003AF76}},
            {0x0015C0C4, new List<int>() {0x0003AF7D}},
            {0x0015C0E0, new List<int>() {0x000078F0}},
            {0x0015C0F0, new List<int>() {0x000078F7}},
            {0x0015C110, new List<int>() {0x00008A40, 0x0002B800, 0x0002B881}},
            {0x0015C118, new List<int>() {0x00008A4D}},
            {0x0015C128, new List<int>() {0x00008A58, 0x000724FC}},
            {0x0015C130, new List<int>() {0x00008A65}},
            {0x0015C140, new List<int>() {0x00008A71, 0x000DBA15}},
            {0x0015C148, new List<int>() {0x00008A7E}},
            {0x0015C158, new List<int>() {0x0000A9C0}},
            {0x0015C174, new List<int>() {0x0000A9C7}},
            {0x0015C1A0, new List<int>() {0x0000A9F6}},
            {0x0015C1A8, new List<int>() {0x0000AA1D}},
            {0x0015C200, new List<int>() {0x0000AA39}},
            {0x0015C268, new List<int>() {0x0000F78B}},
            {0x0015C288, new List<int>() {0x0000F79E}},
            {0x0015C2AC, new List<int>() {0x000106D8}},
            {0x0015C2B8, new List<int>() {0x000106E4}},
            {0x0015C584, new List<int>() {0x00022B29}},
            {0x0015C598, new List<int>() {0x00022B35}},
            {0x0015C64C, new List<int>() {0x0002B33C}},
            {0x0015C660, new List<int>() {0x0002B344}},
            {0x0015C6C4, new List<int>() {0x0002B34B}},
            {0x0015C6E0, new List<int>() {0x0002B353}},
            {0x0015C734, new List<int>() {0x0002B3DD}},
            {0x0015C740, new List<int>() {0x0002B3E5}},
            {0x0015C7CC, new List<int>() {0x0002B3FC}},
            {0x0015C7D8, new List<int>() {0x0002B404}},
            {0x0015C860, new List<int>() {0x0002B441}},
            {0x0015C8F8, new List<int>() {0x0002B458}},
            {0x0015C980, new List<int>() {0x0002B495}},
            {0x0015CA70, new List<int>() {0x0002B49C}},
            {0x0015CB10, new List<int>() {0x0002B582}},
            {0x0015CB28, new List<int>() {0x0002B58A}},
            {0x0015CBE8, new List<int>() {0x0002B5A1}},
            {0x0015CC08, new List<int>() {0x0002B5A9}},
            {0x0015CCB0, new List<int>() {0x0002B5E6}},
            {0x0015CD00, new List<int>() {0x0002B5ED}},
            {0x0015CD58, new List<int>() {0x0002B75E}},
            {0x0015CD60, new List<int>() {0x0002B766}},
            {0x0015CE08, new List<int>() {0x0002B77D}},
            {0x0015CE18, new List<int>() {0x0002B785}},
            {0x0015CE80, new List<int>() {0x0002B7E5, 0x00076AEE, 0x00076E20}},
            {0x0015CE88, new List<int>() {0x0002B809}},
            {0x0015CEF0, new List<int>() {0x0002B820, 0x0002B8AE}},
            {0x0015CEF8, new List<int>() {0x0002B829}},
            {0x0015CF60, new List<int>() {0x0002B874}},
            {0x0015CF68, new List<int>() {0x0002B88A}},
            {0x0015CFD8, new List<int>() {0x0002B8A1}},
            {0x0015CFE0, new List<int>() {0x0002B8B7}},
            {0x0015D038, new List<int>() {0x0002B967}},
            {0x0015D040, new List<int>() {0x0002B96F}},
            {0x0015D0D4, new List<int>() {0x0002B986}},
            {0x0015D0E0, new List<int>() {0x0002B98E}},
            {0x0015D158, new List<int>() {0x0002B9EE, 0x0002B9FB, 0x0002BAB8, 0x0002BB0B, 0x00076B05, 0x00076E37}},
            {0x0015D15C, new List<int>() {0x0002BA16}},
            {0x0015D168, new List<int>() {0x0002BA1F}},
            {0x0015D1F0, new List<int>() {0x0002BA36}},
            {0x0015D200, new List<int>() {0x0002BA3F}},
            {0x0015D298, new List<int>() {0x0002BAC5, 0x0002BB78, 0x000725C2}},
            {0x0015D2A4, new List<int>() {0x0002BAD2}},
            {0x0015D2B0, new List<int>() {0x0002BADA}},
            {0x0015D364, new List<int>() {0x0002BAF1, 0x0002BBA5, 0x0002BC2F, 0x0002BDE5}},
            {0x0015D370, new List<int>() {0x0002BAFE, 0x0002BB98}},
            {0x0015D37C, new List<int>() {0x0002BB18}},
            {0x0015D390, new List<int>() {0x0002BB20}},
            {0x0015D42C, new List<int>() {0x0002BB6B}},
            {0x0015D438, new List<int>() {0x0002BB81}},
            {0x0015D4C8, new List<int>() {0x0002BBAE}},
            {0x0015D530, new List<int>() {0x0002BC12, 0x0002BDC8, 0x000845E7}},
            {0x0015D538, new List<int>() {0x0002BC1F, 0x0002BDD5}},
            {0x0015D548, new List<int>() {0x0002BC28}},
            {0x0015D5D8, new List<int>() {0x0002BC3C, 0x0002BDF2}},
            {0x0015D5E8, new List<int>() {0x0002BC45}},
            {0x0015D690, new List<int>() {0x0002BDDE}},
            {0x0015D768, new List<int>() {0x0002BDFB}},
            {0x0015D850, new List<int>() {0x0002BF53}},
            {0x0015D864, new List<int>() {0x0002BF6A}},
            {0x0015D874, new List<int>() {0x0002BFA7}},
            {0x0015D8A0, new List<int>() {0x0002BFBE}},
            {0x0015D8D0, new List<int>() {0x00030305, 0x0003032A}},
            {0x0015D8D8, new List<int>() {0x000302C9, 0x000302EC}},
            {0x0015D8E0, new List<int>() {0x00030289, 0x000302B0, 0x0003554D, 0x00035570}},
            {0x0015DD64, new List<int>() {0x00034892}},
            {0x0015DD78, new List<int>() {0x0003489C}},
            {0x0015DD98, new List<int>() {0x000348BF}},
            {0x0015DDAC, new List<int>() {0x000348C9}},
            {0x0015DDC0, new List<int>() {0x000348ED}},
            {0x0015DDD0, new List<int>() {0x000348F7}},
            {0x0015DDF4, new List<int>() {0x0003491B}},
            {0x0015DE08, new List<int>() {0x00034925}},
            {0x0015DE24, new List<int>() {0x00034949}},
            {0x0015DE38, new List<int>() {0x00034950}},
            {0x0015DE50, new List<int>() {0x00034971}},
            {0x0015DE64, new List<int>() {0x00034978}},
            {0x0015DE84, new List<int>() {0x00034999}},
            {0x0015DE9C, new List<int>() {0x000349A0}},
            {0x0015DEB0, new List<int>() {0x000349B9}},
            {0x0015DEC0, new List<int>() {0x000349C6}},
            {0x0015DED4, new List<int>() {0x000349EF}},
            {0x0015DEE8, new List<int>() {0x00034A01}},
            {0x0015DEFC, new List<int>() {0x00034A89}},
            {0x0015DF18, new List<int>() {0x00034AA4}},
            {0x0015DF34, new List<int>() {0x00034B0E}},
            {0x0015DF4C, new List<int>() {0x00034B15}},
            {0x0015DF64, new List<int>() {0x00034B2B}},
            {0x0015DF80, new List<int>() {0x00034B32}},
            {0x0015DF98, new List<int>() {0x00034B48}},
            {0x0015DFB4, new List<int>() {0x00034B4F}},
            {0x0015DFC8, new List<int>() {0x00034B56}},
            {0x0015DFE4, new List<int>() {0x00034B63}},
            {0x0015E0FC, new List<int>() {0x00035158}},
            {0x0015E130, new List<int>() {0x000351F4}},
            {0x0015E13C, new List<int>() {0x00035293, 0x001782FC}},
            {0x0015E14C, new List<int>() {0x00035710}},
            {0x0015E1B8, new List<int>() {0x0003577F}},
            {0x0015E1C0, new List<int>() {0x0003579B}},
            {0x0015E1C4, new List<int>() {0x000357F3}},
            {0x0015E1D4, new List<int>() {0x0003580E}},
            {0x0015E1DC, new List<int>() {0x00035815}},
            {0x0015E1E8, new List<int>() {0x00035822}},
            {0x0015E1F4, new List<int>() {0x00035888}},
            {0x0015E204, new List<int>() {0x0003594E}},
            {0x0015E20C, new List<int>() {0x000359EC}},
            {0x0015E21C, new List<int>() {0x00035C76, 0x00099700}},
            {0x0015E3C8, new List<int>() {0x0003A363}},
            {0x0015E3E0, new List<int>() {0x0003A3E5}},
            {0x0015E3EC, new List<int>() {0x0003A4A2, 0x0003AABC, 0x0008BA04, 0x0008BE51}},
            {0x0015E3F4, new List<int>() {0x0003A4BD, 0x0003AAD7}},
            {0x0015E3FC, new List<int>() {0x0003A531}},
            {0x0015E414, new List<int>() {0x0003A538}},
            {0x0015E42C, new List<int>() {0x0003A73A}},
            {0x0015E438, new List<int>() {0x0003A867}},
            {0x0015E448, new List<int>() {0x0003A941}},
            {0x0015E454, new List<int>() {0x0003AA4F}},
            {0x0015E57C, new List<int>() {0x001D7CAC}},
            {0x0015E58C, new List<int>() {0x001D7CA8}},
            {0x0015E594, new List<int>() {0x001D7CA4}},
            {0x0015E5A4, new List<int>() {0x001D7CA0}},
            {0x0015F520, new List<int>() {0x00068DC9}},
            {0x0015F568, new List<int>() {0x00068DE1}},
            {0x0015F5CC, new List<int>() {0x0006A6D5}},
            {0x0015F5E8, new List<int>() {0x0006A6DC, 0x000BD713}},
            {0x0015F664, new List<int>() {0x00178284, 0x001D7FF8}},
            {0x0015F66C, new List<int>() {0x00178280, 0x001D7FF4}},
            {0x0015F670, new List<int>() {0x001D7FF0}},
            {0x0015F67C, new List<int>() {0x001D7FEC}},
            {0x0015F68C, new List<int>() {0x0006CF84}},
            {0x0015F6A0, new List<int>() {0x0006CF9D}},
            {0x0015F6B8, new List<int>() {0x0006CFB6}},
            {0x0015F6CC, new List<int>() {0x0006CFCF}},
            {0x0015F6D8, new List<int>() {0x0006CFE7}},
            {0x0015F700, new List<int>() {0x0006D000}},
            {0x0015F728, new List<int>() {0x0006D019}},
            {0x0015F74C, new List<int>() {0x0006D032}},
            {0x0015F764, new List<int>() {0x0006DBF0}},
            {0x0015F794, new List<int>() {0x0006ED74}},
            {0x0015F7F4, new List<int>() {0x0006F8B9}},
            {0x0015F81C, new List<int>() {0x0006F8F3}},
            {0x0015F828, new List<int>() {0x0006F7C8}},
            {0x0015F840, new List<int>() {0x0006F812}},
            {0x0015F84C, new List<int>() {0x0006F745}},
            {0x0015F860, new List<int>() {0x000704CD}},
            {0x0015F880, new List<int>() {0x000704D4}},
            {0x0015F8A8, new List<int>() {0x000705BD}},
            {0x0015F8BC, new List<int>() {0x0007062A}},
            {0x0015F8D8, new List<int>() {0x000706BD, 0x00070758}},
            {0x0015F900, new List<int>() {0x000706C4, 0x0007075F}},
            {0x0015F954, new List<int>() {0x0007267C}},
            {0x0015F984, new List<int>() {0x000726BB}},
            {0x00160058, new List<int>() {0x00075ACE}},
            {0x00160070, new List<int>() {0x00075B31}},
            {0x00160120, new List<int>() {0x000767BB}},
            {0x00160128, new List<int>() {0x0007698C, 0x000DBB9E}},
            {0x00160130, new List<int>() {0x00076AC2, 0x00076DF4, 0x00080B0C}},
            {0x00160138, new List<int>() {0x00076AD7, 0x00076E09, 0x000A156B}},
            {0x00160164, new List<int>() {0x00076BD4}},
            {0x00160180, new List<int>() {0x00076D0C}},
            {0x001601A8, new List<int>() {0x00076DB5}},
            {0x001601D0, new List<int>() {0x00076F13}},
            {0x001601E0, new List<int>() {0x00077128, 0x0009D4EE}},
            {0x001601E4, new List<int>() {0x00077140, 0x0009D507}},
            {0x001601E8, new List<int>() {0x0007715A, 0x0009D520}},
            {0x001601EC, new List<int>() {0x00077174, 0x0009D539}},
            {0x001601F0, new List<int>() {0x0007718E, 0x0009D552}},
            {0x001601F4, new List<int>() {0x000771A8, 0x0009D568}},
            {0x001601F8, new List<int>() {0x000771C2, 0x0009D57E}},
            {0x001601FC, new List<int>() {0x000771DC, 0x0009D597}},
            {0x00160200, new List<int>() {0x000771F6, 0x0009D5B0}},
            {0x00160214, new List<int>() {0x000775B9}},
            {0x00160220, new List<int>() {0x00077774}},
            {0x00160230, new List<int>() {0x00077838, 0x000778B4}},
            {0x00160234, new List<int>() {0x00077858, 0x000778D4}},
            {0x001602BC, new List<int>() {0x00077E8C, 0x000DBA98}},
            {0x001602C8, new List<int>() {0x00077FB3}},
            {0x001602D8, new List<int>() {0x00078A4A}},
            {0x001603E4, new List<int>() {0x0007E984}},
            {0x001603F0, new List<int>() {0x0007E99D}},
            {0x001604AC, new List<int>() {0x0007ED20, 0x00178238}},
            {0x001604CC, new List<int>() {0x0007ED27, 0x00178220}},
            {0x001604F4, new List<int>() {0x0007ED4C, 0x00080C1A, 0x000DBCAD}},
            {0x00160500, new List<int>() {0x0007ED54, 0x00080C22, 0x000DBC93}},
            {0x00160508, new List<int>() {0x0007ED5C, 0x00080C2A, 0x000DBC70}},
            {0x00160514, new List<int>() {0x0007ED64, 0x00080C32, 0x000DBC57}},
            {0x00160520, new List<int>() {0x0007ED6C, 0x00080C3A, 0x000DBCA0}},
            {0x00160528, new List<int>() {0x0007ED74, 0x00080C42, 0x000DBC86}},
            {0x00160530, new List<int>() {0x0007ED7C, 0x00080C4A, 0x000DBC69}},
            {0x0016053C, new List<int>() {0x0007ED84, 0x00080C52, 0x000DBC50}},
            {0x00160544, new List<int>() {0x0007ED92}},
            {0x001605B8, new List<int>() {0x0008085A}},
            {0x001605C4, new List<int>() {0x00080861}},
            {0x001605D0, new List<int>() {0x0008088C}},
            {0x001605DC, new List<int>() {0x00080893}},
            {0x001605E8, new List<int>() {0x00080C95}},
            {0x0016066C, new List<int>() {0x000813D7}},
            {0x001606A0, new List<int>() {0x000814F0}},
            {0x001606C8, new List<int>() {0x00081514}},
            {0x00160934, new List<int>() {0x00178DF4}},
            {0x00160940, new List<int>() {0x00178DF0}},
            {0x00160950, new List<int>() {0x00178DEC}},
            {0x00160964, new List<int>() {0x00178DE8}},
            {0x00160974, new List<int>() {0x00178DE4}},
            {0x00160984, new List<int>() {0x00178DE0}},
            {0x00160994, new List<int>() {0x00083800}},
            {0x001609B0, new List<int>() {0x00083814}},
            {0x001609CC, new List<int>() {0x000837D9}},
            {0x001609E8, new List<int>() {0x000837E8}},
            {0x00160A04, new List<int>() {0x000837AE}},
            {0x00160A14, new List<int>() {0x000837BD}},
            {0x00160A50, new List<int>() {0x00083BBC}},
            {0x00160A84, new List<int>() {0x00083C67}},
            {0x00160B0C, new List<int>() {0x00084692}},
            {0x00160B40, new List<int>() {0x00084FF6}},
            {0x00160B4C, new List<int>() {0x000850AB}},
            {0x00160E04, new List<int>() {0x00178DC0}},
            {0x00160E0C, new List<int>() {0x00178DBC}},
            {0x00160E1C, new List<int>() {0x00178DB8}},
            {0x00160E2C, new List<int>() {0x00178DB4}},
            {0x00160E3C, new List<int>() {0x00178DB0}},
            {0x00160E4C, new List<int>() {0x00178DAC}},
            {0x00160E58, new List<int>() {0x00178DA8}},
            {0x00160E68, new List<int>() {0x00178DA4}},
            {0x00160E80, new List<int>() {0x00178D80}},
            {0x00160E8C, new List<int>() {0x00178D7C}},
            {0x00160E98, new List<int>() {0x00178D78}},
            {0x00160EA4, new List<int>() {0x00178D74}},
            {0x00160EB4, new List<int>() {0x00178D70}},
            {0x00160EC0, new List<int>() {0x00178D6C}},
            {0x00160ECC, new List<int>() {0x00178D68}},
            {0x00160EDC, new List<int>() {0x00178D64}},
            {0x00160EE8, new List<int>() {0x00088BFC}},
            {0x00160EF0, new List<int>() {0x00088C2A}},
            {0x00160F00, new List<int>() {0x00088C4F}},
            {0x00160F08, new List<int>() {0x00088C7E}},
            {0x00160FD4, new List<int>() {0x0008B7E7}},
            {0x00160FE0, new List<int>() {0x0008B7EE}},
            {0x00160FF0, new List<int>() {0x0008B816}},
            {0x00160FF8, new List<int>() {0x0008B81D}},
            {0x00161004, new List<int>() {0x0008B830, 0x0008B8CF}},
            {0x00161010, new List<int>() {0x0008B837, 0x0008B8DC}},
            {0x0016101C, new List<int>() {0x0008B86A}},
            {0x00161028, new List<int>() {0x0008B871}},
            {0x00161038, new List<int>() {0x0008B896}},
            {0x00161044, new List<int>() {0x0008B8A3}},
            {0x00161054, new List<int>() {0x0008B8B5}},
            {0x0016105C, new List<int>() {0x0008B8C2}},
            {0x0016106C, new List<int>() {0x0008B90F}},
            {0x00161078, new List<int>() {0x0008B916}},
            {0x00161088, new List<int>() {0x0008B93D}},
            {0x00161094, new List<int>() {0x0008B944}},
            {0x001610A0, new List<int>() {0x0008B958}},
            {0x001610B0, new List<int>() {0x0008B95F}},
            {0x001610BC, new List<int>() {0x0008B992}},
            {0x001610CC, new List<int>() {0x0008B999}},
            {0x001610DC, new List<int>() {0x0008B9BE}},
            {0x001610E4, new List<int>() {0x0008B9CB}},
            {0x001610F0, new List<int>() {0x0008B9DD}},
            {0x00161100, new List<int>() {0x0008B9EA}},
            {
                0x00161110,
                new List<int>()
                    {0x0008B9F7, 0x0008BB58, 0x0008BBFE, 0x0008BCDF, 0x0008BD90, 0x0008BE4A, 0x0008BEA7, 0x0008BF30}
            },
            {0x0016115C, new List<int>() {0x0008BA61}},
            {0x0016116C, new List<int>() {0x0008BA68}},
            {0x00161180, new List<int>() {0x0008BA90}},
            {0x00161190, new List<int>() {0x0008BA97}},
            {0x0016119C, new List<int>() {0x0008BAAD}},
            {0x001611AC, new List<int>() {0x0008BABA}},
            {0x001611B8, new List<int>() {0x0008BAC7}},
            {0x001611C4, new List<int>() {0x0008BAD4}},
            {0x001611CC, new List<int>() {0x0008BB0A}},
            {0x001611DC, new List<int>() {0x0008BB11}},
            {0x001611EC, new List<int>() {0x0008BB3E, 0x0008BCFA, 0x0008BDAB, 0x0008BEC2, 0x0008BF4A}},
            {0x001611F0, new List<int>() {0x0008BB45}},
            {0x001611FC, new List<int>() {0x0008BB5F}},
            {0x00161208, new List<int>() {0x0008BBC2}},
            {0x0016121C, new List<int>() {0x0008BBC9}},
            {0x00161228, new List<int>() {0x0008BC09, 0x0008BCE6, 0x0008BD97, 0x0008BEAE}},
            {0x00161230, new List<int>() {0x0008BC23}},
            {0x0016123C, new List<int>() {0x0008BC2A}},
            {0x00161278, new List<int>() {0x0008BCAB}},
            {0x00161288, new List<int>() {0x0008BCB2}},
            {0x00161298, new List<int>() {0x0008BD01, 0x0008BDB2, 0x0008BEC9}},
            {0x001612A0, new List<int>() {0x0008BD37}},
            {0x001612B0, new List<int>() {0x0008BD3E}},
            {0x001612C0, new List<int>() {0x0008BD5F}},
            {0x001612D0, new List<int>() {0x0008BD66}},
            {0x001612DC, new List<int>() {0x0008BDE8}},
            {0x001612E8, new List<int>() {0x0008BDEF}},
            {0x001612F8, new List<int>() {0x0008BE10}},
            {0x00161304, new List<int>() {0x0008BE17}},
            {0x00161314, new List<int>() {0x0008BE6E}},
            {0x00161320, new List<int>() {0x0008BE75}},
            {0x00161328, new List<int>() {0x0008BE83}},
            {0x00161330, new List<int>() {0x0008BE8A}},
            {0x00161338, new List<int>() {0x0008BEFF}},
            {0x00161348, new List<int>() {0x0008BF06}},
            {0x00161358, new List<int>() {0x0008BF37}},
            {0x00161364, new List<int>() {0x0008BF51}},
            {0x00161370, new List<int>() {0x0008BF84}},
            {0x00161380, new List<int>() {0x0008BF8B}},
            {0x00161394, new List<int>() {0x0008BFB8}},
            {0x001613A4, new List<int>() {0x0008BFC5}},
            {0x001613B4, new List<int>() {0x0008BFD6}},
            {0x001613C4, new List<int>() {0x0008BFE3}},
            {0x001613D4, new List<int>() {0x0008BFF0}},
            {0x001613DC, new List<int>() {0x0008BFFD}},
            {0x001613E8, new List<int>() {0x0008C854}},
            {0x00161418, new List<int>() {0x0008C869}},
            {0x00161448, new List<int>() {0x0008C88C}},
            {0x0016147C, new List<int>() {0x0008C8A1}},
            {0x001614AC, new List<int>() {0x0008C8C4}},
            {0x001614CC, new List<int>() {0x0008C8D9}},
            {0x001614F8, new List<int>() {0x0008C8FC}},
            {0x00161514, new List<int>() {0x0008C911}},
            {0x00161538, new List<int>() {0x0008C934}},
            {0x0016154C, new List<int>() {0x0008C949}},
            {0x00161574, new List<int>() {0x0008C96C}},
            {0x00161594, new List<int>() {0x0008C981}},
            {0x001615B8, new List<int>() {0x0008C9A8}},
            {0x001615E0, new List<int>() {0x0008C9C0}},
            {0x0016164C, new List<int>() {0x0008CA18}},
            {0x0016166C, new List<int>() {0x0008CA34}},
            {0x00161694, new List<int>() {0x0008CA57}},
            {0x001616B4, new List<int>() {0x0008CA6C}},
            {0x001616D0, new List<int>() {0x0008CA8F}},
            {0x001616F0, new List<int>() {0x0008CAA4}},
            {0x00161718, new List<int>() {0x0008CAC7}},
            {0x00161734, new List<int>() {0x0008CADC}},
            {0x00161750, new List<int>() {0x0008CB08}},
            {0x00161770, new List<int>() {0x0008CB1D}},
            {0x00161790, new List<int>() {0x0008CB3B}},
            {0x001617B4, new List<int>() {0x0008CB50}},
            {0x001617E0, new List<int>() {0x0008CB73}},
            {0x00161850, new List<int>() {0x0008CB88}},
            {0x001618A0, new List<int>() {0x0008CBAB}},
            {0x001618C4, new List<int>() {0x0008CBC0}},
            {0x00161914, new List<int>() {0x0008EEA7}},
            {0x00161948, new List<int>() {0x0008EF52}},
            {0x0016197C, new List<int>() {0x0008F128}},
            {0x001619B4, new List<int>() {0x0008F24D}},
            {0x001619C8, new List<int>() {0x0008F323}},
            {0x001619D0, new List<int>() {0x00090341}},
            {0x001619E0, new List<int>() {0x0009034E}},
            {0x00161B9C, new List<int>() {0x00091999, 0x00091E75}},
            {0x00161BA4, new List<int>() {0x00091B5C}},
            {0x00161BC8, new List<int>() {0x00091D73}},
            {0x00161BF8, new List<int>() {0x00091DED}},
            {0x00161C10, new List<int>() {0x00091FBB}},
            {0x00161DCC, new List<int>() {0x001D8258}},
            {0x00161E08, new List<int>() {0x001D8254}},
            {0x00161E50, new List<int>() {0x001D8250}},
            {0x00161E98, new List<int>() {0x001D824C}},
            {0x00161EE4, new List<int>() {0x001D8248}},
            {0x00161F24, new List<int>() {0x001D8244}},
            {0x00162038, new List<int>() {0x00096FC7}},
            {0x00162088, new List<int>() {0x00096FCE}},
            {0x00162234, new List<int>() {0x00178C48}},
            {0x00162244, new List<int>() {0x00178C44}},
            {0x00162254, new List<int>() {0x00178C40, 0x00178C4C, 0x00178C50}},
            {0x00162260, new List<int>() {0x00178C3C}},
            {0x0016226C, new List<int>() {0x00178C38}},
            {0x00162278, new List<int>() {0x00178C34}},
            {0x00162288, new List<int>() {0x00178C30}},
            {0x00162298, new List<int>() {0x00178C24}},
            {0x001622A4, new List<int>() {0x00178C20}},
            {0x001622B0, new List<int>() {0x00178C1C, 0x00178C28, 0x00178C2C}},
            {0x001622BC, new List<int>() {0x00178C18}},
            {0x001622C8, new List<int>() {0x00178C14}},
            {0x001622D4, new List<int>() {0x00178C10}},
            {0x001622E0, new List<int>() {0x00178C0C}},
            {0x001622EC, new List<int>() {0x001D8320, 0x001D83A0, 0x001D83C4}},
            {0x001622F4, new List<int>() {0x001D831C, 0x001D83C0}},
            {0x001622FC, new List<int>() {0x001D8318}},
            {0x00162304, new List<int>() {0x001D8314}},
            {0x0016230C, new List<int>() {0x001D8310}},
            {0x00162318, new List<int>() {0x001D830C}},
            {0x00162324, new List<int>() {0x001D8308}},
            {0x00162330, new List<int>() {0x001D8304}},
            {0x0016233C, new List<int>() {0x000979C2}},
            {0x0016234C, new List<int>() {0x000979D1}},
            {0x00162368, new List<int>() {0x0009799B}},
            {0x0016237C, new List<int>() {0x000979AA}},
            {0x00162398, new List<int>() {0x00097975}},
            {0x001623B4, new List<int>() {0x00097984, 0x0009ADF5}},
            {0x001623D0, new List<int>() {0x0009794F}},
            {0x001623E4, new List<int>() {0x0009795E, 0x0009AE4D, 0x0009C750}},
            {0x001623F4, new List<int>() {0x0009808F, 0x00178288}},
            {0x00162400, new List<int>() {0x000980F5, 0x0017828C}},
            {0x0016240C, new List<int>() {0x00097FE0, 0x00178290}},
            {0x00162418, new List<int>() {0x00178294}},
            {0x00162420, new List<int>() {0x000C2296, 0x000C22F0}},
            {0x0016242C, new List<int>() {0x0009807D}},
            {0x0016243C, new List<int>() {0x000980E3}},
            {0x00162448, new List<int>() {0x00097FCE}},
            {0x00162458, new List<int>() {0x00098AC1}},
            {0x0016246C, new List<int>() {0x00098AD5}},
            {0x00162490, new List<int>() {0x00098B02, 0x00098D15}},
            {0x001624A8, new List<int>() {0x00098B0E, 0x00098D21}},
            {0x001624C0, new List<int>() {0x00098BEF}},
            {0x001624DC, new List<int>() {0x00098BF6}},
            {0x00162508, new List<int>() {0x00098CB0}},
            {0x00162524, new List<int>() {0x00098CC9}},
            {0x00162540, new List<int>() {0x00098DB2}},
            {0x0016255C, new List<int>() {0x00098DBE}},
            {0x0016258C, new List<int>() {0x00098EF8}},
            {0x001625AC, new List<int>() {0x00098F04}},
            {0x001625D0, new List<int>() {0x000991BF}},
            {0x001625E8, new List<int>() {0x000991CB}},
            {0x00162610, new List<int>() {0x00099130}},
            {0x00162630, new List<int>() {0x0009913E}},
            {0x00162674, new List<int>() {0x0009914C}},
            {0x00162698, new List<int>() {0x00099162}},
            {0x001626C0, new List<int>() {0x00099170}},
            {0x00162700, new List<int>() {0x0009917E}},
            {0x00162728, new List<int>() {0x00099236}},
            {0x0016273C, new List<int>() {0x00099245}},
            {0x00162760, new List<int>() {0x000992AD, 0x0009B1C0}},
            {0x00162774, new List<int>() {0x000992B4}},
            {0x0016278C, new List<int>() {0x000992D9}},
            {0x001627AC, new List<int>() {0x000992FA}},
            {0x001627D4, new List<int>() {0x00099339, 0x0009B258}},
            {0x001627E4, new List<int>() {0x00099349, 0x0009B268}},
            {0x001627F8, new List<int>() {0x000993F4}},
            {0x00162814, new List<int>() {0x000993FB}},
            {0x0016283C, new List<int>() {0x0009941C}},
            {0x00162850, new List<int>() {0x0009943D}},
            {0x00162864, new List<int>() {0x0009945B}},
            {0x0016287C, new List<int>() {0x00099462}},
            {0x00162898, new List<int>() {0x000994DA}},
            {0x001628B8, new List<int>() {0x000994E1}},
            {0x001628D4, new List<int>() {0x00099634, 0x000E0A38, 0x000E0A63}},
            {0x00162900, new List<int>() {0x0009972E}},
            {0x00162910, new List<int>() {0x00099738}},
            {0x0016293C, new List<int>() {0x00099747}},
            {0x0016294C, new List<int>() {0x00099751}},
            {0x0016298C, new List<int>() {0x0009A23F}},
            {0x00162994, new List<int>() {0x0009A291}},
            {0x001629A4, new List<int>() {0x0009A298}},
            {0x001629AC, new List<int>() {0x0009A2EA}},
            {0x00162AC0, new List<int>() {0x001D83BC}},
            {0x00162AC8, new List<int>() {0x001D83A8}},
            {0x00162AD0, new List<int>() {0x001D839C}},
            {0x00162AD8, new List<int>() {0x001D8398}},
            {0x00162AE0, new List<int>() {0x001D8394}},
            {0x00162AE4, new List<int>() {0x001D838C, 0x001D83B0}},
            {0x00162AEC, new List<int>() {0x001D8388, 0x001D83AC}},
            {0x00162AF4, new List<int>() {0x001D8384}},
            {0x00162AFC, new List<int>() {0x001D8380}},
            {0x00162B04, new List<int>() {0x0009AD8E}},
            {0x00162B10, new List<int>() {0x0009AD9D}},
            {0x00162B24, new List<int>() {0x0009ADBA}},
            {0x00162B30, new List<int>() {0x0009ADC9}},
            {0x00162B48, new List<int>() {0x0009ADE6}},
            {0x00162B58, new List<int>() {0x0009AE12}},
            {0x00162B64, new List<int>() {0x0009AE21}},
            {0x00162B74, new List<int>() {0x0009AE3E}},
            {0x00162B88, new List<int>() {0x0009B1EA}},
            {0x00162BA0, new List<int>() {0x0009B1F1}},
            {0x00162BB4, new List<int>() {0x0009B72A}},
            {0x00162BD4, new List<int>() {0x0009B731}},
            {0x00162BF4, new List<int>() {0x0009B755}},
            {0x00162C08, new List<int>() {0x0009B76B}},
            {0x00162C1C, new List<int>() {0x0009B7C2}},
            {0x00162C38, new List<int>() {0x0009B7C9}},
            {0x00162C50, new List<int>() {0x0009B83A}},
            {0x00162C64, new List<int>() {0x0009B841}},
            {0x00162C80, new List<int>() {0x0009B8AA}},
            {0x00162C98, new List<int>() {0x0009B8B6}},
            {0x00162CB4, new List<int>() {0x0009BBD3}},
            {0x00162CDC, new List<int>() {0x0009BBDA}},
            {0x00162D10, new List<int>() {0x0009BD81}},
            {0x00162D34, new List<int>() {0x0009BDA4}},
            {0x00162D6C, new List<int>() {0x0009BE56}},
            {0x00162D94, new List<int>() {0x0009BE89}},
            {0x00162DC0, new List<int>() {0x0009BEC6, 0x0009C9DF}},
            {0x00162DD8, new List<int>() {0x0009BECD, 0x0009C9EB}},
            {0x00162DF0, new List<int>() {0x0009C51B}},
            {0x00162E14, new List<int>() {0x0009C551}},
            {0x00162E4C, new List<int>() {0x0009C616}},
            {0x00162E70, new List<int>() {0x0009C64C}},
            {0x00162E9C, new List<int>() {0x0009C768}},
            {0x00162EB0, new List<int>() {0x0009C777}},
            {0x00162EC8, new List<int>() {0x0009C741}},
            {0x00162EE0, new List<int>() {0x0009C99E}},
            {0x00162F24, new List<int>() {0x0009C9AA}},
            {0x00162F5C, new List<int>() {0x0009D3B5}},
            {0x00162F90, new List<int>() {0x0009D416}},
            {0x00162FA4, new List<int>() {0x0009D827}},
            {0x00162FB0, new List<int>() {0x0009D834}},
            {0x00162FC4, new List<int>() {0x0009D913}},
            {0x00162FD8, new List<int>() {0x0009D925}},
            {0x00162FE0, new List<int>() {0x0009D986}},
            {0x00162FEC, new List<int>() {0x0009D98D}},
            {0x00163000, new List<int>() {0x0009DAE3}},
            {0x00163038, new List<int>() {0x0009DC5A, 0x0009E374, 0x000C5753}},
            {0x0016303C, new List<int>() {0x0009DC62, 0x0009E37C, 0x000C575B}},
            {0x00163040, new List<int>() {0x0009DC6A, 0x0009E384, 0x000C5763}},
            {0x00163044, new List<int>() {0x0009DC72, 0x0009E38C, 0x000C576B}},
            {0x00163048, new List<int>() {0x0009DC7A, 0x0009E394, 0x000C5773}},
            {0x0016304C, new List<int>() {0x0009DC82, 0x0009E39C, 0x000C577B}},
            {0x00163050, new List<int>() {0x0009DC8A, 0x0009E3A4, 0x000C5783}},
            {0x00163054, new List<int>() {0x0009DC92, 0x0009E3AC, 0x000C578B}},
            {0x00163058, new List<int>() {0x0009DC9A, 0x0009E3B4, 0x000C5793}},
            {0x0016305C, new List<int>() {0x0009DD62}},
            {0x0016306C, new List<int>() {0x0009DE02}},
            {0x0016308C, new List<int>() {0x0009DE2F}},
            {0x0016309C, new List<int>() {0x0009DE56}},
            {0x001630D8, new List<int>() {0x0009DE8F}},
            {0x001630EC, new List<int>() {0x0009DEB4}},
            {0x00163108, new List<int>() {0x0009DEE0}},
            {0x00163124, new List<int>() {0x0009DF07}},
            {0x00163140, new List<int>() {0x0009E097}},
            {0x0016314C, new List<int>() {0x0009E142}},
            {0x00163160, new List<int>() {0x0009E3C0}},
            {0x00163168, new List<int>() {0x0009E3C7}},
            {0x00163174, new List<int>() {0x0009E47F}},
            {0x00163198, new List<int>() {0x0009E486}},
            {0x001631BC, new List<int>() {0x0009E493}},
            {0x001631E4, new List<int>() {0x0009E49A}},
            {0x00163208, new List<int>() {0x0009E57D}},
            {0x00163250, new List<int>() {0x0009E5B4}},
            {0x00163294, new List<int>() {0x0009E611}},
            {0x001632BC, new List<int>() {0x0009E618}},
            {0x001632E0, new List<int>() {0x0009E635}},
            {0x001632EC, new List<int>() {0x0009E63C}},
            {0x001632FC, new List<int>() {0x0009E6AF}},
            {0x0016332C, new List<int>() {0x0009E6C0}},
            {0x00163358, new List<int>() {0x0009E748}},
            {0x00163380, new List<int>() {0x0009E763}},
            {0x001633B8, new List<int>() {0x0009E781}},
            {0x001633E4, new List<int>() {0x0009E788}},
            {0x00163418, new List<int>() {0x0009E7D1}},
            {0x00163444, new List<int>() {0x0009E7E2}},
            {0x00163640, new List<int>() {0x0009FC77}},
            {0x00163658, new List<int>() {0x0009FC7E, 0x000A0AA2}},
            {0x00163670, new List<int>() {0x0009FFD1}},
            {0x00163688, new List<int>() {0x000A002D}},
            {0x001636AC, new List<int>() {0x000A0284}},
            {0x001636E8, new List<int>() {0x000A02B8}},
            {0x00163730, new List<int>() {0x000A037F}},
            {0x00163780, new List<int>() {0x000A03B7}},
            {0x001637CC, new List<int>() {0x000A03F2}},
            {0x0016380C, new List<int>() {0x000A03F9}},
            {0x0016384C, new List<int>() {0x000A0A93}},
            {0x00163864, new List<int>() {0x000A0AC1}},
            {0x00163880, new List<int>() {0x000A0AD7}},
            {0x001638A4, new List<int>() {0x000A0C3A}},
            {0x001638B8, new List<int>() {0x000A0C50}},
            {0x00164B4C, new List<int>() {0x000BC9AE}},
            {0x00164B88, new List<int>() {0x000BC9BA}},
            {0x00164BC4, new List<int>() {0x000BCA51}},
            {0x00164C00, new List<int>() {0x000BCA5D}},
            {0x00164C40, new List<int>() {0x000BCD57, 0x000BCEEC}},
            {0x00164C78, new List<int>() {0x000BCD63, 0x000BCEF8}},
            {0x00164CC8, new List<int>() {0x000BCE61}},
            {0x00164CFC, new List<int>() {0x000BCE70}},
            {0x00164D2C, new List<int>() {0x000BCE35}},
            {0x00164D68, new List<int>() {0x000BCE7F}},
            {0x00164DA8, new List<int>() {0x000BD1DA, 0x000BD4BB}},
            {0x00164DC0, new List<int>() {0x000BD1FF, 0x000BD4CE}},
            {0x00164DE0, new List<int>() {0x000BD275}},
            {0x00164E0C, new List<int>() {0x000BD29A}},
            {0x00164E48, new List<int>() {0x000BD329}},
            {0x00164E88, new List<int>() {0x000BD34E}},
            {0x00164EE0, new List<int>() {0x000BD3F6}},
            {0x00164F38, new List<int>() {0x000BD41B}},
            {0x00164FA8, new List<int>() {0x000BD51A}},
            {0x00164FB8, new List<int>() {0x000BD52D}},
            {0x00164FD0, new List<int>() {0x000BD584}},
            {0x00164FE0, new List<int>() {0x000BD597}},
            {0x00164FF4, new List<int>() {0x000BD70C}},
            {0x001654B0, new List<int>() {0x001782F8}},
            {0x001654B8, new List<int>() {0x001782F4}},
            {0x001654C0, new List<int>() {0x001782F0}},
            {0x001654CC, new List<int>() {0x001782EC}},
            {0x001654DC, new List<int>() {0x001782E8}},
            {0x001654F0, new List<int>() {0x001782E4}},
            {0x00165504, new List<int>() {0x001782E0}},
            {0x00165518, new List<int>() {0x001782DC}},
            {0x0016552C, new List<int>() {0x001782D8}},
            {0x00165540, new List<int>() {0x001782D4}},
            {0x00165554, new List<int>() {0x001782D0}},
            {0x00165568, new List<int>() {0x001782CC}},
            {0x0016557C, new List<int>() {0x001782C8}},
            {0x0016558C, new List<int>() {0x001782C4}},
            {0x0016559C, new List<int>() {0x001782C0}},
            {0x001655AC, new List<int>() {0x001782BC}},
            {0x001655BC, new List<int>() {0x001782B8}},
            {0x001655CC, new List<int>() {0x001782B4}},
            {0x001655DC, new List<int>() {0x001782B0}},
            {0x001655EC, new List<int>() {0x001782AC}},
            {0x001655FC, new List<int>() {0x001782A8}},
            {0x0016560C, new List<int>() {0x001782A4}},
            {0x0016561C, new List<int>() {0x001782A0}},
            {0x00165634, new List<int>() {0x0017829C}},
            {0x00165648, new List<int>() {0x00178298}},
            {0x0016565C, new List<int>() {0x0017827C}},
            {0x00165664, new List<int>() {0x00178278}},
            {0x0016566C, new List<int>() {0x00178274}},
            {0x00165678, new List<int>() {0x00178270}},
            {0x00165688, new List<int>() {0x0017826C}},
            {0x0016569C, new List<int>() {0x00178268}},
            {0x001656B0, new List<int>() {0x00178264}},
            {0x001656B8, new List<int>() {0x00178260}},
            {0x001656C0, new List<int>() {0x0017825C}},
            {0x001656C8, new List<int>() {0x00178258}},
            {0x001656D0, new List<int>() {0x00178254}},
            {0x001656DC, new List<int>() {0x00178250}},
            {0x001656E8, new List<int>() {0x0017824C}},
            {0x001656F4, new List<int>() {0x00178248}},
            {0x00165700, new List<int>() {0x00178234}},
            {0x0016571C, new List<int>() {0x00178230}},
            {0x00165730, new List<int>() {0x0017822C}},
            {0x00165750, new List<int>() {0x00178228}},
            {0x0016576C, new List<int>() {0x00178224}},
            {0x00165790, new List<int>() {0x0017821C}},
            {0x001657A4, new List<int>() {0x00178218}},
            {0x001657B8, new List<int>() {0x00178214}},
            {0x001657CC, new List<int>() {0x00178210}},
            {0x001657E0, new List<int>() {0x0017820C}},
            {0x001657F4, new List<int>() {0x001781FC}},
            {0x00165800, new List<int>() {0x001781F8}},
            {0x00165810, new List<int>() {0x001781F4}},
            {0x0016581C, new List<int>() {0x001781F0}},
            {0x00165828, new List<int>() {0x001781EC}},
            {0x00165834, new List<int>() {0x001781E8}},
            {0x00165840, new List<int>() {0x001781E4}},
            {0x00165850, new List<int>() {0x001781E0}},
            {0x00165858, new List<int>() {0x001781DC}},
            {0x00165860, new List<int>() {0x001781D8}},
            {0x00165868, new List<int>() {0x001781D4}},
            {0x00165870, new List<int>() {0x001781D0}},
            {0x00165984, new List<int>() {0x000C20E1}},
            {0x0016598C, new List<int>() {0x000C20F9}},
            {0x00165994, new List<int>() {0x000C2108}},
            {0x001659A0, new List<int>() {0x000C210F}},
            {0x001659B0, new List<int>() {0x000C2127}},
            {0x001659C0, new List<int>() {0x000C2136}},
            {0x001659D0, new List<int>() {0x000C217E}},
            {0x001659E0, new List<int>() {0x000C2199, 0x000C2222}},
            {0x001659F8, new List<int>() {0x000C21A0, 0x000C2229}},
            {0x00165A10, new List<int>() {0x000C21E7}},
            {0x00165A20, new List<int>() {0x000C2207}},
            {0x00165A38, new List<int>() {0x000C2265}},
            {0x00165A44, new List<int>() {0x000C2270}},
            {0x00165A50, new List<int>() {0x000C227B}},
            {0x00165A58, new List<int>() {0x000C2286}},
            {0x00165A60, new List<int>() {0x000C228F}},
            {0x00165A7C, new List<int>() {0x000C22BF}},
            {0x00165A88, new List<int>() {0x000C22CA}},
            {0x00165A94, new List<int>() {0x000C22D5}},
            {0x00165A9C, new List<int>() {0x000C22E0, 0x000C41CC}},
            {0x00165AA8, new List<int>() {0x000C22E9}},
            {0x00165AE4, new List<int>() {0x000C404B}},
            {0x00165AEC, new List<int>() {0x000C4085}},
            {0x00165AF8, new List<int>() {0x000C4094}},
            {0x00165B00, new List<int>() {0x000C40B0}},
            {0x00165B08, new List<int>() {0x000C412F}},
            {0x00165B18, new List<int>() {0x000C414B}},
            {0x00165B28, new List<int>() {0x000C416B}},
            {0x00165B34, new List<int>() {0x000C417D}},
            {0x00165B40, new List<int>() {0x000C4190}},
            {0x00165B4C, new List<int>() {0x000C41A3}},
            {0x00165B58, new List<int>() {0x000C41C5}},
            {0x00165B68, new List<int>() {0x000C57A9}},
            {0x00165B84, new List<int>() {0x000C57B5}},
            {0x00166200, new List<int>() {0x000DB994}},
            {0x0016620C, new List<int>() {0x000DB99B}},
            {0x0016621C, new List<int>() {0x000DB9C0, 0x000DBA43, 0x000DBAC6, 0x000DBB49, 0x000DBBCC}},
            {0x00166228, new List<int>() {0x000DB9C7, 0x000DBA4A, 0x000DBACD, 0x000DBB50, 0x000DBBD3}},
            {0x00166234, new List<int>() {0x000DB9DB, 0x000DBA5E, 0x000DBAE1, 0x000DBB64, 0x000DBBE7}},
            {0x00166248, new List<int>() {0x000DB9E2, 0x000DBA65, 0x000DBAE8, 0x000DBB6B, 0x000DBBEE}},
            {0x00166258, new List<int>() {0x000DBA1C, 0x000DFFDA, 0x000E0146, 0x000E0223}},
            {0x00166264, new List<int>() {0x000DBA9F}},
            {0x0016626C, new List<int>() {0x000DBB1B}},
            {0x00166278, new List<int>() {0x000DBB22}},
            {0x00166280, new List<int>() {0x000DBBA5}},
            {0x0016628C, new List<int>() {0x000DBC21}},
            {0x0016629C, new List<int>() {0x000DBC28}},
            {0x001662A8, new List<int>() {0x000DBD0A}},
            {0x001662B0, new List<int>() {0x000DBD11}},
            {0x001662B8, new List<int>() {0x000DC3A5}},
            {0x001662D4, new List<int>() {0x000DC3B4}},
            {0x001662FC, new List<int>() {0x000DC3D1}},
            {0x00166320, new List<int>() {0x000DC3E0}},
            {0x00166358, new List<int>() {0x000DC3FD}},
            {0x001663A8, new List<int>() {0x000DC40C}},
            {0x00166400, new List<int>() {0x000DC429}},
            {0x0016642C, new List<int>() {0x000DC438}},
            {0x00166464, new List<int>() {0x000DC455}},
            {0x00166490, new List<int>() {0x000DC464}},
            {0x001664C0, new List<int>() {0x000DC493}},
            {0x00166520, new List<int>() {0x000DC4A7}},
            {0x00166580, new List<int>() {0x000DC4BB}},
            {0x001665D0, new List<int>() {0x000DC4CF}},
            {0x00166628, new List<int>() {0x000DC4F3}},
            {0x00166680, new List<int>() {0x000DC507}},
            {0x001666D8, new List<int>() {0x000DC51B}},
            {0x00166740, new List<int>() {0x000DC52F}},
            {0x001667A4, new List<int>() {0x000DC551}},
            {0x001667C8, new List<int>() {0x000DC560}},
            {0x001668AC, new List<int>() {0x000DF998}},
            {0x001668BC, new List<int>() {0x000DFAE1}},
            {0x001668D0, new List<int>() {0x000DFB20}},
            {0x001668E4, new List<int>() {0x000DFB66}},
            {0x001668F8, new List<int>() {0x000DFB99}},
            {0x0016690C, new List<int>() {0x000DFBAC}},
            {0x00166920, new List<int>() {0x000DFBBF}},
            {0x00166958, new List<int>() {0x000DFCD1}},
            {0x00166964, new List<int>() {0x000DFCDB}},
            {0x00166974, new List<int>() {0x000DFCE2}},
            {0x00166980, new List<int>() {0x000DFCEC}},
            {0x0016698C, new List<int>() {0x000DFFCD, 0x000E0139, 0x000E0212}},
            {0x0016699C, new List<int>() {0x000E07A2, 0x000E0813}},
            {0x001669A8, new List<int>() {0x000E07AC, 0x000E0824}},
            {0x001669B4, new List<int>() {0x000E0A42, 0x000E0ABC}},
            {0x001669BC, new List<int>() {0x000E0C48}},
            {0x001669C8, new List<int>() {0x000E0C4F}},
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
                    input.Seek(kvp.Key, SeekOrigin.Begin);
                    var sub = ReadSubtitle(input, kvp.Key, false);
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
                                    output.Seek(reference, SeekOrigin.Begin);
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
    }
}
