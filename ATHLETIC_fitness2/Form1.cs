using athelticFitness.Controllers;
using athletic_fitness.Data;
using athletic_fitness.Data.Entities;
using athletic_fitness.Enums;
using athleticFitness.Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static athelticFitness.Controllers.UserController;

namespace ATHLETIC_fitness2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
      
        private async void button1_ClickAsync(object sender, EventArgs e)
        {          

            UserController userController = new UserController();

            LoginResult result =
                await userController.LoginAsync(
                    textBox1.Text,
                    textBox2.Text);

            MessageBox.Show(result.Message);

            if (result.Success)
            {
                Session.LoggedUser = result.User;

                if (result.User.Role == RoleType.Admin)
                {
                    AdminForm form = new AdminForm();
                    form.Show();
                }
                else if (result.User.Role==RoleType.Client)
                {
                    ClientForm form = new ClientForm();
                    form.Show();
                }
                else if (result.User.Role == RoleType.Coach)
                {
                    CoachController coachController =
                        new CoachController();

                    Coach coach =
                        await coachController
                        .GetCoachByUserId(result.User.Id);

                    if (coach != null)
                    {
                        CoachForm form = new CoachForm();
                        form.Show();
                    }
                    else
                    {
                        CoachRegistration form =
                            new CoachRegistration(result.User);

                        form.Show();
                    }
                }

                this.Hide();
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ClientRegistrationForm registrationForm = new ClientRegistrationForm();
            registrationForm.ShowDialog();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
