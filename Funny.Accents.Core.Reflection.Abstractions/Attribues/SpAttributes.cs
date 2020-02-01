using System;
using System.Data;

namespace Funny.Accents.Core.Reflection.Abstractions.Attribues
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SpAttributes : Attribute
    {
        private int _size = -1;
        private byte _scale = 0;
        private byte _precision = 0;
        private DbType _dbType = default;

        public string ParameterName { get; set; }
        public SqlDbType SqlDbType { get; set; }
        public int OrdinalPosition { get; set; }
        public ParameterDirection Direction { get; set; }
        public bool IsSizePopulated { get; set; }
        public bool IsPrecisionPopulated { get; set; }
        public bool IsScalePopulated { get; set; }
        public bool IsDbTypePopulated { get; set; }
        public DbType DbType
        {
            get => _dbType;
            set
            {
                _dbType = value;
                IsDbTypePopulated = true;
            }
        }

        public int Size
        {
            get => _size;
            set
            {
                _size = value;
                IsSizePopulated = true;
            }
        }

        public byte Precision
        {
            get => _precision;
            set
            {
                _precision = value;
                IsPrecisionPopulated = true;
            }
        }
        public byte Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                IsScalePopulated = true;
            }
        }
    }/*End of SpAttributes class*/
}/*End of CmkDatabaseUtilitiesCore.interfaces namespace*/
