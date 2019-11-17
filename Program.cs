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

            FileStream textOfCode = new FileStream("e:\\algol.txt", FileMode.Open);

            StreamReader reader = new StreamReader(textOfCode);
            StreamWriter writer = new StreamWriter(File.Create("e:\\result.txt"));

            if (!Lexeme.LexicalAnalisys(ref reader, ref writer))
            {
                Console.WriteLine("Лексический анализ прошёл успешно!");
                Syntax.SyntaxisAnalysis(ref reader, ref writer);
            }

            writer.Close();
            reader.Close();

            Console.WriteLine("\nКомпиляция завершена!");
            Console.ReadKey();
        }
    }
}
