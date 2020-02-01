using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Funny.Accents.Core.Reflection.Abstractions.Attribues;
using Funny.Accents.Core.Reflection.Abstractions.Enumerations;
using Funny.Accents.Core.Reflection.Interfaces;
using Funny.Accents.Core.Reflection.Services;

namespace Funny.Accents.Core.Reflection.Abstractions.Services
{
    public abstract class DapperReflector<T>
        where T : class
    {
        private readonly string _command;
        private readonly ParameterOrientation _orientation;
        private readonly T _spDefinition;

        protected DapperReflector(string command,
            T spDefinition,
            ParameterOrientation orientation)
        {
            _command = command;
            _orientation = orientation;
            _spDefinition = spDefinition;
        }

        public (string spCommand, DynamicParameters spParameters) GetSpDetails()
        {
            return ConstructSpQuery(_spDefinition);
        }

        private (string, DynamicParameters) ConstructSpQuery(T spDefinition)
        {
            return AttributeReflector.ConstructTypeByMemberDetails<T, SpAttributes, (string, DynamicParameters)>(
                spDefinition, IdentifySqlParameters);
        }

        private (string, DynamicParameters) IdentifySqlParameters(
            IEnumerable<IMemberDetails<SpAttributes>> attrDetails)
        {
            var parameterList = new DynamicParameters();

            var memberDetails = attrDetails.ToList();
            var orientedList = _orientation == ParameterOrientation.Ordinal
                ? memberDetails
                    .OrderBy(p => p.AttributeDetails.OrdinalPosition)
                    .ThenBy(p => p.AttributeDetails.ParameterName)
                    .Select(param => param).ToList()
                : memberDetails
                    .Select(param => param).ToList();

            foreach (var attr in orientedList)
            {
                var dbType = attr.AttributeDetails.IsDbTypePopulated
                    ? (DbType?)attr.AttributeDetails.DbType
                    : null;
                var size = attr.AttributeDetails.IsSizePopulated
                    ? (int?)attr.AttributeDetails.Size
                    : null;
                var precision = attr.AttributeDetails.IsPrecisionPopulated
                    ? (byte?)attr.AttributeDetails.Precision
                    : null;
                var scale = attr.AttributeDetails.IsScalePopulated
                    ? (byte?)attr.AttributeDetails.Scale
                    : null;

                parameterList.Add(attr.AttributeDetails.ParameterName,
                    attr.PropertyValue,
                    dbType, attr.AttributeDetails.Direction,
                    size, precision, scale);
            }

            return (_command, parameterList);
        }/*End of IdentifySqlParameters method*/
    }/*End of SqlReflector class*/
}/*End of CmkReflectionUtilitiesCore.Services namespace*/
