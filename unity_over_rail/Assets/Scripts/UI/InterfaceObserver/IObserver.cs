using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface which implement an update method that concrete observers must implement 
/// and ensures a common or consistent way for concrete observers to receive updates 
/// from the subject. Concrete observers implement this interface, allowing them to 
/// react to changes in the subject’s state.
/// </summary>

public interface IObserver
{
    /// <summary>
    /// Receive update from subject
    /// </summary>
    /// <param name="subject"></param>
    void Update(ISubject subject);
}
