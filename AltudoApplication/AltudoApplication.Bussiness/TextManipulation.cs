using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltudoApplication.Business
{
    public class TextManipulation
    {
        public List<object[]> Execute(Uri url)
        {
            var htmlContent = new TextExtractor();

            var text = htmlContent.ExtractTextFromWebSite(url);

            var words = SplitText(text);
            words = GroupElements(words);
            words = OrderElements(words);
            words = FilterElements(words);

            return words;
        }

        private List<object[]> SplitText(string text)
        {
            var words = text
                .Split(" ")
                .Select(word => new object[] { word, 1 })
                .ToList();

            return words;
        }

        private List<object[]> GroupElements(List<object[]> words)
        {
            return words.GroupBy(word => word[0].ToString())
             .Select(x => new object[]
             {
                 x.Key,
                 x.Sum(y => (int)y[1])
             })
             .ToList();
        }

        private List<object[]> OrderElements(List<object[]> words)
        {
           return words.OrderByDescending(word => word[1])
                .ToList();
        }

        private List<object[]> FilterElements(List<object[]> words)
        {
            return words.GetRange(0, 10)
                 .ToList();
        }
    }
}
