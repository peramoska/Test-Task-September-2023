using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using App.Scripts.Libs.Factory;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;


namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel
{
    public class FactoryLevelModel : IFactory<LevelModel, LevelInfo, int>
    {
        public LevelModel Create(LevelInfo value, int levelNumber)
        {
            var model = new LevelModel();

            model.LevelNumber = levelNumber;

            model.Words = value.words;
            model.InputChars = BuildListChars(value.words);

            return model;
        }

        private List<char> BuildListChars(List<string> words)
        {
            LetterFinder levelLetters = new LetterFinder(words);
            return levelLetters.Letters;
        }
    }

    public class LetterFinder
    {
        private List<char> letters = new List<char>();

        public List<char> Letters { get => letters; }

        public LetterFinder(List<string> levelWords)
        {
            foreach (string wrds in levelWords)
            {
                FillLetters(letters, wrds);
            }
        }

        private void AddLetterMultipleTimes(char letter, int times)
        {
            for (int i = 0; i < times; i++)
            {
                letters.Add(letter);
            }
        }

        private char[] SortingLettersInWord(string word)
        {
            char[] ch = word.ToCharArray();
            Array.Sort(ch);
            return ch;
        }

        private int GetCountLetters(string word, char letter)
        {
            int count = 0;
            foreach (char ch in word)
            {
                if (ch == letter)
                {
                    count++;
                }
            }
            return count;
        }

        private int GetCountLetters(List<char> list, char letter)
        {
            int count = 0;
            foreach (char ch in list)
            {
                if (ch == letter)
                {
                    count++;
                }
            }
            return count;
        }


        private void FillLetters(List<char> list, string word)
        {
            char[] wordLetters = SortingLettersInWord(word);
            foreach (char ch in wordLetters)
            {
                int timeOnWord = GetCountLetters(word, ch);
                int timeOnList = GetCountLetters(list, ch);

                if (timeOnList < timeOnWord)
                {
                    AddLetterMultipleTimes(ch, Math.Abs(timeOnList - timeOnWord));
                }

            }

        }


    }
}