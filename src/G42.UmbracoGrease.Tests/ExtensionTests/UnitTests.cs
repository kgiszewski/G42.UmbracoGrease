using G42.UmbracoGrease.Extensions;
using NUnit.Framework;

namespace G42.UmbracoGrease.Tests.ExtensionTests
{
    [Category("Extensions")]
    [TestFixture]
    public class UnitTests
    {
        [TestCase("Jon Snow is Dead!", "Jon Snow is Dead", " ")]
        [TestCase("Jon Snow is n@t Dead!", "Jon Snow is nt Dead", "")]
        [TestCase("Jon Snow is n@t Dead!", "Jon Snow is n_t Dead_", "_")]
        public void Can_Make_Examine_Friendly(string input, string expectedOutput, string replacementCharacter)
        {
            var result = input.ToExamineFriendly(replacementCharacter);

            Assert.AreEqual(expectedOutput, result);
        }

        [TestCase(1, "1 B")]
        [TestCase(1024, "1 KB")]
        [TestCase(1048576, "1 MB")]
        [TestCase(1073741824, "1 GB")]
        [TestCase(262144000, "250 MB")]
        public void Can_Make_Human_Readable_Bytes(int input, string expectedOutput)
        {
            var result = input.ToHumanReadableBytes();

            Assert.AreEqual(expectedOutput, result);
        }
    }
}
