using ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Windows.Forms;

namespace View
{
    internal class ViewSearchResults : ListBox
    {
        GenealogicalTree form;
        Dictionary<string, Person> people;
        public ViewSearchResults(List<Person> people,GenealogicalTree form)
        {
            this.form = form;

            SelectedIndexChanged += new EventHandler(ViewSearchResults_SelectedIndexChanged);

            Dock = DockStyle.Fill;
            ItemHeight = 60;

            this.people = new Dictionary<string, Person>();
            this.BackColor = Color.FromArgb(242, 206, 185);
            this.ForeColor = Color.FromArgb(177, 113, 98); 

            foreach (var person in people)
            {
                this.Items.Add(person.ToString());
                this.people[person.ToString()] = person;
            }
        }
        private void ViewSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(SelectedItem!=null)
            {
                form.toolStripMenuItem1_Click(null,null);
                form.ShowPersonData(people[(SelectedItem as String)].Id);
            }
                
        }
    }
}
