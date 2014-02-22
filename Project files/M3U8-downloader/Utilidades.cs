using System;
using System.Text.RegularExpressions;

namespace M3U8downloader
{
	public class Utilidades
	{
		public static int UnixTimestamp()
		{
			return (int)(DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
		}

		//Tan solo para ver si el pregmatch tira lo que quiero
		public static string print_r_regex(MatchCollection mc){
			string resp = "";
			if(mc.Count > 0)
			{
				resp += "Printing matches...";
				for(int i =0; i < mc.Count; i++)
				{
					resp += "\n";
					resp += "Match["+i+"]: " + mc[i].Value + "\n";
					resp += "Printing groups for this match...\n";
					GroupCollection gc = mc[i].Groups;
					for(int j =0; j < gc.Count; j++)
					{
						resp += "\tGroup["+j+"]: "+ gc[j].Value + "\n";
						resp += "\tPrinting captures for this group...\n";
						CaptureCollection cc = gc[j].Captures;
						for(int k =0; k < cc.Count; k++)
						{
							resp += "\t\tCapture["+k+"]: "+ cc[k].Value + "\n";
						}
					}
				}
			}
			return resp;
		}
	}
}

