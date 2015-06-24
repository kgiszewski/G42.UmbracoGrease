using System;

namespace G42.UmbracoGrease.Interfaces
{
    public interface IG42ErrorHandler
    {
        void Execute(object sender, EventArgs e);
    }
}
