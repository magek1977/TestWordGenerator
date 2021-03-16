using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestWordGenerator
{
    class ThreadManager
    {
        private static object SyncLock = new Object();

        private static int activeTreadCount = 0;
        public  static int ActiveTreadCount
        {
            get {
                lock (SyncLock){
                    return activeTreadCount;
                }
            }
            set{
                lock (SyncLock){
                    activeTreadCount = value;
                }
            }
        }


        public void run()
        {
            int maxNumberOfRunningThreads = Environment.ProcessorCount - 1;
            int noInEachGoup = 10000;

            DatabaseManager dbManager = new DatabaseManager();
            dbManager.init(PasswordGenerator.getFirstWord());
            PasswordGenerator pwg = new PasswordGenerator(dbManager.NextPassword);


            Dictionary<int, List<PasswordHashData>> threadPasswordHashData = new Dictionary<int, List<PasswordHashData>>();

            Console.WriteLine("Processing... press any key to quit");

            while (!Console.KeyAvailable){
//                var sw = Stopwatch.StartNew();

                threadPasswordHashData.Clear();

                for (int i = 0; i < maxNumberOfRunningThreads; i++)
                {
                    int treadId = i;
                    threadPasswordHashData.Add(treadId, new List<PasswordHashData>());
                    String currentWord = pwg.GetCurrentWord();

                    Thread thread = new Thread(() => createHash(treadId, currentWord, noInEachGoup, threadPasswordHashData[treadId]));
                    ActiveTreadCount++;
                    thread.Start();

                    pwg.fastForward(noInEachGoup);
                }

                // Wait for all threads to finish
                while (ActiveTreadCount > 0) {

                    System.Threading.Thread.Sleep(100);
                }

                // Add passwords from all thread to list processedPasswords
                List<PasswordHashData> processedPasswords = new List<PasswordHashData>();
                for (int i = 0; i < maxNumberOfRunningThreads; i++){
                    List<PasswordHashData> passwordHashData = threadPasswordHashData[i];
                    processedPasswords.AddRange(passwordHashData);
                }

                // Save processed passwords to database
                dbManager.save(processedPasswords, pwg.GetCurrentWord());

                Console.Write("\rPasswords: {0:N0}\tCurrent password: {1}", dbManager.NoOfProcessedPasswords, dbManager.NextPassword);

            }

        }
        private void createHash(int threadID, String firstPassword, int noOfPasswords, List<PasswordHashData> processedPasswords) {
            var sw = Stopwatch.StartNew();
            //            DateTime StartDateTime = DateTime.Now;
            try {
                PasswordGenerator wg = new PasswordGenerator(firstPassword);
                HashCalculator hashCalculator = new HashCalculator();
                //                SHA1Managed crypt = new SHA1Managed();

                for (int i = 0; i < noOfPasswords; i++) {
                    PasswordHashData passwordHashData = new PasswordHashData();
                    passwordHashData.Password = wg.GetCurrentWord();
                    hashCalculator.calculateHash(passwordHashData);
                    processedPasswords.Add(passwordHashData);
                    wg.Next();
                }
            } finally {
                //Console.WriteLine(@"Thread {0}: Time Taken in miliseconds {1}", threadID, sw.ElapsedMilliseconds);

                ActiveTreadCount--;
            }
        }

    }
}
