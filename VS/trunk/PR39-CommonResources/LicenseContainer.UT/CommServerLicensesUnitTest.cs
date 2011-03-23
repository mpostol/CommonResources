using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using CAS.Lib.RTLib.Utils;

namespace LicenseContainer.UT
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class CommServerLicensesUnitTest
  {
    public CommServerLicensesUnitTest()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestMethod]
    public void Install_All_CommServer_Licenses()
    {
      //
      // TODO: Add test logic	here
      //
      string template_class;
      using ( StreamReader sr1 = new StreamReader( Assembly.GetExecutingAssembly().GetManifestResourceStream( "LicenseContainer.UT.LicenseTester.cs" ) ) )
      {
        template_class = sr1.ReadToEnd();
      }

      using ( StreamReader sr2 = new StreamReader( Assembly.GetExecutingAssembly().GetManifestResourceStream( "LicenseContainer.UT.CommServer.csv" ) ) )
      {
        string line;
        int linenumber = 0;
        string[] header0 = null;
        string[] header1 = null;
        string[] elements;
        do
        {
          line = sr2.ReadLine();
          if ( line == null )
            break; //end of strem
          elements = line.Split( ';' );
          if ( linenumber == 0 )
            header0 = elements;
          else if ( linenumber == 1 )
            header1 = elements;
          if ( linenumber > 1 )
          {
            try
            {
              CAS.Lib.CodeProtect.LibInstaller.InstalLicense( "TestUser", "CAS", "techsupp@cas.eu", true, "CommServer", elements[ 1 ] );
            }
            catch ( Exception ex )
            {
              Assert.Fail( string.Format( "Cannot install license {0} {1} (reason: {2})", elements[ 0 ], elements[ 1 ], ex.Message ) );
            }
            for ( int i = 2; i < header1.Length; i++ )
            {
              Guid guid = Guid.Empty;
              try { guid = new Guid( header1[ i ] ); }
              catch { }
              if ( !guid.Equals( Guid.Empty ) )
              {
                int expected = int.Parse( elements[ i ] );
                string my_class = template_class.Replace( "//%%GuidAttribute", string.Format( "[GuidAttribute( \"{0}\" )]", header1[ i ] ) );
                CSharpStreamCompiller cssc = new CSharpStreamCompiller( my_class, new string[] { "CAS.CodeProtect.dll" } );
                bool succeded_actual = true;
                try
                {
                  object o = cssc.CompiledAssembly.CreateInstance( "LicenseContainer.UT.LicenseTester" );
                }
                catch
                {
                  succeded_actual = false;
                }
                Assert.AreEqual( expected > 0, succeded_actual, string.Format( "license test has failed!, Product: {0}, Function: {1}", elements[ 0 ], header0[ i ] ) );
              }
            }
          }
          linenumber++;
        } while ( true );
      }
      CAS.Lib.CodeProtect.LibInstaller.InstalLicense( "TestUser", "CAS", "techsupp@cas.eu", true, "CommServer", "DefaultDemo" );
    }
  }
}
