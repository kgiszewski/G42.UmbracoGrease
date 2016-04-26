using System;
using System.Linq;
using G42.UmbracoGrease.Core;
using NUnit.Framework;

namespace G42.UmbracoGrease.Tests.G42404Tests
{
    [Category("404")]
    [TestFixture]
    public class FunctionalTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            PetaPocoUnitOfWork.ConnectionString = "testDb";

            Grease.Services.G42404Service.Create404TrackerTable();
            Grease.Services.G42404Service.Create404DomainPathsTable();

            using (var uow = new PetaPocoUnitOfWork())
            {
                uow.Database.Execute(@"TRUNCATE TABLE G42Grease404Tracker");
                uow.Database.Execute(@"TRUNCATE TABLE G42Grease404TrackerDomainPaths");

                uow.Commit();
            }
        }

        [TestCase("http://test.local", "/foo", "http://google.com", "TestAgent")]
        [TestCase("http://test.local", "/foo2", "http://google.com", "TestAgent")]
        [TestCase("http://test.local", "/foo2", "http://google.com", "TestAgent2")]
        [TestCase("http://test.local", "/foo2", "http://google.com", "TestAgent3")]
        public void Can_Add_404(string domain, string path, string referrer, string agent)
        {
            Console.WriteLine("Testing...");

            var before = Grease.Services.G42404Service.GetResults(1)
                            .FirstOrDefault(x => x.Domain == domain && x.Path == path);

            var beforeCount = 0;

            if (before != null)
            {
                beforeCount = before.Count;
            }

            Grease.Services.G42404Service.Add(domain, path, referrer, agent);

            var after = Grease.Services.G42404Service.GetResults(1)
                            .FirstOrDefault(x => x.Domain == domain && x.Path == path);

            Assert.AreEqual(beforeCount + 1, after.Count);
        }
    }
}
