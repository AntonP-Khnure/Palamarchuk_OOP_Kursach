using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{

    /// <summary>
    /// Контролер для управління деревом генеалогії.
    /// </summary>
    public class TreeController
    {
        /// <summary>
        /// Дерево генеалогії.
        /// </summary>
        public Tree tree;

        /// <summary>
        /// Ініціалізує новий екземпляр класу TreeController з порожнім деревом.
        /// </summary>
        public TreeController()
        {
            tree = new Tree();
            

        }

        /// <summary>
        /// Ініціалізує новий екземпляр класу TreeController з деревом, зчитаним з файлу.
        /// </summary>
        /// <param name="file">Шлях до файлу.</param>
        public TreeController(string file)
        {
            tree = new Tree(file);
        }

        /// <summary>
        /// Знаходить осіб у дереві, частково співпадаючих з вказаним рядком.
        /// </summary>
        /// <param name="part_of_person">Частина рядка для пошуку.</param>
        /// <returns>Список осіб, які частково співпадають з вказаним рядком.</returns>
        public List<Person> Find(string part_of_person)
        {
            return tree.ToList().Where(p => p.ToString().ToLower().Contains(part_of_person.ToLower())).ToList();
        }

        /// <summary>
        /// Знаходить особу за її ідентифікатором.
        /// </summary>
        /// <param name="Id">Ідентифікатор особи.</param>
        /// <returns>Особу з вказаним ідентифікатором або null, якщо особа не знайдена.</returns>
        public Person Find(long Id)
        {
            var temp = tree.ToList().Where(p => p.Id == Id).ToList();
            return temp.Count == 0 ? null : temp[0];
        }

        /// <summary>
        /// Позначає хвороби осіб в дереві, використовуючи хвороби їх батьків.
        /// </summary>
        public void MarkDiseases()
        {
            foreach (var person in tree.ToList())
            {

                if (person.Father != null && person.Mother != null)
                {
                    foreach (var mot_dis in person.Mother.diseases)
                    {
                        var per_dis = person.diseases.FirstOrDefault(d => d.name == mot_dis.name);
                        if (per_dis == null || per_dis.ill != true)
                        {
                            var fat_dis = person.Father.diseases.FirstOrDefault(d => d.name == mot_dis.name);
                            person.diseases.Remove(per_dis);
                            if (Disease.Calculate(mot_dis, fat_dis, person.sex) != null)
                                person.diseases.Add(Disease.Calculate(mot_dis, fat_dis, person.sex));
                        }

                    }
                    foreach (var fat_dis in person.Father.diseases)
                    {
                        var per_dis = person.diseases.FirstOrDefault(d => d.name == fat_dis.name);
                        if (per_dis == null || per_dis.ill != true)
                        {
                            var mot_dis = person.Mother.diseases.FirstOrDefault(d => d.name == fat_dis.name);
                            person.diseases.Remove(person.diseases.FirstOrDefault(d => d.name == fat_dis.name));
                            if (Disease.Calculate(mot_dis, fat_dis, person.sex) != null)
                                person.diseases.Add(Disease.Calculate(mot_dis, fat_dis, person.sex));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Завантажує дерево з файлу.
        /// </summary>
        /// <param name="file">Шлях до файлу.</param>
        /// <returns>True, якщо завантаження пройшло успішно, False - у протилежному випадку.</returns>
        public bool Load(string file)
        {
            tree = Tree.GetFromFile(file);
            if (tree == null)
                return false;
            return true;
        }

        /// <summary>
        /// Зберігає дерево у файл.
        /// </summary>
        /// <param name="file">Шлях до файлу.</param>
        public void Save(string file)
        {
            Tree.WriteToFile(file, tree);
        }
    }

}
