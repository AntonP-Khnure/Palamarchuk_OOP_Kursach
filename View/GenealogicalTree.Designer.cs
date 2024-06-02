namespace View
{
    partial class GenealogicalTree
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            main_panel = new Panel();
            search_text_box = new TextBox();
            search_button = new Button();
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            addToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // main_panel
            // 
            main_panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            main_panel.AutoScroll = true;
            main_panel.Cursor = Cursors.Cross;
            main_panel.Location = new Point(0, 53);
            main_panel.Name = "main_panel";
            main_panel.Size = new Size(828, 425);
            main_panel.TabIndex = 0;
            // 
            // search_text_box
            // 
            search_text_box.BorderStyle = BorderStyle.None;
            search_text_box.Cursor = Cursors.IBeam;
            search_text_box.Dock = DockStyle.Top;
            search_text_box.Font = new Font("Segoe UI", 12F);
            search_text_box.Location = new Point(0, 24);
            search_text_box.Name = "search_text_box";
            search_text_box.Size = new Size(800, 22);
            search_text_box.TabIndex = 1;
            // 
            // search_button
            // 
            search_button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            search_button.Location = new Point(694, 24);
            search_button.Name = "search_button";
            search_button.Size = new Size(106, 23);
            search_button.TabIndex = 0;
            search_button.Text = "Search";
            search_button.UseVisualStyleBackColor = true;
            search_button.Click += search_button_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, saveToolStripMenuItem, loadToolStripMenuItem, addToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(46, 20);
            toolStripMenuItem1.Text = "Main";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(43, 20);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new Size(45, 20);
            loadToolStripMenuItem.Text = "Load";
            loadToolStripMenuItem.Click += loadToolStripMenuItem_Click;
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new Size(41, 20);
            addToolStripMenuItem.Text = "Add";
            addToolStripMenuItem.Click += addToolStripMenuItem_Click;
            // 
            // GenealogicalTree
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(search_button);
            Controls.Add(search_text_box);
            Controls.Add(main_panel);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "GenealogicalTree";
            Text = "Genealogical Tree";
            Load += GenealogicalTree_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel main_panel;
        private TextBox search_text_box;
        private Button search_button;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private ToolStripMenuItem addToolStripMenuItem;
    }
}
