using System;
using System.Text;

namespace Model
{
    public enum Sex
    {
        M,//male 0
        F//female 1
    }
    public class Person
    {
        #region data

        /// <summary>
        /// Отримує або задає ім'я особи.
        /// </summary>
        /// <param name="value">Нове значення імені.</param>
        public string Name { get { return name == default ? "_" : name; } set { name = value != "" ? value : default; } }
        private string name;

        /// <summary>
        /// Отримує або задає друге ім'я особи.
        /// </summary>
        /// <param name="value">Нове значення другого імені.</param>
        public string Second_name { get { return second_name == default ? "_" : second_name; } set { second_name = value != "" ? value : default; } }
        private string second_name;

        /// <summary>
        /// Отримує або задає прізвище особи.
        /// </summary>
        /// <param name="value">Нове значення прізвища.</param>
        public string Surname { get { return surname == default ? "_" : surname; } set { surname = value != "" ? value : default; } }
        private string surname;

        /// <summary>
        /// Отримує повне ім'я особи, включаючи ім'я, друге ім'я та прізвище.
        /// </summary>
        /// <returns>Повне ім'я особи.</returns>
        public string Full_name { get { return Name + " " + Second_name + " " + Surname; } }
        public Sex sex;
        private DateTime birth_date;
        private DateTime death_date;

        /// <summary>
        /// Отримує або задає дату народження особи.
        /// </summary>
        /// <param name="value">Нова дата народження.</param>
        public DateTime Birth_date { get { return birth_date; } set { birth_date = value; } }

        /// <summary>
        /// Отримує або задає дату смерті особи.
        /// </summary>
        /// <param name="value">Нова дата смерті.</param>
        /// <remarks>Якщо встановлювана дата смерті раніше дати народження, вона не буде встановлена.</remarks>
        public DateTime Death_date { get { return death_date; } set { death_date = value > birth_date ? value : default; } }

        /// <summary>
        /// Обчислює вік особи.
        /// </summary>
        /// <returns>Вік особи.</returns>
        public double Age
        {
            get
            {
                if (birth_date == default && death_date == default) { return 0; }
                else if (birth_date != default && death_date == default) { return Math.Round((DateTime.Now - birth_date).Days / 365.0, 1); }
                else if (birth_date == default && death_date != default) { return 0; }
                else { return Math.Round((death_date - birth_date).Days / 365.0, 1); }
            }
        }

        /// <summary>
        /// Список дітей особи.
        /// </summary>
        /// /// <returns>список дітей особи відсортований за їх Id.</returns>
        public List<Person>? Child { get {  child.Sort((x,y)=> x.Id.CompareTo(y.Id));return child; } set { child = value; } }
        private List<Person>? child;

        /// <summary>
        /// Отримує або задає батька особи.
        /// </summary>
        /// <param name="value">Нове значення батька особи.</param>
        /// <remarks>Якщо поточна особа вже має батька, вона буде видалена зі списку дітей цього батька.</remarks>
        public Person? Father
        {
            get { return father; }
            set
            {
                if (father != null)
                {
                    father.Child.Remove(this);
                    father = value;
                }
                else
                    father = value;
            }
        }
        private Person? father;

        /// <summary>
        /// Отримує або задає матір особи.
        /// </summary>
        /// <param name="value">Нове значення матері особи.</param>
        /// <remarks>Якщо поточна особа вже має матір, вона буде видалена зі списку дітей цієї матері.</remarks>
        public Person? Mother
        {
            get { return mother; }
            set
            {
                if (mother != null)
                {
                    mother.Child.Remove(this);
                    mother = value;
                }
                else
                    mother = value;
            }
        }
        private Person? mother;

        /// <summary>
        /// Унікальний ідентифікатор особи.
        /// </summary>
        public long Id { get { id = (id == default ? (Name == "root" ? 1 : DateTime.Now.Ticks) : id); return id; } protected set { id = value; } }
        protected long id;

        /// <summary>
        /// Список хвороб особи.
        /// </summary>
        public HashSet<Disease> diseases;
        #endregion

        /// <summary>
        /// Ініціалізує новий екземпляр класу Person.
        /// </summary>
        public Person()
        {
            child = new List<Person>();
            diseases = new HashSet<Disease>();
        }

        /// <summary>
        /// Представляє особу у вигляді рядка.
        /// </summary>
        /// <returns>Рядкове представлення особи.</returns>
        public override string ToString()
        {
            var res = new StringBuilder();
            res.AppendLine(" Name: " + Full_name);
            res.AppendLine(" Father: " + (father == null ? "-" : father.Full_name));
            res.AppendLine(" Mother: " + (mother == null ? "-" : mother.Full_name));
            res.AppendLine(" Age: " + Age.ToString());
            if (Birth_date != default) { res.AppendLine(" Birth day: " + Birth_date.ToString("d") + " "); }
            if (Death_date != default) { res.AppendLine(" Death day: " + Death_date.ToString("d") + " "); }
            res.AppendLine(" Sex: " + (sex == Sex.M ? "Чоловік" : "Жінка"));

            if (child.Count > 0)
            {
                res.AppendLine(" Kids:");
                foreach (var ch in child)
                {
                    res.AppendLine("  " + ch.Full_name);
                }
            }

            if (diseases.Count > 0)
            {
                res.AppendLine("Diseases: ");
                foreach (var disease in diseases)
                {
                    if (sex == Sex.M)
                    {
                        if (disease.ill)
                        {
                            res.AppendLine("have " + disease.name);
                        }
                        else if (disease.risk)
                        {
                            res.AppendLine("was risk to gain " + disease.name);
                        }
                    }
                    else
                    {
                        if (disease.ill)
                        {
                            res.AppendLine("have " + disease.name);
                        }
                        else if (disease.recessive)
                        {
                            res.AppendLine("recesive " + disease.name);
                        }
                        else if (disease.recessive_risk)
                        {
                            res.AppendLine("may be recesive " + disease.name);
                        }
                    }

                }
            }
            return res.ToString();
        }
    }
    public class RootPerson : Person
    {
        public RootPerson() {
            base.Id = 1;
            base.Birth_date = DateTime.Now;
            base.Name = "root";
        }
         
    }

}
