using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Funny.Accents.Core.Reflection.Abstractions.Attribues;
using Funny.Accents.Core.Reflection.Abstractions.Enumerations;
using Funny.Accents.Core.Reflection.Interfaces;
using Funny.Accents.Core.Reflection.Services;

namespace Funny.Accents.Core.Reflection.Abstractions.Services
{
    public abstract class SqlReflector<T>
        where T : class
    {
        private readonly string _command;
        private readonly ParameterOrientation _orientation;
        private readonly T _spDefinition;

        protected SqlReflector(string command,
            T spDefinition,
            ParameterOrientation orientation)
        {
            _command = command;
            _orientation = orientation;
            _spDefinition = spDefinition;
        }

        public (string spCommand, string spParameterSignature, List<SqlParameter> spParameterList) GetSpDetails()
        {
            return ConstructSpQuery(_spDefinition);
        }

        private (string, string, List<SqlParameter>) ConstructSpQuery(T spDefinition)
        {
            return AttributeReflector.ConstructTypeByMemberDetails<T, SpAttributes, (string, string, List<SqlParameter>)>(
                spDefinition, SpConstructor);
        }

        private (string, string, List<SqlParameter>) SpConstructor(IEnumerable<IMemberDetails<SpAttributes>> attr)
        {
            var spCommand = _command;
            var orientation = _orientation;

            var plist = IdentifySqlParameters(attr);
            var sb = new StringBuilder();
            var firstRec = 0;

            var orientedList = orientation == ParameterOrientation.Ordinal
                ? plist
                    .OrderBy(p => p.position)
                    .ThenBy(p => p.Sqlparam.ParameterName)
                    .Select(param => param.Sqlparam).ToList()
                : plist
                    .Select(param => param.Sqlparam).ToList();

            foreach (var param in orientedList)
            {
                var svalue = firstRec++ > 0 ? "," : " ";
                sb.Append($"{svalue}{param.ParameterName}");
            }

            return (spCommand, sb.ToString(), orientedList);
        }

        private IEnumerable<(int position, SqlParameter Sqlparam)> IdentifySqlParameters(
            IEnumerable<IMemberDetails<SpAttributes>> attrDetails)
        {
            var position = -1;

            return attrDetails.Select(details => new SqlParameter
            {
                ParameterName = details.AttributeDetails.ParameterName,
                SqlDbType = details.AttributeDetails.SqlDbType,
                Direction = details.AttributeDetails.Direction,
                Value = details.PropertyValue,
                Size = details.AttributeDetails.Size
            })
                .Select(param => (position++, param))
                .ToList();
        }/*End of IdentifySqlParameters method*/
    }/*End of SqlReflector class*/
}/*End of CmkReflectionUtilitiesCore.Services namespace*/
