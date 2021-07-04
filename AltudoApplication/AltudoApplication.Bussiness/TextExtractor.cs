using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace AltudoApplication.Business
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
        public string ExtractTextFromWebSite(Uri url)
        {
            var sourceCode = GetSourceCodeFromWebSite(url.AbsoluteUri);     

            var content = FilterSourceCode(sourceCode);

            var textFound = ExtractTextContent(content);

            return textFound;
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
        private string FilterSourceCode(string content)
        {
            content = RemoveTags(content, _scriptOpenTag, _scriptCloseTag);
            content = RemoveTags(content, _styleOpenTag, _styleCloseTag);
            return RemoveTags(content, _commentOpenTag, _commentCloseTag);
        }
        private string RemoveTags(string text, string openTag, string closeTag)
        {
            var execute = true;

            while (execute)
            {
                if (!text.Contains(openTag))
                    break;

                var initialPosition = text.IndexOf(openTag, 0);

                if (initialPosition.Equals(-1))
                    break;

                var finalPosition = text.IndexOf(closeTag, initialPosition) + closeTag.Length;

                if (finalPosition.Equals(-1))
                    break;

                text = text.Remove(initialPosition, finalPosition - initialPosition);                               
            }

            return text;
        }
        private string ExtractTextContent(string sourceCode)
        {
            var finalPosition = 0;
            var execute = true;
            var text = String.Empty;

            while (execute)
            {
                if (sourceCode.Contains(_initialTag))
                {
                    int initialPosition = sourceCode.IndexOf(_initialTag, finalPosition);

                    if (initialPosition.Equals(-1))
                        break;
                    else
                        initialPosition += _initialTag.Length;

                    finalPosition = sourceCode.IndexOf(_finalTag, initialPosition);

                    if (finalPosition.Equals(-1))
                        break;

                    var sentence = sourceCode
                        .Substring(initialPosition, finalPosition - initialPosition);

                    sentence = Regex.Replace(sentence, @"[\n\r\t]", "");
                    sentence = WebUtility.HtmlDecode(sentence);

                    if (!String.IsNullOrEmpty(sentence)
                        && !String.IsNullOrWhiteSpace(sentence))
                         text+= $" {sentence}";
                }
            }

            return text;
        }        
    }
}
