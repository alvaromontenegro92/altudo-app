using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace AltudoApplication.Bussiness
{
    public class TextExtractor
    {
        private const string _scriptOpenTag = "<script";
        private const string _scriptCloseTag = "</script>";
        private const string _styleOpenTag = "<style";
        private const string _styleCloseTag = "</style>";
        private const string _commentOpenTag = "<!-";
        private const string _commentCloseTag = "-->";
        private const string _initialTag = ">";
        private const string _finalTag = "<";
        public List<string> ExtractTextFromWebSite(Uri url)
        {
            var sourceCode = GetSourceCodeFromWebSite(url.AbsoluteUri);
            var imagesExtension = GetImageExtensionsAllowed();            

            var textFound = new List<string>();

            sourceCode = RemoveContent(sourceCode, _scriptOpenTag, _scriptCloseTag);
            sourceCode = RemoveContent(sourceCode, _styleOpenTag, _styleCloseTag);
            sourceCode = RemoveContent(sourceCode, _commentOpenTag, _commentCloseTag);

            textFound = ExtractContent(sourceCode);

            return textFound;
        }

        private string RemoveContent(string text, string openTag, string closeTag)
        {
            var initialPosition = 0;
            var finalPosition = 0;
            var execute = true;

            while (execute)
            {
                if (!text.Contains(openTag))
                    break;

                initialPosition = text.IndexOf(openTag, 0);

                if (initialPosition.Equals(-1))
                    break;

                finalPosition = text.IndexOf(closeTag, initialPosition) + closeTag.Length;

                if (finalPosition.Equals(-1))
                    break;

                text = text.Remove(initialPosition, finalPosition - initialPosition);                               
            }

            return text;
        }

        private List<string> ExtractContent(string sourceCode)
        {
            var initialPosition = 0;
            var finalPosition = 0;
            var lastPositionFound = 0;
            var execute = true;
            var text = new List<string>();

            while (execute)
            {
                if (sourceCode.Contains(_initialTag))
                {
                    initialPosition = sourceCode.IndexOf(_initialTag, lastPositionFound);

                    if (initialPosition.Equals(-1))
                        break;
                    else
                        initialPosition += _initialTag.Length;

                    finalPosition = sourceCode.IndexOf(_finalTag, initialPosition);
                    lastPositionFound = finalPosition;

                    if (lastPositionFound.Equals(-1))
                        break;

                    var sentence = sourceCode
                        .Substring(initialPosition, finalPosition - initialPosition);

                    sentence = Regex.Replace(sentence, @"[\n\r\t]", "");
                    sentence = WebUtility.HtmlDecode(sentence);

                    if (!String.IsNullOrEmpty(sentence)
                        && !String.IsNullOrWhiteSpace(sentence))
                        text.Add(sentence);
                }
            }

            return text;
        }
        private string GetSourceCodeFromWebSite(string url)
        {
            var result = String.Empty;

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        result = content.ReadAsStringAsync().Result;
                    }
                }
            }

            return result;
        }



        private List<string> GetImageExtensionsAllowed()
        {
            return new List<string>(new string[] { 
                ".png", 
                ".jpg", 
                "jpge", 
                ".bmp", 
                ".svg" });
        }
    }
}
