using System;
using System.Collections.Generic;
using System.Text;

namespace InterfazEscritorio
{
    class Configuracion
    {

        public static string getConnectionString
        {
            get
            {
                return Properties.Settings.Default.ConnectionString;
                // Esto obtiene la cadena de conexión del archivo:
                // Settings.settings (de la carpeta Properties)
            }
        }

    }
}
