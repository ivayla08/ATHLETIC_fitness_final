using athelticFitness.Controllers;
using athletic_fitness.Data.Entities;
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
    public partial class CoachRegistration : Form
    {
        private User loggedUser; 

        public CoachRegistration(User user)
        {
            InitializeComponent();
            LoadCombos();
            loggedUser = user;
        }
        private async void LoadCombos()
        {
            GymController controller = new GymController();
            comboBox1.DataSource= await controller.GetAllGyms();
            comboBox1.DisplayMember = "FullAddress";
        }

        private void CoachRegistration_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Invalid first name");
                return;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Invalid last name");
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Invalid phone");
                return;
            }
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Invalid email");
                return;
            }
            CoachController coachController = new CoachController();
            string firstName=textBox1.Text;
            string lastName=textBox2.Text;
            string phone=textBox3.Text;
            string email=textBox4.Text;
            Gym gym =(Gym) comboBox1.SelectedItem;
            Coach coach = new Coach
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                Email = email,
                GymId = gym.Id,
                UserId=loggedUser.Id
            };
            await coachController.CreateCoach(coach);
            MessageBox.Show("Successful registration!");

            this.Hide();

            CoachForm coachForm = new CoachForm();
            coachForm.ShowDialog();
        }
    }
}
