
using System.Windows.Forms;


namespace TestApp
{


    public partial class frmTextEditor : Form
    {


        public frmTextEditor()
        {
            InitializeComponent();
        }


        private static System.Random seed = new System.Random((int)System.DateTime.Now.Ticks);//thanks to McAden
        private string RandomString(int size)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                //ch = System.Convert.ToChar(System.Convert.ToInt32(System.Math.Floor(26 * seed.NextDouble() + 65)));

                int b = seed.Next(0, 2);

                if(b == 0)
                    ch = System.Convert.ToChar(seed.Next(65, 91));
                else
                    ch = System.Convert.ToChar(seed.Next(97, 123));

                builder.Append(ch);
            } // Next i 

            return builder.ToString();
        } // End Function RandomString 


        private void Form1_Load(object sender, System.EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for(int i = 0; i < 100000;++i)
            {
                int iRandomLength = seed.Next(3, 11);

                string strRand = RandomString(iRandomLength);
                sb.Append(strRand);
                if (iRandomLength % 10 == 0)
                    sb.AppendLine(" ");
                else
                    sb.Append(" ");
            } // Next i 

            this.textBox1.Text = sb.ToString();
            Utils.ScrollToBottom(this.textBox1);
        }

        private void speichernToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            sfd.FileName = "LogFile";

            // sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.Filter = "Text files (*.txt)|*.txt|Log files (*.log)|*.log";
            sfd.FilterIndex = 2;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                System.Console.WriteLine(sfd.FileName);
            }

        }

        private void beendenToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        } // End Sub Form1_Load 


    } // End Class frmTextEditor : Form


} // End Namespace TestApp
