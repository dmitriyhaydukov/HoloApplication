using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInterface.WorkspacePanel
{
    public abstract class WorkspacePanelItemControllerBase
    {
        protected WorkspacePanelItem View;

        protected WorkspacePanelItemControllerBase(WorkspacePanelItem view)
        {
            View = view;
        }
    }
}
