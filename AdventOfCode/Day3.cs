namespace AdventOfCode;

public class Day3
{

    int Prio(char x)
    {
        var p = ((int)x) - 96;
        if (p < 0)
        {
            p += 58;
        }

        return p;
    }
    
    char FindCommon(string[] segments)
    {
        var sets = new HashSet<char>[segments.Length];

        for (var i = 0; i < segments.Length; i++)
        {
            var set = sets[i] = new HashSet<char>();
            var segment = segments[i];
            for (var j = 0; j < segment.Length; j++)
            {
                var c = segment[j];
                set.Add(c);
            }
        }
            
        // now, among all the histograms, we 
        var x = sets[0];
        for (var i = 1; i < sets.Length; i++)
        {
            x = new HashSet<char>(x.Intersect(sets[i]));
        }

        var count = x.Count;
        return x.First();
    }
    
    [TestCase(sample, 157)]
    [TestCase(input, 8053)]
    public void Part1(string input, int expected)
    {
        var lines = input.Split("\n");

        var total = 0;
        foreach (var line in lines)
        {
            var length = line.Length / 2;
            var part1 = line.Substring(0, length);
            var part2 = line.Substring(length);

            var foundCommon = false;
            char common = FindCommon(new string[] { part1, part2 });

            var commonPrio = Prio(common);
            total += commonPrio;
        }
        
        Assert.AreEqual(expected, total);
    }
    
    
    [TestCase(sample, 70)]
    [TestCase(input, 2425)]
    public void Part2(string input, int expected)
    {
        var lines = input.Split("\n");
        var total = 0;

        for (var l = 0; l < lines.Length; l+= 3)
        {
            char common = FindCommon(new string[]{lines[l], lines[l+1], lines[l+2]});
            var commonPrio = Prio(common);
            total += commonPrio;
        }
        
        
        Assert.AreEqual(expected, total);
    }

    #region sampleInput

    public const string sample = @"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw";

    #endregion
    
    #region input

    public const string input = @"sfDRhjhHsHhgWPJvPmmQnmPqnW
pTddGVwcpMTTCdnQJqqQqqqVtVms
MdZCZGdcrCNRFZRhFssL
CttWnSnNfSnCHsWrTlTPPpPCTRrLpl
DgqqghjqJBVgDMTPGVlRGwbfLLGP
cgqBBhjqcBdMcWQcQNnNzsfv
lnDWMgTLlTFlHHgDDgngWFnlBWNcBQrdjcrrdQrPBrdjhWhj
JqSVRRVmmRqJJbZGGJqJvbmBNcjPNQNssQPhSSdwPwwwQr
bCRJqGJJmzmJZRCmFNTLTttTzfFfLglf
SPWvWMvCSPcjzjDbcwfjTl
lLNRNLqhhQVQJlRjrjrDwTzzqzzfrb
GRnRVhRJLFnnhtJQNVdLdLgWCmmZlMlgSCSWSgpZtPBM
pTGFrLFTFWFprLDBmLbSbtmBDb
MqjwqJwZlqJjHlqjHHPmSbsffDmsStDnHnQmsm
ZPJjVPZbVMRRPZwMJZVMNJMcGWpWFcWFFNFGrWTzWzFrzG
MffZZtMTnTtSZLdfgSMtCHSbmWsGwbHGSqvmCqWb
lzpQhrhphhlzDDhRPmBvqHGRbBbwbbssCB
JJljpvhFrrjhptnddMJfdtgnMT
drCtpNLCLpTpJSdswQhvDbHZHDLDHQ
WmWgBWRcRzVVWVBgBBnnlfgWHjmvjQhwbbbshQvZDQQjsHZC
fqBzggWPPzBWBzffcfnMJdtFtrpqrGMpdCCTdM
JwJWqNBNNdzzBSzGsqbdNJbVMpptPmZMrVZrrZMtPmPwDp
THgfgffffHRhQRLVMGVQmGtLDGmM
TjGchhlHhGfhRHgRgWJSqzJWWlqNSzsWSJ
dNmPlzdvdspsFWwQmG
bhZSbVJBJnLNTnwWVHMGwQsGMFFw
RnnbbTnSnSSTTnLfRCCPqDPDNDlfCfDDcr
lhhTcnPchPPHCCStwWTHbS
GDRFNqlQJsGJqGJDqVNsqssDQBSZWHQBCZHHwbZbtHtwCbZW
RNJrFJFJDrmqsVjNmDvrvfzfffcvdpMrvlfh
DtLdNGHNfwBJQwgCrncgpSpcnlfC
sGqWPMPTvPPhTjjsqRqPvSlzFFpjnnSrjczprgllFF
vVPGPGMbPGqTRWsMqZhqvbZNLLmLQddQdmBtBwBNBwNB
ChVzhwpdpqHhtNmHHNHt
QsjGTQcTWQjfjbssQDPmHgfrrVrPZnntZD
jTGJSGvWJwqlvlCBqV
pRVcSRffTPfBWfNVfWBWdJdwhvvwGjjFmGvhLTdh
qsrHqtbDZqsnsZqCQDtHnQQLwFvJFhGJwddvwLcCwJJJdv
sgcqHnzqqzgnHnqrstZzqsnSPllRlVVSNpWVNVRgMBlVPW
WRQTtHrTrrDRvQDHrbtJlpdhLdGsDllfspLpphhs
GzCSqCSmSmVSpsljphlpsL
gVwCVGzmNmCNRQTvJQJHnvwQ
psBDsswNjBcqtHtsTHsqtM
vQrPqZPmvgQZrfgmPrfJQlLvnLzVHLnSnnTLTnnHMt
PRCJRPgmqrmZmmqQQDDRwwNwjwDwpFjDDW
VtBgCqbVjPbSbHtPRdrssZMFZlrRsBRw
LzWmhcDqTDvnDWTFMrwRvwFrGZdGlZ
zJLczJDnWDpDNzmDczWLzWzWNCbbttHPCHbSVbqgHSCjffQf
TjTfvJjjvcjTQcDzMDfQTLLbLgVVVhMrWWblghbbLN
dZHFSpqpqpbWrhhlWh
dFwPBHqFSqwZSmZmSlqZjTvvJTmzcJsTTTjfvsfQ
qqqNTlfjzbMGJlHMSZtZzZgRZDgZDzdS
nLCCVVcmgCdZdSlg
cmQscBVpFsppsVlffQGJHjlWHJGq
whwVGGZhVLwhsFFDCTrDccCctrcctL
CzSSvPSzTBStSWND
HllCHvHJPPqjCPvbfdvvbsmhdRRRsmZhsRdMFQMRFQ
gFCfCVfCsLCftsBsDbSHrbJRJJtrmrddrd
hqQpqWhlNlpMlppfdTRhmbmTdbdHJH
NGvvjvpvpfGgGGDCZZBg
rmBtgdddtqmmrqBGbLGJlmctWWvbNzvfpsVVfzzSVSTsWNpz
RPDRjMhDFljvsvzlSs
wDChhnCQwDwmgJclqrgm
WHrrDbWHQPzNrrRVMQJMQGvvsvvjnDLvfsjsvwfGws
dhdhZhcphZZZmtFFTcZSmcZsnfqjLRnngnpwnGfqfvvnRs
FZcZhtmhFCttldFlSSmlSthQJzNVMbRPWWPJzrbbJCNMPH
wMFBpvTppLpwfNfjggmNmGTj
ddSDDbGHnRDQDZRHSZSdRZDQzjjrzNNNfnmNrllhgglhfgWg
bCQqsJqGDZCHbppwvtVMMvJcLL
pSpSVdLDFCvDDvCFvJgwjsJbNtmtJgSjmj
ZcWNNBQfwjsttsbc
WNQMBflQQNGQrFpFVVRDHpCMDR
PfPvqLphWpWLtZSWpWLPjwJbmDwJbbDbmJVjPQ
lQQnRGMllMjswrmwJM
ggRGFFGGdlGGzFFzzFFcNBvSfLQZShZTtdLWWZWSWZSp
lCfgHsVHJDdswNRmsMRQ
vccvvFVrPcvQNRdnmqdR
rctPBrPBTTpPFBLZZcCCgVHJHVjjbLHfCjfS
dfGdsGGrlFFlbWjfgblhJhLDLDDMLNvJNLLBnmLB
tSppwQQHSSVtwStSpZZVqRJmBDzLvwPmzJznBDDmLBBJ
cHvtpRSvptCRbbjrrcfjrWjf
BBdHdjgQdjMMsHJscFnrzLpLgznLFzcF
wvllmNmVvZfvmZWqcPptPztFSWLFGrrFnt
ZbCvqqmNvflfVTbZfNllsjHdjdhhHDTHRBJjjMsc
FNCPtPtgLFJwPwflFwSrLFcMczQZTbMVmzzVZMcNVVVb
DhRDdhpWQDZmzVSQ
vnhBSHppjRBHqpWvrPFtJLJlLvfLPF
nmcSnnWjmfCTcHPHJCvh
zdDdlrrzGFFLPtPhBBhH
NGNGrzrRrzphwwwMmqqfnsfZZNbSjQSN
lgznQGWQLQWlnSzHSQlwnlDhCbZhZhZChPChwDcDphcb
jTRvVVrMvmLCPNcNZhRNcD
jfftvsrVJLsVvJqsmfqjfjlQgzlQWFzHFGGBQgtFgnnH
sllpwssrsCwrTRgCHGCTcnZD
jjzJtSdhdzbJWhdQqLdzqSHmDZBGZmmcGGgBGTDRBQTD
SVjgVhgtbVzJPfFpvvNrNswV
StzdmmnnjSRRdhPPdZZd
VbTbCqFFMbZTFcNQLgRgQbvvRh
pGsqGGGfHGfZVffzwtrHnmJHllznrS
NLWJvtLjtLzBjNSvSMDCHfwHSlDMlSSHfZ
RTPTVmhpnprfcfgZwgRD
PnPGFhGsTphsFpdPnpVdmhwFvBJzbdWNtJJjtNJNjbvJtttN
RvmgjDqqjqRgZRMRDpQjQhWsbPLPFnPFFbVVLbdSbnPSvP
NwczHBrJTzcBJHrfWJBCJcrCdnPPPNSlLnsnbFnnLlbSFddS
rGJwCCCJHwBGGctGDtphQQMppQWZmRpD
RPhhSMqRccBDZPPPRhPcNZSzzTLJrWZLmVVQLWZdTQQJWL
nwggfwCvbjwvbwpzWLpWVLdrrrQVTm
vgnGGCsCtntbFsgqlRVMSqNVBDtPSc
mtstjJmvTNBcjRRCHCfH
gLpglwwlgHbZbgpgFrdBBBfdfSPBLSSrcS
GQGglGWWgMglQFHgbmTmNtDqnDDVJMDMNJ
ZMbBZfvVfFfBbMvfMhgbfDsrSTTszcldmTTPmcPFDz
QqQQnwrqWQpwRWWpWwJRwNzTTSPpzPPdTPpSPmdSscmS
GjjtJRWtwGQjRZVChZMjVCrMMf
fJNPTvDPTpHHTPwvjNNHDfTWthhgQQGdBddtlvMsMQMvQh
rFbZVZrLmLdGrrhMBQWg
FmnzVRFLqVqqVLVRRFZSFmTwfHHjHCNCCDGwjnCDDfNH
gQHHQJgCnNJnQFQPRbDQzLRR
mwrdpctWtrMvvrrWwGMmGWWPLzFFLSbLnDFsLPdDFbZRLz
vGcmGwBBMGtmmrvlMrGlqNghlVjCCnTHHCHgCCjh
LmLvVjVjsrmrtmmr
tfcnbScRnlMZtHQPCgSQssPdHC
RGGGGnRfcwnGbbJRBRcwJfnGtBDhhVptNhDvLLhVvvjBWNvT
dZWNQZgQbbNvdWGgZvbTfLrjtrPlGJfrLqLJlj
TMmDpwzmVMHpBLfrcccMfqLjct
SwnSBTDDTwwzwnnsFSZdRNbQZWRvgSCvbQ
WPgZgQLLbMgdBrdnGqqfdhVVvR
HzssNTzwlwHHcczwFjMFHjVGrqRqnVThVqrGrGTRqvrf
zFzcHFNlzJBLgMQLJCZL
nPLNcWtNtlLMccLlWdTjzzbBfBQSzqzBqPqS
RbbDZZrGRJhJjgJQSjCfqQCC
rrmRbDDwvZDpprbGrbDvtctlVVVHvdtlcMtWHMHV
DWrZJrQjWwFcrhzVzbpmpcVqhb
MFnFHMNSqbMpMMmG
FNngNRBRCgnHCCHRPvLNdgJWwJDlJJDssZDLWWlWQlsl
BQqNsGrbBCNbNCrMpGpbHhthRCDRDRJCmDVRhRJP
nfvWvcnSWncSTdzzFLJtRmhHmPPVPVTwwHHtTh
WfLfnfSJZJvdLFZWngfBMGMppGMrNBbGMpZjrj
rccMjBMVJcjjjNNqmmCf
LLspTTGsTGntsntTFwnNNfFqQmmNgNqfNQmZvQ
tpDTwlGDTGPPsbtsLsnnqGTJzJrJBzHzMVrMRzBMlShVBR
PsrNPRjjPbjzjLRWLbjmvtCnMntnpfmtNZNCNv
dDlfwwJllhJTcllScSCQvmtCnmtCmQmQmG
TTFcdhJwhBFfwJJHhdchVclrsbWsbzqLgbzrrjgVRgsqgW
vvcvvDJFcDZPTzwfcwSLczzScz
VNnnVVsqGNntsqtBRblqBndSfzCCRzwRfCHSjdfSjzSH
pppsMVlGGhhrZwMMDP
LltNHMZNHMfNnfgtLHWWbhWjcblSbVbcTWVP
vFmCZsqRRBqrVPWsWTWPWb
mQBqJRdqQBqQzzRQztgLgntGZttddLMggw
ZTCCrCWfGLGBWSwHvHHmHvmTTH
bllhnsbjDlqFfqjhnFRppwmvJppmpRRwMNSmmw
FlnFDjdtqhDdfZZBrtBrrPLt
CRCTHHJcCmJgTSTRcSMcRMVstssSrtprppVFtdrdspNb
jjllnvgBLqdsGprtqtFG
vQjzWnWZWjBLhjgwcccRJgZPCmJm
VRNmBBRNRFcCRcFVRSVSqZLLvvlLqvLfzfMhjJLC
TdHsHbDsbHMJLqlLzl
bgQGsgWWGGgbDgwGzBNSFrFtVSmwRRNFtn
pCCggQPPzWnvlDcWVHGJcNBl
LhsLMrwwGlnMBlNG
mmhwZmqSLwjLttnFbvgFTpPtPtgFCz
TtZSJzFZhZzTFcgFFcmRRmJJQllCHvPshVQsCrshsCssHVHW
GjGGDGqdGfbpDBjMdjpBjBNbVHtsWWPHlMlrrrrWWlVlVsCs
dBdDdfqLdBjjFRFScRStmLnL
GtVppGGPbVgTVFQrZzfrJfJJtMJr
DslmNmLsnmNHNNnnqQRZSJSQfqrJzSJn
BNljDHsHlvhmBshDljWsDWlHdgvpVTFggVgGcTTpvFPTzGCV
GRcnTRtcQTcBTsNtpvhFCmmFhZvFPC
bBJMgqWfdwBJfMPPPmvPqhmjvvPC
SMJMdJbdfwJgVglMWWVdcQnBzSQDzGGQzRQDTQSB
mvjVzLgTzVzvVjJrJgrlMhZRFTtRlRhMRRRtFZ
HGqnNNqfnHNGGfCHndBqnqfFlcppsJMZplMFpMtlscRlpC
qSnPGqqbnSdVrvQrrSJjSV
lWFSWZZvVqnqfnSrJzMcPDjJBJcBMPFJ
NGppNgHdHbRsHPbsgGspTwHTMcmMDdJMMmzBDcMBMDQmjMBd
TGGLRGwHsGtpHgHpNbpttwrvCnvCrqSSLvWqPqSSnWvn
jwcqBNNdZLjSfvPdddRlfb
CDVmsgMHCnnDnhVghmDnDCzRRrSrbrlTbsSTlzzlvzPb
gCCFmCWDnChGCFHnGCLBcwwjvZQZNtGqqNZc
LBDcNstdNJscccVDhLHNDHVtFvdldlFvCSnSvjSSbblgvZjF
rWznQqGMMrmmRZbbwvSFgjwbwm
RQnTQfWqqTzTLJJLVtBTsc
SvwCTHqCqqqHtwtnnHHDtWgrBQLzzVLLzSQVFhbrSFLL
cZmPNmPJdmPjPdcclRPPdhBCFVVVrQzQCCLbcgVbBV
fNlmfZfpfWMCtGGpnM
bSNssNssbPHVccPhclPGpP
ffQfZdZZBDDZgLvhmhzVmVppmlpGgh
jdQQQJRljSFFTWCT
lvlLtvnhnfvMgtrvWjmTmPPzjHcrmdcjdd
qCbssCJbppQZQbRJDQSZCJRpzhmcQjdcTBmmGzmdcmjGmdmT
SqwSbJZSpwwFJFDDbqtNVMwVMMlVNgNVlLhV
DqGFQGNMGMQwCcgtCJcr
sVfjWlzzVsmzVZsdVlHrhjppcgpjrhpphcSJ
LRdLsZBWWmlZldZRmzPDvDTTDMGTPFPvBTTc
jzzzpjgBzTDQQHPH
gLLtZVdCdsLfnbZCbdZtHDfHTJJPPmJJfmHQDJqD
bVtWndLtcZgnhsvMSBrMFrvBWNrB
sfqhLDcqfqRRqQhQRqMcvlJpJwFgzwpjplwbgpwzLz
CrGttnhTWtmSnGrtTtSCZGFzbgHHFFFjljHjZHHFwgwl
mBnrrTmWWCGStVCmMcDPcPBqcsRhcvPR
GLZLBNrGZdGGVgMVJVhnvn
dmWlcqcQMWCJVhMn
cdpPqtQbcHlmQjmZswFfTRFpBTfwwT
ZhtZpvbnbpPbtLHLvdsNdcRLNd
jDDjlCflGwsHfdrfTLrrdN
MzmljBMBWPtsbtSQtW
GHrzPSrNLFnMtSBZjZBB
WWbfDmVmwmmlbVDldWslNnBMJJNZZJCtJJJn
vwDfffVvmDwdTvDRQvpLNpLpcRFpphhHLPHg
scsTslgcnCTCScSTcqLLWlFWLLqbGvRbpL
NZMBdBPtNbbrLGqqqGvqZF
NttdbhMPfjQfNtbMbMmNjhNcCzczSSCSJTSzTCScnfnzwC
pjdjCGGGWPCMSDfS
JhFMFcrgBHPnSnWFWDDn
HVBBJctBccghsJhgrbwLGTTdtjLdbmTMMb
DtGHgDPfGfPhfLwNWSSJQcpHcr
dvlMCzdnMRFCCTjnZNpNQJcSbrWzrpSQWS
TVvFJJMjJdlMvRvMClllZZgPtPGsftfDqtGfVGsGtqqq
jSmmcjmJqcBgwmWMCLLzCsMz
TnTQVDGQTpZGNQHDZDHHQDwsCCdLrflsrCVzVrwWzwrr
zDFpppnNQtnTQQvZZZNvnhqqjtRccRbgqqbSSSjPRg
FwClNSwCFstWZLDLvhvjvtjhhD
TmsHmsmrggzmqnnGGvPGjTbbRGBhbB
cHVqgcrVzrQqzHmMcrMnczzcWFVCCFNJZWJZswwFCZWwffwS
mzbsmbmLRCZTRbSJFvPLPJPJpJffcP
QqWqNVNNNllnnWTglqTVlGNPJDvwcJpwfwccPgccPJDfJF
HMGnNMltqGMjHGqMzmTSmzTsRSszSm
qlGDfljllCTgqCTvCDfBHHQsbrSZZHSHWtvWZB
NzpnNpRnLLwRpdwpVhtqQbSbsWQWbSWnrrnH
cFqwFNpLdVcDJlDgccTD
BRqjnSBNBpRHHpjpBSnHnRBQfQzzCvzWrsWCTvfsvCsCCsfC
ZMVbhqbMdlbLTdsWvfPdPC
hlZVDMZcwJNSgjJgJFnq
CZwZssQQZrmsCmNNDpDGFblclD
HMjWMbBVfnnbMbnzMpFhlNSNFFSDcDGSzN
LnLLqjnBMjMngHbnWrTgZsCsgZvvvQrvQs
RCFCCJQbCQcprRlHHPpHhd
tWWLwvswfvZshgqDpdpBgfdf
mZtvZtMpjZzwWFjJTcQQbjjnSQ
fBfVwtttLDFctDtwFPWfTppWfmHCHdJhdChT
bGMRsbsvMQSSzMzZSNzsZvRNWTZJlmgZTJJdhhmppHTJCgTg
jssjNSSGMsQHbsRvHNPjtDcDcLPPPPDwLDDV
pClhQjJccrpbpqHhMhVhSMqHPt
dBZGZdgBzRsBsvMwGGVPVqMGwtVH
ZvDddZvDBdDdDmgCmVmbbCNpCCbljW
DTMCpdCnwRDwdfMCDDCssfZmGrBrjpttjrNrgctmGpGr
VVqJQgSSWzhPGGrPtNNQtm
bFvhgWzHJlDdffswTvRd
jwCCPPTtCswCCNTsqRNbMqQMVvVzMMMQSGvQqn
hprHlmFcHcdhWWLchZzHrLMvvnBvJJSBJMVMnnmMnMMJ
WppLcZdHWHplZWlDHhHTfzRzCCsTTtNNgtjgDw
vhmDFcDZmczMrwcqrMrmDFrvggtVSWgtSNwsjBtNVSnBsjsS
dbbRJHbpCWBBpZVgSS
LZLdHlClPmqLGDvMDv
mFbWsvsJVtbbRwfTSP
BGpQllhLGqhplBGZBfLMTSTLwwfwMJwMPT
GlDnDpQZlZZpZBlpWDNcmrgrWmNdNJvc
zbtqTtHQbZZpqbPpvGJdvQdhrhQjdQGs
qDFLLSNqcWwsGhGDJh
LgBcfnFCSFnLccggSVCVtHZlpqPPtTRMftHMbMzb
hzrrWnzRZRnbWVRzjcRHMDdqqQdNMHqHQQjlHM
sGCpCtppBfCTgwBBCwPBCssQqMQvNlSMMQDQNqHGHvDSbQ
tpLFPgfbCsfbzzcnJhRhZLhc
qzzGqfpFvWFmRSPjPjRP
cwwVssBMtNMNLngstgVBnrsPmHSJJmjllhQdQldmhdrjQJ
nDVSsLwcVcMnBGzTDDCvpvfzGT
bcTbbcZGZLPgTMWZpLLDQnrvPVnVmmjmRPFVrF
HJCJqlzBdsSjzCJRmlrlrnVQQDFnVF
BfwffNdNswLtbWbNcpjt
smJwSNNFMzFNDrvbrbfJHvbl
BRQjqZQcBhrbTsbTnfcn
ZLQRZRBjjPWSsmCdSWMgSN
NhwlDpbWggdSBvBggLFg
fRrZsVfjqljmsQQVmmsnFMFSBLLRvFTFMBSvFF
QfqVVzcsQmcQqrcsNwzzzPphHlwNppPH
nnFdsjVdmpBsBVFHzjpvlTfQdPcQQPGPcvlGPv
DWMDCCWbNJhLtMgJMNLgtMgQflZQlfQGjZZhQZGhTfQcQP
rCrtJJgLLMbgDgMDWNRrWRnzsjpFzBzSHmmqHmqnHH
rmjjJmmdwSmGhdsjJtsgGNzFWQFnBFVWHdFQcLLcNz
RCCbfRlvvPfvCTnHLLnNbNLczHnQ
lqZTllRRpDMlpfZRvgQpSmwwtggQjJgtpS
LDsGvTSSsswCwTrLZDqQWHMWbphlHMpGGpQz
RRPfPRccBdVjPcFlpMpMQWzMWfpF
RjPRjRtczcNBJRSCtLDTvTSDCCST
pqQNgNnSntwgqzzQCzNwCNBRcWtBjZcZGrBMcHMGvWcr
mmJdJPFVbJbPPGZbMRbvvrjcMj
lTMVVlLPfLNQhpgqLSLn
HlBHFrgBvlfzFzqvnvFqpCJbJfQpQpLcmhbcmtmm
jDjPGsRRTMMPjdJmjmLpCLth
MRMZMWsNpFFFVFHW
RGgwWcppGSWcWSRWmGdWcttHQFJHfbQwBQJTJQBQfJ
njjZZCMlCZjqMBFbJQZHJHBQft
DsjCPDDvjFNsMNjNqpGspcsGSmcpccrGWS
cVwMZGVZwHNPgPwRZwHttThlHllvlzQpptzppl
DsCWdqLdDCnfJLSCqsqWRsBdlhjlhzlttzQhhtvlhnhhhbzT
JCWWRWCrLDDdBdLsSsLLSCrCNZMVcmMZMFwMZwNZPZVGFPmr
hhPzDzPhPNbfpzhBbdNbDhttzqWtwttHWwntjqmwmWFm
LgGZSdMMrgTLrZLdgLSgsGTFFjrWtFFmmmFtWjqHFnFtjn
vZgdLvZLZQLRQZQQdMZLdQvVpRhNNPfJDbcBbbhVNJNNhf";

    #endregion
}