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
            //string username = textBox1.Text.Trim();
            //string password = textBox2.Text.Trim();

            //UserController userController = new UserController();
            //List<User> users = await userController.GetAllUsers();           

            //User user = users.FirstOrDefault(x =>
            //    x.Username.Trim() == username &&
            //    x.Password.Trim() == password);

            //if (user != null)
            //{           
            //    this.Hide();

            //    if (user.Role == RoleType.Admin)
            //    {                    
            //        AdminForm adminForm = new AdminForm();
            //        adminForm.Show();
            //    }
            //    else if (user.Role == RoleType.Client)
            //    {
            //        ClientForm clientForm = new ClientForm();
            //        clientForm.Show();
            //    }
            //    else if (user.Role == RoleType.Coach)
            //    {
            //        CoachController coachController = new CoachController();    
            //        Coach coach=await coachController.GetCoachByUserId(user.Id);
            //        if (coach != null) 
            //        {
            //            CoachForm coachForm = new CoachForm();
            //            coachForm.Show();
            //        }
            //        else
            //        {
            //            CoachRegistration coachRegistration = new CoachRegistration(user);
            //            coachRegistration.Show();
            //        }

            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //}

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
