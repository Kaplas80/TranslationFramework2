using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TF.IO;
using YakuzaGame.Annotations;

namespace YakuzaGame.Files.Table
{
    public class Column : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name;
        public int Type;
        public int DataCount;
        public int Size;

        public virtual bool HasChanges => false;

        protected int _tableRows;

        private byte[] _data;

        public Column(int type, int tableRows)
        {
            Type = type;
            _tableRows = tableRows;
        }

        public virtual void ReadData(ExtendedBinaryReader input)
        {
            _data = input.ReadBytes(Size);
        }

        public virtual string GetValue(int index)
        {
            return string.Empty;
        }

        public virtual string GetTranslatedValue(int index)
        {
            return string.Empty;
        }

        public virtual void SetTranslatedValue(int index, string value)
        {
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void SaveChanges(ExtendedBinaryWriter output)
        {
        }

        public virtual void LoadChanges(ExtendedBinaryReader input)
        {
        }

        public void WriteInfo(ExtendedBinaryWriter output)
        {
            var pos = output.Position;
            output.WriteString(Name, 48);
            output.Seek(pos + 48, SeekOrigin.Begin);
            output.Write(Type);
            output.Write(DataCount);
            output.Write(GetSize(output.Encoding));
            output.Write(0);
        }

        protected virtual int GetSize(Encoding encoding)
        {
            return _data.Length;
        }

        public virtual void WriteData(ExtendedBinaryWriter output)
        {
            output.Write(_data);
        }
    }

    public class Type0Column : Column
    {
        private IList<string> _data;
        private IList<string> _translatedData;
        private IList<string> _loadedData;
        private byte[] _remainder;

        public override bool HasChanges
        {
            get
            {
                for (var i = 0; i < _data.Count; i++)
                {
                    if (_loadedData[i] != _translatedData[i])
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public Type0Column(int type, int tableRows) : base(type, tableRows)
        {
            _data = new List<string>();
            _translatedData = new List<string>();
            _loadedData = new List<string>();
        }

        public override void ReadData(ExtendedBinaryReader input)
        {
            var startPos = input.Position;
            for (var i = 0; i < DataCount; i++)
            {
                var str = input.ReadString();
                _data.Add(str);
                _translatedData.Add(str);
                _loadedData.Add(str);
            }

            _remainder = input.ReadBytes((int) (startPos + Size - input.Position));
        }

        public override string GetValue(int index)
        {
            return _data[index];
        }

        public override string GetTranslatedValue(int index)
        {
            return _translatedData[index];
        }

        public override void SetTranslatedValue(int index, string value)
        {
            _translatedData[index] = value;
            OnPropertyChanged("Translation");
        }

        public override void SaveChanges(ExtendedBinaryWriter output)
        {
            output.Write(_translatedData.Count);
            foreach (var str in _translatedData)
            {
                output.WriteString(str);
            }
        }

        public override void LoadChanges(ExtendedBinaryReader input)
        {
            var dataCount = input.ReadInt32();
            if (dataCount != _translatedData.Count)
            {
                return;
            }

            for (var i = 0; i < dataCount; i++)
            {
                var str = input.ReadString();
                _translatedData[i] = str;
                _loadedData[i] = str;
            }
        }

        protected override int GetSize(Encoding encoding)
        {
            return _remainder.Length + _translatedData.Sum(str => encoding.GetByteCount(str) + 1);
        }

        public override void WriteData(ExtendedBinaryWriter output)
        {
            foreach (var str in _translatedData)
            {
                output.WriteString(str);
            }
            output.Write(_remainder);
        }
    }

    public class Type1Column : Column
    {
        private IList<string> _data;
        private IList<string> _translatedData;
        private IList<string> _loadedData;
        private byte[] _index;
        private byte[] _remainder;

        public override bool HasChanges
        {
            get
            {
                for (var i = 0; i < _data.Count; i++)
                {
                    if (_loadedData[i] != _translatedData[i])
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public Type1Column(int type, int tableRows) : base(type, tableRows)
        {
            _data = new List<string>();
            _translatedData = new List<string>();
            _loadedData = new List<string>();
            _index = new byte[tableRows];
        }

        public override void ReadData(ExtendedBinaryReader input)
        {
            var startPos = input.Position;
            for (var i = 0; i < DataCount; i++)
            {
                var str = input.ReadString();
                _data.Add(str);
                _translatedData.Add(str);
                _loadedData.Add(str);
            }

            for (var i = 0; i < _tableRows; i++)
            {
                var b = input.ReadByte();
                _index[i] = b;
            }

            _remainder = input.ReadBytes((int)(startPos + Size - input.Position));
        }

        public override string GetValue(int index)
        {
            return _data[_index[index]];
        }

        public override string GetTranslatedValue(int index)
        {
            return _translatedData[_index[index]];
        }

        public override void SetTranslatedValue(int index, string value)
        {
            _translatedData[_index[index]] = value;
            OnPropertyChanged("Translation");
        }

        public override void SaveChanges(ExtendedBinaryWriter output)
        {
            output.Write(_translatedData.Count);
            foreach (var str in _translatedData)
            {
                output.WriteString(str);
            }
        }

        public override void LoadChanges(ExtendedBinaryReader input)
        {
            var dataCount = input.ReadInt32();
            if (dataCount != _translatedData.Count)
            {
                return;
            }

            for (var i = 0; i < dataCount; i++)
            {
                var str = input.ReadString();
                _translatedData[i] = str;
                _loadedData[i] = str;
            }
        }

        protected override int GetSize(Encoding encoding)
        {
            return _index.ToArray().Length + _remainder.Length + _translatedData.Sum(str => encoding.GetByteCount(str) + 1);
        }

        public override void WriteData(ExtendedBinaryWriter output)
        {
            foreach (var str in _translatedData)
            {
                output.WriteString(str);
            }
            output.Write(_index.ToArray());
            output.Write(_remainder);
        }
    }

    public class Type2Column : Column
    {
        private class Info
        {
            public short From;
            public string Value;
            public string TranslatedValue;
            public string LoadedValue;
        }

        private IList<Info> _data;
        private byte[] _remainder;

        public override bool HasChanges
        {
            get
            {
                return _data.Any(t => t.LoadedValue != t.TranslatedValue);
            }
        }

        public Type2Column(int type, int tableRows) : base(type, tableRows)
        {
            _data = new List<Info>();
        }

        public override void ReadData(ExtendedBinaryReader input)
        {
            var startPos = input.Position;
            for (var i = 0; i < DataCount; i++)
            {
                var from = input.ReadInt16();
                var str = input.ReadString();
                var info = new Info {From = @from, Value = str, TranslatedValue = str, LoadedValue = str};
                _data.Add(info);
            }

            _remainder = input.ReadBytes((int)(startPos + Size - input.Position));
        }

        public override string GetValue(int index)
        {
            for (var i = _data.Count - 1; i >= 0; i--)
            {
                if (index >= _data[i].From)
                {
                    return _data[i].Value;
                }
            }

            return string.Empty;
        }

        public override string GetTranslatedValue(int index)
        {
            for (var i = _data.Count - 1; i >= 0; i--)
            {
                if (index >= _data[i].From)
                {
                    return _data[i].TranslatedValue;
                }
            }

            return string.Empty;
        }

        public override void SetTranslatedValue(int index, string value)
        {
            for (var i = _data.Count - 1; i >= 0; i--)
            {
                if (index >= _data[i].From)
                {
                    _data[i].TranslatedValue = value;
                    OnPropertyChanged("Translation");
                    return;
                }
            }
        }

        public override void SaveChanges(ExtendedBinaryWriter output)
        {
            output.Write(_data.Count);
            foreach (var str in _data)
            {
                output.WriteString(str.TranslatedValue);
            }
        }

        public override void LoadChanges(ExtendedBinaryReader input)
        {
            var dataCount = input.ReadInt32();
            if (dataCount != _data.Count)
            {
                return;
            }

            for (var i = 0; i < dataCount; i++)
            {
                var str = input.ReadString();
                _data[i].TranslatedValue = str;
                _data[i].LoadedValue = str;
            }
        }

        protected override int GetSize(Encoding encoding)
        {
            return 2 * _data.Count + _remainder.Length + _data.Sum(info => encoding.GetByteCount(info.TranslatedValue) + 1);
        }

        public override void WriteData(ExtendedBinaryWriter output)
        {
            foreach (var info in _data)
            {
                output.Write(info.From);
                output.WriteString(info.TranslatedValue);
            }
            output.Write(_remainder);
        }
    }

    public class TypeAColumn : Column
    {
        protected IList<int> _data;

        public TypeAColumn(int type, int tableRows) : base(type, tableRows)
        {
            _data = new List<int>();
        }

        public override void ReadData(ExtendedBinaryReader input)
        {
            Debug.Assert(DataCount == _tableRows);
            var startPos = input.Position;
            for (var i = 0; i < DataCount; i++)
            {
                var value = input.ReadInt32();
                _data.Add(value);
            }

            while (input.Position < startPos + Size)
            {
                var b = input.ReadByte();
                Debug.Assert(b == 0);
            }
        }

        public override string GetValue(int index)
        {
            return _data[index].ToString();
        }
    }

    public class Type9Column : TypeAColumn
    {
        public Type9Column(int type, int tableRows) : base(type, tableRows)
        {
        }

        public override string GetValue(int index)
        {
            return _data[index].ToString("X8");
        }
    }
}


