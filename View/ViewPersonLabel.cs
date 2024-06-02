using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View
{   
    internal class ViewPersonLabel : Label
    {
        public static int Width_ = 90;
        public static int Height_ = 50;
        public string full_name;
        public int generation;
        public Person person;
        private ViewTree parent;

        public ViewPersonLabel(Person person,Point coord)
        {
            this.full_name = person.Full_name;
            this.person = person;
            this.parent = parent;
            generation = 0;


            ToolTip toolTip = new ToolTip() { 
                AutoPopDelay = 5000,
                InitialDelay = 500,
                ReshowDelay = 500,
                ShowAlways = true,
                
            };

            Anchor = AnchorStyles.None;
            Text = full_name.Replace(' ', '\n');
            Width = Width_;
            Height = Height_;
            TextAlign = ContentAlignment.MiddleCenter;
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = Color.FromArgb(0, 47, 72, 88);
            ForeColor = Color.FromArgb(177, 113, 98);
            
            
            toolTip.SetToolTip(this, this.ToString());
            this.Width = Width_;
            this.Height = Height_;
            this.Top = coord.Y;
            this.Left = coord.X;

            this.Click += this.ViewPersonLabel_Click;
            this.MouseHover += this.ViewPersonLabel_MouseHover;
            this.MouseLeave += this.ViewPersonLabel_MouseLeave;
             
        }

        private void ViewPersonLabel_Click(object? sender, EventArgs e)
        {
            (Parent.Parent.Parent.Parent as GenealogicalTree).ShowPersonData(person.Id);
        }

        private void ViewPersonLabel_MouseHover(object sender, EventArgs e) { 

           ForeColor = Color.FromArgb(146, 108, 98);
            BorderStyle = BorderStyle.Fixed3D;
        }
        private void ViewPersonLabel_MouseLeave(object sender, EventArgs e) {

            ForeColor = Color.FromArgb(177, 113, 98);
            BorderStyle = BorderStyle.FixedSingle;
        }
        public override string ToString()
        {
            return person.ToString();
        }

    }
}
