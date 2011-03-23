using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
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
      Console.WriteLine( msg);
    }
    protected override void TraceFailureReason( string reason )
    {
      base.TraceFailureReason( reason );
      //throw new LicenseException( this.GetType(), this, reason );
    }
    protected override void TraceNoLicenseFile( string reason )
    {
      base.TraceNoLicenseFile( reason );
      //throw new LicenseException( this.GetType(), this, "No license file found, because: " + reason );
    }
    public LicenseTester()
      : base( int.MaxValue, TimeSpan.MaxValue ) { }
  }
}
