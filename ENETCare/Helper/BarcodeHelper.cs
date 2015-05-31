using System;
using System.Drawing;
using System.Security.Cryptography;
using Aspose.BarCode;

namespace ENETCare.Business
{
	/// <summary>
	/// Helper class to generate barcode numbers and images
	/// </summary>
	public class BarcodeHelper
	{
		/// <summary>
		/// Generates a unique package barcode number.
		/// </summary>
		/// <returns>barcode number</returns>
		public static string GenerateBarcode()
		{
			return Get8Digits();
		}

		/// <summary>
		/// Generates a eight digits random number
		/// </summary>
		/// <returns>a eight digits number</returns>
		public static string Get8Digits()
		{
			var bytes = new byte[4];
			var generator = RandomNumberGenerator.Create();
			generator.GetBytes(bytes);
			uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
			return String.Format("{0:D8}", random);
		}

		/// <summary>
		/// Generates a barcode image.
		/// </summary>
		/// <param name="barcode">barcode number</param>
		/// <returns>barcode image</returns>
		public static Bitmap GenerateBarcodeImage(string barcode)
		{
			BarCodeBuilder builder = new BarCodeBuilder(barcode, Symbology.Code128);
			return builder.GenerateBarCodeImage();
		}
	}
}
