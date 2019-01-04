using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using RDPCOMAPILib;

namespace rdpServerGui
{
    public partial class Form1 : Form
    {
        private RDPSession rdpSession = new RDPSession();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnShare_Click(object sender, EventArgs e)
        {
            rdpSession.OnAttendeeConnected += OpenSession;
            rdpSession.OnAttendeeDisconnected += EndSession;
            rdpSession.OnControlLevelChangeRequest += ControlLevelChange;            
            rdpSession.Open();
        }

        private void OpenSession(Object client)
        {
            IRDPSRAPIAttendee Client = (IRDPSRAPIAttendee)client;
            Client.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_MAX;
        }

        private void EndSession(Object client)
        {
            rdpSession.Close();
            Marshal.ReleaseComObject(rdpSession);
            rdpSession = null;
        }

        private void ControlLevelChange(Object client, CTRL_LEVEL RequestedLevel)
        {
            IRDPSRAPIAttendee Client = (IRDPSRAPIAttendee)client;
            Client.ControlLevel = RequestedLevel;
        }

        private void btnInvite_Click(object sender, EventArgs e)
        {
            IRDPSRAPIInvitation Invitation;
            Invitation = rdpSession.Invitations.CreateInvitation("Test", "Grup", "parola", 1);
            File.WriteAllText(@"C:\rdpcon.txt", Invitation.ConnectionString);
            //Console.WriteLine("Davetiye hazirlandi");
        }
    }
}
