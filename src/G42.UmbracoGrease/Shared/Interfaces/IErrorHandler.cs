using System;

namespace G42.UmbracoGrease.Shared.Interfaces
{
    public interface IG42ErrorHandler
    {
        void Execute(object sender, EventArgs e);
    }
}
