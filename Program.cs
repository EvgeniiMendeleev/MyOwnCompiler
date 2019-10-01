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

            double res;
            Double.TryParse("10e-15", out res);
            Console.WriteLine(res);

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

            LexicalAnalsys(ref reader);

            writer.Close();
            reader.Close();


            Console.WriteLine("\nЛексический анализ завершён успешно!");
            Console.ReadKey();
        }

        static void ShowTypeOfLexeme(ref string word)
        {
            TypeOfLexeme type = Lexeme.isTypeOfLexeme(word);
            Console.WriteLine(word + " имеет значение " + type);
        }

        static void showTypeOfLexeme(char ch)
        {
            TypeOfLexeme type = Lexeme.isTypeOfLexeme(Convert.ToString(ch));
            Console.WriteLine(ch + " имеет значение " + type);
        }

        static void LexicalAnalsys(ref StreamReader reader)
        {
            for (UInt64 numberOfLine = 1; !reader.EndOfStream; numberOfLine++)
            {
                string strFromCode = reader.ReadLine();

                ClassOfSymbol symbolInWord = ClassOfSymbol.null_class;

                string word = "";

                foreach (char ch in strFromCode)
                {
                    switch (Lexeme.checkSymbol(ch))
                    {
                        case ClassOfSymbol.arithmetic_sign:
                            if (symbolInWord != ClassOfSymbol.arithmetic_sign && symbolInWord != ClassOfSymbol.separator)
                            {
                                ShowTypeOfLexeme(ref word);
                                word = "";
                            }

                            symbolInWord = ClassOfSymbol.arithmetic_sign;
                            word += ch;
                            break;

                        case ClassOfSymbol.letter:
                            if (word != "")
                            {
                                if (symbolInWord == ClassOfSymbol.logical_sign || (symbolInWord == ClassOfSymbol.numeral && Char.IsDigit(word[0])))
                                {
                                    ShowTypeOfLexeme(ref word);
                                    word = "";
                                }
                                else if (symbolInWord == ClassOfSymbol.arithmetic_sign)
                                {
                                    ShowTypeOfLexeme(ref word);
                                    word = "";
                                }
                                else if (symbolInWord == ClassOfSymbol.service_symbol)
                                {
                                    ShowTypeOfLexeme(ref word);
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

                                ShowTypeOfLexeme(ref word);
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
                                    ShowTypeOfLexeme(ref word);
                                    word = "";
                                }
                                else if (symbolInWord == ClassOfSymbol.service_symbol && word[word.Length - 1] != '.')
                                {
                                    ShowTypeOfLexeme(ref word);
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
                                ShowTypeOfLexeme(ref word);
                                word = "";

                                if (ch != ' ')
                                {
                                    showTypeOfLexeme(ch);
                                }
                                break;
                            }
                            if (ch != ' ')
                            {
                                ShowTypeOfLexeme(ref word);
                                word = "";
                                break;
                            }

                            break;

                        case ClassOfSymbol.service_symbol:
                            if (symbolInWord == ClassOfSymbol.letter || (symbolInWord == ClassOfSymbol.numeral && ch != '.') || symbolInWord == ClassOfSymbol.service_symbol)
                            {
                                ShowTypeOfLexeme(ref word);
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
                    ShowTypeOfLexeme(ref word);
                }
            }
        }
    }
}
