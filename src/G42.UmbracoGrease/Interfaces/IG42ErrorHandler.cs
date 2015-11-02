using System;

namespace G42.UmbracoGrease.Interfaces
{
    /// <summary>
    /// Interfact that is used to define methods that are required for implementing a custom Grease error handler.
    /// </summary>
    public interface IG42ErrorHandler
    {
        void Execute(object sender, EventArgs e, Exception ex);
    }
}
