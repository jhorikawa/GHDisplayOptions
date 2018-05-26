using System;
using System.Threading;
using System.Threading.Tasks;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using System.Windows.Forms;

namespace GHDisplayOptions
{
    public class GHMenuDisplayOptions : GH_AssemblyPriority
    { 
        private GH_Canvas Owner; 

        public GHMenuDisplayOptions() : base() { }

        public override GH_LoadingInstruction PriorityLoad()
        {
            /** Create a task to register a menu on a different thread. */
            Task task = new Task(OnStartup);
            task.Start();

            return GH_LoadingInstruction.Proceed;
        }


        #region menu registration

        /** Function to run on GH startup */
        private void OnStartup()
        {
            /** Create an event for when canvas is created. */
            Instances.CanvasCreated += OnCanvasCreated;

            /** Check for GH_DocumentEditor every 0.2s */
            GH_DocumentEditor editor = null;
            while (editor == null)
            {
                editor = Grasshopper.Instances.DocumentEditor;
                Thread.Sleep(200);
            }

            /** Register menu when GH_DocumentEditor is not null */
            Populate(editor.MainMenuStrip);

        }

        /** Main Menu Registration */
        private void Populate(MenuStrip mms)
        {
            /** Set a anme for main menu. */
            var tl = "DisplayControl";

            /** Register main menu. */
            ToolStripMenuItem menu = new ToolStripMenuItem(tl);
            mms.Items.Add(menu);

            /** Register sub menus. */
            PopulateSub(mms, menu);
        }

        
        /** Sub Menu Registration. */
        private void PopulateSub(MenuStrip mms, ToolStripMenuItem menu)
        {
            /** Sub menu texts*/
            string[] subMenuTexts =
            {
                "Change Selected Display to Text",
                "Change Selected Display to Icon",
                "Change Selected Display to Application Setting"
            };

            /** Sub menu keys */
            Keys[] keysArr =
            {
                Keys.Control | Keys.T,
                Keys.Control | Keys.I,
                Keys.None
            };

            /** Sub menu event handlers */
            EventHandler[] handlers =
            {
                OnSelectText_Click,
                OnSelectIcon_Click,
                OnSelectApp_Click
            };

            /** sub menu registration */
            for(int i=0; i<subMenuTexts.Length; i++)
            {
                string subMenuText = subMenuTexts[i];
                Keys keys = keysArr[i];
                EventHandler handler = handlers[i];

                PopulateSubMenuMethod(mms, menu, subMenuText, keys, handler);
            }

        }

        /** Menu registration using MethodInvoker */
        private void PopulateSubMenuMethod(MenuStrip mms, ToolStripMenuItem menu, string subMenuText, Keys keys, EventHandler handler)
        {
            MethodInvoker method = delegate
            {
                var subMenuItem = new ToolStripMenuItem(subMenuText);
                subMenuItem.ShortcutKeys = keys;
                subMenuItem.Click += handler;
                menu.DropDownItems.Add(subMenuItem);
            };
            mms.BeginInvoke(method);
        }

        #endregion


        #region EventHandlers

        /** Function to run when selecting menu */
        private void SelectMenu(GH_IconDisplayMode displayMode)
        {
            /** Check if owner is null */
            if (Owner != null)
            {
                /** Check if document exists */
                if (Owner.IsDocument)
                {
                    /** Set display options for every selected component */
                    GH_Document doc = Owner.Document;
                    foreach (IGH_DocumentObject docObject in doc.SelectedObjects())
                    {
                        docObject.IconDisplayMode = displayMode;
                        docObject.ExpireSolution(true);
                    }
                    Instances.RedrawCanvas();
                }
                else
                {
                    MessageBox.Show("Add at least one component.");
                }
            }
            else
            {
                MessageBox.Show("Canvas is null");
            }
        }

        private void OnSelectText_Click(object sender, EventArgs e)
        {
            SelectMenu(GH_IconDisplayMode.name);
        }

        private void OnSelectIcon_Click(object sender, EventArgs e)
        {
            SelectMenu(GH_IconDisplayMode.icon);
        }

        private void OnSelectApp_Click(object sender, EventArgs e)
        {
            SelectMenu(GH_IconDisplayMode.application);
        }

        /** Set Owner variable when canvas is created. */
        private void OnCanvasCreated(GH_Canvas canvas)
        {
            Owner = canvas;
        }

        #endregion
    }
}
