//<summary>
//  Title   : Name of Application
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CASSnippets
{
  public class TestClass : System.ComponentModel.INotifyPropertyChanged
  {

    private string b_MyProperty;

    public string MyProperty
    {
      get
      {
        return b_MyProperty;
      }
      set
      {
        PropertyChanged.SetAndRaise(value, ref b_MyProperty, "MyProperty", this);
      }
    }


    #region INotifyPropertyChanged Members
    public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    #endregion

  }
}
