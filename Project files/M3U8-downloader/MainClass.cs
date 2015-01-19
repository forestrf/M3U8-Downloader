using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Net;

namespace M3U8downloader
{

	public static class MainClass
	{
		public static string ffmpegfile = "";
		public static string m3u8downloaderPath = "";
		public static string relativePath = "";
		public static bool desdeServidor = false;

		public static string version = "0.4";
		
		public static int puerto = 25430;

		public static List<Descargador> descargasEnProceso = new List<Descargador>();
		public static int TempDescargasEnProcesoCantidad = 0;


		public static void Main (string[] cmdLine)
		{
			Console.Title = "M3U8-Downloader V" + version + " - http://www.descargavideos.TV";
			Console.WriteLine ("M3U8-Downloader V" + version + " - http://www.descargavideos.TV");
			Console.WriteLine ("");

			MainClass.m3u8downloaderPath = AppDomain.CurrentDomain.BaseDirectory;
			MainClass.relativePath = Directory.GetCurrentDirectory();

			if (File.Exists (MainClass.m3u8downloaderPath + "\\ffmpeg\\ffmpeg.exe")) {
				MainClass.ffmpegfile = MainClass.m3u8downloaderPath + "\\ffmpeg\\ffmpeg.exe";
			} else if (File.Exists (MainClass.m3u8downloaderPath + "\\ffmpeg.exe")) {
				MainClass.ffmpegfile = MainClass.m3u8downloaderPath + "\\ffmpeg.exe";
			}
			if (MainClass.ffmpegfile == "") {
				//No tsmuxer files
				Console.WriteLine ("Por favor descarga ffmpeg y coloca el archivo ffmpeg.exe dentro de la siguiente carpeta:");
				Console.WriteLine (MainClass.m3u8downloaderPath);
				Console.WriteLine ("");
				Console.WriteLine ("Descargalo aqui:");
				Console.WriteLine ("http://www.ffmpeg.org/download.html");
				Console.WriteLine ("");
				Console.WriteLine ("Tambien puedes encontrar el archivo en el ZIP de M3U8-Downloader");
				Console.WriteLine ("Pulsa cualquier tecla para continuar...");
				Console.ReadKey();
				return;
			}



			//Todo http aqui
			if (cmdLine.Length == 0) {
				desdeServidor = true;
				operaDesdeServidor ();
				return;
			}



			//Desde consola
			Descargador myDownloader = new Descargador ();
			if (cmdLine.Length == 2) {
				myDownloader.Comienza (cmdLine [0], cmdLine [1]);
			} else if (cmdLine.Length == 3) {
				myDownloader.Comienza (cmdLine [0], cmdLine [1], cmdLine [2]);
			} else {
				//incorrect start parameters
				Console.WriteLine ("Uso: M3U8-Downloader <m3u8 File URL> <out file>");
				Console.WriteLine ("Ejemplo: M3U8-Downloader http://web.com/video.m3u8 C:\\video.mp4");
				Console.WriteLine ("");
				Console.WriteLine ("Uso: M3U8-Downloader <m3u8 URL> <HTTP HEADERS> <out file>");
				Console.WriteLine ("Ejemplo: M3U8-Downloader http://web.com/video.m3u8 00112233445566778899AABBCCDDEEFF C:\\video.mp4");
				return;
			}
		}

		public static NetworkServer myServer;

		//Manejo desde HTTP. Todo desde aqui. Una vez terminada la funcion acabara el programa
		public static void operaDesdeServidor(){

			//create server and listen from port 25430
			Console.WriteLine ("Para usar el programa abre en un navegador la siguiente URL:");
			Console.WriteLine ("http://127.0.0.1:"+puerto+"/");
			Console.WriteLine ("");


			myServer = new NetworkServer ();

			if (!myServer.Abre (puerto)) {
				Console.WriteLine ("No se ha podido abrir servidor.");
				Console.WriteLine ("Es posible que ya tengas el programa abierto.");
				Console.WriteLine ("");

				return;
			}
			
			//Abre navegador
			Process.Start("http://127.0.0.1:"+puerto+"/");
			
			while(true) {
				RespuestaHTTP GETurl = myServer.Escucha();

				if (!GETurl.correcto) {
					myServer.CierraCliente ();
					continue;
				}

				string path = GETurl.path;

				string accion = GETurl.getParametro ("accion");

				string nombre = GETurl.getParametro ("nombre");
				string url = GETurl.getParametro ("url");
				string headers = GETurl.getParametro ("headers");

				if (accion == "" || accion == "descargar") {
					if (url != "") {
						string cerrarVentana = GETurl.getParametro ("cerrarVentana");
						if(cerrarVentana == "" || cerrarVentana == "1"){
							myServer.Envia(HTML.cierraConJS());
						}
						//else if(cerrarVentana == "0" || true){
						else{
							myServer.EnviaLocation ("/");
						}
						var t = new Thread(() => lanzaDescarga(url, headers, nombre));
						t.Start();
						continue;
					}
				}

				if(path == "/ayuda"){
					myServer.Envia (HTML.getAyuda());
					continue;
				}
				
				if(path == "/ayuda/ayuda_prev.png"){
					byte[] imgBytes = GetILocalFileBytes.Get("M3U8downloader.ayuda_img.png");
					myServer.EnviaRaw ("image/png", imgBytes);
					continue;
				}
				
				if(path == "/all.css"){
					myServer.Envia (HTML.getAllcss());
					continue;
				}
			
				if (path == "/" && accion == "") {
					myServer.Envia (HTML.getIndex());
					continue;
				}

				if (accion == "seleccionarlista" && GETurl.existeParametro("urlm3u8")) {
					//Mostrar un alert en caso de que se agregue una nueva descarga para conseguir el focus de la pestaña
					String opciones = desglosaListaM3U8(GETurl.getParametro("urlm3u8"));
					myServer.Envia (HTML.getSeleccionLista(opciones));
					
					
					continue;
				}

				if (accion == "progreso") {
					//Mostrar un alert en caso de que se agregue una nueva descarga para conseguir el focus de la pestaña
					if(descargasEnProceso.Count > TempDescargasEnProcesoCantidad){
						myServer.Envia (HTML.getProgreso("Descarga agregada"));
						TempDescargasEnProcesoCantidad = descargasEnProceso.Count;
					}
					else{
						myServer.Envia (HTML.getProgreso());
					}
					continue;
				}
				
				if (accion == "cancelarDescarga") {
					int elem = Convert.ToInt32(GETurl.getParametro ("elem"));
					if(elem >= 0 && elem < descargasEnProceso.Count){
						borraDescarga(descargasEnProceso[elem]);
					}
					myServer.EnviaLocation("/");
					continue;
				}
				
				if (accion == "cerrarPrograma") {
					for(int i=0; i< descargasEnProceso.Count; i++){
						descargasEnProceso[i].Cancelar();
					}
					myServer.Envia(HTML.getCerrado());
					
					myServer.Cierra();
					
					return;
				}


				myServer.Envia ("Na que hacer");
			}
		}

		public static string desglosaListaM3U8(string url){
			if (url.IndexOf ("http://") != 0 && url.IndexOf ("https://") != 0) {
				Console.WriteLine ("URL invalida. La URL debe comenzar por \"http://\" o \"https://\"");
				return "";
			}

			string m3u8List = "";
			try {
				WebClient webClient = new WebClient ();
				m3u8List = webClient.DownloadString (url);
			} catch (Exception e) {
				Console.WriteLine ("No se pudo descargar la URL: \"" + url + "\"");
				return "";
			}
			
			string pattern = "\n([^#]*)[\n$]";
			MatchCollection matchesURL = Regex.Matches (m3u8List, pattern);
			pattern = "\n#EXT-X-STREAM-INF:(.*?(RESOLUTION|BANDWIDTH)=(.*?)[,\n].*?)*";
			MatchCollection matchesDESC = Regex.Matches (m3u8List, pattern);
			//myServer.Envia(Utilidades.print_r_regex(matchesDESC));
			
			string resp = "";
			
			if (matchesURL.Count > 0) {
				string extra = "";
				if (matchesURL[0].Groups[1].Captures[0].Value.IndexOf ("http") != 0) {
					//no es una url completa. completar a partir de la url original. dependiendo de si es relativo o absoluto.
					if (matchesURL[0].Groups[1].Captures[0].Value.IndexOf ("/") == 0) {
						//absoluto
						extra = url.Substring (0, url.IndexOf ("/")) + "/";
					} else {
						//relativo
						extra = url.Substring (0, url.LastIndexOf ("/", url.IndexOf("?")>=0?url.IndexOf("?"):url.Length-1)) + "/";
					}
				}
				
				resp = "";
				
				if(matchesDESC.Count == matchesURL.Count){
					for(int i=0; i<matchesURL.Count; i++){
						resp += "<div><div class=\"calidades\">";
						for(int j=0; j<matchesDESC[i].Groups[2].Captures.Count; j++){
							resp += matchesDESC[i].Groups[2].Captures[j].Value + ": " + matchesDESC[i].Groups[3].Captures[j].Value + (j != matchesDESC[i].Groups[2].Captures.Count -1 ? ", " : "");
						}
						resp += "<div class=\"URL\"><a href=\"http://127.0.0.1:25430/?accion=descargar&url=" + extra + matchesURL[i].Groups[1].Captures[0].Value + "\">" + extra + matchesURL[i].Groups[1].Captures[0].Value + "</a></div></div>";
					}
				}
				else{
					for(int i=0; i<matchesURL.Count; i++){
						resp += "<div class=\"URL\"><a href=\"http://127.0.0.1:25430/?accion=descargar&url=" + extra + matchesURL[i].Groups[1].Captures[0].Value + "\">" + extra + matchesURL[i].Groups[1].Captures[0].Value + "</a></div></div>";
					}
				}
			}
			else{
				resp = "No se ha encontrado nada. Forzar descarga (poner enlace con parametro get que sea forzar = 1. poner en Descargador.cs el detectar ese get, en caso de estar saltar comprobacion)";
			}
			
			
			return resp;
		}

		public static void borraDescarga(Descargador cual){
			cual.Cancelar();
			descargasEnProceso.Remove(cual);
			
			TempDescargasEnProcesoCantidad = descargasEnProceso.Count;
		}

		public static void lanzaDescarga(string url, string nombre){
			lanzaDescarga (url, "", nombre);
		}
		public static void lanzaDescarga(string url, string headers, string nombre){
			Descargador miDescargador = new Descargador ();

			if (nombre == "") {
				nombre = "video.mp4";
			}
			for (int j=1; File.Exists(nombre); j++) {
				nombre = "video" + j + ".mp4";
			}

			if (miDescargador.Comienza (url, headers, nombre)) {
				//Abrir carpeta que tiene el video
				//string myDocspath = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
				string windir = Environment.GetEnvironmentVariable ("WINDIR");
				System.Diagnostics.Process prc = new System.Diagnostics.Process ();
				prc.StartInfo.FileName = windir + @"\explorer.exe";
				prc.StartInfo.Arguments = MainClass.relativePath;
				prc.Start ();
			}
		}
	}
}