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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LicenseContainer.UT
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class CommServerLicensesUnitTest
  {
    readonly string csv_file_templateformat = "LicenseContainer.UT.{0}.csv";
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
    public void Test_All_CommServer_Licenses()
    {
      //
      // TODO: Add test logic	here
      //
      PerformTestOnProduct( "CommServer" );
    }
    [TestMethod]
    public void Test_All_DataPorter_Licenses()
    {
      //
      // TODO: Add test logic	here
      //
      PerformTestOnProduct( "DataPorter" );
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
          if ( linenumber > 1 )
          {
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
              int expected = int.Parse( elements[ giatd.Index ] );
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
              string productAndFunctionInfo = string.Format( "Product: {0}, Function: {1}", elements[ 0 ], header0[ giatd.Index ] );
              Assert.AreEqual( expected > 0, succeded_actual, string.Format( "license test has failed! {0}", productAndFunctionInfo ) );
              Debug.WriteLine( string.Format( "Passed: {0}", productAndFunctionInfo ) );
            }
          }
          linenumber++;
        } while ( true );
      }
    }
  }
}
