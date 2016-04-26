using G42.UmbracoGrease.Core;
using NUnit.Framework;

namespace G42.UmbracoGrease.Tests.G42404Tests
{
    [TestFixture]
    public class FunctionalTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            PetaPocoUnitOfWork.ConnectionString = "testDb";

            Grease.Services.G42404Service.Create404TrackerTable();
            Grease.Services.G42404Service.Create404DomainPathsTable();
        }

        [Test]
        public void Can_Add_404()
        {
            
        }
    }
}
