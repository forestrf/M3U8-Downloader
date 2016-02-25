// Translation by veso266

using System;

namespace M3U8downloader {
	public class HTMLEnglish : HTML {
		public override string GetName() {
			return "English";
		}

		public override string GetIndex() {
			return "<html>" +
				"<head>" +
					"<title>M3U8-Downloader V" + MainClass.version + "</title>" +
					"<link rel=\"stylesheet\" href=\"all.css\">" +
					"<script src=\"http://code.jquery.com/jquery-2.0.3.min.js\"></script>" +
				"</head>" +
				"<body>" +
					"<div id=\"cerrarAplicacion\">" +
						"<a href=\"/?accion=cerrarPrograma\" onclick=\"return confirm('Cancel all downloads in progress.\\n¿Do you really want to close the program?');\">Close M3U8-Downloader</a>" +
					"</div>" +
					"<div id=\"menu\">" +
						"<a href=\"/\" class=\"titulo_menu\">M3U8-Downloader V" + MainClass.version + "</a>" +
						"<a href=\"http://www.descargavideos.tv\">Descargavideos.TV</a>" +
						"<a href=\"http://www.descargavideos.tv/lab#lab_m3u8-downloader\" target=\"_blank\">Check updates</a>" +
						"<a href=\"/ayuda\">Help</a>" +
						"<br/><br/>" +
						GetLanguageList() +
					"</div>" +
					"<div id=\"contenido\">" +
						"New Download:" +
						"<form id=\"form_agregar\" class=\"tabla\" method=\"GET\">" +
							"<input type=\"hidden\" name=\"accion\" value=\"descargar\">" +
							"<input type=\"hidden\" name=\"cerrarVentana\" value=\"0\">" +
							"<div class=\"elemento\">" +
								"<div>URL: </div><div><input type=\"text\" name=\"url\" size=\"60\" placeholder=\"http://www.web.com/archivo.m3u8\"></div>" +
							"</div>" +
							"<div class=\"elemento\">" +
								"<div>File Name: </div><div><input type=\"text\" name=\"nombre\" size=\"60\" placeholder=\"nombre.mp4\"></div>" +
							"</div>" +
							"<div class=\"elemento\">" +
								"<div><input type=\"submit\" value=\"Download\"></div><div></div>" +
							"</div>" +
						"</form>" +
						"Downloads in progress:" +
						"<div id=\"descargando\" class=\"tabla\">" +
							GetProgress() +
						"</div>" +
					"</div>" +
					"<script>" +
						"setInterval(actualizaDescargando, 1000);" +
						"function actualizaDescargando(){" +
							"$.ajax({" +
								"url: \"/?accion=progreso\"" +
							"})" +
							".done(function( res ) {" +
								"$( \"#descargando\" ).html( res );" +
							"});" +
						"}" +
					"</script>" +
				"</body>" +
				"</html>";
		}

		public override string GetProgress(string mensaje) {
			String resp = "<div class=\"elemento titulos\">" +
								"<div class=\"n\">Name</div>" +
								"<div class=\"u\">URL</div>" +
								"<div class=\"p\">Progress</div>" +
								"<div class=\"t\">Time left</div>" +
								"<div class=\"q\">Remove</div>" +
							"</div>";
			for (int i = 0; i < MainClass.descargasEnProceso.Count; i++) {
				if (MainClass.descargasEnProceso[i].fallado != "") {
					resp += "<div class=\"elemento\">" +
						"<div class=\"n\">" + MainClass.descargasEnProceso[i].nombre + "</div>" +
						"<div class=\"u\">" + MainClass.descargasEnProceso[i].url + "</div>" +
						"<div class=\"p\">" + MainClass.descargasEnProceso[i].fallado + "</div>" +
						"<div class=\"t\"></div>" +
						"<div class=\"q\"><a href=\"/?accion=cancelarDescarga&elem=" + i + "\">Remove</a></div>" +
						"</div>";
				} else {
					resp += "<div class=\"elemento\">" +
						"<div class=\"n\">" + MainClass.descargasEnProceso[i].nombre + "</div>" +
						"<div class=\"u\">" + MainClass.descargasEnProceso[i].url + "</div>" +
						"<div class=\"p\"><div class=\"progressBar\"><div style=\"width:" + MainClass.descargasEnProceso[i].porcentaje.ToString().Replace(",", ".") + "%\"></div></div>" + MainClass.descargasEnProceso[i].porcentajeInt + "%</div>" +
						"<div class=\"t\">" + MainClass.descargasEnProceso[i].horaRestanteString + "</div>" +
						"<div class=\"q\"><a href=\"/?accion=cancelarDescarga&elem=" + i + "\">Remove</a></div>" +
						"</div>";
				}
			}

			if (mensaje != "") {
				resp += "<script>alert(\"" + mensaje + "\")</script>";
			}

			return resp;
		}

		public override string CloseWithJS() {
			return "<html><body>You can close this window/tab." +
				"<script>" +
					"ventana=window.self;" +
					"ventana.opener=window.self;" +
					"ventana.close();" +
				"</script></body></html>";
		}

		public override string GetHelp() {
			return "<html>" +
				"<head>" +
					"<title>M3U8-Downloader V" + MainClass.version + "</title>" +
					"<link rel=\"stylesheet\" href=\"all.css\">" +
					"<script src=\"http://code.jquery.com/jquery-2.0.3.min.js\"></script>" +
				"</head>" +
				"<body>" +
					"<div id=\"cerrarAplicacion\">" +
						"<a href=\"/?accion=cerrarPrograma\" onclick=\"return confirm('Cancel all downloads in progress.\\n¿Do you realy want to close the program?');\">Close M3U8-Downloader</a>" +
					"</div>" +
					"<div id=\"menu\">" +
					"<a href=\"/\" class=\"titulo_menu\">M3U8-Downloader V" + MainClass.version + "</a>" +
						"<a href=\"http://www.descargavideos.tv\">Descargavideos.TV</a>" +
						"<a href=\"http://www.descargavideos.tv/lab#lab_m3u8-downloader\" target=\"_blank\">Search for updates</a>" +
						"<a href=\"/ayuda\">Help</a>" +
					"</div>" +
					"<div id=\"contenido\">" +
						"<img src=\"/ayuda/ayuda_prev.png\" class=\"img_ayuda\">" +
						"<ol>" +
							"<li>Program Version." +
								"<br>In the case of the image it is version 0.3.<br>" +
								"If we want to check for updates, it will be understood as a superior version of one that has a value greater than the current number, with version 2.0, for example greater than 1.5 and higher than version 0.3.</li>" +
							"<li>Link to the section of the program versions.<br>" +
								"Clicking on the link a page with the list of all published versions where you can download the latest or any of the earlier versions will open.</li>" +
							"<li>In this form you add new Downloads.<br>" +
								"If you want to manually add a download, enter the URL of the file m3u8 in the URL field and type the name you want to have the video in the Name field. The name of the video must have the file extension, to be a valid name such as<i>video.mp4</i> or <i>Chapter 15.mp4</i>. The name can be left blank.<br>Once completed the form when clicking<i>Add</i>start downloading the video.</li>" +
							"<li>File name as it appears in the folder that contains it.</li>" +
							"<li>URL of the file you are downloading.</li>" +
							"<li>Download Progress.<br>" +
								"After completing each download folder containing the video opens. Until then, you should not move or delete the file.</li>" +
							"<li>Time remaining to complete the download.<br>" +
								"The time is calculated from the percentage downloaded and elapsed time so it is only approximate.</li>" +
							"<li>Remove the download list.<br>" +
								"If the download is not yet completed it will stop downloading and be removed from the list, leaving the incomplete file in the download folder.<br>" +
								"If the download is completed it will be removed from the list automatically</li>" +
							"<li>Close the Program.<br>" +
								"To close the program you must click the button. In doing so, a question is displayed. If you accept, all incomplete downloads in progress will be discontinued and then the program will close. Otherwise the program will not be closed. Once done it will load a new page that will indicate that the program has been successfully closed.<br>If you close the console (black window) instead of clicking on the close button it will stop the discharges in open process, so to stop them it would be necessary to close the process in question.</li>" +
						"</ol>" +
						"<a href=\"/\">Back</a>" +
					"</div>" +
				"</body>" +
				"</html>";
		}

		public override string GetListSelection(string options) {
			return "<html><body><h3>Found several download options</h3><br>A greater BANDWIDTH and/or resolution, higher image quality<br>Please click on the option you want to download:<br><br><br>" + options;
		}

		public override string GetClosed() {
			return "You have closed the program<br>Now you can close the console.";
		}

		public override string GetHelpImageName() {
			return "M3U8downloader.ayuda_img.png";
		}

		public override string GetTXTInvalidURL() {
			return "Invalid URL";
		}

		public override string GetTXTErrorDownloadingURL() {
			return "Error downloading the URL";
		}

		public override string GetTXTM3U8Unsupported() {
			return "M3U8 not supported";
		}

		public override string GetTXTFFMPEGFail() {
			return "FFMPEG failed";
		}

		public override string GetTXTFail() {
			return "Error";
		}
	}
}
