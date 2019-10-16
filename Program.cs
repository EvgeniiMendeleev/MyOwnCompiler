using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LibraryForCompile;
using System.Globalization;

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

            LexicalAnalisys(ref reader, ref writer);

            writer.Close();
            reader.Close();


            Console.WriteLine("\nЛексический анализ завершён успешно!");
            Console.ReadKey();
        }

        static void ShowTypeOfLexeme(ref string word, ref StreamWriter writer)
        {
            TypeOfLexeme type = Lexeme.isTypeOfLexeme(word);
            writer.Write("<< " + word + " >> - " + type + ", ");
        }

        static void showTypeOfLexeme(char ch, ref StreamWriter writer)
        {
            TypeOfLexeme type = Lexeme.isTypeOfLexeme(Convert.ToString(ch));
            writer.Write("<< " + ch + " >> - " + type + ", ");
        }

        static void LexicalAnalisys(ref StreamReader reader, ref StreamWriter writer)
        {
            for (UInt64 numberOfLine = 1; !reader.EndOfStream; numberOfLine++)
            {
                writer.Write(numberOfLine + ": ");
                string strFromCode = reader.ReadLine();     //Read string from text file with program code.

                ClassOfSymbol symbolInWord = ClassOfSymbol.null_class;      

                string word = "";

                foreach (char ch in strFromCode)
                {
                    switch (Lexeme.checkSymbol(ch))
                    {
                        case ClassOfSymbol.arithmetic_sign:
                            #region Some operations for arithmetic signs

                            if (symbolInWord != ClassOfSymbol.arithmetic_sign && symbolInWord != ClassOfSymbol.separator)
                            {
                                ShowTypeOfLexeme(ref word, ref writer);
                                word = "";
                            }

                            symbolInWord = ClassOfSymbol.arithmetic_sign;
                            word += ch;

                            #endregion
                            break;

                        case ClassOfSymbol.letter:
                            #region Some operations for letters

                            if (word != "")
                            {
                                if (symbolInWord == ClassOfSymbol.logical_sign || (symbolInWord == ClassOfSymbol.numeral && Char.IsDigit(word[0])))
                                {
                                    ShowTypeOfLexeme(ref word, ref writer);
                                    word = "";
                                }
                                else if (symbolInWord == ClassOfSymbol.arithmetic_sign)
                                {
                                    ShowTypeOfLexeme(ref word, ref writer);
                                    word = "";
                                }
                                else if (symbolInWord == ClassOfSymbol.service_symbol)
                                {
                                    ShowTypeOfLexeme(ref word, ref writer);
                                    word = "";
                                }
                            }

                            symbolInWord = ClassOfSymbol.letter;
                            word += ch;

                            #endregion
                            break;

                        case ClassOfSymbol.logical_sign:
                            #region Some operations for logical signs

                            if (symbolInWord != ClassOfSymbol.logical_sign && symbolInWord != ClassOfSymbol.separator)
                            {
                                if (symbolInWord == ClassOfSymbol.service_symbol)
                                {
                                    word += ch;
                                }

                                ShowTypeOfLexeme(ref word, ref writer);
                                word = "";
                            }

                            if (symbolInWord != ClassOfSymbol.service_symbol)
                            {
                                word += ch;
                            }

                            symbolInWord = ClassOfSymbol.logical_sign;

                            #endregion
                            break;

                        case ClassOfSymbol.numeral:
                            #region Some operations for numerals

                            if (word != "")
                            {
                                if (symbolInWord == ClassOfSymbol.logical_sign || symbolInWord == ClassOfSymbol.arithmetic_sign)
                                {
                                    ShowTypeOfLexeme(ref word, ref writer);
                                    word = "";
                                }
                                else if (symbolInWord == ClassOfSymbol.service_symbol && word[word.Length - 1] != '.')
                                {
                                    ShowTypeOfLexeme(ref word, ref writer);
                                    word = "";
                                }
                            }

                            symbolInWord = ClassOfSymbol.numeral;
                            word += ch;
                            
                            #endregion
                            break;

                        case ClassOfSymbol.separator:
                            #region Some operations for separators

                            if (word != "")
                            {
                                ShowTypeOfLexeme(ref word, ref writer);
                                word = "";

                                if (ch != ' ')
                                {
                                    showTypeOfLexeme(ch, ref writer);
                                }
                            }
                            else if (symbolInWord == ClassOfSymbol.null_class || (word == "" && ch != ' '))
                            {
                                word += ch;
                            }
                            else if (ch != ' ')
                            {
                                ShowTypeOfLexeme(ref word, ref writer);
                                word = "";
                            }

                            symbolInWord = ClassOfSymbol.separator;

                            #endregion
                            break;

                        case ClassOfSymbol.service_symbol:
                            #region Some operations for service symbols

                            if (symbolInWord == ClassOfSymbol.letter || (symbolInWord == ClassOfSymbol.numeral && ch != '.') || symbolInWord == ClassOfSymbol.service_symbol)
                            {
                                ShowTypeOfLexeme(ref word, ref writer);
                                word = "";
                            }
                            else if (symbolInWord == ClassOfSymbol.separator)
                            {
                                if (word != "")
                                {
                                    ShowTypeOfLexeme(ref word, ref writer);
                                    word = "";
                                }
                            }

                            symbolInWord = ClassOfSymbol.service_symbol;
                            word += ch;

                            #endregion
                            break;

                        case ClassOfSymbol.another_symbol:
                            #region Some operations for another symbols
                            if (ch != '\t')
                            {
                                writer.Write("неразрешённый внешний символ << " + ch + " >>, ");
                                symbolInWord = ClassOfSymbol.another_symbol;
                                word = "";
                            }

                            #endregion
                            break;
                    }

                    if (symbolInWord == ClassOfSymbol.another_symbol) break;
                }

                if (symbolInWord == ClassOfSymbol.another_symbol) { break; }

                if (word != "")
                {
                    ShowTypeOfLexeme(ref word, ref writer);
                }

                writer.WriteLine();
            }
        }
    }
}
