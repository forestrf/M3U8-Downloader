using System;
using System.IO;
using System.Reflection;

namespace M3U8downloader
{
	public class GetILocalFileBytes
	{
		public GetILocalFileBytes (){}
		
		public static byte[] Get(String resourceIDName){
			Stream file = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceIDName);
			byte[] fileBytes = new byte[file.Length];
			file.Read(fileBytes, 0, (int)file.Length);
			return fileBytes;
		}
	}
}

