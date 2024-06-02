using Model;
using ViewModel;

namespace View
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Security.Cryptography;
    using System.Windows.Forms;

    /// <summary>
    /// Форма для відображення генеалогічного дерева.
    /// </summary>
    public partial class GenealogicalTree : Form
    {
        public TreeController tc;
        internal ViewTree vt;
        internal Panel vt_map_panel;
        internal ViewSearchResults vsr;


        /// <summary>
        /// Ініціалізує новий екземпляр форми GeneologicalTree.
        /// </summary>
        public GenealogicalTree()
        {
            InitializeComponent();
            tc = new TreeController(/*"C:\\Users\\timpf\\Downloads\\cursach.json"*/);
            //tc.Save("C:\\Users\\timpf\\Downloads\\cursach.json");
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.BackColor = Color.FromArgb(255, 218, 195);
            this.KeyDown += GeneologicalTree_KeyDown;


            search_text_box.ForeColor = Color.FromArgb(177, 113, 98);
            search_text_box.BackColor = Color.FromArgb(242, 206, 185);


            menuStrip1.BackColor = Color.FromArgb(255, 218, 195);

            addToolStripMenuItem.ForeColor = Color.FromArgb(177, 113, 98);
            loadToolStripMenuItem.ForeColor = Color.FromArgb(177, 113, 98);
            saveToolStripMenuItem.ForeColor = Color.FromArgb(177, 113, 98);
            toolStripMenuItem1.ForeColor = Color.FromArgb(177, 113, 98); 

            search_button.FlatStyle = FlatStyle.Flat;
            search_button.ForeColor = Color.FromArgb(177, 113, 98);
            search_button.BackColor = Color.FromArgb(242, 206, 185);

            vt_map_panel = new Panel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
            };
            vt = new ViewTree(tc) { Parent = vt_map_panel };

            vt_map_panel.Controls.Add(vt);
            main_panel.Controls.Add(vt_map_panel);

        }

        /// <summary>
        /// Обробник натискання клавіші "Escape".
        /// </summary>
        public void GeneologicalTree_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult result = MessageBox.Show("Sure want to leave?,unsaved data will be loose", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }

        private void GenealogicalTree_Load(object sender, EventArgs e)
        {
            vt_map_panel.AutoScrollPosition = new Point(vt_map_panel.VerticalScroll.Maximum / 2, vt_map_panel.HorizontalScroll.Maximum / 2);
        }

        /// <summary>
        /// Показує дані особи.
        /// </summary>
        /// <param name="id">Ідентифікатор особи.</param>
        public void ShowPersonData(long id = 0)
        {
            var person = tc.Find(id);

            var child = new List<Person>();
            if (person != null)
                child = new List<Person>(person.Child);

            var edit_person = new EditPerson(this, person);

            foreach (var kid in child)
            {
                if (person.sex == Sex.M)
                    kid.Father = null;
                else
                    kid.Mother = null;
            }

            edit_person.ShowDialog();

            if (edit_person.person != null)
            {


                var mother = edit_person.person.Mother;
                var father = edit_person.person.Father;
                var kids = edit_person.person.Child;

                edit_person.person.Mother = null;
                edit_person.person.Father = null;
                edit_person.person.Child = new List<Person>();

                if (mother != null)
                {    
                    try
                    {
                        tc.tree.AddChild(mother, edit_person.person);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                
                if (father != null)
                { 
                    try
                    {
                        tc.tree.AddChild(father, edit_person.person);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                
                foreach (var kid in kids)
                {
                    try
                    {
                        tc.tree.AddChild(edit_person.person, kid);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }

                tc.tree.AddChild(tc.tree.Head, edit_person.person);
            }
            vt.Invalidate();
            vt.Update(tc, true);
            toolStripMenuItem1_Click(null, null);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Save = new SaveFileDialog();
            Save.ShowDialog();
            if (Save.FileName == "") { return; }
            tc.Save(Save.FileName);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Load = new OpenFileDialog();
            Load.ShowDialog();
            if (Load.FileName == "") { return; }
            tc.Load(Load.FileName);
            vt.Invalidate();
            vt.Update(tc, true);
        }

        private void search_button_Click(object sender, EventArgs e)
        {
            main_panel.Controls.Clear();
            var people = tc.Find(search_text_box.Text);
            people.Remove(people.FirstOrDefault(p => p.Id == 1));
            vsr = new ViewSearchResults(people, this);
            main_panel.Controls.Add(vsr);
        }

        public void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            main_panel.Controls.Clear();

            vt_map_panel.Controls.Add(vt);
            main_panel.Controls.Add(vt_map_panel);
            vt_map_panel.VerticalScroll.Visible = false;
            vt_map_panel.HorizontalScroll.Visible = false;
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPersonData();
        }
    }

}
