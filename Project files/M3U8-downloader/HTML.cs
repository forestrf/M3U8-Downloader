using System;
using System.Collections.Generic;

namespace M3U8downloader
{
	public static class HTML
	{
		
		public static string getIndex(){
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
							getProgreso() +
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

		public static string getProgreso(){
			return getProgreso("");
		}
		
		public static string getProgreso(string mensaje){
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

		public static string getAllcss(){
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

		public static string cierraConJS(){
			return "<html><body>Puedes cerrar esta ventana/pestaña."+
				"<script>"+
					"ventana=window.self;"+
					"ventana.opener=window.self;"+
					"ventana.close();"+
				"</script></body></html>";
		}
	
		public static string getAyuda(){
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
		
		public static string getSeleccionLista(string opciones){
			return "<html><body><h3>Se han encontrado varias opciones de descarga</h3><br>A mayor BANDWIDTH Y/O RESOLUTION, mayor calidad de imagen<br>Por favor, clica en la opción que quieras descargar:<br><br><br>"+opciones;
		}
	
		public static string getCerrado(){
			return "Has cerrado el programa<br>Ahora puedes cerrar la consola.";
		}
	}
}

