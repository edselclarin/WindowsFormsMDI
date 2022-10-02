using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsMDI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                IsMdiContainer = true;
                WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void newChildWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MdiChildren.Count() == 30)
                {
                    throw new Exception("Too many windows.");
                }

                // Create a new child form.
                var child = new ChildForm()
                {
                    MdiParent = this,
                    Text = $"Child{MdiChildren.Count()}",
                };

                // Capture the event when the child form is closed.
                child.FormClosing += Child_FormClosing;

                // Show the child form.
                child.Show();

                // Create a menu item for the child form.
                var menu = new ToolStripMenuItem(child.Text)
                {
                    Tag = child // Link menu to associated form.
                };

                // Capture menu click event.
                menu.Click += ChildFormMenu_Click;

                // Insert the menu above the seperator.
                int index = 0;
                foreach (ToolStripItem item in windowsToolStripMenuItem.DropDownItems)
                {
                    if (item.GetType() == typeof(ToolStripSeparator))
                    {
                        break;
                    }
                    else
                    {
                        index++;
                    }
                }
                if (index < windowsToolStripMenuItem.DropDownItems.Count)
                {
                    windowsToolStripMenuItem.DropDownItems.Insert(index, menu);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void Child_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Find and remove the corresponding menu item.
                var form = sender as ChildForm;
                var menu = windowsToolStripMenuItem.DropDownItems.Cast<ToolStripItem>()
                    .FirstOrDefault(x => x.Text == form.Text);
                if (menu != null)
                {
                    windowsToolStripMenuItem.DropDownItems.Remove(menu);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void ChildFormMenu_Click(object sender, EventArgs e)
        {
            try
            {
                var menu = sender as ToolStripMenuItem;
                var form = (ChildForm)menu.Tag;
                form.BringToFront();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void closeAllWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            try
            {
                foreach (var child in MdiChildren)
                {
                    child.Close();
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.LayoutMdi(MdiLayout.Cascade);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void tileHorizontallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.LayoutMdi(MdiLayout.TileHorizontal);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void tileVerticallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.LayoutMdi(MdiLayout.TileVertical);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ShowExceptionMessage(Exception ex)
        {
            MessageBox.Show(this, ex.InnerException != null ? ex.InnerException.Message : ex.Message, this.Text,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
