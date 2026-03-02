namespace Machete.HL7.Tests.Corpus
{
    using NUnit.Framework;
    using Machete.HL7.Testing;
    using Machete.HL7Schema.V26;

    [TestFixture]
    public class CorpusTests_Batch08 :
        HL7MacheteTestHarness<MSH, HL7V26Entity>
    {
        [Test]
        public void HL7_SAMPLE_176_Should_parse_ORM_O01_radiology_CT_order()
        {
            const string message = @"MSH|^~\&|CPOE|HOSP|RIS|RADIOLOGY|20210401120000||ORM^O01|ORM_RAD_002|P|2.5
PID|1||MRN223344^^^HOSP^MR||THOMPSON^ROBERT^E||19650810|M
PV1|1|I|MED^201^A||||45678^WILLIAMS^DAVID^^^MD
ORC|NW|RAD_ORD_002^CPOE||||||R^ROUTINE||20210401120000|45678^WILLIAMS^DAVID^^^MD||45678^WILLIAMS^DAVID^^^MD
OBR|1|RAD_ORD_002^CPOE||74178^CT ABDOMEN PELVIS WITH CONTRAST^CPT|||20210402080000||||||||45678^WILLIAMS^DAVID^^^MD|||||||||R||||||||||ABDOMINAL PAIN EVALUATION
NTE|1||Clinical History: 55yo male with 2-week history of RLQ abdominal pain and low-grade fever. Rule out appendicitis vs diverticulitis.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORM"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("THOMPSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("ROBERT"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("NW"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("74178"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CT ABDOMEN PELVIS WITH CONTRAST"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Does.Contain("Clinical History"));
            }
        }

        [Test]
        public void HL7_SAMPLE_177_Should_parse_ORU_R01_CT_scan_report()
        {
            const string message = @"MSH|^~\&|RIS|RADIOLOGY|EMR|HOSP|20210402100000||ORU^R01|ORU_RAD_002|P|2.5
PID|1||MRN223344^^^HOSP^MR||THOMPSON^ROBERT^E||19650810|M
ORC|RE|RAD_ORD_002^CPOE|RADRPT002^RIS
OBR|1|RAD_ORD_002^CPOE|RADRPT002^RIS|74178^CT ABDOMEN PELVIS WITH CONTRAST^CPT|||20210402080000|||||||||45678^WILLIAMS^DAVID^^^MD||||||20210402100000||RAD|F||||||67890^LEE^DAVID^^^MD
OBX|1|TX|59776-5^FINDINGS^LN||TECHNIQUE: CT of the abdomen and pelvis with IV contrast.||||||F
OBX|2|TX|59776-5^FINDINGS^LN||The appendix is dilated measuring 12mm in diameter with periappendiceal fat stranding and a 5mm appendicolith at the base. No free fluid. No abscess.||||||F
OBX|3|TX|19005-8^IMPRESSION^LN||1. Acute appendicitis with appendicolith. No perforation or abscess.||||||F
OBX|4|TX|18783-1^RECOMMENDATION^LN||Surgical consultation recommended.||||||F";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("THOMPSON"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("74178"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CT ABDOMEN PELVIS WITH CONTRAST"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("59776-5"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_178_Should_parse_ORU_R01_multi_department_results()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401140000||ORU^R01|ORU_MULTI_001|P|2.3
PID|1||123456||DOE^JOHN||19800101|M
ORC|RE|ORD001^CPOE|CHEM001^LAB
OBR|1|ORD001^CPOE|CHEM001^LAB|80048^BASIC METABOLIC PANEL^CPT|||20210401130000||||||||||||||20210401140000||CHEM|F
OBX|1|NM|2345-7^Glucose^LN||95|mg/dL|65-99||||F
OBX|2|NM|2160-0^Creatinine^LN||1.1|mg/dL|0.7-1.3||||F
OBX|3|NM|2951-2^Sodium^LN||140|mEq/L|136-145||||F
OBX|4|NM|2823-3^Potassium^LN||4.2|mEq/L|3.5-5.0||||F
ORC|RE|ORD002^CPOE|HEM001^LAB
OBR|2|ORD002^CPOE|HEM001^LAB|85025^CBC W/AUTO DIFF^CPT|||20210401130000||||||||||||||20210401140000||HEM|F
OBX|1|NM|6690-2^WBC^LN||7.5|10*3/uL|4.5-11.0||||F
OBX|2|NM|789-8^RBC^LN||4.8|10*6/uL|4.5-5.5||||F
OBX|3|NM|718-7^Hemoglobin^LN||14.5|g/dL|13.0-17.0||||F
OBX|4|NM|4544-3^Hematocrit^LN||43|%|39-49||||F
OBX|5|NM|777-3^Platelets^LN||250|10*3/uL|150-400||||F";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JOHN"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("80048"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("BASIC METABOLIC PANEL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("2345-7"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_179_Should_parse_ADT_A04_pediatric_registration()
        {
            const string message = @"MSH|^~\&|ADT|PEDS_CLINIC|EMR|PEDS_CLINIC|20210401100000||ADT^A04|ADT_PED_001|P|2.5
EVN|A04|20210401100000
PID|1||PED_MRN001^^^CLINIC^MR||JOHNSON^EMMA^R||20180315|F||||||(555)555-1234||||||||||WILLIAMS
NK1|1|JOHNSON^SARAH^M|MTH^Mother^HL70063|456 ELM ST^^ANYTOWN^NY^12345|(555)555-1234|(555)555-5678
PV1|1|O|PEDS^CLINIC^1||||23456^BAKER^SARAH^^^MD";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A04"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A04"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("JOHNSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("EMMA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }
        }

        [Test]
        public void HL7_SAMPLE_180_Should_parse_ORU_R01_genetic_BRCA1_results()
        {
            const string message = @"MSH|^~\&|GEN_LAB|GENETICS|EMR|HOSP|20210401120000||ORU^R01|ORU_GEN_001|P|2.5
PID|1||MRN112233^^^HOSP^MR||COHEN^RACHEL^S||19780415|F
OBR|1||GEN001|BRCA^BRCA1/2 ANALYSIS^L|||20210315|||||||||34567^PATEL^PRIYA^^^MD||||||20210401120000||GEN|F
OBX|1|CWE|BRCA1^BRCA1 Gene Analysis^L||LA6669-1^Pathogenic Variant Detected^LN||||||F
OBX|2|ST|48004-6^DNA Sequence Variation^LN||c.68_69delAG||||||F
OBX|3|ST|48005-3^Amino Acid Change^LN||p.Glu23Valfs*17||||||F
OBX|4|CWE|53037-8^Genetic Disease Assessed^LN||C0006142^Hereditary Breast/Ovarian Cancer^MeSH||||||F
OBX|5|CWE|74019-1^Clinical Significance^LN||LA6668-3^Pathogenic^LN||||||F
NTE|1||This result indicates an increased lifetime risk for breast and ovarian cancer. Genetic counseling is recommended.";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("COHEN"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("RACHEL"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("BRCA"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("BRCA1/2 ANALYSIS"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("BRCA1"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_181_Should_parse_ADT_A08_multiple_allergies()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A08|ADT_AL_001|P|2.5
EVN|A08|20210401120000
PID|1||MRN998877^^^HOSP^MR||GARCIA^MARIA^L||19850322|F
AL1|1|DA|^PENICILLIN|SV|ANAPHYLAXIS||20100501
AL1|2|FA|^PEANUTS|MO|HIVES, SWELLING||20150301
AL1|3|MC|^LATEX|MI|CONTACT DERMATITIS||20180601";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A08"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A08"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("GARCIA"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MARIA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var al1Result = parsed.Query(q => from msh in q.Select<MSH>() from al1 in q.Select<AL1>() select al1);
            if (al1Result.HasResult)
            {
                Assert.That(al1Result.Result.AllergenTypeCode.Value, Is.EqualTo("DA"));
            }
        }

        [Test]
        public void HL7_SAMPLE_182_Should_parse_ORU_R01_negative_numeric_values()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401120000||ORU^R01|ORU_NEG_001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
OBR|1||LAB001|TEST^NEGATIVE VALUES TEST^L
OBX|1|NM|TEMP^Specimen Temperature^L||-2.5|Cel|||||F
OBX|2|NM|PH_ADJ^pH Adjustment^L||-0.01||||||F
OBX|3|NM|1925-7^Base Excess^LN||-3|mEq/L|-2 to +2|L|||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("TEST"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("TEMP"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_183_Should_parse_ORU_R01_decimal_precision_edge_cases()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401120000||ORU^R01|ORU_DEC_001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
OBR|1||LAB001|TEST^PRECISION TEST^L
OBX|1|NM|T1^Three Decimals^L||0.001|mg/L|||||F
OBX|2|NM|T2^Large Number^L||999999.99|units|||||F
OBX|3|NM|T3^Zero^L||0|units|||||F
OBX|4|NM|T4^Zero Point Zero^L||0.0|units|||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("TEST"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("PRECISION TEST"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("T1"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_184_Should_parse_ADT_A01_full_XAD_address()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A01|MSG_ADDR001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M|||123 MAIN ST^APT 4B^ANYTOWN^NY^12345^USA^H^COUNTY01^36061^999999^A^20200101^20301231^P
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
        public void HL7_SAMPLE_185_Should_parse_ORU_R01_newborn_screening_panel()
        {
            const string message = @"MSH|^~\&|NBS_LAB|STATE_LAB|EMR|HOSP|20210405090000||ORU^R01|ORU_NBS_001|P|2.5
PID|1||NBS12345^^^STATE^MR||BABY^BOY^A||20210402|M
OBR|1||NBS001|54089-8^NEWBORN SCREEN PANEL^LN|||20210403080000||||||||||||||20210405090000||NBS|F
OBX|1|NM|35572-7^PHENYLALANINE^LN||1.2|mg/dL|<4.0||||F|||20210403
OBX|2|NM|29575-8^TSH NEONATAL^LN||8.5|mIU/L|<20.0||||F|||20210403
OBX|3|NM|42906-8^GALACTOSE^LN||3.2|mg/dL|<10.0||||F|||20210403
OBX|4|ST|57703-1^HEMOGLOBIN PATTERN^LN||FA (Normal)||FA||||F|||20210403
OBX|5|NM|48633-2^TRYPSINOGEN IMMUNOREACTIVE^LN||25|ng/mL|<70||||F|||20210403
OBX|6|NM|46739-0^BIOTINIDASE^LN||55|ERU|>30||||F|||20210403
SPM|1|||DBS^Dried Blood Spot^HL70487";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("BABY"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("BOY"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("54089-8"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("NEWBORN SCREEN PANEL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("35572-7"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("PHENYLALANINE"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_186_Should_parse_ORU_R01_critical_panic_values()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401180000||ORU^R01|ORU_CRIT_001|P|2.5
PID|1||MRN445566^^^HOSP^MR||JONES^WILLIAM^R||19551020|M
OBR|1||CRIT001|80048^BMP^CPT|||20210401170000||||||||||||||20210401180000||LAB|F
OBX|1|NM|2823-3^Potassium^LN||6.8|mEq/L|3.5-5.0|HH|||F|||20210401180000
OBX|2|NM|2345-7^Glucose^LN||35|mg/dL|65-99|LL|||F|||20210401180000
NTE|1||CRITICAL VALUES: K 6.8, Glucose 35. Dr. Jones notified at 18:05 by lab tech Smith. Read back confirmed.";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("JONES"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("WILLIAM"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("80048"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("2823-3"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("Potassium"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Does.Contain("CRITICAL VALUES"));
            }
        }

        [Test]
        public void HL7_SAMPLE_187_Should_parse_ORM_O01_blood_bank_order()
        {
            const string message = @"MSH|^~\&|CPOE|HOSP|BB_SYS|BLOOD_BANK|20210401120000||ORM^O01|ORM_BB_001|P|2.3
PID|1||MRN998877^^^HOSP^MR||GARCIA^MARIA^L||19850322|F
PV1|1|I|ICU^301^A
ORC|NW|BB_ORD_001^CPOE||||||S||20210401120000|56789^RODRIGUEZ^CARLOS^^^MD
OBR|1|BB_ORD_001^CPOE||86900^TYPE AND SCREEN^CPT|||20210401120500
ORC|NW|BB_ORD_002^CPOE||||||S||20210401120000|56789^RODRIGUEZ^CARLOS^^^MD
OBR|2|BB_ORD_002^CPOE||86920^CROSSMATCH^CPT|||20210401120500||||||||||||||||||||2^UNITS PRBC";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("GARCIA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("NW"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("86900"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("TYPE AND SCREEN"));
            }
        }

        [Test]
        public void HL7_SAMPLE_188_Should_parse_ADT_A01_complex_visit_number()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A01|MSG_VN001|P|2.5
PID|1||123456^^^HOSP^MR||DOE^JOHN||19800101|M
PV1|1|I|MED^101^A||||12345^SMITH^JANE^^^MD|||||||||||VN12345^3^M10^HOSP^VN^HOSP&1.2.3.4&ISO^20210101^20211231^AUTH";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_VN001"));

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
        public void HL7_SAMPLE_189_Should_parse_ORU_R01_all_OBX_status_codes()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401120000||ORU^R01|ORU_STAT_001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
OBR|1||LAB001|TEST^STATUS TEST^L
OBX|1|NM|T1^Final||1||||||F
OBX|2|NM|T2^Preliminary||2||||||P
OBX|3|NM|T3^Corrected||3||||||C
OBX|4|NM|T4^Deleted||4||||||D
OBX|5|NM|T5^Withdrawn||5||||||W
OBX|6|NM|T6^Incomplete||||||I
OBX|7|NM|T7^Not Verified||7||||||R
OBX|8|NM|T8^Partial||8||||||S
OBX|9|NM|T9^Unavailable||||||U
OBX|10|NM|T10^Not Asked||||||N
OBX|11|NM|T11^Cannot Obtain||||||X";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("TEST"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("STATUS TEST"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("T1"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_190_Should_parse_ADT_A04_VIP_registration()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A04|ADT_VIP_001|P|2.5
EVN|A04|20210401120000
PID|1||MRN112233^^^HOSP^MR||CELEBRITY^FAMOUS^A||19700101|M
PV1|1|I|PVIP^SUITE^1||||12345^SMITH^JANE^^^MD||||||||Y||VN998877||||||||||||||||||||||||||20210401120000
PV2||||||||20210401|20210405||5^DAYS||||||||||WEDDING RING, WALLET WITH ID";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A04"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A04"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("CELEBRITY"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("FAMOUS"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_191_Should_parse_ORU_R01_corrected_result()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401150000||ORU^R01|ORU_CORR_001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
ORC|SC|ORD001^CPOE|LAB001^LAB
OBR|1|ORD001^CPOE|LAB001^LAB|2345-7^GLUCOSE^LN|||20210401120000||||||||||||||20210401150000||LAB|C
OBX|1|NM|2345-7^Glucose^LN||95|mg/dL|65-99||||C|||20210401150000
NTE|1||CORRECTED RESULT: Previous value 195 mg/dL reported at 12:30. Specimen re-analyzed due to sample handling error. Corrected value 95 mg/dL.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("SC"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("2345-7"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("GLUCOSE"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("2345-7"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("C"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Does.Contain("CORRECTED RESULT"));
            }
        }

        [Test]
        public void HL7_SAMPLE_192_Should_parse_ORU_R01_pending_no_OBX()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401120000||ORU^R01|ORU_PEND_001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
ORC|SC|ORD001^CPOE|LAB001^LAB
OBR|1|ORD001^CPOE|LAB001^LAB|80048^BMP^CPT|||20210401115000||||||||||||||20210401120000||LAB|O";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("SC"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("80048"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("BMP"));
            }
        }

        [Test]
        public void HL7_SAMPLE_193_Should_parse_ADT_A01_non_standard_encoding_chars()
        {
            const string message = @"MSH|#@*$|ADT|HOSP|EMR|HOSP|20210401120000||ADT#A01|MSG_ENC001|P|2.5
PID|1||123456||DOE#JOHN||19800101|M
PV1|1|I|MED#101#A";

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
        public void HL7_SAMPLE_194_Should_parse_ORU_R01_scientific_notation()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401120000||ORU^R01|ORU_SCI_001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
OBR|1||LAB001|TEST^SCIENTIFIC NOTATION^L
OBX|1|NM|T1^Large Value^L||1.5E3|copies/mL|||||F
OBX|2|NM|T2^Small Value^L||2.3E-4|mg/dL|||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("TEST"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("SCIENTIFIC NOTATION"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("T1"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("Large Value"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_195_Should_parse_ADT_A01_non_standard_segment_order()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A01|MSG_ORD001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
EVN|A01|20210401120000
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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JOHN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A01"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_196_Should_parse_ORU_R01_empty_PID_and_PV1_segments()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401120000||ORU^R01|MSG_EMPTY001|P|2.3
PID|
PV1|
OBR|1||LAB001|80048^BMP^CPT
OBX|1|NM|2345-7^Glucose^LN||100|mg/dL|65-99||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_EMPTY001"));

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

        [Test]
        public void HL7_SAMPLE_197_Should_parse_ADT_A01_multiple_GT1_guarantors()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A01|MSG_GT001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
PV1|1|I|MED^101^A
GT1|1|G001|DOE^JOHN||123 MAIN^^ANYTOWN^NY^12345|(555)111-1111||19800101|M||SE^Self
GT1|2|G002|DOE^JANE||123 MAIN^^ANYTOWN^NY^12345|(555)222-2222||19820301|F||SP^Spouse
GT1|3|G003|DOE^ROBERT||456 ELM^^ANYTOWN^NY^12345|(555)333-3333||19550101|M||PA^Parent
GT1|4|G004|||789 OAK^^ANYTOWN^NY^12345|(555)444-4444|||||EM^Employer|||ACME CORP
GT1|5|G005|SMITH^WILLIAM||111 PINE^^ANYTOWN^NY^12345|(555)555-5555||19600601|M||OT^Other";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_GT001"));

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
        public void HL7_SAMPLE_198_Should_parse_ADT_A01_character_set_8859_15()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A01|MSG_CS001|P|2.5|||AL|AL||8859/15
PID|1||123456||MUELLER^HANS||19700101|M
PV1|1|I|MED^101^A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_CS001"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("MUELLER"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("HANS"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_199_Should_parse_ORU_R01_full_UCUM_units()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210401120000||ORU^R01|ORU_UNIT_001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
OBR|1||LAB001|2345-7^GLUCOSE^LN
OBX|1|NM|2345-7^Glucose^LN^GLU^Glucose^LOCAL||100|mg/dL^milligrams per deciliter^UCUM^MG/DL^mg/dl^L|65-99^mg/dL||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("2345-7"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("GLUCOSE"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("2345-7"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("Glucose"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_200_Should_parse_ADT_A01_milestone_standard_admission()
        {
            const string message = @"MSH|^~\&|ADT_SYS|GENERAL_HOSP|EMR_SYS|GENERAL_HOSP|20210401120000||ADT^A01^ADT_A01|MSG_200_MILESTONE|P|2.5|||AL|NE
EVN|A01|20210401120000||||200601
PID|1||MRN200200^^^HOSP^MR~SSN200200^^^SSA^SS||MILESTONE^PATIENT^T^JR||19800401|M||2106-3^White^CDCREC|100 CORPUS DRIVE^^TESTVILLE^TX^75001^USA^H||(555)200-0200^PRN^PH|(555)200-0201^WPN^PH||M^Married^HL70002||200200200^^^HOSP^AN|200-20-0200||||2186-5^Not Hispanic^CDCREC||||||||N
NK1|1|MILESTONE^SPOUSE^A|SPO^Spouse^HL70063|100 CORPUS DRIVE^^TESTVILLE^TX^75001|(555)200-0202|||EC
PV1|1|I|MED^200^A^GENERAL_HOSP^^^^2||||12345^ADMITTING^DR^A^^^MD|67890^ATTENDING^DR^B^^^MD||MED|||||||12345^ADMITTING^DR^A^^^MD|I|VN200200|||||||||||||||||||GENERAL_HOSP|||||20210401120000
DG1|1||J18.9^Pneumonia, unspecified organism^I10|Pneumonia|20210401|A
IN1|1|COMM|INS200|GENERAL INSURANCE CO|PO BOX 200^^INSURANCE CITY^TX^75002||^WPN^PH^^1^800^2000200|GRP200||||||PPO||||MILESTONE^PATIENT^T^JR|1|19800401||||||||||||||INS200200";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ADT_A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_200_MILESTONE"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("ADT_SYS"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("GENERAL_HOSP"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A01"));
            }

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
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
            {
                Assert.That(dg1Result.Result.DiagnosiCode.HasValue, Is.True);
            }
        }
    }
}
