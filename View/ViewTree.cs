using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace View
{
    public class ViewTree : Panel
    {
        internal List<ViewPersonLabel> people;

        public const int top_offset = 2000;
        public const int left_offset = 2000;
        public const int Height_ = 4000;
        public const int Width_ = 4000;

        private bool dragging = false;
        private Point dragStartPoint = new Point();

        public const int distance_between_generations = 80;
        public const int distance_between_people = 30;

        public ViewTree(TreeController tc) {


            people = new List<ViewPersonLabel>();

            BackColor = Color.FromArgb(255, 218, 195);

            this.MouseDown += new MouseEventHandler(this.ViewTree_MouseDown);
            this.MouseUp += new MouseEventHandler(this.ViewTree_MouseUp);
            this.MouseMove += new MouseEventHandler(this.ViewTree_MouseMove);
            

            Width = Width_;
            Height = Height_;
            AutoSize = false;
        }

        private void ViewTree_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.dragging = true;
                this.dragStartPoint = new Point(e.X, e.Y);
                this.Cursor = Cursors.NoMove2D;
            }
        }

        private void ViewTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.dragging = false;
                this.Cursor = Cursors.Default;
            }
        }

        private void ViewTree_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragging)
            {
                Point currentPosition = (Parent as Panel).AutoScrollPosition;
                (Parent as Panel).AutoScrollPosition = new Point(dragStartPoint.X - e.X - currentPosition.X, dragStartPoint.Y - e.Y - currentPosition.Y);
                
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (people == null || people.Count == 0)
                return;

            var pen = new Pen(Color.Black);
            var graphics = e.Graphics;

            foreach (var person in people)
            {
                // Draw line from person to their Father
                if (person.person.Father != null)
                {
                    var Father = people.FirstOrDefault(p => p.person.Id == person.person.Father.Id);
                    if (Father != null)
                    {
                        pen = new Pen(Color.Blue,2);
                        graphics.DrawLine(pen, 
                            new PointF(person.Left + ViewPersonLabel.Width_ / 2, person.Top) , 
                            new PointF(Father.Left + ViewPersonLabel.Width_ / 2, Father.Top + ViewPersonLabel.Height_));
                    }
                }

                // Draw line from person to their Mother
                if (person.person.Mother != null)
                {
                    var Mother = people.FirstOrDefault(p => p.person.Id == person.person.Mother.Id);
                    if (Mother != null)
                    {
                        pen = new Pen(Color.Pink,3);
                        graphics.DrawLine(pen, 
                            new PointF(person.Left + ViewPersonLabel.Width_ / 2, person.Top), 
                            new PointF(Mother.Left + ViewPersonLabel.Width_ / 2, Mother.Top + ViewPersonLabel.Height_));
                    }
                }
            }
        }
        public void Update(TreeController tc,bool force = false)
        {
                
                if (tc.tree.Count == people.Count && !force)
                    return;

                var visited = new List<long>();
                var coords = new List<Point>();
                var generations_width = new List<int>();

                people = new List<ViewPersonLabel>();

                tc.MarkDiseases();
                GetCoordsList(coords, visited,tc.tree.Head, generations_width);
                AplyGlobalOffset(coords, GetGlobalOffset(coords));
                coords.RemoveAt(visited.IndexOf(1));
                var temp = tc.tree.ToList();
                temp.RemoveAt(visited.IndexOf(1));
                FillViewPeopleList(coords, temp);
                

                this.Invoke(() =>
                    {
                        Controls.Clear();
                        foreach (var vp in people)
                        {
                            Controls.Add(vp);
                        }
                    }
                );

        }
        private Point GetGlobalOffset(List<Point> data) {
            var X = left_offset;
            var Y = data.Max(p => p.Y) + top_offset; 

            return new Point(X, Y);
        }
        private void AplyGlobalOffset(List<Point> data,Point offset) {
            for (int i = 0; i < data.Count; i++)
            {
                data[i] = new Point(offset.X + data[i].X, offset.Y - data[i].Y);
            }
        }
        private void GetCoordsList(List<Point> output, List<long> visited,Person current, List<int> generations_width,int generation = 20)
        {

            

            if (current == null)
                return;

            while(generations_width.Count < generation + 1)
            {
                generations_width.Add(0);
            }
            
            if (current.Id == 1)
                generations_width[generation] -= 1;
            
            var left = ViewPersonLabel.Width_ * generations_width[generation] + generation % 2 * (ViewPersonLabel.Width_ + distance_between_people) / 2 + generations_width[generation] * distance_between_people;
            var top = ViewPersonLabel.Height_ * generation + generation * distance_between_generations;
            visited.Add(current.Id);
            output.Add(new Point(left,top));

            generations_width[generation] += 1;

            if (current.Father != null && -1 == visited.IndexOf(current.Father.Id))
            {
                GetCoordsList(output, visited, current.Father,generations_width,generation + 1);
            }
            if (current.Mother != null && -1 == visited.IndexOf(current.Mother.Id))
            {
                GetCoordsList(output, visited, current.Mother, generations_width, generation + 1);
            }

            foreach (Person ch in current.Child)
            {
                if (visited.IndexOf(ch.Id) == -1)
                {
                    GetCoordsList(output, visited, ch, generations_width, generation - 1);
                }
            }
            
        }
        private void FillViewPeopleList(List<Point> coords,List<Person> persons) { 
            for(int i = 0; i < coords.Count; i++)
            {
                people.Add(new ViewPersonLabel(persons[i], coords[i]){ Parent = this });
            } 
        }
    }
}
