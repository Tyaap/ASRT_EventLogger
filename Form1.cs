using System;
using System.Diagnostics;
using System.Windows.Forms;
using static MemoryHelper;

namespace EventLogger
{
    public partial class Form1 : Form
    {
        public Timer timer = new Timer() { Interval = 100 };
        public Logger logger = new Logger();
        public int lobbyState = 0;

        public Form1()
        {
            InitializeComponent();
            timer.Tick += LogManager;
            timer.Start();
            FormMessage("Logging started! (custom games only)");
            FormMessage("Log folder: " + AppContext.BaseDirectory);
            FormMessage("Close this window to stop logging");
        }

        public void LogManager(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("ASN_App_PcDx9_Final");
            if (processes.Length == 0)
            {
                if (processId != 0)
                {
                    FormMessage("S&ASRT was closed");
                    MemoryHelper.Reset();
                }
                return;
            }
            else if (processes[0].Id != processId)
            {
                FormMessage("S&ASRT is running");
                MemoryHelper.Initialise(processes[0].Id);
                Write(0x70665C, new byte[] { 0xEB, 0x28 });
                Write(0x706686, new byte[] { 0x8B, 0xF1, 0x89, 0x3D, 0xFC, 0x0F, 0xFF, 0x00, 0xEB, 0xCE });
            }

            int newLobbyState = GetLobbyState();
            if (logger.currentSession == null && lobbyState > 0 && (ReadByte(ReadInt(0xEC1A88) + 0x101D6C) & 0x1F) == 16) // must be an active custom lobby
            {
                logger.NewSession();
                FormMessage("Session started");
            }

            if (lobbyState == 3 && newLobbyState == 4)
            {
                logger.NewEvent();
                FormMessage("Event started: " + logger.currentSession.lastEvent.map.GetDescription() + " " + logger.currentSession.lastEvent.type.GetDescription());
            }

            if (lobbyState == 18 && newLobbyState != 18)
            {
                logger.LogEventResults();
                logger.WriteLogFiles();
                FormMessage("Results logged!");
            }
            lobbyState = newLobbyState;
        }

        public int GetLobbyState()
        {
            int lobbyState = 0;
            if (ReadByte(ReadInt(0xEC1A88) + 0x525) > 0)
            {
                if (ReadInt(ReadInt(0xBCE920)) != 0)
                {
                    lobbyState = ReadByte(0xFF0FFC);
                }
                else
                {
                    lobbyState = Math.Max((byte)1, ReadByte(ReadInt(0xEC1A88) + 0x101D72));
                }
            }
            return lobbyState;
        }

        public void FormMessage(string message)
        {
            message = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + message;
            if (!string.IsNullOrEmpty(richTextBox1.Text))
            {
                message = "\n" + message;
            }
            richTextBox1.Text += message; 
        }
    }
}
