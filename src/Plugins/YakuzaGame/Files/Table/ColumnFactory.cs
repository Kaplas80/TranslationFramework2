namespace YakuzaGame.Files.Table
{
    public static class ColumnFactory
    {
        public static Column GetColumn(int type, int tableRows)
        {
            if (type == 0x00 || type == 0x08)
            {
                return new Type0Column(type, tableRows);
            }

            if (type == 0x01)
            {
                return new Type1Column(type, tableRows);
            }

            if (type == 0x02)
            {
                return new Type2Column(type, tableRows);
            }

            /*
            if (type == 0x09 || type == 0x0B || type == 0x06)
            {
                return new Type9Column(type, tableRows);
            }

            if (type == 0x0A)
            {
                return new TypeAColumn(type, tableRows);
            }
            */
            var column = new Column(type, tableRows);
            return column;
        }
    }
}