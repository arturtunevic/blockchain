using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Blockchain
{
    public partial class Form1 : Form
    {
        Label rinkimaiLabel;
        Server srv = new Server();
        Blockchain rinkimai;
        List<Candidate> kandidatai;
        List<Label> votesLabels = new List<Label>();
        public Form1()
        {
            InitializeComponent();

            int style = NativeWinAPI.GetWindowLong(this.Handle, NativeWinAPI.GWL_EXSTYLE);
            style |= NativeWinAPI.WS_EX_COMPOSITED;
            NativeWinAPI.SetWindowLong(this.Handle, NativeWinAPI.GWL_EXSTYLE, style);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rinkimaiLabel = new Label();
            rinkimaiLabel.Text = "Prezidento rinkimai";
            rinkimaiLabel.Font = new Font("Arial", 18);
            rinkimaiLabel.Location = new Point(114, 15);
            rinkimaiLabel.Width = 250;
            panel1.Controls.Add(rinkimaiLabel);
            Server.path = Application.StartupPath;
            DAO dao = new DAO();
            kandidatai = dao.CreateCandidates();
            CandidatesSortByVotes(kandidatai);
            ShowCandidates(kandidatai);
            rinkimai = new Blockchain();
            srv.SetBlockChain(rinkimai, this);

            if (Server.path.Length > 0)
                backgroundWorker1.RunWorkerAsync();

            Client.AskForBlockChain(srv.GetPort());
        }

        internal static class NativeWinAPI
        {
            internal static readonly int GWL_EXSTYLE = -20;
            internal static readonly int WS_EX_COMPOSITED = 0x02000000;

            [DllImport("user32")]
            internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32")]
            internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        }

        public List<Candidate> CandidatesSortByVotes(List<Candidate> kandidatai)
        {
            kandidatai = kandidatai.OrderByDescending(o => o.GetVotes()).ToList();
            return kandidatai;
        }
        public void ShowCandidates(List<Candidate> kandidatai)
        {
            panel2.Controls.Clear();
            int x = 0;
            int yName = 150;
            int yButton = 180;
            int yVotes = 210;
            foreach (Candidate kandidatas in kandidatai)
            {
                PictureBox imageControl = new PictureBox();
                imageControl.Location = new Point(x, 0);
                Image image = Image.FromFile(kandidatas.GetImg());
                imageControl.SizeMode = PictureBoxSizeMode.StretchImage;
                imageControl.Image = image;
                imageControl.Height = 140;
                imageControl.Width = 125;
                panel2.Controls.Add(imageControl);
                Label name = new Label();
                name.Text = kandidatas.GetName();
                name.Location = new Point(x, yName);
                Button vote = new Button();
                vote.Tag = name.Text;
                vote.Location = new Point(x, yButton);
                vote.Text = "Balsuoti";
                vote.Click += vote_Click;
                Label votes = new Label();
                votes.Text = kandidatas.GetVotes().ToString();
                votes.Location = new Point(x, yVotes);
                votesLabels.Add(votes);
                x += 160;
                panel2.Controls.Add(name);
                panel2.Controls.Add(vote);
                panel2.Controls.Add(votes);
            }
        }
        void vote_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string name = (string)b.Tag;
            Block newBlock = new Block(DateTime.Now, null, name);
            Client.Vote(newBlock);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            srv.StartServer();
        }

        public void RecalculateVotes()
        {
            foreach(Candidate candidate in kandidatai)
            {
                candidate.SetVotesToZero();
            }

            foreach(Block block in rinkimai.GetChain())
            {
                foreach(Candidate kandidatas in kandidatai)
                {
                    if(kandidatas.GetName() == block.GetData())
                    {
                        kandidatas.AddVote();
                    }
                }
            }

            int j = 0;
            foreach (Candidate kandidatas in kandidatai)
            {
                votesLabels[j].Text = kandidatas.GetVotes().ToString();
                j++;
            }

            List<Candidate> sortedCandidates = CandidatesSortByVotes(kandidatai);
            ShowCandidates(sortedCandidates);
        }

        public void RecalculateInvoker()
        {
            this.Invoke(new MethodInvoker(() => this.RecalculateVotes()));
        }

        public void SetRinkimai(Blockchain blockChain)
        {
            this.rinkimai = blockChain;
        }

        public void SetRinkimaiInvoker(Blockchain blockChain)
        {
            this.Invoke(new MethodInvoker(() => this.SetRinkimai(blockChain)));
        }

        public void SendBlockChain(int port, Blockchain blockChain)
        {
            Client.SendBlockChain(port, blockChain);
        }

        public void SendBlockChainInvoker(int port, Blockchain blockChain)
        {
            this.Invoke(new MethodInvoker(() => this.SendBlockChain(port, blockChain)));
        }

    }
}
