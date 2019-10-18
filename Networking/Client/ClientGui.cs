using System;
using System.Windows.Forms;

namespace Client
{
	public partial class ClientGui : Form
	{
		public ClientGui()
		{
			InitializeComponent();
			client = new Client();
			client.NewData += (msg) => txtChat.AppendText(msg + System.Environment.NewLine);
		}

		private void BtnConnect_Click(object sender, EventArgs e)
		{
			client.Connect();
		}

		private void BtnDisconnect_Click(object sender, EventArgs e)
		{
			client.Disconnect();
		}

		private void BtnSend_Click(object sender, EventArgs e)
		{
			client.Send(txtMessage.Text);
			txtMessage.Text = string.Empty;
			// send msg
		}

		Client client;
	}
}
