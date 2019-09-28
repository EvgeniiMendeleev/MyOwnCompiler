using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LibraryForCompile;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите путь к файлу для компиляции: ");
            string filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Указанного файла не существует!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Указанный файл существует на диске!\n");

            FileStream textOfCode = new FileStream(filePath, FileMode.Open);
            StreamReader reader = new StreamReader(textOfCode);
            StreamWriter writer = new StreamWriter(File.Create("d:\\ResultOfCompiler.txt"));

            LexicalAnalize(ref reader);

            writer.Close();
            reader.Close();

            Console.WriteLine("\nЛексический анализ завершён успешно!");
            Console.ReadKey();
        }

        static void LexicalAnalysis(ref StreamReader reader)
        {
            for (UInt64 numberOfLine = 1; !reader.EndOfStream; numberOfLine++)
            {
                string strFromCode = reader.ReadLine();

                ClassOfSymbol symbolInWord = ClassOfSymbol.null_class;
                TypeOfLexeme type;

                string word = "";

                foreach (char ch in strFromCode)
                {
                    switch (Lexeme.checkSymbol(ch))
                    {
                        case ClassOfSymbol.arithmetic_sign:
                            if (symbolInWord != ClassOfSymbol.arithmetic_sign && symbolInWord != ClassOfSymbol.separator)
                            {
                                type = Lexeme.isTypeOfLexeme(word);

                                Console.WriteLine(word + " имеет значение " + type);
                                word = "";
                            }

                            symbolInWord = ClassOfSymbol.arithmetic_sign;
                            word += ch;
                            break;

                        case ClassOfSymbol.letter:
                            if (word != "")
                            {
                                if (symbolInWord == ClassOfSymbol.logical_sign)
                                {
                                    type = Lexeme.isTypeOfLexeme(word);

                                    Console.WriteLine(word + " имеет значение " + type);
                                    word = "";
                                }
                            }

                            symbolInWord = ClassOfSymbol.letter;
                            word += ch;
                            break;

                        case ClassOfSymbol.logical_sign:
                            if (symbolInWord != ClassOfSymbol.logical_sign && symbolInWord != ClassOfSymbol.separator)
                            {
                                if (symbolInWord == ClassOfSymbol.service_symbol)
                                {
                                    word += ch;
                                }

                                type = Lexeme.isTypeOfLexeme(word);
                                Console.WriteLine(word + " имеет значение " + type);
                                word = "";
                            }

                            if (symbolInWord != ClassOfSymbol.service_symbol)
                            {
                                word += ch;
                            }

                            symbolInWord = ClassOfSymbol.logical_sign;
                            break;

                        case ClassOfSymbol.numeral:
                            if (word != "")
                            {
                                if (symbolInWord == ClassOfSymbol.logical_sign || symbolInWord == ClassOfSymbol.arithmetic_sign)
                                {
                                    type = Lexeme.isTypeOfLexeme(word);

                                    Console.WriteLine(word + " имеет значение " + type);
                                    word = "";
                                }
                            }

                            symbolInWord = ClassOfSymbol.numeral;
                            word += ch;
                            break;

                        case ClassOfSymbol.separator:
                            symbolInWord = ClassOfSymbol.separator;
                            if (word != "")
                            {
                                type = Lexeme.isTypeOfLexeme(word);

                                Console.WriteLine(word + " имеет значение " + type);
                                word = "";

                                if (ch != ' ')
                                {
                                    type = Lexeme.isTypeOfLexeme(Convert.ToString(ch));
                                    Console.WriteLine(ch + " имеет значение " + type);
                                }
                                break;
                            }
                            if (ch != ' ')
                            {
                                type = Lexeme.isTypeOfLexeme(word);

                                Console.WriteLine(word + " имеет значение " + type);
                                word = "";
                                break;
                            }

                            break;

                        case ClassOfSymbol.service_symbol:
                            if (symbolInWord == ClassOfSymbol.letter)
                            {
                                type = Lexeme.isTypeOfLexeme(word);

                                Console.WriteLine(word + " имеет значение " + type);
                                word = "";
                            }
                            symbolInWord = ClassOfSymbol.service_symbol;
                            word += ch;
                            break;

                        case ClassOfSymbol.another_symbol:
                            if (ch != '\t')
                            {
                                symbolInWord = ClassOfSymbol.another_symbol;
                                word += ch;
                            }
                            break;
                    }
                }

                if (word != "")
                {
                    type = Lexeme.isTypeOfLexeme(word);

                    Console.WriteLine(word + " имеет значение " + type);
                }
            }
        }
    }
}
