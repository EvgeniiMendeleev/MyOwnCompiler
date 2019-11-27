using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using LibraryForCompile;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Console.Write("Введите путь к файлу для компиляции: ");
            string filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Указанного файла не существует!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Указанный файл существует на диске!\n");
            Console.Write("Введите путь для вывода результата компиляции (файл должен быть в формате .txt): ");
            string resultFile = Console.ReadLine();*/

            FileStream textOfCode = new FileStream("d:\\algol.txt", FileMode.Open);

            StreamReader reader = new StreamReader(textOfCode);
            StreamWriter writer = new StreamWriter(File.Create("d:\\result.txt"));
            writer.WriteLine("***************************************");
            if (!Lexeme.LexicalAnalisys(ref reader, ref writer))
            {
                writer.WriteLine("Лексический анализ прошёл успешно!");
                writer.WriteLine("***************************************");
                
                if (!Syntax.SyntaxisAnalysis(ref reader, ref writer))
                {
                    CodeGenerator.Generation();
                }

                writer.WriteLine("***************************************");
            }

            writer.WriteLine("\nКомпиляция завершена!");
            
            writer.Close();
            reader.Close();
        }
    }
}
