using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using CAS.Lib.RTLib.Utils;

namespace LicenseContainer.UT
{
  class GuidInstanceAndTestDefinition
  {
    static string template_class;
    static GuidInstanceAndTestDefinition()
    {
      using ( StreamReader sr1 = new StreamReader( Assembly.GetExecutingAssembly().GetManifestResourceStream( "LicenseContainer.UT.LicenseTester.cs" ) ) )
      {
        template_class = sr1.ReadToEnd();
      }
    }
    internal Guid guid { get; private set; }
    internal string FunctionalityName { get; private set; }
    internal Assembly CompiledAssembly { get; private set; }
    internal int Index { get; private set; }
    internal Dictionary<int,string> TestDictionary { get; private set; }

    internal GuidInstanceAndTestDefinition( Guid g, string functionality_name, int index )
    {
      guid = g;
      FunctionalityName = functionality_name;
      Index = index;
      string my_class = template_class.Replace( "//%%GuidAttribute", string.Format( "[GuidAttribute( \"{0}\" )]", guid.ToString() ) );
      CSharpStreamCompiller cssc = new CSharpStreamCompiller( my_class, new string[] { "CAS.CodeProtect.dll" } );
      CompiledAssembly = cssc.CompiledAssembly;
      TestDictionary = new Dictionary<int, string>();
    }
  }
}
