namespace Machete.HL7.Tests.Corpus
{
    using NUnit.Framework;
    using Machete.HL7.Testing;
    using Machete.HL7Schema.V26;

    [TestFixture]
    public class CorpusTests_Batch12 :
        HL7MacheteTestHarness<MSH, HL7V26Entity>
    {
        [Test]
        public void HL7_SAMPLE_276_Should_parse_ORU_deep_nesting()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20240701100000||ORU^R01|MSG00276|P|2.5|||AL|NE
PID|1||12345^^^MAIN_HOSP&2.16.840.1.113883.19.5&ISO^MR^MAIN_HOSP&2.16.840.1.113883.19.5&ISO^^20200101^20251231^MAIN_HOSP^GOOD THRU 2025||NESTED^TEST^PATIENT^JR^MR^PHD~ALIAS^OTHER^NAME^^^^M|MAIDEN^NAME|19800101|M||2106-3^White^HL70005|123 DEEP ST^^NEST CITY^NS^12345^USA^H^DEEP COUNTY~456 WORK RD^^OFFICE PARK^NS^67890^USA^O||^PRN^PH^^1^555^1234567^890~^WPN^PH^^1^555^9876543|^WPN^PH^^1^555^1112233
ORC|RE|ORD001
OBR|1|ORD001||CBC|||20240701090000|||||||||||||||20240701100000|||F
OBX|1|NM|6690-2^WBC^LOINC||7.5|10*3/uL|4.5-11.0|N|||F|||20240701100000||MAIN_LAB&2.16.840.1.113883.19.5&ISO|MAIN LAB^L^^^^MAIN_HOSP&2.16.840.1.113883.19.5&ISO|123 LAB WAY^^LAB CITY^LS^11111^USA^B";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("LAB"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("HOSP"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("NESTED"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("TEST"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("6690-2"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("WBC"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_277_Should_parse_ORU_with_repetitions()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20240801100000||ORU^R01|MSG00277|P|2.5|||AL|NE
PID|1||MRN001^^^HOSP^MR~SSN123-45-6789^^^SSA^SS~DL-XYZ123^^^DMV^DL~ACCT-999^^^HOSP^AN~INS-888^^^BCBS^IN||REPEATER^TEST^PATIENT||19750315|M|||123 HOME ST^^CITY1^ST^11111^USA^H~456 WORK RD^^CITY2^ST^22222^USA^O~789 VACATION LN^^CITY3^ST^33333^USA^V||5551111111~5552222222~5553333333~5554444444
NK1|1|REPEATER^SPOUSE^S|SPO|999 SPOUSE RD^^CITY^ST^44444|5556666666~5557777777~5558888888
ORC|RE|ORD001
OBR|1|ORD001||CBC|||20240801090000|||||||||||||||20240801100000|||F
OBX|1|NM|718-7^HEMOGLOBIN^LOINC||7.5|g/dL|12.0-16.0|LL~AA|||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00277"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("REPEATER"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("TEST"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("718-7"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("HEMOGLOBIN"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_278_Should_parse_ADT_A01_v22()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|199501151200||ADT^A01|MSG00278|P|2.2
EVN|A01|199501151200
PID||EXT-12345|INT-67890||DOE^JOHN||19500101|M||W|100 MAIN ST^^ANYTOWN^CA^90210||5551234567
NK1|1|DOE^JANE|SPO|100 MAIN ST^^ANYTOWN^CA^90210|5551234567
PV1|1|I|MED^301^A|||MED^301^B|1234^SMITH^ROBERT|||MED||||ADM|A0||||1|||||||||||||||||||||199501151200
IN1|1|BCBS|BC001|BLUE CROSS||||||||||DOE^JOHN|01|19500101|||||||||||||||||||POL-12345";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.2"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00278"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A01"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JOHN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_279_Should_parse_ADT_A01_v28()
        {
            const string message = @"MSH|^~\&|ADT|MODERN_HOSP|EMR|MODERN_HOSP|20240901080000||ADT^A01^ADT_A01|MSG00279|P|2.8|||AL|NE||||||
EVN|A01|20240901080000||||20240901075500
PID|1||MRN-V28-001^^^MODERN_HOSP^MR||MODERN^PATIENT^TEST^JR^MR||19900101120000|M||2106-3^White^HL70005|123 FUTURE ST^^NEW CITY^NS^12345^USA^H^^COUNTY01||^PRN^PH^^1^555^1234567||||M|CAT^Catholic^HL70006|ACCT-V28|||N^Not Hispanic^HL70189|BIRTHPLACE^^BIRTH CITY^BS^00000^USA|Y||||||||||||||20240901080000
PV1|1|I|MED^301^A^MODERN_HOSP^N^BED^FLOOR3||||1234^ATTENDING^DOC^A^DR^MD^MODERN_HOSP|5678^REFERRING^DOC^B^DR^MD|9012^CONSULTING^DOC^C^DR^MD|MED|||||ADM||1234^ATTENDING^DOC^A^DR^MD|IP||V|||||||||||||||||MODERN_HOSP||A|||20240901080000
ROL|1|AD|ADMITTING|1234^ATTENDING^DOC^A^DR^MD^MODERN_HOSP|20240901080000";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ADT_A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.8"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("MODERN_HOSP"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A01"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("MODERN"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("PATIENT"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_280_Should_parse_ORU_v21_earliest_version()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|199001011200||ORU^R01|MSG00280|P|2.1
PID||12345||DOE^JOHN||19500101|M
OBR|1|ORD001||CBC|||199001011100
OBX|1|NM|WBC||8.5|10*3/uL|4.5-11.0|N|||F
OBX|2|NM|HGB||15.0|g/dL|12.0-16.0|N|||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.1"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00280"));

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("WBC"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_281_Should_parse_ORU_v26_with_CWE()
        {
            const string message = @"MSH|^~\&|LAB|MAIN_HOSP|EMR|MAIN_HOSP|20200615140000||ORU^R01^ORU_R01|MSG00281|P|2.6|||AL|NE
PID|1||MRN-V26-001^^^MAIN_HOSP^MR||VERSIONED^PATIENT^SIX||19850301|F
ORC|RE|ORD-V26-001||CM
OBR|1|ORD-V26-001||24323-8^CMP^LOINC|||20200615120000|||||||||DR1100^CHEN||||||20200615140000|||F
OBX|1|NM|2345-7^GLUCOSE^LOINC||105|mg/dL|74-106|N|||F|||20200615140000||||MAIN_LAB|MAIN LAB^L|123 LAB WAY^^LAB CITY^LS^11111|DR9900^LABDIR^MEDICAL^D^^MD
OBX|2|NM|2160-0^CREATININE^LOINC||1.1|mg/dL|0.7-1.3|N|||F|||20200615140000||||MAIN_LAB|MAIN LAB^L|123 LAB WAY^^LAB CITY^LS^11111|DR9900^LABDIR^MEDICAL^D^^MD";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ORU_R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.6"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("VERSIONED"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("24323-8"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CMP"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("2345-7"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("GLUCOSE"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_282_Should_parse_MDM_T01_document_notification()
        {
            const string message = @"MSH|^~\&|TRANSCRIPTION|SURG_CTR|EMR|MAIN_HOSP|20240320160000||MDM^T01|MSG00282|P|2.5|||AL|NE
EVN|T01|20240320160000
PID|1||MRN445500^^^MAIN_HOSP^MR||JACKSON^WILLIAM^R||19670815|M|||567 VINE ST^^NASHVILLE^TN^37201||6155553344
PV1|1|I|SURG^OR-3||||DR8899^CHEN^LISA^M^^MD||||SURG||||||||V-2024-4455
TXA|1|OP^OPERATIVE REPORT|TX|20240320143000|DR8899^CHEN^LISA^M^^MD|20240320155000|20240320160000||||DR8899^CHEN^LISA^M^^MD|||DOC-2024-5566|REP-2024-5566||AU^Authenticated|Y||DR8899^CHEN^LISA^M^^MD|20240320160000";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("MDM"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("T01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("TRANSCRIPTION"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("T01"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("JACKSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("WILLIAM"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_283_Should_parse_MDM_T06_addendum()
        {
            const string message = @"MSH|^~\&|TRANSCRIPTION|MAIN_HOSP|EMR|MAIN_HOSP|20240422100000||MDM^T06|MSG00283|P|2.5|||AL|NE
EVN|T06|20240422100000
PID|1||MRN667744^^^MAIN_HOSP^MR||WILLIAMS^FRANK^D||19600325|M
PV1|1|I|MED^401^B||||DR2233^BROWN^ELIZABETH^A^^MD
TXA|1|AD^ADDENDUM|TX|20240422093000|DR2233^BROWN^ELIZABETH^A^^MD|20240422100000|||||DR2233^BROWN^ELIZABETH^A^^MD||DOC-2024-3344|DOC-2024-5577|||AU^Authenticated|Y||DR2233^BROWN^ELIZABETH^A^^MD|20240422100000
OBX|1|TX|AD^ADDENDUM||ADDENDUM TO DISCHARGE SUMMARY (DOC-2024-3344)\.br\\.br\Date of Addendum: 04/22/2024\.br\\.br\The patient contacted the office on 04/21/2024 reporting mild shortness of breath. He was advised to continue current antibiotic regimen and was prescribed an albuterol inhaler PRN. Follow-up moved up to 3 days from discharge instead of 1 week.\.br\\.br\Electronically signed by: Dr. Elizabeth Brown, MD\.br\Date: 04/22/2024 10:00 AM||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("MDM"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("T06"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("T06"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("WILLIAMS"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("FRANK"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("AD"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("ADDENDUM"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_284_Should_parse_REF_I12_referral()
        {
            const string message = @"MSH|^~\&|PCP_EMR|PCP_CLINIC|CARDIO_EMR|CARDIO_OFFICE|20240515143000||REF^I12|MSG00284|P|2.5|||AL|NE
RF1|A^Accepted^HL70283|MED^Medical^HL70281|UR^Urgent^HL70282|||20240515|20240615|R^Restricted^HL70285
PRD|RP^Referring Provider^HL70286|SMITH^JOHN^M^^DR^^MD|789 PRIMARY CARE DR^^ANYTOWN^NY^10001||^WPN^PH^^1^212^5551234||1234567890^NPI
PRD|RT^Referred To Provider^HL70286|HEART^ANNA^K^^DR^^MD|456 CARDIOLOGY WAY^^ANYTOWN^NY^10002||^WPN^PH^^1^212^5559876||0987654321^NPI
PID|1||MRN556677^^^PCP_CLINIC^MR||JONES^PATRICIA^A||19680101|F|||123 MAPLE ST^^ANYTOWN^NY^10001||2125557890
DG1|1||R07.9^CHEST PAIN UNSPECIFIED^ICD10||20240515|A
NTE|1||Patient presents with 2-week history of exertional chest pain. Family history significant for premature CAD (father MI at age 52). Resting ECG shows nonspecific ST changes. Requesting stress test and cardiology evaluation.
AUT|BCBS^BLUE CROSS BLUE SHIELD|AUTH-2024-9988|20240515|20240615||20240515|DR SMITH";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("REF"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("I12"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("PCP_EMR"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("JONES"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("PATRICIA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
            {
                Assert.That(dg1Result.Result.DiagnosiCode.Value.Identifier.Value, Is.Not.Null.Or.Empty);
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
        }

        [Test]
        public void HL7_SAMPLE_285_Should_parse_ORL_O22_order_response()
        {
            const string message = @"MSH|^~\&|LAB|MAIN_HOSP|CPOE|MAIN_HOSP|20240601100500||ORL^O22|MSG00285|P|2.5|||AL|NE
MSA|AA|MSG-OML-001
PID|1||MRN778800^^^MAIN_HOSP^MR||BAKER^STEVEN^R||19800505|M
ORC|OK|ORD-A001|LAB-A001-FILL||IP
OBR|1|ORD-A001|LAB-A001-FILL|58410-2^CBC W DIFF^LOINC|||20240601100000
SPM|1|SPEC-001||119297000^Blood specimen^SCT||||RM^Room temperature||P^Patient|10|mL|20240601095000|20240601100500||||MAIN_LAB";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORL"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O22"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AA"));
                Assert.That(msaResult.Result.MessageControlId.Value, Is.EqualTo("MSG-OML-001"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("BAKER"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("OK"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("58410-2"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CBC W DIFF"));
            }
        }

        [Test]
        public void HL7_SAMPLE_286_Should_parse_ORU_thyroid_panel()
        {
            const string message = @"MSH|^~\&|LAB|ENDO_CLINIC|EMR|ENDO_CLINIC|20240301090000||ORU^R01|MSG00286|P|2.5|||AL|NE
PID|1||MRN223344^^^ENDO_CLINIC^MR||PARK^JENNY^S||19780820|F||2028-9^Asian^HL70005|789 THYROID LN^^BOSTON^MA^02101||6175554433
PV1|1|O|ENDO^A
ORC|RE|ORD-THY-001|LAB-THY-001||CM
OBR|1|ORD-THY-001|LAB-THY-001|94911-8^THYROID PANEL^LOINC|||20240301080000|||||||||DR5566^PARK^JENNY^ENDO^^MD||DR7788^PRIMARY^CARE^^^MD|||||20240301090000|||F
OBX|1|NM|11580-8^TSH^LOINC||8.5|mIU/L|0.27-4.20|H|||F|||20240301090000||TECH001^LAB^TECH|MAIN_LAB|MAIN LAB||
OBX|2|NM|3024-7^FREE T4^LOINC||0.6|ng/dL|0.93-1.70|L|||F|||20240301090000||TECH001^LAB^TECH|MAIN_LAB|MAIN LAB||
OBX|3|NM|3051-0^FREE T3^LOINC||2.8|pg/mL|2.0-4.4|N|||F|||20240301090000||TECH001^LAB^TECH|MAIN_LAB|MAIN LAB||
NTE|1|L|Elevated TSH with low Free T4 is consistent with primary hypothyroidism. Clinical correlation recommended. Consider levothyroxine therapy.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("LAB"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("PARK"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JENNY"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("94911-8"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("THYROID PANEL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("11580-8"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("TSH"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_287_Should_parse_ORU_lipid_panel()
        {
            const string message = @"MSH|^~\&|LAB|MAIN_HOSP|EMR|MAIN_HOSP|20240401100000||ORU^R01|MSG00287|P|2.5|||AL|NE
PID|1||MRN887766^^^MAIN_HOSP^MR||ADAMS^HENRY^J||19600812|M
ORC|RE|ORD-LIP-001|LAB-LIP-001||CM
OBR|1|ORD-LIP-001|LAB-LIP-001|57698-3^LIPID PANEL^LOINC|||20240401083000|||||||||DR2233^JONES||||||20240401100000|||F
OBX|1|NM|2093-3^TOTAL CHOLESTEROL^LOINC||248|mg/dL|<200|H|||F
OBX|2|NM|2085-9^HDL CHOLESTEROL^LOINC||38|mg/dL|>40|L|||F
OBX|3|NM|2089-1^LDL CHOLESTEROL CALC^LOINC||172|mg/dL|<100|H|||F
OBX|4|NM|2571-8^TRIGLYCERIDES^LOINC||190|mg/dL|<150|H|||F
OBX|5|NM|13457-7^VLDL CHOLESTEROL^LOINC||38|mg/dL|5-40|N|||F
OBX|6|NM|9830-1^CHOLESTEROL/HDL RATIO^LOINC||6.5||<5.0|H|||F
OBX|7|NM|43396-1^NON-HDL CHOLESTEROL^LOINC||210|mg/dL|<130|H|||F
OBX|8|CE|LP35812-2^CARDIOVASCULAR RISK^LOINC||HIGH^High Risk^LOCAL||||||F
NTE|1|L|Patient has atherogenic lipid profile. LDL goal <100 mg/dL for patients with additional risk factors. Consider statin therapy.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("ADAMS"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("HENRY"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("57698-3"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("LIPID PANEL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("2093-3"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("TOTAL CHOLESTEROL"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_288_Should_parse_ORU_drug_screen()
        {
            const string message = @"MSH|^~\&|TOX_LAB|MAIN_HOSP|EMR|MAIN_HOSP|20240501150000||ORU^R01|MSG00288|P|2.5|||AL|NE
PID|1||MRN556644^^^MAIN_HOSP^MR||TAYLOR^MARK^R||19850620|M
ORC|RE|ORD-TOX-001|LAB-TOX-001||CM
OBR|1|ORD-TOX-001|LAB-TOX-001|DRUGSCR^URINE DRUG SCREEN W CONFIRMATION^LOCAL|||20240501100000|||||||||DR3344^SMITH||||||20240501150000|||F
OBX|1|ST|19295-5^OPIATES SCREEN^LOINC||Positive||Negative|A|||F
OBX|2|NM|19295-5^OPIATES CONFIRMATION^LOINC||450|ng/mL|<300|A|||F
OBX|3|ST|19261-7^AMPHETAMINES SCREEN^LOINC||Negative||Negative|N|||F
OBX|4|ST|19292-2^BENZODIAZEPINES SCREEN^LOINC||Positive||Negative|A|||F
OBX|5|NM|19292-2^BENZODIAZEPINES CONFIRMATION^LOINC||380|ng/mL|<200|A|||F
OBX|6|ST|19659-2^CANNABINOIDS SCREEN^LOINC||Negative||Negative|N|||F
OBX|7|ST|19285-9^COCAINE METABOLITES SCREEN^LOINC||Negative||Negative|N|||F
NTE|1||Confirmation performed on positive screening results. All confirmed values exceed established cutoff concentrations.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("TOX_LAB"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("TAYLOR"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MARK"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("DRUGSCR"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("URINE DRUG SCREEN W CONFIRMATION"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("19295-5"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("OPIATES SCREEN"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_289_Should_parse_DFT_P11_financial()
        {
            const string message = @"MSH|^~\&|BILLING|MAIN_HOSP|FINANCE|MAIN_HOSP|20240601160000||DFT^P11|MSG00289|P|2.5|||AL|NE
EVN|P11|20240601160000
PID|1||MRN334455^^^MAIN_HOSP^MR||WILSON^JAMES^T||19520415|M
PV1|1|I|CARD^201^A||||DR6677^HEART^ANNA||||||||||V-2024-8899|||MS-DRG^291||||||||||||||||||||20240525|20240601
GP1|291^Heart Failure with MCC^MS-DRG|||1^Medicare
FT1|1|CHG-001||20240525|20240601|CG|99223^INITIAL HOSPITAL CARE HIGH^CPT||1|||285.00
FT1|2|CHG-002||20240526|20240601|CG|99233^SUBSEQUENT HOSPITAL CARE HIGH^CPT||5|||175.00
FT1|3|CHG-003||20240601|20240601|CG|99238^HOSPITAL DISCHARGE DAY^CPT||1|||150.00
GP2|1|99223^CPT|285.00|A^Allowed^HL70459
GP2|2|99233^CPT|875.00|A^Allowed^HL70459
GP2|3|99238^CPT|150.00|A^Allowed^HL70459
DG1|1||I50.23^ACUTE ON CHRONIC SYSTOLIC HF^ICD10||20240525|A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("DFT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("P11"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("BILLING"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("P11"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("WILSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JAMES"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
            {
                Assert.That(dg1Result.Result.DiagnosiCode.Value.Identifier.Value, Is.Not.Null.Or.Empty);
            }
        }

        [Test]
        public void HL7_SAMPLE_290_Should_parse_ORU_with_Z_segments()
        {
            const string message = @"MSH|^~\&|LAB|CUSTOM_HOSP|EMR|CUSTOM_HOSP|20240701100000||ORU^R01|MSG00290|P|2.5
PID|1||MRN112233||CUSTOM^PATIENT||19800101|M
ZPM|1|PREMIUM|VIP_LEVEL_3|CUSTOM_FLAG_A|CUSTOM_FLAG_B
PV1|1|I|MED^301
ORC|RE|ORD001
OBR|1|ORD001||CBC|||20240701090000|||||||||||||||20240701100000|||F
ZOR|1|PRIORITY_RUSH|OVERRIDE_DR_SMITH|AUTH-9988|PANEL_SET_A
OBX|1|NM|WBC||8.0|10*3/uL|4.5-11.0|N|||F
ZRE|1|VERIFIED_BY_SUPERVISOR|INSTRUMENT_XN3000|QC_PASS|DELTA_CHECK_OK
OBX|2|NM|HGB||14.5|g/dL|12.0-16.0|N|||F
ZRE|2|VERIFIED_AUTO|INSTRUMENT_XN3000|QC_PASS|DELTA_CHECK_OK
ZXX|1|BATCH_2024_07_01|REPORT_GENERATED_AT_100500|CUSTOM_FOOTER_V3";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00290"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("CUSTOM"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("PATIENT"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("WBC"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_291_Should_parse_ORU_with_escape_sequences()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20240801100000||ORU^R01|MSG00291|P|2.5
PID|1||MRN-ESC-001||ESCAPE^TEST^PATIENT
ORC|RE|ORD-ESC-001
OBR|1|ORD-ESC-001||ESCTEST|||20240801090000|||||||||||||||20240801100000|||F
OBX|1|TX|ESCTEST^ESCAPE TEST^LOCAL||The result contains: pipe \F\ caret \S\ tilde \R\ ampersand \T\ and backslash \E\ characters. Also hex \X0D0A\ and spacing \.sp 3\ commands.||||||F
OBX|2|FT|FMTTEST^FORMAT TEST^LOCAL||This is \H\highlighted\N\ text with \.br\line break and \F\literal pipe\F\ in formatted text.||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00291"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("ESCAPE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("TEST"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("ESCTEST"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("ESCAPE TEST"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_292_Should_parse_ORU_with_custom_delimiters()
        {
            const string message = @"MSH#@!?%#SENDING_APP#SENDING_FAC#RECEIVING_APP#RECEIVING_FAC#20240101120000##ORU@R01#MSG-CUSTOM-292#P#2.5
PID#1##MRN999##DOE@JOHN@Q##19700101#M
ORC#RE#ORD001
OBR#1#ORD001##CBC###20240101110000###################20240101120000###F
OBX#1#NM#WBC##7.5#10*3/uL#4.5-11.0#N###F
OBX#2#NM#HGB##14.0#g/dL#12.0-16.0#N###F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG-CUSTOM-292"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_293_Should_parse_ORU_with_max_OBR_fields()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20240901100000||ORU^R01|MSG00293|P|2.5
PID|1||MRN-MAX-001||MAXFIELD^TEST||19800101|M
ORC|RE|ORD-MAX-001|LAB-MAX-001||CM
OBR|1|ORD-MAX-001|LAB-MAX-001|CBC|R|20240901080000|20240901090000|20240901090500|2^mL|EDTA|N|Y|FASTING|BLOOD|DR1100^ORDERING^DOC|5551234567|CALLBACK|ORD-MAX-001|LAB-MAX-001|20240901100000||CHEM|F|CHANGE1|DR2200^PRINCIPAL^DOC|20240901094500|20240901095000|1234.56|LAB^MAIN^1|R|REASON FOR STUDY|DR3300^ASSISTANT^DOC|TECH001^TECH^LAB|DR4400^TRANSCRIBER^DOC|20240901095500|MAIN_LAB^MAIN LAB|SECTION_A|METHOD_1|TRANSPORT_LOG|COLLECT_VOL|COLLECT_METHOD|SOURCE_TABLE|TRANSPORT_ARR|COLLECTOR_ID|ACTION_CODE|DANGER_CODE|RELEVANT_CLINICAL|20240901080000|20240901100000||F^REPORT TO FILE|58410-2^CBC^LOINC
OBX|1|NM|WBC||7.5|10*3/uL|4.5-11.0|N|||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00293"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("MAXFIELD"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("CBC"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("WBC"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_294_Should_parse_ADT_A08_with_allergies()
        {
            const string message = @"MSH|^~\&|ADT|MAIN_HOSP|EMR|MAIN_HOSP|20240515100000||ADT^A08|MSG00294|P|2.5|||AL|NE
EVN|A08|20240515100000
PID|1||MRN776655^^^MAIN_HOSP^MR||RODRIGUEZ^MARIA^C||19780830|F||2135-2^Hispanic^HL70005|456 HIBISCUS LN^^MIAMI^FL^33101||3055559012
PV1|1|I|MED^205^A||||DR4455^GONZALEZ^CARLOS
AL1|1|DA^Drug Allergy|70618^PENICILLIN^RXNORM|SV^Severe|ANAPHYLAXIS|20100301
AL1|2|DA^Drug Allergy|2670^CODEINE^RXNORM|MO^Moderate|NAUSEA AND VOMITING|20120515
AL1|3|FA^Food Allergy|SHELLFISH^SHELLFISH^LOCAL|MI^Mild|URTICARIA|20150801
AL1|4|EA^Environmental Allergy|POLLEN^TREE POLLEN^LOCAL|MI^Mild|RHINITIS, SNEEZING|20080601";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A08"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00294"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A08"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("RODRIGUEZ"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MARIA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }

            var al1Result = parsed.Query(q => from msh in q.Select<MSH>() from al1 in q.Select<AL1>() select al1);
            if (al1Result.HasResult)
            {
                Assert.That(al1Result.Result.AllergenTypeCode.Value, Is.EqualTo("DA"));
            }
        }

        [Test]
        public void HL7_SAMPLE_295_Should_parse_ORU_with_long_message_control_id()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20240101120000||ORU^R01|a]b1c2d3-e4f5-6789-0abc-def012345678-20240101120000-LAB-HOSP-OUTBOUND-PROD-001-BATCH-999-SEQ-12345|P|2.5
PID|1||MRN001||LONG^ID^TEST||19800101|M
ORC|RE|ORD001
OBR|1|ORD001||CBC|||20240101110000|||||||||||||||20240101120000|||F
OBX|1|NM|WBC||7.5|10*3/uL|4.5-11.0|N|||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("a]b1c2d3-e4f5-6789-0abc-def012345678-20240101120000-LAB-HOSP-OUTBOUND-PROD-001-BATCH-999-SEQ-12345"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("LONG"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("ID"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("WBC"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_296_Should_parse_ORU_with_empty_routing_fields()
        {
            const string message = @"MSH|^~\&|||||||ORU^R01|MSG00296|P|2.5
PID|1||MRN001||BLANK^ROUTING^TEST||19800101|M
ORC|RE|ORD001
OBR|1|ORD001||CBC|||20240101110000|||||||||||||||20240101120000|||F
OBX|1|NM|WBC||7.5|10*3/uL|4.5-11.0|N|||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00296"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("BLANK"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("ROUTING"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("WBC"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_297_Should_parse_ORU_with_three_component_message_type()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20240101120000||ORU^R01^ORU_R01|MSG00297|P|2.5|||AL|NE|USA|ASCII|EN|||||2.16.840.1.113883.9.16^HL7^ISO~2.16.840.1.113883.9.28^HL7^ISO
PID|1||MRN001||PROFILE^TEST^PATIENT||19800101|M
ORC|RE|ORD001
OBR|1|ORD001||CBC|||20240101110000|||||||||||||||20240101120000|||F
OBX|1|NM|WBC||7.5|10*3/uL|4.5-11.0|N|||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ORU_R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00297"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("LAB"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("PROFILE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("TEST"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("WBC"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_298_Should_parse_ORU_with_formatted_text_escapes()
        {
            const string message = @"MSH|^~\&|NOTES|HOSP|EMR|HOSP|20240301100000||ORU^R01|MSG00298|P|2.5
PID|1||MRN-FMT-001||FORMAT^TEST^PATIENT||19750515|F
ORC|RE|ORD-FMT-001
OBR|1|ORD-FMT-001||CLINOTE|||20240301090000|||||||||||||||20240301100000|||F
OBX|1|FT|CLINOTE^CLINICAL NOTE^LOCAL||\H\ASSESSMENT AND PLAN\N\\.br\\.br\1. \H\Hypertension\N\ - Blood pressure 158/94 today, above goal of 130/80\.br\\.in+5\- Increase lisinopril from 10mg to 20mg daily\.br\- Recheck BP in 2 weeks\.br\- Low sodium diet counseling provided\.br\\.in-5\\.br\2. \H\Type 2 Diabetes\N\ - A1c improved from 8.2\F\ to 7.1\F\\.br\\.in+5\- Continue metformin 1000mg BID\.br\- Repeat A1c in 3 months\.br\- Referral to nutritionist\.br\\.in-5\\.br\3. \H\Hyperlipidemia\N\ - LDL 172, above goal of \S\100\.br\\.in+5\- Start atorvastatin 20mg nightly\.br\- Repeat lipid panel in 6 weeks\.br\\.in-5\||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("NOTES"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00298"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("FORMAT"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("TEST"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("CLINOTE"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("CLINICAL NOTE"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_299_Should_parse_ADT_A01_comprehensive()
        {
            const string message = @"MSH|^~\&|ADT|MAIN_HOSP|MULTI_SYS|MAIN_HOSP|20240901080000||ADT^A01^ADT_A01|MSG00299|P|2.5|||AL|NE
EVN|A01|20240901080000||||20240901075500
PID|1||MRN-COMP-001^^^MAIN_HOSP^MR~SSN555-66-7777^^^SSA^SS||COMPREHENSIVE^ADAM^T^JR^MR||19650315|M||2106-3^White^HL70005|100 TEST BLVD^^COMPREHENSIVE CITY^CS^12345^USA^H^^COUNTY01|(555)111-2222^PRN^PH|(555)333-4444^WPN^PH||M|CAT^Catholic^HL70006|ACCT-COMP-001|||N^Not Hispanic^HL70189
PD1||||DR1100^PCP^DOCTOR^A^^^MAIN_HOSP||Y|I|20240101
ROL|1|AD|FHCP^Family Health Care Provider|DR1100^PCP^DOCTOR^A^^MD|20200101
NK1|1|COMPREHENSIVE^EVE^M|SPO^Spouse^HL70063|100 TEST BLVD^^COMPREHENSIVE CITY^CS^12345|(555)555-6666||EC^Emergency Contact
NK1|2|COMPREHENSIVE^ROBERT^A|FTH^Father^HL70063|200 PARENT LN^^PARENT CITY^CS^12346|(555)777-8888||NK^Next of Kin
PV1|1|I|MED^301^A^MAIN_HOSP^^^^FLOOR3||ED^TRIAGE^01|DR2200^ATTENDING^DOC^A^^MD|DR3300^REFERRING^DOC^B^^MD|DR4400^CONSULTING^DOC^C^^MD|MED|||CR^Critical|ADM|ER|0||DR2200^ATTENDING^DOC^A^^MD|IP|V-COMP-001||||||||||||||||||MAIN_HOSP||A|||20240901080000|||1
PV2||||||||20240901|5^days||CHEST PAIN EVALUATION AND CARDIAC MONITORING
ROL|2|AD|ADMITTING|DR2200^ATTENDING^DOC^A^^MD|20240901080000
DB1|1|AP^Handicap Parking|Y|20230101||HE^Hearing Impaired
OBX|1|NM|29463-7^BODY WEIGHT^LOINC||85.5|kg|||||F
OBX|2|NM|8302-2^BODY HEIGHT^LOINC||178|cm|||||F
AL1|1|DA^Drug Allergy|70618^PENICILLIN^RXNORM|SV^Severe|ANAPHYLAXIS
AL1|2|FA^Food Allergy|PEANUTS^PEANUTS^LOCAL|MO^Moderate|HIVES
DG1|1||I21.0^ACUTE MI ANTERIOR WALL^ICD10|ACUTE MI|20240901|A
DG1|2||I10^ESSENTIAL HYPERTENSION^ICD10|HTN|20200101|W
DRG|291^HEART FAILURE WITH MCC^MS-DRG|20240901|N||Y|1
PR1|1||93458^LEFT HEART CATH^CPT|LEFT HEART CATHETERIZATION|20240901090000|A||||||DR2200^ATTENDING^DOC^A^^MD
GT1|1||COMPREHENSIVE^ADAM^T^JR|100 TEST BLVD^^COMPREHENSIVE CITY^CS^12345|(555)111-2222|||||19650315|M|P|01^Self|SSN555-66-7777|||||EMPLOYER INC|999 WORK PLAZA^^WORK CITY^WS^67890|(555)999-0000
IN1|1|BCBS001^BLUE CROSS|BCBS|BLUE CROSS BLUE SHIELD|PO BOX 1234^^INSURANCE CITY^IN^45678|(800)555-1234|||GRP-9876||EMPLOYER INC|20240101|20241231|||COMPREHENSIVE^ADAM^T|01^Self|19650315|100 TEST BLVD^^COMPREHENSIVE CITY^CS^12345|||1||||||||||||||POL-COMP-001
IN2|1||SCN^Work Status^HL70066||||||||||||||||||||||||||||||||||||||||||||SSN555-66-7777
IN3|1||CERTIFIED^AUTH-001^BCBS|||20240901|20241231|||DR5500^AUTH^DOCTOR^A^^MD
ACC|20240831180000|43^AUTO^HL70050|INTERSECTION OF MAIN AND OAK ST";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ADT_A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00299"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A01"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("COMPREHENSIVE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("ADAM"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }

            var al1Result = parsed.Query(q => from msh in q.Select<MSH>() from al1 in q.Select<AL1>() select al1);
            if (al1Result.HasResult)
            {
                Assert.That(al1Result.Result.AllergenTypeCode.Value, Is.EqualTo("DA"));
            }

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
            {
                Assert.That(dg1Result.Result.DiagnosiCode.Value.Identifier.Value, Is.Not.Null.Or.Empty);
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("29463-7"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("BODY WEIGHT"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_300_Should_parse_ORU_comprehensive_multi_panel()
        {
            const string message = @"MSH|^~\&|MEGA_LAB|MAIN_HOSP|EMR|MAIN_HOSP|20240915103000||ORU^R01^ORU_R01|MSG00300|P|2.5|||AL|NE
SFT|MEGA LAB SYSTEMS|5.2.1|LAB INFORMATION SYSTEM|Binary ID 20240901|20240901
PID|1||MRN-300-001^^^MAIN_HOSP^MR~SSN444-55-6666^^^SSA^SS||MILESTONE^PATIENT^THREE-HUNDRED^JR||19750520|M||2054-5^Black^HL70005|300 CORPUS DRIVE^^FINAL CITY^FC^30000^USA^H||(555)300-3000|(555)300-3001||M|||ACCT-300-001
NK1|1|MILESTONE^SPOUSE^JANE|SPO^Spouse^HL70063|300 CORPUS DRIVE^^FINAL CITY^FC^30000|(555)300-3002||EC
PV1|1|O|LAB^DRAW^01||||DR3000^ATTENDING^DOC^THREE-HUNDRED^^MD
ORC|RE|ORD-300-A|LAB-300-A||CM||||20240915090000|||DR3000^ATTENDING^DOC^THREE-HUNDRED^^MD
OBR|1|ORD-300-A|LAB-300-A|58410-2^CBC W AUTO DIFFERENTIAL^LOINC|||20240915090000|||||||||DR3000^ATTENDING^DOC||||||20240915103000|||F
OBX|1|NM|6690-2^LEUKOCYTES^LOINC||12.5|10*3/uL|4.5-11.0|H|||F|||20240915103000
OBX|2|NM|789-8^ERYTHROCYTES^LOINC||4.8|10*6/uL|4.5-5.5|N|||F
OBX|3|NM|718-7^HEMOGLOBIN^LOINC||14.2|g/dL|13.5-17.5|N|||F
OBX|4|NM|4544-3^HEMATOCRIT^LOINC||42.5|%|38.0-50.0|N|||F
OBX|5|NM|777-3^PLATELETS^LOINC||185|10*3/uL|150-400|N|||F
NTE|1|L|Elevated WBC may indicate infection or inflammation. Clinical correlation recommended.
ORC|RE|ORD-300-B|LAB-300-B||CM||||20240915090000|||DR3000^ATTENDING^DOC^THREE-HUNDRED^^MD
OBR|2|ORD-300-B|LAB-300-B|51990-0^BASIC METABOLIC PANEL^LOINC|||20240915090000|||||||||DR3000^ATTENDING^DOC||||||20240915103000|||F
OBX|1|NM|2345-7^GLUCOSE^LOINC||215|mg/dL|74-106|HH|||F|||20240915103000
OBX|2|NM|2951-2^SODIUM^LOINC||138|mmol/L|136-145|N|||F
OBX|3|NM|2823-3^POTASSIUM^LOINC||5.8|mmol/L|3.5-5.1|H|||F|||20240915103000
OBX|4|NM|2160-0^CREATININE^LOINC||1.8|mg/dL|0.7-1.3|H|||F
NTE|1|L|CRITICAL: Glucose 215 mg/dL. Potassium 5.8 mmol/L is above critical threshold. Physician notified at 10:35 AM.
ORC|RE|ORD-300-C|LAB-300-C||CM||||20240915090000|||DR3000^ATTENDING^DOC^THREE-HUNDRED^^MD
OBR|3|ORD-300-C|LAB-300-C|24356-8^URINALYSIS COMPLETE^LOINC|||20240915090000|||||||||DR3000^ATTENDING^DOC||||||20240915103000|||F
OBX|1|ST|5778-6^COLOR^LOINC||Dark Yellow||||||F
OBX|2|NM|2756-5^PH^LOINC||5.5||5.0-8.0|N|||F
OBX|3|NM|2965-2^SPECIFIC GRAVITY^LOINC||1.030||1.005-1.030|N|||F
OBX|4|ST|5804-0^PROTEIN^LOINC||Trace||Negative|A|||F
NTE|1|L|Trace protein may be physiologic. Repeat if persistent. Dark urine color may indicate dehydration.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ORU_R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00300"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("MEGA_LAB"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("MILESTONE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("PATIENT"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("58410-2"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CBC W AUTO DIFFERENTIAL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("6690-2"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("LEUKOCYTES"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
            }
        }
    }
}
