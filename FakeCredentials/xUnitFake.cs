using System;
using System.IO;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace FakeCredentials
{
    public class xUnitFake
    {
        [Fact]
        public void FakeTest2()
        {
            // Arrange
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string configName = Settings.DefaultConfigName;
            configName = Path.Combine(baseDirectory, configName);
            string sqliteFile = "webgoat_coins.sqlite";
            sqliteFile = Path.Combine(baseDirectory, sqliteFile);
            string[] lines =
            {
                "dbtype=Sqlite",
                "filename=" + sqliteFile
            };
            File.WriteAllLines(configName, lines);
            ConfigFile configFile = new ConfigFile(configName);
            configFile.Load();
            IDbProvider dbProvider = DbProviderFactory.Create(configFile);
            string fakeEmail = "someone@somewhere";
            string fakePassword = DateTime.Now.ToString();
            string goodEmail = "bob@ateliergraphique.com";
            string goodPassword = Encoder.Decode("MTIzNDU2");
            string hackEmail = "' or 1 = 1 --";
            string hackPassword = "";

            // Act
            bool loginOk = dbProvider.IsValidCustomerLogin(fakeEmail, fakePassword);
            bool loginFail = dbProvider.IsValidCustomerLogin(goodEmail, goodPassword);
            bool hackFail = dbProvider.IsValidCustomerLogin(hackEmail, hackPassword);

            // Assert
            Assert.True(loginOk);
            Assert.False(loginFail);
            Assert.False(hackFail);
        }
    }
}
