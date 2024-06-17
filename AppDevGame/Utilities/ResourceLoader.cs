using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

using AppDevGame;

namespace AppDevGame
{
    public abstract class ResourceLoader
    {
        protected string _subfolder;
        protected List<string> _allowedTypes;
    
        public ResourceLoader(string subfolder, List<string> allowedFileTypes)
        {
            _subfolder = subfolder;
            _allowedTypes = allowedFileTypes;
        }

        public abstract void LoadContent();
    }
}