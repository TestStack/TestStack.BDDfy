// Copyright (C) 2011, Mehdi Khalili
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the <organization> nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System.IO;

namespace Bddify.Reporters.HtmlReporter
{
    public static class LazyFileLoader
    {
        static string _cssFile;
        static string _jqueryFile;
        static string _myJsFile;

        public static string BddifyCssFile
        {
            get
            {
                if (_cssFile == null)
                {
                    _cssFile = GetEmbeddedFileResource("Bddify.Reporters.HtmlReporter.bddify.css");
                }

                return _cssFile;
            }
        }

        public static string JQueryFile
        {
            get
            {
                if (_jqueryFile == null)
                {
                    _jqueryFile = GetEmbeddedFileResource("Bddify.Reporters.HtmlReporter.jquery-1.7.1.min.js");
                }
                return _jqueryFile;
            }
        }

        public static string BddifyJsFile
        {
            get
            {
                if (_myJsFile == null)
                {
                    _myJsFile = GetEmbeddedFileResource("Bddify.Reporters.HtmlReporter.bddify.js");
                }

                return _myJsFile;
            }
        }

        static string GetEmbeddedFileResource(string fileResourceName)
        {
            string fileContent;
            var templateResourceStream = typeof(LazyFileLoader).Assembly.GetManifestResourceStream(fileResourceName);
            using (var sr = new StreamReader(templateResourceStream))
            {
                fileContent = sr.ReadToEnd();
            }

            return fileContent;
        }
    }
}
