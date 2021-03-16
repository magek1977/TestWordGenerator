using System;
using System.Collections.Generic;
using System.Text;

namespace TestWordGenerator
{
    public class PasswordGenerator
    {
        private static String validChars = "abcdefghijklmnopqrstuvwxyzåäöABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ0123456789!?";
        private static int validCharsCount;

//        private readonly object generatorLock = new object();

        private List<int> listOfNumbers = new List<int>();
        string text;
//        Linker l = new Linker(validChars);
        public PasswordGenerator(string wordToStartFrom)
        {
            validCharsCount = validChars.Length;

            text = wordToStartFrom;
            if (wordToStartFrom == "")
            {
                listOfNumbers.Add(0);
            }
            else
            {
                convertToNumbers(wordToStartFrom);
            }
        }
        public static String getFirstWord() {
            return Char.ToString(validChars[0]);
        }
        public void Next()
        {
            listOfNumbers[0] += 1;
            checkOverflow();
            transLateToLetters();
        }

        private void checkOverflow()
        {
            int i = 0;
            do
            {
                if (listOfNumbers[i] > validCharsCount)
                {
                    int carryOver = listOfNumbers[i] / validCharsCount;
                    int rest = listOfNumbers[i] % validCharsCount;
                    listOfNumbers[i] = rest;

                    if (rest == 0)
                    {
                        carryOver-=1;
                        listOfNumbers[i] = validCharsCount;
                    }

                    if (carryOver > 0)
                    {
                        if (i + 1 < listOfNumbers.Count) //space to carry
                        {
                            listOfNumbers[i + 1] += carryOver;
                        }
                        else
                        {
                            listOfNumbers.Add(carryOver);
                        }
                    }
                }
                i++;
            } while (i < listOfNumbers.Count);
        }

        public string GetCurrentWord()
        {
            return text;
        }

        public void fastForward(int count)
        {
            listOfNumbers[0] += count;
            checkOverflow();
            transLateToLetters();
        }
        private void transLateToLetters()
        {
            int[] reversed = new int[listOfNumbers.Count];
            StringBuilder builder = new StringBuilder();
            listOfNumbers.CopyTo(reversed);
            Array.Reverse(reversed);

            for (int i = 0; i < reversed.Length; i++)
            {
                builder.Append(NumberToString(reversed[i]));
            }
            text = Convert.ToString(builder);
        }
        private void convertToNumbers(string lastEntry)
        {
            char[] entry;
            entry = lastEntry.ToCharArray();
            for (int i = 0; i < entry.Length; i++)
            {
                listOfNumbers.Add(StringToNumber(entry[i]));

            }
            listOfNumbers.Reverse();
        }

        private string NumberToString(int i) {
            return validChars[i - 1].ToString();
        }
        private int StringToNumber(char letter) {
            return validChars.IndexOf(letter) + 1;
        }
    }
}
