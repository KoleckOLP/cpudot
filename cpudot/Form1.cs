using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace cpudot
{
    public partial class Form1 : Form
    {

        public Label label1;
        public PictureBox pictureBox1;

        private PerformanceCounter cpuCounter;
        private Timer timer;

        private Point lastCursorPosition;
        private bool isDragging = false;
        private NotifyIcon notifyIcon;

        public Form1()
        {
            InitializeComponent();

            // Subscribe to label's mouse events
            label1.MouseDown += label1_MouseDown;
            label1.MouseMove += label1_MouseMove;
            label1.MouseUp += label1_MouseUp;

            // Create the NotifyIcon instance
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Properties.Resources.Icon; // Set the icon for the notification area
            notifyIcon.Text = "cpudot"; // Set the tooltip text
            notifyIcon.Visible = true;

            // Handle double-click on the notification icon to show/hide the form
            notifyIcon.MouseClick += NotifyIcon_MouseClick;

            ShowInTaskbar = false;

            // Create a PerformanceCounter for CPU usage
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            // Initialize the timer
            timer = new Timer();
            timer.Interval = 1000; // 1000 milliseconds = 1 second
            timer.Tick += Timer_Tick;

            // Start the timer
            timer.Start();

            // Update the label immediately after starting the timer
            Timer_Tick(null, null);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Get the current CPU usage
            float cpuUsage = cpuCounter.NextValue();

            int cpuUsageInt = (int)cpuUsage;

            // Update the label with the CPU usage
            label1.Text = "CPU Usage: " + cpuUsageInt.ToString() + "%";
        }

        private void MinimizeMenuItem_Click(object sender, EventArgs e)
        {
            // Hide the form
            Hide();
        }

        private void CloseMenuItem_Click(object sender, EventArgs e)
        {
            // Close the program
            Close();
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Toggle visibility of the form
                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal; // Restore from minimized
                }
                else if (Visible)
                {
                    Hide(); // Hide the form if it's already visible
                }
                else
                {
                    Show(); // Show the form if it's hidden
                }
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lastCursorPosition = e.Location;
                isDragging = true;
            }
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentCursorPosition = PointToScreen(e.Location);
                Location = new Point(currentCursorPosition.X - lastCursorPosition.X, currentCursorPosition.Y - lastCursorPosition.Y);
            }
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;

            if (e.Button == MouseButtons.Right)
            {
                // Display a context menu with options to minimize or close the program
                ContextMenu contextMenu = new ContextMenu();

                // Add menu items for minimize and close
                MenuItem minimizeMenuItem = new MenuItem("Minimize");
                MenuItem closeMenuItem = new MenuItem("Close");

                // Add event handlers for the menu items
                minimizeMenuItem.Click += MinimizeMenuItem_Click;
                closeMenuItem.Click += CloseMenuItem_Click;

                // Add menu items to the context menu
                contextMenu.MenuItems.Add(minimizeMenuItem);
                contextMenu.MenuItems.Add(closeMenuItem);

                // Display the context menu at the current mouse position
                contextMenu.Show(label1, e.Location);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dispose of the NotifyIcon instance when the form closes
            notifyIcon.Dispose();
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(257, 31);
            this.label1.TabIndex = 3;
            this.label1.Text = "CPU Usage: 100%";
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.ClientSize = new System.Drawing.Size(280, 50);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
