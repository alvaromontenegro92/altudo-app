using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace AltudoApplication.Business
{
    public class ImageExtractor
    {
        public List<Uri> ExtractImagesFromWebSite(Uri url)
        {
            var sourceCode = GetSourceCodeFromWebSite(url.AbsoluteUri);
            var imagesExtension = GetImageExtensionsAllowed();

            var initialCharacter = "src=\"";
            var finalCharacter = "\"";
            var initialPosition = 0;
            var finalPosition = 0;
            var lastPositionFound = 0;

            var imagesFound = new List<Uri>();            
            
            while (1 == 1)
            {
                if (sourceCode.Contains(initialCharacter))
                {
                    initialPosition = sourceCode.IndexOf(initialCharacter, lastPositionFound);

                    if (initialPosition.Equals(-1))
                        break;
                    else
                        initialPosition += initialCharacter.Length;

                    finalPosition = sourceCode.IndexOf(finalCharacter, initialPosition);
                    lastPositionFound = finalPosition;

                    var image = sourceCode.Substring(initialPosition, finalPosition - initialPosition);

                    var fullPathImage = new Uri(url, image);

                    var imageExtension = Path.GetExtension(fullPathImage.AbsoluteUri);

                    if (imagesExtension.Contains(imageExtension)
                        && !imagesFound.Contains(fullPathImage))
                        imagesFound.Add(fullPathImage);
                }
            }

            return imagesFound;
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
