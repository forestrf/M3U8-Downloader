using System;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace M3U8downloader
{
	public class Descargador
	{
		public string url = "a";
		public string nombre = "b";
		public string headers = "";

		public double tiempoTotal = -1;
		public double tiempoActual = -1;
		public double horaInicio = -1;

		public int porcentajeInt = 0;
		public double porcentaje = 0;
		public string horaRestanteString = "";
		
		Boolean cancelado = false;
		public String fallado = "";
		
		ProcessStartInfo procesoFFMPEG;
		Process exeProcessProcesoFFMPEG;

		public bool Comienza (string url, string nombre)
		{
			this.url = url;
			this.nombre = nombre;

			return download ();
		}
		public bool Comienza (string url, string headers, string nombre)
		{
			this.url = url;
			this.headers = headers;
			this.nombre = nombre;

			return download ();
		}

		public void Cancelar ()
		{
			if(exeProcessProcesoFFMPEG!=null)
				if(!exeProcessProcesoFFMPEG.HasExited)
					exeProcessProcesoFFMPEG.Kill();
			cancelado = true;
		}

		public bool download ()
		{

				if (url.IndexOf ("http://") != 0 && url.IndexOf ("https://") != 0) {
					Console.WriteLine ("URL invalida. La URL debe comenzar por \"http://\" o \"https://\"");
					fallado = "URL invalida";
					MainClass.descargasEnProceso.Add (this);
					return false;
				}
	
				string m3u8 = "";
				try {
					WebClient webClient = new WebClient ();
					m3u8 = webClient.DownloadString (url);
				} catch (Exception e) {
					Console.WriteLine ("No se pudo descargar la URL: \"" + url + "\"");
					fallado = "Error al descargar la URL";
					MainClass.descargasEnProceso.Add (this);
					return false;
				}



			//remove all \r
			m3u8 = m3u8.Replace ("\r", String.Empty);



			Regex regex = new Regex (@"^[a-zA-Z0-9/\-_](.*?)$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline);

			MatchCollection matches = regex.Matches (m3u8);

			if (matches.Count > 0) {
				if (matches [0].Value.IndexOf (".ts") < 0) {

					//FFMPEG se encargara de elegir que descargar
					Console.WriteLine ("Has introducido una lista de m3u8 o un archivo m3u8 sin archivos .ts");
					
					//Abre navegador
					Process.Start("http://127.0.0.1:" + MainClass.puerto + "/?accion=seleccionarlista&urlm3u8=" + Uri.EscapeUriString(url) );

					MainClass.borraDescarga(this);
					
					return false;
				}
			} else {
				Console.WriteLine ("Formato de m3u8 no soportado");
				fallado = "M3U8 no soportado";
				MainClass.descargasEnProceso.Add (this);
				return false;
			}

			MainClass.descargasEnProceso.Add (this);

			try {
				Console.WriteLine ("Descargando y uniendo partes. Espere por favor...");

				procesoFFMPEG = new ProcessStartInfo ();
				procesoFFMPEG.FileName = MainClass.ffmpegfile;
				procesoFFMPEG.Arguments = "-i \""+url+"\" -c copy -bsf:a aac_adtstoasc \""+nombre+"\"";
				if(headers != "")
					procesoFFMPEG.Arguments += " -headers \""+headers+"\"";

				procesoFFMPEG.UseShellExecute = false;
				procesoFFMPEG.RedirectStandardOutput = true;
				procesoFFMPEG.RedirectStandardError = true;
				procesoFFMPEG.CreateNoWindow = true;


				exeProcessProcesoFFMPEG = Process.Start (procesoFFMPEG);

				exeProcessProcesoFFMPEG.OutputDataReceived += p_OutputDataReceived;
				exeProcessProcesoFFMPEG.ErrorDataReceived += p_ErrorDataReceived;

				exeProcessProcesoFFMPEG.BeginOutputReadLine();
				exeProcessProcesoFFMPEG.BeginErrorReadLine();

				exeProcessProcesoFFMPEG.WaitForExit ();
			} catch (Exception e) {
				//En caso de que FFmpeg falle no siempre dara excepcion (por ejemplo, cuando es necesario cambiar de proxy el server los enlaces no funcionan bien, pero no se activa este fallo)
				Console.WriteLine ("FFMPEG ha fallado.");
				fallado = "FFMPEG ha fallado";
				return false;
			}

			/*
			if(!cancelado)
				return true;
			else
				return true;
			*/

			if (porcentajeInt == 0)
				fallado = "Fallo";
			
			return !cancelado;
		}

		public void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			//FFMPEG NO USA ESTO
			//Console.WriteLine("Received from standard out: " + e.Data);
		}

		public void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			//Console.WriteLine("Received from standard error: " + e.Data);
			//Console.WriteLine (e.Data);
			if (!String.IsNullOrEmpty(e.Data)) {
				if (tiempoTotal == -1) {
					if (e.Data.IndexOf ("Duration: ") >= 0) {
						int inicio = e.Data.IndexOf ("Duration: ") + 10;
						int final = e.Data.IndexOf (",", inicio);
						//tiempo = 00:00:00.00
						string tiempo = e.Data.Substring (inicio, final - inicio);
						tiempoTotal = double.Parse (tiempo.Substring (6, 2));
						tiempoTotal += double.Parse (tiempo.Substring (3, 2)) * 60;
						tiempoTotal += double.Parse (tiempo.Substring (0, 2)) * 60 * 60;

						//Console.WriteLine (tiempo);
						//Console.WriteLine (tiempoTotal);

						horaInicio = Utilidades.UnixTimestamp();
					}
				} else {
					//Console.WriteLine(e.Data);
					if (e.Data.IndexOf ("frame=") == 0 && e.Data.IndexOf ("time=") >= 0) {
						int inicio = e.Data.IndexOf ("time=") + 5;
						int final = e.Data.IndexOf (" ", inicio);
						string tiempo = e.Data.Substring (inicio, final - inicio);

						tiempoActual = double.Parse (tiempo.Substring (6, 2));
						tiempoActual += double.Parse (tiempo.Substring (3, 2)) * 60;
						tiempoActual += double.Parse (tiempo.Substring (0, 2)) * 60 * 60;

						//Console.WriteLine (tiempo);
						//Console.WriteLine (tiempoActual);

						porcentaje = Math.Round (tiempoActual / tiempoTotal * 100.0,
													2, MidpointRounding.AwayFromZero);

						porcentajeInt = (int)porcentaje;

						int horaActual = Utilidades.UnixTimestamp();

						int horaTranscurrida = (int)horaActual - (int)horaInicio;
						int horaRestante = (int)((horaTranscurrida/porcentaje)*(100-porcentaje));

						horaRestanteString = segundosATiempo (horaRestante);


						if (MainClass.desdeServidor) {
							Console.WriteLine (nombre + " - " + porcentajeInt + "%" + " - Quedan: " + horaRestanteString);
						} else {
							string linea = "[";
							int size = 60;
							for (int i = 0; i < size; i++) {
								if ((porcentaje * size) / 100 > i)
									linea += "=";
								else if ((porcentaje * size) / 100 > i - 1)
									linea += ">";
								else
									linea += " ";
							}
							linea += "]";

							Console.Clear ();
							Console.WriteLine ("Descargando, por favor espere...\n"+
							                   "Nombre del archivo: " + nombre + "\n"+
							                   "\n"+
							                   "No cierre esta ventana hasta que la descarga no haya terminado.\n"+
							                   "\n"+
							                   "\n"+
							                   linea + " " + porcentajeInt + "%" + "\n"+
							                   "\n"+
							                   "Tiempo restante para finalizar: " + horaRestanteString);
						}


					}
				}
			}
		}

		public string segundosATiempo(int seg_ini) {
			int horas = (int)Math.Floor((double)(seg_ini/3600));
			int minutos = (int)Math.Floor((double)((seg_ini-(horas*3600))/60));
			int segundos = seg_ini-(horas*3600)-(minutos*60);
			if(horas > 0)
				return horas+" horas, "+minutos+"min, "+segundos+"seg";
			if(minutos > 0)
				return minutos+" min, "+segundos+" seg";
			return segundos+" seg";
		}
	}
}

