namespace Machete.HL7.Tests.Corpus
{
    using NUnit.Framework;
    using Machete.HL7.Testing;
    using Machete.HL7Schema.V26;

    [TestFixture]
    public class CorpusTests_Batch10 :
        HL7MacheteTestHarness<MSH, HL7V26Entity>
    {
        [Test]
        public void HL7_SAMPLE_226_Should_parse_ADT_A06_outpatient_to_inpatient()
        {
            const string message = @"MSH|^~\&|ADT|CITY_HOSP|BILLING|CITY_HOSP|20240405143000||ADT^A06|MSG00226|P|2.5|||AL|NE
EVN|A06|20240405143000
PID|1||MRN223344^^^CITY_HOSP^MR||TORRES^MICHAEL^A||19710628|M||2106-3^White^HL70005|233 SUMMIT AVE^^CHICAGO^IL^60614||3125557788|||M|||SSN445-66-7789
PV1|1|I|CICU^101^A^CITY_HOSP^^^^2N|||ED^TRIAGE^01|DR8899^COOPER^JAMES^R^^MD|DR7766^HEART^ANNA^K^^MD|CAR|||||||DR8899^COOPER^JAMES^R^^MD|EM|V-2024-5566|||||||||||||||||||||||20240405100000|20240405143000
DG1|1||I21.0^Acute transmural MI of anterior wall^ICD10||20240405|A
IN1|1|BCBS001^BLUE CROSS BLUE SHIELD|BC001|BLUE CROSS ILLINOIS|||||||GRP-4455||20240101|20241231|||TORRES^MICHAEL^A|01^Self|19710628|233 SUMMIT AVE^^CHICAGO^IL^60614|||1||||||||||||||POL-998877";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A06"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00226"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A06"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("TORRES"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MICHAEL"));
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
        public void HL7_SAMPLE_227_Should_parse_ADT_A07_inpatient_to_outpatient()
        {
            const string message = @"MSH|^~\&|ADT|CITY_HOSP|EMR|CITY_HOSP|20240320110000||ADT^A07|MSG00227|P|2.5|||AL|NE
EVN|A07|20240320110000
PID|1||MRN887766^^^CITY_HOSP^MR||PHILLIPS^KAREN^S||19630912|F|||456 LAKE VIEW DR^^MILWAUKEE^WI^53202||4145553322
PV1|1|O|ORTHO^CLINIC^A||||DR2233^RICHARDS^PETER^M^^MD||||REHAB|||||||V-2024-7788|||||||||||||||||||||||20240301080000|20240320110000
PV2||||||||20240320|7^days||KNEE REPLACEMENT REHAB FOLLOW-UP";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A07"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00227"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A07"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("PHILLIPS"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("KAREN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }
        }

        [Test]
        public void HL7_SAMPLE_228_Should_parse_ADT_A09_patient_departing_tracking()
        {
            const string message = @"MSH|^~\&|ADT|GENERAL_HOSP|TRACKING|GENERAL_HOSP|200512081400||ADT^A09|MSG00228|P|2.3
EVN|A09|200512081400
PID|1||MRN445566^^^GENERAL_HOSP^MR||FORD^JAMES^L||19590318|M
PV1|1|I|MEDSURG^402^B||||DR3344^BLAKE^SAMUEL|||MED|||||||||V-12345||||||||||||||||||||RAD^CT-1^A|||20051208";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A09"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00228"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A09"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("FORD"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JAMES"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_229_Should_parse_ADT_A10_patient_arriving_tracking()
        {
            const string message = @"MSH|^~\&|TRACKING|GENERAL_HOSP|ADT|GENERAL_HOSP|200512081415||ADT^A10|MSG00229|P|2.3
EVN|A10|200512081415
PID|1||MRN445566^^^GENERAL_HOSP^MR||FORD^JAMES^L||19590318|M
PV1|1|I|RAD^CT-1^A||||DR3344^BLAKE^SAMUEL|||MED|||||||||V-12345||||||||||||||||||||MEDSURG^402^B|||20051208";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A10"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00229"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("TRACKING"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A10"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("FORD"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JAMES"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_230_Should_parse_ADT_A14_pending_admit()
        {
            const string message = @"MSH|^~\&|ADT|CANCER_CTR|EMR|CANCER_CTR|20240322140000||ADT^A14|MSG00230|P|2.4|||AL|NE
EVN|A14|20240322140000
PID|1||MRN667788^^^CANCER_CTR^MR||KIM^GRACE^Y||19800415|F||2028-9^Asian^HL70005|789 CHERRY BLOSSOM WAY^^SEATTLE^WA^98101||2065559012
PV1|1|P|ONCO^502^A^CANCER_CTR||||DR4455^LEE^DAVID^H^^MD||||ONC|||||||V-PEND-001
PV2||||||||20240325080000|||CHEMOTHERAPY CYCLE 3 - BREAST CANCER TREATMENT";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A14"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00230"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A14"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("KIM"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("GRACE"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("P"));
            }
        }

        [Test]
        public void HL7_SAMPLE_231_Should_parse_ADT_A15_pending_transfer()
        {
            const string message = @"MSH|^~\&|ADT|MAIN_HOSP|ICU_SYS|MAIN_HOSP|20240418093000||ADT^A15|MSG00231|P|2.4|||AL|NE
EVN|A15|20240418093000
PID|1||MRN998877^^^MAIN_HOSP^MR||MARTINEZ^ROBERT^C||19550720|M|||321 OAK ST^^PHOENIX^AZ^85001||6025557766
PV1|1|I|MED^301^A||||DR1122^NGUYEN^THU^T^^MD||||RESP||||||||V-2024-8899||||||||||||||||||ICU^005^A|||20240416
PV2|||||||20240418140000|||WORSENING RESPIRATORY STATUS - REQUIRES ICU MONITORING";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A15"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00231"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A15"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("MARTINEZ"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("ROBERT"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_232_Should_parse_ADT_A16_pending_discharge()
        {
            const string message = @"MSH|^~\&|ADT|MAIN_HOSP|DISCHARGE_PLAN|MAIN_HOSP|20240501160000||ADT^A16|MSG00232|P|2.4|||AL|NE
EVN|A16|20240501160000
PID|1||MRN112244^^^MAIN_HOSP^MR||CHANG^LISA^W||19880102|F|||567 WILLOW CT^^SAN JOSE^CA^95112||4085553344
PV1|1|I|SURG^204^B||||DR5566^PATEL^ANIL^S^^MD||||SURG||||||||V-2024-3355|||||||||||||||||||||||20240428
PV2||||||20240502090000|||PENDING ATTENDING APPROVAL|01^Home^HL70112";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A16"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00232"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A16"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("CHANG"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("LISA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_233_Should_parse_ADT_A21_leave_of_absence()
        {
            const string message = @"MSH|^~\&|ADT|PSYCH_HOSP|EMR|PSYCH_HOSP|200908141000||ADT^A21|MSG00233|P|2.3
EVN|A21|200908141000
PID|1||MRN334411^^^PSYCH_HOSP^MR||PARKER^WILLIAM^T||19750520|M|||890 MEADOW LN^^AUSTIN^TX^78701
PV1|1|I|PSYCH^201^A||||DR7788^BAKER^SUSAN|||PSY|||||||||V-09-4455
PV2|||||||||20090816100000|48^hours|THERAPEUTIC HOME VISIT";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A21"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00233"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A21"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("PARKER"));
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
        public void HL7_SAMPLE_234_Should_parse_ADT_A22_return_from_leave()
        {
            const string message = @"MSH|^~\&|ADT|PSYCH_HOSP|EMR|PSYCH_HOSP|200908161015||ADT^A22|MSG00234|P|2.3
EVN|A22|200908161015
PID|1||MRN334411^^^PSYCH_HOSP^MR||PARKER^WILLIAM^T||19750520|M
PV1|1|I|PSYCH^201^A||||DR7788^BAKER^SUSAN|||PSY|||||||||V-09-4455";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A22"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00234"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A22"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("PARKER"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("WILLIAM"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_235_Should_parse_ADT_A25_cancel_pending_discharge()
        {
            const string message = @"MSH|^~\&|ADT|MAIN_HOSP|DISCHARGE_PLAN|MAIN_HOSP|20240502060000||ADT^A25|MSG00235|P|2.4|||AL|NE
EVN|A25|20240502060000
PID|1||MRN112244^^^MAIN_HOSP^MR||CHANG^LISA^W||19880102|F
PV1|1|I|SURG^204^B||||DR5566^PATEL^ANIL^S^^MD||||SURG||||||||V-2024-3355|||||||||||||||||||||||20240428";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A25"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00235"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A25"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("CHANG"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("LISA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_236_Should_parse_ADT_A26_cancel_pending_transfer()
        {
            const string message = @"MSH|^~\&|ADT|MAIN_HOSP|ICU_SYS|MAIN_HOSP|20240418150000||ADT^A26|MSG00236|P|2.4|||AL|NE
EVN|A26|20240418150000
PID|1||MRN998877^^^MAIN_HOSP^MR||MARTINEZ^ROBERT^C||19550720|M
PV1|1|I|MED^301^A||||DR1122^NGUYEN^THU^T^^MD||||RESP||||||||V-2024-8899";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A26"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00236"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A26"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("MARTINEZ"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("ROBERT"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_237_Should_parse_ADT_A27_cancel_pending_admit()
        {
            const string message = @"MSH|^~\&|ADT|CANCER_CTR|EMR|CANCER_CTR|20240324100000||ADT^A27|MSG00237|P|2.4|||AL|NE
EVN|A27|20240324100000
PID|1||MRN667788^^^CANCER_CTR^MR||KIM^GRACE^Y||19800415|F
PV1|1|P|ONCO^502^A||||DR4455^LEE^DAVID^H^^MD||||ONC|||||||V-PEND-001";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A27"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00237"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A27"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("KIM"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("GRACE"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("P"));
            }
        }

        [Test]
        public void HL7_SAMPLE_238_Should_parse_ADT_A38_cancel_pre_admit()
        {
            const string message = @"MSH|^~\&|ADT|ORTHO_CTR|EMR|ORTHO_CTR|20240215093000||ADT^A38|MSG00238|P|2.4|||AL|NE
EVN|A38|20240215093000
PID|1||MRN776655^^^ORTHO_CTR^MR||WHITE^NANCY^L||19720811|F|||234 MAPLE AVE^^BOSTON^MA^02101||6175558877
PV1|1|P|ORTHO^305^A||||DR9900^JOHNSON^MARK^A^^MD||||ORTHO";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A38"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00238"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A38"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("WHITE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("NANCY"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("P"));
            }
        }

        [Test]
        public void HL7_SAMPLE_239_Should_parse_ADT_A52_cancel_leave_of_absence()
        {
            const string message = @"MSH|^~\&|ADT|PSYCH_HOSP|EMR|PSYCH_HOSP|200908141100||ADT^A52|MSG00239|P|2.5|||AL|NE
EVN|A52|200908141100
PID|1||MRN334411^^^PSYCH_HOSP^MR||PARKER^WILLIAM^T||19750520|M
PV1|1|I|PSYCH^201^A||||DR7788^BAKER^SUSAN|||PSY|||||||||V-09-4455";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A52"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00239"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A52"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("PARKER"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("WILLIAM"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_240_Should_parse_ORU_R01_CBC_with_differential()
        {
            const string message = @"MSH|^~\&|LAB|MAIN_HOSP|EMR|MAIN_HOSP|20240612083000||ORU^R01|MSG00240|P|2.5.1|||AL|NE
PID|1||MRN445599^^^MAIN_HOSP^MR||SANTOS^MARIA^L||19850914|F||2135-2^Hispanic^HL70005|678 ROSA PARKS BLVD^^MIAMI^FL^33101||3055558877
PV1|1|O|LAB^DRAW^01
ORC|RE|ORD77001|ORD77001||CM
OBR|1|ORD77001|LAB-2024-77001|58410-2^CBC W AUTO DIFFERENTIAL^LOINC|||20240612080000|||||||||DR3344^REYES^CARLOS||||||20240612083000|||F
OBX|1|NM|6690-2^LEUKOCYTES^LOINC||7.2|10*3/uL|4.5-11.0|N|||F
OBX|2|NM|789-8^ERYTHROCYTES^LOINC||4.1|10*6/uL|3.8-5.1|N|||F
OBX|3|NM|718-7^HEMOGLOBIN^LOINC||10.8|g/dL|12.0-16.0|L|||F
OBX|4|NM|4544-3^HEMATOCRIT^LOINC||33.2|%|36.0-46.0|L|||F
OBX|5|NM|787-2^MCV^LOINC||81.0|fL|80.0-100.0|N|||F
OBX|6|NM|785-6^MCH^LOINC||26.3|pg|27.0-33.0|L|||F
OBX|7|NM|786-4^MCHC^LOINC||32.5|g/dL|32.0-36.0|N|||F
OBX|8|NM|777-3^PLATELETS^LOINC||245|10*3/uL|150-400|N|||F
OBX|9|NM|751-8^NEUTROPHILS ABS^LOINC||4.3|10*3/uL|1.8-7.7|N|||F
OBX|10|NM|731-0^LYMPHOCYTES ABS^LOINC||2.1|10*3/uL|1.0-4.8|N|||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00240"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("LAB"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("SANTOS"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MARIA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
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

        [Test]
        public void HL7_SAMPLE_241_Should_parse_ORU_R01_comprehensive_metabolic_panel()
        {
            const string message = @"MSH|^~\&|CHEM_LAB|MAIN_HOSP|EMR|MAIN_HOSP|20240515101500||ORU^R01|MSG00241|P|2.5|||AL|NE
PID|1||MRN332211^^^MAIN_HOSP^MR||KIM^DAVID^S||19700203|M|||1234 TECH BLVD^^SAN FRANCISCO^CA^94105||4155551234
ORC|RE|ORD88100|ORD88100||CM
OBR|1|ORD88100|LAB-2024-88100|24323-8^COMPREHENSIVE METABOLIC PANEL^LOINC|||20240515093000|||||||||DR1100^WONG^HENRY||||||20240515101500|||F
OBX|1|NM|2345-7^GLUCOSE^LOINC||95|mg/dL|74-106|N|||F
OBX|2|NM|3094-0^BUN^LOINC||16|mg/dL|6-24|N|||F
OBX|3|NM|2160-0^CREATININE^LOINC||1.0|mg/dL|0.7-1.3|N|||F
OBX|4|NM|2951-2^SODIUM^LOINC||140|mmol/L|136-145|N|||F
OBX|5|NM|2823-3^POTASSIUM^LOINC||4.2|mmol/L|3.5-5.1|N|||F
OBX|6|NM|2075-0^CHLORIDE^LOINC||102|mmol/L|98-106|N|||F
OBX|7|NM|2028-9^CO2^LOINC||24|mmol/L|20-29|N|||F
OBX|8|NM|17861-6^CALCIUM^LOINC||9.4|mg/dL|8.5-10.5|N|||F
OBX|9|NM|2885-2^TOTAL PROTEIN^LOINC||7.0|g/dL|6.0-8.3|N|||F
OBX|10|NM|1751-7^ALBUMIN^LOINC||4.2|g/dL|3.5-5.5|N|||F
OBX|11|NM|1975-2^TOTAL BILIRUBIN^LOINC||0.8|mg/dL|0.1-1.2|N|||F
OBX|12|NM|6768-6^ALKALINE PHOSPHATASE^LOINC||72|U/L|44-147|N|||F
OBX|13|NM|1920-8^AST^LOINC||25|U/L|10-40|N|||F
OBX|14|NM|1742-6^ALT^LOINC||30|U/L|7-56|N|||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00241"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("CHEM_LAB"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("KIM"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("DAVID"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("24323-8"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("COMPREHENSIVE METABOLIC PANEL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("2345-7"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("GLUCOSE"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
            }
        }

        [Test]
        public void HL7_SAMPLE_242_Should_parse_ORU_R01_mixed_OBX_datatypes_pathology()
        {
            const string message = @"MSH|^~\&|PATH_LAB|MAIN_HOSP|EMR|MAIN_HOSP|20240301140000||ORU^R01|MSG00242|P|2.5|||AL|NE
PID|1||MRN556688^^^MAIN_HOSP^MR||JOHNSON^PATRICIA^A||19650729|F
ORC|RE|ORD99200|ORD99200||CM
OBR|1|ORD99200|PATH-2024-99200|88305^SURGICAL PATHOLOGY^CPT|||20240228||||||||DR4455^MILLER^SUSAN||||||20240301140000|||F
OBX|1|ST|22634-0^PATH REPORT SITE^LOINC||Right breast, upper outer quadrant||||||F
OBX|2|CE|22637-3^PATH DIAGNOSIS^LOINC||8500/3^Infiltrating duct carcinoma NOS^ICD-O-3||||||F
OBX|3|NM|44648-4^TUMOR SIZE^LOINC||1.8|cm|||||F
OBX|4|ST|21892-5^TUMOR GRADE^LOINC||Grade 2 - Moderately differentiated||||||F
OBX|5|CE|85337-4^ER STATUS^LOINC||10828004^Positive^SCT||||||F
OBX|6|CE|85339-0^PR STATUS^LOINC||10828004^Positive^SCT||||||F
OBX|7|CE|85318-4^HER2 STATUS^LOINC||260385009^Negative^SCT||||||F
OBX|8|NM|85344-0^KI-67 INDEX^LOINC||15|%|||||F
OBX|9|DT|33731-7^DATE SPECIMEN COLLECTED^LOINC||20240228||||||F
OBX|10|FT|22638-1^PATH COMMENTS^LOINC||Margins are clear\.br\No lymphovascular invasion identified\.br\Sentinel lymph node: 0/3 positive||||||F
OBX|11|TX|22636-5^PATH NARRATIVE^LOINC||The specimen consists of a lumpectomy measuring 4.2 x 3.1 x 2.8 cm. A firm, stellate mass measuring 1.8 cm in greatest dimension is identified in the upper outer quadrant. The tumor is well-circumscribed with pushing borders. No necrosis is identified.||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00242"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("PATH_LAB"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("JOHNSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("PATRICIA"));
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
        public void HL7_SAMPLE_243_Should_parse_ORU_R01_multiple_OBR_groups()
        {
            const string message = @"MSH|^~\&|LAB|MAIN_HOSP|EMR|MAIN_HOSP|20240401120000||ORU^R01|MSG00243|P|2.5|||AL|NE
PID|1||MRN778800^^^MAIN_HOSP^MR||BAKER^STEVEN^R||19800505|M
PV1|1|O|CLINIC^A
ORC|RE|ORD-A001|LAB-A001||CM
OBR|1|ORD-A001|LAB-A001|51990-0^BASIC METABOLIC PANEL^LOINC|||20240401100000|||||||||DR2200^CHEN^WEI||||||20240401120000|||F
OBX|1|NM|2345-7^GLUCOSE^LOINC||102|mg/dL|74-106|N|||F
OBX|2|NM|2951-2^SODIUM^LOINC||141|mmol/L|136-145|N|||F
OBX|3|NM|2823-3^POTASSIUM^LOINC||4.0|mmol/L|3.5-5.1|N|||F
OBX|4|NM|2160-0^CREATININE^LOINC||0.9|mg/dL|0.7-1.3|N|||F
ORC|RE|ORD-B002|LAB-B002||CM
OBR|2|ORD-B002|LAB-B002|24356-8^URINALYSIS COMPLETE^LOINC|||20240401100000|||||||||DR2200^CHEN^WEI||||||20240401120000|||F
OBX|1|ST|5778-6^COLOR^LOINC||Yellow||||||F
OBX|2|ST|5767-9^APPEARANCE^LOINC||Clear||||||F
OBX|3|NM|2756-5^PH^LOINC||6.0||5.0-8.0|N|||F
OBX|4|NM|2965-2^SPECIFIC GRAVITY^LOINC||1.020||1.005-1.030|N|||F
OBX|5|ST|5804-0^PROTEIN^LOINC||Negative||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00243"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("BAKER"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("STEVEN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("51990-0"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("BASIC METABOLIC PANEL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("2345-7"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }
        }

        [Test]
        public void HL7_SAMPLE_244_Should_parse_ORU_R01_encapsulated_data_PDF()
        {
            const string message = @"MSH|^~\&|RAD|MAIN_HOSP|EMR|MAIN_HOSP|20240510090000||ORU^R01|MSG00244|P|2.5|||AL|NE
PID|1||MRN889900^^^MAIN_HOSP^MR||THOMPSON^JAMES^W||19720815|M
ORC|RE|ORD11223|RAD11223||CM
OBR|1|ORD11223|RAD11223|71046^CHEST X-RAY 2 VIEWS^CPT|||20240510083000|||||||||DR5566^WILSON^SARAH||||||20240510090000|||F
OBX|1|ED|71046^CHEST X-RAY REPORT^CPT||MAIN_HOSP^application^pdf^Base64^JVBERi0xLjQKMSAwIG9iago8PCAvVHlwZSAvQ2F0YWxvZyAvUGFnZXMgMiAwIFIgPj4KZW5kb2JqCjIgMCBvYmoKPDwgL1R5cGUgL1BhZ2VzIC9LaWRzIFszIDAgUl0gL0NvdW50IDEgPj4KZW5kb2Jq||||||F
OBX|2|FT|71046^CHEST X-RAY IMPRESSION^CPT||No acute cardiopulmonary disease\.br\Heart size is normal\.br\Lungs are clear bilaterally\.br\No pleural effusion or pneumothorax\.br\Bony structures are intact.||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00244"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("RAD"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("THOMPSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JAMES"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("71046"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CHEST X-RAY 2 VIEWS"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_245_Should_parse_ORU_R01_reference_pointer()
        {
            const string message = @"MSH|^~\&|RAD|MAIN_HOSP|EMR|MAIN_HOSP|20240510093000||ORU^R01|MSG00245|P|2.5|||AL|NE
PID|1||MRN889900^^^MAIN_HOSP^MR||THOMPSON^JAMES^W||19720815|M
ORC|RE|ORD11224|RAD11224||CM
OBR|1|ORD11224|RAD11224|71046^CHEST X-RAY 2 VIEWS^CPT|||20240510083000|||||||||DR5566^WILSON^SARAH||||||20240510093000|||F
OBX|1|RP|71046^CHEST X-RAY IMAGE^CPT||https://pacs.mainhosp.org/wado?studyUID=1.2.840.113619.2.55.3.2024.1&seriesUID=1.2.3.4&objectUID=1.2.3.4.5^PACS^IMAGE||||||F
OBX|2|ST|71046^CHEST X-RAY IMPRESSION^CPT||No acute findings. Normal chest radiograph.||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00245"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("RAD"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("THOMPSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JAMES"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("71046"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("CHEST X-RAY 2 VIEWS"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
            }
        }

        [Test]
        public void HL7_SAMPLE_246_Should_parse_DFT_P03_financial_transaction()
        {
            const string message = @"MSH|^~\&|BILLING|CLINIC_A|FINANCE|MAIN_HOSP|20240315160000||DFT^P03|MSG00246|P|2.4|||AL|NE
EVN|P03|20240315160000
PID|1||MRN112233^^^CLINIC_A^MR||GARCIA^ROSA^M||19780322|F|||456 PALM DR^^TAMPA^FL^33601||8135556677
PV1|1|O|CLINIC^A^01||||DR3344^HERNANDEZ^MIGUEL||||||||||V-2024-001
FT1|1|CHG-001||20240315|20240315|CG|99213^OFFICE VISIT LEVEL 3^CPT||1|||75.00||||||DR3344^HERNANDEZ^MIGUEL|||||I10^ESSENTIAL HYPERTENSION^ICD10
FT1|2|CHG-002||20240315|20240315|CG|93000^ECG COMPLETE^CPT||1|||45.00||||||DR3344^HERNANDEZ^MIGUEL|||||I10^ESSENTIAL HYPERTENSION^ICD10
PR1|1||99213^OFFICE VISIT LEVEL 3^CPT|OFFICE VISIT|20240315|||DR3344^HERNANDEZ^MIGUEL
PR1|2||93000^ECG COMPLETE^CPT|ECG|20240315|||DR3344^HERNANDEZ^MIGUEL
DG1|1||I10^ESSENTIAL HYPERTENSION^ICD10||20240315|A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("DFT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("P03"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00246"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("BILLING"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("GARCIA"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("ROSA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("P03"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
            {
                Assert.That(dg1Result.Result.DiagnosiCode.Value.Identifier.Value, Is.Not.Null.Or.Empty);
            }
        }

        [Test]
        public void HL7_SAMPLE_247_Should_parse_BAR_P05_update_account_dual_insurance()
        {
            const string message = @"MSH|^~\&|BILLING|MAIN_HOSP|FINANCE|MAIN_HOSP|20240401100000||BAR^P05|MSG00247|P|2.4|||AL|NE
EVN|P05|20240401100000
PID|1||MRN998800^^^MAIN_HOSP^MR||ADAMS^HENRY^J||19450812|M|||123 RETIREMENT LN^^SARASOTA^FL^34236||9415553344||||||||SSN123-45-6789
PV1|1|I|PULM^201^A||||DR6677^PATEL^SANJAY||||||||||V-2024-2244|||||||||||||||||||||||20240330
DG1|1||J44.1^COPD WITH ACUTE EXACERBATION^ICD10||20240330|A
IN1|1|MCR001^MEDICARE|MCR|MEDICARE|||||||||||ADAMS^HENRY^J|01^Self|19450812||||||||||||||||1EG4-TE5-MK72|||||MCR001
IN1|2|AARP001^AARP SUPPLEMENT|AARP|AARP SUPPLEMENTAL INSURANCE|||||||||||ADAMS^HENRY^J|01^Self|19450812||||||||||||||||AARP-998877|||||AARP001
IN2|1||||||||||||||||||||||||||||||||||||||||||||123-45-6789";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("BAR"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("P05"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00247"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("BILLING"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("ADAMS"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("HENRY"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("P05"));
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
        public void HL7_SAMPLE_248_Should_parse_VXU_V04_multi_vaccination()
        {
            const string message = @"MSH|^~\&|EHR|PEDS_CLINIC|IIS|STATE_REGISTRY|20240318100000||VXU^V04^VXU_V04|MSG00248|P|2.5.1|||ER|AL|||||Z23.9^IMMUNIZATION^HL7
PID|1||MRN334455^^^PEDS_CLINIC^MR||RODRIGUEZ^SOPHIA^M||20200115|F||2135-2^Hispanic^HL70005|789 SUNSHINE DR^^ORLANDO^FL^32801||4075551234||||||||||N
PD1||||DR8899^MARTINEZ^ANA^M^^MD^^NPI
NK1|1|RODRIGUEZ^MARIA^L|MTH^Mother^HL70063||4075551234
ORC|RE||IZ-001|||||||||DR8899^MARTINEZ^ANA^M^^MD^^NPI
RXA|0|1|20240318|20240318|20^DTaP^CVX|0.5|mL|IM^Intramuscular^HL70162|LA^Left Arm^HL70163|DR8899^MARTINEZ^ANA^M^^MD^^NPI||PMC^Sanofi Pasteur^MVX|||LOT-DTaP-2024A||20250601|A|20240318
RXR|IM^Intramuscular^HL70162|LA^Left Arm^HL70163
OBX|1|CE|30956-7^VACCINE TYPE^LN|1|20^DTaP^CVX||||||F
OBX|2|DT|29768-9^VIS PUBLICATION DATE^LN|1|20200801||||||F
OBX|3|DT|29769-7^VIS PRESENTATION DATE^LN|1|20240318||||||F
ORC|RE||IZ-002|||||||||DR8899^MARTINEZ^ANA^M^^MD^^NPI
RXA|0|1|20240318|20240318|10^IPV^CVX|0.5|mL|IM^Intramuscular^HL70162|RA^Right Arm^HL70163|DR8899^MARTINEZ^ANA^M^^MD^^NPI||PMC^Sanofi Pasteur^MVX|||LOT-IPV-2024B||20250901|A|20240318
RXR|IM^Intramuscular^HL70162|RA^Right Arm^HL70163
OBX|1|CE|30956-7^VACCINE TYPE^LN|1|10^IPV^CVX||||||F
ORC|RE||IZ-003|||||||||DR8899^MARTINEZ^ANA^M^^MD^^NPI
RXA|0|1|20240318|20240318|03^MMR^CVX|0.5|mL|SC^Subcutaneous^HL70162|LA^Left Arm^HL70163|DR8899^MARTINEZ^ANA^M^^MD^^NPI||MSD^Merck^MVX|||LOT-MMR-2024C||20251201|A|20240318
RXR|SC^Subcutaneous^HL70162|LA^Left Arm^HL70163
OBX|1|CE|30956-7^VACCINE TYPE^LN|1|03^MMR^CVX||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("VXU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("V04"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("VXU_V04"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00248"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("EHR"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("RODRIGUEZ"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("SOPHIA"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
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
        public void HL7_SAMPLE_249_Should_parse_MDM_T02_discharge_summary_document()
        {
            const string message = @"MSH|^~\&|TRANSCRIPTION|MAIN_HOSP|EMR|MAIN_HOSP|20240420150000||MDM^T02|MSG00249|P|2.5|||AL|NE
EVN|T02|20240420150000
PID|1||MRN667744^^^MAIN_HOSP^MR||WILLIAMS^FRANK^D||19600325|M|||345 VINE ST^^NASHVILLE^TN^37201||6155558899
PV1|1|I|MED^401^B||||DR2233^BROWN^ELIZABETH^A^^MD||||MED||||||||V-2024-6677|||||||||||||||||||||||20240415|20240420
TXA|1|DS^DISCHARGE SUMMARY|TX|20240420143000||20240420150000|||||DR2233^BROWN^ELIZABETH^A^^MD|||DOC-2024-3344|||AU^Authenticated|Y||DR2233^BROWN^ELIZABETH^A^^MD|20240420150000
OBX|1|TX|DS^DISCHARGE SUMMARY||DISCHARGE SUMMARY\.br\\.br\Patient: Frank D. Williams\.br\DOB: 03/25/1960\.br\Admission Date: 04/15/2024\.br\Discharge Date: 04/20/2024\.br\\.br\PRINCIPAL DIAGNOSIS: Community-acquired pneumonia\.br\\.br\HOSPITAL COURSE: Patient presented with 3-day history of productive cough, fever, and dyspnea. Chest X-ray confirmed right lower lobe pneumonia. Treated with IV antibiotics (ceftriaxone and azithromycin) with clinical improvement over 5 days. Converted to oral antibiotics for discharge.\.br\\.br\DISCHARGE MEDICATIONS:\.br\1. Amoxicillin-clavulanate 875mg PO BID x 7 days\.br\2. Acetaminophen 650mg PO Q6H PRN fever\.br\\.br\FOLLOW-UP: PCP in 1 week.||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("MDM"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("T02"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00249"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("TRANSCRIPTION"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("T02"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("WILLIAMS"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("FRANK"));
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
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_250_Should_parse_ORU_R01_continuation_pointer_DSC()
        {
            const string message = @"MSH|^~\&|LAB|MAIN_HOSP|EMR|MAIN_HOSP|20240601080000||ORU^R01|MSG00250|P|2.5|||AL|NE
PID|1||MRN001122^^^MAIN_HOSP^MR||JACKSON^MARK^T||19880901|M
ORC|RE|ORD-CONT-001|LAB-CONT-001||CM
OBR|1|ORD-CONT-001|LAB-CONT-001|57021-8^COMPREHENSIVE TOXICOLOGY PANEL^LOINC|||20240601070000|||||||||DR9900^TAYLOR^JAMES||||||20240601080000|||P
OBX|1|NM|3298-7^ETHANOL^LOINC||<10|mg/dL|<10|N|||P
OBX|2|ST|19659-2^AMPHETAMINES SCREEN^LOINC||Negative||Negative||||P
OBX|3|ST|19295-5^BENZODIAZEPINES SCREEN^LOINC||Negative||Negative||||P
DSC|CONT-TOX-001-PART2|F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("MSG00250"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("LAB"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("JACKSON"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MARK"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("57021-8"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("COMPREHENSIVE TOXICOLOGY PANEL"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("3298-7"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("P"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
            }
        }
    }
}
