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
using System.ComponentModel;
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
    /// <summary>
    /// Extension method that sets a new value in a variable and then executes the event handler if the new value
    /// differs from the old one.  Used to easily implement INotifyPropeprtyChanged.
    /// </summary>
    /// <typeparam name="T">The type of values being handled (usually the type of the property).</typeparam>
    /// <param name="handler">The event handler to execute in the event of actual value change.</param>
    /// <param name="newValue">The new value to set.</param>
    /// <param name="oldValue">The old value to replace (and the value holder).</param>
    /// <param name="propertyName">The property's name as required by <typeparamref name="System.ComponentModel.PropertyChangedEventArgs"/>.</param>
    /// <param name="sender">The object to be appointed as the executioner of the handler.</param>
    /// <returns>A boolean value that indicates if the new value was truly different from the old value according to <code>object.Equals()</code>.</returns>
    public static bool RaiseHandler<T>(this PropertyChangedEventHandler handler, T newValue, ref T oldValue, string propertyName, object sender)
    {
      bool changed = !Object.Equals(oldValue, newValue);
      if (changed)
      {
        //Save the new value. 
        oldValue = newValue;
        //Raise the event 
        if (handler != null)
        {
          handler(sender, new PropertyChangedEventArgs(propertyName));
        }
      }
      //Signal what happened. 
      return changed;
    }
  }
}
