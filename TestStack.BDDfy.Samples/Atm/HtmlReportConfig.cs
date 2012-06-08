﻿// Copyright (C) 2011, Mehdi Khalili
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

using TestStack.BDDfy.Processors.HtmlReporter;

namespace TestStack.BDDfy.Samples.Atm
{
    /// <summary>
    /// This overrides the default html report setting
    /// </summary>
    public class HtmlReportConfig : DefaultHtmlReportConfiguration
    {
        public override bool RunsOn(Core.Story story)
        {
            return story.MetaData.Type.Namespace != null && story.MetaData.Type.Namespace.EndsWith("Atm");
        }

        /// <summary>
        /// Change the output file name
        /// </summary>
        public override string OutputFileName
        {
            get
            {
                return "ATM.html";
            }
        }

        /// <summary>
        /// Change the report header to your project
        /// </summary>
        public override string ReportHeader
        {
            get
            {
                return "ATM Solutions";
            }
        }

        /// <summary>
        /// Change the report description
        /// </summary>
        public override string ReportDescription
        {
            get
            {
                return "A reliable solution for your offline banking needs";
            }
        }
    }
}