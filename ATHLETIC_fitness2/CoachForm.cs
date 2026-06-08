using athelticFitness.Controllers;
using athletic_fitness.Data.Entities;
using athleticFitness.Data.Data.Entities;
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
    public partial class CoachForm : Form
    {
        public CoachForm()
        {
            InitializeComponent();
        }

        private void CoachForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            int userId = Session.LoggedUser.Id;
            richTextBox1.Clear();
            WorkoutController workoutController = new WorkoutController();
            CoachController controller = new CoachController();
            Coach coach = await controller.GetCoachByUserId(userId);
            List<Workout> workouts = await workoutController.GetWorkoutsForCoach(coach.Id);
            List<Workout> selectedWorkouts = workouts.Where(x => x.DateTime.Date == dateTimePicker1.Value.Date).ToList();
            if (selectedWorkouts.Count == 0)
            {
                richTextBox1.AppendText($"No workouts on {dateTimePicker1.Value:dd-MM-yyyy}");
            }
            foreach (var item in selectedWorkouts)
            {
                richTextBox1.AppendText($"{item.Name} Capacity: {item.Capacity} Duration: {item.Duration}" + Environment.NewLine);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Invalid first name");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Invalid last name");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Invalid email");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Invalid phone");
                return;
            }
            int userId = Session.LoggedUser.Id;
            CoachController controller = new CoachController();
            Coach currentCoach = await controller.GetCoachByUserId(userId);
            if (currentCoach == null)
            {
                MessageBox.Show("Something went wrong!");
            }
            else
            {
                currentCoach.FirstName = textBox1.Text;
                currentCoach.LastName = textBox2.Text;
                currentCoach.Email = textBox4.Text;
                currentCoach.Phone = textBox3.Text;
                bool result = await controller.UpdateCoach(currentCoach);
                if (result)
                {
                    MessageBox.Show("Coach updated");
                }
                else
                {
                    MessageBox.Show("Cannot update coach");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }
    }
}
