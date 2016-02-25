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
		public virtual string GetProgress(string mensaje) { return ""; }
		public virtual string CloseWithJS() { return ""; }
		public virtual string GetHelp() { return ""; }
		public virtual string GetListSelection(string options) { return ""; }
		public virtual string GetClosed() { return ""; }
		public virtual string GetHelpImageName() { return ""; }
		public virtual string GetTXTInvalidURL() { return ""; }
		public virtual string GetTXTErrorDownloadingURL() { return ""; }
		public virtual string GetTXTM3U8Unsupported() { return ""; }
		public virtual string GetTXTFFMPEGFail() { return ""; }
		public virtual string GetTXTFail() { return ""; }


		public string GetLanguageList() {
			string txt = "Change language:<br/>";
			for (int i = 0; i < HTML.availableLanguages.Length; i++) {
				txt += "<a href=\"/lang/"+ HTML.availableLanguages[i].Key + "\">" + HTML.availableLanguages[i].Value.GetName() + "</a>";
			}
			return txt;
		}

		public virtual string GetProgress() {
			return GetProgress("");
		}

		public virtual string GetAllcss() {
			//RESET.css
			return "html, body, div, span, applet, object, iframe," +
				"h1, h2, h3, h4, h5, h6, p, blockquote, pre," +
				"a, abbr, acronym, address, big, cite, code," +
				"del, dfn, em, img, ins, kbd, q, s, samp," +
				"small, strike, strong, sub, sup, tt, var," +
				"b, u, i, center,\ndl, dt, dd, ol, ul, li," +
				"fieldset, form, label, legend," +
				"table, caption, tbody, tfoot, thead, tr, th, td," +
				"article, aside, canvas, details, embed," +
				"figure, figcaption, footer, header, hgroup," +
				"menu, nav, output, ruby, section, summary," +
				"time, mark, audio, video {" +
					"margin: 0;" +
					"padding: 0;" +
					"border: 0;" +
					"font-size: 100%;" +
					"vertical-align: baseline;" +
				"}" +
				"article, aside, details, figcaption, figure," +
				"footer, header, hgroup, menu, nav, section {" +
					"display: block;" +
				"}" +
				"body {" +
					"line-height: 1;" +
				"}" +
				"ol, ul {" +
					"list-style: none;" +
				"}" +
				"blockquote, q {" +
					"quotes: none;" +
				"}" +
				"blockquote:before, blockquote:after," +
				"q:before, q:after {" +
					"content: '';" +
					"content: none;" +
				"}" +
				"table {" +
					"border-collapse: collapse;" +
					"border-spacing: 0;" +
				"}" +

				//All.css
				"body {" +
					"background-color: #6E9500;" +
					"font-family: Tahoma;" +
				"}" +
				"a {" +
					"color: #0FF;" +
				"}" +
				"a:hover {" +
					"color: #FFF;" +
				"}" +
				"#cerrarAplicacion > a:hover {" +
					"background-color: #FF0000;" +
				"}" +
				"#cerrarAplicacion > a {" +
					"background: none repeat scroll 0 0 #9C0000;" +
					"border: 1px double #FFFFFF;" +
					"border-radius: 10px;" +
					"color: #FFFFFF;" +
					"margin: 5px;" +
					"padding: 10px;" +
					"position: fixed;" +
					"right: 0;" +
					"text-decoration: none;" +
					"top: 0;" +
				"}" +
				"#menu {" +
					"background-color: #1A1A1A;" +
					"box-shadow: 0 0 15px 0 #000000;" +
					"color: #FFFFFF;" +
					"height: 100%;" +
					"padding: 30px 0;" +
					"position: fixed;" +
					"text-align: center;" +
					"width: 230px;" +
				"}" +
				"#menu a {" +
					"color: #B2F100;" +
					"display: block;" +
					"font-size: 16px;" +
					"font-weight: bold;" +
					"line-height: 20px;" +
					"padding: 10px 0;" +
					"text-decoration: none;" +
				"}" +
				"#menu a:hover {" +
					"background-color: #EEEEEE;" +
					"box-shadow: 0 0 9px #929292 inset;" +
					"color: #333333;" +
				"}" +
				"#menu .titulo_menu {" +
					"background-color: #990000;" +
					"color: #FFFFFF;" +
					"font-size: 18px;" +
					"font-weight: normal;" +
					"line-height: 18px;" +
					"margin-bottom: 35px;" +
					"padding: 20px;" +
				"}" +
				"#menu .titulo_menu:hover {" +
					"background-color: #FFFFFF;" +
					"box-shadow: none;" +
					"color: #000000;" +
				"}" +
				"#contenido {" +
					"color: #FFFFFF;" +
					"display: inline-block;" +
					"margin: 25px 20px 0 270px;" +
					"padding: 0 0 50px;" +
				"}" +
				"#descargando {" +
					"width: 100%;" +
				"}" +
				"#descargando .n{" +
					"width: 18%;" +
				"}" +
				"#descargando .u{" +
					"width: 27%;" +
				"}" +
				"#descargando .p{" +
					"width: 27%;" +
				"}" +
				"#descargando .t{" +
					"width: 19%;" +
				"}" +
				"#descargando .q{" +
					"width: 9%;" +
				"}" +
				"#descargando .q a {" +
					"background-color: #FF0000;" +
					"border: 1px solid #FFFFFF;" +
					"border-radius: 10px;" +
					"color: #FFFFFF;" +
					"display: inline-block;" +
					"padding: 4px;" +
					"text-decoration: none;" +
				"}" +
				"#descargando .q a:hover {" +
					"background-color: #FFFFFF;" +
					"color: #F00;" +
					"border-color: #F00;" +
				"}" +
				".tabla{" +
					"display: table;" +
					"margin: 15px 0 50px;" +
					"position: relative;" +
					"table-layout: fixed;" +
					"word-break: break-all;" +
					"word-wrap: break-word;" +
				"}" +
				".tabla > .elemento:nth-child(odd) {" +
					"background-color: #B6CCAF;" +
				"}" +
				".tabla > .elemento:nth-child(even) {" +
					"background-color: #CBE1A4;" +
				"}" +
				".tabla > .elemento {" +
					"border-bottom: 1px solid #000000;" +
					"color: #000000;" +
					"display: table-row;" +
					"line-height: 22px;" +
				"}" +
				".tabla > .titulos:first-child {" +
					"background-color: #24302A;" +
					"color: #ECFFC9;" +
					"font-weight: bold;" +
				"}" +
				".tabla > .elemento > div {" +
					"display: table-cell;" +
					"font-size: 14px;" +
					"line-height: 14px;" +
					"padding: 5px 10px;" +
				"}" +
				".progressBar {" +
					"background-color: #000000;" +
					"border-radius: 10px;" +
					"box-shadow: 0 -9px 10px -5px #858585 inset;" +
					"display: inline-block;" +
					"height: 10px;" +
					"margin: 0 8px 0 0;" +
					"padding: 1px;" +
					"width: calc(100% - 60px);" +
				"}" +
				".progressBar > div {" +
					"background-color: #FFC8C8;" +
					"box-shadow: 0 -2px 4px 2px #FF0000 inset;" +
					"border-radius: 10px;" +
					"height: 10px;" +
				"}" +
				"ol {" +
					"list-style: decimal outside none;" +
				"}" +
				"li {" +
					"margin-bottom: 1em;" +
				"}" +
				".img_ayuda{" +
					"background-color: #C2C2C2;" +
					"border: 1px solid #FFFFFF;" +
					"padding: 10px;" +
					"width: 80%" +
				"}";
		}

	}
}
