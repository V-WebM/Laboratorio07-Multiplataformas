using System;
using System.Collections.Generic;
using System.Text;

namespace DependecyServices
{
    internal interface ITextToSpeech
    {
        void Speak(string text);
    }
}
