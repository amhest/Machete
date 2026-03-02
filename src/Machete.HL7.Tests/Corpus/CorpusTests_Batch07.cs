namespace Machete.HL7.Tests.Corpus
{
    using NUnit.Framework;
    using Machete.HL7.Testing;
    using Machete.HL7Schema.V26;

    [TestFixture]
    public class CorpusTests_Batch07 :
        HL7MacheteTestHarness<MSH, HL7V26Entity>
    {
        [Test]
        public void HL7_SAMPLE_151_Should_parse_ORM_pharmacy_vancomycin_order()
        {
            const string message = @"MSH|^~\&|CPOE|ICU|PHARMACY|HOSP|20210316160000||ORM^O01|ORM_RX_001|P|2.3
PID|1||MRN998877^^^HOSP^MR||GARCIA^MARIA^L||19850322|F
PV1|1|I|ICU^301^A
ORC|NW|RX_LOAD_001^CPOE||||||S||20210316160000|56789^RODRIGUEZ^CARLOS^^^MD
OBR|1|RX_LOAD_001^CPOE||VANC_LOAD^VANCOMYCIN LOADING DOSE^L
RXO|VANCOMYCIN^VANCOMYCIN 1G/200ML^L|1|1|G^GRAM||IV^INTRAVENOUS|1^^ONCE^^NOW
ORC|NW|RX_MAINT_001^CPOE||||||R||20210316160000|56789^RODRIGUEZ^CARLOS^^^MD
OBR|1|RX_MAINT_001^CPOE||VANC_MAINT^VANCOMYCIN MAINTENANCE^L
RXO|VANCOMYCIN^VANCOMYCIN 1G/200ML^L|1|1|G^GRAM||IV^INTRAVENOUS|1^^Q12H^^20210316180000^20210323180000";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORM"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("CPOE"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("ICU"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("GARCIA"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MARIA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("NW"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("VANC_LOAD"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("VANCOMYCIN LOADING DOSE"));
            }
        }

        [Test]
        public void HL7_SAMPLE_152_Should_parse_ORU_ABG_results()
        {
            const string message = @"MSH|^~\&|ABG_ANALYZER|ICU|EMR|HOSP|20210316170000||ORU^R01|ORU_ABG_001|P|2.5
PID|1||MRN998877^^^HOSP^MR||GARCIA^MARIA^L||19850322|F
ORC|RE|ORD_ABG_001^ICU
OBR|1|ORD_ABG_001||82803^BLOOD GAS ARTERIAL^CPT|||20210316165500|||||||||56789^RODRIGUEZ^CARLOS^^^MD||||||20210316170000||LAB|F
OBX|1|NM|2744-1^pH^LN||7.35||7.35-7.45||||F
OBX|2|NM|2019-8^pCO2^LN||45|mmHg|35-45||||F
OBX|3|NM|2703-7^pO2^LN||80|mmHg|80-100||||F
OBX|4|NM|1963-8^HCO3^LN||24|mEq/L|22-26||||F
OBX|5|NM|1925-7^Base Excess^LN||-1|mEq/L|-2 to +2||||F
OBX|6|NM|2708-6^O2 Saturation^LN||95|%|95-100||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("ABG_ANALYZER"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("GARCIA"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("82803"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("BLOOD GAS ARTERIAL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("2744-1"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("pH"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_153_Should_parse_ORU_hepatic_function_panel()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210323100000||ORU^R01|ORU_LFT_001|P|2.5
PID|1||MRN223344^^^HOSP^MR||THOMPSON^ROBERT^E||19650810|M
ORC|RE|ORD_LFT_001
OBR|1|ORD_LFT_001||24326-1^HEPATIC FUNCTION PANEL^LN|||20210323090000||||||||||||||20210323100000||LAB|F
OBX|1|NM|1742-6^ALT^LN||55|IU/L|7-56|H|||F
OBX|2|NM|1920-8^AST^LN||48|IU/L|10-40|H|||F
OBX|3|NM|6768-6^Alkaline Phosphatase^LN||95|IU/L|44-147||||F
OBX|4|NM|1975-2^Total Bilirubin^LN||1.8|mg/dL|0.1-1.2|H|||F
OBX|5|NM|1968-7^Direct Bilirubin^LN||0.8|mg/dL|0.0-0.3|H|||F
OBX|6|NM|1751-7^Albumin^LN||3.2|g/dL|3.5-5.5|L|||F
OBX|7|NM|2885-2^Total Protein^LN||7.0|g/dL|6.0-8.3||||F
OBX|8|NM|2324-2^GGT^LN||85|IU/L|0-65|H|||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ORU_LFT_001"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("THOMPSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("ROBERT"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("24326-1"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("HEPATIC FUNCTION PANEL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("1742-6"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("ALT"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_154_Should_parse_ORU_cardiac_markers()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210319210000||ORU^R01|ORU_CARD_001|P|2.5
PID|1||MRN334455^^^HOSP^MR||MARTINEZ^CARLOS^E||19680714|M
ORC|RE|ORD_CARD_001
OBR|1|ORD_CARD_001||CARDIAC^CARDIAC MARKERS^L|||20210319200000|||||||||89012^SMITH^JENNIFER^^^MD||||||20210319210000||LAB|F
OBX|1|NM|10839-9^Troponin I^LN||0.15|ng/mL|0.00-0.04|HH|||F|||20210319210000
OBX|2|NM|13969-1^CK-MB^LN||8.5|ng/mL|0.0-5.0|H|||F|||20210319210000
OBX|3|NM|30934-4^BNP^LN||450|pg/mL|0-100|H|||F|||20210319210000
NTE|1||CRITICAL VALUE: Troponin I elevated. Provider notified at 21:05.";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("MARTINEZ"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("CARLOS"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("CARDIAC"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CARDIAC MARKERS"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("10839-9"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("Troponin I"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Does.Contain("CRITICAL VALUE"));
            }
        }

        [Test]
        public void HL7_SAMPLE_155_Should_parse_ORU_escaped_pipe_in_value()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210320090000||ORU^R01|ORU_ESC_001|P|2.3
PID|1||123456||DOE^JOHN||19800101|M
OBR|1||LAB001|TEST^ESCAPE TEST^L
OBX|1|ST|TEST^Escape Test||Range: 10\F\20\F\30||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ORU_ESC_001"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("TEST"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("ESCAPE TEST"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("Escape Test"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_156_Should_parse_ORU_chained_escape_sequences()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210320090000||ORU^R01|ORU_ESC_002|P|2.5
OBX|1|ST|TEST^Chained Escapes||\F\\S\\R\\T\\E\||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ORU_ESC_002"));

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("Chained Escapes"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_157_Should_parse_ADT_heavy_field_repetition()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A01|MSG_REP001|P|2.5
PID|1||MRN001^^^HOSP^MR~SSN001^^^SSA^SS~DL001^^^DMV^DL||SMITH^JOHN^A^JR^MR^^L~SMITHY^JOHNNY^^^^^A||19800101|M|||123 HOME ST^^ANYTOWN^NY^12345^USA^H~PO BOX 456^^ANYTOWN^NY^12345^USA^M||(555)555-1111^PRN^PH~(555)555-2222^WPN^PH~(555)555-3333^ORN^CP
PV1|1|I|MED^101^A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("SMITH"));
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
        public void HL7_SAMPLE_158_Should_parse_ORU_empty_observation_value()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401120000||ORU^R01|ORU_EMPTY_001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
OBR|1||LAB001|80048^BMP^CPT
OBX|1|NM|2345-7^Glucose^LN||||65-99|||X|||20210401
NTE|1||Specimen hemolyzed. Unable to perform glucose analysis.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ORU_EMPTY_001"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("80048"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("BMP"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("2345-7"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("X"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Does.Contain("hemolyzed"));
            }
        }

        [Test]
        public void HL7_SAMPLE_159_Should_parse_ADT_full_name_components()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A04|MSG_NAME001|P|2.5
PID|1||123456^^^HOSP^MR||SMITH^JOHN^MICHAEL^III^SIR^PHD^L||19800101|M
PV1|1|O|CLINIC^101^1";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A04"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("SMITH"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JOHN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }
        }

        [Test]
        public void HL7_SAMPLE_160_Should_parse_ORU_non_numeric_NM_value()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401120000||ORU^R01|ORU_NM_001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
OBR|1||LAB001|2093-3^Cholesterol^LN
OBX|1|NM|2093-3^Cholesterol^LN||>1000|mg/dL|<200|HH|||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ORU_NM_001"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("2093-3"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("Cholesterol"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("2093-3"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_161_Should_parse_ORU_structured_numeric_comparators()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401120000||ORU^R01|ORU_SN_001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
OBR|1||LAB001|TEST^MULTIPLE SN^L
OBX|1|SN|1554-5^Glucose^LN||^182|mg/dL|70-105||||F
OBX|2|SN|TEST1^Low Value^L||<^10|mg/dL|||||F
OBX|3|SN|TEST2^High Value^L||>^100|mg/dL|||||F
OBX|4|SN|TEST3^GTE Value^L||>=^5.0|mg/dL|||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ORU_SN_001"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("TEST"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("MULTIPLE SN"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("1554-5"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("Glucose"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_162_Should_parse_ADT_very_long_address()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A01|MSG_LONG001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M|||APARTMENT BUILDING NUMBER TWELVE HUNDRED AND THIRTY FOUR ON THE CORNER OF FIRST AVENUE AND MAIN STREET SUITE FOUR HUNDRED AND FIFTY SIX FLOOR TWENTY THREE WING B SECTION C UNIT D^^SPRINGFIELD^IL^62701
PV1|1|I|MED^101^A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_LONG001"));

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
        public void HL7_SAMPLE_163_Should_parse_ORU_50_OBX_segments()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401120000||ORU^R01|ORU_MANY_001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
OBR|1||LAB001|PANEL^COMPREHENSIVE PANEL^L
OBX|1|NM|TEST01^Test 01^L||1.0|units|||||F
OBX|2|NM|TEST02^Test 02^L||2.0|units|||||F
OBX|3|NM|TEST03^Test 03^L||3.0|units|||||F
OBX|4|NM|TEST04^Test 04^L||4.0|units|||||F
OBX|5|NM|TEST05^Test 05^L||5.0|units|||||F
OBX|6|NM|TEST06^Test 06^L||6.0|units|||||F
OBX|7|NM|TEST07^Test 07^L||7.0|units|||||F
OBX|8|NM|TEST08^Test 08^L||8.0|units|||||F
OBX|9|NM|TEST09^Test 09^L||9.0|units|||||F
OBX|10|NM|TEST10^Test 10^L||10.0|units|||||F
OBX|11|NM|TEST11^Test 11^L||11.0|units|||||F
OBX|12|NM|TEST12^Test 12^L||12.0|units|||||F
OBX|13|NM|TEST13^Test 13^L||13.0|units|||||F
OBX|14|NM|TEST14^Test 14^L||14.0|units|||||F
OBX|15|NM|TEST15^Test 15^L||15.0|units|||||F
OBX|16|NM|TEST16^Test 16^L||16.0|units|||||F
OBX|17|NM|TEST17^Test 17^L||17.0|units|||||F
OBX|18|NM|TEST18^Test 18^L||18.0|units|||||F
OBX|19|NM|TEST19^Test 19^L||19.0|units|||||F
OBX|20|NM|TEST20^Test 20^L||20.0|units|||||F
OBX|21|NM|TEST21^Test 21^L||21.0|units|||||F
OBX|22|NM|TEST22^Test 22^L||22.0|units|||||F
OBX|23|NM|TEST23^Test 23^L||23.0|units|||||F
OBX|24|NM|TEST24^Test 24^L||24.0|units|||||F
OBX|25|NM|TEST25^Test 25^L||25.0|units|||||F
OBX|26|NM|TEST26^Test 26^L||26.0|units|||||F
OBX|27|NM|TEST27^Test 27^L||27.0|units|||||F
OBX|28|NM|TEST28^Test 28^L||28.0|units|||||F
OBX|29|NM|TEST29^Test 29^L||29.0|units|||||F
OBX|30|NM|TEST30^Test 30^L||30.0|units|||||F
OBX|31|NM|TEST31^Test 31^L||31.0|units|||||F
OBX|32|NM|TEST32^Test 32^L||32.0|units|||||F
OBX|33|NM|TEST33^Test 33^L||33.0|units|||||F
OBX|34|NM|TEST34^Test 34^L||34.0|units|||||F
OBX|35|NM|TEST35^Test 35^L||35.0|units|||||F
OBX|36|NM|TEST36^Test 36^L||36.0|units|||||F
OBX|37|NM|TEST37^Test 37^L||37.0|units|||||F
OBX|38|NM|TEST38^Test 38^L||38.0|units|||||F
OBX|39|NM|TEST39^Test 39^L||39.0|units|||||F
OBX|40|NM|TEST40^Test 40^L||40.0|units|||||F
OBX|41|NM|TEST41^Test 41^L||41.0|units|||||F
OBX|42|NM|TEST42^Test 42^L||42.0|units|||||F
OBX|43|NM|TEST43^Test 43^L||43.0|units|||||F
OBX|44|NM|TEST44^Test 44^L||44.0|units|||||F
OBX|45|NM|TEST45^Test 45^L||45.0|units|||||F
OBX|46|NM|TEST46^Test 46^L||46.0|units|||||F
OBX|47|NM|TEST47^Test 47^L||47.0|units|||||F
OBX|48|NM|TEST48^Test 48^L||48.0|units|||||F
OBX|49|NM|TEST49^Test 49^L||49.0|units|||||F
OBX|50|NM|TEST50^Test 50^L||50.0|units|||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ORU_MANY_001"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("PANEL"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("COMPREHENSIVE PANEL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("TEST01"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_164_Should_parse_ADT_full_location_components()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A01|MSG_LOC001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
PV1|1|I|MED^201^A^HOSP^Active^Wing_B^3^Nursing^Room_Type||||12345^SMITH^JANE^^^MD";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_LOC001"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_165_Should_parse_ORU_CWE_blood_culture()
        {
            const string message = @"MSH|^~\&|MICRO|HOSP|EMR|HOSP|20210401120000||ORU^R01|ORU_CWE_001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
OBR|1||MCR001|87040^BLOOD CULTURE^CPT
OBX|1|CWE|600-7^BACTERIA IDENTIFIED^LN||112283007^Escherichia coli^SCT^ECOLI^E. coli^L||||||F
OBX|2|CWE|18861-1^AMOXICILLIN SUSCEPTIBILITY^LN||R^Resistant^HL70078||||||F
OBX|3|CWE|18862-9^AMPICILLIN SUSCEPTIBILITY^LN||R^Resistant^HL70078||||||F
OBX|4|CWE|18928-8^GENTAMICIN SUSCEPTIBILITY^LN||S^Susceptible^HL70078||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("MICRO"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("87040"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("BLOOD CULTURE"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("600-7"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("BACTERIA IDENTIFIED"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_166_Should_parse_ADT_year_only_DOB()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A01|MSG_DOB001|P|2.5
PID|1||123456||DOE^JOHN||1980|M
PV1|1|I|MED^101^A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_DOB001"));

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
        public void HL7_SAMPLE_167_Should_parse_ADT_non_binary_sex_code()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A01|MSG_SEX001|P|2.5
PID|1||123456||DOE^ALEX||19950101|O
PV1|1|I|MED^101^A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_SEX001"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("ALEX"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("O"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_168_Should_parse_ORU_datetime_precision_variation()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210315093000||ORU^R01|ORU_DT_001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
OBR|1||LAB001|TEST^DATETIME TEST^L
OBX|1|NM|T1^Test 1||1||||||F|||2021
OBX|2|NM|T2^Test 2||2||||||F|||202103
OBX|3|NM|T3^Test 3||3||||||F|||20210315
OBX|4|NM|T4^Test 4||4||||||F|||2021031509
OBX|5|NM|T5^Test 5||5||||||F|||202103150930
OBX|6|NM|T6^Test 6||6||||||F|||20210315093000
OBX|7|NM|T7^Test 7||7||||||F|||20210315093000.1234";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ORU_DT_001"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("TEST"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("DATETIME TEST"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("T1"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_169_Should_parse_ADT_full_MSH_fields()
        {
            const string message = @"MSH|^~\&|ADT_SYS|HOSP|EMR|HOSP|20210401120000|SECURITY_TOKEN|ADT^A01^ADT_A01|MSG_FULL_MSH|P|2.5|123|CONT_PTR|AL|AL|USA|UNICODE UTF-8|EN^English^ISO639|C|PROFILE001^HL7^2.16.840.1.113883.9.1^ISO
PID|1||123456||DOE^JOHN||19800101|M
PV1|1|I|MED^101^A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ADT_A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_FULL_MSH"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("ADT_SYS"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("HOSP"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_170_Should_parse_ADT_security_field()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000|SEC_TOKEN_XYZ|ADT^A01|MSG_SEC001|P|2.3
PID|1||123456||DOE^JOHN||19800101|M
PV1|1|I|MED^101^A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_SEC001"));

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
        public void HL7_SAMPLE_171_Should_parse_ORU_lipid_panel_with_ratios()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210325090000||ORU^R01|ORU_LIPID_001|P|2.5
PID|1||MRN445566^^^HOSP^MR||JONES^WILLIAM^R||19551020|M
OBR|1||LP001|57698-3^LIPID PANEL^LN|||20210325080000||||||||||||||20210325090000||LAB|F
OBX|1|NM|2093-3^Cholesterol Total^LN||225|mg/dL|<200|H|||F
OBX|2|NM|2571-8^Triglycerides^LN||200|mg/dL|<150|H|||F
OBX|3|NM|2085-9^HDL Cholesterol^LN||35|mg/dL|>40|LL|||F
OBX|4|NM|13457-7^LDL Cholesterol Calc^LN||150|mg/dL|<100|H|||F
OBX|5|NM|13458-5^VLDL Cholesterol Calc^LN||40|mg/dL|5-40||||F
OBX|6|NM|9830-1^Total/HDL Ratio^LN||6.4||<5.0|H|||F
NTE|1||High cardiovascular risk profile. Recommend lifestyle modification and statin therapy evaluation.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ORU_LIPID_001"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("JONES"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("WILLIAM"));
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
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("Cholesterol Total"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Does.Contain("cardiovascular risk"));
            }
        }

        [Test]
        public void HL7_SAMPLE_172_Should_parse_ORU_CSF_analysis()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401180000||ORU^R01|ORU_CSF_001|P|2.5
PID|1||MRN887766^^^HOSP^MR||DAVIS^MICHAEL^T||19450612|M
OBR|1||CSF001|89050^CSF ANALYSIS^CPT|||20210401170000||||||||||||||20210401180000||LAB|F
OBX|1|ST|30404-8^CSF Appearance^LN||Cloudy||Clear|A|||F
OBX|2|NM|26464-8^CSF WBC^LN||25|cells/uL|0-5|HH|||F
OBX|3|NM|2880-3^CSF Protein^LN||85|mg/dL|15-45|H|||F
OBX|4|NM|2342-4^CSF Glucose^LN||45|mg/dL|50-80|L|||F
OBX|5|ST|664-3^Gram Stain^LN||Gram positive cocci in pairs||No organisms seen|A|||F
NTE|1||CRITICAL: Elevated WBC, elevated protein, and positive gram stain. Findings consistent with bacterial meningitis. Provider notified.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ORU_CSF_001"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DAVIS"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MICHAEL"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("89050"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CSF ANALYSIS"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("30404-8"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("CSF Appearance"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Does.Contain("bacterial meningitis"));
            }
        }

        [Test]
        public void HL7_SAMPLE_173_Should_parse_ORU_HbA1c_diabetes()
        {
            const string message = @"MSH|^~\&|LAB|CLINIC|EMR|CLINIC|20210401090000||ORU^R01|ORU_A1C_001|P|2.5
PID|1||MRN554433^^^CLINIC^MR||WILLIAMS^SARAH^M||19680301|F
OBR|1||HBA1C001|4548-4^HEMOGLOBIN A1C^LN|||20210401080000||||||||||||||20210401090000||LAB|F
OBX|1|NM|4548-4^Hemoglobin A1c^LN||8.5|%|<5.7|H|||F
NTE|1||Estimated Average Glucose (eAG) = 197 mg/dL
NTE|2||ADA Guidelines: <5.7% Normal, 5.7-6.4% Prediabetes, >=6.5% Diabetes
NTE|3||ADA target for most adults with diabetes: <7.0%";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ORU_A1C_001"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("LAB"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("CLINIC"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("WILLIAMS"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("SARAH"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("4548-4"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("HEMOGLOBIN A1C"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("4548-4"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("Hemoglobin A1c"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Does.Contain("eAG"));
            }
        }

        [Test]
        public void HL7_SAMPLE_174_Should_parse_ADT_multi_race_values()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A01|MSG_RACE001|P|2.5
PID|1||123456^^^HOSP^MR||DOE^JOHN||19900101|M||2054-5^Black or African American^CDCREC~2106-3^White^CDCREC~2076-8^Native Hawaiian or Other Pacific Islander^CDCREC|123 MAIN ST^^ANYTOWN^NY^12345|||||||||||2135-2^Hispanic or Latino^CDCREC
PV1|1|I|MED^101^A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_RACE001"));

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
        public void HL7_SAMPLE_175_Should_parse_ORU_training_mode_long_control_id()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401120000||ORU^R01|ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0123|T|2.5
PID|1||123456||DOE^JOHN||19800101|M
OBR|1||LAB001|80048^BMP^CPT|||20210401
OBX|1|NM|2345-7^Glucose^LN||95|mg/dL|65-99||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0123"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("80048"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("BMP"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("2345-7"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("Glucose"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }
    }
}
