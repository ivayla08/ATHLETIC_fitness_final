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
    public partial class AdminForm : Form
    {
        CoachController coachController = new CoachController();
        GymController gymController = new GymController();
        ClientController clientController = new ClientController();
        WorkoutController workoutController = new WorkoutController();
        UserController userController = new UserController();
        MembershipController membershipController = new MembershipController();

        public AdminForm()
        {
            InitializeComponent();
            LoadCombos();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {

        }
        private async void LoadCombos()
        {
            comboBox1.Items.Add("Coach");
            comboBox1.Items.Add("Admin");

            comboBox2.Items.Add(LevelType.Begginer);
            comboBox2.Items.Add(LevelType.Advanced);
            comboBox2.Items.Add(LevelType.Professional);

            comboBox3.DataSource = await gymController.GetAllGyms();
            comboBox3.DisplayMember = "FullAddress";

            if (comboBox3.SelectedItem != null)
            {
                Gym gym = (Gym)comboBox3.SelectedItem;

                comboBox4.DataSource = await coachController.GetAllCoachesByGym(gym.Id);
                comboBox4.DisplayMember = "FullName";
            }

            comboBox5.DataSource = await gymController.GetAllGyms();
            comboBox5.DisplayMember = "FullAddress";

            comboBox6.DataSource = await coachController.GetAllCoaches();
            comboBox6.DisplayMember = "FullName";

            comboBox7.DataSource = await clientController.GetAllClients();
            comboBox7.DisplayMember = "FullName";

            comboBox8.DataSource = await workoutController.GetWorkouts();
            comboBox8.DisplayMember = "Name";

            comboBox9.DataSource = await gymController.GetAllGyms();
            comboBox9.DisplayMember = "FullAddress";


        }

        private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }

        private async void button9_Click(object sender, EventArgs e)
        {
            UserController userController = new UserController();
            CoachController coachController = new CoachController();

            string username = textBox1.Text;
            string password = textBox2.Text;

            if(string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || comboBox1.SelectedItem == null)
            {
                MessageBox.Show("All fields are required!");
                return;
            }

            if (comboBox1.SelectedItem.ToString() == RoleType.Admin.ToString())
            {
                User user = new User
                {
                    Username = username,
                    Password = password,
                    Role = RoleType.Admin
                };

                await userController.CreateUser(user);
                MessageBox.Show("Admin added");
            }
            else if (comboBox1.SelectedItem.ToString() == RoleType.Coach.ToString())
            {
                User user = new User
                {
                    Username = username,
                    Password = password,
                    Role = RoleType.Coach
                };

                await userController.CreateUser(user);
                CoachRegistration form = new CoachRegistration(user);
                MessageBox.Show("Coach added");
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            GymController gymController = new GymController();
            string city = textBox4.Text;
            string address = textBox5.Text;
            if (string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text))
            {            
                MessageBox.Show("All fields are required!");
                return;
            }
            Gym gym = new Gym
            {
                City = city,
                Address = address
            };
            await gymController.CreateGym(gym);
            MessageBox.Show("Gym added");
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Enter a workout name.");
                return;
            }

            if (!DateTime.TryParse(textBox7.Text, out DateTime date))
            {
                MessageBox.Show("Invalid date and time format.");
                return;
            }

            if (!int.TryParse(textBox8.Text, out int dur) || dur <= 0)
            {
                MessageBox.Show("Enter a valid positive number for duration.");
                return;
            }

            if (!int.TryParse(textBox9.Text, out int capacity) || capacity <= 0)
            {
                MessageBox.Show("Enter a valid positive number for capacity.");
                return;
            }

            if (comboBox2.SelectedItem == null || comboBox3.SelectedItem == null || comboBox4.SelectedItem == null)
            {
                MessageBox.Show("Select Level, Gym, and Coach from the lists.");
                return;
            }

            LevelType level = (LevelType)comboBox2.SelectedItem;
            Gym gym = (Gym)comboBox3.SelectedItem;
            Coach coach = (Coach)comboBox4.SelectedItem;

            Workout workout = new Workout
            {
                Name = textBox6.Text,
                DateTime = date,
                Duration = dur,
                Capacity = capacity,
                Level = level,
                GymId = gym.Id,
                CoachId = coach.Id
            };

            WorkoutController workoutController = new WorkoutController();
            var result = await workoutController.AddWorkoutToScheduleAsync(workout);

            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Success", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show(result.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void button13_Click(object sender, EventArgs e)
        {
            try
            {
                Gym? gym = (Gym)comboBox5.SelectedItem;
                if (gym != null)
                {
                    await gymController.DeleteGym(gym.Id);
                    MessageBox.Show("Gym deleted");
                }
                else
                {
                    MessageBox.Show("Select a gym");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot delete gym");
            }

        }

        private async void button14_Click(object sender, EventArgs e)
        {
            try
            {
                Coach? coach = (Coach)comboBox6.SelectedItem;
                if (coach != null)
                {
                    await coachController.DeleteCoach(coach.Id);
                    MessageBox.Show("Coach deleted");
                }
                else
                {
                    MessageBox.Show("Select a coach");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot delete coach");
            }
        }

        private async void button15_Click(object sender, EventArgs e)
        {
            try
            {
                Workout? workout = (Workout)comboBox8.SelectedItem;
                if (workout != null)
                {
                    await workoutController.DeleteWorkout(workout.Id);
                    MessageBox.Show("Workout deleted");
                }
                else
                {
                    MessageBox.Show("Select a workout");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot delete workout");
            }
        }

        private async void button16_Click(object sender, EventArgs e)
        {
            try
            {
                UserController userController = new UserController();
                Client? client = (Client)comboBox7.SelectedItem;
                User user = await userController.GetUserById(client.UserId);
                if (client != null)
                {
                    await clientController.DeleteClient(client.Id);
                    await userController.DeleteUser(user.Id);
                    MessageBox.Show("Client deleted");
                }
                else
                {
                    MessageBox.Show("Select a client");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot delete client");
            }
        }

        private async void button17_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            List<User> users = await userController.GetAllUsers();
            foreach (User user in users)
            {
                richTextBox1.AppendText($"{user.Id} - {user.Username} {user.Password}" + Environment.NewLine);
            }
        }

        private async void button18_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            List<Workout> workouts = await workoutController.GetWorkouts();
            foreach (var item in workouts)
            {
                richTextBox1.AppendText($"{item.Name} {item.Coach.FirstName} {item.Coach.LastName} {item.DateTime}" + Environment.NewLine);
            }
        }

        private async void button19_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            List<Gym> gyms = await gymController.GetAllGyms();
            foreach (var item in gyms)
            {
                richTextBox1.AppendText($"{item.City} {item.Address}" + Environment.NewLine);
            }
        }

        private async void button20_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            List<Membership> memberships = await membershipController.GetAllMemberships();
            foreach (var item in memberships)
            {
                richTextBox1.AppendText($"{item.StartDate} - {item.EndDate} {item.Client.FirstName} {item.Client.LastName}" + Environment.NewLine);
            }

        }

        private void button21_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private async void button24_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox10.Text) || string.IsNullOrWhiteSpace(textBox11.Text) || string.IsNullOrWhiteSpace(textBox12.Text) || string.IsNullOrWhiteSpace(textBox13.Text) || string.IsNullOrWhiteSpace(textBox14.Text))
            {
                MessageBox.Show("All fields are required!");
                return;
            }

            int id = int.Parse(textBox10.Text);
            string username = textBox11.Text;
            string password = textBox12.Text;
            string newPass = textBox13.Text;            

            List<User> users = await userController.GetAllUsers();
            User currentUser = users.FirstOrDefault(x => x.Username == username && x.Id == id && x.Password == password);
            if (currentUser == null)
            {
                MessageBox.Show("Invalid user");
            }
            else
            {
                currentUser.Password = newPass;
                bool result = await userController.UpdateUser(currentUser);
                if (result)
                {
                    MessageBox.Show("User updated");
                }
                else
                {
                    MessageBox.Show("Cannot update user");
                }

            }


        }

        private async void button25_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox15.Text) || string.IsNullOrWhiteSpace(textBox16.Text) || string.IsNullOrWhiteSpace(textBox17.Text) || string.IsNullOrWhiteSpace(textBox18.Text))
            {
                MessageBox.Show("All fields are required!");
                return;
            }

            int id = int.Parse(textBox15.Text);
            string city = textBox16.Text;
            string address = textBox17.Text;
          
            List<Gym> gyms = await gymController.GetAllGyms();
            Gym currentGym = gyms.FirstOrDefault(x => x.Id == id && x.City == city && x.Address == address);
            if (currentGym == null)
            {
                MessageBox.Show("Invalid data");
            }
            else
            {
                currentGym.Address = textBox18.Text;
                bool result = await gymController.UpdateGym(currentGym);
                if (result)
                {
                    MessageBox.Show("Gym updated");
                }
                else
                {
                    MessageBox.Show("Cannot update gym");
                }
            }
        }

        private async void button26_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            List<Client> clients = await clientController.GetAllClients();
            List<Client> topTen = clients.OrderByDescending(x => x.Reservations.Count).Take(10).ToList();
            for (int i = 1; i < topTen.Count; i++)
            {
                richTextBox2.AppendText($"{i} - {topTen[i].FirstName} {topTen[i].LastName} Workouts: {topTen[i].Reservations.Count}" + Environment.NewLine);
            }

        }

        private async void button27_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            if (comboBox9.SelectedItem == null)
            {
                List<Coach> coaches = await coachController.GetAllCoaches();
                List<Coach> topTen = coaches.OrderByDescending(x => x.Workouts.Count).Take(10).ToList();
                for (int i = 0; i < topTen.Count; i++)
                {
                    richTextBox2.AppendText($"{i} - {topTen[i].FirstName} {topTen[i].LastName} {topTen[i].Gym.FullAddress}" + Environment.NewLine);
                }
            }
            else
            {
                Gym gym = (Gym)comboBox9.SelectedItem;
                List<Coach> coaches = await coachController.GetAllCoaches();
                List<Coach> topTen = coaches.Where(x => x.GymId == gym.Id).OrderByDescending(x => x.Workouts.Count).Take(10).ToList();
                for (int i = 0; i < topTen.Count; i++)
                {
                    richTextBox2.AppendText($"{i} - {topTen[i].FirstName} {topTen[i].LastName} {topTen[i].Gym.FullAddress}" + Environment.NewLine);
                }
            }

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
    }
}
