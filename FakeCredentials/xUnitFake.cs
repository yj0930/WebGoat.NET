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
        public void FakeTest1()
        {
            // Arrange
            ConfigFile configFile = new ConfigFile(null);

            // Act
            configFile.Set("key1", "value1");
            string value1 = configFile.Get("key1");

            // Assert
            Assert.Equal(value1, "value1");
        }

        [Fact]
        public void FakeTest2()
        {
            // Arrange
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string configName = Settings.DefaultConfigName;
            configName = Path.Combine(baseDirectory, configName);
            ConfigFile configFile = new ConfigFile(configName);
            configFile.Set("dbtype", "Sqlite");
            configFile.Set("filename", "webgoat_coins.sqlite");
            configFile.Save();
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
