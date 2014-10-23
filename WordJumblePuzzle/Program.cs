using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace WordFindPuzzle
{
    class Program
    {
        //Must manually change to these to search different jumbles or word lists
        //Found in project bin/Debug file
        public static string puzzle = @"WordSearch.txt";
        public static string wordBank = @"WordList.txt";
        public static char[,] wordJumble;
        public static int height = 0;
        public static int length = 0;
        public static int numWords = 0;
        public static Dictionary<int, string> wordList;
        public static Dictionary<int, string> directionDict;

        //Main
        static void Main(string[] args)
        {

            LetsDoTheWordFind(puzzle);

        }

        //LetsDoTheWordFind: Primary logic loop for the entire application
        public static void LetsDoTheWordFind(string fileName)
        {
            height = CountLines(fileName);
            wordJumble = new char[length, height];

            SetUpJumble(fileName);
            Console.WriteLine("\n<FINISHED LOADING WORD FIND PUZZLE>\n");
            Console.WriteLine("*********************************\n");
            Console.WriteLine("<NOW READING IN WORD BANK>\n");
            numWords = ReadWordBank();
            CreateDirectionalDict();
            if (numWords != 0)
            {
                Console.WriteLine("<SUCCESSFULLY READ IN WORD BANK>\n");
            }
            else
            {
                Console.WriteLine("<ERROR: WORDBANK CAN NOT BE READ IN>\n");
                return;
            }

            Console.WriteLine("<NOW SEARCHING FOR WORDS>\n");
            //Commence the search!
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int direction = 0; direction < 8; direction++)
                    {
                        FindTheseWords(i, j, "", direction);
                    }
                }
            }

            Console.WriteLine("\n<FINISHED SEARCHING FOR WORDS>\n");

        }

        //SetUpJumble: Setting up the letter field to search for words
        public static int SetUpJumble(string fileName)
        {
            int row = 0;

            if (File.Exists(fileName))
            {
                using (StreamReader file = new StreamReader(fileName))
                {
                    if (file != null)
                    {
                        Console.WriteLine("<NOW READING IN WORD FIND PUZZLE>\n");
                        while (true)
                        {
                            string line = file.ReadLine();

                            if (line == null)
                            {
                                return row;
                            }
                            else
                            {
                                for (int i = 0; i < line.Length; i++)
                                {
                                    wordJumble[i, row] = line[i];
                                }
                                Console.WriteLine(line);
                                row++;
                            }
                        }
                    }
                    return row;
                }
            }
            return row;


        }

        //CountLines: Counts out how many rows are in the word jumble
        public static int CountLines(string fileName)
        {
            string line;
            int count = 0;

            if (File.Exists(fileName))
            {
                using (StreamReader file = new StreamReader(fileName))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        length = line.Length;
                        count++;
                    }
                }
            }
            return count;
        }

        //ReadWordBank: Reads in word list from the provided text. Concatenates out the white space in compound words listed.
        public static int ReadWordBank()
        {
            string line;
            int index = 0;
            wordList = new Dictionary<int, string>();
            if (File.Exists(wordBank))
            {
                using (StreamReader file = new StreamReader(wordBank))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.Contains(" "))
                        {
                            wordList[index] = line.Replace(" ", String.Empty).ToUpper();
                        }
                        else
                        {
                            wordList[index] = line.ToUpper();
                        }
                        index++;
                    }
                }
            }

            return index;
        }

        //CreateDirectionalDict: Initializer for directional dictionary. Makes output formatting simpler.
        public static void CreateDirectionalDict()
        {
            directionDict = new Dictionary<int, string>();

            for (int i = 0; i < 8; i++)
            {
                switch (i)
                {
                    case 0:
                        directionDict[i] = "LR";
                        break;
                    case 1:
                        directionDict[i] = "D";
                        break;
                    case 2:
                        directionDict[i] = "DDR";
                        break;
                    case 3:
                        directionDict[i] = "RL";
                        break;
                    case 4:
                        directionDict[i] = "U";
                        break;
                    case 5:
                        directionDict[i] = "DUL";
                        break;
                    case 6:
                        directionDict[i] = "DDL";
                        break;
                    case 7:
                        directionDict[i] = "DUR";
                        break;
                }
            }

        }

        //FindTheseWords: Core search method; Recursively looks through the given word jumble for each word; Returns if out of bounds or if word is found.
        public static void FindTheseWords(int x, int y, string word, int direction)
        {
            if (x >= length || x < 0 || y >= height || y < 0)
            {
                return;
            }

            char currentLetter = wordJumble[x, y];

            string tempWord = word + currentLetter;

            if (IsThisAWord(tempWord))
            {
                Console.WriteLine("WORD FOUND: {0} ENDING AT {1}, {2} GOING {3}", tempWord, x, y, directionDict[direction]);

                return;
            }

            switch (direction)
            {
                case 0:
                    FindTheseWords(x + 1, y, tempWord, direction);
                    break;
                case 1:
                    FindTheseWords(x, y + 1, tempWord, direction);
                    break;
                case 2:
                    FindTheseWords(x + 1, y + 1, tempWord, direction);
                    break;
                case 3:
                    FindTheseWords(x - 1, y, tempWord, direction);
                    break;
                case 4:
                    FindTheseWords(x, y - 1, tempWord, direction);
                    break;
                case 5:
                    FindTheseWords(x - 1, y - 1, tempWord, direction);
                    break;
                case 6:
                    FindTheseWords(x - 1, y + 1, tempWord, direction);
                    break;
                case 7:
                    FindTheseWords(x + 1, y - 1, tempWord, direction);
                    break;
            }
        }

        //IsThisAWord: Checks if the input is a word in our word list; returns true if it is or false if it is not
        public static bool IsThisAWord(string word)
        {
            for (int i = 0; i < wordList.Count; i++)
            {
                if (wordList[i] == word)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
