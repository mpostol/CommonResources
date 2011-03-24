//<summary>
//  Title   : License Tester - this file is compiled in runtime
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C)2011, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CAS.Lib.CodeProtect;
using CAS.Lib.CodeProtect.LicenseDsc;

namespace LicenseContainer.UT
{
  //%%GuidAttribute
  [LicenseProvider( typeof( CodeProtectLP ) )]
  sealed internal class LicenseTester: IsLicensed<LicenseTester>
  {
    /// <summary>
    /// It is called by the constructor at the end of the license validation process if a proper license file can be opened.
    /// If implemented by the derived class the current license can be used to get more information from it, e.g. <typeparamref name="Warning"/>.
    /// </summary>
    /// <param name="license">The current license for temporal use. The current license is disposed just after returning from this method</param>
    /// <remarks>
    /// Because the current license is disposed in the base class just after returning from this method, it must not be
    /// assigned to locally to keep it for future use. To keep it for future use the derived class must make a local copy.
    /// </remarks>
    protected override void TraceCurrentLicence( LicenseFile license )
    {
      base.TraceCurrentLicence( license );
      string msg = string.Format
        ( "Obtained valid license for {0}, runtime={1}, volume={2}", typeof( LicenseTester ), license.RunTimeConstrain, license.VolumeConstrain );
      Debug.WriteLine( msg );
    }
    protected override void TraceFailureReason( string reason )
    {
      base.TraceFailureReason( reason );
      Debug.WriteLine( reason );
    }
    protected override void TraceNoLicenseFile( string reason )
    {
      base.TraceNoLicenseFile( reason );
      Debug.WriteLine( reason );
    }
    public readonly string MyGuid = "//%%GuidInfo";
    public readonly string MyFunction = "//%%MyFunction";
    public override string ToString()
    {
      return string.Format( "{0} {1} {2}", MyFunction, MyGuid, base.ToString() );
    }
    public LicenseTester()
      : base( int.MaxValue, TimeSpan.MaxValue ) { }
  }
}
