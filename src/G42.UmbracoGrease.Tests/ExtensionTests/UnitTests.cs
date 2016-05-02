using System.Web;
using G42.UmbracoGrease.Extensions;
using NUnit.Framework;
using Umbraco.Web;

namespace G42.UmbracoGrease.Tests.ExtensionTests
{
    [Category("Extensions")]
    [TestFixture]
    public class UnitTests
    {
        [TestCase("Jon Snow is Dead!", "Jon Snow is Dead", ' ')]
        [TestCase("Jon Snow is n@t Dead!", "Jon Snow is n_t Dead", '_')]
        public void Can_Make_Examine_Friendly(string input, string expectedOutput, char replacementCharacter)
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

        [TestCase(1, "1st")]
        [TestCase(2, "2nd")]
        [TestCase(3, "3rd")]
        [TestCase(4, "4th")]
        [TestCase(5, "5th")]
        [TestCase(6, "6th")]
        [TestCase(7, "7th")]
        [TestCase(8, "8th")]
        [TestCase(9, "9th")]
        [TestCase(10, "10th")]

        public void Can_Make_Ordinals(int input, string expectedOutput)
        {
            var result = input.ToOrdinal();

            Assert.AreEqual(expectedOutput, result);
        }

        [TestCase("The quick brown fox jumped over the lazy dog!", "The quick brown fox jumped over the lazy dog!", 100, null)]
        [TestCase("The quick brown fox jumped over the lazy dog!", "The quick…", 10, null)]
        [TestCase("The quick brown fox jumped over the lazy dog!", "The...", 10, "...")]
        public void Can_Truncate_At_Word(string input, string expectedOutput, int maxCharacters, string suffix)
        {
            var result = "";

            if (string.IsNullOrEmpty(suffix))
            {
                result = input.TruncateAtWord(maxCharacters);
            }
            else
            {
                result = input.TruncateAtWord(maxCharacters, suffix);
            }

            Assert.AreEqual(expectedOutput, result);
        }

        [TestCase("The quick brown fox jumped over the lazy dog!", "dog", "The quick brown fox jumped over the lazy <strong>dog</strong>!")]
        [TestCase("The quick brown fox jumped over the lazy dog!", "dog fox", "The quick brown <strong>fox</strong> jumped over the lazy <strong>dog</strong>!")]
        public void Can_Hightlight_Words(string input, string query, string expectedOutput)
        {
            var result = input.HighlightKeywords(query);

            Assert.AreEqual(expectedOutput, result);
        }

        [TestCase("foo", "foo")]
        [TestCase("foo&bar", "<![CDATA[foo&bar]]>")]
        [TestCase("!@#$%^", "!@#$%^")]
        public void Can_Make_An_Xml_Safe_String(string input, string expectedOutput)
        {
            var result = input.ToXmlSafeString();

            Assert.AreEqual(expectedOutput, result);
        }

        [TestCase("http://foo.local", "https://foo.local")]
        [TestCase("https://foo.local", "https://foo.local")]
        public void Can_Make_Https_Url(string input, string expectedOutput)
        {
            var result = input.ToHttpsUrl();

            Assert.AreEqual(expectedOutput, result);
        }

        [TestCase("http://foo.local/bar.jpg", "http://foo.local/bar.jpg?mode=pad", false)]
        [TestCase("https://foo.local/bar.jpg", "https://foo.local/bar.jpg?mode=pad", true)]
        [TestCase("https://foo.com/bar.jpg", "https://foo.com/remote.axd/foo.com/bar.jpg?mode=pad", true)]
        public void Can_Transform_To_Azure_Blob_Url(string input, string expectedOutput, bool useSameProtocolAsRequest)
        {
            var context = new HttpContext(new HttpRequest(null, input, null), new HttpResponse(null));

            var result = input.GetCropUrl().ToAzureBlobUrl(useSameProtocolAsRequest, context);

            Assert.AreEqual(expectedOutput, result);
        }
    }
}
