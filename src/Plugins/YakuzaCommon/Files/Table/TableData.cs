using System.Collections.Generic;
using System.Linq;

namespace YakuzaCommon.Files.Table
{
    public class TableData
    {
        public IList<Column> Columns { get; }
        public int NumRows { get; set; }
        public TableData()
        {
            Columns = new List<Column>();
        }
    }
}
