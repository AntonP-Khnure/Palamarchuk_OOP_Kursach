using Model;
using System.Text;


namespace View
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// Форма для редагування або додавання особи у генеалогічне дерево.
    /// </summary>
    public partial class EditPerson : Form
    {
        GenealogicalTree gt;
        public Person person;

        // Властивість ім'я.
        string Name_ { get { return textBox1.Text; } set { textBox1.Text = value; } }
        // Властивість прізвище.
        string Second_name_ { get { return textBox2.Text; } set { textBox2.Text = value; } }
        // Властивість по батькові.
        string Surname_ { get { return textBox3.Text; } set { textBox3.Text = value; } }

        // Властивість стать.
        Sex sex
        {
            get { return radioButton1.Checked ? Sex.M : Sex.F; }
            set { radioButton1.Checked = value == Sex.M; radioButton2.Checked = value == Sex.F; }
        }
        // Властивість дата народження.
        DateTime Birth_date
        {
            get { return !checkBox2.Checked ? dateTimePicker1.Value : DateTime.Now; }
            set { dateTimePicker1.Value = value.Year > 1752 ? value : dateTimePicker1.Value; checkBox2.Checked = value == default; }
        }
        // Властивість дата смерті.
        DateTime Deaht_date
        {
            get { return checkBox1.Checked ? dateTimePicker2.Value : DateTime.Now; }
            set { dateTimePicker2.Value = value.Year > 1752 ? value : dateTimePicker2.Value; checkBox1.Checked = value != default; }
        }

        // Властивість список дітей.
        List<Person> child
        {
            get
            {
                var temp = new List<Person>();
                foreach (var item in checkedListBox1.CheckedItems)
                {
                    if ((item as Person).Id != 1)
                        temp.Add(item as Person);
                }
                return temp;
            }
            set
            {
                foreach (var item in value)
                {
                    if (item.Id != 1)
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(item), true);
                }
            }
        }
        // Властивість хвороби.
        HashSet<Disease> diseases
        {
            get
            {
                var temp = new HashSet<Disease>();
                foreach (var row in textBox5.Lines)
                {
                    if (!string.IsNullOrEmpty(row.Trim()))
                        temp.Add(new Disease() { name = row, ill = true });
                }
                return temp;
            }
            set
            {
                foreach (var item in value)
                {
                    if (item.ill == true)
                        textBox5.Lines = textBox5.Lines.Append(item.name).ToArray();
                }
            }
        }
        // Властивість батько.
        Person Father
        {
            get { return comboBox1.SelectedItem == "-" ? null : comboBox1.SelectedItem as Person; }
            set { comboBox1.SelectedItem = value; }
        }
        // Властивість мати.
        Person Mother
        {
            get { return comboBox2.SelectedItem == "-" ? null : comboBox2.SelectedItem as Person; }
            set { comboBox2.SelectedItem = value; }
        }

        Person last_choosed_father;
        Person last_choosed_mother;

        /// <summary>
        /// Ініціалізує новий екземпляр форми EditPerson.
        /// </summary>
        /// <param name="gt">Генеалогічне дерево.</param>
        /// <param name="person">Особа для редагування або null для створення нової особи.</param>
        public EditPerson(GenealogicalTree gt, Person person = null)
        {
            InitializeComponent();

            this.BackColor = Color.FromArgb(255, 218, 195);
            button5.Enabled = false;
            button4.Enabled = false;

            foreach (var c in Controls)
            {
                if (!(c is Label) && !(c is RadioButton) && !(c is CheckBox) && !(c is ComboBox))
                    (c as Control).BackColor = Color.FromArgb(242, 206, 185);
            }

            this.gt = gt;

            var temp_parents = gt.tc.tree.ToList();
            var temp_kids = gt.tc.tree.ToList();


            temp_parents.Remove(gt.tc.tree.Head);
            temp_kids.Remove(gt.tc.tree.Head);
            if (person != null)
            {
                temp_parents.Remove(person);
                temp_kids.Remove(person);

                if (person.Father != null)
                    temp_kids.Remove(person.Father);

                if (person.Mother != null)
                    temp_kids.Remove(person.Mother);
            }

            foreach (var per in temp_kids)
            {
                checkedListBox1.Items.Add(per);
            }
            comboBox1.Items.Add("-");
            comboBox2.Items.Add("-");
            foreach (var per in temp_parents)
            {
                if (per.sex == Sex.M)
                    comboBox1.Items.Add(per);
                else
                    comboBox2.Items.Add(per);
            }

            checkedListBox1.ItemCheck += ItemCheck;
            comboBox1.TextChanged += FatherTextChanged;
            comboBox2.TextChanged += MotherTextChanged;

            if (person != null)
            {
                this.person = person;
                Name_ = person.Name;
                Second_name_ = person.Second_name;
                Surname_ = person.Surname;
                sex = person.sex;
                Birth_date = person.Birth_date;
                Deaht_date = person.Death_date;
                diseases = person.diseases;
                Father = person.Father;
                Mother = person.Mother;
                child = person.Child;

                if (person.sex == Sex.M)
                    radioButton2.Enabled = false;
                else
                    radioButton1.Enabled = false;

                dateTimePicker1.Enabled = false;
                checkBox1.Enabled = !checkBox1.Checked;
                checkBox2.Enabled = checkBox2.Checked;
                button5.Enabled = true;
                button4.Enabled = true;
            }
        }
        private void EditPerson_Load(object sender, EventArgs e)
        {

        }
        private void MotherTextChanged(object? sender, EventArgs e)
        {
            if (last_choosed_mother != null)
                checkedListBox1.Items.Add(last_choosed_mother);

            checkedListBox1.Items.Remove((sender as ComboBox).SelectedItem);

            last_choosed_mother = ((sender as ComboBox).SelectedItem as Person);
        }

        private void FatherTextChanged(object? sender, EventArgs e)
        {
            if (last_choosed_father != null)
                checkedListBox1.Items.Add(last_choosed_father);

            checkedListBox1.Items.Remove((sender as ComboBox).SelectedItem);

            last_choosed_father = ((sender as ComboBox).SelectedItem as Person);
        }

        private void ItemCheck(object? sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                if (((sender as CheckedListBox).Items[e.Index] as Person).sex == Sex.M)
                    comboBox1.Items.Remove((sender as CheckedListBox).Items[e.Index]);
                else
                    comboBox2.Items.Remove((sender as CheckedListBox).Items[e.Index]);
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                if (((sender as CheckedListBox).Items[e.Index] as Person).sex == Sex.M)
                    comboBox1.Items.Add((sender as CheckedListBox).Items[e.Index]);
                else
                    comboBox2.Items.Add((sender as CheckedListBox).Items[e.Index]);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (person == null)
            {
                person = new Person();
            }
            person.Name = Name_;
            person.Surname = Surname_;
            person.Second_name = Second_name_;
            person.sex = sex;
            person.Birth_date = Birth_date;
            person.Death_date = Deaht_date;
            person.Child = child;
            person.diseases = diseases;
            person.Father = Father;
            person.Mother = Mother;

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!textBox5.Text.Contains(textBox4.Text) && textBox4.Text != "")
                textBox5.Lines = textBox5.Lines.Append(textBox4.Text).ToArray();
            textBox4.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox5.Lines.Length > 0)
            {
                var lines = textBox5.Lines.ToList();
                lines.RemoveAt(lines.Count - 1);
                textBox5.Lines = lines.ToArray();
            }
        }

        private void EditPerson_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (person != null)
                person.Child = child;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show($"Are you sure, it will deleate {Name_} permanently", "", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (person.Mother != null)
                    person.Mother.Child.Remove(person);
                if (person.Father != null)
                    person.Father.Child.Remove(person);

                gt.tc.tree.Head.Child.Remove(person);
                person = null;
                Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(person!=null)
                person.Child = child;   
                SavePersonGeneologicalData();
        }

        private void SavePersonGeneologicalData() {
            var dialog = new SaveFileDialog();
            dialog.ShowDialog();
            var file = dialog.FileName;
             if (file != null) {
               
                if (!File.Exists(file))
                {
                    using (var stream = File.Create(file)) { }
                }
                using (var sw = new StreamWriter(file))
                {
                    var res = new StringBuilder();
                    GetDirectrAncestorsData(res,person);
                    GetDirectrKidsData(res,person);
                    
                    sw.Write(res);
                }
            }
        }

        private string GetUnusualPersonString(Person per) {
            var res = new StringBuilder();
            res.AppendLine(" Name: " + per.Full_name);
            res.AppendLine(" Age: " + per.Age.ToString());
            if (per.Birth_date != default) { res.AppendLine(" Birth day: " + per.Birth_date.ToString("d") + " "); }
            if (per.Death_date != default) { res.AppendLine(" Death day: " + per.Death_date.ToString("d") + " "); }
            res.AppendLine(" Sex: " + (sex == Sex.M ? "Чоловік" : "Жінка"));

            if (per.diseases.Count > 0)
            {
                res.AppendLine(" Diseases: ");
                foreach (var disease in per.diseases)
                {
                    if (sex == Sex.M)
                    {
                        if (disease.ill)
                        {
                            res.AppendLine("  have " + disease.name);
                        }
                        else if (disease.risk)
                        {
                            res.AppendLine("  was risk to gain " + disease.name);
                        }
                    }
                    else
                    {
                        if (disease.ill)
                        {
                            res.AppendLine("  have " + disease.name);
                        }
                        else if (disease.recessive)
                        {
                            res.AppendLine("  recesive " + disease.name);
                        }
                        else if (disease.recessive_risk)
                        { 
                            res.AppendLine("  may be recesive " + disease.name);
                        }
                    }

                }
            }
            return res.ToString();
        }
        private void GetDirectrAncestorsData(StringBuilder output, Person current, int generation = 0)
        {
            if (current == null)
                return;

            switch (generation)
            {
                case 0:
                    output.AppendLine("PERSON");
                    break;
                case -1:
                    output.AppendLine(current.sex == Sex.M ? "FATHER" : "MOTHER");
                    break;
                case -2:
                    output.AppendLine(current.sex == Sex.M ? "GRANDFATHER" : "GRANDMOTHER");
                    break;
                default:
                    var res = "";
                    for (int i = generation + 1; i <= 0; i++)
                        res += "GREAT-";
                    res += current.sex == Sex.M ? "GRANDFATHER" : "GRANDMOTHER";
                    output.AppendLine(res);
                    break;
            }

            output.AppendLine(GetUnusualPersonString(current));

            if(current.Father != null)
                GetDirectrAncestorsData(output, current.Father, generation - 1);
            if (current.Mother != null)
                GetDirectrAncestorsData(output, current.Mother, generation - 1);


        }
        private void GetDirectrKidsData(StringBuilder output, Person current, int generation = 0)
        {
            if (current == null)
                return;
            switch (generation)
            {
                case 0:
                   
                    break;
                case 1:
                    output.AppendLine(current.sex == Sex.M ? "SON" : "DOTHER");
                    break;
                case 2:
                    output.AppendLine(current.sex == Sex.M ? "GRANDSON" : "GRANDDOTHER");
                    break;
                default:
                    var str = "";
                    for (int i = generation + 1; i <= 0; i++)
                        str += "GREAT-";
                    str += current.sex == Sex.M ? "GRANDSON" : "GRANDDOTHER";
                    output.AppendLine(str);
                    
                    break;
            }
            if(current.Id != person.Id)
                output.AppendLine(GetUnusualPersonString(current));


            foreach(var ch in current.Child)
            {
                GetDirectrKidsData(output,ch, generation + 1); 
            }
        }
    }

}
