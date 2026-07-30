using System;

namespace Blanquita_Inventarios.Site.Helpers
{
    public static class AjustesProgress
    {
        private static readonly object _lock = new object();

        private static int _total;
        private static int _procesados;
        private static string _etapa;
        private static bool _enProceso;
        private static DateTime _inicio;

        private static bool _finalizado;
        private static bool _exitoso;
        private static string _mensaje;

        public static int Total
        {
            get
            {
                lock (_lock)
                {
                    return _total;
                }
            }
            set
            {
                lock (_lock)
                {
                    _total = value;
                }
            }
        }

        public static int Procesados
        {
            get
            {
                lock (_lock)
                {
                    return _procesados;
                }
            }
            set
            {
                lock (_lock)
                {
                    _procesados = value;
                }
            }
        }

        public static string Etapa
        {
            get
            {
                lock (_lock)
                {
                    return _etapa;
                }
            }
            set
            {
                lock (_lock)
                {
                    _etapa = value;
                }
            }
        }

        public static bool EnProceso
        {
            get
            {
                lock (_lock)
                {
                    return _enProceso;
                }
            }
            set
            {
                lock (_lock)
                {
                    _enProceso = value;
                }
            }
        }

        public static bool Finalizado
        {
            get
            {
                lock (_lock)
                {
                    return _finalizado;
                }
            }
            set
            {
                lock (_lock)
                {
                    _finalizado = value;
                }
            }
        }

        public static bool Exitoso
        {
            get
            {
                lock (_lock)
                {
                    return _exitoso;
                }
            }
            set
            {
                lock (_lock)
                {
                    _exitoso = value;
                }
            }
        }

        public static string Mensaje
        {
            get
            {
                lock (_lock)
                {
                    return _mensaje;
                }
            }
            set
            {
                lock (_lock)
                {
                    _mensaje = value;
                }
            }
        }

        public static DateTime Inicio
        {
            get
            {
                lock (_lock)
                {
                    return _inicio;
                }
            }
            set
            {
                lock (_lock)
                {
                    _inicio = value;
                }
            }
        }

        public static int Porcentaje
        {
            get
            {
                lock (_lock)
                {
                    if (_total == 0)
                        return 0;

                    return (int)((_procesados * 100.0) / _total);
                }
            }
        }

        public static void Reset()
        {
            lock (_lock)
            {
                _total = 0;
                _procesados = 0;
                _etapa = string.Empty;
                _enProceso = false;
                _inicio = DateTime.Now;

                _finalizado = false;
                _exitoso = false;
                _mensaje = string.Empty;
            }
        }
    }
}