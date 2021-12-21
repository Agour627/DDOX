using BarcodeLib;
using DDOX.API.Core.Models.File;
using Microsoft.AspNetCore.Http;
using BarcodeLib.BarcodeReader;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDOX.API.Core.Models;
using DDOX.API.Infrastructure.Enums;
using Serilog;

namespace DDOX.API.Core.Helpers
{
    public class ColorCount
    {
        public Color Color { get; set; }
        public int Count { get; set; }
    }

    public static class SeperatorSettings
    {


        public static Func<IFormFile, Seperator, int, bool> IsSeperator = delegate (IFormFile file,
                                                                                    Seperator seperator,
                                                                                    int filesCount)
          {
              switch (seperator.SeperatorType)
              {
                  case SeperatorType.BlanckPage:
                      return IsBlankPage(file);

                  case SeperatorType.Barcode:
                      return IsBarCode(file);

                  case SeperatorType.PageNumber:
                      return filesCount == seperator.PagesNumber;

                  default:
                      return false;
              }
          };

        public static FileToDownload GenerateBarcode()
        {
            try
            {
                Barcode barcode = new Barcode();
                Image image = barcode.Encode(TYPE.CODE128, "DMSIlead", Color.Black, Color.White, 250, 100);
                var memoryStream = new MemoryStream();
                image.Save(memoryStream, ImageFormat.Png);
                var fileToDownload = new FileToDownload
                {
                    Stream = memoryStream,
                    FileName = "seperator.png",
                    MimeType = "image/jpeg"

                };
                Log.Information("The Barcode have been created successfully");
                return fileToDownload;
            }
            catch (Exception ex)
            {
                Log.Warning(ex, ex.Message);
                return null;
            }
        }

        public static bool IsBarCode(IFormFile barCodeImage)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    barCodeImage.CopyTo(memoryStream);
                    var barCodeList = BarcodeReader.read(memoryStream, BarcodeReader.CODE128);
                    if (barCodeList != null && barCodeList.Length > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch(Exception ex)
            {
                Log.Warning(ex, ex.Message);
                return false;
            }
        }

        public static bool IsBlankPage(IFormFile file)
        {
            try
            {
                var noiseThreshold = 10;
                var blankThreshold = 99.99;

                var memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                var bitmap = new Bitmap(Image.FromStream(memoryStream));

                var colorsFrequency = GetPixels(bitmap)
                                     .GroupBy(color => color)
                                     .Select(grp => new ColorCount
                                     {
                                         Color = grp.Key,
                                         Count = grp.Count()
                                     })
                                   .OrderByDescending(x => x.Count).ToList();

                if (colorsFrequency.Count > 0)
                {
                    Color dominantColor = colorsFrequency[0].Color;
                    var numberOfPixels = colorsFrequency.Sum(w => w.Count);
                    var numberOfDominantPixels = NoiseReduction(colorsFrequency, dominantColor, noiseThreshold);

                    var averageMostColor = ((double)numberOfDominantPixels / (double)numberOfPixels) * 100;
                    return averageMostColor >= blankThreshold;
                }
                return false;
            }
            catch(Exception ex)
            {
                Log.Warning(ex, ex.Message);
                return false;
            }
        }

        private static int NoiseReduction(List<ColorCount> colorsFrequency, Color dominantColor, int threshold)
        {
            int frequency = 0;
            foreach (var color in colorsFrequency)
            {
                if (Math.Abs((int)dominantColor.R - (int)color.Color.R) < threshold &&
                    Math.Abs((int)dominantColor.G - (int)color.Color.G) < threshold &&
                    Math.Abs((int)dominantColor.B - (int)color.Color.B) < threshold)
                {
                    frequency += color.Count;
                }
            }

            return frequency;
        }
        private static IEnumerable<Color> GetPixels(Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    yield return pixel;
                }
            }
        }

    }
}
