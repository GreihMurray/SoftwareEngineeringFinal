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
using System.Security.Cryptography;

namespace Password_Management_Software
{
    public partial class Form1 : Form
    {
        int passwords = -1;
        int location = 0;
        List<RadioButton> radioButtons = new List<RadioButton>();
        List<TextBox> Catagoriesboxes = new List<TextBox>();
        List<TextBox> Namelist = new List<TextBox>();
        List<RadioButton> tmpradioButtons = new List<RadioButton>();
        List<TextBox> tmpCatagoriesboxes = new List<TextBox>();
        List<TextBox> tmpNamelist = new List<TextBox>();
        List<string> catagorylist = new List<string>();
        int clickedbutton = 0;
        bool number = false;
        bool uppercase = false;

        public Form1()
        {
            InitializeComponent();
            load_file();
        }

        private void button1_Click(object sender, EventArgs e)//sets the new deatils of the password
        {
            if (clickedbutton < 0 || textBox6.Text == "")
            {
                return;
            }
            else
            {
                if (textBox2.Text != "")
                {
                    radioButtons[location].Text = textBox2.Text;
                }
                else
                {
                    radioButtons[location].Text = "No Password";
                }
                Namelist[location].Text = textBox6.Text;
                Catagoriesboxes[location].Text = comboBox1.Text;
            }
            refresh();
            textBox2.Text = "";
            textBox1.Text = "";
            textBox6.Text = "";
            comboBox1.Text = "";
        }

        private void create_new_name(String name)//creates new name box
        {
            TextBox textBox = new TextBox();
            textBox.AutoSize = true;
            textBox.Location = new System.Drawing.Point(200, (10 + passwords * 20));
            textBox.Name = name;
            textBox.Size = new System.Drawing.Size(180, 19);
            textBox.TabIndex = 1;
            textBox.TabStop = true;
            textBox.Text = name;
            textBox.BackColor = System.Drawing.SystemColors.Control;
            textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            Controls.Add(textBox);
            panel1.Controls.Add(textBox);
            tmpNamelist.Add(textBox);
            Namelist.Add(textBox);
        }

        private void create_new_category(String category)//creates new category box
        {
            TextBox textBox = new TextBox();
            textBox.AutoSize = true;
            textBox.Location = new System.Drawing.Point(10, (10 + passwords * 20));
            textBox.Name = category;
            textBox.Size = new System.Drawing.Size(100, 19);
            textBox.TabIndex = 1;
            textBox.TabStop = true;
            textBox.Text = category;
            textBox.BackColor = System.Drawing.SystemColors.Control;
            textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            Controls.Add(textBox);
            panel1.Controls.Add(textBox);
            tmpCatagoriesboxes.Add(textBox);
            Catagoriesboxes.Add(textBox);
            catagorylist.Add(category);
        }

        private void create_new_password(string password)//creates new password buttons on the screen
        {
            if (passwords < 0)
            {
                passwords = 0;
            }
            RadioButton radioButton = new RadioButton();
            radioButton.AutoSize = true;
            radioButton.Location = new System.Drawing.Point(400, (10 + passwords * 20));
            radioButton.Name = "" + passwords;
            radioButton.Size = new System.Drawing.Size(190, 19);
            radioButton.TabIndex = 1;
            radioButton.TabStop = true;
            if (password == "")
            {
                radioButton.Text = "New Password" + passwords;
            }
            else
            {
                radioButton.Text = password;
            }

            radioButton.UseVisualStyleBackColor = true;
            radioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            passwords = passwords + 1;
            Controls.Add(radioButton);
            panel1.Controls.Add(radioButton);
            tmpradioButtons.Add(radioButton);
            radioButtons.Add(radioButton);
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)//puts the passwords details into the editing area textbox
        {

            RadioButton radioButton = (RadioButton)sender;
            for (int i = 0; i < Namelist.Count(); i++)
            {
                //Namelist[i].Text == radioButton.Text&& Catagoriesboxes[i].Text == Catagoriesboxes[clickedbutton].Text && radioButtons[i].Text == radioButtons[clickedbutton].Text
                if (radioButton.Text == radioButtons[i].Text && Catagoriesboxes[i].Text == radioButton.AccessibleDescription && Namelist[i].Text == radioButton.AccessibleName)
                {
                    location = i;
                    break;
                }
            }
            textBox1.Text = radioButton.Text;
            clickedbutton = Int32.Parse(radioButton.Name);
            textBox6.Text = tmpNamelist[clickedbutton].Text;
            comboBox1.Text = tmpCatagoriesboxes[clickedbutton].Text;
        }

        private void create_new_password_from_file(string password, string name, string category)//creates new password buttons on the screen
        {
            if (passwords < 0)
            {
                passwords = 0;
            }
            if (name == "")
            {
                create_new_name("No Name");
            }
            else
            {
                create_new_name(name);
            }
            if (category == "" || category == "Uncategorized")
            {
                create_new_category("Uncategorized");
            }
            else
            {
                if (checkformatch(category) == true)
                {
                    comboBox1.Items.AddRange(new object[] { category });
                    comboBox4.Items.AddRange(new object[] { category });
                    comboBox2.Items.AddRange(new object[] { category });
                }
                create_new_category(category);
            }
            create_new_password(password);
        }

        private bool checkformatch(String catagory)//sees if there is already a catagory by that name
        {
            for (int i = 0; i < catagorylist.Count; i++)
            {
                if (catagory == catagorylist[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void button5_Click(object sender, EventArgs e)//creates new password buttons
        {
            create_new_name("No Name");
            create_new_category("Uncategorized");
            create_new_password("");
        }

        private void button7_Click(object sender, EventArgs e)//save passwords to the file
        {
            save();
        }

        private void save()
        {
            string filename = "passwords.txt";
            StreamWriter outputfile = new StreamWriter(filename);
            using (AesManaged myAes = new AesManaged())
            {
                outputfile.WriteLine("{0}", Convert.ToBase64String(myAes.Key));
                outputfile.WriteLine("{0}", Convert.ToBase64String(myAes.IV));
                for (int i = 0; i < radioButtons.Count; i++)
                {

                    outputfile.WriteLine(Catagoriesboxes[i].Text + ',' + Namelist[i].Text + "," + Convert.ToBase64String(encrypt_passwords(radioButtons[i].Text, myAes.Key, myAes.IV)));

                }
            }
            outputfile.Close();
        }

        private void hideFile(String filename)
        {
            FileInfo fInfo = new FileInfo(filename);

            fInfo.Attributes = FileAttributes.Hidden;
        }

        public byte[] encrypt_passwords(String password, byte[] Key, byte[] IV)
        {
            if (password == null || password.Length <= 0)
            {
                throw new ArgumentNullException("password");
            }
            if (Key == null || Key.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }
            if (IV == null || IV.Length <= 0)
            {
                throw new ArgumentNullException("IV");
            }

            byte[] encrypted;

            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(password);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;

        }

        private void load_file()
        {
            byte[] Key = { };
            byte[] IV = { };
            bool breaker = false;
            string filename = "passwords.txt";
            if (!File.Exists(filename))
            {
                MessageBox.Show("File does not exist");
                Console.WriteLine("File does not exist or empty");
                return;
            }
            else
            {
                StreamReader inputFile = new StreamReader(filename);

                if (File.ReadLines(filename).Count() < 3)
                {
                    breaker = true;
                }

                if (inputFile != null && breaker == false)
                {
                    String line;

                    Key = Convert.FromBase64String(inputFile.ReadLine());
                    IV = Convert.FromBase64String(inputFile.ReadLine());

                    using (AesManaged myAes = new AesManaged())
                    {

                        while ((line = inputFile.ReadLine()) != null)
                        {
                            string[] values = line.Split(',');//splits the data into its seperate values  
                            string password = values[2];
                            string name = values[1];
                            string category = values[0];
                            String decryptedPassword = decrypt_passwords(Convert.FromBase64String(password), Key, IV);
                            create_new_password_from_file(decryptedPassword, name, category);

                        }

                    }
                }

                inputFile.Close();//close file
                refresh();
            }
        }

        public String decrypt_passwords(byte[] password, byte[] Key, byte[] IV)
        {
            if (password == null || password.Length <= 0)
            {
                throw new ArgumentNullException("password");
            }
            if (Key == null || Key.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }
            if (IV == null || IV.Length <= 0)
            {
                throw new ArgumentNullException("IV");
            }

            String decryptedPassword = null;

            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(password))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            decryptedPassword = srDecrypt.ReadToEnd();
                        }
                    }

                }
            }
            return decryptedPassword;
        }

        private void button2_Click(object sender, EventArgs e)//password generator
        {
            Random rnd = new Random();
            textBox3.Text = "";
            string lowerletters = "abcdefghijklmnopqrstuvwxyz";
            string upperletters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (textBox4.Text == "")
            {
                return;
            }
            for (int i = 0; i < Int32.Parse(textBox4.Text); i++)
            {
                int roll = rnd.Next(0, 2);
                int upper = rnd.Next(0, 2);
                if (number == true && roll != 1)//add a number
                {
                    textBox3.Text = textBox3.Text + rnd.Next(0, 10);
                }
                else if (uppercase == true && upper != 1)//add a number
                {
                    textBox3.Text = textBox3.Text + upperletters[rnd.Next(0, 26)];
                }
                else//add a letter
                {
                    textBox3.Text = textBox3.Text + lowerletters[rnd.Next(0, 26)];
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)//adds a new catagory to the combo boxes
        {
            if (textBox5.Text == "")
            {
                return;
            }
            else
            {
                if (checkformatch(textBox5.Text) == true)
                {
                    comboBox1.Items.AddRange(new object[] { textBox5.Text });
                    comboBox2.Items.AddRange(new object[] { textBox5.Text });
                    comboBox4.Items.AddRange(new object[] { textBox5.Text });
                    catagorylist.Add(textBox5.Text);
                }
            }
            textBox5.Text = "";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) //pick search catagory, wipe screen and display search matches
        {
            refresh();
        }

        private void refresh()//refreshes the screen
        {
            passwords = 0;
            for (int i = 0; i < tmpradioButtons.Count(); i++)
            {
                Controls.Remove(tmpradioButtons[i]);
                panel1.Controls.Remove(tmpradioButtons[i]);
                Controls.Remove(tmpCatagoriesboxes[i]);
                panel1.Controls.Remove(tmpCatagoriesboxes[i]);
                Controls.Remove(tmpNamelist[i]);
                panel1.Controls.Remove(tmpNamelist[i]);
            }
            tmpNamelist.Clear();
            tmpCatagoriesboxes.Clear();
            tmpradioButtons.Clear();

            if (comboBox2.Text == "All")
            {
                for (int i = 0; i < radioButtons.Count(); i++)
                {
                    create_shiz(radioButtons[i].Text, Catagoriesboxes[i].Text, Namelist[i].Text);
                }
            }
            else
            {
                for (int i = 0; i < radioButtons.Count(); i++)
                {
                    if (comboBox2.Text == Catagoriesboxes[i].Text)
                    {
                        create_shiz(radioButtons[i].Text, Catagoriesboxes[i].Text, Namelist[i].Text);
                    }
                }
            }
        }

        private void create_shiz(string pass, string cat, string nam)
        {
            RadioButton radioButton = new RadioButton();
            radioButton.AutoSize = true;
            radioButton.AccessibleName = nam;
            radioButton.AccessibleDescription = cat;
            radioButton.Location = new System.Drawing.Point(400, (10 + passwords * 20));
            radioButton.Name = "" + passwords;
            radioButton.Size = new System.Drawing.Size(190, 19);
            radioButton.TabIndex = 1;
            radioButton.TabStop = true;
            radioButton.Text = pass;
            radioButton.UseVisualStyleBackColor = true;
            radioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            Controls.Add(radioButton);
            panel1.Controls.Add(radioButton);

            TextBox textBox = new TextBox();
            textBox.AutoSize = true;
            textBox.Location = new System.Drawing.Point(10, (10 + passwords * 20));
            textBox.Name = cat;
            textBox.Size = new System.Drawing.Size(100, 19);
            textBox.TabIndex = 1;
            textBox.TabStop = true;
            textBox.Text = cat;
            textBox.BackColor = System.Drawing.SystemColors.Control;
            textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            Controls.Add(textBox);
            panel1.Controls.Add(textBox);

            TextBox textBox2 = new TextBox();
            textBox2.AutoSize = true;
            textBox2.Location = new System.Drawing.Point(200, (10 + passwords * 20));
            textBox2.Name = nam;
            textBox2.Size = new System.Drawing.Size(180, 19);
            textBox2.TabIndex = 1;
            textBox2.TabStop = true;
            textBox2.Text = nam;
            textBox2.BackColor = System.Drawing.SystemColors.Control;
            textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            Controls.Add(textBox2);
            panel1.Controls.Add(textBox2);
            passwords = passwords + 1;

            tmpradioButtons.Add(radioButton);
            tmpCatagoriesboxes.Add(textBox);
            tmpNamelist.Add(textBox2);
        }

        private void button3_Click(object sender, EventArgs e)//IF UPPERCASE
        {
            if (uppercase == true)
            {
                uppercase = false;
            }
            else
            {
                uppercase = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)//if number
        {
            if (number == true)
            {
                number = false;
            }
            else
            {
                number = true;
            }
        }

        private void button9_Click(object sender, EventArgs e)//message box if you click exit
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to save before leaving?", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                save();
                Application.Exit();
            }
            else
            {
                Application.Exit();
            }
        }

        private void button10_Click(object sender, EventArgs e)//delete catagory
        {
            if (comboBox4.Text == "All" || comboBox4.Text == "")
            {

            }
            else
            {
                comboBox1.Items.Remove(comboBox4.Text);
                comboBox2.Items.Remove(comboBox4.Text);
                comboBox4.Items.Remove(comboBox4.Text);
            }
            for (int i = 0; i < Catagoriesboxes.Count(); i++) //error whenever it deletes, it only deletes the last catagory even when adding a new one after
            {
                if (Catagoriesboxes[i].Text == comboBox4.Text)
                {
                    Catagoriesboxes[i].Text = "test";
                }
            }
            refresh();
            comboBox4.Text = "";
        }

        private void button11_Click(object sender, EventArgs e)//delete password
        {
            if (textBox2.Text == "" || textBox1.Text == "" || textBox6.Text == "" || comboBox1.Text == "")
            {

            }
            else
            {
                radioButtons.RemoveAt(location);
                Namelist.RemoveAt(location);
                Catagoriesboxes.RemoveAt(location);
                refresh();
                textBox2.Text = "";
                textBox1.Text = "";
                textBox6.Text = "";
                comboBox1.Text = "";
            }
        }
    }
}
