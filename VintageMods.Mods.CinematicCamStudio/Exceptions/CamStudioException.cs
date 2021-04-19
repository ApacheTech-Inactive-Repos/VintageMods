using System;
using System.Runtime.Serialization;

namespace VintageMods.Mods.CinematicCamStudio.Exceptions
{
    [Serializable]
    public class CamStudioException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public CamStudioException()
        {
        }

        public CamStudioException(string message) : base(message)
        {
        }

        public CamStudioException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CamStudioException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}