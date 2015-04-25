using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormSnapping
{
    public partial class Form2 : Form
    {
        int currentPanel;
        Point sourceLocation;
        PanelLocation[] locations = new PanelLocation[4];
        int sIndex, dIndex;
        public Form2()
        {
            InitializeComponent();
            updateLocationList();
            Console.WriteLine("================initialization=======================");
            printInfo();
        }

        private void printInfo()
        {
            for (int k = 0; k < locations.Length; k++)
            {
                if (locations[k] != null)
                    Console.Write(locations[k].name + "-" + locations[k].panel.TabIndex + ":" + locations[k].panel.Location +":"+ locations [k].point);
            }
            Console.WriteLine();
        }

        private void updateLocationList()
        {
            Control.ControlCollection coll = this.Controls; int i = 0;
            foreach (Control c in coll)
            {
              
                if (c is Panel)
                {
                    PanelLocation p = new PanelLocation(c.Name, c.Location, c.TabIndex, (Panel)c);
                    locations[i++]=p;
                }

                

                
            }
            Array.Sort(locations);
        }



        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            currentPanel = 1; sourceLocation = this.panel1.Location;
            sIndex = this.panel1.TabIndex;
            
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            currentPanel = 0;
         
            Panel source = (Panel)sender;
            SwapPosition(source, sourceLocation);
            reArrangeLocation(source);
           
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            sIndex = this.panel2.TabIndex; currentPanel = 2; sourceLocation = this.panel2.Location;
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            currentPanel = 0;
            Panel source = (Panel)sender;
            SwapPosition(source, sourceLocation);
            reArrangeLocation(source);

        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            currentPanel = 3; sIndex = this.panel3.TabIndex; sourceLocation = this.panel3.Location;
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            currentPanel = 0;
            Panel source = (Panel)sender;
            SwapPosition(source, sourceLocation); reArrangeLocation(source); 
        }

        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            currentPanel = 4; sourceLocation = this.panel4.Location; sIndex = this.panel4.TabIndex;
        }

        private void panel4_MouseUp(object sender, MouseEventArgs e)
        {
            currentPanel = 0;
            Panel source = (Panel)sender;
            SwapPosition(source, sourceLocation); reArrangeLocation(source);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(currentPanel==1)
            {
                panel1.Location = new Point(Cursor.Position.X-200, Cursor.Position.Y-200);
            }
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentPanel == 2)
            {
                panel2.Location = new Point(Cursor.Position.X - 200, Cursor.Position.Y - 200);
            }
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentPanel == 3)
            {
                panel3.Location = new Point(Cursor.Position.X - 200, Cursor.Position.Y - 200);
            }
        }

        private void panel4_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentPanel == 4)
            {
                panel4.Location = new Point(Cursor.Position.X - 200, Cursor.Position.Y - 200);
            }
        }



        private void SwapPosition(Panel source,Point sourceLocation)
        {
            Control crtl = source as Control;

            Control.ControlCollection coll = this.Controls; Panel destination;
            foreach (Control c in coll)
            {
                Point position = Cursor.Position;
                if (c is Panel && c != source)
                {
                     if (c.ClientRectangle.Contains(c.PointToClient(Cursor.Position)) || c.RectangleToClient(c.ClientRectangle).Contains(Cursor.Position))
                    { destination = (Panel)c;
                    source.Location = destination.Location; 
                     source.TabIndex = dIndex;
                    //destination.Location = sourceLocation;
                    dIndex = destination.TabIndex;
                   
                    if (sIndex < dIndex)
                    {
                        moveLeft(sIndex, dIndex, crtl);
                        Console.WriteLine("==============After Move Left=============");
                        printInfo();
                        break;
                    }
                    else if (sIndex > dIndex)
                    { moveRight(sIndex, dIndex, source);
                    Console.WriteLine("==============After Move Right=============");
                    printInfo();
                    break;
                    }
                           
                    
                    }


                }
            }

           
           
        }

        private void reArrangeLocation(Panel source) {
            for (int i = 0; i < locations.Length; i++)
            {
                if (locations[i] != null && locations[i].panel != source)
                    locations[i].updateControl();

              
                
            }
            Console.WriteLine("==============After Update============");
            printInfo();

            Array.Sort(locations);
        }

        private void moveRight(int sIndex, int dIndex, Panel source)
        {
             Panel current = null, prev = null, temp = null;
            
            Control.ControlCollection coll = this.Controls;

            foreach (Control c in this.Controls.Cast<Control>()
                                         .Where(c => c.TabIndex < sIndex && c.TabIndex >= dIndex)
                                         .OrderBy(c => c.TabIndex))
            {
                if (c is Panel)
                {
                    temp = prev;
                    prev = current;
                     current = (Panel)c;
                     if (current != null && prev != null)
                     {
                         MessageBox.Show(prev.Name + ":" + current.Name);
                         updatePanel(prev.Name, current.Location, current.TabIndex);
                     }
                }
            }

            updatePanel(current.Name, sourceLocation, sIndex);

        }

        private void moveLeft(int sIndex, int dIndex, Control ctrl)
        {
            int i = 0;
            
            Control.ControlCollection coll = this.Controls;

            foreach (Control c in this.Controls.Cast<Control>()
                                         .Where(c => c.TabIndex > sIndex && c.TabIndex <= dIndex)
                                         .OrderBy(c => c.TabIndex))
            {
                
                if (c is Panel)
                {
                    //MessageBox.Show(c.Name +":"+ctrl.Name);
                    if (i > 0)
                        updatePanel(c.Name, ctrl.Location, ctrl.TabIndex);
                    else {
                        updatePanel(c.Name, sourceLocation, sIndex); i++;
                    }
                    ctrl = c;
                   
                }
                
            }


        }

        private void updatePanel(String p, Point l, int i)
        {
            for (int j = 0; j < locations.Length;j++ )
            {
                if (locations[j]!=null && locations[j].name == p)
                {
                    locations[j].index = i;
                    locations[j].point = l;
                }
            }
           
        }

    }


    class PanelLocation:IComparable<PanelLocation> 
    {
    

        public String name;
        public Point point;
        public Panel panel;
        public int index;

        public PanelLocation(String p, Point point, int index, Panel c)
        {
            // TODO: Complete member initialization
            this.name = p;
            this.point = point;
            this.panel =c;
            this.index = index;
        }
        public int CompareTo(PanelLocation that)
        {
            if (that == null) return 1;
            if (this.panel.TabIndex > that.panel.TabIndex) return 1;
            if (this.panel.TabIndex < that.panel.TabIndex) return -1;
            return 0;
        }

        public void updateControl (){
        this.panel.Location =this.point;
        this.panel.TabIndex =this.index;
        }
    }
}
