namespace Machete.HL7.Tests.Corpus
{
    using NUnit.Framework;
    using Machete.HL7.Testing;
    using Machete.HL7Schema.V26;

    [TestFixture]
    public class CorpusTests_Batch01 :
        HL7MacheteTestHarness<MSH, HL7V26Entity>
    {
        [Test]
        public void HL7_SAMPLE_001_Should_parse_ADT_A01_admission()
        {
            const string message = @"MSH|^~\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20060529090131-0500||ADT^A01^ADT_A01|01052901|P|2.5|||AL|NE
EVN|A01|20060529090131-0500
PID|||56782445~58244752^^^MRN||KLEINSAMPLE^BARRY^Q^JR||19620910|M||2028-9^^HL70005|260 GOODWIN CREST DRIVE^^BIRMINGHAM^AL^35209^^M||^PRN^PH^^^205^5551234|||M^^HL70002|||999-99-9999
PV1||I|W^389^1^UABH^^^^3||||12345^MORGAN^REX^J^^^MD^0010^UAMC^L||67890^GRAINGER^LUCY^X^^^MD^0010^UAMC^L|MED|||||A0||13579^POTTER^SHERMAN^T^^^MD^0010^UAMC^L|||||||||||||||||||||||||||200605290900
OBX|1|NM|^Body Height||1.80|m^Meter^ISO+|||||F
OBX|2|NM|^Body Weight||79|kg^Kilogram^ISO+|||||F
AL1|1|DA|^ASPIRIN||SV
DG1|1||786.50^CHEST PAIN, UNSPECIFIED^I9||200605290900|A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A01"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("KLEINSAMPLE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("BARRY"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
                Assert.That(pidResult.Result.DateTimeOfBirth.Value.Year, Is.EqualTo(1962));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));

            var al1Result = parsed.Query(q => from msh in q.Select<MSH>() from al1 in q.Select<AL1>() select al1);
            if (al1Result.HasResult)
                Assert.That(al1Result.Result.AllergenTypeCode.Value, Is.EqualTo("DA"));
        }

        [Test]
        public void HL7_SAMPLE_002_Should_parse_ORU_R01_immunization_observations()
        {
            const string message = @"MSH|^~\&|LinkLogic-2149|2149001^BMGPED|CHIRPS-Out|BMGPED|20060915210000||ORU^R01|1473973200100600|P|2.3|||NE|NE
PID|1||00000-0000000|000000|AAAAAAAA^AAAAAA^A||00000000|M||U|00000 A AA AA AAA^^AAAAAA^AA^00000||(000)000-0000|||S|||000-00-0000
PV1|1|O|^^^BMGPED||||dszczepaniak
OBR|1|||5^Preload|||20060915095920|||||||||donaldduck||ZZ
OBX|1|ST|CPT-90707.2^MMR #2||given||||||R|||20040506095950
OBX|2|ST|CPT-90737.4^HEMINFB#4||given||||||R|||19931103100050
OBX|3|ST|CPT-90707.1^MMR #1||given||||||R|||19931103095950
OBX|4|ST|CPT-90731.3^HEPBVAX#3||given||||||R|||19930712100120
OBX|5|ST|CPT-90731.2^HEPBVAX#2||given||||||R|||19930112100120
OBX|6|ST|CPT-90737.3^HEMINFB#3||given||||||R|||19930112100050
OBX|7|ST|CPT-90731.1^HEPBVAX#1||given||||||R|||19921027100120
OBX|8|ST|CPT-90737.2^HEMINFB#2||given||||||R|||19921027100050
OBX|9|ST|CPT-90737.1^HEMINFB#1||given||||||R|||19920826100050";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("5"));

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("MMR #2"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("R"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
        }

        [Test]
        public void HL7_SAMPLE_003_Should_parse_ORU_R01_CBC_with_escape_sequences()
        {
            const string message = @"MSH|^~\&|LAB|MYFAC|LAB||201411130917||ORU^R01|3216598|D|2.3|||AL|NE
PID|1|ABC123DF|AND234DA_PID3|PID_4_ALTID|Patlast^Patfirst^Mid||19670202|F|||4505 21 st^^LAKE COUNTRY^BC^V4V 2S7||222-555-8484|||||MF0050356/15
PV1|1|O|MYFACSOMPL||||^Xavarie^Sonna^^^^^XAVS|||||||||||REF||SELF|||||||||||||||||||MYFAC||REG|||201411071440||||||||23390^PV1_52Surname^PV1_52Given^H^^Dr^^PV1_52Mnemonic
ORC|RE|PT103933301.0100|||CM|N|||201411130917|^Kyle^Andra^J.^^^^KYLA||^Xavarie^Sonna^^^^^XAVS|MYFAC
OBR|1|PT1311:H00001R301.0100|PT1311:H00001R|301.0100^Complete Blood Count (CBC)^00065227^57021-8^CBC \T\ Auto Differential^pCLOCD|R||201411130914|||KYLA||||201411130914||^Xavarie^Sonna^^^^^XAVS||00065227||||201411130915||LAB|F||^^^^^R|^Xavarie^Sonna^^^^^XAVS
OBX|1|NM|301.0500^White Blood Count (WBC)^00065227^6690-2^Leukocytes^pCLOCD|1|10.1|10\S\9/L|3.1-9.7|H||A~S|F|||201411130916|MYFAC^MyFake Hospital^L
OBX|2|NM|301.0600^Red Blood Count (RBC)^00065227^789-8^Erythrocytes^pCLOCD|1|3.2|10\S\12/L|3.7-5.0|L||A~S|F|||201411130916|MYFAC^MyFake Hospital^L
OBX|3|NM|301.0700^Hemoglobin (HGB)^00065227^718-7^Hemoglobin^pCLOCD|1|140|g/L|118-151|N||A~S|F|||201411130916|MYFAC^MyFake Hospital^L
OBX|4|NM|301.0900^Hematocrit (HCT)^00065227^4544-3^Hematocrit^pCLOCD|1|0.34|L/L|0.33-0.45|N||A~S|F|||201411130916|MYFAC^MyFake Hospital^L
OBX|5|NM|301.1100^MCV^00065227^787-2^Mean Corpuscular Volume^pCLOCD|1|98.0|fL|84.0-98.0|N||A~S|F|||201411130916|MYFAC^MyFake Hospital^L
OBX|6|NM|301.1300^MCH^00065227^785-6^Mean Corpuscular Hemoglobin^pCLOCD|1|27.0|pg|28.3-33.5|L||A~S|F|||201411130916|MYFAC^MyFake Hospital^L
OBX|7|NM|301.1500^MCHC^00065227^786-4^Mean Corpuscular Hemoglobin Concentration^pCLOCD|1|330|g/L|329-352|N||A~S|F|||201411130916|MYFAC^MyFake Hospital^L
OBX|8|NM|301.1700^RDW^00065227^788-0^Erythrocyte Distribution Width^pCLOCD|1|12.0|%|12.0-15.0|N||A~S|F|||201411130916|MYFAC^MyFake Hospital^L
OBX|9|NM|301.1900^Platelets^00065227^777-3^Platelets^pCLOCD|1|125|10\S\9/L|147-375|L||A~S|F|||201411130916|MYFAC^MyFake Hospital^L
OBX|10|NM|301.2100^Neutrophils^00065227^751-8^Neutrophils^pCLOCD|1|8.0|10\S\9/L|1.2-6.0|H||A~S|F|||201411130916|MYFAC^MyFake Hospital^L
OBX|11|NM|301.2300^Lymphocytes^00065227^731-0^Lymphocytes^pCLOCD|1|1.0|10\S\9/L|0.6-3.1|N||A~S|F|||201411130916|MYFAC^MyFake Hospital^L
OBX|12|NM|301.2500^Monocytes^00065227^742-7^Monocytes^pCLOCD|1|1.0|10\S\9/L|0.1-0.9|H||A~S|F|||201411130916|MYFAC^MyFake Hospital^L
OBX|13|NM|301.2700^Eosinophils^00065227^711-2^Eosinophils^pCLOCD|1|0.0|10\S\9/L|0.0-0.5|N||A~S|F|||201411130916|MYFAC^MyFake Hospital^L
OBX|14|NM|301.2900^Basophils^00065227^704-7^Basophils^pCLOCD|1|0.0|10\S\9/L|0.0-0.2|N||A~S|F|||201411130916|MYFAC^MyFake Hospital^L
ZDR||^Xavarie^Sonna^^^^^XAVS^^^^^XX^^ATP
ZPR||";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("Patlast"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("Patfirst"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
                Assert.That(pidResult.Result.DateTimeOfBirth.Value.Year, Is.EqualTo(1967));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("301.0100"));

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("White Blood Count (WBC)"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
        }

        [Test]
        public void HL7_SAMPLE_004_Should_parse_SIU_S12_scheduling()
        {
            const string message = @"MSH|^~\&|MESA_OP|XYZ_HOSPITAL|iFW|ABC_HOSPITAL|20110613061611||SIU^S12|24916560|P|2.3||||||
SCH|10345^10345|2196178^2196178|||10345|OFFICE^Office visit|reason for the appointment|OFFICE|60|m|^^60^20110617084500^20110617093000|||||9^DENT^ARTHUR^||||9^DENT^COREY^|||||Scheduled
PID|1||42||SMITH^PAUL||19781012|M|||1 Broadway Ave^^Fort Wayne^IN^46804||(260)555-1234|||S||999999999|||||||||||||||||||
PV1|1|O|||||1^Smith^Miranda^A^MD^^^^|2^Withers^Peter^D^MD^^^^||||||||||||||||||||||||||||||||||||||||||99158||
RGS|1|A
AIG|1|A|1^White, Charles|D^^
AIL|1|A|OFFICE^^^OFFICE|^Main Office||20110614084500|||45|m^Minutes||Scheduled
AIP|1|A|1^White^Charles^A^MD^^^^|D^White, Douglas||20110614084500|||45|m^Minutes||Scheduled";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("SIU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("S12"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("SMITH"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("PAUL"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
                Assert.That(pidResult.Result.DateTimeOfBirth.Value.Year, Is.EqualTo(1978));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
        }

        [Test]
        public void HL7_SAMPLE_005_Should_parse_VXU_V04_vaccination_update()
        {
            const string message = @"MSH|^~\&|EPIC|SIISCLIENT818^LINDAS TEST ORGANIZATION|^SIIS||20150202115044||VXU^V04^VXU_V04|225|P|2.5.1||||AL
PID|1||E46700^^^^MR^||DOE^JOHN^C^JR^^^L|SMITH|20140515|M|SMITH^JOHN|2106-3^WHITE^HL70005|115 MAINSTREET^^GOODTOWN^KY^42010^USA^L^010||^PRN^PH^^^270^6009800||EN^ENGLISH^HL70296||||523968712|||2186-5^NOT HISPANIC OR LATINO^HL70012||||||||N
PD1|||||||||||02^Reminder/recall-any method^HL70215|||||A^Active^HL70441|20150202^20150202
NK1|1|DOE^MARY|MTH^MOTHER^HL70063
ORC|RE||9645^SIISCLIENT818||||||20150202111146|2001^HARVEY^MARVIN^K
RXA|0|1|20150202|20150202|20^DTaP^CVX^90700^DTAP^CPT|.5|ML^mL^ISO+||00^New immunization record^NIP001|JONES^MARK|^^^SIISCLIENT818||||A7894-2|20161115|PMC^SANOFI PASTEUR^MVX||||A
RXR|ID^INTRADERMAL^HL70162|LD^LEFT ARM^HL70163
OBX|1|CE|64994-7^VACCINE FUNDING PROGRAM ELIGIBILITY CATEGORY^LN|1|V02^MEDICAID^HL70064||||||F|||20150202|||VXC40^ELIGIBILITY CAPTURED AT THE IMMUNIZATION LEVEL^CDCPHINVS
OBX|2|CE|30956-7^VACCINE TYPE^LN|2|88^FLU^CVX||||||F|||20150202102525
OBX|3|TS|29768-9^Date vaccine information statement published^LN|2|20120702||||||F
OBX|4|TS|29769-7^Date vaccine information statement presented^LN|2|20120202||||||F
RXA|0|1|20141215|20141115|141^influenza, SEASONAL 36^CVX^90658^Influenza Split^CPT|999|||01^HISTORICAL INFORMATION - SOURCE UNSPECIFIED^NIP001||||||||||||A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("VXU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("V04"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JOHN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
                Assert.That(pidResult.Result.DateTimeOfBirth.Value.Year, Is.EqualTo(2014));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
        }

        [Test]
        public void HL7_SAMPLE_006_Should_parse_ACK_negative_acknowledgment()
        {
            const string message = @"MSH|^~\&|^^|DOE^^|DCC^^|DOE^^|20050829141336||ACK^|1125342816253.100000055|P|2.3.1
MSA|AE|00000001|Patient id was not found, must be of type 'MR'|||^^HL70357
ERR|PID^1^3^^^HL70357";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ACK"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3.1"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AE"));
                Assert.That(msaResult.Result.MessageControlId.Value, Is.EqualTo("00000001"));
            }
        }

        [Test]
        public void HL7_SAMPLE_007_Should_parse_ORU_R01_Australian_variant()
        {
            const string message = @"MSH|^~\&|MERIDIAN|Demo Server|||20100202163120+1100||ORU^R01|XX02021630854-1539|P|2.3.1^AUS&&ISO^AS4700.2&&L|||||AU
PID|1||||SMITH^Jessica^^^^^L||19700201|F|||1 Test Street^^WODEN^ACT^2606^AUS^C~2 Test Street^^WODEN^ACT^2606^AUS^C";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3.1"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("SMITH"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("Jessica"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
                Assert.That(pidResult.Result.DateTimeOfBirth.Value.Year, Is.EqualTo(1970));
            }
        }

        [Test]
        public void HL7_SAMPLE_008_Should_parse_QCK_query_acknowledgment()
        {
            const string message = @"MSH|^~\&|5.0^QSInsight^L|^^|DBO^QSInsight^L|QS4444^^|20051019154952||QCK^|1129754992182.100000002|P|2.3.1
MSA|AA|QS444437861000000042|No patients found for this query
QAK||NF";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("QCK"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3.1"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AA"));
                Assert.That(msaResult.Result.MessageControlId.Value, Is.EqualTo("QS444437861000000042"));
            }
        }

        [Test]
        public void HL7_SAMPLE_009_Should_parse_VXQ_V01_vaccination_query()
        {
            const string message = @"MSH|^~\&|DBO^QSInsight^L|QS4444|5.0^QSInsight^L||20030828104856+0000||VXQ^V01|QS444437861000000042|P|2.3.1|||NE|AL
QRD|20030828104856+0000|R|I|QueryID01|||5|000000001^Bucket^Pail^^^^^^^^^^MR|VXI|SIIS
QRF|QS4444|20030828104856+0000|20030828104856+0000||100000001~19460401~~~~~~~~~~1 Somewhere Lane Boulevard^Indianapolis^IN~10000";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("VXQ"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("V01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3.1"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("QS444437861000000042"));
        }

        [Test]
        public void HL7_SAMPLE_010_Should_parse_VXR_V03_vaccination_record_response()
        {
            const string message = @"MSH|^~\&|5.0^QSInsight^L|^^|DBO^QSInsight^L|QS4444^^|20051019163315||VXR^V03|1129757595953.100000029|P|2.3.1
MSA|AA|QS444437861000000042
QRD|20030828104856|R|I|QueryID01|||5|41565^SNOW^MARY^^^^^^^^^^SR|VXI^Vaccine Information^HL70048|SIIS
QRF|QS4444|20030828104856|20030828104856||100000001~20021223
PID|1||41565^^^^SR~2410629811:72318911||FROG^KERMIT^^^^^L||20021223|F|||3 SOUTH WAY RD^^MOORESVILLE^INDIANA^46158^^M||(317)222-1234^^PH||EN^English^HL70296|||||||||||||||N
PD1|||^^^^^^SR|^^^^^^^^^^^^SR|||||||02^Reminder/recall -any method^HL70215|||||A^Active^HL70441
PV1||R";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("VXR"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("V03"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3.1"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AA"));
                Assert.That(msaResult.Result.MessageControlId.Value, Is.EqualTo("QS444437861000000042"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("FROG"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("KERMIT"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
                Assert.That(pidResult.Result.DateTimeOfBirth.Value.Year, Is.EqualTo(2002));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("R"));
        }

        [Test]
        public void HL7_SAMPLE_011_Should_parse_VXX_V02_multi_patient_response()
        {
            const string message = @"MSH|^~\&|5.0^QSInsight^L|^^|DBO^QSInsight^L|QS4444^^|20051019163235||VXX^V02|1129757555111.100000025|P|2.3.1
MSA|AA|QS444437861000000042
QRD|20030828104856|R|I|QueryID01|||5|10^SNOW^MARY^^^^^^^^^^SR|VXI^Vaccine Information^HL70048|SIIS
QRF|QS4444|20030828104856|20030828104856||100000001~20021223
PID|1||41565^^^^SR~2410629811:72318911||SNOW^MARY^^^^^L||20021223|F|||2 NORTH WAY RD^^MOORESVILLE^INDIANA^46158^^M||(317)123-4567^^PH||EN^English^HL70296|||||||||||||||N
PID|2||28694^^^^SR~2663391364:111111111||FROG^KERMIT^^^^^L||20021223|
NK1|1|PIGGY^MISS|GRD^Guardian^HL70063";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("VXX"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("V02"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3.1"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AA"));
                Assert.That(msaResult.Result.MessageControlId.Value, Is.EqualTo("QS444437861000000042"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("SNOW"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MARY"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_012_Should_parse_ADT_A04_registration_with_NK1_and_insurance()
        {
            const string message = @"MSH|^~\&|REGADT|MCM|IFENG||199112311501||ADT^A04^ADT_A01|000001|P|2.4|||
EVN|A04|199901101500|199901101400|01||199901101410
PID|||191919^^GENHOS^MR~371-66-9256^^^USSSA^SS|253763|MASSIE^JAMES^A||19560129|M|||171 ZOBERLEIN^^ISHPEMING^MI^49849^""""^|(900)485-5344|(900)485-5344||S^^HL70002|C^^HL70006|10199925^^^GENHOS^AN|371-66-9256
NK1|1|MASSIE^ELLEN|SPOUSE^^HL70063|171 ZOBERLEIN^^ISHPEMING^MI^49849^""""^|(900)485-5344|(900)545-1234~(900)545-1200|EC1^FIRST EMERGENCY CONTACT^HL70131
NK1|2|MASSIE^MARYLOU|MOTHER^^HL70063|300 ZOBERLEIN^^ISHPEMING^MI^49849^""""^|(900)485-5344|(900)545-1234~(900)545-1200|EC2^SECOND EMERGENCY CONTACT^HL70131
NK1|3
NK1|4|||123 INDUSTRY WAY^^ISHPEMING^MI^49849^""""^||(900)545-1200|EM^EMPLOYER^HL70131|19940605||PROGRAMMER|||ACME SOFTWARE COMPANY
PV1||O|O/R||||0148^ADDISON,JAMES|0148^ADDISON,JAMES||AMB|||||||0148^ADDISON,JAMES|S|1400|A|||||||||||||||||||GENHOS|||||199501101410
PV2||||||||199901101400|||||||||||||||||||||||||199901101400
ROL||AD|CP^^HL70443|0148^ADDISON,JAMES
OBX||NM|3141-9^BODY WEIGHT^LN||62|kg|||||F
OBX||NM|3137-7^HEIGHT^LN||190|cm|||||F
DG1|1|19||R63.4^LOSS OF WEIGHT^I10|||00
GT1|1||MASSIE^JAMES^""""^""""^""""^""""^||171 ZOBERLEIN^^ISHPEMING^MI^49849^""""^|(900)485-5344|(900)485-5344||||SE^SELF^HL70063|371-66-925||||MOOSES AUTO CLINIC|171 ZOBERLEIN^^ISHPEMING^MI^49849^""""|(900)485-5344
IN1|0|0^HL70072|BC1|BLUE CROSS|171 ZOBERLEIN^^ISHPEMING^M149849^""""^||(900)485-5344|90||||||50 OK";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A04"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A04"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("MASSIE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JAMES"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
                Assert.That(pidResult.Result.DateTimeOfBirth.Value.Year, Is.EqualTo(1956));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("3141-9"));
        }

        [Test]
        public void HL7_SAMPLE_013_Should_parse_ORU_R01_glucose_SN_datatype()
        {
            const string message = @"MSH|^~\&|GHH LAB|ELAB-3|GHH OE|BLDG4|200202150930||ORU^R01|CNTRL-3456|P|2.4
PID|||555-44-4444||EVERYWOMAN^EVE^E^^^^L|JONES|196203520|F|||153 FERNWOOD DR.^^STATESVILLE^OH^35292||(206)3345232|(206)752-121||||AC555444444||67-A4335^OH^20030520
OBR|1|845439^GHH OE|1045813^GHHLAB|1554-5^GLUCOSE^LN|||200202150730|||||||||555-55-5555^PRIMARY^PATRICIA P^^^^MD^^LEVEL SEVEN HEALTHCARE, INC.|||||||||F|||||||444-44-4444&HIPPOCRATES&HOWARD H&&&&MD
OBX|1|SN|1554-5^GLUCOSE POST 12H CFST^LN||^182|mg/dl|70-105|H|||F";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("EVERYWOMAN"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("EVE"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("1554-5"));

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("GLUCOSE POST 12H CFST"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_014_Should_parse_QBP_Q11_immunization_query()
        {
            const string message = @"MSH|^~\&||GA0000||MA0000|199705221605||QBP^Q11^QBP_Q11|19970522GA40|T|2.5.1|||NE|AL|||||Z34^CDCPHINVS
QPD|Z34^Request Immunization History^CDCPHINVS|19970522GA05|25^^^STATE_IIS^MR|FLOYD^FRANK^R^^^^L|MALLARD^F|20030123|M|8444 N. 90th Street^Suite 100^Scottsdale^AZ^85258^USA^L|^PRN^PH^^^480^7458554
RCP|I|20^RD|R";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("QBP"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("Q11"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("QBP_Q11"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("19970522GA40"));
        }

        [Test]
        public void HL7_SAMPLE_015_Should_parse_RSP_K11_immunization_response()
        {
            const string message = @"MSH|^~\&|^^|MA0000^^|^^|GA0000^^|20111105122535||RSP^K11^RSP_K11|1320521135996.100000002|T|2.5.1|||||||||Z32^CDCPHINVS^^
MSA|AA|19970522GA40
QAK|||Z34^Request Immunization History^HL70471
QPD|Z34^Request Immunization History^HL70471|19970522GA05||FLOYD^FRANK^^^^^L|MALIFICENT|20030123|M|L
PID|1||25^^^^SR~0001||FLOYD^FRANK^^^^^L||20030123|M|||612 S WRIGHT CT^^KENNEWICK^WASHINGTON^99366^United States^M||(509)421-0355^^PH^^^509^4210355^|||||||||||||||||N
PD1|||^^^^^^SR|21^MATT^SHAKY^K^^^^^^^^^SR~1679652135|||||||02^Reminder/recall -any method^HL70215|||||A^Active^HL70441
NK1|1|FLOYD^MALIFICENT|GRD^Guardian^HL70063
PV1||R
ORC|RE||25.34.20100723
RXA|0|999|20100723|20100723|83^Hep A, ped/adol, 2 dose^CVX^90633^Hep A 2 dose - Ped/Adol^CPT~34^Hep A 2 dose - Ped/Adol^STC0292|999|||00^New immunization record^NIP001||IRMS-1000||||AHAVB379AA||SKB^GlaxoSmithKline^HL70227||||A|20111105122536
RXR|IM^Intramuscular^HL70162|LT^Left Thigh^HL70163
OBX|1|CE|VFC-STATUS^VFC Status^STC||V02||||||F
OBX|1|CE|30963-3^Vaccine purchased with^LN||PBF^Public funds^NIP008||||||F
OBX|1|CE|VFC-STATUS^VFC Status^STC||||||||F
OBX|1|DT|29768-9^date vaccine information statement published^LN||20100120||||||F
OBX|1|DT|29769-7^date vaccine information statement presented^LN||20100723||||||F
ORC|RE||25.34.20100728";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("RSP"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("K11"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("RSP_K11"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AA"));
                Assert.That(msaResult.Result.MessageControlId.Value, Is.EqualTo("19970522GA40"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("FLOYD"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("FRANK"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
                Assert.That(pidResult.Result.DateTimeOfBirth.Value.Year, Is.EqualTo(2003));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
        }

        [Test]
        public void HL7_SAMPLE_016_Should_parse_ORU_R01_radiology_report()
        {
            const string message = @"MSH|^~\&|MESA_RPT_MGR|EAST_RADIOLOGY|iFW|XYZ|||ORU^R01|MESA3b|P|2.4||||||||
PID|||CR3^^^ADT1||CRTHREE^PAUL|||||||||||||PatientAcct||||||||||||
PV1||1|CE||||12345^SMITH^BARON^H|||||||||||
OBR|||||||20010501141500.0000||||||||||||||||||F||||||||||||||||
OBX|1|HD|SR Instance UID||1.113654.1.2001.30.2.1||||||F||||||
OBX|2|TX|SR Text||Radiology Report History Cough Findings PA evaluation of the chest demonstrates the lungs to be expanded and clear.  Conclusions Normal PA chest x-ray.||||||F||||||
CTI|study1|^1|^10_EP1";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MESA3b"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("CRTHREE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("PAUL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
        }

        [Test]
        public void HL7_SAMPLE_017_Should_parse_ORM_O01_radiology_order()
        {
            const string message = @"MSH|^~\&|MESA_OP|XYZ_HOSPITAL|iFW|ABC_RADIOLOGY|||ORM^O01|101104|P|2.3||||||||
PID|1||20891312^^^^EPI||APPLESEED^JOHN^A^^MR.^||19661201|M||AfrAm|505 S. HAMILTON AVE^^MADISON^WI^53505^US^^^DN |DN|(608)123-4567|(608)123-5678||S|| 11480003|123-45-7890||||^^^WI^^
PD1|||FACILITY(EAST)^^12345|1173^MATTHEWS^JAMES^A^^^
PV1|||^^^CARE HEALTH SYSTEMS^^^^^||| |1173^MATTHEWS^JAMES^A^^^||||||||||||610613||||||||||||||||||||||||||||||||V
ORC|NW|987654^EPIC|76543^EPC||Final||^^^20140418170014^^^^||20140418173314|1148^PATTERSON^JAMES^^^^||1173^MATTHEWS^JAMES^A^^^|1133^^^222^^^^^|(618)222-1122||
OBR|1|363463^EPC|1858^EPC|73610^X-RAY ANKLE 3+ VW^^^X-RAY ANKLE ||||||||||||1173^MATTHEWS^JAMES^A^^^|(608)258-8866||||||||Final||^^^20140418170014^^^^|||||6064^MANSFIELD^JEREMY^^^^||1148010^1A^EAST^X-RAY^^^|^|
DG1||I10|S82^ANKLE FRACTURE^I10|ANKLE FRACTURE||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORM"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("APPLESEED"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JOHN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
                Assert.That(pidResult.Result.DateTimeOfBirth.Value.Year, Is.EqualTo(1966));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("NW"));

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("73610"));

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
                Assert.That(dg1Result.Result.DiagnosiCode.Value.Identifier.Value, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void HL7_SAMPLE_018_Should_parse_ADT_A04_emergency_with_allergies()
        {
            const string message = @"MSH|^~\&|MESA_ADT|XYZ_ADMITTING|iFW|ZYX_HOSPITAL|||ADT^A04|103102|P|2.4||||||||
EVN||200007010800||||200007010800
PID|||583295^^^ADT1||DOE^JANE||19610615|M-||2106-3|123 MAIN STREET^^GREENSBORO^NC^27401-1020|GL|(919)379-1212|(919)271-3434~(919)277-3114||S||PATID12345001^2^M10|123456789|9-87654^NC
NK1|1|BATES^RONALD^L|SPO|||||20011105
PV1||E||||||5101^NELL^FREDERICK^P^^DR|||||||||||V1295^^^ADT1|||||||||||||||||||||||||200007010800||||||||
PV2|||^ABDOMINAL PAIN
OBX|1|HD|SR Instance UID||1.123456.2.2000.31.2.1||||||F||||||
AL1|1||^PENICILLIN||PRODUCES HIVES~RASH
AL1|2||^CAT DANDER
DG1|001|I9|1550|MAL NEO LIVER, PRIMARY|19880501103005|F||
PR1|2234|M11|111^CODE151|COMMON PROCEDURES|198809081123
ROL|45^RECORDER^ROLE MASTER LIST|AD|CP|KATE^SMITH^ELLEN|199505011201
GT1|1122|1519|BILL^GATES^A
IN1|001|A357|1234|BCMD|||||132987
IN2|ID1551001|SSN12345678";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A04"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JANE"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("E"));

            var al1Result = parsed.Query(q => from msh in q.Select<MSH>() from al1 in q.Select<AL1>() select al1);
            if (al1Result.HasResult)
                Assert.That(al1Result.Result.AllergenTypeCode.HasValue, Is.True);

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
                Assert.That(dg1Result.Result.DiagnosiCode.Value.Identifier.Value, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void HL7_SAMPLE_019_Should_parse_ACK_positive_acknowledgment()
        {
            const string message = @"MSH|^~\&|Main_HIS|XYZ_HOSPITAL|iFW|ABC_Lab|20160915003015||ACK|9B38584D|P|2.6.1
MSA|AA|9B38584D|Everything was okay dokay!";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ACK"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.6.1"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("9B38584D"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AA"));
                Assert.That(msaResult.Result.MessageControlId.Value, Is.EqualTo("9B38584D"));
            }
        }

        [Test]
        public void HL7_SAMPLE_020_Should_parse_BAR_P01_billing_account()
        {
            const string message = @"MSH|^~\&|MESA_OP|XYZ_HOSPITAL|iFW|ABC_HOSPITAL|040112043835||BAR^P01|0000000001|T|2.3
EVN||20200420134725||
PID|||3000222452||DOE^JOHN^E||19931114|M||||||||||1546740|666381774
PV1||I|BRACKENRIDGE|||||023434|||||||||023434|||||||||||||||||||||||||||20031121||
GT1|0||DOE^JOHN^E||756 E FANNIN ST^^LAGRANGE^TX^789450000|9799660489|||||M|||||CARE INN|457 NMAIN^^LAGRANGE^TX^78945
IN1|1|T71|||MEDICAID";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("BAR"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("P01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JOHN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
                Assert.That(pidResult.Result.DateTimeOfBirth.Value.Year, Is.EqualTo(1993));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
        }

        [Test]
        public void HL7_SAMPLE_021_Should_parse_DFT_P03_financial_transaction()
        {
            const string message = @"MSH|^~\&|MESA_OP|XYZ_HOSPITAL|iFW|ABC_RADIOLOGY|201504201347|12|DFT^P03|24885|P|2.5
EVN||20150420134725||
PID|1|12345|12345^^^MIE&1.2.840.114398.1.100&ISO^MR||MOUSE^MICKEY^S||19281118|M|||123 Main St.^^Lake Buena Vista^FL^3283|||||||||||||||||||
FT1|1|1133||20150325000000||CG|99244|Consultation-Level 4||1|0.000000|||||^^^^OFFICE^^^^Office||BILLING|B69^Cysticercosis^I10~B60.0^Babesiosis^I10|123^Physician^Dr|||1133|1^Adams^Douglas|99244|
PR1|1|CPT|99244|Consultation-Level 4|20150325000000|||||||123^Physician^Dr|B69^Cysticercosis^I10~B60.0^Babesiosis^I10
DG1|1|ICD10|B69^Cysticercosis^I10|Cysticercosis|20150325000000|F|||||||||0|123^Physician^Dr
DG1|2|ICD10|B60.0^Babesiosis^I10|Babesiosis|20150325000000|F|||||||||0|123^Physician^Dr";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("DFT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("P03"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("MOUSE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MICKEY"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
                Assert.That(pidResult.Result.DateTimeOfBirth.Value.Year, Is.EqualTo(1928));
            }

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
                Assert.That(dg1Result.Result.DiagnosiCode.Value.Identifier.Value, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void HL7_SAMPLE_022_Should_parse_ORM_O01_multiple_order_groups()
        {
            const string message = @"MSH|^~\&|MACHETELAB|^DOSC|MACHETE|18779|20130405125146269||ORM^O01|1999077678|P|2.3|||AL|AL
NTE|1||KOPASD
NTE|2||A3RJ
NTE|3||7ADS
NTE|4||G46DG
PID|1|000000000026|60043^^^MACHETE^MRN||MACHETE^JOE||19890909|F|||123 SEASAME STREET^^Oakland^CA^94600||5101234567|5101234567||||||||||||||||N
PD1|M|F|N||||F|
NTE|1||IN42
PV1|1|O|||||92383^Machete^Janice||||||||||||12345|||||||||||||||||||||||||201304051104
PV2||||||||20150615|20150616|1||||||||||||||||||||||||||N
IN1|1|||MACHETE INC|1234 Fruitvale ave^^Oakland^CA^94601^USA||5101234567^^^^^510^1234567|074394|||||||A1|MACHETE^JOE||19890909|123 SEASAME STREET^^Oakland^CA^94600||||||||||||N|||||666889999|0||||||F||||T||60043^^^MACHETE^MRN
GT1|1|60043^^^MACHETE^MRN|MACHETE^JOE||123 SEASAME STREET^^Oakland^CA^94600|5416666666|5418888888|19890909|F|P
AL1|1|FA|^pollen allergy|SV|jalubu daggu||
ORC|NW|PRO2350||XO934N|||^^^^^R||20130405125144|91238^Machete^Joe||92383^Machete^Janice
OBR|1|PRO2350||11636^Urinalysis, with Culture if Indicated^L|||20130405135133||||N|||||92383^Machete^Janice|||||||||||^^^^^R
DG1|1|I9|788.64^URINARY HESITANCY^I9|URINARY HESITANCY
OBX|1||URST^Urine Specimen Type^^^||URN
NTE|1||abc
NTE|2||dsa
ORC|NW|PRO2351||XO934N|||^^^^^R||20130405125144|91238^Machete^Joe||92383^Machete^Janice
OBR|1|PRO2350||11637^Urinalysis, with Culture if Indicated^L|||20130405135133||||N|||||92383^Machete^Janice|||||||||||^^^^^R
DG1|1|I9|788.64^URINARY HESITANCY^I9|URINARY HESITANCY
OBX|1||URST^Urine Specimen Type^^^||URN
NTE|1||abc
NTE|2||dsa
ORC|NW|PRO2352||XO934N|||^^^^^R||20130405125144|91238^Machete^Joe||92383^Machete^Janice
OBR|1|PRO2350||11638^Urinalysis, with Culture if Indicated^L|||20130405135133||||N|||||92383^Machete^Janice|||||||||||^^^^^R
DG1|1|I9|788.64^URINARY HESITANCY^I9|URINARY HESITANCY
OBX|1||URST^Urine Specimen Type^^^||URN
NTE|1||abc
NTE|2||dsa";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORM"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("MACHETE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JOE"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
                Assert.That(pidResult.Result.DateTimeOfBirth.Value.Year, Is.EqualTo(1989));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("NW"));

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("11636"));

            var al1Result = parsed.Query(q => from msh in q.Select<MSH>() from al1 in q.Select<AL1>() select al1);
            if (al1Result.HasResult)
                Assert.That(al1Result.Result.AllergenTypeCode.Value, Is.EqualTo("FA"));

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
                Assert.That((string)nteResult.Result.Comment[0].Value, Is.EqualTo("KOPASD"));

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
        }

        [Test]
        public void HL7_SAMPLE_023_Should_parse_ADT_A01_v26_with_Z_segment()
        {
            const string message = @"MSH|^~\&|DATASERVICES|CORPORATE|||20120711120510.2-0500||ADT^A01^ADT_A01|9c906177-dfca-4bbe-9abd-d8eb43df93a0|D|2.6
EVN||20120701000000-0500
PID|1||397979797^^^SN^SN~4242^^^BKDMDM^PI~1000^^^YARDI^PI||Williams^Rory^H^^^^A||19641028000000-0600|M||||||||||31592^^^YARDI^AN
NK1|1|Pond^Amelia^Q^^^^A|SPO|1234 Main St^^Sussex^WI^53089|^PRS^CP^^^^^^^^^555-1212||N
NK1|2|Smith^John^^^^^A~^The Doctor^^^^^A|FND|1234 S Water St^^New London^WI^54961||^WPN^PH^^^^^^^^^555-9999|C
PV1|2|I||R
GT1|1||Doe^John^A^^^^A||5678 Maple Ave^^Sussex^WI^53089|^PRS^PH^^^^^^^^^555-9999|||||OTH
IN1|1|CAP1000|YYDN|ACME HealthCare||||GR0000001|||||||HMO|||||||||||||||||||||PCY-0000042
IN1|2||||||||||||||Medicare|||||||||||||||||||||123-45-6789-A
IN1|3||||||||||||||Medicaid|||||||||||||||||||||987654321L
ZFA|6|31592|12345|YARDI|20120201000000-0600";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ADT_A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.6"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("9c906177-dfca-4bbe-9abd-d8eb43df93a0"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            Assert.That(evnResult.HasResult, Is.True);

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("Williams"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("Rory"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
                Assert.That(pidResult.Result.DateTimeOfBirth.Value.Year, Is.EqualTo(1964));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
        }

        [Test]
        public void HL7_SAMPLE_024_Should_parse_ADT_A03_discharge_with_early_Z_segment()
        {
            const string message = @"MSH|^~\&|IRIS|SANTER|AMB_R|SANTER|200803051508||ADT^A03|263206|P|2.5
EVN||200803051509||||200803031508
ZZZ|aaa
PID|||5520255^^^PK^PK~ZZZZZZ83M64Z148R^^^CF^CF~ZZZZZZ83M64Z148R^^^SSN^SSN^^20070103^99991231~^^^^TEAM||ZZZ^ZZZ||19830824|F||||||||||||||||||||||N
PV1||I|6402DH^^^^^^^^MED. 1 - ONCOLOGIA^^OSPEDALE MAGGIORE DI LODI&LODI|||^^^^^^^^^^OSPEDALE MAGGIORE DI LODI&LODI|13936^TEST^TEST||||||||||5068^TEST2^TEST2||2008003369||||||||||||||||||||||||||200803031508
PR1|1||1111^Mastoplastica|Protesi|20090224|02|";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A03"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("263206"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            Assert.That(evnResult.HasResult, Is.True);

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("ZZZ"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
                Assert.That(pidResult.Result.DateTimeOfBirth.Value.Year, Is.EqualTo(1983));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
        }

        [Test]
        public void HL7_SAMPLE_025_Should_parse_ADT_A08_minimal_v22()
        {
            const string message = @"MSH|^~\&|STML|001|STML|001|20020307142717||ADT^A08|01501|T|2.2|||AL|NE
EVN|A08|20020307142652";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A08"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.2"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("01501"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("STML"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("001"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A08"));
        }
    }
}
