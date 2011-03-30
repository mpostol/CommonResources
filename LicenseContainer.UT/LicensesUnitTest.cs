//<summary>
//  Title   : Test of licenses in the container
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using CAS.Lib.CodeProtect;
using CAS.Lib.CodeProtect.EnvironmentAccess;
using CAS.Lib.CodeProtect.LicenseDsc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LicenseContainer.UT
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class LicensesUnitTest
  {
    readonly string csv_file_templateformat = "LicenseContainer.UT.{0}.csv";
    public LicensesUnitTest()
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
    public void Test_All_CommServer_Licenses()
    {
      PerformTestOnProduct( "CommServer" );
    }

    [TestMethod]
    public void Test_All_DataPorter_Licenses()
    {
      PerformTestOnProduct( "DataPorter" );
    }


    [TestMethod]
    public void Test_All_CommServerUA_Licenses()
    {
      PerformTestOnProduct( "CommServerUA" );
    }

    [TestMethod]
    public void Test_All_UAModelDesigner_Licenses()
    {
      PerformTestOnProduct( "UA.ModelDesigner" );
    }

    private void PerformTestOnProduct( string productName )
    {
      using ( StreamReader sr2 = new StreamReader( Assembly.GetExecutingAssembly().GetManifestResourceStream( string.Format( csv_file_templateformat, productName ) ) ) )
      {
        string line;
        int linenumber = 0;
        string[] header0 = null;
        string[] header1 = null;
        string[] elements;
        List<GuidInstanceAndTestDefinition> tests = new List<GuidInstanceAndTestDefinition>();
        do
        {
          line = sr2.ReadLine();
          if ( line == null )
            break; //end of strem
          elements = line.Split( ';' );
          if ( linenumber == 0 )
            header0 = elements;
          else if ( linenumber == 1 )
          {
            header1 = elements;
            PrepareTestDefinitions( header0, header1, tests );
          }
          if ( linenumber > 1 )
          {
            PerformAllTestsOnOneLicense( productName, header0, elements, tests );
          }
          linenumber++;
        } while ( true );
      }
    }

    private static void PerformAllTestsOnOneLicense( string productName, string[] header0, string[] elements, List<GuidInstanceAndTestDefinition> tests )
    {
      try
      {
        try
        {
          LicenseFile.Uninstal();
          FileNames.DeleteKeys();
          ManifestManagement.DeleteDeployManifest();
        }
        catch { }
        try
        {
          CAS.Lib.CodeProtect.LibInstaller.InstalLicense( "TestUser", "CAS", "techsupp@cas.eu", true, productName, elements[ 1 ] );
        }
        catch ( Exception ex )
        {
          Assert.Fail( string.Format( "Cannot install license {0} {1} (reason: {2})", elements[ 0 ], elements[ 1 ], ex.Message ) );
        }
        foreach ( GuidInstanceAndTestDefinition giatd in tests )
        {
          #region foreach test
          string productAndFunctionInfo = string.Format( "Product: {0}, Function: {1}", elements[ 0 ], header0[ giatd.Index ] );
          bool? expected = null;
          try
          {
            expected = int.Parse( elements[ giatd.Index ] ) > 0;
          }
          catch ( Exception ex )
          {
            Assert.Fail( String.Format( "Cannot parse value='{0}' (from CSV) to valid integer (reason: {1})(product: {2})",
              elements[ giatd.Index ], ex.Message, productAndFunctionInfo ) );
          }
          bool succeded_actual = true;
          IIsLicensed o = null;
          try
          {
            o = (IIsLicensed)giatd.CompiledAssembly.CreateInstance( "LicenseContainer.UT.LicenseTester" );
          }
          catch
          {
            succeded_actual = false;
          }
          if ( succeded_actual && o != null )
            succeded_actual = o.Licensed;
          Assert.AreEqual( expected, succeded_actual, string.Format( "license test has failed! {0}", productAndFunctionInfo ) );
          foreach ( KeyValuePair<int, string> kvp in giatd.TestDictionary )
          {
            int? expectedValue = null;
            try
            {
              expectedValue = int.Parse( elements[ kvp.Key ] );
            }
            catch ( Exception ex )
            {
              Assert.Fail( String.Format( "Cannot parse value='{0}' (from CSV) to valid integer (reason: {1})", elements[ kvp.Key ], ex.Message ) );
            }
            int? actualValue = 0;
            switch ( kvp.Value )
            {
              case "Volume":
                if ( o.Licensed )
                  actualValue = o.Volumen;
                break;
              case "Runtime":
                if ( o.RunTime.Value.Equals( TimeSpan.MaxValue ) )
                  actualValue = -1;
                else
                  actualValue = o.RunTime.Value.Hours;
                break;
              default:
                break;
            }
            Assert.AreEqual( expectedValue, actualValue, string.Format( "license test has failed! {0}, Property: {1}", productAndFunctionInfo, kvp.Value ) );
          }


          Debug.WriteLine( string.Format( "Passed: {0}", productAndFunctionInfo ) );
          #endregion foreach test
        }
      }
      finally
      {
        LicenseFile.Uninstal();
        FileNames.DeleteKeys();
        ManifestManagement.DeleteDeployManifest();
      }
    }

    private static void PrepareTestDefinitions( string[] header0, string[] header1, List<GuidInstanceAndTestDefinition> tests )
    {
      GuidInstanceAndTestDefinition singleTestDefinition = null;
      for ( int i = 2; i < header1.Length; i++ )
      {
        Guid guid = Guid.Empty;
        try { guid = new Guid( header1[ i ] ); }
        catch { }
        if ( !guid.Equals( Guid.Empty ) )
        {
          singleTestDefinition = new GuidInstanceAndTestDefinition( guid, header0[ i ], i );
          tests.Add( singleTestDefinition );
        }
        else
          if ( singleTestDefinition != null )
          {
            singleTestDefinition.TestDictionary.Add( i, header1[ i ] );
          }
      }
    }
  }
}
