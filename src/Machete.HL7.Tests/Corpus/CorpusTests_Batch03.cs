namespace Machete.HL7.Tests.Corpus
{
    using NUnit.Framework;
    using Machete.HL7.Testing;
    using Machete.HL7Schema.V26;

    [TestFixture]
    public class CorpusTests_Batch03 :
        HL7MacheteTestHarness<MSH, HL7V26Entity>
    {
        [Test]
        public void HL7_SAMPLE_051_Should_parse_ADT_A02_patient_transfer()
        {
            const string message = @"MSH|^~\&|AccMgr|1|||20050110114442||ADT^A02|59910287|P|2.3|||
EVN|A02|20050110114442||||||
PID|1||10006579^^^1^MRN^1||DUCK^DONALD^D||19241010|M||1|111^DUCK ST^^FOWL^CA^999990000^^M|1|8885551212|8885551212|1|2||40007716^^^AccMgr^VN^1|123121234|||||||||||NO
PV1|1|I|IN1^214^1^1^^^S|3||PREOP^101^|37^DISNEY^WALT^^^^^^AccMgr^^^^CI|||01||||1|||37^DISNEY^WALT^^^^^^AccMgr^^^^CI|2|40007716^^^AccMgr^VN|4|||||||||||||||||||1||I|||20050110045253||||||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A02"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A02"));
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
        public void HL7_SAMPLE_052_Should_parse_ADT_A03_discharge()
        {
            const string message = @"MSH|^~\&|AccMgr|1|||20050112154645||ADT^A03|59912415|P|2.3|||
EVN|A03|20050112154642||||||
PID|1||10006579^^^1^MRN^1||DUCK^DONALD^D||19241010|M||1|111^DUCK ST^^FOWL^CA^999990000^^M|1|8885551212|8885551212|1|2||40007716^^^AccMgr^VN^1|123121234|||||||||||NO
PV1|1|I|IN1^214^1^1^^^S|3||IN1^214^1|37^DISNEY^WALT^^^^^^AccMgr^^^^CI|||01||||1|||37^DISNEY^WALT^^^^^^AccMgr^^^^CI|2|40007716^^^AccMgr^VN|4||||||||||||||||1|||1||P|||20050110045253|20050112152000|3115.89|3115.89|||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A03"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A03"));
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
        public void HL7_SAMPLE_053_Should_parse_ADT_A11_cancel_admit()
        {
            const string message = @"MSH|^~\&|AccMgr|1|||20050112154645||ADT^A11|59912415|P|2.3|||
EVN|A03|20050112154642||||||
PID|1||10006579^^^1^MRN^1||DUCK^DONALD^D||19241010|M||1|111^DUCK ST^^FOWL^CA^999990000^^M|1|8885551212|8885551212|1|2||40007716^^^AccMgr^VN^1|123121234|||||||||||NO
PV1|1|I|IN1^214^1^1^^^S|3||IN1^214^1|37^DISNEY^WALT^^^^^^AccMgr^^^^CI|||01||||1|||37^DISNEY^WALT^^^^^^AccMgr^^^^CI|2|40007716^^^AccMgr^VN|4||||||||||||||||1|||1||P|||20050110045253|20050112152000|3115.89|3115.89|||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A11"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A03"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DUCK"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }
        }

        [Test]
        public void HL7_SAMPLE_054_Should_parse_ADT_A12_cancel_transfer()
        {
            const string message = @"MSH|^~\&|AccMgr|1|||20050110114442||ADT^A12|59910287|P|2.3|||
EVN|A02|20050110114442||||||
PID|1||10006579^^^1^MRN^1||DUCK^DONALD^D||19241010|M||1|111^DUCK ST^^FOWL^CA^999990000^^M|1|8885551212|8885551212|1|2||40007716^^^AccMgr^VN^1|123121234|||||||||||NO
PV1|1|I|IN1^214^1^1^^^S|3||IN1^214^1|37^DISNEY^WALT^^^^^^AccMgr^^^^CI|||01||||1|||37^DISNEY^WALT^^^^^^AccMgr^^^^CI|2|40007716^^^AccMgr^VN|4|||||||||||||||||||1||I|||20050110045253||||||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A12"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A02"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DUCK"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_055_Should_parse_ADT_A13_cancel_discharge()
        {
            const string message = @"MSH|^~\&|AccMgr|1|||20050110045504||ADT^A13|599102|P|2.3|||
EVN|A01|20050110045502||||||
PID|1||10006579^^^1^MRN^1||DUCK^DONALD^D||19241010|M||1|111 DUCK ST^^FOWL^CA^999990000^^M|1|8885551212|8885551212|1|2||40007716^^^AccMgr^VN^1|123121234|||||||||||NO
NK1|1|DUCK^HUEY|SO|3583 DUCK RD^^FOWL^CA^999990000|8885552222||Y||||||||||||||
PV1|1|I|PREOP^101^1^1^^^S|3|||37^DISNEY^WALT^^^^^^AccMgr^^^^CI|||01||||1|||37^DISNEY^WALT^^^^^^AccMgr^^^^CI|2|40007716^^^AccMgr^VN|4|||||||||||||||||||1||G|||20050110045253||||||
GT1|1|8291|DUCK^DONALD^D||111^DUCK ST^^FOWL^CA^999990000|8885551212||19241010|M||1|123121234||||#Cartoon Ducks Inc|111^DUCK ST^^FOWL^CA^999990000|8885551212||PT|
DG1|1|I9|71596^OSTEOARTHROS NOS-L/LEG ^I9|OSTEOARTHROS NOS-L/LEG ||A|
IN1|1|MEDICARE|3|MEDICARE|||||||Cartoon Ducks Inc|19891001|||4|DUCK^DONALD^D|1|19241010|111^DUCK ST^^FOWL^CA^999990000|||||||||||||||||123121234A||||||PT|M|111 DUCK ST^^FOWL^CA^999990000|||||8291
IN2|1||123121234|Cartoon Ducks Inc|||123121234A|||||||||||||||||||||||||||||||||||||||||||||||||||||||||8885551212
IN1|2|NON-PRIMARY|9|MEDICAL MUTUAL CALIF.|PO BOX 94776^^HOLLYWOOD^CA^441414776||8003621279|PUBSUMB|||Cartoon Ducks Inc||||7|DUCK^DONALD^D|1|19241010|111 DUCK ST^^FOWL^CA^999990000|||||||||||||||||056269770||||||PT|M|111^DUCK ST^^FOWL^CA^999990000|||||8291
IN2|2||123121234|Cartoon Ducks Inc||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||8885551212
IN1|3|SELF PAY|1|SELF PAY|||||||||||5||1";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A13"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A01"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DUCK"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("DONALD"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
            {
                Assert.That(dg1Result.Result.DiagnosiCode.Value.Identifier.Value, Is.Not.Null.Or.Empty);
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_056_Should_parse_ADT_A28_add_person_MPI()
        {
            const string message = @"MSH|^~\&|Regional MPI||Master MPI|Alpha Hospital|20060501140010||ADT^A28|3948375|P^T|2.4|||ER
EVN|A28|20060501140008|||000338475^Author^Arthur^^^^^^Regional MPI&2.16.840.1.113883.19.201&ISO^L|20060501140008
PID|||000197245^^^NationalPN&2.16.840.1.113883.19.3&ISO^PN~4532^^^CarefulCareClinic&2.16.840.1.113883.19.2.400566&ISO^PI~3242346^^^GoodmanGP&2.16.840.1.113883.19.2.450998&ISO^PI||Patient^Particia^^^^^L||19750103|F|||Randomroad 23a&Randomroad&23a^^Anytown^^1200^^H||555 3542557^ORN^PH~555 3542558^ORN^FX|555 5557865^WPN^PH
PV1||N|";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A28"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("Regional MPI"));
            Assert.That(mshResult.Result.SendingFacility.HasValue, Is.True);

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A28"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("Patient"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("Particia"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("N"));
            }
        }

        [Test]
        public void HL7_SAMPLE_057_Should_parse_ADT_A05_pre_admit()
        {
            const string message = @"MSH|^~\&|AccMgr|1|||20050110045504||ADT^A05|599102|P|2.3|||
EVN|A01|20050110045502|||||
PID|1||10006579^^^1^MRN^1||DUCK^DONALD^D||19241010|M||1|111 DUCK ST^^FOWL^CA^999990000^^M|1|8885551212|8885551212|1|2||40007716^^^AccMgr^VN^1|123121234|||||||||||NO
NK1|1|DUCK^HUEY|SO|3583 DUCK RD^^FOWL^CA^999990000|8885552222||Y||||||||||||||
PV1|1|I|PREOP^101^1^1^^^S|3|||37^DISNEY^WALT^^^^^^AccMgr^^^^CI|||01||||1|||37^DISNEY^WALT^^^^^^AccMgr^^^^CI|2|40007716^^^AccMgr^VN|4|||||||||||||||||||1||G|||20050110045253||||||
GT1|1|8291|DUCK^DONALD^D||111^DUCK ST^^FOWL^CA^999990000|8885551212||19241010|M||1|123121234||||#Cartoon Ducks Inc|111^DUCK ST^^FOWL^CA^999990000|8885551212||PT|
DG1|1|I9|71596^OSTEOARTHROS NOS-L/LEG ^I9|OSTEOARTHROS NOS-L/LEG ||A|
IN1|1|MEDICARE|3|MEDICARE|||||||Cartoon Ducks Inc|19891001|||4|DUCK^DONALD^D|1|19241010|111^DUCK ST^^FOWL^CA^999990000|||||||||||||||||123121234A||||||PT|M|111 DUCK ST^^FOWL^CA^999990000|||||8291
IN2|1||123121234|Cartoon Ducks Inc|||123121234A|||||||||||||||||||||||||||||||||||||||||||||||||||||||||8885551212
IN1|2|NON-PRIMARY|9|MEDICAL MUTUAL CALIF.|PO BOX 94776^^HOLLYWOOD^CA^441414776||8003621279|PUBSUMB|||Cartoon Ducks Inc||||7|DUCK^DONALD^D|1|19241010|111 DUCK ST^^FOWL^CA^999990000|||||||||||||||||056269770||||||PT|M|111^DUCK ST^^FOWL^CA^999990000|||||8291
IN2|2||123121234|Cartoon Ducks Inc||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||8885551212
IN1|3|SELF PAY|1|SELF PAY|||||||||||5||1";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A05"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("AccMgr"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A01"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DUCK"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("DONALD"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
            {
                Assert.That(dg1Result.Result.DiagnosiCode.Value.Identifier.Value, Is.Not.Null.Or.Empty);
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("I"));
            }
        }

        [Test]
        public void HL7_SAMPLE_058_Should_parse_ADT_A01_Wikipedia_sample()
        {
            const string message = @"MSH|^~\&|MegaReg|XYZHospC|SuperOE|XYZImgCtr|20060529090131-0500||ADT^A01^ADT_A01|01052901|P|2.5
EVN||200605290901||||200605290900
PID|||56782445^^^UAReg^PI||KLEINSAMPLE^BARRY^Q^JR||19620910|M||2028-9^^HL70005^RA99113^^XYZ|260 GOODWIN CREST DRIVE^^BIRMINGHAM^AL^35209^^M~NICKELL'S PICKLES^10000 W 100TH AVE^BIRMINGHAM^AL^35200^^O|||||||0105I30001^^^99DEF^AN
PV1||I|W^389^1^UABH^^^^3||||12345^MORGAN^REX^J^^^MD^0010^UAMC^L||67890^GRAINGER^LUCY^X^^^MD^0010^UAMC^L|MED|||||A0||13579^POTTER^SHERMAN^T^^^MD^0010^UAMC^L|||||||||||||||||||||||||||200605290900
OBX|1|N^K&M|^Body Height||1.80|m^Meter^ISO+|||||F
OBX|2|NM|^Body Weight||79|kg^Kilogram^ISO+|||||F
AL1|1||^ASPIRIN
DG1|1||786.50^CHEST PAIN, UNSPECIFIED^I9|||A";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ADT_A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("MegaReg"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("XYZHospC"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("KLEINSAMPLE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("BARRY"));
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
                Assert.That(al1Result.Result.AllergenTypeCode.HasValue, Is.True);
            }
        }

        [Test]
        public void HL7_SAMPLE_059_Should_parse_VXU_V04_immunization()
        {
            const string message = @"MSH|^~\&|Test EHR Application|X68||NIST Test Iz Reg|201207010822||VXU^V04^VXU_V04|NIST-IZ-020.00|P|2.5.1|||AL|ER
PID|1||252430^^^MAA^MR||Curry^Qiang^Trystan^^^^L||20090819|M
ORC|RE||IZ-783278^NDA|||||||||57422^RADON^NICHOLAS^^^^^^NDA^L
RXA|0|1|20120814||140^Influenza^CVX|0.5|mL^MilliLiter [SI Volume Units]^UCUM||00^New immunization record^NIP001||||||W1356FE|20121214|SKB^GlaxoSmithKline^MVX|||CP|A
RXR|C28161^Intramuscular^NCIT|RA^Right Arm^HL70163
OBX|1|CE|64994-7^Vaccine funding program eligibility category^LN|1|V03^VFC eligible - Uninsured^HL70064||||||F|||20120701|||VXC40^Eligibility captured at the immunization level^CDCPHINVS
OBX|2|CE|30956-7^vaccine type^LN|2|88^Influenza, unspecified formulation^CVX||||||F
OBX|3|TS|29768-9^Date vaccine information statement published^LN|2|20120702||||||F
OBX|4|TS|29769-7^Date vaccine information statement presented^LN|2|20120814||||||F
ORC|RE||IZ-783276^NDA
RXA|0|1|20110214||133^PCV 13^CVX|999|||01^Historical information - source unspecified^NIP001
ORC|RE||IZ-783282^NDA|||||||||57422^RADON^NICHOLAS^^^^^^NDA^L
RXA|0|1|20120814||110^DTaP-Hep B-IPV^CVX|0.5|mL^MilliLiter [SI Volume Units]^UCUM||00^New immunization record^NIP001||||||78HH34I|20121214|SKB^GlaxoSmithKline^MVX|||CP|A
RXR|C28161^Intramuscular^NCIT|LA^Left Arm^HL70163
OBX|1|CE|64994-7^Vaccine funding program eligibility category^LN|1|V03^VFC eligible - Uninsured^HL70064||||||F|||20120701|||VXC40^Eligibility captured at the immunization level^CDCPHINVS
OBX|2|CE|30956-7^vaccine type^LN|2|107^DTaP^CVX||||||F
OBX|3|TS|29768-9^Date vaccine information statement published^LN|2|20070517||||||F
OBX|4|TS|29769-7^Date vaccine information statement presented^LN|2|20120814||||||F
OBX|5|CE|30956-7^vaccine type^LN|3|89^Polio^CVX||||||F
OBX|6|TS|29768-9^Date vaccine information statement published^LN|3|20111108||||||F
OBX|7|TS|29769-7^Date vaccine information statement presented^LN|3|20120814||||||F
OBX|8|CE|30956-7^vaccine type^LN|4|45^Hep B, unspecified formulation^CVX||||||F
OBX|9|TS|29768-9^Date vaccine information statement published^LN|4|20120202||||||F
OBX|10|TS|29769-7^Date vaccine information statement presented^LN|4|20120814||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("VXU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("V04"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("VXU_V04"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("NIST-IZ-020.00"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("Curry"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("Qiang"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
            }
        }

        [Test]
        public void HL7_SAMPLE_060_Should_parse_ADT_A03_syndromic_surveillance()
        {
            const string message = @"MSH|^~\&||LakeMichMC^9879874000^NPI|||201204020040||ADT^A03^ADT_A03|NIST-SS-003.32|P|2.5.1|||||||||PH_SS-NoAck^SS Sender^2.16.840.1.114222.4.10.3^ISO
EVN||201204020030|||||LakeMichMC^9879874000^NPI
PID|1||33333^^^^MR||^^^^^^~^^^^^^S|||F||2106-3^^CDCREC|^^^^53217^^^^55089|||||||||||2186-5^^CDCREC
PV1|1||||||||||||||||||33333_001^^^^VN|||||||||||||||||09||||||||201204012130
DG1|1||0074^Cryptosporidiosis^I9CDX|||F
DG1|2||27651^Dehydration^I9CDX|||F
DG1|3||78791^Diarrhea^I9CDX|||F
OBX|1|CWE|SS003^^PHINQUESTION||261QE0002X^Emergency Care^NUCC||||||F
OBX|2|NM|21612-7^^LN||45|a^^UCUM|||||F
OBX|3|CWE|8661-1^^LN||^^^^^^^^Diarrhea, stomach pain, dehydration||||||F";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A03"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("ADT_A03"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5.1"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("NIST-SS-003.32"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }

            var dg1Result = parsed.Query(q => from msh in q.Select<MSH>() from dg1 in q.Select<DG1>() select dg1);
            if (dg1Result.HasResult)
            {
                Assert.That(dg1Result.Result.DiagnosiCode.HasValue, Is.True);
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_061_Should_parse_ADT_A01_Datica_tutorial()
        {
            const string message = @"MSH|^~\&|EPICADT|DH|LABADT|DH|201301011226||ADT^A01|HL7MSG00001|P|2.3|
EVN|A01|201301011223||
PID|||MRN12345^5^M11||APPLESEED^JOHN^A^III||19710101|M||C|1^DATICA STREET^^MADISON^WI^53005-1020|GL|(414)379-1212|(414)271-3434||S||MRN12345001^2^M10|123456789|987654^NC|
NK1|1|APPLESEED^BARBARA^J|WIFE||||||NK^NEXT OF KIN
PV1|1|I|2000^2012^01||||004777^GOOD^SIDNEY^J.|||SUR||||ADM|A0|";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("EPICADT"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("HL7MSG00001"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A01"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("APPLESEED"));
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
        public void HL7_SAMPLE_062_Should_parse_ACK_positive_acknowledgment()
        {
            const string message = @"MSH|^~\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110614075841||ACK|1407511|P|2.3||||||
MSA|AA|1407511|Success||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ACK"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("SENDING_APPLICATION"));
            Assert.That(mshResult.Result.SendingFacility.Value.NamespaceId.Value, Is.EqualTo("SENDING_FACILITY"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("1407511"));

            var msaResult = parsed.Query(q => from msh in q.Select<MSH>() from msa in q.Select<MSA>() select msa);
            if (msaResult.HasResult)
            {
                Assert.That(msaResult.Result.AcknowledgmentCode.Value, Is.EqualTo("AA"));
                Assert.That(msaResult.Result.MessageControlId.Value, Is.EqualTo("1407511"));
            }
        }

        [Test]
        public void HL7_SAMPLE_063_Should_parse_ADT_A01_with_Z_segment()
        {
            const string message = @"MSH|^~\&|SUNS1|OVI02|AZIS|CMD|200606221348||ADT^A01|1049691900|P|2.3
EVN|A01|200803051509||||200803031508
PID|||5520255^^^PK^PK~ZZZZZZ83M64Z148R^^^CF^CF~ZZZZZZ83M64Z148R^^^SSN^SSN^^20070103^99991231~^^^^TEAM||ZZZ^ZZZ||19830824|F||||||||||||||||||||||N
ZPV|Some Custom Notes|Additional custom description of the visit goes here";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("1049691900"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A01"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("ZZZ"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_064_Should_parse_MDM_T02_history_physical()
        {
            const string message = @"MSH|^~\&|MESA_OP|XYZ_HOSPITAL|iFW|ABC_HOSPITAL|20090130123||MDM^T02|20090130123|P|2.3|||NE
EVN|T02|200901301235
PID|||||DOE^JOHN||19920115|M||||||||||G83186|005729288
PV1||1|CE||||12345^WILLIS^LAYLA^H|||||||||||
TXA|1|HP^History & Physical|TX|200901151010|||||||||||||DO||AV
OBX|1|TX|||
OBX|2|TX|||
OBX|3|TX|||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("MDM"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("T02"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("MESA_OP"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JOHN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationResultStatus.HasValue, Is.False);
            }
        }

        [Test]
        public void HL7_SAMPLE_065_Should_parse_MDM_T02_viral_sinusitis()
        {
            const string message = @"MSH|^~\&|Epic|Epic|||20160510071633||MDM^T02|12345|D|2.3
PID|1||12983718^^^^EPI||Thomas^Mike^J^^MR.^||19500101|M||AfrAm|506 S. HAMILTON AVE^^MADISON^WI^53505^US^^^DN |DN|(608)123-9998|(608)123-5679||S||18273652|123-45-9999||||^^^WI^^
PV1|||^^^CARE HEALTH SYSTEMS^^^^^||||||1173^MATTHEWS^JAMES^A^^^||||||||||
TXA||CN||20160510071633|1173^MATTHEWS^JAMES^A^^^|||||||^^12345|||||PA
OBX|1|TX|||Clinical summary: Based on the information provided, the patient likely has viral sinusitis commonly called a head cold.
OBX|2|TX|||Diagnosis: Viral Sinusitis";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("MDM"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("T02"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("Epic"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("12345"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("Thomas"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("Mike"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
        }

        [Test]
        public void HL7_SAMPLE_066_Should_parse_MDM_T01_notification()
        {
            const string message = @"MSH|^~\&|MedOne|FACILITY A|CARECENTER^HL7NOTES|HFH|20060105180000|D61AFEF1-B10E-11D5-8666-0004ACD80749|MDM^T01|20060105180000999999|T|2.3
EVN|T01|20060105180000
PID|1||1112388^BS||ESPARZA^MARIA
PV1|1|O|BS^15^15
TXA|1|GENNOTES|TX|200601051800|50041^SMITH^CHRIS^M|200601051800|200601051800|200601051800|||SC^ROBINSON^JESSICA^A|1234567890||||FILE0001.TXT|PR";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("MDM"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("T01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("MedOne"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("T01"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("ESPARZA"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("MARIA"));
            }

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("O"));
            }
        }

        [Test]
        public void HL7_SAMPLE_067_Should_parse_SIU_S12_appointment_booking()
        {
            const string message = @"MSH|^~\&|HIS|RIH|EKG|EKG|20060529090131||SIU^S12|599102|P|2.3|
SCH|12345|67890|RIH|123|45|BOOKED|200605291200|200605291230|^^^ABC CLINIC|CARDIOLOGY CONSULT|
PID|1||123456||DOE^JOHN^A||19610615|M|||1234 MAIN ST^^PATOWN^CA^91455||(555)555-5555||(555)555-5556|||||999-99-9999|
RGS|1|
AIG|1|Equipment|||01|1|MACHINE^MRI SCANNER|
AIL|1|Location|Room|||01|1|^MRI ROOM^MAIN HOSPITAL|
AIP|1|Personnel|||01|1|1234^DOE^JANE^M.D.|";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("SIU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("S12"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("599102"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("DOE"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("JOHN"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }
        }

        [Test]
        public void HL7_SAMPLE_068_Should_parse_SIU_S12_Epic_radiology()
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
        public void HL7_SAMPLE_069_Should_parse_REF_I12_patient_referral()
        {
            const string message = @"MSH|^~\&|BLAH|Default Facility|||20100604104559||REF^I12^REF_I12|||2.5
SFT|BLAH|BLAH|BLAH|2010/06/04 10:44, branch : trunk
RF1||||||15|20100601000000
PRD|RP^Referring Provider|foo^doctor^^^DR|^^^^^^O||||999998
PRD|RT^Referred to Provider|moto^moto^^^r6|^^^^^^O||^^^^^^^^^^^411||8
PID|1||^^^^^^20100525^21000101^ON||aaa^aaa^^^^^L||19000101|M|||^^^ON^^^H
NTE|||asfd notes|^APPOINTMENT_NOTES
NTE|||engine oil problem, and maybe needs new plugs|^REASON_FOR_CONSULTATION
NTE|||86k km|^CLINICAL_INFORMATION
NTE|||goes too slow|^CONCURRENT_PROBLEMS
NTE|||91 octane|^CURRENT_MEDICATIONS
NTE|||scooters|^ALLERGIES";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("REF"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("I12"));
            Assert.That(mshResult.Result.MessageType.Value.MessageStructure.Value, Is.EqualTo("REF_I12"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.5"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("aaa"));
                Assert.That(pidResult.Result.AdministrativeSex.Value, Is.EqualTo("M"));
            }

            var nteResult = parsed.Query(q => from msh in q.Select<MSH>() from nte in q.Select<NTE>() select nte);
        }

        [Test]
        public void HL7_SAMPLE_070_Should_parse_ORU_R01_Salmonella_culture()
        {
            const string message = @"MSH|^~\&||Big Laboratory^33D0123456^CLIA|GEN2|NYSDOH|20060802101649||ORU^R01|200608021016491003|D|2.3
I|||13198751^^^^^Big Laboratory&33D0123456&CLIA||DREST^NATALIE^||19500101|F|||123 MAIN ST^^SPRINGFIELD^NY^12345||^^^^^518^1234567
OBR|1||13198751|^^^207252^CULTURE,SALMONELLA/SHIG^L|||200607280943|||||||20060729101650|STOOL-STOOL&STOOL-STOOL|^HERTZ, JOHN Q|^^^^^518^5551212||||||||F
ZLR|456 WASHINGTON BLVD^SUITE 100^ALBANY^NY^12345|HERTZ^JOHN^Q^^^MD|456 WASHINGTON BLVD^SUITE 100^ALBANY^NY^12345|^^^^^518^4567890
OBX|1|CE|^^^60101058^CULTURE,SALMONELLA/SHIGELLA,STOOL^L|1|^^^SASP^Salmonella sp., not typhi^L||||||F|||200608011318|33D0123456^Big Laboratory^CLIA";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));

            var obrResult = parsed.Query(q => from msh in q.Select<MSH>() from obr in q.Select<OBR>() select obr);
            if (obrResult.HasResult)
            {
                Assert.That(obrResult.Result.UniversalServiceIdentifier.HasValue, Is.True);
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_071_Should_parse_ORU_R01_non_standard_delimiters()
        {
            const string message = @"MSH^~|\&^HDRVTLS^552~DAYTDEV.FO-BAYPINES.MED.VA.GOV~DNS^GMRV VDEF IESIDE^200HD~HDR.MED.VA.GOV~DNS^20061006151615-0800^^ORU~R01^55253408603^T^2.4^^^AL^NE^US
ORC^RE^^6240020~552_120.5^^^^^^^^^^OBS23~325~~~~~~~23 HOUR OBSERVATION^^^^552~DAYTON~L^^^^DAYTON
OBR^^^6240020~552_120.5^4500635~PAIN~99VA120.51^^^200610061445-0800^20061006144607-0800^^^^^^^^^^^^^^20061006144607-0800^^^E^^^^^^^^^123456&NURSE&THREE&A&III&MS&RN&VistA200
OBX^^ST^4500635~PAIN~99VA120.51^^3^~~L^^^^^W^^^^^123456&NURSE&THREE&A&III&MS&RN&VistA200
OBX^^CE^Error Reasons^^4500627~INCORRECT READING~99VA8985.1^^^^^^W^^^^^123456&NURSE&THREE&A&III&MS&RN&VistA200
ZSC^^291^OBSERVATION SURGERY";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.4"));

            var orcResult = parsed.Query(q => from msh in q.Select<MSH>() from orc in q.Select<ORC>() select orc);
            if (orcResult.HasResult)
            {
                Assert.That(orcResult.Result.OrderControl.Value, Is.EqualTo("RE"));
            }
        }

        [Test]
        public void HL7_SAMPLE_072_Should_parse_ADT_A17_swap_patients_with_Z_segments()
        {
            const string message = @"MSH|^~\&|ULTRA|TML|TML||200903120021||ADT^A17|66239404|T|2.3.1
EVN|A17|201002130003||||201002130003|G^4265^L
PID|1
ZZA|ZZAVALUE
ZZA|ZZAVALUE
ZZB|ZZBVALUE
ZZB|ZZBVALUE
PV1||E
PID|2
ZZA|ZZAVALUE
ZZA|ZZAVALUE
ZZB|ZZBVALUE
ZZB|ZZBVALUE
ZZC|ZZCVALUE";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ADT"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("A17"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3.1"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("66239404"));

            var evnResult = parsed.Query(q => from msh in q.Select<MSH>() from evn in q.Select<EVN>() select evn);
            if (evnResult.HasResult)
            {
                Assert.That(evnResult.Result.EventTypeCode.Value, Is.EqualTo("A17"));
            }

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);

            var pv1Result = parsed.Query(q => from msh in q.Select<MSH>() from pv1 in q.Select<PV1>() select pv1);
            if (pv1Result.HasResult)
            {
                Assert.That(pv1Result.Result.PatientClass.Value, Is.EqualTo("E"));
            }
        }

        [Test]
        public void HL7_SAMPLE_073_Should_parse_ORU_R01_device_implant_date()
        {
            const string message = @"MSH|^~\&|ULTRA|TML|OLIS|OLIS|200905011130||ORU^R01|20169838|T|2.3
ZPI|||7005728^^^TML^MR||TEST^RACHEL^DIAMOND||19310313|F|||200 ANYWHERE ST^^TORONTO^ON^M6G 2T9||(416)888-8888||||||1014071185^KR
PID|||7005728^^^TML^MR||TEST^RACHEL^DIAMOND||19310313|F|||200 ANYWHERE ST^^TORONTO^ON^M6G 2T9||(416)888-8888||||||1014071185^KR
PV1|1||OLIS||||OLIST^BLAKE^DONALD^THOR^^^^^921379^^^^OLIST
ORC|RE||T09-100442-RET-0^^OLIS_Site_ID^ISO|||||||||OLIST^BLAKE^DONALD^THOR^^^^L^921379
OBR|0||T09-100442-RET-0^^OLIS_Site_ID^ISO|RET^RETICULOCYTE COUNT^HL79901 literal|||200905011106|||||||200905011106||OLIST^BLAKE^DONALD^THOR^^^^L^921379||7870279|7870279|T09-100442|MOHLTC|200905011130||B7|F||1^^^200905011106^^R
OBX|8|DT|GDT-00108^Device Implant Date^GDT-LATITUDE||20090505||||||F||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("ORU"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("R01"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("20169838"));

            var pidResult = parsed.Query(q => from msh in q.Select<MSH>() from pid in q.Select<PID>() select pid);
            if (pidResult.HasResult)
            {
                Assert.That(pidResult.Result.PatientName[0].Value.FamilyName.Value.Surname.Value, Is.EqualTo("TEST"));
                Assert.That(pidResult.Result.PatientName[0].Value.GivenName.Value, Is.EqualTo("RACHEL"));
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
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Identifier.Value, Is.EqualTo("RET"));
                Assert.That(obrResult.Result.UniversalServiceIdentifier.Value.Text.Value, Is.EqualTo("RETICULOCYTE COUNT"));
            }

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
            if (obxResult.HasResult)
            {
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Identifier.Value, Is.EqualTo("GDT-00108"));
                Assert.That(obxResult.Result.ObservationIdentifier.Value.Text.Value, Is.EqualTo("Device Implant Date"));
                Assert.That(obxResult.Result.ObservationResultStatus.Value, Is.EqualTo("F"));
            }
        }

        [Test]
        public void HL7_SAMPLE_074_Should_parse_OBX_with_ampersand_in_value()
        {
            const string message = @"MSH|^~\&|
OBX||ST|||Blood&Plasma - Bio (Green)||||||||||";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);

            var obxResult = parsed.Query(q => from msh in q.Select<MSH>() from obx in q.Select<OBX>() select obx);
        }

        [Test]
        public void HL7_SAMPLE_075_Should_parse_MFN_M13_master_file_notification()
        {
            const string message = @"MSH|^~\&|PM|PM|||20240220083617||MFN^M13|934576120110613083617|P|2.3
MFI|EMP||UPD|20180122161758||NE
MFE|MUP||20180122161758|1
ZEM|1^1|Better Corp. UPDATE|123 Test Rd^suite 100^Cleveland^OH^44128^US|Policy Holder Only|5555555555";

            var parsed = Parser.Parse(CleanupText(message));
            Assert.That(parsed.HasResult, Is.True);

            var mshResult = parsed.Query(q => from msh in q.Select<MSH>() select msh);
            Assert.That(mshResult.HasResult, Is.True);
            Assert.That(mshResult.Result.MessageType.Value.MessageCode.Value, Is.EqualTo("MFN"));
            Assert.That(mshResult.Result.MessageType.Value.TriggerEvent.Value, Is.EqualTo("M13"));
            Assert.That(mshResult.Result.VersionId.Value.VersionId.Value, Is.EqualTo("2.3"));
            Assert.That(mshResult.Result.SendingApplication.Value.NamespaceId.Value, Is.EqualTo("PM"));
            Assert.That(mshResult.Result.MessageControlId.Value, Is.EqualTo("934576120110613083617"));
        }
    }
}
