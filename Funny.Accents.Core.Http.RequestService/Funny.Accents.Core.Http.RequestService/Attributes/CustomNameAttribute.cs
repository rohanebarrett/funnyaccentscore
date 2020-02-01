using System;

namespace Funny.Accents.Core.Http.RequestService.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class CustomNameAttribute : Attribute
    {
        public string MemberName { get; private set; }

        public CustomNameAttribute(string memberNameParam)
        {
            MemberName = memberNameParam;
        }
    }/*End of CustomNameAttribute class*/
}/*End of Funny.Accents.Core.Http.RequestService.Attributes namespace*/
