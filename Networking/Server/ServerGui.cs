using System.Linq;
using System.Windows.Forms;

namespace Server
{
	public partial class ServerGui : Form
	{
		public ServerGui()
		{
			InitializeComponent();
			InitialiseServer();
		}

		public void InitialiseServer()
		{
			server = new Server();
			server.NewData += (msg) => txtMessages.AppendText(msg + System.Environment.NewLine);
		}

		Server server;

		private void BtnStart_Click(object sender, System.EventArgs e)
		{
			server.Start();
		}

		private void BtnStop_Click(object sender, System.EventArgs e)
		{
			server.Stop();
		}
	}
}
