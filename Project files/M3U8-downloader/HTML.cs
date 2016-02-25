using System.Collections.Generic;

namespace M3U8downloader
{
	public abstract class HTML {
		public static KeyValuePair<string, HTML>[] availableLanguages = new KeyValuePair<string, HTML>[] {
			new KeyValuePair<string, HTML>("es", new HTMLSpanish()),
			new KeyValuePair<string, HTML>("en", new HTMLEnglish())
		};

		public virtual string GetName() { return ""; }
		public virtual string GetIndex() { return ""; }
		public virtual string GetProgress() { return ""; }
		public virtual string GetProgress(string mensaje) { return ""; }
		public virtual string GetAllcss() { return ""; }
		public virtual string CloseWithJS() { return ""; }
		public virtual string GetHelp() { return ""; }
		public virtual string GetListSelection(string options) { return ""; }
		public virtual string GetClosed() { return ""; }
		public virtual string GetHelpImageName() { return ""; }

		public string GetLanguageList() {
			string txt = "Change language:<br/>";
			for (int i = 0; i < HTML.availableLanguages.Length; i++) {
				txt += "<a href=\"/lang/"+ HTML.availableLanguages[i].Key + "\">" + HTML.availableLanguages[i].Value.GetName() + "</a>";
			}
			return txt;
		}
	}
}
