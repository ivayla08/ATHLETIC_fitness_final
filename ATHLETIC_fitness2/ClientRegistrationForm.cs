using athelticFitness.Controllers;
using athletic_fitness.Data.Entities;
using athletic_fitness.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATHLETIC_fitness2
{
    public partial class ClientRegistrationForm : Form
    {
        public ClientRegistrationForm()
        {
            InitializeComponent();
           
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox6.Text) || string.IsNullOrWhiteSpace(textBox7.Text))
            {
                MessageBox.Show("All fields are required!");
            }
            else 
            {
                if(textBox6.Text!=textBox7.Text)
                {
                    MessageBox.Show("Invalid passoword");
                }
                else
                {
                    UserController userController = new UserController();
                    ClientController clientController = new ClientController();

                    User newUser = new User
                    {
                        Username = textBox5.Text,
                        Password = textBox6.Text,
                        Role = RoleType.Client
                    };

                    User createdUser = await userController.CreateUser(newUser);

                    Client newClient = new Client
                    {
                        FirstName = textBox1.Text,
                        LastName = textBox2.Text,
                        Email = textBox4.Text,
                        Phone = textBox3.Text,
                        UserId = createdUser.Id
                    };

                    await clientController.CreateClient(newClient);

                    MessageBox.Show("Registration successful!");
                    this.Close();

                    ClientForm fom = new ClientForm();
                    fom.ShowDialog();
                }                    
            }
           
        }

        private void ClientRegistrationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
