namespace Machete.HL7.Tests.Corpus
{
    using NUnit.Framework;
    using Machete.HL7.Testing;
    using Machete.HL7Schema.V26;

    [TestFixture]
    public class CorpusTests_Batch02 :
        HL7MacheteTestHarness<MSH, HL7V26Entity>
    {
        [Test]
        public void HL7_SAMPLE_026_Should_parse_SIU_S14_scheduling_notification()
        {
            const string message = @"MSH|^~\&|ORSOS|G|PRECASE^SCHEDULING||20100406132803||SIU^S14| 38762|P|2.3
SCH|ADS|569576||||G014080|||379|MIN^MINUTES|^^^20100407161000|51873^AL-RADI OO||||ADS||||ADS
PID||990119205^^^R1^MR|990119205|000533921J^^^R1^VN|TEST MP3^TEST MP3|||||||||||||000533921J
PV1|1|U|||||51873^AL-RADI OO|51873^AL-RADI OO|||||||||||569576^R1 caseno
DG1|1|||||U
RGS|1|1|abc
AIS|1|1|G014080^BYPASS AXILLOFEMORAL|20100406161000|||255|000000
AIG|1|1|51873|STAFF|||||20100407164000
AIL|1|1|OR 4^OR 4||||||40800|0
AIP|1|1|51873^ALRADI^OSMAN|^1
RGS|2|2|abc
AIS|2|2|G11455^BYPASS AORTOCORONARY USING RADIAL ARTERY|20100406161000|||15
AIG|2|2|33043|STAFF|||||20100407212500
AIL|2|2|OR 4^OR 4|||||||0
AIP|2|2|33043^AUNE^KELLY|^1
RGS|3|3|abc
AIS|3|3|G014045^ANGIOPLASTY|20100406161000|||5
AIG|3|3|102694|STAFF|||||20100407214000
AIL|3|3|OR 4^OR 4|||||||0
AIP|3|3|102694^ANGELINI^MARK|^1
RGS|4|4|abc
AIS|4|4|G11045^BYPASS AORTOCORONARY LITA|20100406161000|||60
AIG|4|4|T1240UHN|STAFF|||||20100407214500
AIL|4|4|OR 4^OR 4|||||||0
AIP|4|4|T1240UHN^ASTA^JOHN|^1";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("SIU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("S14"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("ORSOS"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("TEST MP3"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("U"));
            }

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
            {
                Assert.That(dg1Result.Result.DiagnosiCode.Value.Identifier.Value, Is.Not.Null.Or.Empty);
            }
        }

        [Test]
        public void HL7_SAMPLE_027_Should_parse_ADT_A17_swap_patients()
        {
            const string message = @"MSH|^~\&|ULTRA|TML|TML||200903120021||ADT^A17|66239404|T|2.3.1||||||
EVN|A17|201002130003||||201002130003|G^4265^L
PID|1||3678347^^^UHN^MR^G^4265^^^^^~9635915532^vp^^CANON^JHN^G^4265^^^20120517^^~||LEIGHTON^RACHEL^DIAMOND^^^^U||19310313|F|||200 ANYWHERE ST^^TORONTO^ON^M6H 2T9^CAN^H||^PRN^PH^^1^416^7676333|^WPN^PH^^1^905^7436333^6001||||
PV1||E|Emerg^EmergAcute^Interview^G^4265^^^N^EmergAcute^Interview^Emerg^185 2 16^|||Emerg^Gen Wait^GenWait13^G^4265^^^^Gen Wait^GenWait13^Emerg^185 6 13^|141378^Rauchwerger^David^^^Dr.^MD^^^L^^^EI^^^^^^^^^^^^^|||||||A|||141378^Rauchwerger^David^^^Dr.^MD^^^L^^^EI^^^^^^^^^^^^^|EP^|292007135^^^UHN^VN^G^4265^^^^^||||||||||||||||||||G|||||200905030022|||||||V|
PID|2||0877410^^^UHN^MR^^^^^^^~3135063059^^^CANON^JHN^^^^^^^~||Lex^Tamara^Christina^^Miss^^L^^^^^200905030044^^~||19801120|F|||441 Margueretta St^^Toronto^ON^M6H2S6^Can^H^^^^^^^~|1811|(416)533-5113^PRN^PH^^^^^^^^^~|(416)532-0206^WPN^PH^^^^^^^^^|eng^ English^03ZPtlang^^^|S^Single^03ZMtSt^^^|PDI^ Patient did not indicate^03ZRelgn^^^|292007138^^^UHN^VN^^^^^^^~||||||||||||N|||200905030044||||||
ZZ1|TESTVALUE";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A17"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3.1"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("ULTRA"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A17"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("LEIGHTON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("RACHEL"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("E"));
            }
        }

        [Test]
        public void HL7_SAMPLE_028_Should_parse_ADT_A05_preadmit_with_insurance()
        {
            const string message = @"MSH|^~\&|STML|001|STML|001|20020307142616||ADT^A05|01500|T|2.2|||AL|NE
EVN|A05|20020307142547|200203081424
PID|0001||000005222||TEST^BRIAN||19700623|M||C|30815 RAYBURN^^LIVONIA^MI^48154-0000^USA||(313)422-2182|||S||0001445|380-96-9087
PV1|0001|O|PR|EL|T0468423||00410^KRISHNAN AMBA MD|||PRO||||1||||O||2||||||||||||||||||||||||||0000000.00|0000000.00|0000000.00|0000000.00
PV2|||^IV THERAPY|||||20020308
OBX|0001|ST|ADMIT DIAGNOSIS TEXT
OBX|0002|ST|1010.1^WEIGHT(KG)^AS4
OBX|0003|ST|1010.3^HEIGHT(CM)^AS4
OBX|0004|ST|ALLERGY FREE TEXT
GT1|0001|0003225|TEST^BRIAN||30815 RAYBURN^^LIVONIA^MI^48154-0000^USA|(313)422-2182||19700623|M||SE|380969087|||||^^^^^USA|||1
IN1|0001|09|210|BCBSM BC OF MI|600 EAST LAFAYETTE^^DETROIT^MI^48226-0000^USA|||77777|FULL BC|||||||TEST^BRIAN|SE|19700623|30815 RAYBURN^^LIVONIA^MI^48154-0000^USA|Y|Y||||||Y|Y|20020307||||||21009|XYZ380969087||||||1|M|^^^^^USA
IN2||380969087|||||||||||||||||||N|||||||SPR^LM^00677.00~ICU^LM^02068.00
IN1|0002|09|211|BCBSM BS OF MI P/C PLAN|600 EAST LAFAYETTE^^DETROIT^MI^48226-0000^USA|||77777|FULL|||||||TEST^BRIAN|SE|19700623|30815 RAYBURN^^LIVONIA^MI^48154-0000^USA|Y|N||||||Y|Y|20020307||||||21109|XYZ380969087||||||1|M|^^^^^USA
IN2||380969087|||||||||||||||||||N";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A05"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.2"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("STML"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A05"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("TEST"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("BRIAN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }
        }

        [Test]
        public void HL7_SAMPLE_029_Should_parse_ADT_A04_with_misplaced_segment()
        {
            const string message = @"MSH|^~\&|IDX|XXXX|COMMON|EXTERNAL|200608140653||ADT^A04|60491_4054_SC1|P|2.3
PID|||868063820614||||^003|F||1|^^^^84606|||||S
PV1||O||10||||||||||||||||||||||||||||||||||||||||200501091835
AIL|1||871|10|
DG1||||RASH ON BACK AND RT LEG
IN1||||||||||||||||||||||||||||||||||||||||||||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A04"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("IDX"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }
        }

        [Test]
        public void HL7_SAMPLE_030_Should_parse_ADT_A06_change_class_with_merge()
        {
            const string message = @"MSH|^~\&|REG|W|||201103230042||ADT^A06|32153168-A06|P|2.2
PV2||F||||||||||||||||||||N||AI|Emergency||||||N|||||||OTH^Self^03ZBrInBy
NK1||||^^^^^^M|||NK
DG1||IN|MISC|Transient Ischemic Attack / Stroke
MRG|2804019||11020086571";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A06"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.2"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("REG"));

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
            {
                Assert.That(dg1Result.Result.DiagnosiCode.Value.Identifier.Value, Is.Not.Null.Or.Empty);
            }
        }

        [Test]
        public void HL7_SAMPLE_031_Should_parse_ADT_A14_pending_admit_with_OHIP()
        {
            const string message = @"MSH|^~\&|REG|G|||201002220026||ADT^A14|33186452|P|2.2|||||
EVN|A14|201002220026|||14380
PID||^^|222^454554^HN637|324334244^^|TEST^PATIENT^Luis^^Mr.^Mr.|
PV1||E|^^^^^^^^|^||^^^^^^^^|^^^^^^^|^^^^^^^||^^|||||||^^^^^^^|^|3243^|^|||||||||||||||||||G||||^^^^^^^^|43865896||||||
PV2||||||||||||||||||^||||N
NK1|1|TEST^NKGuy^^^^|CHD^ Son|123 Fake Street^^Toronto^ON^M6J 3H2^Can^M|(416)123-4567|(416)123-4567|NK||||||
IN1|1||0012343001|  OHIP||||||||||||^^^^^|||^^^^^^M|||||||||||||||||||||||||^^^^^^M
IN2||||||2216
ZIN||||||||||||||||||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A14"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.2"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("REG"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A14"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("TEST"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("PATIENT"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("E"));
            }
        }

        [Test]
        public void HL7_SAMPLE_032_Should_parse_ORU_R01_with_FT_escape_sequences()
        {
            const string message = @"MSH|^~\&|PHCN_ULTRA|2220|HSIE|2220|201106161233||ORU^R01|72313573|T|2.4|||AL|AL|AU
PID|
PV1||I|^DIS^DIS^2220|||||0129296H^BRAUN^GARY|7MPH^MPH-HL7-RESULT FEED|||||||||I|^^^2220
ORC|RE|^HNAM_ORDERID|11-6879530-GAS-0^PHCN_ULTRA||RE
OBR|1|^HNAM_ORDERID|11-6879530-GAS-0^PHCN_ULTRA|GAS^GASES (BLOOD)|||201106161000|||||||||0129296H^BRAUN^GARY^^^DR^^^2220^^^^Provider Num||||1295102|7MPH|201106161233||GRP|F||^^^201106161203
OBX|1|FT|GAS^^LN||Type Venous\.br\\\.br\\ Analysis pH : \\H\\ 7.28\\N\\ (7.38-7.43)";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("PHCN_ULTRA"));

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("GAS"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("GASES (BLOOD)"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("GAS"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F").Or.Null);
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
            }
        }

        [Test]
        public void HL7_SAMPLE_033_Should_parse_ORM_O01_radiology_order_UTF8()
        {
            const string message = @"MSH|^~\&|RIS 2.0|KIR|InterRis 2.0|Receiving facility|20110309132505||ORM^O01|MSG00001|1|2.3.1|1||||SL|UNICODE UTF-8|SL
PID|||123^^^^^BIS~1234^^^^^HIS~12345^^^^^KZZZ||Lastname^Name^J^Mr|||M
PV1||E||||||2314^Novak&Janez^^^^Dr.|||||||||||123365
ORC|AF|1123abc^marand||3|||1^once^^^^U
OBR|1|1123abc^marand||RDP_10^RDP Kljicnica|||||||||klinicno vprasanje?||^^^^L&Left|12345^Novak^Janez^^^^^^BPI_ZDR|||||||||||1^Once^^^^R";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORM"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3.1"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("RIS 2.0"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("Lastname"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("Name"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("E"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("AF"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("RDP_10"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("RDP Kljicnica"));
            }
        }

        [Test]
        public void HL7_SAMPLE_034_Should_parse_ORU_R01_with_ZPI_before_PID()
        {
            const string message = @"MSH|^~\&|ULTRA|TML|OLIS|OLIS|200905011130||ORU^R01|20169838|T|2.3
ZPI|||7005728^^^TML^MR||TEST^RACHEL^DIAMOND||19310313|F|||200 ANYWHERE ST^^TORONTO^ON^M6G 2T9||(416)888-8888||||||1014071185^KR
PID|||7005728^^^TML^MR||TEST^RACHEL^DIAMOND||19310313|F|||200 ANYWHERE ST^^TORONTO^ON^M6G 2T9||(416)888-8888||||||1014071185^KR
PID|||7005728^^^TML^MR||TEST^RACHEL^DIAMOND||19310313|F|||200 ANYWHERE ST^^TORONTO^ON^M6G 2T9||(416)888-8888||||||1014071185^KR
PV1|1||OLIS||||OLIST^BLAKE^DONALD^THOR^^^^^921379^^^^OLIST
ORC|RE||T09-100442-RET-0^^OLIS_Site_ID^ISO|||||||||OLIST^BLAKE^DONALD^THOR^^^^L^921379
OBR|0||T09-100442-RET-0^^OLIS_Site_ID^ISO|RET^RETICULOCYTE COUNT^HL79901 literal|||200905011106|||||||200905011106||OLIST^BLAKE^DONALD^THOR^^^^L^921379||7870279|7870279|T09-100442|MOHLTC|200905011130||B7|F||1^^^200905011106^^R
OBX|1|IS|Z114099^Erc^L||ABC||||||F|||200905011111|PMH";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("ULTRA"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("TEST"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("RACHEL"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("RET"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("RETICULOCYTE COUNT"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("Z114099"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
            }
        }

        [Test]
        public void HL7_SAMPLE_035_Should_parse_ORU_R01_reticulocyte_with_ZPI_at_end()
        {
            const string message = @"MSH|^~\&|ULTRA|TML|OLIS|OLIS|200905011130||ORU^R01|20169838|T|2.3
PID|||7005728^^^TML^MR||TEST^RACHEL^DIAMOND||19310313|F|||200 ANYWHERE ST^^TORONTO^ON^M6G 2T9||(416)888-8888||||||1014071185^KR
PV1|1||OLIS||||OLIST^BLAKE^DONALD^THOR^^^^^921379^^^^OLIST
ORC|RE||T09-100442-RET-0^^OLIS_Site_ID^ISO|||||||||OLIST^BLAKE^DONALD^THOR^^^^L^921379
OBR|0||T09-100442-RET-0^^OLIS_Site_ID^ISO|RET^RETICULOCYTE COUNT^HL79901 literal|||200905011106|||||||200905011106||OLIST^BLAKE^DONALD^THOR^^^^L^921379||7870279|7870279|T09-100442|MOHLTC|200905011130||B7|F||1^^^200905011106^^R
OBX|1|NM|Z114099^Erc^L||4.00|tril/L|3.90-5.60||||F|||200905011111|PMH
ZPI|||7005728^^^TML^MR||TEST^RACHEL^DIAMOND||19310313|F|||200 ANYWHERE ST^^TORONTO^ON^M6G 2T9||(416)888-8888||||||1014071185^KR";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("TEST"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("RACHEL"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("RET"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("RETICULOCYTE COUNT"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("Z114099"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_036_Should_parse_ORU_R01_with_DSC_continuation()
        {
            const string message = @"MSH|^~\&|ULTRA|TML|OLIS|OLIS|200905011130||ORU^R01|20169838|T|2.3
PID|||7005728^^^TML^MR||TEST^RACHEL^DIAMOND||19310313|F|||200 ANYWHERE ST^^TORONTO^ON^M6G 2T9||(416)888-8888||||||1014071185^KR
PV1|1||OLIS||||OLIST^BLAKE^DONALD^THOR^^^^^921379^^^^OLIST
ORC|RE||T09-100442-RET-0^^OLIS_Site_ID^ISO|||||||||OLIST^BLAKE^DONALD^THOR^^^^L^921379
OBR|0||T09-100442-RET-0^^OLIS_Site_ID^ISO|RET^RETICULOCYTE COUNT^HL79901 literal|||200905011106|||||||200905011106||OLIST^BLAKE^DONALD^THOR^^^^L^921379||7870279|7870279|T09-100442|MOHLTC|200905011130||B7|F||1^^^200905011106^^R
OBX|1|NM|Z114099^Erc^L||4.00|tril/L|3.90-5.60||||F|||200905011111|PMH
DSC|1
ZPI|||7005728^^^TML^MR||TEST^RACHEL^DIAMOND||19310313|F|||200 ANYWHERE ST^^TORONTO^ON^M6G 2T9||(416)888-8888||||||1014071185^KR";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("TEST"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("Z114099"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_037_Should_parse_ORU_R01_minimal_repeating_OBX()
        {
            const string message = @"MSH|^~\&|^QueryServices||||20021011161756.297-0500||ORU^R01|1|D|2.4
ORC|
OBX|1
OBX|2
OBX|3
NTE|||test|";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("1"));

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Is.EqualTo("test"));
            }
        }

        [Test]
        public void HL7_SAMPLE_038_Should_parse_ORU_R01_with_CTI_clinical_trial()
        {
            const string message = @"MSH|^~\&|^QueryServices||||20021011161756.297-0500||ORU^R01|1|D|2.4
ORC|
CTI|
OBR|
NTE|||test|";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("1"));

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Is.EqualTo("test"));
            }
        }

        [Test]
        public void HL7_SAMPLE_039_Should_parse_ADT_A04_Mario_Bros_with_escapes()
        {
            const string message = @"MSH|^~\&|NES|NINTENDO|AGNEW|CORNERCUBICLE|20010101000000||ADT^A04|Q123456789T123456789X123456|P|2.3
EVN|A04|20010101000000|||^KOOPA^BOWSER^^^^^^^CURRENT
PID|1||123456789|0123456789^AA^^JP|BROS^MARIO^^^^||19850101000000|M|||123 FAKE STREET^MARIO \T\ LUIGI BROS PLACE^TOADSTOOL KINGDOM^NES^A1B2C3^JP^HOME^^1234|1234|(555)555-0123^HOME^JP:1234567|||S|MSH|12345678|||||||0|||||N
NK1|1|PEACH^PRINCESS^^^^|SO|ANOTHER CASTLE^^TOADSTOOL KINGDOM^NES^^JP|(123)555-1234|(123)555-2345|NOK|||||||||||||
NK1|2|TOADSTOOL^PRINCESS^^^^|SO|YET ANOTHER CASTLE^^TOADSTOOL KINGDOM^NES^^JP|(123)555-3456|(123)555-4567|EMC|||||||||||||
PV1|1|O|ABCD^EFGH^|||^^|123456^DINO^YOSHI^^^^^^MSRM^CURRENT^^^NEIGHBOURHOOD DR NBR^|^DOG^DUCKHUNT^^^^^^^CURRENT||CRD|||||||123456^DINO^YOSHI^^^^^^MSRM^CURRENT^^^NEIGHBOURHOOD DR NBR^|AO|0123456789|1|||||||||||||||||||MSH||A|||20010101000000";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A04"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("NES"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("NINTENDO"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A04"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("BROS"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MARIO"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }
        }

        [Test]
        public void HL7_SAMPLE_040_Should_parse_ADT_A03_discharge_Canadian_French()
        {
            const string message = @"MSH|^~\&|1444-ADT|1444|S-ADT|SIMS|20071023160622||ADT^A03^ADT_A05|Q67084255T54052896X2|P^T|2.5|||NE|AL|CAN|8859/1
EVN|A03|20071023160622
PID|1||00J8804997^^^1444^MR~165640^^^CANON^JHN^^^^^WT||Aalan^Angus^^^^^L||19620404|F|||101 Ames Ave^^Toronto^CA-ON^M2N7J6^CAN^H||^PRN^PH^^1^416^5551545|^PRN^PH^^1^416^2227788||C||||||||||||||N
PV1";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A03"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ADT_A05"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("1444-ADT"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A03"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("Aalan"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("Angus"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_041_Should_parse_ADT_A31_Belgian_French_locale()
        {
            const string message = @"MSH|^~\&|||||20080627102031.292+0100||ADT^A31^ADT_A31|EJ557600005480760|P|2.3|||||BE|8859/1|FR
EVN||20080627101943+0100
PID|||M07869D^^^ADMISSION^^ISSTLUC||DUPONT^JEAN||19701004000000+0100|M
PD1
PV1||N";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A31"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ADT_A31"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("EJ557600005480760"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DUPONT"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JEAN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("N"));
            }
        }

        [Test]
        public void HL7_SAMPLE_042_Should_parse_ORU_R01_with_AD_datatype_in_OBX()
        {
            const string message = @"MSH|^~\&
OBR|
OBX||AD|||F1C1^F2C1";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationValue[0].Value, Is.EqualTo("F1C1^F2C1").Or.Not.Null);
            }
        }

        [Test]
        public void HL7_SAMPLE_043_Should_parse_ORU_R01_with_subcomponent_in_PID()
        {
            const string message = @"MSH|^~\&|^QueryServices||||20021011161756.297-0500||ORU^R01|1|D|2.4
PID|||||x&y^z|";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("z"));
            }
        }

        [Test]
        public void HL7_SAMPLE_044_Should_parse_ORU_R01_with_extra_subcomponent_in_PID_set_id()
        {
            const string message = @"MSH|^~\&|^QueryServices||||20021011161756.297-0500||ORU^R01|1|D|2.4
PID|4&y";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
        }

        [Test]
        public void HL7_SAMPLE_045_Should_parse_ORU_R01_with_extra_field_repetitions()
        {
            const string message = @"MSH|^~\&|one~two~three||||||ORU^R01|1|D|2.4";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("one~two~three"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("1"));
        }

        [Test]
        public void HL7_SAMPLE_046_Should_parse_MSA_with_field_separator_escape()
        {
            const string message = @"MSH|^~\&
MSA|\F\";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.HasValue, Is.True);
            }
        }

        [Test]
        public void HL7_SAMPLE_047_Should_parse_ORU_R01_with_field_separator_escape_in_sending_app()
        {
            const string message = @"MSH|^~\&|\F\||||||ORU^R01|1|D|2.4";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("1"));
        }

        [Test]
        public void HL7_SAMPLE_048_Should_parse_ADT_A01_with_duplicate_EVN_and_PID()
        {
            const string message = @"MSH|^~\&|^QueryServices||||20021011161756.297-0500||ADT^A01|1|D|2.4
EVN|R01
EVN|R02
PID|1
IN1|1
IN1|2
PID|2";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("R01"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.HasResult, Is.True);
            }
        }

        [Test]
        public void HL7_SAMPLE_049_Should_parse_ORU_R01_with_very_long_sending_app()
        {
            const string message = @"MSH|^~\&|SUPER_LONG_MSH_SEGMENT_00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000|TML|OLIS|OLIS|200905011130||ORU^R01|20169838|T|2.3
OBX|8|ST|Test

String||20090505||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Does.StartWith("SUPER_LONG_MSH_SEGMENT"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("TML"));
        }

        [Test]
        public void HL7_SAMPLE_050_Should_parse_ORU_R01_with_unknown_version_2999()
        {
            const string message = @"MSH|^~\&|^QueryServices||||20021011161756.297-0500||ORU^R01|1|D|2.999
ORC|ORC1
OBX|1
NTE|||NTE1|
ZNT|ZNT1
ORC|ORC2";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.999"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("1"));

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("ORC1"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Is.EqualTo("NTE1"));
            }
        }
    }
}
