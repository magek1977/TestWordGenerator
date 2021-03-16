using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWordGenerator.Database;

namespace TestWordGenerator {
    class DatabaseManager {
        public String NextPassword { get; set; }
        public long NoOfProcessedPasswords { get; set; }
        public void init(String defaultPassword) {

            using (var db = new HashingContext()) {
                ProcessingInfo processingInfo = db.ProcessingInfo.FirstOrDefault();
                if (processingInfo == null) {
                    db.Add(new ProcessingInfo { NextPassword = defaultPassword, NoOfProcessedPasswords = 0 });
                    db.SaveChanges();
                    NextPassword = defaultPassword;
                    NoOfProcessedPasswords = 0;
                } else {
                    NextPassword = processingInfo.NextPassword;
                    NoOfProcessedPasswords = processingInfo.NoOfProcessedPasswords;
                }
            }
        }

        public void save(List<PasswordHashData> passwordHashDataList, String nexPassword) {
            bulkSave(passwordHashDataList);
            saveProgress(passwordHashDataList.Count, nexPassword);
        }

        private void saveProgress(long noOfProcessedPasswords, String nexPassword) {

            using (var db = new HashingContext()) {
                ProcessingInfo processingInfo = db.ProcessingInfo.FirstOrDefault();
                processingInfo.NextPassword = nexPassword;
                processingInfo.NoOfProcessedPasswords += noOfProcessedPasswords;
                db.SaveChanges();

                NoOfProcessedPasswords = processingInfo.NoOfProcessedPasswords;
                NextPassword = processingInfo.NextPassword;
            }
        }

        private void bulkSave(List<PasswordHashData> passwordHashDataList) {
            
            using (var bulkCopy = new SqlBulkCopy(HashingContext.connectionString)) {
                bulkCopy.DestinationTableName = "dbo.PasswordHashes";
                bulkCopy.ColumnMappings.Add(nameof(PasswordHashData.Password), "Password");
                bulkCopy.ColumnMappings.Add(nameof(PasswordHashData.HashSha256), "HashSha256");
                bulkCopy.ColumnMappings.Add(nameof(PasswordHashData.HashSha384), "HashSha384");
                bulkCopy.ColumnMappings.Add(nameof(PasswordHashData.HashSha512), "HashSha512");
                bulkCopy.WriteToServer(toDataTable(passwordHashDataList));
            }
        }

        private DataTable toDataTable(List<PasswordHashData> passwordHashDataList) {
           
            DataTable dt = new DataTable();
            dt.Columns.Add("Password");
            dt.Columns.Add("HashSha256");
            dt.Columns.Add("HashSha384");
            dt.Columns.Add("HashSha512");

            object[] row = new object[4];
            foreach (var data in passwordHashDataList) {
                row[0] = data.Password;
                row[1] = data.HashSha256;
                row[2] = data.HashSha384;
                row[3] = data.HashSha512;
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
