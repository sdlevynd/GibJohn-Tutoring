using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace GibJohn_Tutoring.Windows
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        Dictionary<string, string> answers;
        List<string> questions;

        private int timer;
        private int qNumber;
        private string question;
        private bool quizInProgress;
        private int score;
        public HomePage()
        {
            InitializeComponent();
            questions = new List<string>();
            answers = new Dictionary<string, string>();
            session.connect();
            string getSubjects = "SELECT questionsubject FROM questions";
            using (MySqlCommand cmd = new MySqlCommand(getSubjects, session.getConnection()))
            {
                session.open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string subject = rdr.GetString(0);
                    if (!subjectCmb.Items.Contains(subject))
                    {
                        subjectCmb.Items.Add(subject);
                    }
                }
                session.close();
            }
        }

        private void btnGetQuestions_Click(object sender, RoutedEventArgs e)
        {
            if (subjectCmb.Text != "")
            {
                questions = new List<string>();
                answers = new Dictionary<string, string>();
                string getQuestions = "SELECT questiontext, questionanswer FROM questions WHERE questionsubject = @paramSubject";
                using (MySqlCommand cmd = new MySqlCommand(getQuestions, session.getConnection()))
                {
                    cmd.Parameters.AddWithValue("@paramSubject", subjectCmb.Text);
                    session.open();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        string question = rdr.GetString(0);
                        string answer = rdr.GetString(1);
                        questions.Add(question);
                        answers[question] = answer;
                    }
                    session.close();
                }
            }
            else
            {
                MessageBox.Show("select a subject!");
            }

        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!quizInProgress)
            {
                score = 0;
                quizInProgress = true;
                questionLoop();
            }
        }
        private async Task questionLoop()
        {   
            qNumber = 0;
            while (true)
            {
                await Task.Delay(100);
                Trace.WriteLine("1");
                if (timer == 0)
                {
                    timer = 100;
                    question = questions[qNumber];
                    qNumber++;
                }
                else
                {
                    timer-=2;
                    timerBar.Value = timer;
                }
                update();
            }
        }
        private void update()
        {
            lblTimer.Content = timer.ToString() + "s";
            lblQuestion.Content = question;
            lblScore.Content = $"Score: {score}";
        }

        private void btnAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (txtAnswer.Text == answers[question])
            {
                score += timer;
                timer = 0;
            }
        }
    }
}
