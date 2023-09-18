using System;
using System.IO;
using System.Text.RegularExpressions;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using static System.Net.Mime.MediaTypeNames;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        public LevelInfo LoadLevelData(int levelIndex)
        {
            WordLevelJsonFile file = new WordLevelJsonFile(levelIndex);
            ParserLevelFile levelString = new ParserLevelFile(file.Text);
            
            string[] levelWords = levelString.LevelText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            LevelInfo info = new LevelInfo();
            for (int i = 0; i < levelWords.Length; i++)
            {
                info.words.Add(levelWords[i]);
            }

            return info;
        }

    }

    public class ParserLevelFile
    {
        private string textFile;
        public string LevelText { get => textFile; }

        public ParserLevelFile(string text)
        {
            textFile = GetLevelClearText(text);
        }

        private string DeleteRegularPart(string levelText, Regex pattern)
        {
            string result;
            result = pattern.Replace(levelText, " ");

            return result;
        }

        private string GetLevelClearText(string textFromFile)
        {
            string text = textFromFile;
            Regex[] regularRules = new Regex[4];

            regularRules[0] = new Regex(@"[{]\s+""words""\s+:\s+");
            regularRules[1] = new Regex(@"[[,""}]");
            regularRules[2] = new Regex(@"[]]");
            regularRules[3] = new Regex(@"\s+");

            for (int i = 0; i < regularRules.Length; i++)
            {
                text = DeleteRegularPart(text, regularRules[i]);
            }
            text.Trim();

            return text;
        }


    }

    public class WordLevelJsonFile
    {
        private string pathWordFilesDirectory = "Assets/App/Resources/WordSearch/Levels/";
        private string wordFilenameExtension = ".json";

        private string textFile;

        public string Text { get => textFile; }

        public WordLevelJsonFile(int levelIndex)
        {
            string pathWordLevelFile;
            pathWordLevelFile = pathWordFilesDirectory + levelIndex.ToString() + wordFilenameExtension;
            textFile = GetLevelText(pathWordLevelFile);
        }

        private string GetLevelText(string pathLevelTextFile)
        {
            string text;
            using (StreamReader readerTextFile = new StreamReader(pathLevelTextFile))
            {
                return text = readerTextFile.ReadToEnd();
            }

            throw new Exception();
        }

    }
}