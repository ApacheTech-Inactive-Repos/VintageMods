﻿using System;
using System.Runtime.Serialization;

namespace VintageMods.CinematicCamStudio.Exceptions
{
    [Serializable]
    public class CamMotionException : CamStudioException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public CamMotionException()
        {
        }

        public CamMotionException(string message) : base(message)
        {
        }

        public CamMotionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CamMotionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}