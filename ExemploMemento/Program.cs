using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace ConsoleApplication43
{
    class Program
    {

        #region Originator
        class Arquivo
        {
            private DateTime _dataModificacao;
            private string _nomeArquivo;
            private string _textoArquivo;
            private string _diretorio;
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
                return new Commits(_dataModificacao, _nomeArquivo, _textoArquivo, _diretorio);
            }
            public void VoltarCommits(Commits a)
            {

                Console.WriteLine("Regredindo versão...");
                this._nomeArquivo = a.NomeArquivo;
                this._diretorio = a.Diretorio;
                this._textoArquivo = a.Texto;
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
                Console.ReadLine();
            } while (true);
            Console.WriteLine(userNome + computadorNome);
            Arquivo a = new Arquivo();
            Github b = new Github();
            //vs 1
            a.Diretorio = @"C:\Users\aluno\Object3D";
            a.NomeArquivo = "Musicas";
            a.Texto = "Eu curto rock balboa";

            //salvando vs 1
            b.CareTaker = a.SalvarCommits();
            //vs 2
            a.Diretorio = @"C:\Users\aluno\Object3D";
            a.NomeArquivo = "Maçã";
            a.Texto = "Maçã verde, cultivada em 1987 e mumificada pelo própio Isaaac Newton";
            
            //salvando vs 2
            b.CareTaker = a.SalvarCommits();
            //vs 3
            a.Diretorio = @"C:\Users\aluno\JogoPi";
            a.NomeArquivo = "Rodavort";
            a.Texto = "O melhor jogo do mundo?";
            
            //regredindo para vs 2
            a.VoltarCommits(b.CareTaker);
            //regredindo para vs 1
            a.VoltarCommits(b.CareTaker);
        }
    }
}
