using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotInsideNode
{
    class NullView : IView
    {
        public void Draw()
        {
           
        }
    }

    class Column3View:IView
    {
        public void Clear()
        {
            LeftView = new NullView();
            MidView = new NullView();
            RightView = new NullView();
        }

        public IView LeftView
        {
            get; set;
        } = new NullView();

        public IView MidView
        {
            get; set;
        } = new NullView();

        public IView RightView
        {
            get; set;
        } = new NullView();

        public static ImGuiTableFlags TableFlags = ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Hideable;

        public virtual void Draw()
        {
            if (ImGui.BeginTable("EditorTable", 3, TableFlags))
            {
                ImGui.TableNextRow();
                //Left
                ImGui.TableSetColumnIndex(0);
                LeftView.Draw();

                //Middle
                ImGui.TableSetColumnIndex(1);
                MidView.Draw();

                //Right
                ImGui.TableSetColumnIndex(2);
                RightView.Draw();

                ImGui.EndTable();

            }
        }

    }
}
