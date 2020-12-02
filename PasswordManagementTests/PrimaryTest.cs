using Microsoft.VisualStudio.TestTools.UnitTesting;
using Password_Management_Software;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.AccessControl;
using System.Security.Cryptography;


namespace PasswordManagementTests
{
    [TestClass]
    public class PrimaryTest
    {
        [TestMethod]
        public void TestEncryption()
        {
            byte[] key_IV = { 31, 32, 33, 34, 35, 36, 37, 38, 39, 30, 31, 32, 33, 34, 35, 36 };

            String expected = "JSb3jQ6UmK+32dJhJZr2Zw==";

            Form1 frm = new Form1();

            var result = Convert.ToBase64String(frm.encrypt_passwords("Hello World", key_IV, key_IV));

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestDecryption() {
            byte[] key_IV = { 31, 32, 33, 34, 35, 36, 37, 38, 39, 30, 31, 32, 33, 34, 35, 36 };

            byte[] password = Convert.FromBase64String("JSb3jQ6UmK+32dJhJZr2Zw==");

            String expected = "Hello World";

            Form1 frm = new Form1();

            var result = frm.decrypt_passwords(password, key_IV, key_IV);

            Assert.AreEqual(expected, result);
        }
    }
}
