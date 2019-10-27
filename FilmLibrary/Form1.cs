using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FilmLibrary
{
    public partial class Form1 : Form
    {
        SqlConnection sql = new SqlConnection(@"Data Source=LAPTOP-A7TOTK85\SQLEXPRESS;Initial Catalog=Film;Integrated Security=True");
        int movieId = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // check the state of the connection
                if (sql.State == ConnectionState.Closed)
                    sql.Open();

                //create an object
                SqlCommand cmd = new SqlCommand("addOrUpdateMovies", sql);
                cmd.CommandType = CommandType.StoredProcedure;
                //add data
                if (!(string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtLength.Text) || cmbGenre.SelectedItem == null || cmbRating.SelectedItem == null))
                {
                    cmd.Parameters.AddWithValue("@mode", "Add");
                    cmd.Parameters.AddWithValue("@movieId", 0);
                    cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@genre", cmbGenre.Text.Trim());
                    cmd.Parameters.AddWithValue("@rating", cmbRating.Text.Trim());
                    cmd.Parameters.AddWithValue("@releaseDate", dtpDate.Value.Date);
                    cmd.Parameters.AddWithValue("@length", txtLength.Text.Trim());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully added a movie");
                    cancel();
                    viewMovie();
                }
                else
                {
                    MessageBox.Show("Fields are empty");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Could not add Movie");
            }
            finally
            {
                sql.Close();
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            genre();
            rating();
            cancel();
            viewMovie();
            lblSpan.Visible = false;
        }

        void genre()
        {
            cmbGenre.Items.Add("Action");
            cmbGenre.Items.Add("Adventure");
            cmbGenre.Items.Add("Animation");
            cmbGenre.Items.Add("Drama");
            cmbGenre.Items.Add("Horror");
            cmbGenre.Items.Add("Musical/Dance");
            cmbGenre.Items.Add("Sci-fi");
            cmbGenre.Items.Add("Romance");
        }

        void rating()
        {
            cmbRating.Items.Add("G");
            cmbRating.Items.Add("PG");
            cmbRating.Items.Add("PG-13");
            cmbRating.Items.Add("R");
            cmbRating.Items.Add("NC-17");
            cmbRating.Items.Add("NR");
        }

        void searchMovie()
        {
            if (sql.State == ConnectionState.Closed)
                sql.Open();
            SqlDataAdapter Da = new SqlDataAdapter("searchMovie", sql);
            Da.SelectCommand.CommandType = CommandType.StoredProcedure;
            Da.SelectCommand.Parameters.AddWithValue("@title", txtSearch.Text.Trim());
            DataTable dt = new DataTable();
            Da.Fill(dt);
            dgvMovies.DataSource = dt;
            dgvMovies.Columns[0].Visible = false;
            sql.Close();
        }

        void viewMovie()
        {
            if (sql.State == ConnectionState.Closed)
                sql.Open();
            SqlDataAdapter sqlData = new SqlDataAdapter("viewMovie", sql);
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable data = new DataTable();
            sqlData.Fill(data);
            dgvMovies.DataSource = data;
            dgvMovies.Columns[0].Visible = false;
            sql.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(string.IsNullOrWhiteSpace(txtSearch.Text)))
                {
                    searchMovie();
                    txtSearch.Text = string.Empty;
                }
                else
                {
                    viewMovie();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Could not perform search operation");
            }
        }

        private void dgvMovies_DoubleClick(object sender, EventArgs e)
        {
            //check if current row index is valid
            if(dgvMovies.CurrentRow.Index != -1)
            {
                movieId = int.Parse(dgvMovies.CurrentRow.Cells[0].Value.ToString());
                txtTitle.Text = dgvMovies.CurrentRow.Cells[1].Value.ToString();
                cmbGenre.Text = dgvMovies.CurrentRow.Cells[2].Value.ToString();
                cmbRating.Text = dgvMovies.CurrentRow.Cells[3].Value.ToString();
                dtpDate.Value = Convert.ToDateTime(dgvMovies.CurrentRow.Cells[4].Value.ToString());
                txtLength.Text = dgvMovies.CurrentRow.Cells[5].Value.ToString();
                lblSpan.Visible = true;
                lblSpan.Text = "Click on datetimepicker to view the release date";
                lblSpan.ForeColor = Color.Red;
                btnDelete.Enabled = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (sql.State == ConnectionState.Closed)
                    sql.Open();
                
                SqlCommand cmd = new SqlCommand("addOrUpdateMovies", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                if (!(string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtLength.Text) || cmbGenre.SelectedItem == null || cmbRating.SelectedItem == null))
                {
                    cmd.Parameters.AddWithValue("@mode", "Edit");
                    cmd.Parameters.AddWithValue("@movieId", movieId);
                    cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@genre", cmbGenre.Text.Trim());
                    cmd.Parameters.AddWithValue("@rating", cmbRating.Text.Trim());
                    cmd.Parameters.AddWithValue("@releaseDate", dtpDate.Value.Date);
                    cmd.Parameters.AddWithValue("@length", txtLength.Text.Trim());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully Updated a movie");
                    viewMovie();
                    cancel();
                }
                else
                {
                    MessageBox.Show("There is an Empty field");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Could not Update Movie");
            }
            finally
            {
                sql.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (sql.State == ConnectionState.Closed)
                    sql.Open();

                SqlCommand cmd = new SqlCommand("deleteMovie", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                if (!(string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtLength.Text) || cmbGenre.SelectedItem == null || cmbRating.SelectedItem == null))
                {
                    cmd.Parameters.AddWithValue("@movieId", movieId);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Movie Deleted");
                    cancel();
                    viewMovie();
                }
                else
                {
                    MessageBox.Show("Could not delete the Movie because of missig data");
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Could Not Delete Movie");
            }
            finally
            {
                sql.Close();
            }
        }

        void cancel()
        {
            txtTitle.Text = txtLength.Text = string.Empty;
            cmbGenre.SelectedItem = null;
            cmbRating.SelectedItem = null;
            movieId = 0;
            dtpDate.Format = DateTimePickerFormat.Custom;
            dtpDate.CustomFormat = " ";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancel();
        }

        private void dtpDate_MouseDown(object sender, MouseEventArgs e)
        {
            ((DateTimePicker)sender).Format = DateTimePickerFormat.Long;
            lblSpan.Visible = false;
        }
        
    }
}
