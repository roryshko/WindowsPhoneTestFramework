using System;

namespace WindowsPhoneTestFramework.CommandLine.EmuHost
{
    public class ApplicationDefinitionArgAttribute : Attribute
    {
        public string Prefix { get; set; }
        public string Name { get; set; }

        public string FullName { get { return Prefix + Name; } }

        public ApplicationDefinitionArgAttribute(string prefix, string name)
        {
            prefix = prefix.Trim();

            if (prefix.Length > 0 && !prefix.EndsWith("."))
                prefix = prefix + ".";

            Prefix = prefix;
            Name = name;
        }
    }
}