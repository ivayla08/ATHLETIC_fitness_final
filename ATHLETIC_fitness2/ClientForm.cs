using athelticFitness.Controllers;
using athletic_fitness.Data.Entities;
using athletic_fitness.Enums;
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
    public partial class ClientForm : Form
    {
        User currentUser = Session.LoggedUser;
        ReservationController reservationController = new ReservationController();
        public ClientForm()
        {
            InitializeComponent();
            LoadCombos();
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private async void LoadCombos()
        {
            GymController controller = new GymController();
            comboBox1.DataSource = await controller.GetAllGyms();
            comboBox1.DisplayMember = "FullAddress";
            if (comboBox1.SelectedItem != null)
            {
                Gym gym = (Gym)comboBox1.SelectedItem;
                WorkoutController workoutController = new WorkoutController();
                comboBox2.DataSource = await workoutController.GetWorkoutsForGym(gym.Id);
                comboBox2.DisplayMember = "WorkoutInfo";
            }
            else
            {
                MessageBox.Show("Select a gym!");
            }
            comboBox3.Items.Add("ANNUAL MEMBERSHIP €249.99");
            comboBox3.Items.Add("MONTHLY MEMBERSHIP €29.99");
            comboBox3.Items.Add("3 MONTH MEMBERSHIP €69.99");
            comboBox3.Items.Add("ONE-TIME WORKOUT €4.99");

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

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            try
            {
                ClientController clientController = new ClientController();
                Workout workout = (Workout)comboBox2.SelectedItem;
                Client client = await clientController.GetClientByUserId(currentUser.Id);
                Reservation reservation = new Reservation
                {
                    WorkoutId = workout.Id,
                    ClientId = client.Id,

                };
                await reservationController.CreateReservation(reservation);
                MessageBox.Show("You successfully booked a workout class!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            ClientController clientController = new ClientController();
            Client client = await clientController.GetClientByUserId(currentUser.Id);
            UserController controller = new UserController();
            if (textBox1.Text != currentUser.Username || textBox2.Text != currentUser.Password)
            {
                MessageBox.Show("Invalid username or password");
            }
            else
            {
                currentUser.Password = textBox3.Text;
                client.FirstName = textBox5.Text;
                client.LastName = textBox6.Text;
                client.Email = textBox7.Text;
                client.Phone = textBox8.Text;
                bool result = await controller.UpdateUser(currentUser);
                bool result2 = await clientController.UpdateClient(client);
                if (result && result2)
                {
                    MessageBox.Show("Password updated!");
                }
                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            List<Reservation> myRes = await reservationController.GetAllReservations(currentUser.Id);
            foreach (var item in myRes)
            {
                if (item.Workout.DateTime > DateTime.Now)
                {
                    richTextBox1.AppendText($"{item.Workout.Name} on {item.Workout.DateTime} in {item.Workout.Gym.FullAddress}" + Environment.NewLine);
                }

            }
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            List<Reservation> myRes = await reservationController.GetAllReservations(currentUser.Id);
            foreach (var item in myRes)
            {
                if (item.Workout.DateTime < DateTime.Now)
                {
                    richTextBox1.AppendText($"{item.Workout.Name} on {item.Workout.DateTime} in {item.Workout.Gym.FullAddress}" + Environment.NewLine);
                }

            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        string type;
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                richTextBox3.Clear();
                richTextBox3.AppendText($"Validity: 12 months\r\nValid for all fitness clubs\r\nGym with fitness equipment\r\n1 Group training per day\r\nOption to freeze membership 2 times for a total of 30 days\r\n2 hours free parking\r\nAccess to Athletic Online gym for the period of the card");
                type = "Year";
            }
            else if (comboBox3.SelectedIndex == 1)
            {
                richTextBox3.Clear();
                type = "Month";
                richTextBox3.AppendText("Validity: 1 month\r\nValid for all fitness clubs\r\nGym with fitness equipment\r\n1 Group training per day\r\n2 hours of free parking\r\nAccess to Athletic Online gym for the period of the card\r\nLoyalty program\r\nWhen renewing the card before the expiration date, a 10% discount is applied.\r\n");
            }
            else if (comboBox3.SelectedIndex == 2)
            {
                richTextBox3.Clear();
                type = "ThreeMonths";
                richTextBox3.AppendText("Validity: 3 months\r\nValid for all fitness clubs\r\nGym with fitness equipment\r\n1 Group training per day\r\n2 hours of free parking\r\nAccess to Athletic Online gym for the period of the card\r\nLoyalty program\r\nWhen renewing the card before the expiration date, a 10% discount is applied.\r\n");
            }
            else if (comboBox3.SelectedIndex == 3)
            {
                richTextBox3.Clear();
                type = "Day";
                richTextBox3.AppendText("Gym with fitness equipment\r\n1 Group workout\r\n2 hours free parking\r\nLoyalty program\r\n");
            }
        }

        private async void button16_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox9.Text) && !string.IsNullOrWhiteSpace(textBox10.Text) && !string.IsNullOrWhiteSpace(textBox11.Text) && !string.IsNullOrWhiteSpace(textBox12.Text))
            {
                ClientController clientController = new ClientController();
                Client client = await clientController.GetClientByUserId(currentUser.Id);

                MembershipController membershipController = new MembershipController();

                if (!Enum.TryParse(type, out MembershipType memType))
                {
                    MessageBox.Show("Invalid membership type");
                    return;
                }
                DateTime startDate = dateTimePicker1.Value;
                DateTime endDate = DateTime.Now;
                decimal price = 0;
                switch (memType)
                {
                    case MembershipType.Day:
                        endDate = startDate.AddDays(1);
                        price = 4.99m;
                        break;
                    case MembershipType.Month:
                        endDate = startDate.AddMonths(1);
                        price = 29.99m;
                        break;
                    case MembershipType.ThreeMonths:
                        endDate = startDate.AddMonths(3);
                        price = 69.99m;
                        break;
                    case MembershipType.Year:
                        endDate = startDate.AddYears(1);
                        price = 249.99m;
                        break;
                }


                Membership membership = new Membership
                {
                    MembershipType = memType,
                    StartDate = startDate,
                    EndDate = endDate,
                    Price = price,
                    ClientId = client.Id,

                };
                await membershipController.CreateMembership(membership);
                MessageBox.Show("Successful payment");
            }
            else
            {
                MessageBox.Show("Invalid card data");
            }
        }

        private async void button17_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 7;
            MembershipController membershipController = new MembershipController();
            ClientController clientController = new ClientController();
            Client client = await clientController.GetClientByUserId(currentUser.Id);
            Membership? mem = await membershipController.GetMembershipByCleintId(client.Id);

            if (mem == null)
            {
                richTextBox4.AppendText($"No active membership");
            }
            else
            {
                richTextBox4.AppendText($"Type: {mem.MembershipType} \n{mem.StartDate} - {mem.EndDate}\nPrice: {mem.Price}");
            }

        }

        private void button18_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
    }
}
