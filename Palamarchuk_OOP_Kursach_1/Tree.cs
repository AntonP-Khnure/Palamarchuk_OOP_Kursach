using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    /// <summary>
    /// Представляє генеалогічне дерево.
    /// </summary>
    public class Tree
    {
        /// <summary>
        /// Головна особа дерева.
        /// </summary>
        public Person Head { get { return head; } private set { head = value; } }
        private Person head;

        /// <summary>
        /// Кількість осіб у дереві.
        /// </summary>
        public int Count
        {
            get
            {
                return ToList().Count;
            }
        }

        /// <summary>
        /// Ініціалізує новий екземпляр класу Tree з файлу.
        /// </summary>
        /// <param name="file">Шлях до файлу.</param>
        public Tree(string file)
        {
            var temp = GetFromFile(file);
            if (temp != null)
            {
                head = temp.Head;
            }
            else
            {
                head = null;
            }
        }

        /// <summary>
        /// Ініціалізує новий екземпляр класу Tree з вказаною особою як головою.
        /// </summary>
        /// <param name="person">Особа, яка стане головою дерева.</param>
        public Tree(Person person) : this()
        {
            AddChild(head, person);
        }

        /// <summary>
        /// Ініціалізує новий екземпляр класу Tree без голови.
        /// </summary>
        public Tree()
        {
            head = new RootPerson();
        }

        /// <summary>
        /// Зчитує дерево з файлу.
        /// </summary>
        /// <param name="file">Шлях до файлу.</param>
        /// <returns>Дерево, зчитане з файлу, або null, якщо зчитування не вдалося.</returns>
        public static Tree GetFromFile(string file)
        {
            try
            {
                using (var sr = new StreamReader(file))
                {
                    var text = sr.ReadToEnd();
                    var res = ToTree(JsonConvert.DeserializeObject<List<Person>>(text));
                    return res;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Записує дерево у файл.
        /// </summary>
        /// <param name="file">Шлях до файлу.</param>
        /// <param name="tree">Дерево, яке потрібно записати у файл.</param>
        public static void WriteToFile(string file, Tree tree)
        {
            if (!File.Exists(file))
            {
                using (var stream = File.Create(file)) { }
            }
            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented,
            };
            using (var sw = new StreamWriter(file))
            {
                var res = JsonConvert.SerializeObject(tree.ToList(), settings);
                sw.Write(res);
            }
        }

        /// <summary>
        /// Додає дитину до батька або матері.
        /// </summary>
        /// <param name="per">Батько або мати.</param>
        /// <param name="child">Дитина, яку потрібно додати.</param>
        /// <exception cref="Exception">Виникає, якщо батько або дитина дорівнюють null, або якщо спроба додати предків як дитину.</exception>
        public void AddChild(Person per, Person child)
        {
            if (per .Id == 1)
            {
                if(!head.Child.Contains(child))
                    per.Child.Add(child);
                return;
            }


            if (per == null || child == null)
                throw new Exception("person == null || child == null");

            var temp = new List<Person>();
            GetDirectAncestors(per, temp);
            if (temp.Contains(child))
                throw new Exception("tried add ancestors as child");

            if (child.Birth_date.Date < per.Birth_date.Date)
                throw new Exception("tried add older as child");

            if (per.Child.IndexOf(child) != -1)
                return;


            if (per.sex == Sex.M)
            {
                if (child.Father != null)
                    child.Father.Child.Remove(child);
                child.Father = per;
            }
            else
            {
                if (child.Mother != null)
                    child.Mother.Child.Remove(child);
                child.Mother = per;
            }

            per.Child.Add(child);
        }

        /// <summary>
        /// Отримує безпосередніх предків особи.
        /// </summary>
        /// <param name="current">Поточна особа.</param>
        /// <param name="output">Список предків для заповнення.</param>
        public void GetDirectAncestors(Person current, List<Person> output)
        {
            if (current == null)
                return;

            output.Add(current);

            if (current.Father != null)
            {
                GetDirectAncestors(current.Father, output);
            }
            if (current.Mother != null)
            {
                GetDirectAncestors(current.Mother, output);
            }
        }


        /// <summary>
        /// Повертає список осіб у дереві.
        /// </summary>
        /// <returns>Список осіб у дереві.</returns>
        public List<Person> ToList()
        {
            List<long> visited = new List<long>();
            List<Person> output = new List<Person>();
            round(output, visited, this.Head);
            return output;
        }

        /// <summary>
        /// Рекурсивно обходить дерево та додає осіб у вихідний список.
        /// </summary>
        /// <param name="output">Список для заповнення.</param>
        /// <param name="visited">Список відвіданих осіб для уникнення зациклення.</param>
        /// <param name="current">Поточна особа.</param>
        private void round(List<Person> output, List<long> visited, Person current)
        {
            if (current == null)
                return;

            output.Add(current);
            visited.Add(current.Id);

            if (current.Father != null && -1 == visited.IndexOf(current.Father.Id))
            {
                round(output, visited, current.Father);
            }
            if (current.Mother != null && -1 == visited.IndexOf(current.Mother.Id))
            {
                round(output, visited, current.Mother);
            }
            foreach (Person ch in current.Child)
            {
                if (visited.IndexOf(ch.Id) == -1)
                {
                    round(output, visited, ch);
                }
            }
        }

        /// <summary>
        /// Перетворює список осіб на дерево, використовуючи першу особу як голову.
        /// </summary>
        /// <param name="list">Список осіб.</param>
        /// <returns>Дерево, створене з вказаного списку осіб.</returns>
        static public Tree ToTree(List<Person> list)
        {
            if (list.Count == 0)
            {
                return new Tree();
            }
            Tree tree = new Tree();
            tree.Head = list[0];
            return tree;
        }
    }

}
