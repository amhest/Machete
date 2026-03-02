namespace Machete.HL7.Tests.Corpus
{
    using NUnit.Framework;
    using Machete.HL7.Testing;
    using Machete.HL7Schema.V26;

    [TestFixture]
    public class CorpusTests_Batch11 :
        HL7MacheteTestHarness<MSH, HL7V26Entity>
    {
        [Test]
        public void HL7_SAMPLE_251_Should_parse_ORM_O01_multiple_orders()
        {
            const string message =
                @"MSH|^~\&|CPOE|SURG_CTR|LAB|MAIN_HOSP|200910051030||ORM^O01|MSG00251|P|2.3
PID|1||MRN554400^^^MAIN_HOSP^MR||NELSON^RICHARD^A||19651122|M|||567 BROADWAY^^NEW YORK^NY^10012||2125559988
PV1|1|I|SURG^PRE-OP^01||||DR4455^ROGERS^WILLIAM|||SURG|||||||||V-09-7788
ORC|NW|ORD-A|||||^^^^^S||200910051030|||DR4455^ROGERS^WILLIAM
OBR|1|ORD-A||58410-2^CBC W DIFF^LOINC|||200910051030||||||||||DR4455^ROGERS^WILLIAM
ORC|NW|ORD-B|||||^^^^^S||200910051030|||DR4455^ROGERS^WILLIAM
OBR|2|ORD-B||51990-0^BASIC METABOLIC PANEL^LOINC|||200910051030||||||||||DR4455^ROGERS^WILLIAM
ORC|NW|ORD-C|||||^^^^^S||200910051030|||DR4455^ROGERS^WILLIAM
OBR|3|ORD-C||11580-8^TSH^LOINC|||200910051030||||||||||DR4455^ROGERS^WILLIAM";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("NELSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("RICHARD"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("NW"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("58410-2"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CBC W DIFF"));
            }
        }

        [Test]
        public void HL7_SAMPLE_252_Should_parse_ORM_O01_cancel_order()
        {
            const string message =
                @"MSH|^~\&|CPOE|MAIN_HOSP|RIS|RAD_DEPT|200812151400||ORM^O01|MSG00252|P|2.3
PID|1||MRN776600^^^MAIN_HOSP^MR||CLARK^SUSAN^E||19780515|F
PV1|1|I|NEURO^303^A||||DR5566^SHAH^PRIYA
ORC|CA|ORD-RAD-001|RAD-FILL-001||||||||DR5566^SHAH^PRIYA|||||DUP^Duplicate Order^LOCAL
OBR|1|ORD-RAD-001|RAD-FILL-001|70450^CT HEAD WITHOUT CONTRAST^CPT|||200812151300";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("CLARK"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("SUSAN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("CA"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("70450"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CT HEAD WITHOUT CONTRAST"));
            }
        }

        [Test]
        public void HL7_SAMPLE_253_Should_parse_ORM_O01_hold_order()
        {
            const string message =
                @"MSH|^~\&|BLOOD_BANK|MAIN_HOSP|CPOE|MAIN_HOSP|20040722160000||ORM^O01|MSG00253|P|2.4
PID|1||MRN223355^^^MAIN_HOSP^MR||WRIGHT^THOMAS^B||19500830|M|||789 OAK DR^^ATLANTA^GA^30301
PV1|1|I|SURG^ICU^03||||DR7788^JONES^MICHAEL
ORC|HD|ORD-BT-001|||HD|||||||||HOLD PENDING TYPE AND SCREEN CONFIRMATION
OBR|1|ORD-BT-001||36430^TRANSFUSION RBC^CPT|||20040722155000||||||||DR7788^JONES^MICHAEL
NTE|1||Hold transfusion until type and screen results confirmed. Patient has history of transfusion reactions.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORM"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("WRIGHT"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("THOMAS"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("HD"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("36430"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("TRANSFUSION RBC"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Does.Contain("Hold transfusion"));
            }
        }

        [Test]
        public void HL7_SAMPLE_254_Should_parse_ORM_O01_discontinue_order()
        {
            const string message =
                @"MSH|^~\&|CPOE|MAIN_HOSP|PHARMACY|MAIN_HOSP|200603201800||ORM^O01|MSG00254|P|2.3
PID|1||MRN445511^^^MAIN_HOSP^MR||LEE^JENNIFER^K||19870630|F
PV1|1|I|MED^201^A||||DR2233^PATEL^AMIT
ORC|DC|ORD-IV-003|||DC||||200603201800||DR2233^PATEL^AMIT||||Patient tolerating oral fluids
RXO|0009-7983-01^NORMAL SALINE 0.9% 1000ML^NDC||1000|mL|IV^Intravenous||||||DR2233^PATEL^AMIT";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("LEE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JENNIFER"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("DC"));
            }
        }

        [Test]
        public void HL7_SAMPLE_255_Should_parse_ORU_R01_microbiology_blood_culture()
        {
            const string message =
                @"MSH|^~\&|MICRO_LAB|MAIN_HOSP|EMR|MAIN_HOSP|200507281400||ORU^R01|MSG00255|P|2.3.1
PID|1||MRN889944^^^MAIN_HOSP^MR||HARRIS^DONNA^M||19680214|F
OBR|1|ORD-BC001|LAB-BC001|600-7^BLOOD CULTURE^LOINC|||200507270800||||||||DR3344^SMITH^JOHN||||||200507281400|||F
OBX|1|CE|600-7^BLOOD CULTURE^LOINC|1|3092008^Staphylococcus aureus^SCT||||||F
OBX|2|NM|564-5^COLONY COUNT^LOINC|1|>100000|CFU/mL|||||F
OBR|2|ORD-BC001-S|LAB-BC001-S|29576-6^BACTERIAL SUSCEPTIBILITY PANEL^LOINC|||200507270800||||||||DR3344^SMITH^JOHN||||||200507281400|||F|||||||ORD-BC001^LAB-BC001
OBX|1|ST|18900-1^CEFAZOLIN SUSCEPTIBILITY^LOINC|1|S||||||F
OBX|2|NM|18900-1^CEFAZOLIN MIC^LOINC|1|<=2|ug/mL|<=8||||F
OBX|3|ST|18961-3^OXACILLIN SUSCEPTIBILITY^LOINC|1|S||||||F
OBX|4|ST|18964-7^PENICILLIN SUSCEPTIBILITY^LOINC|1|R||||||F
OBX|5|ST|18993-6^VANCOMYCIN SUSCEPTIBILITY^LOINC|1|S||||||F
OBX|6|NM|18993-6^VANCOMYCIN MIC^LOINC|1|1|ug/mL|<=2||||F
OBX|7|ST|18878-9^CIPROFLOXACIN SUSCEPTIBILITY^LOINC|1|I||||||F
OBX|8|NM|18878-9^CIPROFLOXACIN MIC^LOINC|1|2|ug/mL|<=1||||F";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("HARRIS"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("600-7"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("BLOOD CULTURE"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("600-7"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_256_Should_parse_ORU_R01_genetic_BRCA1()
        {
            const string message =
                @"MSH|^~\&|GENETICS_LAB|MAIN_HOSP|EMR|MAIN_HOSP|20240801140000||ORU^R01^ORU_R01|MSG00256|P|2.7|||AL|NE||||||
PID|1||MRN554477^^^MAIN_HOSP^MR||TAYLOR^AMANDA^L||19820510|F||2106-3^White^HL70005|456 GARDEN RD^^HOUSTON^TX^77001||7135554433
ORC|RE|ORD-GEN001|GEN-001||CM
OBR|1|ORD-GEN001|GEN-001|81211^BRCA1 FULL GENE ANALYSIS^CPT|||20240725|||||||||DR1100^CHEN^LINDA||||||20240801140000|||F||||||||||||||NGS^Next Generation Sequencing
OBX|1|CWE|48018-6^GENE STUDIED^LN||BRCA1^BRCA1^HGNC||||||F|||20240801140000
OBX|2|CWE|81252-9^DISCRETE GENETIC VARIANT^LN||c.5266dupC^BRCA1 c.5266dupC (p.Gln1756Profs*74)^HGVS||||||F
OBX|3|CWE|53037-8^GENETIC DISEASE ASSESSED^LN||718220008^Hereditary breast and ovarian cancer syndrome^SCT||||||F
OBX|4|CWE|69548-6^GENETIC VARIANT ASSESSMENT^LN||LA6668-3^Pathogenic^LN||||||F
OBX|5|TX|51967-8^GENETIC ANALYSIS SUMMARY^LN||A pathogenic variant in BRCA1 (c.5266dupC) was identified. This variant is associated with significantly increased lifetime risk for breast cancer (60-80%) and ovarian cancer (20-40%). Genetic counseling is strongly recommended.||||||F
OBX|6|CWE|LA14020-4^TESTING METHOD^LN||NGS^Next Generation Sequencing^LN||||||F|||20240801140000||GENETICS_LAB^GENETICS_LAB^L";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.7"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00256"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("TAYLOR"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("AMANDA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("81211"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("BRCA1 FULL GENE ANALYSIS"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("48018-6"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_257_Should_parse_ORU_R01_structured_numeric()
        {
            const string message =
                @"MSH|^~\&|LAB|MAIN_HOSP|EMR|MAIN_HOSP|20240601100000||ORU^R01|MSG00257|P|2.5.1|||AL|NE
PID|1||MRN223366^^^MAIN_HOSP^MR||COOPER^JESSICA^N||19900228|F
ORC|RE|ORD-SN001|LAB-SN001||CM
OBR|1|ORD-SN001|LAB-SN001|SNTESTS^SN DATATYPE TESTS^LOCAL|||20240601090000|||||||||DR1100^CHEN^WEI||||||20240601100000|||F
OBX|1|SN|COMP1^GREATER THAN VALUE^LOCAL||>^100|mg/dL|||||F
OBX|2|SN|COMP2^LESS THAN VALUE^LOCAL||<^0.5|ng/mL|||||F
OBX|3|SN|COMP3^LESS OR EQUAL^LOCAL||<=^10|mIU/mL|||||F
OBX|4|SN|COMP4^GREATER OR EQUAL^LOCAL||>=^200|mg/dL|||||F
OBX|5|SN|RANGE1^RANGE VALUE^LOCAL||^10^-^20|mmol/L|||||F
OBX|6|SN|RATIO1^RATIO VALUE^LOCAL||^1^:^128||||||F
OBX|7|SN|TITER1^TITER VALUE^LOCAL||^1^:^256||||||F
OBX|8|SN|PLAIN1^PLAIN NUMERIC IN SN^LOCAL||^42.5|mg/dL|||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("COOPER"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JESSICA"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("SNTESTS"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_258_Should_parse_ADT_A31_update_person()
        {
            const string message =
                @"MSH|^~\&|REG|MAIN_HOSP|MPI|MAIN_HOSP|20240601140000||ADT^A31|MSG00258|P|2.5|||AL|NE
EVN|A31|20240601140000
PID|1||MRN112288^^^MAIN_HOSP^MR||LEE^CHRISTOPHER^J||19830717|M||2106-3^White^HL70005|999 NEW RESIDENCE DR^^AUSTIN^TX^78702||5125559900~5125559901|||M|||SSN334-55-6677
NK1|1|LEE^SARAH^M|SPO^Spouse^HL70063|999 NEW RESIDENCE DR^^AUSTIN^TX^78702|5125559902||EC^Emergency Contact
GT1|1||LEE^CHRISTOPHER^J||999 NEW RESIDENCE DR^^AUSTIN^TX^78702|5125559900||19830717|M||FT^Full Time|SSN334-55-6677|||TECH CORP|5000 INNOVATION BLVD^^AUSTIN^TX^78759|5125550000";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A31"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A31"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("LEE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("CHRISTOPHER"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }
        }

        [Test]
        public void HL7_SAMPLE_259_Should_parse_ADT_A17_swap_patients()
        {
            const string message =
                @"MSH|^~\&|ADT|MAIN_HOSP|BED_MGMT|MAIN_HOSP|20240315140000||ADT^A17|MSG00259|P|2.4|||AL|NE
EVN|A17|20240315140000
PID|1||MRN111100^^^MAIN_HOSP^MR||JOHNSON^MARY^A||19700101|F
PV1|1|I|MED^301^A||||DR2233^BROWN^ELIZABETH
PID|2||MRN222200^^^MAIN_HOSP^MR||SMITH^ROBERT^B||19650515|M
PV1|2|I|MED^301^B||||DR4455^TAYLOR^JAMES";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A17"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A17"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("JOHNSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MARY"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_260_Should_parse_ADT_A60_adverse_reaction()
        {
            const string message =
                @"MSH|^~\&|ALLERGY_SYS|MAIN_HOSP|EMR|MAIN_HOSP|20240701100000||ADT^A60|MSG00260|P|2.5|||AL|NE
EVN|A60|20240701100000
PID|1||MRN443322^^^MAIN_HOSP^MR||WILSON^SANDRA^K||19750820|F|||456 ELM ST^^PORTLAND^OR^97201||5035557788
IAM|1|DA^Drug Allergy^HL70127|70618^PENICILLIN^RXNORM|SV^Severe^HL70128|39579001^Anaphylaxis^SCT|||20100315|||||DR2233^PATEL^AMIT||20240701
IAM|2|DA^Drug Allergy^HL70127|363523^SULFA DRUGS^RXNORM|MO^Moderate^HL70128|271807003^Skin rash^SCT|||20150622|||||DR2233^PATEL^AMIT||20240701
IAM|3|MA^Miscellaneous Allergy^HL70127|111088007^LATEX^SCT|MI^Mild^HL70128|40275004^Contact dermatitis^SCT|||20180901|||||DR2233^PATEL^AMIT||20240701";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A60"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A60"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("WILSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("SANDRA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_261_Should_parse_RSP_K23_query_response()
        {
            const string message =
                @"MSH|^~\&|MPI|REGIONAL|CPOE|HOSP_A|20240501120000||RSP^K23^RSP_K23|MSG00261|P|2.5|||AL|NE
MSA|AA|QRY-2024-001
QAK|QRY-2024-001|OK||3^3
QPD|Q23^GET CORRESPONDING IDS^HL7|QRY-2024-001|MRN-A-12345^^^HOSP_A^MR
PID|||MRN-A-12345^^^HOSP_A^MR~MRN-B-67890^^^HOSP_B^MR~MRN-C-11111^^^HOSP_C^MR||MARTINEZ^ELENA^R||19850612|F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("RSP"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("K23"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AA"));
                Assert.That(msaResult.Result.MessageControlId.Value, Is.EqualTo("QRY-2024-001"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("MARTINEZ"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("ELENA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_262_Should_parse_QBP_Q23_query()
        {
            const string message =
                @"MSH|^~\&|CPOE|HOSP_A|MPI|REGIONAL|20240501115900||QBP^Q23^QBP_Q21|MSG00262|P|2.5|||AL|NE
QPD|Q23^GET CORRESPONDING IDS^HL7|QRY-2024-001|MRN-A-12345^^^HOSP_A^MR
RCP|I|10^RD|R";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("QBP"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("Q23"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00262"));
        }

        [Test]
        public void HL7_SAMPLE_263_Should_parse_ORU_R01_COVID19_panel()
        {
            const string message =
                @"MSH|^~\&|MOLEC_LAB|MAIN_HOSP|PH_REPORTING|STATE_DOH|20240115100000||ORU^R01|MSG00263|P|2.5|||AL|NE
PID|1||MRN667799^^^MAIN_HOSP^MR||CHEN^WILLIAM^H||19650412|M||2028-9^Asian^HL70005|123 LANTERN ST^^SAN FRANCISCO^CA^94102||4155553344
ORC|RE|ORD-COV-001|LAB-COV-001||CM
OBR|1|ORD-COV-001|LAB-COV-001|94531-1^SARS-COV-2 RNA PANEL^LOINC|||20240115083000|||||||||DR5566^WONG^LISA||||||20240115100000|||F
OBX|1|CWE|94500-6^SARS-COV-2 RNA QUALITATIVE^LOINC||260373001^Detected^SCT||Not Detected|A|||F|||20240115100000
OBX|2|NM|94511-3^SARS-COV-2 CT VALUE^LOINC||18.5||||||F
OBX|3|CWE|94558-4^SARS-COV-2 AG QUALITATIVE^LOINC||260373001^Detected^SCT||Not Detected||||F
OBX|4|CWE|31208-2^SPECIMEN SOURCE^LOINC||258500001^Nasopharyngeal swab^SCT||||||F
SPM|1|SPEC-COV-001||258500001^Nasopharyngeal swab^SCT|||||||||||||20240115083000|20240115084000";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("CHEN"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("WILLIAM"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("94531-1"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("SARS-COV-2 RNA PANEL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("94500-6"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_264_Should_parse_ORU_R01_ABG_critical_values()
        {
            const string message =
                @"MSH|^~\&|ABG_LAB|ICU|EMR|MAIN_HOSP|20240222063000||ORU^R01|MSG00264|P|2.5|||AL|NE
PID|1||MRN998811^^^MAIN_HOSP^MR||BROWN^CATHERINE^A||19580301|F
PV1|1|I|ICU^012^A||||DR7788^PATEL^RAVI
ORC|RE|ORD-ABG-001|LAB-ABG-001||CM
OBR|1|ORD-ABG-001|LAB-ABG-001|24336-0^BLOOD GAS PANEL ARTERIAL^LOINC|||20240222062500|||||||||DR7788^PATEL^RAVI||||||20240222063000|||F
OBX|1|NM|2744-1^PH ARTERIAL^LOINC||7.28||7.35-7.45|LL|||F
OBX|2|NM|2019-8^PCO2 ARTERIAL^LOINC||58|mmHg|35-45|HH|||F
OBX|3|NM|2703-7^PO2 ARTERIAL^LOINC||55|mmHg|80-100|LL|||F
OBX|4|NM|1959-6^HCO3 ARTERIAL^LOINC||26|mEq/L|22-28|N|||F
OBX|5|NM|2708-6^O2 SATURATION ARTERIAL^LOINC||85|%|95-100|L|||F
OBX|6|NM|11555-0^BASE EXCESS ARTERIAL^LOINC||-2|mEq/L|-2 to +2|N|||F
NTE|1||CRITICAL VALUE: pH 7.28, pO2 55. Notified Dr. Patel at 0631 by RN Smith.";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("BROWN"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("CATHERINE"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("24336-0"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("BLOOD GAS PANEL ARTERIAL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("2744-1"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Does.Contain("CRITICAL VALUE"));
            }
        }

        [Test]
        public void HL7_SAMPLE_265_Should_parse_SIU_S13_reschedule()
        {
            const string message =
                @"MSH|^~\&|SCHEDULING|MAIN_HOSP|RAD_RIS|RAD_DEPT|20240225150000||SIU^S13|MSG00265|P|2.5|||AL|NE
SCH|APT-MRI-001|APT-MRI-001|||RESCHEDULED|ROUTINE|76881^MRI KNEE^CPT||60|MIN|1^^^20240315100000^^60^MIN|||||^EQUIPMENT MAINTENANCE||||DR3344^ORTEGA^DANIEL|||||RESCHEDULED
PID|1||MRN445577^^^MAIN_HOSP^MR||DAVIS^RACHEL^E||19950818|F|||234 PINE AVE^^SEATTLE^WA^98101||2065559988
PV1|1|O|RAD^MRI^01||||DR3344^ORTEGA^DANIEL||||ORTHO
RGS|1|A
AIS|1||76881^MRI KNEE^CPT|20240315100000|0|MIN|60|MIN
AIG|1||MRI-MACHINE-03^MRI UNIT 3^LOCAL|MRI EQUIPMENT||20240315100000|0|MIN|60|MIN
AIP|1||DR3344^ORTEGA^DANIEL|ORTHOPEDIC SURGEON||20240315100000|0|MIN|60|MIN";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("SIU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("S13"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DAVIS"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("RACHEL"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }
        }

        [Test]
        public void HL7_SAMPLE_266_Should_parse_MFN_M05_patient_location()
        {
            const string message =
                @"MSH|^~\&|FACILITY_MGMT|MAIN_HOSP|ADT|MAIN_HOSP|20240101080000||MFN^M05|MSG00266|P|2.5|||AL|NE
MFI|LOC^Patient Location^HL7|MAIN_HOSP|UPD|||NE
MFE|MAD||20240101|MED-301-A
LOC|MED^301^A^MAIN_HOSP|MEDICAL SURGICAL 301A|N^Nursing Unit^HL70260||MED-3RD-FLOOR|A^Active
LCH|MED^301^A|I^Inpatient^HL70206
LDP|MED^301^A|MED^Medical^HL70264||A^Active|20240101||GEN^General Medical^LOCAL
MFE|MAD||20240101|ICU-005-A
LOC|ICU^005^A^MAIN_HOSP|ICU BED 5A|N^Nursing Unit^HL70260||ICU-UNIT|A^Active
LCH|ICU^005^A|I^Inpatient^HL70206
LDP|ICU^005^A|ICU^ICU^HL70264||A^Active|20240101||MON^Monitored Bed^LOCAL";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("MFN"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("M05"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00266"));
        }

        [Test]
        public void HL7_SAMPLE_267_Should_parse_MFN_M08_test_definition()
        {
            const string message =
                @"MSH|^~\&|LAB_MGMT|MAIN_HOSP|LIS|MAIN_HOSP|20240201100000||MFN^M08|MSG00267|P|2.5|||AL|NE
MFI|OMA^Numeric Test/Observation^HL7|MAIN_HOSP|UPD|||NE
MFE|MAD||20240201|HBA1C
OM1|1|4548-4^HEMOGLOBIN A1C^LOINC|HBA1C|NM|Y||||CHEMISTRY||%|||||HPLC^High-Performance Liquid Chromatography|F|
OM2|1||%|4.0^7.0|3.0^15.0||<5.7^Normal~5.7-6.4^Pre-diabetes~>=6.5^Diabetes
MFE|MAD||20240201|CREAT
OM1|2|2160-0^CREATININE^LOINC|CREAT|NM|Y||||CHEMISTRY||mg/dL|||||ENZYMATIC|F|
OM2|2||mg/dL|0.5^1.5|0.2^15.0||0.7-1.3^Adult Male~0.6-1.1^Adult Female";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("MFN"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("M08"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00267"));
        }

        [Test]
        public void HL7_SAMPLE_268_Should_parse_RGV_O15_pharmacy_give()
        {
            const string message =
                @"MSH|^~\&|NURSING|ICU|PHARMACY|MAIN_HOSP|20240310143000||RGV^O15|MSG00268|P|2.5|||AL|NE
PID|1||MRN667733^^^MAIN_HOSP^MR||WILLIAMS^ROBERT^J||19550420|M
PV1|1|I|ICU^008^A||||DR4455^JONES^MICHAEL
ORC|RE|ORD-RX-001|ORD-RX-001||IP
RXG|1|1|20240310143000||0409-6509-01^VANCOMYCIN 1G IV^NDC|1000|mg||A^Admin^HL70167||||||RN5566^NURSE^JANE^M
RXR|IV^Intravenous^HL70162|LH^Left Hand^HL70163
RXC|B^Base^HL70166|0409-7983-09^NS 0.9% 250ML^NDC|250|mL
RXC|A^Additive^HL70166|0409-6509-01^VANCOMYCIN 1G^NDC|1000|mg";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("RGV"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O15"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("WILLIAMS"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("ROBERT"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
            }
        }

        [Test]
        public void HL7_SAMPLE_269_Should_parse_ORU_R01_trailing_delimiters()
        {
            const string message =
                @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20240101120000||ORU^R01|MSG00269|P|2.5||||||||||
PID|1||MRN999||DOE^JANE|||F|||||||||||||||||||||
PV1|1|O|||||||||||||||||||||||||||||||||||||||||||||
ORC|RE|ORD001||||||||||||||||||
OBR|1|ORD001||CBC|||20240101||||||||||||||||||||||||||||||
OBX|1|NM|WBC||7.5|10*3/uL|4.5-11.0|N|||F||||||||
OBX|2|NM|HGB||14.2|g/dL|12.0-16.0|N|||F||||||||";

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
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JANE"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_270_Should_parse_ORU_R01_non_standard_segment_order()
        {
            const string message =
                @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20240301090000||ORU^R01|MSG00270|P|2.5
PV1|1|O|LAB^DRAW
PID|1||MRN445566||SMITH^JOHN^T||19701225|M
ZLB|1|LAB-CUSTOM-001|SPECIAL HANDLING REQUIRED
ORC|RE|ORD001
OBR|1|ORD001||GLUCOSE|||20240301080000|||||||||||||||20240301090000|||F
NTE|1|L|Patient was fasting for 12 hours prior to specimen collection.
OBX|1|NM|2345-7^GLUCOSE^LOINC||98|mg/dL|74-106|N|||F
NTE|1|L|Result verified by supervisor due to instrument flag.";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("SMITH"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JOHN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("2345-7"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("GLUCOSE"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Does.Contain("fasting"));
            }
        }

        [Test]
        public void HL7_SAMPLE_271_Should_parse_ORU_R01_long_narrative_text()
        {
            const string message =
                @"MSH|^~\&|RAD|HOSP|EMR|HOSP|20240415140000||ORU^R01|MSG00271|P|2.5
PID|1||MRN778899||GARCIA^ELENA^M||19800505|F
ORC|RE|ORD-RAD-001
OBR|1|ORD-RAD-001||71260^CT CHEST WITH CONTRAST^CPT|||20240415120000|||||||||||||||20240415140000|||F
OBX|1|TX|71260^CT CHEST REPORT^CPT||CLINICAL HISTORY: 44-year-old female with persistent cough and weight loss. Rule out malignancy.\.br\\.br\TECHNIQUE: CT of the chest was performed with intravenous contrast administration. Axial images were obtained from the thoracic inlet through the adrenal glands with coronal and sagittal reformations.\.br\\.br\COMPARISON: Chest X-ray dated 04/01/2024.\.br\\.br\FINDINGS:\.br\\.br\LUNGS AND AIRWAYS: There is a 2.3 cm spiculated nodule in the right upper lobe (series 3, image 45). A smaller 0.8 cm ground-glass nodule is noted in the left lower lobe (series 3, image 112). No endobronchial lesion is identified. The airways are patent to the segmental level bilaterally. No pleural effusion. No pneumothorax.\.br\\.br\MEDIASTINUM AND HILA: A 1.5 cm pretracheal lymph node is identified (series 3, image 22). Right hilar lymphadenopathy measuring 1.8 cm is noted. No pericardial effusion. The heart size is normal.\.br\\.br\CHEST WALL AND BONES: No suspicious osseous lesion. Mild degenerative changes of the thoracic spine.\.br\\.br\UPPER ABDOMEN: Limited evaluation shows no focal hepatic lesion. Adrenal glands are normal.\.br\\.br\IMPRESSION:\.br\1. 2.3 cm spiculated right upper lobe nodule suspicious for primary lung malignancy. Recommend PET/CT for further evaluation.\.br\2. 0.8 cm left lower lobe ground-glass nodule - recommend follow-up CT in 3 months.\.br\3. Pretracheal and right hilar lymphadenopathy concerning for nodal metastatic disease.\.br\\.br\Findings discussed with Dr. Garcia by telephone at 2:15 PM on 04/15/2024.||||||F";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("GARCIA"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("ELENA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("71260"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CT CHEST WITH CONTRAST"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("71260"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_272_Should_parse_ORU_R01_null_and_error_values()
        {
            const string message =
                @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20240501100000||ORU^R01|MSG00272|P|2.5
PID|1||MRN112233||BROWN^PATRICIA||19750815|F
ORC|RE|ORD-NULL-001
OBR|1|ORD-NULL-001||24323-8^CMP^LOINC|||20240501090000|||||||||||||||20240501100000|||F
OBX|1|NM|2345-7^GLUCOSE^LOINC||98|mg/dL|74-106|N|||F
OBX|2|NM|2823-3^POTASSIUM^LOINC||""""""|mmol/L|3.5-5.1||||X|||20240501100000
NTE|1||Specimen hemolyzed. Potassium result unreliable - specimen rejected.
OBX|3|NM|2951-2^SODIUM^LOINC||141|mmol/L|136-145|N|||F
OBX|4|IS|HEMOLYSIS^HEMOLYSIS INDEX^LOCAL||2+^Moderate^HL70920||||||F";

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
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("BROWN"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("PATRICIA"));
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
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Does.Contain("hemolyzed"));
            }
        }

        [Test]
        public void HL7_SAMPLE_273_Should_parse_ACK_application_error()
        {
            const string message =
                @"MSH|^~\&|EMR|HOSP_B|ADT|HOSP_A|20240601120100||ACK^A01|ACK-MSG001|P|2.5|||AL|NE
MSA|AE|MSG-ADT-001||207^Application internal error^HL70357
ERR|PID^1^3^^||101^Required field missing^HL70357|E||||Patient Identifier List (PID-3) is required but was empty
ERR|PID^1^7^^||102^Data type error^HL70357|E||||Date of Birth (PID-7) contains invalid date format: '19801332'. Day value exceeds month limit.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ACK"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ACK-MSG001"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AE"));
                Assert.That(msaResult.Result.MessageControlId.Value, Is.EqualTo("MSG-ADT-001"));
            }
        }

        [Test]
        public void HL7_SAMPLE_274_Should_parse_ACK_application_reject()
        {
            const string message =
                @"MSH|^~\&|PHARMACY|HOSP|CPOE|HOSP|20240601120200||ACK^O01|ACK-MSG002|P|2.5|||AL|NE
MSA|AR|MSG-ORM-001||200^Unsupported message type^HL70357
ERR|||200^Unsupported message type^HL70357|E||||Message type ORM^O01 is not supported by this application. Supported types: RDE^O11, RDS^O13.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ACK"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ACK-MSG002"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AR"));
                Assert.That(msaResult.Result.MessageControlId.Value, Is.EqualTo("MSG-ORM-001"));
            }
        }

        [Test]
        public void HL7_SAMPLE_275_Should_parse_ORU_R01_fully_populated_MSH()
        {
            const string message =
                @"MSH|^~\&|LAB_SYS|MAIN_HOSP|EMR_SYS|MAIN_HOSP|200301011200|SECURE-TOKEN-XYZ|ORU^R01|MSG00275|P|2.3|12345|CONT-PTR-001|AL|AL|USA|ASCII~8859/1|EN^English^ISO639|ISO 2022-1994
PID|1||MRN-FULL-001^^^MAIN_HOSP^MR||FULLMSH^TEST^PATIENT||19700101|M
ORC|RE|ORD-FULL-001
OBR|1|ORD-FULL-001||CBC|||200301011100|||||||||||||||200301011200|||F
OBX|1|NM|WBC||8.0|10*3/uL|4.5-11.0|N|||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00275"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("LAB_SYS"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("MAIN_HOSP"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("FULLMSH"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("TEST"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }
    }
}
