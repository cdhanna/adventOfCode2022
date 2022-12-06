using System.Security.Principal;

namespace AdventOfCode;

public class Day6
{
    
    
    //the start of a packet is indicated by a sequence of four characters that are all different.
    [TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7)]
    [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz", 5)]
    [TestCase("nppdvjthqldpwncqszvftbrmjlhg", 6)]
    [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10)]
    [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]
    [TestCase(input, 1647)]
    public void Part1(string input, int expected)
    {
        var index = 0;
        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];

            string slice = null;
            if (i > 3)
            {
                slice = input.Substring(i - 4, 4);
                if (new HashSet<char>(slice).Count == 4)
                {
                    index = i;
                    break;
                }
            }
            
        }
        Assert.AreEqual(expected, index);
    }
    
    [TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19)]
    [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz", 23)]
    [TestCase("nppdvjthqldpwncqszvftbrmjlhg", 23)]
    [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29)]
    [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26)]
    [TestCase(input, 2447)]
    public void Part2(string input, int expected)
    {
        var index = 0;
        var set = new HashSet<char>();
        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];

            string slice = null;
            if (i > 13)
            {
                slice = input.Substring(i - 14, 14);
                set.Clear();
                for (var j = 0; j < slice.Length; j++)
                {
                    set.Add(slice[j]);
                }
                if (set.Count == 14)
                {
                    index = i;
                    break;
                }
            }
            
        }
        Assert.AreEqual(expected, index);
    }
    
    
    #region sample

    public const string sample = @"mjqjpqmgbljsphdztnvjfqwrcgsmlb";

    #endregion
    
    #region input

    public const string input = @"dfsfmfbbbjnbbpddfcfjcjbjwjqqbtbntnhtnncfnfpnffwpphwppvbvtvztzszfzhhnqnvnpppmzmczcmzczbznnbssbhhvghhzqzvzttjvtvcccdhccmvccgdgttghthppwlwqlwwcswspsfsdfddhwwzlwzwttlglttsjjthhgcgfcfhfhtftrffwcfcsfccnntmmpvpgpfgfsgfflgfgmffbmmqsmqmmcqqmjjzczwczzpjzzgtgnnqqvdqvqllbttftgtztcztttlslrslrlfldljlwjjnvjjmrrhdrhhflhffgnfnjffmvmvjjspsvvrcchhgngwwdwbwwfhwffmggbsgspsjsfjjdwwbtbdtbdbgdbbbzhzvhhwpwlwfwfswwrgggmggssfwwdrwrccssjttltrrnggdbggvzzzrfzfwzfffnfnmnnzmnznpzzdtzdttbvvgffmnfmfmbbqppcgpgtgpgggcwwlpplslbslblhlmlfmlmldmmjhjjgllglhlssswcssprrqrjrhrvrffpfnpfnppzgztgtdgtdggftgftggqrgrnnqpnpgnpnggsjsvscsnswshwswfwfwqffcrrmprrrcgrrggmnnmzmwzmwzzgzzhnzzthtggtmmdrmrbmmqwwjswwphhzvvjqjrrvzrrmssdvdlvvlhvvjggbwbrwwjqjqmmfllttvpttdvtdtddrttpltpllzrlrwlrrvcvpcphcccqwwtpwwbmmtntfntftvfvfqvvwnvwwvhhlccvrcrlclcqcvqqsvvfjjqmmfhfvvctczzlhlsslpspfsswnsnzsspzpccmssmpmbpbmmvnvsnvsszttrppvgpgnncllrvlrvllcvlvwvhvghgvvnhvvmffclfccvwcvccrwrhhrffqrqcrqqhbqhqssmzsmzsmmzrztzllmclmmbgmbbrmrgrwrgwrrgngvvtqqpbqpppgbgccsjjcmmtdmdjmmjcchvchhbccnjngjjnwjnnrvnrrsqshshszzqnngbnnrnggvwwlzwwlqwqmqdqrdqqrhqhffpgffqgqdggfjgglfglgvgjvjmjtthghrhvhgvhvnncnwcnwntwntnjjqjwwrdwddsggfddnhnsnvsstbbjddfhddhbddwbwbnbrnnrhrqhqdhdllvbbnzzbmbttpjjngnqqcffrsrnsnnhzzgllgvlvcchfhzzmzrmrggdvgvgqqwnqqpdqqvjjvnvhnhhgvhhgllhlzlclbcccwppztzhhvmvzzzsbzbppvdvdtdtdvdsdlsscnnqhnhgngghrrzprzzpddmvvhcvvprplpnlppscctzthtptspttmftmftfjjdfdjfjrfrfbfcchmhnnbddwzwvvpvrvnnslnlnhlnhlltslttqpqvvgzvzsstcstsrtrbtbbzmmzrzqrzqqnmqnnpjpttwgtgzgqqgmgnnzgzrggpbggvssqvssmhmshmssvlvlmvvnhnhddwbbllffgbffbbztthbhdhdghhrccfmfrfmmbdbfdbffzfgfrfqqptpgpjjlvvbjjdzdbbszslzlldnngwgddbmbpbwwhphnhmhhlthhgfhfvvpmvmhvmvdvsddsjtzvfmpsrwrrzgcvnnllfjmvfptwncppfmgqbfzrdpnfddghsqfmnqfwfslrsgjmqtfqwhdddsbhtbtpswcbfppcbhzfzbsqljzndcsrlhrrtstgfhhfsqqrwgnncsmstdmjvfjhqnrczlftzzzhqdzjdcdqcgfpmbqntdhzcvbtpssrvmgjwzwfvtpsrsrwrvrsjgrmzqzvbttscldsnnwzvmlztnnpdjrwvhshpdwgvhmlrnhtfccjnldlnhtfncfjjjztjmhrdqpvhggtqzwjsvwdzhdmwhsmgzjcwzqzlwbrlzsmlwhpjvflnppvrbgrsblmjpnqvgpjbpwbjgjqzwvjbgcplccjgbfwlblzfjqpwszbqbcnlbmfmqpmgspscgfdgfwnmcdzcqnjznndjcvlblszcnpflbjqltpfzhffdbwbshtpnwwlspltpcrvbdtflwbjrfnvrflqpgqtjzqwmmsdvtsmgjtrtbrzchwhpfsznjqcbrjcvwqgrcsqpvfzhrdlmnvvhjzpgpnlrmqfvcnlrlcfjblfcgvngdjfdczsrtnnwjndsfcsdlhdnbtplfnhsmmbldmsjwcblghhgqwbnjvqbqhddrmrtncvwnchsfpddzgrrtzntmwnmdwlrvnjgnzjqvptztnqnqmcjmmrhmtstgdvhffbbmphnbtsdmpmscsfdbnfnchrhhjpsfhhswfszgqfcbdbgnrqhrflpfgfgdcrjvrwbvsfmzzhvzvqzgshcqzlfcljnlgshdlhwdchhhvwlshwdrgjfbnptqqglbpcfgrmqjhqvlbzdwgnzfzlpcpjzqwhbfjljszvjdsrmfzntgnjflhnwhpfrlbpvgzmbqwzlgphmbvbfdqfgqqbhzzvrjftnwjzhlrqccwcfzvntscnbfcsrqqnvlvhszpgwtrzjrqbtslctbhtbczwtmsgwczncbjmzqvnthpwjmsbsjnfpsmghnvtqjjnjfwtnmlthrlcpqhjpnvnbbnwrdjfshwhpdwmsbngfhbhsqphlqspcgzwrgfjmqqtlsfgnvqtdgnhmdvvqzjwlhsnvjczbssrnlhwdmdthmtprjjfttfzbbswfwsvvslfnbcvprzhtcqwdrzjrnjjctqfsjrsddlhzcnstqfppjlqhvcbjbfwndwdtdfvnlwgvdvhzzrqthdhdmddfdwschmpwwrnlgsldzhgjrlmtzrnrrtqfctvbncpcjlsvnwjvhgnbshhwlqhtjghvrctlvngjgjrlgshhwscrdvzjqtfrrbssvqlcjjdljpmlzfqqnqmffpsbvgcqzqdjwczbqwjgfvpdgjglnqdshppcsqcmszhrbcpnhjnczlhwsbnfnzsczjvmftngqcvhgpgwlzbmjnmmdvdjfrcwnjrncvfstrvvqsstphmqdpwjqhzgppmgwlgjhgfwqgmjrlsvqpfqznvbqzgtngvpbpttzvngjwtrjgdnvdggzlnmpgbzhtnfnrhgwvhnpqdwfdvvftzllpqblgdglclwtwbchlvwcmmvtchlntlghztfvgfjczcbqmzgnqmrjcmqvjmjhfpztjcvclblmrctzfmsdfvfpwdcbsglgrsjqcgtcblhbtgcgjlwhhqnwhdnwzlhvphtvmlfnbfmgpnnbzqtdtzqrbhfljlwstlbscnrsvhrbrcthvzcngrttddcjqhtmsfpsgldgtsgjtprsttlssmrrfjmrddqvnqcfmphbnjtdsqvptrdzqbqfjqtnrqtjgdpbrlzrlvwcbcqbcmncfmwcpdhgpjdrdcmrqnflsqbllrqslmhsljwghnwcjwvhchcnlgppmphbqtcdfzjpcqcgsjzvmgfjgfsvmvjfqvtpffbpmhnnnrjmqhhhrhrqfqdwzdvzssslzvqhngdpszztgrvjntcpzhbfmhbpvcndsjbtnwgpmztpbrtjmfqrsvndrspdqmlsbldgghfflncszhnsttfslvwhvfnsmmhbbvjqslsjfqplndndwmbmvgchgvhzclrcnhvgbgmpctrggvqpvqvgvncmdwhpmwhzwhlgsnlnwggfbbvdvqrrsmhwzrrpgrjfshzgzjpfwjhpqmqhbjlwhwsfszlshpzprvgprlprrvlcrmbttjrpqsrdcdfwdrzbcfjpvrlrjjdwhbspqmrblvtldqdhtjtjphpqswgvqfftdgqrtjgsmthlhvlcqrwlqtthwjgrcpwcnsqtssqzpzqptrwjjdfchfmmsrsccnlvqbdmbcdjmhpgvnnlttfhggfphvbwqtcztbnsflztcfpbcpjbcmsplhjdbsmzhgnmfrhscmwmfqbljvhgllvvgqzphzbswdzlhmpcvnntczrcnqvlphhjdjjjnhfzzcjjsdlfccwvswvjfgvmlnpvjvcbpglsgtpj";

    #endregion
}