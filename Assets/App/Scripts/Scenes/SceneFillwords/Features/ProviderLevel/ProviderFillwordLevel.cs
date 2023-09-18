using System;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        public GridFillWords LoadModel(int index)
        {
            TextLevelFile text = new TextLevelFile(index);

            LevelLoadModel lvl = new LevelLoadModel(text.Words, text.Positions, text.Size);

            if (lvl.Status == true)
            {
                GridFillWords loadGrid = new GridFillWords(lvl.Size);

                int letterIndex = 0;
                for (int row = 0; row < loadGrid.Size.x; row++)
                {
                    for (int column = 0; column < loadGrid.Size.y; column++)
                    {
                        CharGridModel letter = new CharGridModel(lvl.Letters[letterIndex]);
                        loadGrid.Set(row, column, letter);
                        letterIndex++;
                    }
                }

                return loadGrid;
            }
            else 
            {
                throw new Exception();
            }

        }
     
    }


    public class LevelLoadModel
    {
        private char[] levelLetter;
        private int[] levelIndex;
        private Vector2Int levelSize;


        public bool Status { get => CheckValidity(); }
        public Vector2Int Size { get => levelSize; }
        public char[] Letters { get => levelLetter; }
        public int[] Indexes { get => levelIndex; }

        public LevelLoadModel(string wordsFromFile, int[] indexFromFile, int sizeFromFile)
        {
            levelLetter = wordsFromFile.ToCharArray();
            levelIndex = indexFromFile;
            levelSize = LevelSize(sizeFromFile);

            SortingLevel();
        }

        private void SortingLevel()
        {

            for (int i = 1; i < levelIndex.Length; i++)
            {
                int k = levelIndex[i];
                char buff = levelLetter[i];
                int j = i - 1;

                while (j >= 0 && levelIndex[j] > k)
                {
                    levelIndex[j + 1] = levelIndex[j];
                    levelLetter[j + 1] = levelLetter[j];
                    j--;
                }
                levelIndex[j + 1] = k;
                levelLetter[j + 1] = buff;
            }
        }

        private Vector2Int LevelSize(int levelSize)
        {
            Vector2Int vectorSize;
            double size = Math.Sqrt(levelSize);

            if (size % 1 == 0)
            {
                int dimension = Convert.ToInt32(size);
                vectorSize = new Vector2Int(dimension, dimension);
                return vectorSize;
            }
            else
            {
                vectorSize = new Vector2Int(levelSize, 1);
                return vectorSize;

            }

        }

        private bool CheckValidity()
        {
            if (Letters.Length != Indexes.Length)
            {
                return false;
            }
            return true;
        }


    }


    public class TextLevelFile
    {
        private string pathLevelTextFile = "Assets/App/Resources/Fillwords/pack_0.txt";
        private string pathWordTextFile = "Assets/App/Resources/Fillwords/words_list.txt";

        private int level;
        private int[] positions;
        private string words;
        private int size;
        private int[] positionsLetters;

        public string Words { get => words; }
        public int Size { get => size; }
        public int[] Positions { get { return positions; } }


        //private string wordsSplit;

        public TextLevelFile(int lvl)
        {
            string textLevelFile = null;
            level = lvl - 1;
            textLevelFile = GetLevelLineFromTextFile();
            words = GetLevelWords(textLevelFile);
            FindWordPositions(textLevelFile);
        }


        private string GetLevelLineFromTextFile()
        {
            string line = null;
            int i = 0;

            using (StreamReader readerTextFile = new StreamReader(pathLevelTextFile))
            {
                line = readerTextFile.ReadLine();
                while (i != level)
                {
                    line = readerTextFile.ReadLine();
                    i++;
                }
            }

            return line;
        }


        private string GetWordFromDictionaryFile(int wordIndex)
        {
            string levelLineFileText = null;

            using (StreamReader readerWordFile = new StreamReader(pathWordTextFile))
            {
                int i = 0;

                levelLineFileText = readerWordFile.ReadLine();

                while (i != wordIndex)
                {
                    levelLineFileText = readerWordFile.ReadLine();
                    i++;
                }
            }
            return levelLineFileText;
        }


        private string GetLevelWords(string levelText)
        {
            Regex regularFindWord = new Regex(@"(^\d+)|(\s\d+\s)");
            MatchCollection levelString = regularFindWord.Matches(levelText);
            string words = null;

            for (int i = 0; i < levelString.Count; i++)
            {
                string str = levelString[i].Value;
                words = words + GetWordFromDictionaryFile(Convert.ToInt32(str));
            }

            return words;
        }

        public void FindWordPositions(string levelText)
        {
            Regex regularFindLettersPosition = new Regex(@"(?:(\d+[;]){1,}\d+)");
            Regex regularPosition = new Regex(@"\d+");

            MatchCollection stroka = regularFindLettersPosition.Matches(levelText);

            string[] lettersPositions;
            lettersPositions = new string[stroka.Count];

            int lettersCount = 0;
            for (int i = 0; i < stroka.Count; i++)
            {
                string str = stroka[i].Value;
                lettersPositions[i] = str;
                MatchCollection letters = regularPosition.Matches(str);
                lettersCount = lettersCount + letters.Count;
            }

            size = lettersCount;
            positions = new int[lettersCount];

            int j = 0;
            for (int i = 0; i < lettersPositions.Length; i++)
            {
                MatchCollection positionLetter = regularPosition.Matches(lettersPositions[i]);
                for (int count = 0; count < positionLetter.Count; count++)
                {
                    int index = Convert.ToInt32(positionLetter[count].Value);
                    positions[j] = index;
                    j++;
                }


            }

        }



    }
}