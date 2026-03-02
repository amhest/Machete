namespace Machete.HL7.Tests.Corpus
{
    using NUnit.Framework;
    using Machete.HL7.Testing;
    using Machete.HL7Schema.V26;

    [TestFixture]
    public class CorpusTests_Batch09 :
        HL7MacheteTestHarness<MSH, HL7V26Entity>
    {
        [Test]
        public void HL7_SAMPLE_201_Should_parse_SIU_S12_appointment_booking()
        {
            const string message = @"MSH|^~\&|EPIC|EPIC|||20160502162033||SIU^S12|538|D|2.3||
SCH|01928374|57483920|||||||1|hr|1^^^20160515133000|||||||||1173^MATTHEWS^JAMES^A|||||BOOKED
PID|1||30745109^^^^EPI||FREDERICKS^JANE^I^^MRS.^||19730501|F||Cauc|421 N. BAKER ST^^MADISON^WI^53513^US^^^DN|DN|(608)555-6789|(608)555-4321||S||11396810|321-87-6543||||^^^WI^^
PV1|||^^^CARE HEALTH SYSTEMS^^^^^||||1173^MATTHEWS^JAMES^A^^^||||||||||||610613||||||||||||||||||||||||||||||||V
DG1||I10|S82^ANKLE FRACTURE^I10|ANKLE FRACTURE||
RGS|1|A|094
AIS|1||73610^X-RAY ANKLE 3+ VW^CPT|20160515134500|15|min|45|min||
AIP|1||1069^GOOD^ALLAN^B|RADIOLOGIST||20160515134500|15|min|45|min||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("SIU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("S12"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("EPIC"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("EPIC"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("FREDERICKS"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JANE"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
            {
                Assert.That(dg1Result.Result.DiagnosiCode.Value.Identifier.Value, Is.Not.Null.Or.Empty);
            }
        }

        [Test]
        public void HL7_SAMPLE_202_Should_parse_SIU_S14_appointment_modification()
        {
            const string message = @"MSH|^~&|GPMS|CTX||MED2000|200803060953||SIU^S14|20080306953450|P|2.3||||||||
SCH|00331839401|||||58||HLCK^HEALTHCHECK ANY AGE|20|MIN|^^^200803061000|||||JOHN||||VALERIE|||||ARRIVED|";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("SIU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("S14"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("GPMS"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("CTX"));
        }

        [Test]
        public void HL7_SAMPLE_203_Should_parse_SIU_S15_cancellation_minimal()
        {
            const string message = @"MSH|^~\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613072836||SIU^S15|24916597|P|2.3||||||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("SIU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("S15"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("SENDING_APPLICATION"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("SENDING_FACILITY"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("24916597"));
        }

        [Test]
        public void HL7_SAMPLE_204_Should_parse_SIU_S26_no_show()
        {
            const string message = @"MSH|^~\&|SCHEDULING|MAIN_HOSP|CARDIOLOGY|MAIN_HOSP|20240315160000||SIU^S26|MSG00204|P|2.5|||AL|NE
SCH|APT93847|APT93847||||ROUTINE|99243^CARDIOLOGY CONSULT^CPT||30|min|^^^20240315140000^^30^min|||||^MILLER^DAVID||||||DR1234^CHEN^SARAH^M^^DR|||NOSHOW
PID|1||MRN449283^^^MAIN_HOSP^MR||MILLER^DAVID^R||19680422|M|||88 OAK LANE^^SPRINGFIELD^IL^62701||2175559876|||M|||SSN667-88-9012
PV1|1|O|CARDIO^301^A||||DR1234^CHEN^SARAH^M^^DR||||CARD||||||||V29384||||||||||||||||||||||||20240315
RGS|1|A
AIS|1||99243^CARDIOLOGY CONSULT^CPT|20240315140000|||30|min
AIP|1||DR1234^CHEN^SARAH^M|CARDIOLOGIST||20240315140000|||30|min";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("SIU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("S26"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("SCHEDULING"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00204"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("MILLER"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("DAVID"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }
        }

        [Test]
        public void HL7_SAMPLE_205_Should_parse_ADT_A28_add_person_mpi()
        {
            const string message = @"MSH|^~\&|Regional MPI||Master MPI|Alpha Hospital|20060501140010||ADT^A28|3948375|P^T|2.4|||ER
EVN|A28|20060501140008|||000338475^Author^Arthur^^^^^Regional MPI&2.16.840.1.113883.19.201&ISO^L|20060501140008
PID|||000197245^^^NationalPN&2.16.840.1.113883.19.3&ISO^PN~4532^^^CarefulCareClinic&2.16.840.1.113883.19.2.400566&ISO^PI~3242346^^^GoodmanGP&2.16.840.1.113883.19.2.450998&ISO^PI||Patient^Patricia^^^^^L||19750103|F|||Randomroad 23a&Randomroad&23a^^Anytown^^1200^^H||555 3542557^ORN^PH~555 3542558^ORN^FX|555 5557865^WPN^PH
PV1||N|";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A28"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("Regional MPI"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("3948375"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A28"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("Patient"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("Patricia"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_206_Should_parse_ORU_R01_glucose_high()
        {
            const string message = @"MSH|^~\&|GHH LAB|ELAB-3|GHH OE|BLDG4|200202150930||ORU^R01|CNTRL-3456|P|2.4
PID|||555-44-4444||EVERYWOMAN^EVE^E^^^^L|JONES|19620320|F|||153 FERNWOOD DR.^^STATESVILLE^OH^35292||(206)3345232|(206)752-121||||AC555444444||67-A4335^OH^20030520
OBR|1|845439^GHH OE|1045813^GHH LAB|15545^GLUCOSE|||200202150730|||||||||555-55-5555^PRIMARY^PATRICIA P^^^^MD^^|||||||||F||||||444-44-4444^HIPPOCRATES^HOWARD H^^^^MD
OBX|1|SN|1554-5^GLUCOSE^POST 12H CFST:MCNC:PT:SER/PLAS:QN||^182|mg/dl|70_105|H|||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("GHH LAB"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("ELAB-3"));

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
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("15545"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("GLUCOSE"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("1554-5"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("GLUCOSE"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_207_Should_parse_QRY_Q01_demographics_query()
        {
            const string message = @"MSH|^~\&|SystemA|ClinicA|SystemB|ClinicB|202208241100||QRY^Q01|12345|P|2.3
QRD|202208241100|R|I|GetPatient|||1^RD|10101|DEM||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("QRY"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("Q01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("SystemA"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("ClinicA"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("12345"));
        }

        [Test]
        public void HL7_SAMPLE_208_Should_parse_MFN_M02_master_file_staff()
        {
            const string message = @"MSH|^~\&|SystemA|ClinicA|SystemB|ClinicB|202208241200||MFN^M02|67890|P|2.3
MFE|MUP|123456|202208241200
STF|Jones|Bob|Nursing||Active";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("MFN"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("M02"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("SystemA"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("67890"));
        }

        [Test]
        public void HL7_SAMPLE_209_Should_parse_RDE_O13_pharmacy_order()
        {
            const string message = @"MSH|^~\&|iScribe|XYZHosp|Pharmacy|ABCHealth|202208241300||RDE^O13|34567|P|2.5
PID|||1234||Doe^John
ORC|NW|987654
RXE|RXD^Drug1^DrugDB|500|mg|||||||||Pharmacy^XYZHospital";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("RDE"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O13"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("iScribe"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("Doe"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("John"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("NW"));
            }
        }

        [Test]
        public void HL7_SAMPLE_210_Should_parse_RDS_O13_pharmacy_dispense()
        {
            const string message = @"MSH|^~\&|Pharmacy|XYZHosp|iScribe|ABCHealth|202208241400||RDS^O13|45678|P|2.5
PID|||1234||Doe^John
ORC|CM|987654
RXD|RXD^Drug1^DrugDB|500|mg||202208241400||Pharmacy^XYZHospital";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("RDS"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O13"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("Pharmacy"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("Doe"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("John"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("CM"));
            }
        }

        [Test]
        public void HL7_SAMPLE_211_Should_parse_ADT_A01_outpatient_registration()
        {
            const string message = @"MSH|^~\&|RegSys|XYZHosp|EKG|ABCImgCtr|202208221340||ADT^A01|123456789|P|2.5
PID|||1234||Doe^John||19611015|M|||123 Main St.^Apt 101^Benbrook^TX^76107||555-555-5555|||M||1234567890||||||||||^XYZHosp
PV1||O|OP^PAREG^||||2342^Jones^Bob|||OP|||||||||2|||||||||||||||||||||||||202208221340|||1";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("RegSys"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("123456789"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("Doe"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("John"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }
        }

        [Test]
        public void HL7_SAMPLE_212_Should_parse_ACK_A01_nonstandard_structure()
        {
            const string message = @"MSH|^~\&|EKG|ABCImgCtr|RegSys|XYZHosp|202208221340||ACK^A01|56789|P|2.5
ACK|||AA|Message received successfully.";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ACK"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("EKG"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("56789"));

            // Note: This message uses a non-standard ACK segment instead of MSA
            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AA"));
            }
        }

        [Test]
        public void HL7_SAMPLE_213_Should_parse_ORU_R01_minimal_positive_result()
        {
            const string message = @"MSH|^~\&|LAB|XYZHosp|EKG|ABCImgCtr|202208221340||ORU^R01|67890|P|2.5
PID|||1234||Doe^John||19611015|M|||123 Main St.^Apt 101^Benbrook^TX^76107||555-555-5555|||M||1234567890||||||||||^XYZHosp
OBR|||1234567890||Test^T123^Test description
OBX|||Test^T123^Test description||Positive||||||||202208221340";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("LAB"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("67890"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("Doe"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("John"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("Test"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("T123"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("Test"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("T123"));
            }
        }

        [Test]
        public void HL7_SAMPLE_214_Should_parse_ORM_O01_glucose_lab_order()
        {
            const string message = @"MSH|^~\&|CARDIAC|XYZHosp|LAB|ABCImgCtr|202208221340||ORM^O01|78901|P|2.5
PID|||1234||Doe^John||19611015|M|||123 Main St.^Apt 101^Benbrook^TX^76107||555-555-5555|||M||1234567890||||||||||^XYZ
ORC|NW|1234|||CM||200708071255|||1234^Doctor^Bob||^XYZHosp
OBR|1|1234|1234567890|1651-3^GLUCOSE|||200708071255||||||||1234^Doctor^Bob||123456^LAB^XYZ||||200708071255|||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORM"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("CARDIAC"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("78901"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("Doe"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("John"));
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
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("1651-3"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("GLUCOSE"));
            }
        }

        [Test]
        public void HL7_SAMPLE_215_Should_parse_PPR_PC1_patient_problem_add()
        {
            const string message = @"MSH|^~\&|PCIS|GOOD HEALTH HOSPITAL|REPOSITORY|GOOD HEALTH HOSPITAL|||PPR^PC1||P|2.3
PID||0123456-1||EVERYMAN^ADAM^H|||||||9821111|
PV1|1|I|2000^2012^01||||004777^EVERYMAN1^ADAM^A|||SUR||||ADM|A0|
PRB|AD|199505011200|04411^Restricted Circulation^Nursing Problem List|||||199505011200|||IP^Inpatient^Problem Classification List|NU^Nursing^Management Discipline List|Acute^Acute^Persistence List|C^Confirmed^Confirmation Status List|A1^Active^Life Cycle Status List|199505011200|199504250000||2^Secondary^Ranking List|HI^High^Certainty Coding List||1^Fully^Awareness Coding List|2^Good^Prognosis Coding List||||
ROL|1^Diagnosing Provider^Role Master List|AD|^Edwards^A^^MD|199505011200||||
ROL|45^Recorder^Role Master List|AD|^EVERYWOMAN^EVE^^^^|199505011201||||
OBX|001|TX|^Peripheral Dependent Edema|1|Increasing Edema in lower limbs|
GOL|AD|199505011200|00312^Improve Peripheral Circulation^Goal Master List||||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("PPR"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("PC1"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("PCIS"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("GOOD HEALTH HOSPITAL"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("Peripheral Dependent Edema"));
            }
        }

        [Test]
        public void HL7_SAMPLE_216_Should_parse_RAS_O17_pharmacy_administration()
        {
            const string message = @"MSH|^~\&|NURSING|WEST_WING|PHARMACY|MAIN_HOSP|20240110140500||RAS^O17|MSG00216|P|2.5|||AL|NE
PID|1||MRN776543^^^MAIN_HOSP^MR||WATSON^EMILY^J||19850317|F|||234 PINE ST^^HARTFORD^CT^06103||8605557890
PV1|1|I|MED^412^B||||DR9876^PATEL^RAVI^K^^MD
ORC|RE|ORD44821|ORD44821||IP|||1^BID^^20240108^20240115||||DR9876^PATEL^RAVI^K^^MD
RXE|^^^20240108^20240115|00093-3107-01^AMOXICILLIN 500MG^NDC|500||mg|CAP^Capsule^FormCode||||30|CAP^Capsules|||||||||||||||PO^Oral^HL70162
RXR|PO^Oral^HL70162|MOU^Mouth^HL70163
RXA|0|1|20240110140000|20240110140100|00093-3107-01^AMOXICILLIN 500MG^NDC|500|mg|||||||||||||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("RAS"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O17"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("NURSING"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("WEST_WING"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00216"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("WATSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("EMILY"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_217_Should_parse_OMG_O19_radiology_ct_order()
        {
            const string message = @"MSH|^~\&|CPOE|MAIN_HOSP|RIS|RAD_DEPT|20240220103000||OMG^O19|MSG00217|P|2.5.1|||AL|NE
PID|1||MRN883721^^^MAIN_HOSP^MR||CHEN^ROBERT^W||19770814|M|||567 MAPLE DR^^PORTLAND^OR^97201||5035558234
PV1|1|I|MED^508^A||||DR5544^PARK^LISA^M^^MD
ORC|NW|ORD55912||GRP001|||||20240220103000|||DR5544^PARK^LISA^M^^MD|||||MAIN_HOSP
NTE|1|L|Patient reports intermittent abdominal pain for 2 weeks. R/O appendicitis vs diverticulitis.
OBR|1|ORD55912||74178^CT ABD AND PELVIS WITH IV CONTRAST^CPT|||20240220|||||||20240220103000|||||||||20240220|||1^^^20240220^^R||||||||||||||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("OMG"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O19"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("CPOE"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00217"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("CHEN"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("ROBERT"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("NW"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
            if (nteResult.HasResult)
            {
                Assert.That(nteResult.Result.Comment[0].Value, Is.EqualTo("Patient reports intermittent abdominal pain for 2 weeks. R/O appendicitis vs diverticulitis."));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("74178"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CT ABD AND PELVIS WITH IV CONTRAST"));
            }
        }

        [Test]
        public void HL7_SAMPLE_218_Should_parse_OMI_O23_imaging_order_chest_xray()
        {
            const string message = @"MSH|^~\&|RIS|RAD_DEPT|PACS|IMG_ARCHIVE|20240305091500||OMI^O23|MSG00218|P|2.5.1|||AL|NE
PID|1||MRN991234^^^MAIN_HOSP^MR||THOMPSON^SARAH^L||19900622|F|||789 ELM ST^^DENVER^CO^80202||3035554567
PV1|1|O|RAD^LOBBY||||DR7788^WILSON^JAMES^P^^MD
ORC|NW|ORD66234||||||20240305091500|||DR7788^WILSON^JAMES^P^^MD
OBR|1|ORD66234|RAD-2024-66234|71046^CHEST X-RAY 2 VIEWS^CPT|||20240305|||||||20240305091500|||||||||20240305|||1
IPC|ACC-2024-66234|RP-2024-66234|1.2.840.113619.2.55.3.604888192.2024.66234|SPS-001|XR^Digital Radiography|CR01^CHEST ROOM 1||||71046^CHEST X-RAY 2 VIEWS^CPT";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("OMI"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O23"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("RIS"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("RAD_DEPT"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00218"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("THOMPSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("SARAH"));
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
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("71046"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CHEST X-RAY 2 VIEWS"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }
        }

        [Test]
        public void HL7_SAMPLE_219_Should_parse_OMS_O05_stock_requisition()
        {
            const string message = @"MSH|^~\&|MATMGMT|SUPPLY|PURCHASING|MAIN_HOSP|20240115080000||OMS^O05|MSG00219|P|2.5|||AL|NE
ORC|NW|REQ-2024-001||||||20240115080000|||ADM5566^MARTINEZ^CARLOS^J
RQD|1|SYR10ML^DISPOSABLE SYRINGE 10ML^LOCAL|100||EA|ED^Emergency Department|CC-4400|MED-SUPPLY-INC^Medical Supply Inc
ORC|NW|REQ-2024-002||||||20240115080000|||ADM5566^MARTINEZ^CARLOS^J
RQD|2|GLV-M^EXAM GLOVES MEDIUM^LOCAL|50||BX|ED^Emergency Department|CC-4400|MED-SUPPLY-INC^Medical Supply Inc";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("OMS"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O05"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("MATMGMT"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("SUPPLY"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00219"));

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("NW"));
            }
        }

        [Test]
        public void HL7_SAMPLE_220_Should_parse_OMD_O03_dietary_order()
        {
            const string message = @"MSH|^~\&|DIETETICS|FOOD_SVC|CPOE|MAIN_HOSP|20240201120000||OMD^O03|MSG00220|P|2.5|||AL|NE
PID|1||MRN554433^^^MAIN_HOSP^MR||GREEN^HAROLD^T||19550815|M
PV1|1|I|CARD^201^A||||DR3344^JONES^WILLIAM
ORC|NW|DIET-001||||||20240201120000|||DR3344^JONES^WILLIAM
ODS|D|CRD^Cardiac Diet|LOW-NA^Low Sodium 2g/day|REG^Regular Texture
ODT|L^Lunch|20240201120000||Small portions, patient has dentures
ORC|NW|DIET-002||||||20240201120000|||DR3344^JONES^WILLIAM
ODS|S|ENSURE^Ensure Plus Oral Supplement|VAN^Vanilla Flavor|
ODT|M^Morning Snack|20240201100000||Between meals supplementation";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("OMD"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("O03"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("DIETETICS"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("FOOD_SVC"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00220"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("GREEN"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("HAROLD"));
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
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("NW"));
            }
        }

        [Test]
        public void HL7_SAMPLE_221_Should_parse_ADT_A18_merge_patient()
        {
            const string message = @"MSH|^~\&|ADT|HOSP|MPI|HOSP|200301151030||ADT^A18|MSG00221|P|2.3
EVN|A18|200301151030
PID|1||11223344^^^HOSP^MR||SMITH^JOHN^D||19650412|M|||100 MAIN ST^^ANYTOWN^CA^90210||5551234567
MRG|99887766^^^HOSP^MR
PV1|1|I|MED^301^A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A18"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("HOSP"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00221"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A18"));
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
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_222_Should_parse_ADT_A23_delete_patient_record()
        {
            const string message = @"MSH|^~\&|ADT|MAIN_HOSP|EMR|MAIN_HOSP|20240310153000||ADT^A23|MSG00222|P|2.4|||AL|NE
EVN|A23|20240310153000
PID|1||MRN334455^^^MAIN_HOSP^MR||BROWN^ALICE^M||19820716|F
PV1|1|E|ED^TRIAGE^01||||DR1122^SMITH^ROBERT||||||||||V-2024-8876|||||||||||||||||||||||||20240308190000";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A23"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("MAIN_HOSP"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00222"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A23"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("BROWN"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("ALICE"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("E"));
            }
        }

        [Test]
        public void HL7_SAMPLE_223_Should_parse_ADT_A24_link_mother_baby()
        {
            const string message = @"MSH|^~\&|ADT|WOMENS_HOSP|MPI|WOMENS_HOSP|20240225093000||ADT^A24|MSG00223|P|2.4|||AL|NE
EVN|A24|20240225093000
PID|1||556677^^^WOMENS_HOSP^MR||WILSON^JENNIFER^A||19920305|F|||44 BIRCH RD^^SALEM^MA^01970||9785553456
PV1|1|I|OB^305^A||||DR4455^GARCIA^MARIA^L^^MD
PID|2||556678^^^WOMENS_HOSP^MR||WILSON^BABY GIRL^^^^||20240225|F
PV1|2|I|NICU^101^A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A24"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("WOMENS_HOSP"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00223"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A24"));
            }

            // First PID - mother
            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("WILSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JENNIFER"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_224_Should_parse_ADT_A44_move_account()
        {
            const string message = @"MSH|^~\&|BILLING|MAIN_HOSP|ADT|MAIN_HOSP|20240112141500||ADT^A44|MSG00224|P|2.4|||AL|NE
EVN|A44|20240112141500
PID|1||778899^^^MAIN_HOSP^MR||DAVIS^MARGARET^E||19780930|F|||55 CEDAR LN^^RENO^NV^89501||7755558901||||||||ACC-9999
MRG|667788^^^MAIN_HOSP^MR||ACC-9999";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A44"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("BILLING"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("MAIN_HOSP"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00224"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A44"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DAVIS"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MARGARET"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_225_Should_parse_ADT_A45_move_visit()
        {
            const string message = @"MSH|^~\&|ADT|MAIN_HOSP|EMR|MAIN_HOSP|20240118100000||ADT^A45|MSG00225|P|2.4|||AL|NE
EVN|A45|20240118100000
PID|1||445566^^^MAIN_HOSP^MR||ANDERSON^THOMAS^J||19880214|M
MRG|112233^^^MAIN_HOSP^MR||||V-2024-3344
PV1|1|I|SURG^401^B||||DR6677^WILLIAMS^NANCY^K^^MD|||||||||V-2024-3344";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A45"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("MAIN_HOSP"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00225"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A45"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("ANDERSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("THOMAS"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }
    }
}
