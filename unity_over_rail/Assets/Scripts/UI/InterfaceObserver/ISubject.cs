using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The subject maintains a list of observers (subscribers or listeners). 
/// It Provides methods to register and unregister observers dynamically 
/// and defines a method to notify observers of changes in its state.
/// </summary>

public interface ISubject
{
    /// <summary>
    /// Attach an observer to the subject.
    /// </summary>
    /// <param name="observer"></param>
    void Attach(IObserver observer);

    /// <summary>
    /// Detach an observer from the subject.
    /// </summary>
    /// <param name="observer"></param>
    void Detach(IObserver observer);

    /// <summary>
    /// Notify all observers about an event.
    /// </summary>
    void Notify();
}
