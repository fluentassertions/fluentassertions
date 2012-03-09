﻿namespace FluentAssertions.Frameworks
 {
     internal class MbUnitTestFramework : LateBoundTestFramework
     {
         protected override string AssemblyName
         {
             get { return "MbUnit.Framework"; }
         }

         protected override string ExceptionFullName
         {
             get { return "MbUnit.Core.Exceptions.AssertionException"; }
         }
     }
 }