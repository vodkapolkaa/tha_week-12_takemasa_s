using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace THA_Week_12_TAKEMASA_S
{
    public partial class Form1 : Form
    {
        MySqlConnection sqlConnection;
        MySqlCommand sqlCommand;
        MySqlDataAdapter sqlAdapter;
        MySqlDataReader sqlReader;


        string sqlQuery;
        DataTable teams = new DataTable();
        DataTable nation = new DataTable();
        DataTable players = new DataTable();
        DataTable manager = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connection = "server=localhost;uid=root;pwd=vodkai989;database=premier_league";
            sqlConnection = new MySqlConnection(connection);


            sqlQuery = "select team_name, team_id from team;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(teams);
            comboBoxteamplayer.DataSource = teams;
            comboBoxteamplayer.DisplayMember = "team_name";
            comboBoxteamplayer.ValueMember = "team_id";
            comboBox1.DataSource = teams;
            comboBoxteamsmanager.DataSource = teams;
            comboBoxteamsmanager.DisplayMember = "team_name";
            comboBoxteamsmanager.ValueMember = "team_id";
            comboBox1.DisplayMember = "team_name";
            comboBox1.ValueMember = "team_id";

            sqlQuery = "select nation, nationality_id from nationality;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(nation);
            comboBoxnation.DataSource = nation;
            comboBoxnation.ValueMember = "nationality_id";
            comboBoxnation.DisplayMember = "nation";

            players = new DataTable();
            sqlQuery = "select p.player_id, p.team_number, p.player_name, n.nation, p.playing_pos, p.height, p.weight, p.birthdate, t.team_name " +
                "from player p, nationality n, team t " +
                "where p.nationality_id = n.nationality_id and t.team_id = p.team_id and p.team_id = '" + comboBox1.SelectedValue + "' and p.status = 1;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(players);
            dataGridView3.DataSource = players;
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            string command = "insert into player value ('" + textBoxID.Text +  "','" + textBoxnumber.Text + "','" +  textBoxname.Text + "', '" +
                comboBoxnation.SelectedValue +"', '"+  comboBoxpositi.SelectedItem.ToString() +"'," +
                    "'" + textBoxheight.Text + "', '"+textBoxweight.Text+"', '"+dateTimePicker1.Text+"', '"+comboBoxteamplayer.SelectedValue+"', 1, 0)";
            try
            {
                sqlConnection.Open();
                sqlCommand = new MySqlCommand(command, sqlConnection);
                sqlReader = sqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void buttonremove_Click(object sender, EventArgs e)
        {

            if(dataGridView3.Rows.Count < 13)
            {
                MessageBox.Show("Limit 11");
            }
            else
            {
                string id = dataGridView3.CurrentRow.Cells[0].Value.ToString();
                label12.Text = id;
                string com = $"update player set status = 0 where player_id = '"+ id + "'";
                try
                {
                    sqlConnection.Open();
                    sqlCommand = new MySqlCommand(com, sqlConnection);
                    sqlReader = sqlCommand.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                    players = new DataTable();
                    sqlQuery = "select p.player_id, p.team_number, p.player_name, n.nation, p.playing_pos, p.height, p.weight, p.birthdate, t.team_name " +
                        "from player p, nationality n, team t " +
                        "where p.nationality_id = n.nationality_id and t.team_id = p.team_id and p.team_id = '" + comboBox1.SelectedValue + "' and p.status = 1;";
                    sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
                    sqlAdapter = new MySqlDataAdapter(sqlCommand);
                    sqlAdapter.Fill(players);
                    dataGridView3.DataSource = players;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            players = new DataTable();
            sqlQuery = "select p.player_id, p.team_number, p.player_name, n.nation, p.playing_pos, p.height, p.weight, p.birthdate, t.team_name " +
                "from player p, nationality n, team t " +
                "where p.nationality_id = n.nationality_id and t.team_id = p.team_id and p.team_id = '" + comboBox1.SelectedValue + "' and p.status = 1;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(players);
            dataGridView3.DataSource = players;
        }

        private void btn_change_Click(object sender, EventArgs e)
        {
            string id = dataGridView2.CurrentRow.Cells[0].Value.ToString();

            string com = $"update team set manager_id = '" + id + "' " +
                "where team_id = '" + comboBoxteamsmanager.SelectedValue + "';";
            try
            {
                sqlConnection.Open();
                sqlCommand = new MySqlCommand(com, sqlConnection);
                sqlReader = sqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
            string id2 = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string comm = $"update manager set working = 0 where manager_id = '" + id2 + "';";
            try
            {
                sqlConnection.Open();
                sqlCommand = new MySqlCommand(comm, sqlConnection);
                sqlReader = sqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            string commm = $"update manager set working = 1 where manager_id = '" + id+ "';";
            try
            {
                sqlConnection.Open();
                sqlCommand = new MySqlCommand(commm, sqlConnection);
                sqlReader = sqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();


                manager = new DataTable();
                sqlQuery = "select m.manager_id, m.manager_name, n.nation AS nation, m.birthdate from manager m, team t, nationality n where m.manager_id = t.manager_id " +
                    "and m.nationality_id = n.nationality_id and t.team_id = '" + comboBoxteamsmanager.SelectedValue + "';";
                sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
                sqlAdapter = new MySqlDataAdapter(sqlCommand);
                sqlAdapter.Fill(manager);
                dataGridView1.DataSource = manager;

                manager = new DataTable();
                sqlQuery = "select mn.manager_id, mn.manager_name, n.nation, mn.birthdate from manager mn, nationality n where mn.nationality_id = n.nationality_id AND mn.working = 0;";
                sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
                sqlAdapter = new MySqlDataAdapter(sqlCommand);
                sqlAdapter.Fill(manager);
                dataGridView2.DataSource = manager;
            }
        }

        private void comboBoxteamsmanager_SelectedIndexChanged(object sender, EventArgs e)
        {
            manager = new DataTable();
            sqlQuery = "select m.manager_id, m.manager_name, n.nation, m.birthdate " +
                "from manager m, team t, nationality n where m.manager_id = t.manager_id " +
                "and m.nationality_id = n.nationality_id and t.team_id = '"+comboBoxteamsmanager.SelectedValue+"';" ;
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(manager);
            dataGridView1.DataSource = manager;

            manager = new DataTable();
            sqlQuery = "select mn.manager_id, mn.manager_name, n.nation, mn.birthdate from manager mn, nationality n where mn.nationality_id = n.nationality_id AND mn.working = 0;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(manager);
            dataGridView2.DataSource = manager;
        }
    }
}
