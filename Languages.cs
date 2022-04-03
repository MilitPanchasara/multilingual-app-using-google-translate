using System;
using System.Collections.Generic;

namespace Multilingual
{
    public class LanguageData
    {
        public object Data { get; set; }
    }


    public class LanguageTranslatedData
    {
        public LanguageTranslatedDataInner Data { get; set; }
    }

    public class LanguageTranslatedDataInner
    {
        public LanguageTranslatedtext[] Translations { get; set; }
    }

    public class LanguageTranslatedtext {
        public string translatedText { get; set; }
    }

}
