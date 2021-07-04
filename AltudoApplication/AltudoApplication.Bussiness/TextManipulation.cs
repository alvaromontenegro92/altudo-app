using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltudoApplication.Business
{
    public class TextManipulation
    {
        public List<object[]> GetTOP10Words(Uri url, string sourceCode)
        {
            var htmlTextContent = new TextExtractor();

            var text = htmlTextContent.ExtractTextFromSourceCode(sourceCode);

            var words = SplitTextIntoWords(text);
            GroupWords(ref words);
            OrderWords(ref words);
            FilterTop10Words(ref words);

            return words;
        }

        private List<object[]> SplitTextIntoWords(string text)
        {
            var words = text
                .Split(" ")
                .Select(word => new object[] { word, 1 })
                .ToList();

            return words;
        }

        private void GroupWords(ref List<object[]> words)
        {
            words =  words.GroupBy(word => word[0].ToString())
             .Select(x => new object[]
             {
                 x.Key,
                 x.Sum(y => (int)y[1])
             })
             .ToList();
        }

        private void OrderWords(ref List<object[]> words)
        {
           words = words.OrderByDescending(word => word[1])
                .ToList();
        }

        private void FilterTop10Words(ref List<object[]> words)
        {
            words = words
                .Where(word => word[0].ToString() != "")
                .ToList()
                .GetRange(0, 10)
                .ToList();
        }
    }
}
