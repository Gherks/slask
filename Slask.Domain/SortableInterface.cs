using System;
using System.Collections.Generic;
using System.Text;

namespace Slask.Domain
{
    public interface SortableInterface
    {
        public int SortOrder { get; }

        public void UpdateSortOrder();
    }
}
