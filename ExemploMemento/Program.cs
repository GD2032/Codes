using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Security.AccessControl;

namespace ConsoleApplication43
{
    class Program
    {
        #region MonitoramentoArquivo
        public static string nomeArquivoModificado;
        public static string diretorioArquivoModificado;
        private static FileSystemWatcher _monitorarArquivo;
        public static void MonitorarArquivos(string path,string filtro)
        {
            _monitorarArquivo = new FileSystemWatcher(path, filtro) { IncludeSubdirectories = false };
            _monitorarArquivo.EnableRaisingEvents = true;
            _monitorarArquivo.Renamed += OnFileRenamed;
        }
        private static void OnFileRenamed(object sender, RenamedEventArgs b)
        {
            nomeArquivoModificado = b.Name;
            diretorioArquivoModificado = b.FullPath;
        }
        #endregion
        #region Originator
        class Arquivo
        {
            private DateTime _dataModificacao;
            private string _nomeArquivo;
            private string _textoArquivo;
            private string _diretorio;
            private string[] _matrizAuxiliar;
            public Arquivo(string path)
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    _textoArquivo = sr.ReadLine();
                }
                _diretorio = path;
                _matrizAuxiliar = path.Split(char.Parse(@"\"));
                _nomeArquivo = _matrizAuxiliar[_matrizAuxiliar.Length - 1];
                Console.WriteLine(_nomeArquivo);
            }
            #region Propiedades
            public string NomeArquivo
            {
                get { return _nomeArquivo; }
                set
                {
                    this._nomeArquivo = value;
                    this._dataModificacao = DateTime.Now;
                }
            }
            public string Diretorio
            {
                get { return _diretorio; }
                set
                {
                    this._diretorio = value;
                    this._dataModificacao = DateTime.Now;
                }
            }
            public string Texto
            {
                get { return _textoArquivo; }
                set
                {
                    this._textoArquivo = value;
                    this._dataModificacao = DateTime.Now;
                }
            }
            #endregion
            public Commits SalvarCommits()
            {
                Console.WriteLine("Salvando versão...");
                if (_nomeArquivo != _matrizAuxiliar[_matrizAuxiliar.Length - 1])
                    this._nomeArquivo = _matrizAuxiliar[_matrizAuxiliar.Length - 1];
                using (StreamReader sr = File.OpenText(_diretorio))
                {
                    _textoArquivo = sr.ReadLine();
                }
                Console.WriteLine(_textoArquivo + "  ue");
                return new Commits(_dataModificacao, _nomeArquivo, _textoArquivo, _diretorio);
            }
            public void VoltarCommits(Commits a)
            {
                Console.WriteLine("Regredindo versão...");
                this._nomeArquivo = a.NomeArquivo;
                this._diretorio = a.Diretorio;
                this._textoArquivo = a.Texto;
                File.Delete(_diretorio);
                using(StreamWriter sw = File.CreateText(_diretorio))
                {
                    sw.WriteLine(_textoArquivo + " pegou");
                }
            }
        }
        #endregion
        #region Memento
        class Commits
        {
            DateTime dataModificacao;
            private string _nomeArquivo;
            private string _textoArquivo;
            private string _diretorio;

            public Commits(DateTime a, string b, string c, string d)
            {
                this.dataModificacao = a;
                this._nomeArquivo = b;
                this._textoArquivo = c;
                this._diretorio = d;
            }
            #region Propiedades
            public string NomeArquivo
            {
                get { return _nomeArquivo; }
            }
            public string Diretorio
            {
                get { return _diretorio; }
            }
            public string Texto
            {
                get { return _textoArquivo; }

            }
            #endregion

        }
        #endregion
        #region CareTaker
        class Github
        {
            private Stack<Commits> _mementos = new Stack<Commits>();
            public Commits CareTaker
            {
                get { return _mementos.Pop(); }
                set { _mementos.Push(value); }
            }
        }
        #endregion


        static void Main(string[] args)
        {
            Console.Title = "GitBash";

            string userNome = Environment.UserName;
            string computadorNome = Environment.MachineName;
            string[] matrizAuxiliar = Environment.CurrentDirectory.Split(Char.Parse(@"\"));
            string pathPadrao = matrizAuxiliar[0] + @"\" + matrizAuxiliar[1] + @"\" + matrizAuxiliar[2] + @"\";
            string path = pathPadrao;
            string input;
            Arquivo a = new Arquivo(path + "teste.txt");
            Github b = new Github();
            MonitorarArquivos(path,"*teste.txt");
            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("{0}@{1} ",userNome, computadorNome);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("MINGW{0} ", Environment.Is64BitOperatingSystem ? "64" : "32");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(path == pathPadrao ? "~" : path);
                Console.ResetColor();
                Console.Write("$");
                input = Console.ReadLine();
                try
                {
                    switch (input.Substring(0, 3))
                    {
                        case "cd ":
                            if(Directory.Exists(path + @input.Substring(3)))
                            {
                                a.Diretorio = path + @input.Substring(3);
                                path = a.Diretorio;
                            }
                            else
                            {
                                Console.WriteLine("Este Caminho não existe");
                            }
                            goto default;
                        case "git":
                            switch (input.Substring(4))
                            {
                                case "commit -m":
                                    if (!Directory.Exists(a.Diretorio))
                                    {
                                        a.Diretorio = diretorioArquivoModificado;
                                        a.NomeArquivo = nomeArquivoModificado;
                                    }
                                    b.CareTaker = a.SalvarCommits();
                                    break;
                                case "checkout":
                                    a.VoltarCommits(b.CareTaker);
                                    break;
                            }
                            goto default;
                        default:
                            Console.WriteLine();
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine();
                }
            } while (true);
        }
    }
}
