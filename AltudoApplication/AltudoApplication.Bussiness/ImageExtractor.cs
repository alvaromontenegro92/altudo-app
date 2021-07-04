using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace AltudoApplication.Business
{
    public class ImageExtractor
    {
        private const string _initialCharacter = "src=\"";
        private const string _finalCharacter = "\"";
        private string[] _imagesExtension = {".png", ".jpg", ".jpge", ".bmp", ".svg", ".tiff", ".gif", ".avif", ".exif", ".raw", ".webp" };
        public List<Uri> ExtractImagesFromSourceCode(Uri url, string sourceCode)
        {            
            var imagesExtension = GetImageExtensionsAllowed();
            var imagesAddress = GetImageAddress(url, sourceCode, imagesExtension);

            return imagesAddress;
        }
        private List<string> GetImageExtensionsAllowed()
        {
            return new List<string>(_imagesExtension);
        }

        private List<Uri> GetImageAddress(Uri url, string sourceCode, List<string> imagesExtension)
        {
            var find = true;
            var initialPosition = 0;
            var finalPosition = 0;
            var lastPositionFound = 0;
            var imagesAddressFound = new List<Uri>();

            while (find)
            {
                if (sourceCode.Contains(_initialCharacter))
                {
                    initialPosition = sourceCode.IndexOf(_initialCharacter, lastPositionFound);

                    if (initialPosition.Equals(-1))
                        break;
                    else
                        initialPosition += _initialCharacter.Length;

                    finalPosition = sourceCode.IndexOf(_finalCharacter, initialPosition);

                    if (finalPosition.Equals(-1))
                        break;

                    lastPositionFound = finalPosition;

                    var image = sourceCode.Substring(initialPosition, finalPosition - initialPosition);

                    var fullPathImage = new Uri(url, image);

                    var imageExtension = Path.GetExtension(fullPathImage.AbsolutePath);

                    if (imagesExtension.Contains(imageExtension)
                        && !imagesAddressFound.Exists(image => image.AbsolutePath == fullPathImage.AbsolutePath))
                        imagesAddressFound.Add(fullPathImage);
                }
            }

            return imagesAddressFound;
        }
    }
}
