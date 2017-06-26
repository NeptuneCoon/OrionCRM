using System;
using System.Collections.Generic;
using System.Text;
using Orion.CRM.DataAccess;

namespace Orion.CRM.Application
{
    public class ResourceNoteAppService
    {
        private ResourceNoteDataAdapter adapter = new ResourceNoteDataAdapter();
        public int InsertResourceNote(Entity.ResourceNote note)
        {
            return adapter.InsertResourceNote(note);
        }

        public bool DeleteResourceNote(int id)
        {
            return adapter.DeleteResourceNote(id);
        }

        public IEnumerable<Entity.ResourceNote> GetNotesByResourceId(int resourceId)
        {
            return adapter.GetNotesByResourceId(resourceId);
        }
    }
}
