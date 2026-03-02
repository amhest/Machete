namespace Machete.HL7.Tests.Corpus
{
    using NUnit.Framework;
    using Machete.HL7.Testing;
    using Machete.HL7Schema.V26;

    [TestFixture]
    public class CorpusTests_Batch06 :
        HL7MacheteTestHarness<MSH, HL7V26Entity>
    {
        [Test]
        public void HL7_SAMPLE_126_Should_parse_ORU_microbiology_culture_sensitivity()
        {
            const string message = @"MSH|^~\&|MICRO_LAB|HOSP|EMR|HOSP|20210316140000||ORU^R01|ORU_MICRO_001|P|2.5
PID|1||MRN445566^^^HOSP^MR||JONES^WILLIAM^R||19551020|M
ORC|RE|ORD_MICRO_001^CPOE|MCR001^MICRO_LAB
OBR|1|ORD_MICRO_001^CPOE|MCR001^MICRO_LAB|87070^CULTURE, BACTERIAL, ANY SOURCE^CPT|||20210316120000|||||||||45678^WILLIAMS^DAVID^^^MD||||||20210316140000||MB|F
OBX|1|CE|11475-1^MICROORGANISM IDENTIFIED^LN|1|3092008^Staphylococcus aureus^SCT||||||F
OBX|2|ST|18900-1^OXACILLIN SUSCEPTIBILITY^LN|1|R^Resistant||||||F
OBX|3|NM|18900-1^OXACILLIN MIC^LN|1|>2|ug/mL|<=2||||F
OBX|4|ST|18996-9^VANCOMYCIN SUSCEPTIBILITY^LN|1|S^Sensitive||||||F
OBX|5|NM|18996-9^VANCOMYCIN MIC^LN|1|1|ug/mL|<=2||||F
OBX|6|ST|18878-9^CLINDAMYCIN SUSCEPTIBILITY^LN|1|S^Sensitive||||||F
OBX|7|ST|18998-5^TRIMETHOPRIM-SULFAMETHOXAZOLE SUSCEPTIBILITY^LN|1|S^Sensitive||||||F
NTE|1||MRSA detected. Contact precautions recommended.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("MICRO_LAB"));

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
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("87070"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CULTURE, BACTERIAL, ANY SOURCE"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("11475-1"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Is.EqualTo("MRSA detected. Contact precautions recommended."));
            }
        }

        [Test]
        public void HL7_SAMPLE_127_Should_parse_ORU_urinalysis_mixed_datatypes()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210317080000||ORU^R01|ORU_UA_001|P|2.5
PID|1||MRN778899^^^HOSP^MR||BROWN^LISA^A||19901115|F
ORC|RE|ORD_UA_001^CPOE|UA001^LAB
OBR|1|ORD_UA_001^CPOE|UA001^LAB|81001^URINALYSIS^CPT|||20210317070000|||||||||34567^PATEL^PRIYA^^^MD||||||20210317080000||LAB|F
OBX|1|ST|5778-6^Color^LN||Yellow||||||F
OBX|2|ST|5767-9^Clarity^LN||Clear||||||F
OBX|3|NM|5811-5^Specific Gravity^LN||1.025||1.005-1.030||||F
OBX|4|NM|5803-2^pH^LN||6.0||5.0-8.0||||F
OBX|5|ST|5804-0^Protein^LN||Negative||Negative||||F
OBX|6|ST|5792-7^Glucose^LN||Negative||Negative||||F
OBX|7|ST|5797-6^Ketones^LN||Negative||Negative||||F
OBX|8|ST|5794-3^Blood^LN||Trace||Negative|A|||F
OBX|9|NM|5821-4^WBC^LN||1|/hpf|0-5||||F
OBX|10|ST|5769-5^Bacteria^LN||Few||None||||F";

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
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("LISA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("81001"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("URINALYSIS"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("5778-6"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_128_Should_parse_ORU_ecg_waveform_results()
        {
            const string message = @"MSH|^~\&|EKG_SYS|CARDIOLOGY|EMR|HOSP|20210318090000||ORU^R01|ORU_ECG_001|P|2.5
PID|1||MRN112233^^^HOSP^MR||WILSON^MICHAEL^J||19700520|M
ORC|RE|ORD_ECG_001^CPOE|ECG001^EKG_SYS
OBR|1|ORD_ECG_001^CPOE|ECG001^EKG_SYS|93000^ELECTROCARDIOGRAM^CPT|||20210318085000|||||||||78901^CHANG^ANDREW^^^MD||||||20210318090000||CARD|F
OBX|1|NM|8867-4^Heart rate^LN||72|{beats}/min|60-100||||F
OBX|2|NM|8625-6^P-R interval^LN||160|ms|120-200||||F
OBX|3|NM|8633-0^QRS duration^LN||90|ms|70-120||||F
OBX|4|NM|8634-8^Q-T interval^LN||380|ms|||||F
OBX|5|NM|8636-3^Q-T interval corrected^LN||416|ms|<=440||||F
OBX|6|TX|18844-1^ECG Impression^LN||Normal sinus rhythm. Normal ECG.||||||F
OBX|7|RP|WAVEFORM^ECG Waveform||ECG20210318085000.dcm^DICOM^Image||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("EKG_SYS"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("CARDIOLOGY"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("WILSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MICHAEL"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("93000"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("ELECTROCARDIOGRAM"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("8867-4"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("Heart rate"));
            }
        }

        [Test]
        public void HL7_SAMPLE_129_Should_parse_ADT_A04_emergency_registration()
        {
            const string message = @"MSH|^~\&|EDIS|ER_DEPT|ADT|HOSP|20210319200000||ADT^A04^ADT_A01|ADT_ER_001|P|2.5
EVN|A04|20210319200000
PID|1||MRN334455^^^HOSP^MR||MARTINEZ^CARLOS^E||19680714|M|||234 MAPLE DR^^ANYTOWN^CA^90210||^PRN^PH^^1^310^5551111
NK1|1|MARTINEZ^ANA^M|SPO^SPOUSE^HL70063|234 MAPLE DR^^ANYTOWN^CA^90210|^PRN^PH^^1^310^5552222
PV1|1|E|ER^TRIAGE^1^ER_DEPT|U|||89012^SMITH^JENNIFER^^^MD|||||||||89012^SMITH^JENNIFER^^^MD|EP^|VN556677|||||||||||||||||3^URGENT|7^EMERGENCY ROOM||||20210319200000
DG1|1||R07.9^Chest pain, unspecified^I10|Chest pain|20210319|A
DG1|2||I10^Essential (primary) hypertension^I10|Hypertension|20210319|A
IN1|1|BXBS|INS001|BLUE CROSS BLUE SHIELD|PO BOX 1234^^ANYTOWN^CA^90210||^WPN^PH^^1^800^5551234|GRP12345||||||PPO||||MARTINEZ^CARLOS^E|1|19680714||||||||||||||||INS123456";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A04"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ADT_A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("EDIS"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A04"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("MARTINEZ"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("CARLOS"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("E"));
            }

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
            {
                Assert.That(dg1Result.Result.DiagnosiCode.Value.Identifier.Value, Is.Not.Null.Or.Empty);
            }
        }

        [Test]
        public void HL7_SAMPLE_130_Should_parse_ORU_pathology_report()
        {
            const string message = @"MSH|^~\&|PATH_SYS|PATHOLOGY|EMR|HOSP|20210320110000||ORU^R01|ORU_PATH_001|P|2.3
PID|1||MRN556677^^^HOSP^MR||ANDERSON^SUSAN^K||19580303|F
ORC|RE|ORD_PATH_001^SURG|SP21-001^PATH_SYS
OBR|1|ORD_PATH_001^SURG|SP21-001^PATH_SYS|88305^SURGICAL PATHOLOGY^CPT|||20210319140000|||||||||67890^LEE^DAVID^^^MD||||||20210320110000||SP|F
OBX|1|TX|22634-0^PATHOLOGY REPORT^LN|1|SPECIMEN: Right breast, excisional biopsy||||||F
OBX|2|TX|22634-0^GROSS DESCRIPTION^LN|2|Received fresh, a 3.2 x 2.8 x 1.5 cm irregular tan-white tissue fragment. Serial sections reveal a firm 1.2 cm tan-white nodule. Entirely submitted in 4 cassettes.||||||F
OBX|3|TX|22634-0^MICROSCOPIC DESCRIPTION^LN|3|Sections show fibrous breast tissue with a well-circumscribed nodule composed of bland spindle cells in a collagenous stroma. No atypia or mitotic figures identified.||||||F
OBX|4|TX|22634-0^DIAGNOSIS^LN|4|RIGHT BREAST, EXCISIONAL BIOPSY: FIBROADENOMA. No evidence of malignancy.||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("PATH_SYS"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("PATHOLOGY"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("ANDERSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("SUSAN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("88305"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("SURGICAL PATHOLOGY"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("22634-0"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_131_Should_parse_ADT_A01_special_characters_in_name()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A01|ADT_SPEC001|P|2.5
EVN|A01|20210401120000
PID|1||MRN889900^^^HOSP^MR||O'BRIEN-SMITH^MARY^CATHERINE^JR.^MRS.^PHD||19750815|F
PV1|1|I|MED^201^A||||12345^MULLER^HANS^^^DR";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ADT_SPEC001"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A01"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("O'BRIEN-SMITH"));
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
        public void HL7_SAMPLE_132_Should_parse_ORU_prenatal_panel()
        {
            const string message = @"MSH|^~\&|LAB|OB_CLINIC|EMR|OB_CLINIC|20210501090000||ORU^R01|ORU_PRENATAL_001|P|2.5
PID|1||MRN111222^^^CLINIC^MR||WASHINGTON^JESSICA^T||19920310|F
ORC|RE|ORD_PRE_001^OB||||||||||23456^BAKER^SARAH^^^MD
OBR|1|ORD_PRE_001^OB||PRENATAL^PRENATAL PANEL^L|||20210501080000|||||||||23456^BAKER^SARAH^^^MD||||||20210501090000||OB|F
OBX|1|CE|882-1^ABO+Rh Group^LN||278149003^O Positive^SCT||||||F
OBX|2|CE|890-4^Rh Antibody Screen^LN||260385009^Negative^SCT||||||F
OBX|3|CE|5334-8^Rubella IgG^LN||10828004^Immune^SCT||||||F
OBX|4|CE|5196-1^Hepatitis B Surface Antigen^LN||260385009^Negative^SCT||Negative||||F
OBX|5|CE|7918-6^HIV 1+2 Screen^LN||260385009^Negative^SCT||Negative||||F
OBX|6|CE|72607-4^Group B Streptococcus^LN||10828004^Positive^SCT||Negative|A|||F
NTE|1||GBS positive. Recommend intrapartum antibiotic prophylaxis.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("LAB"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("OB_CLINIC"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("WASHINGTON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JESSICA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("PRENATAL"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("PRENATAL PANEL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("882-1"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("ABO+Rh Group"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Is.EqualTo("GBS positive. Recommend intrapartum antibiotic prophylaxis."));
            }
        }

        [Test]
        public void HL7_SAMPLE_133_Should_parse_ADT_A01_date_only_timestamp()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401||ADT^A01|MSG_DATE001|P|2.3
PID|1||123456||DOE^JOHN||19800101|M
PV1|1|I|MED^101^A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_DATE001"));

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
        public void HL7_SAMPLE_134_Should_parse_ADT_A01_timezone_offset()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000+0530||ADT^A01|MSG_TZ001|P|2.5
PID|1||123456||PATEL^RAJESH||19800101|M
PV1|1|I|MED^101^A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_TZ001"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("PATEL"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("RAJESH"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_135_Should_parse_ADT_A01_max_precision_timestamp()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000.1234-0500||ADT^A01|MSG_PREC001|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
PV1|1|I|MED^101^A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_PREC001"));

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
        public void HL7_SAMPLE_136_Should_parse_ACK_A01_typed_acknowledgment()
        {
            const string message = @"MSH|^~\&|EMR|HOSP|ADT|HOSP|20210401120100||ACK^A01|ACK_MSG001|P|2.5
MSA|AA|ADT_SPEC001|Message accepted";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ACK"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ACK_MSG001"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AA"));
                Assert.That(msaResult.Result.MessageControlId.Value, Is.EqualTo("ADT_SPEC001"));
            }
        }

        [Test]
        public void HL7_SAMPLE_137_Should_parse_ACK_application_error()
        {
            const string message = @"MSH|^~\&|EMR|HOSP|ADT|HOSP|20210401120200||ACK|ACK_MSG002|P|2.5
MSA|AE|ADT_BAD001|Required field missing
ERR||PID^1^3|101^Required field missing^HL70357|E|||||Missing patient identifier in PID-3";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ACK"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ACK_MSG002"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AE"));
                Assert.That(msaResult.Result.MessageControlId.Value, Is.EqualTo("ADT_BAD001"));
            }
        }

        [Test]
        public void HL7_SAMPLE_138_Should_parse_ACK_application_reject()
        {
            const string message = @"MSH|^~\&|EMR|HOSP|ADT|HOSP|20210401120300||ACK|ACK_MSG003|P|2.5
MSA|AR|MSG_UNSUPPORTED|Unsupported message type
ERR||MSH^1^9|200^Unsupported message type^HL70357|E";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ACK"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ACK_MSG003"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AR"));
                Assert.That(msaResult.Result.MessageControlId.Value, Is.EqualTo("MSG_UNSUPPORTED"));
            }
        }

        [Test]
        public void HL7_SAMPLE_139_Should_parse_ORU_drug_screen_toxicology()
        {
            const string message = @"MSH|^~\&|TOX_LAB|HOSP|EMR|HOSP|20210318150000||ORU^R01|ORU_TOX_001|P|2.3
PID|1||MRN667788^^^HOSP^MR||TAYLOR^JAMES^R||19850920|M
ORC|RE|ORD_TOX_001^ER
OBR|1|ORD_TOX_001^ER||UDSCR^URINE DRUG SCREEN^L|||20210318140000|||||||||89012^SMITH^JENNIFER^^^MD||||||20210318150000||TOX|F
OBX|1|ST|19261-2^AMPHETAMINES SCREEN^LN||NEGATIVE||NEGATIVE||||F
OBX|2|ST|19270-3^BENZODIAZEPINES SCREEN^LN||POSITIVE||NEGATIVE|A|||F
OBX|3|ST|19271-1^CANNABINOIDS SCREEN^LN||NEGATIVE||NEGATIVE||||F
OBX|4|ST|19272-9^COCAINE SCREEN^LN||NEGATIVE||NEGATIVE||||F
OBX|5|ST|19295-0^OPIATES SCREEN^LN||POSITIVE||NEGATIVE|A|||F
OBX|6|ST|19286-9^PCP SCREEN^LN||NEGATIVE||NEGATIVE||||F
NTE|1||Positive results will be confirmed by GC/MS. Results pending.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("TOX_LAB"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("TAYLOR"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JAMES"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("UDSCR"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("URINE DRUG SCREEN"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("19261-2"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("AMPHETAMINES SCREEN"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Is.EqualTo("Positive results will be confirmed by GC/MS. Results pending."));
            }
        }

        [Test]
        public void HL7_SAMPLE_140_Should_parse_ORU_coagulation_panel()
        {
            const string message = @"MSH|^~\&|COAG_LAB|HOSP|EMR|HOSP|20210319100000||ORU^R01|ORU_COAG_001|P|2.5
PID|1||MRN998877^^^HOSP^MR||GARCIA^MARIA^L||19850322|F
ORC|RE|ORD_COAG_001
OBR|1|ORD_COAG_001||85610^COAGULATION PANEL^CPT|||20210319090000|||||||||56789^RODRIGUEZ^CARLOS^^^MD||||||20210319100000||HEM|F
OBX|1|NM|5902-2^Prothrombin time^LN||14.2|sec|11.0-13.5|H|||F
OBX|2|NM|6301-6^INR^LN||1.3||0.9-1.1|H|||F
OBX|3|NM|3173-2^PTT^LN||32|sec|25-35||||F
OBX|4|NM|3255-7^Fibrinogen^LN||280|mg/dL|200-400||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("COAG_LAB"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("GARCIA"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MARIA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("85610"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("COAGULATION PANEL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("5902-2"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("Prothrombin time"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_141_Should_parse_ADT_A01_extra_MSH9_components()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A01^ADT_A01^CUSTOM|MSG_EXTRA001|P|2.3
PID|1||123456||DOE^JOHN||19800101|M
PV1|1|I|MED^101^A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ADT_A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG_EXTRA001"));

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
        public void HL7_SAMPLE_142_Should_parse_ADT_A01_special_chars_in_control_id()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|EMR|HOSP|20210401120000||ADT^A01|MSG.2021-04-01_120000.123|P|2.5
PID|1||123456||DOE^JOHN||19800101|M
PV1|1|I|MED^101^A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG.2021-04-01_120000.123"));

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
        public void HL7_SAMPLE_143_Should_parse_ORU_covid19_antigen_positive()
        {
            const string message = @"MSH|^~\&|POC_DEVICE|CLINIC|PH_DEPT|STATE|20210401100000||ORU^R01^ORU_R01|ORU_COVID_AG_001|P|2.5.1|||NE|NE|USA||||PHLabReport-NoAck^ELR251^2.16.840.1.113883.9.11^ISO
PID|1||PAT999^^^CLINIC^MR||DOE^JANE||19900101|F||2106-3^White^CDCREC|123 MAIN^^ANYTOWN^ST^12345|||||||||||2186-5^Not Hispanic^CDCREC
ORC|RE|ORD_AG001^CLINIC|AGR001^POC
OBR|1|ORD_AG001^CLINIC|AGR001^POC|94558-4^SARS-CoV-2 Ag Respiratory^LN|||20210401095000|||||||||12345^SMITH^JOHN^^^MD||||||20210401100000||LAB|F
OBX|1|CWE|94558-4^SARS-CoV-2 Ag Respiratory^LN|1|260373001^Detected^SCT||Not Detected|A|||F|||20210401
OBX|2|DT|65222-2^Date of symptom onset^LN|2|20210328||||||F
OBX|3|CWE|77974-4^Hospitalized^LN|3|N^No^HL70136||||||F
SPM|1|||258500001^Nasal swab^SCT";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ORU_R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("POC_DEVICE"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("CLINIC"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JANE"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("94558-4"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("SARS-CoV-2 Ag Respiratory"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("94558-4"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_144_Should_parse_ORU_thyroid_function_panel()
        {
            const string message = @"MSH|^~\&|LAB|HOSP|EMR|HOSP|20210322090000||ORU^R01|ORU_THY_001|P|2.5
PID|1||MRN445566^^^HOSP^MR||CHEN^LILY^W||19780215|F
ORC|RE|ORD_THY_001
OBR|1|ORD_THY_001||THYROID^THYROID FUNCTION PANEL^L|||20210322080000|||||||||45678^WILLIAMS^DAVID^^^MD||||||20210322090000||LAB|F
OBX|1|NM|11580-8^TSH^LN||2.5|mIU/L|0.4-4.0||||F
OBX|2|NM|3024-7^Free T4^LN||1.2|ng/dL|0.8-1.8||||F
OBX|3|NM|3051-0^Free T3^LN||3.1|pg/mL|2.3-4.2||||F
OBX|4|NM|3026-2^Total T4^LN||8.0|ug/dL|4.5-12.0||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("ORU_THY_001"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("CHEN"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("LILY"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("THYROID"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("THYROID FUNCTION PANEL"));
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
        public void HL7_SAMPLE_145_Should_parse_DFT_P03_financial_transaction()
        {
            const string message = @"MSH|^~\&|BILLING|HOSP|FIN_SYS|HOSP|20210320120000||DFT^P03|DFT_001|P|2.3
EVN||20210320120000
PID|1||MRN334455^^^HOSP^MR||MARTINEZ^CARLOS^E||19680714|M
PV1|1|E|ER^TRIAGE^1||||89012^SMITH^JENNIFER^^^MD||||||||||VN556677|||||||||||||||||||||||||20210319200000|20210320060000
FT1|1||20210319|20210320|P|99284^ED VISIT LEVEL 4^CPT||||1|475.00||||||||||R07.9^Chest pain^I10
FT1|2||20210319|20210320|P|71046^CHEST XRAY 2 VIEWS^CPT||||1|125.00||||||||||R07.9^Chest pain^I10
DG1|1||R07.9^Chest pain, unspecified^I10|Chest pain|20210319|A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("DFT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("P03"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("BILLING"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("MARTINEZ"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("CARLOS"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("E"));
            }

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
            {
                Assert.That(dg1Result.Result.DiagnosiCode.Value.Identifier.Value, Is.Not.Null.Or.Empty);
            }
        }

        [Test]
        public void HL7_SAMPLE_146_Should_parse_SIU_S14_appointment_modification()
        {
            const string message = @"MSH|^~\&|SCHED|CARDIOLOGY|EMR|HOSP|20210405100000||SIU^S14|SIU_MOD_001|P|2.5
SCH|APT12345|APT12345|||APT12345|FOLLOWUP^Follow-up Visit|Cardiology follow-up|CLINIC|30|min|^^30^20210412140000^20210412143000|||||67890^CHANG^ANDREW^^^MD||||67890^CHANG^ANDREW^^^MD|||||Rescheduled
PID|1||MRN112233^^^HOSP^MR||WILSON^MICHAEL^J||19700520|M
PV1|1|O|CARD^CLINIC^1||||67890^CHANG^ANDREW^^^MD
RGS|1|A
AIS|1||99213^OFFICE VISIT LEVEL 3^CPT|20210412140000|15|min|30|min
AIP|1||67890^CHANG^ANDREW^^^MD|PHYSICIAN||20210412140000|15|min|30|min";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("SIU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("S14"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("SCHED"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("CARDIOLOGY"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("SIU_MOD_001"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("WILSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MICHAEL"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }
        }

        [Test]
        public void HL7_SAMPLE_147_Should_parse_SIU_S15_appointment_cancellation()
        {
            const string message = @"MSH|^~\&|SCHED|CARDIOLOGY|EMR|HOSP|20210406090000||SIU^S15|SIU_CAN_001|P|2.5
SCH|APT12345|APT12345||||CANCELLED^Patient Request|Patient called to cancel|CLINIC|30|min|^^30^20210412140000^20210412143000|||||67890^CHANG^ANDREW^^^MD||||67890^CHANG^ANDREW^^^MD|||||Cancelled
PID|1||MRN112233^^^HOSP^MR||WILSON^MICHAEL^J||19700520|M";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("SIU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("S15"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("SIU_CAN_001"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("SCHED"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("WILSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MICHAEL"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }
        }

        [Test]
        public void HL7_SAMPLE_148_Should_parse_MFN_M14_master_file_code_table()
        {
            const string message = @"MSH|^~\&|HL7REG|UH|HL7LAB|CH|200106290544||MFN^M14^MFN_Z99|MSGID001|P|2.6
MFI|HL70006^RELIGION^HL70175||UPD|||NE
MFE|MAD|||BUD^Buddhism^HL70006
MFE|MAD|||HIN^Hinduism^HL70006
MFE|MAD|||SIK^Sikhism^HL70006";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("MFN"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("M14"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("MFN_Z99"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.6"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSGID001"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("HL7REG"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("UH"));
        }

        [Test]
        public void HL7_SAMPLE_149_Should_parse_MFN_M13_location_master_file()
        {
            const string message = @"MSH|^~\&|HL7REG|UH|HL7LAB|CH|200106290544||MFN^M13^MFN_M01|MSGID004|P|2.6||AL|AL
MFI|LOC^Location Master File^HL70175||UPD|||NE
MFE|MAD|||ICU301
LOC|ICU301^ICU Room 301|ICU^Intensive Care Unit|N^Nursing Unit^HL70260||GENERAL_HOSP^General Hospital|^WPN^PH^^1^555^5551234";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("MFN"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("M13"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("MFN_M01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.6"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSGID004"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("HL7REG"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("UH"));
        }

        [Test]
        public void HL7_SAMPLE_150_Should_parse_ACK_M13_master_file_acknowledgment()
        {
            const string message = @"MSH|^~\&|HL7LAB|CH|HL7REG|UH|200106290545||ACK^M13^ACK|MSGID99004|P|2.5
MSA|AA|MSGID004
MFA|MAD|20010629||S^Successful^HL70179";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ACK"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("M13"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ACK"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSGID99004"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("HL7LAB"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AA"));
                Assert.That(msaResult.Result.MessageControlId.Value, Is.EqualTo("MSGID004"));
            }
        }
    }
}
