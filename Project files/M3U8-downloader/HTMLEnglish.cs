// TO DO

using System;

namespace M3U8downloader
{
	public class HTMLEnglish : HTML
	{
		public override string GetName() {
			return "English";
		}

		public override string GetIndex(){
			return "<html>" +
				"<head>" +
					"<title>M3U8-Downloader V" + MainClass.version + "</title>" +
					"<link rel=\"stylesheet\" href=\"all.css\">" +
					"<script src=\"http://code.jquery.com/jquery-2.0.3.min.js\"></script>" +
				"</head>" +
				"<body>" +
					"<div id=\"cerrarAplicacion\">" +
						"<a href=\"/?accion=cerrarPrograma\" onclick=\"return confirm('Se cancelarán todas las descargas en progreso\\n¿Seguro que quieres cerrar el programa?');\">Cerrar M3U8-Downloader</a>" +
					"</div>" +
					"<div id=\"menu\">" +
						"<a href=\"/\" class=\"titulo_menu\">M3U8-Downloader V" + MainClass.version + "</a>" +
						"<a href=\"http://www.descargavideos.tv\">Descargavideos.TV</a>" +
						"<a href=\"http://www.descargavideos.tv/lab#lab_m3u8-downloader\" target=\"_blank\">Buscar actualizaciones</a>" +
						"<a href=\"/ayuda\">Ayuda</a>" +
						"<br/><br/>" +
						GetLanguageList() +
					"</div>" +
					"<div id=\"contenido\">" +
						"Nueva descarga:" +
						"<form id=\"form_agregar\" class=\"tabla\" method=\"GET\">" +
							"<input type=\"hidden\" name=\"accion\" value=\"descargar\">" +
							"<input type=\"hidden\" name=\"cerrarVentana\" value=\"0\">" +
							"<div class=\"elemento\">" +
								"<div>URL: </div><div><input type=\"text\" name=\"url\" size=\"60\" placeholder=\"http://www.web.com/archivo.m3u8\"></div>" +
							"</div>" +
							"<div class=\"elemento\">" +
								"<div>Nombre: </div><div><input type=\"text\" name=\"nombre\" size=\"60\" placeholder=\"nombre.mp4\"></div>" +
							"</div>" +
							"<div class=\"elemento\">" +
								"<div><input type=\"submit\" value=\"Agregar\"></div><div></div>" +
							"</div>" +
						"</form>" +
						"Descargas en progreso:" +
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
		
		public override string GetProgress(string mensaje){
			String resp = "<div class=\"elemento titulos\">" +
								"<div class=\"n\">Nombre</div>" +
								"<div class=\"u\">URL</div>" +
								"<div class=\"p\">Progreso</div>" +
								"<div class=\"t\">Tiempo restante</div>" +
								"<div class=\"q\">Quitar</div>" +
							"</div>";
			for (int i=0; i<MainClass.descargasEnProceso.Count; i++) {
				if (MainClass.descargasEnProceso [i].fallado != "") {
					resp += "<div class=\"elemento\">" +
						"<div class=\"n\">" + MainClass.descargasEnProceso [i].nombre + "</div>" +
						"<div class=\"u\">" + MainClass.descargasEnProceso [i].url + "</div>" +
						"<div class=\"p\">"+MainClass.descargasEnProceso [i].fallado+"</div>" +
						"<div class=\"t\"></div>" +
						"<div class=\"q\"><a href=\"/?accion=cancelarDescarga&elem=" + i + "\">Quitar</a></div>" +
						"</div>";
				} else {
					resp += "<div class=\"elemento\">" +
						"<div class=\"n\">" + MainClass.descargasEnProceso [i].nombre + "</div>" +
						"<div class=\"u\">" + MainClass.descargasEnProceso [i].url + "</div>" +
						"<div class=\"p\"><div class=\"progressBar\"><div style=\"width:" + MainClass.descargasEnProceso [i].porcentaje.ToString ().Replace (",", ".") + "%\"></div></div>" + MainClass.descargasEnProceso [i].porcentajeInt + "%</div>" +
						"<div class=\"t\">" + MainClass.descargasEnProceso [i].horaRestanteString + "</div>" +
						"<div class=\"q\"><a href=\"/?accion=cancelarDescarga&elem=" + i + "\">Quitar</a></div>" +
						"</div>";
				}
			}
			
			if(mensaje != ""){
				resp += "<script>alert(\""+mensaje+"\")</script>";
			}
			
			return resp;
		}
		
		public override string CloseWithJS(){
			return "<html><body>Puedes cerrar esta ventana/pestaña."+
				"<script>"+
					"ventana=window.self;"+
					"ventana.opener=window.self;"+
					"ventana.close();"+
				"</script></body></html>";
		}
	
		public override string GetHelp(){
			return "<html>" +
				"<head>" +
					"<title>M3U8-Downloader V" + MainClass.version + "</title>" +
					"<link rel=\"stylesheet\" href=\"all.css\">" +
					"<script src=\"http://code.jquery.com/jquery-2.0.3.min.js\"></script>" +
				"</head>" +
				"<body>" +
					"<div id=\"cerrarAplicacion\">" +
						"<a href=\"/?accion=cerrarPrograma\" onclick=\"return confirm('Se cancelarán todas las descargas en progreso\\n¿Seguro que quieres cerrar el programa?');\">Cerrar M3U8-Downloader</a>" +
					"</div>" +
					"<div id=\"menu\">" +
					"<a href=\"/\" class=\"titulo_menu\">M3U8-Downloader V" + MainClass.version + "</a>" +
						"<a href=\"http://www.descargavideos.tv\">Descargavideos.TV</a>" +
						"<a href=\"http://www.descargavideos.tv/lab#lab_m3u8-downloader\" target=\"_blank\">Buscar actualizaciones</a>" +
						"<a href=\"/ayuda\">Ayuda</a>" +
					"</div>" +
					"<div id=\"contenido\">" +
						"<img src=\"/ayuda/ayuda_prev.png\" class=\"img_ayuda\">" +
						"<ol>" +
							"<li>Versión del programa." +
								"<br>En el caso de la imagen se trata de la versión 0.3.<br>" +
								"Si queremos buscar actualizaciones, se entenderá como una versión superior aquella que tenga un número mayor que el actual, siendo por ejemplo la versión 2.0 superior a la 1.5 y a su vez superior a la versión 0.3.</li>" +
							"<li>Enlace a la sección de versiones del programa en Descargavideos.<br>" +
								"Al hacer clic en el enlace se abrirá una página con el listado de versiones publicadas donde podrás descargar la más reciente o cualquiera de las versiones anteriores.</li>" +
							"<li>Formulario para agregar nuevas descargas.<br>" +
								"En caso de querer agregar manualmente una descarga, introduce la URL del archivo m3u8 en el campo URL y escribe el nombre que quieres que tenga el vídeo en el campo Nombre. El nombre del vídeo debe tener la extensión del archivo, siendo un nombre válido por ejemplo <i>video.mp4</i> o <i>capítulo 15.mp4</i>. El nombre puede dejarse en blanco.<br>Una vez completado el formulario al clicar <i>Agregar</i> comenzará la descarga del vídeo.</li>" +
							"<li>Nombre del archivo tal y como figura en la carpeta que lo contiene.</li>" +
							"<li>URL del archivo que se está descargando.</li>" +
							"<li>Progreso de la descarga.<br>" +
								"Una vez completada cada descarga se abrirá la carpeta que contiene el vídeo. Hasta entonces, no debe moverse o borrarse el archivo.</li>" +
							"<li>Tiempo restante para completar la descarga.<br>" +
								"El tiempo es calculado a partir del porcentaje descargado y el tiempo transcurrido por lo que únicamente es aproximado.</li>" +
							"<li>Quitar la descarga de la lista.<br>" +
								"En caso de que la descarga no esté finalizada detendrá la descarga y la quitará de la lista, dejando el archivo incompleto en la carpeta de descargas.<br>" +
								"En caso de que la descarga esté finalizada, únicamente la quitará de la lista.</li>" +
							"<li>Cerrar el programa.<br>" +
								"Para cerrar el programa se debe de hacer clic en el botón. Al hacerlo, se mostrará una pregunta. En caso de que aceptemos, todas las descargas incompletas en curso se interrumpirán y después se cerrará el programa. De lo contrario no se cerrará el programa. Una vez hecho esto cargará una nueva página en la que indicará que el programa se ha cerrado con éxito.<br>En caso de cerrar la consola (la ventana negra) en lugar de clicar en el botón de cerrar dejará las descargas en proceso abiertas, por lo que para detenerlas sería necesario cerrar el proceso en cuestión.</li>" +
						"</ol>" +
						"<a href=\"/\">atras</a>" +
					"</div>" +
				"</body>" +
				"</html>";
		}
		
		public override string GetListSelection(string options){
			return "<html><body><h3>Se han encontrado varias opciones de descarga</h3><br>A mayor BANDWIDTH Y/O RESOLUTION, mayor calidad de imagen<br>Por favor, clica en la opción que quieras descargar:<br><br><br>"+options;
		}
	
		public override string GetClosed(){
			return "Has cerrado el programa<br>Ahora puedes cerrar la consola.";
		}

		public override string GetHelpImageName() {
			return "M3U8downloader.ayuda_img.png";
		}
	}
}

