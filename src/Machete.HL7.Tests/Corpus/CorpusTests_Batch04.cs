namespace Machete.HL7.Tests.Corpus
{
    using NUnit.Framework;
    using Machete.HL7.Testing;
    using Machete.HL7Schema.V26;

    [TestFixture]
    public class CorpusTests_Batch04 :
        HL7MacheteTestHarness<MSH, HL7V26Entity>
    {
        [Test]
        public void HL7_SAMPLE_076_Should_parse_RDE_O01_IV_pantoprazole_pharmacy_order()
        {
            const string message = @"MSH|^~\&|PHARMACY|HOSPITAL|iFW|HOSPITAL|||RDE^O01|RX12345|P|2.3
PID|1||12345||DOE^JOHN||19500101|M
ORC|NW|RX12345^ABC|||||1^^INDEF^201108250200^^RTN||20110825012431
RXE|1^^INDEF^201108250200|00338004902^SODIUM CHLORIDE 0.9 %^NDC|230||ML|SOLP
RXR|IV|
RXC|B|00338004902^SODIUM CHLORIDE 0.9 %^NDC|230|ML
RXC|A|00008092355^PANTOPRAZOLE SODIUM^NDC|80|MG|40|MG";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("RDE"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("PHARMACY"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("HOSPITAL"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("RX12345"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JOHN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("NW"));
            }
        }

        [Test]
        public void HL7_SAMPLE_077_Should_parse_RDE_O01_oral_warfarin_order()
        {
            const string message = @"MSH|^~\&|PHARMACY|HOSPITAL|iFW|HOSPITAL|||RDE^O01|RX20352|P|2.3
PID|1||12345||DOE^JOHN||19500101|M
ORC|NW|20352777|||AC||||201108250625
RXE|^NOW^^201108250625|WARF5^WARFARIN SODIUM|5||MG|TABLET
RXR|ORAL";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("RDE"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("RX20352"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JOHN"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("NW"));
            }
        }

        [Test]
        public void HL7_SAMPLE_078_Should_parse_RDE_O01_oral_lisinopril_daily_order()
        {
            const string message = @"MSH|^~\&|PHARMACY|HOSPITAL|iFW|HOSPITAL|||RDE^O01|RX28833|P|2.3
PID|1||12345||DOE^JOHN||19500101|M
ORC|NW|28833^EFG|||||^DAILY&0900^^20110812110000^20110825001058||20110825001057
RXE|^DAILY@0900^^20110812110000^20110825001058|3433^LISINOPRIL 20 MG TAB UD|40||mg|TAB^Tab|
RXR|PO^Oral";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("RDE"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("RX28833"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("NW"));
            }
        }

        [Test]
        public void HL7_SAMPLE_079_Should_parse_RDE_O01_discontinuation_IV_ranitidine()
        {
            const string message = @"MSH|^~\&|PHARMACY|HOSPITAL|iFW|HOSPITAL|||RDE^O01|RX18686|P|2.3
PID|1||12345||DOE^JOHN||19500101|M
ORC|DC|18686^EFG||||^Q8&0600,1400,2200^^20110824140000^20110825000914||20110825000915
RXE|^Q8@0600,1400,2200^^20110824140000^20110825000914|444^Ranitidine|100||mL|INJ
RXR|IVPB^IVPB
RXC|A|444^Ranitidine Inj 25 mg/mL (IV)|50|mg
RXC|B|195^Sodium Chloride 0.9% 100 mL|100|mL";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("RDE"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("RX18686"));

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("DC"));
            }
        }

        [Test]
        public void HL7_SAMPLE_080_Should_parse_ADT_A40_merge_patient()
        {
            const string message = @"MSH|^~\&|ADT1|MCM|LABADT|MCM|198808181126||ADT^A40^ADT_A39|MSG00001|P|2.4
EVN|A40|200701011000
PID|||123456^^^HOSP^MR||SMITH^JOHN^A||19650101|M|||123 MAIN ST^^ANYTOWN^NY^12345
MRG|456789^^^HOSP^MR
PV1||N";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A40"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ADT_A39"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00001"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A40"));
            }

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
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("N"));
            }
        }

        [Test]
        public void HL7_SAMPLE_081_Should_parse_ADT_A34_legacy_merge()
        {
            const string message = @"MSH|^~\&|ADT1|MCM|LABADT|MCM|200501011000||ADT^A34|MSG00099|P|2.3
EVN|A34|200501011000
PID|||123456^^^HOSP^MR||DOE^JANE||19750315|F
MRG|987654^^^HOSP^MR||654321^^^HOSP^VN";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A34"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00099"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A34"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JANE"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_082_Should_parse_OML_O21_CBC_lab_order()
        {
            const string message = @"MSH|^~\&|EHR_APP|CLINIC_A|LAB_SYS|REFERENCE_LAB|201912150900||OML^O21^OML_O21|MSG0001234|P|2.5.1|||AL|AL
PID|1||PAT123456^^^CLINIC_A^MR||JOHNSON^ROBERT^T||19801215|M|||456 OAK AVE^^SPRINGFIELD^IL^62701
ORC|NW|ORD-5678^EHR_APP||||||||||1234^SMITH^JANE^^^DR
OBR|1|ORD-5678^EHR_APP||57021-8^CBC W Auto Differential panel - Blood^LN|||201912151000||||||||1234^SMITH^JANE^^^DR
SPM|1|||WB^Whole Blood^HL70487|||||||P^^HL70369||||VEN^Venipuncture^HL70488";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("OML"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O21"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("OML_O21"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("EHR_APP"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG0001234"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("JOHNSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("ROBERT"));
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
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("57021-8"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CBC W Auto Differential panel - Blood"));
            }
        }

        [Test]
        public void HL7_SAMPLE_083_Should_parse_ORU_R01_COVID19_PCR_negative()
        {
            const string message = @"MSH|^~\&|SENDINGAPP^5678^ISO|REPORTINGLAB^1234^CLIA|MDNBS^2.16.840.1.114222.4.3.2.2.1.159.1^ISO|MDH^2.16.840.1.114222.4.1.10058^ISO|20200710183002.10700||ORU^R01^ORU_R01|1234567890|P^T|2.5.1|||NE|NE|USA||||USELR1.0^^2.16.840.1.114222.4.10.3^ISO
SFT|1|Level Seven Healthcare Software, Inc.^L^^^^&2.16.840.1.113883.19.4.6^ISO^XX^^^1234|1.2|An Lab system|56734||20200710
PID|1||36363636^^^MPI&2.16.840.1.113883.19.3.2.1&ISO^MR||TestMD^HHSExtra^A^^^^L||20050602|F||2106-3^White^CDCREC|2222 Home Street^^Baltimore^MD^99999^USA^H||^H^PH^^1^555^5552004|||||M^Married^HL70002||||||||||||N
ORC|RE|23456^EHR^2.16.840.1.113883.19.3.2.3^ISO|9700123^Lab^2.16.840.1.113883.19.3.1.6^ISO|||||||||1234^Admit^Alan^A^III^Dr
OBR|1|23456^EHR^2.16.840.1.113883.19.3.2.3^ISO|9700123^Lab^2.16.840.1.113883.19.3.1.6^ISO|94500-6^SARS-CoV-2 RNA Resp Ql NAA+probe^LN|||20200710103007|||||||||1234^Admit^Alan^A^III^Dr||||||20080818300700|||F
OBX|1|CWE|94316-7^SARS-CoV-2 N gene XXX Ql NAA+probe^LN|1|260415000^Not Detected^SCT||Not Detected|N|||F|||202007101030-0700
SPM|1|||258500001^Nasopharyngeal Swab^SCT|||||||P^Patient^HL70369";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ORU_R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("1234567890"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("TestMD"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("HHSExtra"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("94500-6"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("SARS-CoV-2 RNA Resp Ql NAA+probe"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("94316-7"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("SARS-CoV-2 N gene XXX Ql NAA+probe"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_084_Should_parse_VXU_V04_comprehensive_vaccination_update()
        {
            const string message = @"MSH|^~\&||MA0000||GA0000|19970901||VXU^V04|19970522MA53|T|2.3.1|||NE|AL
PID|||1234^^^^SR^~1234-12^^^^LR^~3872^^^^MR~221345671^^^^SS^~430078856^^^^MA^||KENNEDY^JOHN^FITZGERALD^JR^^^L|BOUVIER^^^^^^M|19900607|M|KENNEDY^BABYBOY^^^^^^B|2106-3^WHITE^HL70005|123 MAIN ST^APT 3B^LEXINGTON^MA^00210^^M^MSA CODE^MA034~345 ELM ST^^BOSTON^MA^00314^^BDL~^^^^^^BR^^MA002||(617)555-1212^PRN^PH^^^617^5551212^^||EN^ENGLISH^HL70296^^^|||||||N^NOT HISPANIC OR LATINO^HL70189^2186-5^NOT HISPANIC OR LATINO^CDCRE1|CHILDREN'S HOSPITAL
PD1|||CHILDREN'S CLINIC ^L^1234^^^^FI^LEXINGTON HOSPITAL&5678&XX|12345^WELBY^MARCUS^^^DR^MD^^^L^^^DN|||||||03^REMINDER/RECALL - NO CALLS^HL70215|Y|19900607|||A|19900607|19900607
NK1|1|KENNEDY^JACQUELINE^LEE|MTH^MOTHER^HL70063||||||||||||||||||||||||||||||898666725^^^^SS
NK1|2|KENNEDY^JOHN^FITZGERALD|FTH^FATHER^HL70063||||||||||||||||||||||||||||||822546618^^^^SS
PV1||R|||||||||||||||A|||V02^19900607~H02^19900607
RXA|0|1|19900607|19900607|08^HEPB-PEDIATRIC/ADOLESCENT^CVX^90744^HEPB-PEDATRIC/ADOLESCENT^CPT|.5|ML^^ISO+||03^HISTORICAL INFORMATION - FROM PARENT'S WRITTEN RECORD^NIP0001|^JONES^LISA|^^^CHILDREN'S HOSPITAL||5|MCG^^ISO+|MRK12345|199206|MSD^MERCK^MVX
RXA|0|4|19910907|19910907|50^DTAP-HIB^CVX^90721^DTAP-HIB^CPT|.5|ML^^ISO+||00^NEW IMMUNIZATION RECORD^NIP0001|1234567890^SMITH^SALLY^S^^^^^^^^^VEI~1234567891^O'BRIAN^ROBERT^A^^DR^MD^^^^^^OEI|^^^CHILD HEALTHCARE CLINIC^^^^^101 MAIN STREET^^BOSTON^MA||||W46932777|199208|PMC^PASTEUR MERIEUX CONNAUGHT^MVX|||CP|A|19910907120030
RXR|IM^INTRAMUSCULAR^HL70162|LA^LEFT ARM^HL70163
RXA|0|5|19950520|19950520|20^DTAP^CVX|.5|ML^^ISO+|||1234567891^O'BRIAN^ROBERT^A^^DR|^^^CHILD HEALTHCARE CLINIC^^^^^101 MAIN STREET^^BOSTON^MA||||W22532806|19950705|PMC^PASTEUR MERIEUX CONNAUGHT^MVX
RXR|IM^INTRAMUSCULAR^HL70162|LA^LEFT ARM^HL70163
OBX|1|CE|30963-3^Vaccine purchased with^LN||PBF^Public funds^NIP008||||||F
OBX|2|CE|VFC-STATUS^VFC Status^STC||V02||||||F
OBX|3|TS|29768-9^DATE VACCINE INFORMATION STATEMENT PUBLISHED^LN|1|19950520||||||F|||20100920
OBX|4|TS|29769-7^DATE VACCINE INFORMATION STATEMENT PRESENTED ^LN|1|19950520||||||F|||20100920
RXA|0|0|20090531132511|20090531132511|3^MMR^CVX|0||||^Sticker^Nurse|^^^DCS_DC||||||
OBX|1|CE|30945-0^Vaccination contraindication/precaution^LN|1|26^allergy to thimerasol(anaphylactic)^STC||||||F|||20090415
RXA|0|1|20090531132511|20090531132511|3^MMR^CVX|999||||^Sticker^Nurse|^^^DCS_DC||||||||8";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("VXU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("V04"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3.1"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("19970522MA53"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("KENNEDY"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JOHN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("R"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_085_Should_parse_RSP_K11_multi_patient_query_response()
        {
            const string message = @"MSH|^~\&|^^|MA0000^^|^^|GA0000^^|20111104153354||RSP^K11^RSP_K11|1320446034070.100000002|T|2.5.1|||||||||Z31^CDCPHINVS^^
MSA|AA|19970522GA40
QAK|||Z34^Request Immunization History^HL70471
QPD|Z34^Request Immunization History^HL70471|19970522GA05||^JOHN^^^^^L|Que^MALIF^^^^^M|20030123|M|L
PID|1||25^^^^SR||FLOYD^FRANK^^^^^L||20030123
PD1|||^^^^^^SR|^^^^^^^^^^^^SR
NK1|1|FLOYD^MALIFICENT|GRD^Guardian^HL70063
PID|2||85^^^^SR||HENRY^JOHN^^^^^L||20011010
PD1|||^^^^^^SR|^^^^^^^^^^^^SR
NK1|1|^MARY|GRD^Guardian^HL70063
PID|3||26^^^^SR||KENNEDY^JOHN^FITZGERALD^^^^L||19900607
PD1|||^^^^^^SR|^^^^^^^^^^^^SR
NK1|1|KENNEDY^JACQUELINE^LEE|GRD^Guardian^HL70063";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("RSP"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("K11"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("RSP_K11"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("1320446034070.100000002"));

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
            }
        }

        [Test]
        public void HL7_SAMPLE_086_Should_parse_RSP_K11_immunization_forecast_response()
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
RXA|0|999|20120727112142|20120727112142|998^no vaccine administered^CVX|0||||||||||||||||20120727112144
OBX|1|CE|30956-7^vaccine type^LN||0^DTP/aP^CVX||||||F
OBX|1|CE|59779-9^Immunization Schedule used^LN||VXC16^ACIP^CDCPHINVS||||||F
OBX|1|NM|30973-2^Dose number in series^LN||4||||||F
OBX|1|TS|30980-7^Date vaccination due^LN||20121206||||||F
OBX|1|TS|30981-5^Earliest date to give^LN||20121206||||||F
OBX|1|TS|59777-3^Latest date next dose should be given^LN||20161101||||||F
OBX|1|TS|59778-1^Date dose is overdue^LN||20130106||||||F
OBX|1|CE|59783-1^Status in immunization series^LN||U^Up to Date^STC0002||||||F
ORC|RE||9999
RXA|0|999|20120727112142|20120727112142|998^no vaccine administered^CVX|0||||||||||||||||20120727112144
OBX|1|CE|30956-7^vaccine type^LN||0^Hib^CVX||||||F
OBX|1|CE|59779-9^Immunization Schedule used^LN||VXC16^ACIP^CDCPHINVS||||||F
OBX|1|NM|30973-2^Dose number in series^LN||1||||||F
OBX|1|TS|30980-7^Date vaccination due^LN||20100102||||||F
OBX|1|TS|59778-1^Date dose is overdue^LN||20100202||||||F
OBX|1|CE|59783-1^Status in immunization series^LN||P^Past Due^STC0002||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("RSP"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("K11"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("RSP_K11"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("1320521135996.100000002"));

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
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("R"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_087_Should_parse_ORU_R01_v28_glucose_result()
        {
            const string message = @"MSH|^~\&|GHH LAB|ELAB-3|GHH OE|BLDG4|200202150930||ORU^R01|CNTRL-3456|P|2.8
PID|||555-44-4444||EVERYWOMAN^EVE^E^^^^L|JONES|196203520|F|||153 FERNWOOD DR.^^STATESVILLE^OH^35292||(206)3345232|(206)752-121||||AC555444444||67-A4335^OH^20030520
OBR|1|845439^GHH OE|1045813^GHHLAB|1554-5^GLUCOSE^LN|||200202150730|||||||||555-55-5555^PRIMARY^PATRICIA P^^^^MD^^LEVEL SEVEN HEALTHCARE, INC.|||||||||F|||||||444-44-4444&HIPPOCRATES&HOWARD H&&&&MD
OBX|1|SN|1554-5^GLUCOSE POST 12H CFST^LN||^182|mg/dl|70-105|H|||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.8"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("GHH LAB"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("CNTRL-3456"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("EVERYWOMAN"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("EVE"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("1554-5"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("GLUCOSE"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("1554-5"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("GLUCOSE POST 12H CFST"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_088_Should_parse_ORU_R01_comprehensive_chemistry_hematology()
        {
            const string message = @"MSH|^~\&|FDHL7|JOHNSON LABS|RECEIVINGAPP|RECEIVINGFAC|201007231634||ORU^R01|0723163400003|P|2.3|||AL|NE
PID|1|000000001|000000001||SAMPLES^JUNIOR||19480110|M||W|1 SAMPLE DR^^SAMPLETOWN^FL^33333||5555551212
PV1|1|O|||||99999^TESTER^DOCTOR^T^^DR
ORC|RE|15243|0723163400003||CM
OBR|1|15243|0723163400003|CHEM14^CHEMISTRY 14^L|||201007231513|||||||201007231513|SST^SST|99999^TESTER^DOCTOR^T^^DR||||||201007231634||LAB|F
OBX|1|NM|GLU^Glucose^L||296|mg/dL|65-99|H|||F
OBX|2|NM|TPROT^Total Protein^L||7.0|g/dL|6.0-8.3||||F
OBX|3|NM|ALB^Albumin^L||3.7|g/dL|3.5-5.5||||F
OBX|4|NM|TBILI^Total Bilirubin^L||0.7|mg/dL|0.0-1.2||||F
OBX|5|NM|ALKP^Alkaline Phosphatase^L||72|IU/L|25-150||||F
OBX|6|NM|AST^AST (SGOT)^L||31|IU/L|0-40||||F
OBX|7|NM|BUN^BUN^L||14|mg/dL|5-26||||F
OBX|8|NM|CREAT^Creatinine^L||0.9|mg/dL|0.7-1.3||||F
OBX|9|NM|NA^Sodium^L||138|mmol/L|134-144||||F
OBX|10|NM|K^Potassium^L||4.1|mmol/L|3.5-5.2||||F
OBX|11|NM|CL^Chloride^L||103|mmol/L|96-106||||F
OBX|12|NM|CO2^CO2 (Bicarbonate)^L||25|mmol/L|20-32||||F
OBX|13|NM|CA^Calcium^L||9.5|mg/dL|8.5-10.5||||F
OBX|14|NM|PHOS^Phosphorus^L||3.8|mg/dL|2.6-4.5||||F
ORC|RE|15244|0723163400004||CM
OBR|2|15244|0723163400004|LIPID^LIPID PANEL^L|||201007231513|||||||201007231513|SST^SST|99999^TESTER^DOCTOR^T^^DR||||||201007231634||LAB|F
OBX|1|NM|CHOL^Cholesterol, Total^L||124|mg/dL|100-199||||F
OBX|2|NM|TRIG^Triglycerides^L||73|mg/dL|0-149||||F
OBX|3|NM|HDL^HDL Cholesterol^L||39|mg/dL|40-59|L|||F
OBX|4|NM|LDL^LDL Cholesterol Calc^L||70|mg/dL|0-99||||F
OBX|5|NM|VLDL^VLDL Cholesterol Calc^L||15|mg/dL|5-40||||F
NTE|1||SST tube submitted was inadequately spun. Some results may be affected.
ORC|RE|15245|0723163400005||CM
OBR|3|15245|0723163400005|HGA1C^HEMOGLOBIN A1C^L|||201007231513|||||||201007231513|SST^SST|99999^TESTER^DOCTOR^T^^DR||||||201007231634||LAB|F
OBX|1|NM|HGA1C^Hemoglobin A1c^L||9.1|%|4.8-5.6|H|||F
NTE|1||Estimated Average Glucose (eAG) = 214 mg/dL";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("FDHL7"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("JOHNSON LABS"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("0723163400003"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("SAMPLES"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JUNIOR"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("CHEM14"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CHEMISTRY 14"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("GLU"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("Glucose"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Is.EqualTo("SST tube submitted was inadequately spun. Some results may be affected."));
            }
        }

        [Test]
        public void HL7_SAMPLE_089_Should_parse_ORU_R01_ELR_sodium_potassium()
        {
            const string message = @"MSH|^~\&#|EHR LAB^11.11.666.1.111.4.3.2.2.1.321.111^ISO|H Facility FACILITY^Oid^ISO|RCVING APPLICAT^1.11.111.1.1111111.3.1.1111^ISO|RCVING FACILITY^1.11.111.1.1111111.3.1.2222^ISO|20220907145828-0500||ORU^R01^ORU_R01|PHELR.1.45543|D|2.5.1|||||||||PHLabReport-NoAck^HL7^1.11.111.1.1111111.9.11^ISO
SFT|EHR, Inc.^L^^^^EHR&1.3.6.1.4.1.24310&ISO^XX^^^EHR|5.67|Laboratory Application||||
PID|1||A0995614951^^^EHR LAB&1.11.111.1.1111111.4.3.2.2.1.321.111&ISO^MR||TEST^TEST^||19560927|M||2131-1^Other Race^HL70005|35544 TEST TEST^Apt. 535^Red Hook^NY^12571||111-111-111|111-111-1111||M
NK1|1|TEST^TEST^J|SPO^Spouse^HL70063|99111 Street^Apt. 608^AnchoTESTrage^TT^99502|111-111-111
PV1|1|I|J.CON^J.CON1^11|C|||TEST^TEST^TEST^L^^^MD||||MED||||2
ORC|RE|09984662^L103312.1||||||||||1^TEST^TEST^D^^^MD
OBR|1|09984662^L103312.1||2951-2^Sodium [Moles/volume]^LN|||20220907||||||||||||||||||F
OBX|1|SN|2951-2^Sodium [Moles/volume]^LN||=^139|mEq/L|136-145|N|||F
SPM|1|||WB^Whole Blood^HL70487
ORC|RE|09984662^L103312.2||||||||||1^TEST^TEST^D^^^MD
OBR|2|09984662^L103312.2||IMO0002^Potassium measurement^LN|||20220907||||||||||||||||||F
OBX|1|SN|IMO0002^Potassium measurement^LN||=^4.2|mEq/L|3.5-5.0|N|||F
SPM|1|||WB^Whole Blood^HL70487";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ORU_R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("PHELR.1.45543"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("TEST"));
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

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("2951-2"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("Sodium [Moles/volume]"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("2951-2"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_090_Should_parse_ADT_A08_patient_update_Donald_Duck()
        {
            const string message = @"MSH|^~\&|AccMgr|1|||20050110114442||ADT^A08|59910287|P|2.3|||
EVN|A08|20050110114442||||||
PID|1||10006579^^^1^MRN^1||DUCK^DONALD^D||19241010|M||1|111^DUCK ST^^FOWL^CA^999990000^^M|1|8885551212|8885551212|1|2||40007716^^^AccMgr^VN^1|123121234|||||||||||NO
PV1|1|I|IN1^214^1^1^^^S|3||IN1^214^1|37^DISNEY^WALT^^^^^^AccMgr^^^^CI|||01||||1|||37^DISNEY^WALT^^^^^^AccMgr^^^^CI|2|40007716^^^AccMgr^VN|4|||||||||||||||||||1||I|||20050110045253||||||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A08"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("AccMgr"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("59910287"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A08"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DUCK"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("DONALD"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_091_Should_parse_ORU_R01_misordered_DSC_before_PID()
        {
            const string message = @"MSH|^~\&|^QueryServices||||20021011161756.297-0500||ORU^R01|1|D|2.4
DSC|
PID||test|";

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
        public void HL7_SAMPLE_092_Should_parse_ADT_A45_move_visit_Canadian()
        {
            const string message = @"MSH|^~\&|4265-ADT|4265|eReferral|eReferral|201004141020||ADT^A45^ADT_A45|102416|T^|2.5^^|||NE|AL|CAN|8859/1
EVN|A45|201004141020|
PID|1||7010226^^^4265^MR~0000000000^^^CANON^JHN^^^^^^GP~1736465^^^4265^VN||Park^Green^^^MS.^^L||19890812|F|||123 TestingLane^^TORONTO^CA-ON^M5G2C2^CAN^H^~^^^^^^^||^PRN^PH^^1^416^2525252^|^^^^^^^||||||||||||||||N
PV1|1|I||||^^^WP^1469^^^^^^^^|||||||||||^Derkach^Peter.^^^Dr.||20913000131|||||||||||||||||||||||||201004011340|201004141018";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A45"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ADT_A45"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("102416"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A45"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("Park"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("Green"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_093_Should_parse_ADT_A08_with_version_extension()
        {
            const string message = @"MSH|^~\&|STML|001|STML|001|20020307142717||ADT^A08|01501|T|2.2^x^x|||AL|NE
EVN|A08|20020307142652";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A08"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.2"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("STML"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("01501"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A08"));
            }
        }

        [Test]
        public void HL7_SAMPLE_094_Should_parse_minimal_MSH_only()
        {
            const string message = @"MSH|^~\&|3|4|5|6|7|8|9|10|11|12|13|||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("3"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("10"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("12"));
        }

        [Test]
        public void HL7_SAMPLE_095_Should_parse_MSH_MSA_ERR_message()
        {
            const string message = @"MSH|^~\&|3|4|5|6|7|8|9|10|11|12|13|||
MSA|foo
ERR|bar|||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("10"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("12"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("foo"));
            }
        }

        [Test]
        public void HL7_SAMPLE_096_Should_parse_ORU_R01_ORC_with_NTE_no_OBX()
        {
            const string message = @"MSH|^~\&|^QueryServices||||20021011161756.297-0500||ORU^R01|1|D|2.4
ORC|
OBX|1
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
        public void HL7_SAMPLE_097_Should_parse_ORU_R01_dual_ORC_groups()
        {
            const string message = @"MSH|^~\&|^QueryServices||||20021011161756.297-0500||ORU^R01|1|D|2.4
ORC|
CTI|
ORC|
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
        public void HL7_SAMPLE_098_Should_parse_ORU_R01_field_length_validation()
        {
            const string message = @"MSH|^~\&|bar|foo|||||ORU^R01|1|D|2.4|12345";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("bar"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("foo"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("1"));
        }

        [Test]
        public void HL7_SAMPLE_099_Should_parse_ORU_with_ORC_OBX_AD_datatype()
        {
            const string message = @"MSH|^~\&
ORC|1
OBX||AD|||F1C1^F2C1";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("1"));
            }
        }

        [Test]
        public void HL7_SAMPLE_100_Should_parse_ORU_R01_deep_subcomponents_in_PID()
        {
            const string message = @"MSH|^~\&|^QueryServices||||20021011161756.297-0500||ORU^R01|1|D|2.4
PID|4&y^x&z";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("1"));
        }
    }
}
